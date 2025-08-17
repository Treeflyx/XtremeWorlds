using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Core;
using static Core.Globals.Type;
using static Core.Globals.Command;
using static Core.Net.Packets;
using System.Reflection.Metadata.Ecma335;
using Core.Globals;
using Core.Net;
using Server.Game;
using Server.Net;
using EventCommand = Core.Globals.EventCommand;

namespace Server
{
    public class EventLogic
    {
        // ******** Enhancements and Explanations ********

        // 1. Asynchronous Operations:
        //    - Changed SpawnGlobalEvents and SpawnAllMapGlobalEvents to be async.  This allows these potentially
        //      long-running operations (especially with large maps and many events) to run without blocking the main thread.
        //    - Used Task.Run where appropriate to offload work to a background thread.
        //    - Added ConfigureAwait(false) to async calls to avoid deadlocks in some contexts.

        // 2. LINQ for Concise Queries:
        //    - Used LINQ (Language Integrated Query) in several places to replace loops with more readable and
        //      often more efficient queries.  This makes the code easier to understand and maintain.

        // 3. Improved Data ures and Error Handling:
        //    - Added null checks and boundary checks to prevent potential `IndexOutOfRangeException` errors.
        //    - Used `?.` (null-conditional operator) and `??` (null-coalescing operator) for safer and more concise null handling.
        //    - Replaced some manual array resizing with `List<T>` and then converted back to arrays when needed. Lists are generally
        //      easier to work with for dynamic resizing.
        //    - Simplified logic in several places by combining conditions and using more direct comparisons.
        //    - Replaced some magic numbers with named constants or enums if they weren't already defined.

        // 4. Code Clarity and Readability:
        //    - Improved code formatting for better readability (consistent indentation, spacing).
        //    - Added comments to explain complex logic sections.
        //    - Replaced some verbose `Conversions.ToBoolean(0)` and `true` with `false` and `true` respectively.
        //    - Replaced some older VB-style string functions (like `InStr`, `Mid`, `Len`, `Val`) with their C# equivalents (`Contains`,
        //      `Substring`, `Length`, `int.Parse` or `double.Parse`).

        // 5. Optimization:
        //    - Cached frequently accessed properties and array lengths to avoid repeated calculations.
        //    - Reduced redundant code by extracting common logic into helper methods.
        //    - Optimized event processing loop by avoiding unnecessary checks and iterations.
        //    - Used StringBuilder for efficient string concatenation in ParseEventText.

        // 6. Event Processing System Enhancements (Conceptual, not fully implemented)
        //    - Added a comment suggesting a possible priority system for events. This wasn't fully implemented,
        //    but it's a good example of a feature enhancement.  A real priority system would probably involve a more sophisticated data structure.

        // 7. Modern C# Features
        //   - Used 'ref' local variables, more directly showing the intent to modify the original structure.
        //   - Used 'var' to declare variables with implicit typing.

        // Constants for enhanced code clarity and maintainability:
        private const int DefaultMovementSpeed = 1; // Example default speed

        // Helper methods for better readability:
        private static bool IsEventVisible(ref MapEvent eventPage) => eventPage.Visible;
        private static int GetEventId(ref MapEvent eventPage) => eventPage.EventId;
        private static EventPage GetEventPage(int mapNum, int eventId, int pageId) => Data.Map[mapNum].Event[eventId].Pages[pageId];

        public static void RemoveDeadEvents()
        {
            // Use LINQ to iterate through connected players  
            Parallel.ForEach(Enumerable.Range(0, Data.TempPlayer.Length), i =>
            {
                if (Data.TempPlayer[i].EventMap.CurrentEvents > 0 && !Data.TempPlayer[i].GettingMap)
                {
                    int mapNum = GetPlayerMap(i);

                    // Use LINQ to filter and process relevant event pages  
                    var relevantPages = Data.TempPlayer[i].EventMap.EventPages
                        .Where((page, x) => x < Data.TempPlayer[i].EventMap.EventPages.Length)
                        .Where(page => page.EventId < Data.TempPlayer[i].EventMap.CurrentEvents) //Boundary check  
                        .Where(page => mapNum >= 0 && mapNum < Data.Map.Length && page.EventId < Data.Map[mapNum].Event.Length) // Boundary check.  
                        .ToList(); // Materialize the query to avoid issues with modifying the collection.  

                    foreach (var eventPage in relevantPages)
                    {
                        int id = eventPage.EventId;
                        int page = eventPage.PageId;

                        // Check if the event and page still exist  
                        if (id >= 0 && mapNum >= 0 && mapNum < Data.Map.Length &&
                            id < Data.Map[mapNum].Event.Length &&
                            Data.Map[mapNum].Event[id].Pages != null &&
                            page >= 0 && page < Data.Map[mapNum].Event[id].Pages.Length)
                        {
                            ref var playerEventPage = ref Data.TempPlayer[i].EventMap.EventPages[Array.IndexOf(Data.TempPlayer[i].EventMap.EventPages, eventPage)]; //find actual index of eventpage  

                            if (IsEventVisible(ref playerEventPage))
                            {
                                // Check conditions to see if the event should be hidden  
                                EventPage mapEventPage = GetEventPage(mapNum, id, page);

                                if (mapEventPage.ChkHasItem == 1 && Player.HasItem(i, mapEventPage.HasItemIndex) == 0)
                                {
                                    playerEventPage.Visible = false;
                                }

                                if (mapEventPage.ChkSelfSwitch == 1)
                                {
                                    int compare = mapEventPage.SelfSwitchCompare == 0 ? 0 : 1;
                                    bool selfSwitchConditionMet;

                                    if (Data.Map[mapNum].Event[id].Globals == 1)
                                    {
                                        selfSwitchConditionMet = Data.Map[mapNum].Event[id].SelfSwitches[mapEventPage.SelfSwitchIndex] == compare;
                                    }
                                    else
                                    {
                                        selfSwitchConditionMet = Data.TempPlayer[i].EventMap.EventPages[id].SelfSwitches[mapEventPage.SelfSwitchIndex] == compare;
                                    }

                                    if (!selfSwitchConditionMet)
                                    {
                                        playerEventPage.Visible = false;
                                    }
                                }

                                if (mapEventPage.ChkVariable == 1)
                                {
                                    int playerVar = Data.Player[i].Variables[mapEventPage.VariableIndex];
                                    int condition = mapEventPage.VariableCondition;
                                    bool variableConditionMet = false;

                                    switch (mapEventPage.VariableCompare)
                                    {
                                        case 0: variableConditionMet = playerVar == mapEventPage.VariableCondition; break;
                                        case 1: variableConditionMet = playerVar >= mapEventPage.VariableCondition; break;
                                        case 2: variableConditionMet = playerVar <= mapEventPage.VariableCondition; break;
                                        case 3: variableConditionMet = playerVar > mapEventPage.VariableCondition; break;
                                        case 4: variableConditionMet = playerVar < mapEventPage.VariableCondition; break;
                                        case 5: variableConditionMet = playerVar != mapEventPage.VariableCondition; break;
                                    }

                                    if (!variableConditionMet)
                                    {
                                        playerEventPage.Visible = false;
                                    }
                                }

                                if (mapEventPage.ChkSwitch == 1)
                                {
                                    //Simplified with XOR  
                                    if ((mapEventPage.SwitchCompare == 1) ^ (Data.Player[i].Switches[mapEventPage.SwitchIndex] == 1)) //we are expecting true  
                                    {
                                        playerEventPage.Visible = false;
                                    }
                                }

                                if (Data.Map[mapNum].Event[id].Globals == 1 && !IsEventVisible(ref playerEventPage))
                                {
                                    Event.TempEventMap[mapNum].Event[id].Active = 0;
                                }

                                if (!IsEventVisible(ref playerEventPage) && id >= 0)
                                {
                                    int pageNum = Array.IndexOf(Data.Map[mapNum].Event[id].Pages, mapEventPage);
                                    if (pageNum < 0 || pageNum >= Data.TempPlayer[i].EventMap.EventPages.Length)
                                        return;

                                    // Send packet to hide the event  
                                    var packetWriter = new PacketWriter();
                                    packetWriter.WriteEnum(ServerPackets.SSpawnEvent);
                                    packetWriter.WriteInt32(Data.TempPlayer[i].EventMap.CurrentEvents);
                                    packetWriter.WriteInt32(id);
                                    ref var withBlock = ref Data.TempPlayer[i].EventMap.EventPages[pageNum]; //find actual index of eventpage  
                                    packetWriter.WriteString(Data.Map[GetPlayerMap(i)].Event[withBlock.EventId].Name);
                                    packetWriter.WriteInt32(withBlock.Dir);
                                    packetWriter.WriteByte(withBlock.GraphicType);
                                    packetWriter.WriteInt32(withBlock.Graphic);
                                    packetWriter.WriteInt32(withBlock.GraphicX);
                                    packetWriter.WriteInt32(withBlock.GraphicX2);
                                    packetWriter.WriteInt32(withBlock.GraphicY);
                                    packetWriter.WriteInt32(withBlock.GraphicY2);
                                    packetWriter.WriteInt32(withBlock.MovementSpeed);
                                    packetWriter.WriteInt32(withBlock.X);
                                    packetWriter.WriteInt32(withBlock.Y);
                                    packetWriter.WriteByte(withBlock.Position);
                                    packetWriter.WriteBoolean(withBlock.Visible);
                                    packetWriter.WriteInt32(Data.Map[mapNum].Event[id].Pages[page].WalkAnim);
                                    packetWriter.WriteInt32(Data.Map[mapNum].Event[id].Pages[page].DirFix);
                                    packetWriter.WriteInt32(Data.Map[mapNum].Event[id].Pages[page].WalkThrough);
                                    packetWriter.WriteInt32(Data.Map[mapNum].Event[id].Pages[page].ShowName);

                                    PlayerService.Instance.SendDataTo(i, packetWriter.GetBytes());
                                }
                            }
                        }
                    }
                }
            });
        }

        public static void SpawnNewEvents()
        {
            // Use Parallel.For for potential performance gains on multi-core systems.
            Parallel.ForEach(PlayerService.Instance.PlayerIds, i =>
            {
                int mapNum = GetPlayerMap(i);

                if (Data.TempPlayer[i].EventMap.EventPages != null)
                {
                    // Iterate through the player's current events.  Use a List for easier manipulation.
                    var eventPagesList = Data.TempPlayer[i].EventMap.EventPages.ToList();

                    for (int x = 0; x < eventPagesList.Count; x++)
                    {
                        int p = -1;
                        int id = eventPagesList[x].EventId;

                        // Basic bounds check.
                        if (id < 0 || id >= eventPagesList.Count) continue;

                        int pageId = eventPagesList[x].PageId;

                        if (!eventPagesList[x].Visible)
                            pageId = 0;

                        // Another bounds check.
                        if (Data.Map[mapNum].Event == null)
                        {
                            break;
                        }

                        if (id >= Data.Map[mapNum].Event.Length) continue;

                        // Iterate through event pages to find the highest-priority page that meets conditions
                        for (int z = 0; z < Data.Map[mapNum].Event[id].PageCount; z++)
                        {
                            bool spawnEvent = true;
                            if (Data.Map[mapNum].Event[id].Pages == null)
                                break;
                            EventPage page = Data.Map[mapNum].Event[id].Pages[z];

                            // Check conditions (Item, Self Switch, Variable, Switch).
                            if (page.ChkHasItem == 1 && Player.HasItem(i, page.HasItemIndex) == 0)
                            {
                                spawnEvent = false;
                            }

                            if (page.ChkSelfSwitch == 1)
                            {
                                int compare = page.SelfSwitchCompare; // 0 or 1
                                bool selfSwitchStatus;

                                if (Data.Map[mapNum].Event[id].Globals == 1)
                                    selfSwitchStatus = Data.Map[mapNum].Event[id].SelfSwitches[page.SelfSwitchIndex] == compare;
                                else
                                    selfSwitchStatus = Data.TempPlayer[i].EventMap.EventPages[id].SelfSwitches[page.SelfSwitchIndex] == compare;

                                if (!selfSwitchStatus)
                                    spawnEvent = false;
                            }


                            if (page.ChkVariable == 1)
                            {
                                int playerVar = Data.Player[i].Variables[page.VariableIndex];
                                bool conditionMet = false;
                                switch (page.VariableCompare)
                                {
                                    case 0: conditionMet = playerVar == page.VariableCondition; break;
                                    case 1: conditionMet = playerVar >= page.VariableCondition; break;
                                    case 2: conditionMet = playerVar <= page.VariableCondition; break;
                                    case 3: conditionMet = playerVar > page.VariableCondition; break;
                                    case 4: conditionMet = playerVar < page.VariableCondition; break;
                                    case 5: conditionMet = playerVar != page.VariableCondition; break;
                                }

                                if (!conditionMet)
                                    spawnEvent = false;
                            }


                            if (page.ChkSwitch == 1)
                            {
                                // Using XOR for concise switch check.
                                if ((page.SwitchCompare == 0) ^ (Data.Player[i].Switches[page.SwitchIndex] == 0)) //we want false
                                {
                                    spawnEvent = false; //and switch is true, don't spawn.
                                }
                            }


                            if (spawnEvent)
                            {
                                p = z; // Store the highest-priority valid page index
                            }
                        }

                        // Determine if we should spawn a *new* event (p >= 0 and it wasn't already visible)
                        if (p >= 0 && !eventPagesList[x].Visible)
                        {
                            int z = p;

                            // Reset any active event processing for this event ID.
                            for (int n = 0; n < Data.TempPlayer[i].EventProcessing.Length; n++)
                            {
                                if (Data.TempPlayer[i].EventProcessing[n].EventId == id)
                                {
                                    Data.TempPlayer[i].EventProcessing[n].EventId = -1;
                                    Data.TempPlayer[i].EventProcessing[n].Active = 0;
                                }
                            }


                            // Set up the event page data.
                            ref var withBlock = ref Data.TempPlayer[i].EventMap.EventPages[x]; // Use x, as this is the correct index into *this player's* event list
                            EventPage newPage = Data.Map[mapNum].Event[id].Pages[z];

                            withBlock.Dir = newPage.GraphicType == 1
                                ? (newPage.GraphicY % 4) switch
                                {
                                    0 => (int) Direction.Down,
                                    1 => (int) Direction.Left,
                                    2 => (int) Direction.Right,
                                    _ => (int) Direction.Up // 3
                                }
                                : 0;

                            withBlock.Graphic = newPage.Graphic;
                            withBlock.GraphicType = newPage.GraphicType;
                            withBlock.GraphicX = newPage.GraphicX;
                            withBlock.GraphicY = newPage.GraphicY;
                            withBlock.GraphicX2 = newPage.GraphicX2;
                            withBlock.GraphicY2 = newPage.GraphicY2;

                            withBlock.MovementSpeed = newPage.MoveSpeed switch
                            {
                                0 => 2,
                                1 => 3,
                                2 => 4,
                                3 => 6,
                                4 => 12,
                                5 => 24,
                                _ => DefaultMovementSpeed // Handle unexpected values
                            };


                            withBlock.Position = newPage.Position;
                            withBlock.EventId = id; // This should be the event ID, not the index in the player's event list.
                            withBlock.PageId = z;
                            withBlock.Visible = true;
                            withBlock.MoveType = newPage.MoveType;

                            if (withBlock.MoveType == 2) // Custom Move Route
                            {
                                withBlock.MoveRouteCount = newPage.MoveRouteCount;
                                if (newPage.MoveRouteCount > 0)
                                {
                                    // Copy the move route.
                                    withBlock.MoveRoute = new MoveRoute[newPage.MoveRouteCount];
                                    Array.Copy(newPage.MoveRoute, withBlock.MoveRoute, newPage.MoveRouteCount);
                                    withBlock.MoveRouteComplete = 0; // Ensure it's reset.
                                }
                                else
                                {
                                    withBlock.MoveRouteComplete = 1; // No route = complete.
                                }
                            }
                            else
                            {
                                withBlock.MoveRouteComplete = 1;
                            }

                            withBlock.RepeatMoveRoute = newPage.RepeatMoveRoute;
                            withBlock.IgnoreIfCannotMove = newPage.IgnoreMoveRoute;
                            withBlock.MoveFreq = newPage.MoveFreq;
                            withBlock.MoveSpeed = newPage.MoveSpeed;
                            withBlock.WalkThrough = newPage.WalkThrough;
                            withBlock.ShowName = newPage.ShowName;
                            withBlock.WalkingAnim = newPage.WalkAnim;
                            withBlock.FixedDir = newPage.DirFix;

                            if (Data.Map[mapNum].Event[id].Globals == 1)
                            {
                                Event.TempEventMap[mapNum].Event[id].Active = z;
                                Event.TempEventMap[mapNum].Event[id].Position = newPage.Position;
                            }

                            // Send the spawn event packet.
                            var buffer = new PacketWriter();
                            if (id <= 0)
                                continue;

                            buffer.WriteEnum(ServerPackets.SSpawnEvent);
                            buffer.WriteInt32(Data.TempPlayer[i].EventMap.CurrentEvents);
                            buffer.WriteInt32(id); // Event ID

                            ref var withBlock1 = ref Data.TempPlayer[i].EventMap.EventPages[x];
                            buffer.WriteString(Data.Map[mapNum].Event[withBlock1.EventId].Name);
                            buffer.WriteInt32(withBlock1.Dir);
                            buffer.WriteByte(withBlock1.GraphicType);
                            buffer.WriteInt32(withBlock1.Graphic);
                            buffer.WriteInt32(withBlock1.GraphicX);
                            buffer.WriteInt32(withBlock1.GraphicX2);
                            buffer.WriteInt32(withBlock1.GraphicY);
                            buffer.WriteInt32(withBlock1.GraphicY2);
                            buffer.WriteInt32(withBlock1.MovementSpeed);
                            buffer.WriteInt32(withBlock1.X);
                            buffer.WriteInt32(withBlock1.Y);
                            buffer.WriteByte(withBlock1.Position);
                            buffer.WriteBoolean(withBlock1.Visible);
                            buffer.WriteInt32(Data.Map[mapNum].Event[id].Pages[z].WalkAnim);
                            buffer.WriteInt32(Data.Map[mapNum].Event[id].Pages[z].DirFix);
                            buffer.WriteInt32(Data.Map[mapNum].Event[id].Pages[z].WalkThrough);
                            buffer.WriteInt32(Data.Map[mapNum].Event[id].Pages[z].ShowName);

                            PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                        }
                    }
                }
            });
        }

        public static void ProcessEventMovement()
        {
            // Iterate through all maps.
            for (int i = 0; i < Core.Globals.Constant.MaxMaps; i++)
            {
                // Process global events on this map.
                for (int x = 0; x < Event.TempEventMap[i].EventCount; x++)
                {
                    if (Event.TempEventMap[i].Event[x].Active <= 0) continue;

                    // Check if it's time to process movement.
                    if (Event.TempEventMap[i].Event[x].MoveTimer > General.GetTimeMs()) continue;

                    ref var globalEvent = ref Event.TempEventMap[i].Event[x];

                    // Process movement based on MoveType.
                    switch (globalEvent.MoveType)
                    {
                        case 0: // Fixed, do nothing.
                            break;

                        case 1: // Random Movement
                        {
                            int rand = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4)); // 0-3 for direction.
                            if (Event.CanEventMove(0, i, globalEvent.X, globalEvent.Y, x, globalEvent.WalkThrough, (byte) rand, true))
                            {
                                int actualMoveSpeed = globalEvent.MoveSpeed switch
                                {
                                    0 => 2,
                                    1 => 3,
                                    2 => 4,
                                    3 => 6,
                                    4 => 12,
                                    5 => 24,
                                    _ => DefaultMovementSpeed
                                };
                                Event.EventMove(0, i, x, rand, actualMoveSpeed, true);
                            }
                            else
                            {
                                Event.EventDir(0, i, x, rand, true); // Just change direction.
                            }

                            break;
                        }
                        case 2: // Custom Move Route
                        {
                            ref var withBlock = ref Event.TempEventMap[i].Event[x];
                            bool isGlobal = true;
                            int mapNum = i;
                            int playerId = 0;
                            int eventId = x;
                            int walkThrough = withBlock.WalkThrough;
                            bool doNotProcessMoveRoute = false;

                            if (withBlock.MoveRouteCount > 0)
                            {
                                if (withBlock.MoveRouteStep >= withBlock.MoveRouteCount)
                                {
                                    if (withBlock.RepeatMoveRoute == 1)
                                    {
                                        withBlock.MoveRouteStep = 0;
                                        withBlock.MoveRouteComplete = 1; // Reset for repeating routes.
                                    }
                                    else
                                    {
                                        doNotProcessMoveRoute = true;
                                        withBlock.MoveRouteComplete = 1; // Mark as complete if not repeating.
                                    }
                                }
                                else //still moving
                                    withBlock.MoveRouteComplete = 0;


                                if (!doNotProcessMoveRoute)
                                {
                                    withBlock.MoveRouteStep++;

                                    int actualmovespeed = withBlock.MoveSpeed switch
                                    {
                                        0 => 2,
                                        1 => 3,
                                        2 => 4,
                                        3 => 6,
                                        4 => 12,
                                        5 => 24,
                                        _ => DefaultMovementSpeed
                                    };


                                    // Get next move route step, handling potential out-of-bounds access.
                                    if (withBlock.MoveRouteStep < 0 || withBlock.MoveRouteStep >= withBlock.MoveRoute.Length)
                                    {
                                        //Error, route step out of bounds
                                        break;
                                    }

                                    var nextMove = withBlock.MoveRoute[withBlock.MoveRouteStep];


                                    bool sendUpdate = false;
                                    switch (nextMove.Index)
                                    {
                                        case 1: // Move Up
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Up, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, (int) Direction.Up, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 2: // Move Down
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Down, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, (int) Direction.Down, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 3: // Move Left
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Left, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, (int) Direction.Left, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 4: // Move Right
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Right, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, (int) Direction.Right, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 5: // Move Random
                                        {
                                            int z = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4)); // 0-3
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, z, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        }

                                        case 6: // Move Toward Player
                                        {
                                            if (!isGlobal) //should never be global.
                                            {
                                                // Determine if the event is one block away from the player.
                                                if (Event.IsOneBlockAway(withBlock.X, withBlock.Y, GetPlayerX(playerId), GetPlayerY(playerId)))
                                                {
                                                    // Face the player.
                                                    Event.EventDir(playerId, GetPlayerMap(playerId), eventId, Event.GetDirToPlayer(playerId, GetPlayerMap(playerId), eventId), false);
                                                    if (withBlock.IgnoreIfCannotMove == 0)
                                                    {
                                                        withBlock.MoveRouteStep--;
                                                    }
                                                }
                                                else
                                                {
                                                    // Try to move towards the player.
                                                    int z = Event.CanEventMoveTowardsPlayer(playerId, mapNum, eventId);
                                                    if (z < 4) // Valid direction (0-3).
                                                    {
                                                        if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                                        {
                                                            Event.EventMove(playerId, mapNum, eventId, z, actualmovespeed, isGlobal);
                                                        }
                                                        else if (withBlock.IgnoreIfCannotMove == 0)
                                                        {
                                                            withBlock.MoveRouteStep--;
                                                        }
                                                    }
                                                    else if (withBlock.IgnoreIfCannotMove == 0) // Cannot move towards player and we don't ignore.
                                                    {
                                                        withBlock.MoveRouteStep--;
                                                    }
                                                }
                                            }

                                            break;
                                        }

                                        case 7: // Move Away from Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.CanEventMoveAwayFromPlayer(playerId, mapNum, eventId);
                                                if (z < 5)
                                                {
                                                    if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                                    {
                                                        Event.EventMove(playerId, mapNum, eventId, z, actualmovespeed, isGlobal);
                                                    }
                                                    else if (withBlock.IgnoreIfCannotMove == 0)
                                                    {
                                                        withBlock.MoveRouteStep--;
                                                    }
                                                }
                                            }

                                            break;
                                        }

                                        case 8: // Move Forward
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) withBlock.Dir, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, withBlock.Dir, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 9: // Move Backward
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Down,
                                                (byte) Direction.Down => (byte) Direction.Up,
                                                (byte) Direction.Left => (byte) Direction.Right,
                                                (byte) Direction.Right => (byte) Direction.Left,
                                                _ => withBlock.Dir // Invalid direction, keep current.
                                            };
                                            if (Event.CanEventMove(playerId, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                            {
                                                Event.EventMove(playerId, mapNum, eventId, z, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        }

                                        case 10: withBlock.MoveTimer = General.GetTimeMs() + 100; break;
                                        case 11: withBlock.MoveTimer = General.GetTimeMs() + 500; break;
                                        case 12: withBlock.MoveTimer = General.GetTimeMs() + 1000; break;

                                        case 13: Event.EventDir(playerId, mapNum, eventId, (byte) Direction.Up, isGlobal); break;
                                        case 14: Event.EventDir(playerId, mapNum, eventId, (byte) Direction.Down, isGlobal); break;
                                        case 15: Event.EventDir(playerId, mapNum, eventId, (byte) Direction.Left, isGlobal); break;
                                        case 16: Event.EventDir(playerId, mapNum, eventId, (byte) Direction.Right, isGlobal); break;

                                        // Turn 90 degrees clockwise, counter-clockwise, 180 degrees, or at random
                                        case 17: // Turn Right 90 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Right,
                                                (byte) Direction.Right => (byte) Direction.Down,
                                                (byte) Direction.Left => (byte) Direction.Up,
                                                (byte) Direction.Down => (byte) Direction.Left,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 18: // Turn Left 90 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Left,
                                                (byte) Direction.Right => (byte) Direction.Up,
                                                (byte) Direction.Left => (byte) Direction.Down,
                                                (byte) Direction.Down => (byte) Direction.Right,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 19: // Turn 180 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Down,
                                                (byte) Direction.Right => (byte) Direction.Left,
                                                (byte) Direction.Left => (byte) Direction.Right,
                                                (byte) Direction.Down => (byte) Direction.Up,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 20: // Turn Random
                                        {
                                            int z = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4));
                                            Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 21: // Turn Toward Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.GetDirToPlayer(playerId, mapNum, eventId);
                                                Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            }

                                            break;
                                        }

                                        case 22: // Turn Away from Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.GetDirAwayFromPlayer(playerId, mapNum, eventId);
                                                Event.EventDir(playerId, mapNum, eventId, z, isGlobal);
                                            }

                                            break;
                                        }

                                        // Change Speed, Frequency, Graphic
                                        case 23: withBlock.MoveSpeed = 0; break;
                                        case 24: withBlock.MoveSpeed = 1; break;
                                        case 25: withBlock.MoveSpeed = 2; break;
                                        case 26: withBlock.MoveSpeed = 3; break;
                                        case 27: withBlock.MoveSpeed = 4; break;
                                        case 28: withBlock.MoveSpeed = 5; break;

                                        case 29: withBlock.MoveFreq = 0; break;
                                        case 30: withBlock.MoveFreq = 1; break;
                                        case 31: withBlock.MoveFreq = 2; break;
                                        case 32: withBlock.MoveFreq = 3; break;
                                        case 33: withBlock.MoveFreq = 4; break;

                                        case 34: // Turn On Walking Animation
                                            withBlock.WalkingAnim = 1;
                                            sendUpdate = true;
                                            break;
                                        case 35: // Turn Off Walking Animation
                                            withBlock.WalkingAnim = 0;
                                            sendUpdate = true;
                                            break;

                                        case 36: // Turn On Direction Fix
                                            withBlock.FixedDir = 1;
                                            sendUpdate = true;
                                            break;
                                        case 37: // Turn Off Direction Fix
                                            withBlock.FixedDir = 0;
                                            sendUpdate = true;
                                            break;

                                        case 38: // Turn On Through
                                            withBlock.WalkThrough = 1;
                                            break;
                                        case 39: // Turn Off Through
                                            withBlock.WalkThrough = 0;
                                            break;
                                        case 40: //Turn on Fix Position
                                            withBlock.Position = 1;
                                            sendUpdate = true;
                                            break;
                                        case 41: //Turn off Fix Position
                                            withBlock.Position = 0;
                                            sendUpdate = true;
                                            break;
                                        case 42: //Turn on Below Player
                                            withBlock.Position = 2;
                                            sendUpdate = true;
                                            break;

                                        case 43: // Change Graphic
                                        {
                                            withBlock.GraphicType = (byte) nextMove.Data1;
                                            withBlock.Graphic = nextMove.Data2;
                                            withBlock.GraphicX = nextMove.Data3;
                                            withBlock.GraphicX2 = nextMove.Data4;
                                            withBlock.GraphicY = nextMove.Data5;
                                            withBlock.GraphicY2 = nextMove.Data6;

                                            // Adjust direction if it's a character graphic.
                                            if (withBlock.GraphicType == 1)
                                            {
                                                withBlock.Dir = withBlock.GraphicY switch
                                                {
                                                    0 => (int) Direction.Down,
                                                    1 => (int) Direction.Left,
                                                    2 => (int) Direction.Right,
                                                    3 => (int) Direction.Up,
                                                    _ => withBlock.Dir
                                                };
                                            }

                                            sendUpdate = true;
                                            break;
                                        }
                                    }


                                    if (sendUpdate)
                                    {
                                        var buffer = new PacketWriter();
                                        {
                                            buffer.WriteEnum(ServerPackets.SSpawnEvent);
                                            buffer.WriteInt32(Data.TempPlayer[i].EventMap.CurrentEvents);
                                            buffer.WriteInt32(eventId); // Event ID.

                                            ref var withBlock1 = ref Event.TempEventMap[i].Event[x];
                                            buffer.WriteString(Data.Map[i].Event[x].Name); // Global event, use map index
                                            buffer.WriteInt32(withBlock1.Dir);
                                            buffer.WriteByte(withBlock1.GraphicType);
                                            buffer.WriteInt32(withBlock1.Graphic);
                                            buffer.WriteInt32(withBlock1.GraphicX);
                                            buffer.WriteInt32(withBlock1.GraphicX2);
                                            buffer.WriteInt32(withBlock1.GraphicY);
                                            buffer.WriteInt32(withBlock1.GraphicY2);
                                            buffer.WriteInt32(withBlock1.MoveSpeed);
                                            buffer.WriteInt32(withBlock1.X);
                                            buffer.WriteInt32(withBlock1.Y);
                                            buffer.WriteByte(withBlock1.Position);
                                            buffer.WriteInt32(withBlock1.Active);
                                            buffer.WriteInt32(withBlock1.WalkingAnim); // Corrected property names
                                            buffer.WriteInt32(withBlock1.FixedDir);
                                            buffer.WriteInt32(withBlock1.WalkThrough);
                                            buffer.WriteInt32(withBlock1.ShowName);
                                            NetworkConfig.SendDataToMap(i, buffer.GetBytes());
                                        }
                                    }
                                }

                                doNotProcessMoveRoute = false; // Reset for next iteration.
                            }

                            break;
                        }
                    }

                    // Set the next move timer based on MoveFreq.
                    globalEvent.MoveTimer = General.GetTimeMs() + globalEvent.MoveFreq switch
                    {
                        0 => 4000,
                        1 => 2000,
                        2 => 1000,
                        3 => 500,
                        4 => 250,
                        _ => 1000 // Default if invalid.
                    };
                }
            }
        }


        public static void ProcessLocalEventMovement()
        {
            // Parallel processing for each player.
            Parallel.ForEach(PlayerService.Instance.PlayerIds, i =>
            {
                if (Data.TempPlayer[i].EventMap.CurrentEvents <= 0) return;

                int mapNum = GetPlayerMap(i);

                // Iterate through local events for the player.
                for (int x = 0; x < Data.TempPlayer[i].EventMap.CurrentEvents; x++)
                {
                    if (x >= Data.TempPlayer[i].EventMap.EventPages.Length)
                        break;

                    // Bounds check.
                    if (Data.TempPlayer[i].EventMap.EventPages[x].EventId >= Data.Map[mapNum].Event.Length) continue;


                    ref var localEvent = ref Data.TempPlayer[i].EventMap.EventPages[x];


                    // Only process visible, non-global events.
                    if (Data.Map[mapNum].Event[localEvent.EventId].Globals != 0 || !localEvent.Visible) continue;


                    // Check move timer.
                    if (localEvent.MoveTimer > General.GetTimeMs()) continue;

                    // Process movement based on MoveType.
                    switch (localEvent.MoveType)
                    {
                        case 0: // Fixed
                            break;

                        case 1: // Random
                        {
                            int rand = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4));
                            if (Event.CanEventMove(i, mapNum, localEvent.X, localEvent.Y, x, localEvent.WalkThrough, (byte) rand, false))
                            {
                                int actualMoveSpeed = localEvent.MoveSpeed switch
                                {
                                    0 => 2,
                                    1 => 3,
                                    2 => 4,
                                    3 => 6,
                                    4 => 12,
                                    5 => 24,
                                    _ => DefaultMovementSpeed
                                };
                                Event.EventMove(i, mapNum, x, rand, actualMoveSpeed, false);
                            }
                            else
                            {
                                Event.EventDir(i, mapNum, x, rand, false);
                            }

                            break;
                        }
                        case 2: // Custom Move Route
                        {
                            ref var withBlock = ref Data.TempPlayer[i].EventMap.EventPages[x];
                            bool isGlobal = false;
                            bool sendUpdate = false;
                            int eventId = x;
                            int walkThrough = withBlock.WalkThrough;
                            bool doNotProcessMoveRoute = false;

                            if (withBlock.MoveRouteCount > 0)
                            {
                                if (withBlock.MoveRouteStep >= withBlock.MoveRouteCount)
                                {
                                    if (withBlock.RepeatMoveRoute == 1)
                                    {
                                        withBlock.MoveRouteStep = 0;
                                        withBlock.MoveRouteComplete = 1; // Reset for repeating.
                                    }
                                    else
                                    {
                                        doNotProcessMoveRoute = true;
                                        withBlock.MoveRouteComplete = 1; // Mark as complete.
                                    }
                                }
                                else //still moving
                                    withBlock.MoveRouteComplete = 0;


                                if (!doNotProcessMoveRoute)
                                {
                                    withBlock.MoveRouteStep++;

                                    int actualmovespeed = withBlock.MoveSpeed switch
                                    {
                                        0 => 2,
                                        1 => 3,
                                        2 => 4,
                                        3 => 6,
                                        4 => 12,
                                        5 => 24,
                                        _ => DefaultMovementSpeed
                                    };


                                    // Get next move route step, handling potential out-of-bounds access.
                                    if (withBlock.MoveRouteStep < 0 || withBlock.MoveRouteStep >= withBlock.MoveRoute.Length)
                                    {
                                        //error, route step out of range
                                        break; // Exit the switch statement.
                                    }

                                    var nextMove = withBlock.MoveRoute[withBlock.MoveRouteStep];

                                    switch (nextMove.Index)
                                    {
                                        case 1: // Move Up
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Up, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, (int) Direction.Up, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 2: // Move Down
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Down, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, (int) Direction.Down, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 3: // Move Left
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Left, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, (int) Direction.Left, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 4: // Move Right
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) Direction.Right, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, (int) Direction.Right, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        case 5: // Move Random
                                        {
                                            int z = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4));
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, z, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        }

                                        case 6: // Move Toward Player
                                        {
                                            if (!isGlobal)
                                            {
                                                if (Event.IsOneBlockAway(withBlock.X, withBlock.Y, GetPlayerX(i), GetPlayerY(i)))
                                                {
                                                    Event.EventDir(i, mapNum, eventId, Event.GetDirToPlayer(i, mapNum, eventId), false);

                                                    // Activate event if triggered by player action.
                                                    if (Data.Map[mapNum].Event[eventId].Pages[Data.TempPlayer[i].EventMap.EventPages[eventId].PageId].Trigger == 1)
                                                    {
                                                        if (Data.Map[mapNum].Event[eventId].Pages[Data.TempPlayer[i].EventMap.EventPages[eventId].PageId].CommandListCount > 0)
                                                        {
                                                            // Start event processing.
                                                            ref var eventProcessing = ref Data.TempPlayer[i].EventProcessing[eventId]; // Use EventId (local index)
                                                            eventProcessing.Active = 1;
                                                            eventProcessing.ActionTimer = General.GetTimeMs();
                                                            eventProcessing.CurList = 0;
                                                            eventProcessing.CurSlot = 0;
                                                            eventProcessing.EventId = eventId; // This should be the *map* event ID
                                                            eventProcessing.PageId = Data.TempPlayer[i].EventMap.EventPages[eventId].PageId; // Local page ID.
                                                            eventProcessing.WaitingForResponse = 0;
                                                        }
                                                    }

                                                    if (withBlock.IgnoreIfCannotMove == 0)
                                                    {
                                                        withBlock.MoveRouteStep--;
                                                    }
                                                }
                                                else
                                                {
                                                    int z = Event.CanEventMoveTowardsPlayer(i, mapNum, eventId);
                                                    if (z < 4)
                                                    {
                                                        if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                                        {
                                                            Event.EventMove(i, mapNum, eventId, z, actualmovespeed, isGlobal);
                                                        }
                                                        else if (withBlock.IgnoreIfCannotMove == 0)
                                                        {
                                                            withBlock.MoveRouteStep--;
                                                        }
                                                    }
                                                    else if (withBlock.IgnoreIfCannotMove == 0)
                                                    {
                                                        withBlock.MoveRouteStep = withBlock.MoveRouteStep - 1;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                        case 7: // Move Away From Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.CanEventMoveAwayFromPlayer(i, mapNum, eventId);
                                                if (z < 5) // Valid direction.
                                                {
                                                    if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                                    {
                                                        Event.EventMove(i, mapNum, eventId, z, actualmovespeed, isGlobal);
                                                    }
                                                    else if (withBlock.IgnoreIfCannotMove == 0)
                                                    {
                                                        withBlock.MoveRouteStep--;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                        case 8: // Move Forward
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) withBlock.Dir, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, withBlock.Dir, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;

                                        case 9: // Move Backward
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Down,
                                                (byte) Direction.Down => (byte) Direction.Up,
                                                (byte) Direction.Left => (byte) Direction.Right,
                                                (byte) Direction.Right => (byte) Direction.Left,
                                                _ => withBlock.Dir
                                            };
                                            if (Event.CanEventMove(i, mapNum, withBlock.X, withBlock.Y, eventId, walkThrough, (byte) z, isGlobal))
                                            {
                                                Event.EventMove(i, mapNum, eventId, z, actualmovespeed, isGlobal);
                                            }
                                            else if (withBlock.IgnoreIfCannotMove == 0)
                                            {
                                                withBlock.MoveRouteStep--;
                                            }

                                            break;
                                        }
                                        case 10: withBlock.MoveTimer = General.GetTimeMs() + 100; break;
                                        case 11: withBlock.MoveTimer = General.GetTimeMs() + 500; break;
                                        case 12: withBlock.MoveTimer = General.GetTimeMs() + 1000; break;

                                        case 13: Event.EventDir(i, mapNum, eventId, (byte) Direction.Up, isGlobal); break;
                                        case 14: Event.EventDir(i, mapNum, eventId, (byte) Direction.Down, isGlobal); break;
                                        case 15: Event.EventDir(i, mapNum, eventId, (byte) Direction.Left, isGlobal); break;
                                        case 16: Event.EventDir(i, mapNum, eventId, (byte) Direction.Right, isGlobal); break;

                                        // Turn 90 degrees clockwise, counter-clockwise, 180 degrees
                                        case 17: // Turn Right 90 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Right,
                                                (byte) Direction.Right => (byte) Direction.Down,
                                                (byte) Direction.Left => (byte) Direction.Up,
                                                (byte) Direction.Down => (byte) Direction.Left,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 18: // Turn Left 90 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Left,
                                                (byte) Direction.Right => (byte) Direction.Up,
                                                (byte) Direction.Left => (byte) Direction.Down,
                                                (byte) Direction.Down => (byte) Direction.Right,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 19: // Turn 180 Degrees
                                        {
                                            int z = withBlock.Dir switch
                                            {
                                                (byte) Direction.Up => (byte) Direction.Down,
                                                (byte) Direction.Right => (byte) Direction.Left,
                                                (byte) Direction.Left => (byte) Direction.Right,
                                                (byte) Direction.Down => (byte) Direction.Up,
                                                _ => withBlock.Dir
                                            };
                                            Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 20: // Turn Random
                                        {
                                            int z = (int) Math.Floor((double) General.GetRandom.NextInt(0, 4));
                                            Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            break;
                                        }
                                        case 21: // Turn Toward Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.GetDirToPlayer(i, mapNum, eventId);
                                                Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            }

                                            break;
                                        }
                                        case 22: // Turn Away from Player
                                        {
                                            if (!isGlobal)
                                            {
                                                int z = Event.GetDirAwayFromPlayer(i, mapNum, eventId);
                                                Event.EventDir(i, mapNum, eventId, z, isGlobal);
                                            }

                                            break;
                                        }

                                        // Change Speed, Frequency, Graphic
                                        case 23: withBlock.MoveSpeed = 0; break;
                                        case 24: withBlock.MoveSpeed = 1; break;
                                        case 25: withBlock.MoveSpeed = 2; break;
                                        case 26: withBlock.MoveSpeed = 3; break;
                                        case 27: withBlock.MoveSpeed = 4; break;
                                        case 28: withBlock.MoveSpeed = 5; break;

                                        case 29: withBlock.MoveFreq = 0; break;
                                        case 30: withBlock.MoveFreq = 1; break;
                                        case 31: withBlock.MoveFreq = 2; break;
                                        case 32: withBlock.MoveFreq = 3; break;
                                        case 33: withBlock.MoveFreq = 4; break;

                                        case 34:
                                            withBlock.WalkingAnim = 1;
                                            sendUpdate = true;
                                            break; // Turn On Walking Animation
                                        case 35:
                                            withBlock.WalkingAnim = 0;
                                            sendUpdate = true;
                                            break; // Turn Off Walking Animation
                                        case 36:
                                            withBlock.FixedDir = 1;
                                            sendUpdate = true;
                                            break; // Turn On Direction Fix
                                        case 37:
                                            withBlock.FixedDir = 0;
                                            sendUpdate = true;
                                            break; // Turn Off Direction Fix
                                        case 38: withBlock.WalkThrough = 1; break; // Turn On Through
                                        case 39: withBlock.WalkThrough = 0; break; // Turn Off Through
                                        case 40:
                                            withBlock.Position = 1;
                                            sendUpdate = true;
                                            break; // Turn On Fixed
                                        case 41:
                                            withBlock.Position = 0;
                                            sendUpdate = true;
                                            break; // Turn Off Fixed
                                        case 42:
                                            withBlock.Position = 2;
                                            sendUpdate = true;
                                            break; //Turn on Below player

                                        case 43: // Change Graphic
                                        {
                                            withBlock.GraphicType = (byte) nextMove.Data1;
                                            withBlock.Graphic = nextMove.Data2;
                                            withBlock.GraphicX = nextMove.Data3;
                                            withBlock.GraphicX2 = nextMove.Data4;
                                            withBlock.GraphicY = nextMove.Data5;
                                            withBlock.GraphicY2 = nextMove.Data6;

                                            // Adjust direction if it's a character graphic.
                                            if (withBlock.GraphicType == 1)
                                            {
                                                withBlock.Dir = withBlock.GraphicY switch
                                                {
                                                    0 => (int) Direction.Down,
                                                    1 => (int) Direction.Left,
                                                    2 => (int) Direction.Right,
                                                    3 => (int) Direction.Up,
                                                    _ => withBlock.Dir
                                                };
                                            }

                                            sendUpdate = true;
                                            break;
                                        }
                                    }

                                    // Send update if necessary.
                                    if (sendUpdate && Data.TempPlayer[i].EventMap.EventPages[eventId].EventId >= 0)
                                    {
                                        var buffer = new PacketWriter();
                                        buffer.WriteEnum(ServerPackets.SSpawnEvent);
                                        buffer.WriteInt32(Data.TempPlayer[i].EventMap.CurrentEvents);
                                        buffer.WriteInt32(Data.TempPlayer[i].EventMap.EventPages[eventId].EventId); // Use map event ID

                                        ref var withBlock1 = ref Data.TempPlayer[i].EventMap.EventPages[eventId];
                                        buffer.WriteString(Data.Map[mapNum].Event[withBlock1.EventId].Name); //use map event Id
                                        buffer.WriteInt32(withBlock1.Dir);
                                        buffer.WriteByte(withBlock1.GraphicType);
                                        buffer.WriteInt32(withBlock1.Graphic);
                                        buffer.WriteInt32(withBlock1.GraphicX);
                                        buffer.WriteInt32(withBlock1.GraphicX2);
                                        buffer.WriteInt32(withBlock1.GraphicY);
                                        buffer.WriteInt32(withBlock1.GraphicY2);
                                        buffer.WriteInt32(withBlock1.MovementSpeed); // Use consistent naming
                                        buffer.WriteInt32(withBlock1.X);
                                        buffer.WriteInt32(withBlock1.Y);
                                        buffer.WriteByte(withBlock1.Position);
                                        buffer.WriteBoolean(withBlock1.Visible);
                                        buffer.WriteInt32(withBlock1.WalkingAnim);
                                        buffer.WriteInt32(withBlock1.FixedDir);
                                        buffer.WriteInt32(withBlock1.WalkThrough);
                                        buffer.WriteInt32(withBlock1.ShowName);
                                        PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                    }
                                }

                                doNotProcessMoveRoute = false; // Reset for the next loop iteration.
                            }

                            break;
                        }
                    }

                    // Set next move timer based on MoveFreq.
                    localEvent.MoveTimer = General.GetTimeMs() + localEvent.MoveFreq switch
                    {
                        0 => 4000,
                        1 => 2000,
                        2 => 1000,
                        3 => 500,
                        4 => 250,
                        _ => 1000
                    };
                }
            });
        }


        public static void ProcessEventCommands()
        {
            // Parallel processing for each player.
            Parallel.ForEach(PlayerService.Instance.PlayerIds, i =>
            {
                if (!NetworkConfig.IsPlaying(i) || Data.TempPlayer[i].GettingMap || Data.TempPlayer[i].EventMap.CurrentEvents <= 0) return;

                int mapNum = Data.Player[i].Map; // Cache map number.

                // Iterate through the player's events.
                for (int x = 0; x < Data.TempPlayer[i].EventMap.CurrentEvents; x++)
                {
                    if (x >= Data.TempPlayer[i].EventMap.EventPages.Length)
                        break;

                    if (Data.TempPlayer[i].EventProcessingCount <= 0) continue;

                    ref var eventPage = ref Data.TempPlayer[i].EventMap.EventPages[x];

                    if (!eventPage.Visible) continue;

                    // Check event and page validity.
                    if (eventPage.EventId >= Data.Map[mapNum].Event.Length || Data.Map[mapNum].Event == null || Data.Map[mapNum].Event[eventPage.EventId].Pages == null || eventPage.PageId >= Data.Map[mapNum].Event[eventPage.EventId].Pages.Length) continue;

                    // Handle parallel process events (Trigger == 2).
                    if (Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].Trigger == 2)
                    {
                        // If not already active, start the event processing.
                        if (Data.TempPlayer[i].EventProcessing[eventPage.EventId].Active == 0) // Use map event ID for indexing.
                        {
                            if (Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].CommandListCount > 0)
                            {
                                ref var eventProcessing = ref Data.TempPlayer[i].EventProcessing[eventPage.EventId]; // And here.
                                eventProcessing.Active = 1;
                                eventProcessing.ActionTimer = General.GetTimeMs();
                                eventProcessing.CurList = 0;
                                eventProcessing.CurSlot = 0;
                                eventProcessing.EventId = eventPage.EventId;
                                eventProcessing.PageId = eventPage.PageId;
                                eventProcessing.WaitingForResponse = 0;

                                // Allocate ListLeftOff array.
                                eventProcessing.ListLeftOff = new int[Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].CommandListCount];
                            }
                        }
                    }
                }
            });

            // Process active event commands for each player.
            Parallel.ForEach(PlayerService.Instance.PlayerIds, i =>
            {
                if (!NetworkConfig.IsPlaying(i) || Data.TempPlayer[i].EventProcessingCount <= 0 || Data.TempPlayer[i].GettingMap) return;

                int mapNum = GetPlayerMap(i); // Cache map number
                bool restartloop;
                do
                {
                    restartloop = false;
                    for (int x = 0; x <= Data.TempPlayer[i].EventProcessingCount; x++)
                    {
                        if (Data.TempPlayer[i].EventProcessing[x].Active != 1) continue;

                        ref var withBlock1 = ref Data.TempPlayer[i].EventProcessing[x];

                        // Basic validity checks
                        if (withBlock1.EventId < 0 || withBlock1.EventId >= Data.Map[mapNum].Event.Length) continue;

                        bool removeEventProcess = false;

                        // Handle waiting states (shop, bank, event movement).
                        switch (withBlock1.WaitingForResponse)
                        {
                            case 2: // Waiting for shop to close.
                                if (Data.TempPlayer[i].InShop == -1)
                                {
                                    withBlock1.WaitingForResponse = 0;
                                }

                                break;
                            case 3: // Waiting for bank to close.
                                if (!Data.TempPlayer[i].InBank)
                                {
                                    withBlock1.WaitingForResponse = 0;
                                }

                                break;
                            case 4: // Waiting for event movement to complete.
                            {
                                //check to make sure event still exists
                                if (withBlock1.EventMovingId < 0 || withBlock1.EventMovingId >= Data.TempPlayer[i].EventMap.EventPages.Length)
                                    break;

                                if (withBlock1.EventMovingType == 0) // Local event.
                                {
                                    if (Data.TempPlayer[i].EventMap.EventPages[withBlock1.EventMovingId].MoveRouteComplete == 1)
                                    {
                                        withBlock1.WaitingForResponse = 0;
                                    }
                                }
                                else // Global event.
                                {
                                    //check that map still exists
                                    if (GetPlayerMap(i) < 0 || GetPlayerMap(i) >= Event.TempEventMap.Length)
                                        break;

                                    //check that event still exists.
                                    if (withBlock1.EventMovingId < 0 || withBlock1.EventMovingId >= Event.TempEventMap[GetPlayerMap(i)].Event.Length)
                                        break;

                                    if (Event.TempEventMap[GetPlayerMap(i)].Event[withBlock1.EventMovingId].MoveRouteComplete == 1)
                                    {
                                        withBlock1.WaitingForResponse = 0;
                                    }
                                }

                                break;
                            }
                        }

                        if (withBlock1.WaitingForResponse == 0 && withBlock1.ActionTimer <= General.GetTimeMs())
                        {
                            // Process event commands until a wait, branch, or end condition is encountered.
                            bool restartlist = true;
                            bool endprocess = false;
                            while (restartlist && !endprocess && withBlock1.WaitingForResponse == 0)
                            {
                                restartlist = false;

                                // Check for null or out-of-bounds conditions.
                                if (withBlock1.ListLeftOff == null) continue; // Should not happen, but handle it.

                                var commandList = Data.Map[mapNum].Event[withBlock1.EventId].Pages[withBlock1.PageId].CommandList;

                                // More boundary checks
                                if (withBlock1.CurList >= commandList.Length)
                                {
                                    removeEventProcess = true;
                                    endprocess = true;
                                    continue; // Exit the inner loop.
                                }

                                if (withBlock1.CurSlot >= commandList[withBlock1.CurList].Commands.Length)
                                {
                                    if (withBlock1.CurList == commandList[withBlock1.CurList].ParentList)
                                    {
                                        removeEventProcess = true;
                                        endprocess = true;
                                    }
                                    else
                                    {
                                        withBlock1.CurList = commandList[withBlock1.CurList].ParentList;
                                        withBlock1.CurSlot = 0;
                                        restartlist = true;
                                    }

                                    continue;
                                }

                                // Restore saved position in the command list, if any.
                                if (withBlock1.ListLeftOff[withBlock1.CurList] > 0)
                                {
                                    withBlock1.CurSlot = withBlock1.ListLeftOff[withBlock1.CurList] + 1;
                                    withBlock1.ListLeftOff[withBlock1.CurList] = 0; // Clear the saved position.
                                }

                                // Check again, since curslot and curlist may have changed
                                if (withBlock1.CurList >= commandList.Length)
                                {
                                    removeEventProcess = true;
                                    endprocess = true;
                                    continue; // Exit inner loop.
                                }

                                if (withBlock1.CurSlot >= commandList[withBlock1.CurList].CommandCount)
                                {
                                    if (withBlock1.CurList == commandList[withBlock1.CurList].ParentList) //should be itself
                                    {
                                        removeEventProcess = true; // End of the main list.
                                        endprocess = true;
                                    }
                                    else
                                    {
                                        withBlock1.CurList = commandList[withBlock1.CurList].ParentList;
                                        withBlock1.CurSlot = 0;
                                        restartlist = true;
                                    }

                                    continue;
                                }


                                if (!restartlist && !endprocess)
                                {
                                    // Process the current event command.
                                    var command = commandList[withBlock1.CurList].Commands[withBlock1.CurSlot];

                                    switch (command.Index)
                                    {
                                        case (byte) EventCommand.AddText:
                                        {
                                            switch (command.Data2)
                                            {
                                                case 0: // Player
                                                    NetworkSend.PlayerMsg(i, command.Text1, command.Data1);
                                                    break;
                                                case 1: // Map
                                                    NetworkSend.MapMsg(mapNum, command.Text1);
                                                    break;
                                                case 2: // Global
                                                    NetworkSend.GlobalMsg(command.Text1);
                                                    break;
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.ShowText:
                                        {
                                            var buffer = new PacketWriter();
                                            {
                                                buffer.WriteEnum(ServerPackets.SEventChat);
                                                buffer.WriteInt32(withBlock1.EventId);
                                                buffer.WriteInt32(withBlock1.PageId);
                                                buffer.WriteInt32(command.Data1); // Face Icon
                                                buffer.WriteString(ParseEventText(i, command.Text1));

                                                // Determine if there's a next command to influence display behavior.
                                                int nextCommandType = 0; // 0: None, 1: ShowText/Choices, 2: Condition
                                                if (withBlock1.CurSlot + 1 < commandList[withBlock1.CurList].CommandCount)
                                                {
                                                    byte nextIndex = (byte) commandList[withBlock1.CurList].Commands[withBlock1.CurSlot + 1].Index;
                                                    if (nextIndex == (byte) EventCommand.ShowText || nextIndex == (byte) EventCommand.ShowChoices)
                                                    {
                                                        nextCommandType = 1;
                                                    }
                                                    else if (nextIndex == (byte) EventCommand.ConditionalBranch)
                                                    {
                                                        nextCommandType = 2;
                                                    }
                                                }
                                                else //end of list
                                                    nextCommandType = 2;

                                                buffer.WriteInt32(nextCommandType);
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            withBlock1.WaitingForResponse = 0; // No response needed.
                                            break;
                                        }
                                        case (byte) EventCommand.ShowChoices:
                                        {
                                            var buffer = new PacketWriter();
                                            {
                                                buffer.WriteEnum(ServerPackets.SEventChat);
                                                buffer.WriteInt32(withBlock1.EventId);
                                                buffer.WriteInt32(withBlock1.PageId);
                                                buffer.WriteInt32(command.Data5); // Face Icon
                                                buffer.WriteString(ParseEventText(i, command.Text1));

                                                // Determine the number of choices.
                                                int w = 0;
                                                if (!string.IsNullOrEmpty(command.Text2))
                                                {
                                                    w = 1;
                                                    if (!string.IsNullOrEmpty(command.Text3))
                                                    {
                                                        w = 2;
                                                        if (!string.IsNullOrEmpty(command.Text4))
                                                        {
                                                            w = 3;
                                                            if (!string.IsNullOrEmpty(command.Text5))
                                                            {
                                                                w = 4;
                                                            }
                                                        }
                                                    }
                                                }

                                                buffer.WriteInt32(w);

                                                // Write choice texts.
                                                for (int v = 1; v <= w; v++)
                                                {
                                                    switch (v)
                                                    {
                                                        case 1: buffer.WriteString(ParseEventText(i, command.Text2)); break;
                                                        case 2: buffer.WriteString(ParseEventText(i, command.Text3)); break;
                                                        case 3: buffer.WriteString(ParseEventText(i, command.Text4)); break;
                                                        case 4: buffer.WriteString(ParseEventText(i, command.Text5)); break;
                                                    }
                                                }

                                                // Next command logic (similar to ShowText).
                                                int nextCommandType = 0;
                                                if (withBlock1.CurSlot + 1 < commandList[withBlock1.CurList].CommandCount)
                                                {
                                                    byte nextIndex = (byte) commandList[withBlock1.CurList].Commands[withBlock1.CurSlot + 1].Index;
                                                    if (nextIndex == (byte) EventCommand.ShowText || nextIndex == (byte) EventCommand.ShowChoices)
                                                    {
                                                        nextCommandType = 1;
                                                    }
                                                    else if (nextIndex == (byte) EventCommand.ConditionalBranch)
                                                    {
                                                        nextCommandType = 2;
                                                    }
                                                }
                                                else
                                                    nextCommandType = 2;

                                                buffer.WriteInt32(nextCommandType);
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            withBlock1.WaitingForResponse = 0; // No response needed (choices handled separately).
                                            break;
                                        }
                                        case (byte) EventCommand.ModifyVariable:
                                        {
                                            switch (command.Data2)
                                            {
                                                case 0: // Set
                                                    Data.Player[i].Variables[command.Data1] = command.Data3;
                                                    break;
                                                case 1: // Add
                                                    Data.Player[i].Variables[command.Data1] += command.Data3;
                                                    break;
                                                case 2: // Subtract
                                                    Data.Player[i].Variables[command.Data1] -= command.Data3;
                                                    break;
                                                case 3: // Random
                                                    Data.Player[i].Variables[command.Data1] = (int) General.GetRandom.NextDouble(command.Data3, command.Data4);
                                                    break;
                                            }

                                            // Check for new event pages
                                            SpawnMapEventsFor(i, mapNum);
                                            break;
                                        }
                                        case (byte) EventCommand.ModifySwitch:
                                        {
                                            Data.Player[i].Switches[command.Data1] = (byte) (command.Data2 == 0 ? 0 : 1);

                                            // Check for new event pages
                                            SpawnMapEventsFor(i, mapNum);
                                            break;
                                        }

                                        case (byte) EventCommand.ModifySelfSwitch:
                                        {
                                            // Determine whether it's a global or local self switch.
                                            if (Data.Map[mapNum].Event[withBlock1.EventId].Globals == 1)
                                            {
                                                Data.Map[mapNum].Event[withBlock1.EventId].SelfSwitches[command.Data1 + 1] = (byte) (command.Data2 == 0 ? 0 : 1);
                                            }
                                            else
                                            {
                                                Data.TempPlayer[i].EventMap.EventPages[withBlock1.EventId].SelfSwitches[command.Data1 + 1] = (byte) (command.Data2 == 0 ? 0 : 1);
                                            }

                                            // Check for new event pages
                                            SpawnMapEventsFor(i, mapNum);
                                            break;
                                        }
                                        case (byte) EventCommand.ConditionalBranch:
                                        {
                                            bool conditionMet = false;
                                            var branch = command.ConditionalBranch;

                                            switch (branch.Condition)
                                            {
                                                case 0: // Variable
                                                {
                                                    int playerVar = Data.Player[i].Variables[branch.Data1];
                                                    switch (branch.Data2)
                                                    {
                                                        case 0: conditionMet = playerVar == branch.Data3; break;
                                                        case 1: conditionMet = playerVar >= branch.Data3; break;
                                                        case 2: conditionMet = playerVar <= branch.Data3; break;
                                                        case 3: conditionMet = playerVar > branch.Data3; break;
                                                        case 4: conditionMet = playerVar < branch.Data3; break;
                                                        case 5: conditionMet = playerVar != branch.Data3; break;
                                                    }

                                                    break;
                                                }
                                                case 1: // Switch
                                                {
                                                    bool switchState = Data.Player[i].Switches[branch.Data1] == 1;
                                                    conditionMet = (branch.Data2 == 0 && switchState) || (branch.Data2 == 1 && !switchState);
                                                    break;
                                                }
                                                case 2: // Item
                                                    conditionMet = Player.HasItem(i, branch.Data1) >= branch.Data2;
                                                    break;
                                                case 3: // Class
                                                    conditionMet = Data.Player[i].Job == branch.Data1;
                                                    break;
                                                case 4: // Skill
                                                    conditionMet = HasSkill(i, branch.Data1);
                                                    break;
                                                case 5: // Level
                                                {
                                                    int playerLevel = GetPlayerLevel(i);
                                                    switch (branch.Data2)
                                                    {
                                                        case 0: conditionMet = playerLevel == branch.Data1; break;
                                                        case 1: conditionMet = playerLevel >= branch.Data1; break;
                                                        case 2: conditionMet = playerLevel <= branch.Data1; break;
                                                        case 3: conditionMet = playerLevel > branch.Data1; break;
                                                        case 4: conditionMet = playerLevel < branch.Data1; break;
                                                        case 5: conditionMet = playerLevel != branch.Data1; break;
                                                    }

                                                    break;
                                                }
                                                case 6: // Self Switch
                                                {
                                                    bool selfSwitchState;
                                                    if (Data.Map[mapNum].Event[withBlock1.EventId].Globals == 1)
                                                        selfSwitchState = Data.Map[mapNum].Event[withBlock1.EventId].SelfSwitches[branch.Data1 + 1] == 1;
                                                    else
                                                        selfSwitchState = Data.TempPlayer[i].EventMap.EventPages[withBlock1.EventId].SelfSwitches[branch.Data1 + 1] == 1;

                                                    conditionMet = (branch.Data2 == 0 && selfSwitchState) || (branch.Data2 == 1 && !selfSwitchState);
                                                    break;
                                                }

                                                case 7: //Timer - Not currently implemented
                                                    break;
                                                case 8: // Gender
                                                    conditionMet = Data.Player[i].Sex == branch.Data1;
                                                    break;
                                                case 9: // Time of Day
                                                    conditionMet = Clock.Instance.TimeOfDay == (TimeOfDay) branch.Data1;
                                                    break;
                                            }

                                            // Set the next command list and slot based on the condition.
                                            withBlock1.ListLeftOff[withBlock1.CurList] = withBlock1.CurSlot;
                                            withBlock1.CurList = conditionMet ? branch.CommandList : branch.ElseCommandList;
                                            withBlock1.CurSlot = 0;
                                            endprocess = true; //end process so we dont increment curslot, but instead start at the top of the conditional list.

                                            break;
                                        }

                                        case (byte) EventCommand.ExitEventProcess:
                                            removeEventProcess = true;
                                            endprocess = true;
                                            break;

                                        case (byte) EventCommand.ChangeItems:
                                        {
                                            switch (command.Data2)
                                            {
                                                case 0: // Set
                                                    if (Player.HasItem(i, command.Data1) > 0)
                                                    {
                                                        SetPlayerInvValue(i, Player.FindItemSlot(i, command.Data1), command.Data3);
                                                    }

                                                    break;
                                                case 1: // Give
                                                    Player.GiveInv(i, command.Data1, command.Data3, true);
                                                    break;
                                                case 2: // Take
                                                {
                                                    int itemAmount = Player.HasItem(i, command.Data1);
                                                    if (itemAmount >= command.Data3)
                                                    {
                                                        Player.TakeInv(i, command.Data1, command.Data3);
                                                    }

                                                    break;
                                                }
                                            }

                                            NetworkSend.SendInventory(i);
                                            break;
                                        }

                                        case (byte) EventCommand.RestoreHealth:
                                            SetPlayerVital(i, Vital.Health, GetPlayerMaxVital(i, Vital.Health));
                                            NetworkSend.SendVital(i, Vital.Health);
                                            break;

                                        case (byte) EventCommand.RestoreMana:
                                            SetPlayerVital(i, Vital.Mana, GetPlayerMaxVital(i, Vital.Mana));
                                            NetworkSend.SendVital(i, Vital.Mana);
                                            break;

                                        case (byte) EventCommand.RestoreStamina:
                                            SetPlayerVital(i, Vital.Stamina, GetPlayerMaxVital(i, Vital.Stamina));
                                            NetworkSend.SendVital(i, Vital.Stamina);
                                            break;

                                        case (byte) EventCommand.GiveExperience:
                                            SetPlayerExp(i, GetPlayerNextLevel(i));
                                            Player.CheckPlayerLevelUp(i);
                                            NetworkSend.SendExp(i);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.ChangeLevel:
                                            SetPlayerLevel(i, command.Data1);
                                            SetPlayerExp(i, 0);
                                            NetworkSend.SendExp(i);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.ChangeSkills:
                                        {
                                            if (command.Data2 == 0) // Learn
                                            {
                                                if (FindOpenSkill(i) >= 0 && !HasSkill(i, command.Data1))
                                                {
                                                    SetPlayerSkill(i, FindOpenSkill(i), command.Data1);
                                                }
                                            }
                                            else if (command.Data2 == 1) // Forget
                                            {
                                                for (int p = 0; p < Core.Globals.Constant.MaxPlayerSkills; p++)
                                                {
                                                    if (Data.Player[i].Skill[p].Num == command.Data1)
                                                    {
                                                        SetPlayerSkill(i, p, 0);
                                                    }
                                                }
                                            }

                                            NetworkSend.SendPlayerSkills(i);
                                            break;
                                        }

                                        case (byte) EventCommand.ChangeJob:
                                            Data.Player[i].Job = (byte) command.Data1;
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.ChangeSprite:
                                            SetPlayerSprite(i, command.Data1);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.ChangeSex:
                                            Data.Player[i].Sex = (byte) (command.Data1 == 0 ? Sex.Male : Sex.Female);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.SetPlayerKillable:
                                            Data.Player[i].Pk = (command.Data1 == 0 ? false : true);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.WarpPlayer:
                                        {
                                            int dir = command.Data4 == 0 ? Data.Player[i].Dir : (byte) (command.Data4 - 1);
                                            Player.PlayerWarp(i, command.Data1, command.Data2, command.Data3, dir);
                                            break;
                                        }

                                        case (byte) EventCommand.SetMoveRoute:
                                        {
                                            // Check if the event exists.
                                            if (command.Data1 < Data.Map[mapNum].Event.Length)
                                            {
                                                if (Data.Map[mapNum].Event[command.Data1].Globals == 1) // Global event
                                                {
                                                    // Directly modify the global event.
                                                    ref var globalEvent = ref Event.TempEventMap[mapNum].Event[command.Data1];
                                                    globalEvent.MoveType = 2; // Custom route
                                                    globalEvent.IgnoreIfCannotMove = command.Data2;
                                                    globalEvent.RepeatMoveRoute = command.Data3;
                                                    globalEvent.MoveRouteCount = command.MoveRouteCount;
                                                    if (command.MoveRouteCount > 0)
                                                    {
                                                        globalEvent.MoveRoute = new MoveRoute[command.MoveRouteCount];
                                                        Array.Copy(command.MoveRoute, globalEvent.MoveRoute, command.MoveRouteCount);
                                                    }

                                                    globalEvent.MoveRouteStep = 0;
                                                    globalEvent.MoveRouteComplete = (command.MoveRouteCount == 0) ? 1 : 0; //if routecount is 0, complete = true
                                                }
                                                else // Local event
                                                {
                                                    // Modify the local event copy for this player.
                                                    ref var localEvent = ref Data.TempPlayer[i].EventMap.EventPages[command.Data1]; // Assuming Data1 is the event index
                                                    localEvent.MoveType = 2;
                                                    localEvent.IgnoreIfCannotMove = command.Data2;
                                                    localEvent.RepeatMoveRoute = command.Data3;
                                                    localEvent.MoveRouteCount = command.MoveRouteCount;
                                                    if (command.MoveRouteCount > 0)
                                                    {
                                                        localEvent.MoveRoute = new MoveRoute[command.MoveRouteCount];
                                                        Array.Copy(command.MoveRoute, localEvent.MoveRoute, command.MoveRouteCount);
                                                    }

                                                    localEvent.MoveRouteStep = 0;
                                                    localEvent.MoveRouteComplete = (command.MoveRouteCount == 0) ? 1 : 0; // If no route, it's complete.
                                                }
                                            }

                                            break;
                                        }

                                        case (byte) EventCommand.PlayAnimation:
                                        {
                                            switch (command.Data2)
                                            {
                                                case 0: // On Player
                                                    Animation.SendAnimation(mapNum, command.Data1, GetPlayerX(i), GetPlayerY(i), (byte) TargetType.Player, i);
                                                    break;
                                                case 1: // On Event
                                                {
                                                    //check for valid event
                                                    if (command.Data3 < 0 || command.Data3 >= Data.Map[mapNum].Event.Length)
                                                        break;

                                                    if (Data.Map[mapNum].Event[command.Data3].Globals == 1)
                                                    {
                                                        // Play on global event.
                                                        Animation.SendAnimation(mapNum, command.Data1,
                                                            Data.Map[mapNum].Event[command.Data3].X,
                                                            Data.Map[mapNum].Event[command.Data3].Y);
                                                    }
                                                    else
                                                    {
                                                        //check that local event exists for this player.
                                                        if (command.Data3 < 0 || command.Data3 >= Data.TempPlayer[i].EventMap.EventPages.Length)
                                                            break;

                                                        // Play on local event.
                                                        Animation.SendAnimation(mapNum, command.Data1,
                                                            Data.TempPlayer[i].EventMap.EventPages[command.Data3].X,
                                                            Data.TempPlayer[i].EventMap.EventPages[command.Data3].Y,
                                                            (byte) TargetType.Event, command.Data3);
                                                    }

                                                    break;
                                                }
                                                case 2: // On Coordinates
                                                    Animation.SendAnimation(mapNum, command.Data1, command.Data3, command.Data4, 0, 0);
                                                    break;
                                            }

                                            break;
                                        }

                                        case (byte) EventCommand.PlayBgm:
                                        {
                                            var buffer = new PacketWriter();
                                            {
                                                buffer.WriteEnum(ServerPackets.SPlayBgm);
                                                buffer.WriteString(command.Text1);
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.FadeOutBgm:
                                        {
                                            var buffer = new PacketWriter(4);
                                            buffer.WriteEnum(ServerPackets.SFadeoutBgm);
                                            PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            break;
                                        }

                                        case (byte) EventCommand.PlaySound:
                                        {
                                            var buffer = new PacketWriter();
                                            {
                                                buffer.WriteEnum(ServerPackets.SPlaySound);
                                                buffer.WriteString(command.Text1);
                                                buffer.WriteInt32(Data.Map[mapNum].Event[withBlock1.EventId].X);
                                                buffer.WriteInt32(Data.Map[mapNum].Event[withBlock1.EventId].Y);
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            break;
                                        }

                                        case (byte) EventCommand.StopSound:
                                        {
                                            var buffer = new PacketWriter(4);
                                            buffer.WriteEnum(ServerPackets.SStopSound);
                                            PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            break;
                                        }
                                        case (byte) EventCommand.SetAccessLevel:
                                            SetPlayerAccess(i, (byte) command.Data1);
                                            NetworkSend.SendPlayerData(i);
                                            break;

                                        case (byte) EventCommand.OpenShop:
                                        {
                                            // Check if the shop exists and has a valid name.
                                            if (command.Data1 > 0 && command.Data1 < Data.Shop.Length && !string.IsNullOrEmpty(Data.Shop[command.Data1].Name))
                                            {
                                                NetworkSend.SendOpenShop(i, command.Data1);
                                                Data.TempPlayer[i].InShop = command.Data1;
                                                withBlock1.WaitingForResponse = 2; // Wait for shop to close.
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.OpenBank:
                                            NetworkSend.SendBank(i);
                                            Data.TempPlayer[i].InBank = true;
                                            withBlock1.WaitingForResponse = 3; // Wait for bank to close.
                                            break;

                                        case (byte) EventCommand.ShowChatBubble:
                                        {
                                            ColorName color = ColorName.Blue; // Or any default color you prefer
                                            switch (command.Data1)
                                            {
                                                case (byte) TargetType.Player:
                                                    NetworkSend.SendChatBubble(mapNum, i, command.Data1, command.Text1, (int) color);
                                                    break;
                                                case (byte) TargetType.Npc:
                                                    NetworkSend.SendChatBubble(mapNum, command.Data2, command.Data1, command.Text1, (int) color);
                                                    break;
                                                case (byte) TargetType.Event:
                                                    NetworkSend.SendChatBubble(mapNum, command.Data2, command.Data1, command.Text1, (int) color);
                                                    break;
                                            }

                                            break;
                                        }

                                        case (byte) EventCommand.Label:
                                            // No action needed, just a label for GoToLabel.
                                            break;

                                        case (byte) EventCommand.GoToLabel:
                                            // Find the label and update the command list position.
                                            FindEventLabel(command.Text1, mapNum, withBlock1.EventId, withBlock1.PageId, ref withBlock1.CurSlot, ref withBlock1.CurList, ref withBlock1.ListLeftOff);
                                            break;

                                        case (byte) EventCommand.SpawnNpc:
                                            if (command.Data1 > 0 && command.Data1 < Data.Map[mapNum].Npc.Length) // Check if Npc exists
                                            {
                                                Npc.SpawnNpc(command.Data1, mapNum);
                                            }

                                            break;

                                        case (byte) EventCommand.FadeIn:
                                            Event.SendSpecialEffect(i, Event.EffectTypeFadein);
                                            break;

                                        case (byte) EventCommand.FadeOut:
                                            Event.SendSpecialEffect(i, Event.EffectTypeFadeout);
                                            break;

                                        case (byte) EventCommand.FlashScreen:
                                            Event.SendSpecialEffect(i, Event.EffectTypeFlash);
                                            break;

                                        case (byte) EventCommand.SetFog:
                                            Event.SendSpecialEffect(i, Event.EffectTypeFog, command.Data1, command.Data2, command.Data3);
                                            break;

                                        case (byte) EventCommand.SetWeather:
                                            Event.SendSpecialEffect(i, Event.EffectTypeWeather, command.Data1, command.Data2);
                                            break;

                                        case (byte) EventCommand.SetScreenTint:
                                            Event.SendSpecialEffect(i, Event.EffectTypeTint, command.Data1, command.Data2, command.Data3, command.Data4);
                                            break;

                                        case (byte) EventCommand.Wait:
                                            withBlock1.ActionTimer = General.GetTimeMs() + command.Data1;
                                            break;

                                        case (byte) EventCommand.ShowPicture:
                                        {
                                            var buffer = new PacketWriter();
                                            {
                                                buffer.WriteEnum(ServerPackets.SPic);
                                                buffer.WriteInt32(withBlock1.EventId); // Event ID.
                                                buffer.WriteByte((byte) command.Data1); // Picture ID.
                                                buffer.WriteByte((byte) command.Data2); // X
                                                buffer.WriteByte((byte) command.Data3); // Y
                                                buffer.WriteByte((byte) command.Data4); // Transparency
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.HidePicture:
                                        {
                                            var buffer = new PacketWriter(8);
                                            {
                                                buffer.WriteEnum(ServerPackets.SPic);
                                                buffer.WriteByte(0); // Hide picture.
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.WaitMovementCompletion:
                                        {
                                            // Ensure the event exists.
                                            if (command.Data1 < Data.Map[mapNum].Event.Length)
                                            {
                                                if (Data.Map[mapNum].Event[command.Data1].Globals == 1)
                                                {
                                                    withBlock1.WaitingForResponse = 4;
                                                    withBlock1.EventMovingId = command.Data1; // Global event ID.
                                                    withBlock1.EventMovingType = 1; // Global.
                                                }
                                                else
                                                {
                                                    //check that local event exists on player
                                                    if (command.Data1 < 0 || command.Data1 >= Data.TempPlayer[i].EventMap.EventPages.Length)
                                                        break;

                                                    withBlock1.WaitingForResponse = 4;
                                                    withBlock1.EventMovingId = command.Data1; // Local event ID.
                                                    withBlock1.EventMovingType = 0; // Local.
                                                }
                                            }

                                            break;
                                        }
                                        case (byte) EventCommand.HoldPlayer:
                                        {
                                            var buffer = new PacketWriter(8);
                                            buffer.WriteEnum(ServerPackets.SHoldPlayer);
                                            buffer.WriteInt32(0); // Hold
                                            PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            break;
                                        }
                                        case (byte) EventCommand.ReleasePlayer:
                                        {
                                            var buffer = new PacketWriter(8);
                                            {
                                                buffer.WriteInt32((int) ServerPackets.SHoldPlayer);
                                                buffer.WriteInt32(1); // Release
                                                PlayerService.Instance.SendDataTo(i, buffer.GetBytes());
                                            }

                                            break;
                                        }
                                    }
                                }

                                // Increment to the next command, unless we've branched or ended.
                                if (!endprocess)
                                    withBlock1.CurSlot++;
                            }
                        }


                        // Clean up finished event processes.
                        if (removeEventProcess)
                        {
                            withBlock1.Active = 0;
                            restartloop = true;
                        }
                    }
                } while (restartloop);
            });
        }

        public static void UpdateEventLogic()
        {
            // These functions have been optimized to reduce redundant calls and improve clarity.
            RemoveDeadEvents();
            SpawnNewEvents();
            ProcessEventMovement();
            ProcessLocalEventMovement();
            ProcessEventCommands();

            // Consider adding a priority system here to determine which events should be processed first.
            // This is just a conceptual example.  A real priority system would need more work.
            // ProcessHighPriorityEvents();
            // ProcessNormalEvents();
            // ProcessLowPriorityEvents();
        }


        public static string ParseEventText(int index, string txt)
        {
            // Use StringBuilder for efficient string manipulation.
            var sb = new System.Text.StringBuilder(txt);

            sb.Replace("/name", Data.Player[index].Name);
            sb.Replace("/p", Data.Player[index].Name);
            sb.Replace("$playername$", Data.Player[index].Name);
            sb.Replace("$playerclass$", Data.Job[Data.Player[index].Job].Name);

            // Process variables (/v[variableIndex]).
            int start = sb.ToString().IndexOf("/v"); // Find the first occurrence.
            while (start >= 0)
            {
                int end = start + 2;
                // Find the end of the number.
                while (end < sb.Length && char.IsDigit(sb[end]))
                {
                    end++;
                }

                if (end > start + 2) // Ensure we found a number.
                {
                    string varIndexStr = sb.ToString(start + 2, end - (start + 2));
                    if (int.TryParse(varIndexStr, out int varIndex))
                    {
                        // Make sure the variable index is within bounds
                        if (varIndex >= 0 && varIndex < Data.Player[index].Variables.Length)
                        {
                            sb.Remove(start, end - start);
                            sb.Insert(start, Data.Player[index].Variables[varIndex].ToString());
                        }
                        else
                        {
                            //invalid variable, remove it from the output.
                            sb.Remove(start, end - start);
                        }
                    }
                    else //should never occur, but just in case.
                        sb.Remove(start, end - start); //if it wasn't a valid number, remove it.
                }
                else // If no number, remove /v
                {
                    sb.Remove(start, 2);
                }

                start = sb.ToString().IndexOf("/v"); //check for any others
            }

            return sb.ToString();
        }

        public static void FindEventLabel(string label, int mapNum, int eventId, int pageId, ref int curSlot, ref int curList, ref int[] listLeftOff)
        {
            // Check for valid map, event, and page.
            if (mapNum < 0 || mapNum >= Data.Map.Length || eventId < 0 || eventId >= Data.Map[mapNum].Event.Length ||
                pageId < 0 || pageId >= Data.Map[mapNum].Event[eventId].Pages.Length)
            {
                //invalid event, don't do anything.
                return;
            }

            int tmpCurSlot = curSlot;
            int tmpCurList = curList;
            int[] tmpListLeftOff = listLeftOff;

            // Initialize data structures.
            var commandList = Data.Map[mapNum].Event[eventId].Pages[pageId].CommandList;

            // Check if commandList is null
            if (commandList == null)
                return;

            listLeftOff = new int[commandList.Length];
            int[] currentListOption = new int[commandList.Length];

            curList = 0;
            curSlot = 0;

            bool removeEventProcess = false;
            bool restartlist;

            while (!removeEventProcess)
            {
                restartlist = false;

                // Restore position if returning from a nested list.
                if (listLeftOff[curList] > 0)
                {
                    curSlot = listLeftOff[curList];
                    listLeftOff[curList] = 0;
                }

                // Check for out-of-bounds conditions.
                if (curList >= commandList.Length)
                {
                    removeEventProcess = true; // Invalid list index.
                    continue;
                }

                if (curSlot >= commandList[curList].CommandCount)
                {
                    if (curList == commandList[curList].ParentList) //should be itself
                    {
                        removeEventProcess = true; // Reached the end of a top-level list.
                    }
                    else
                    {
                        curList = commandList[curList].ParentList;
                        curSlot = 0;
                        restartlist = true;
                    }

                    continue;
                }

                if (!restartlist && !removeEventProcess)
                {
                    // Get the current command.
                    var command = commandList[curList].Commands[curSlot];

                    switch (command.Index)
                    {
                        case (byte) EventCommand.ShowChoices:
                        {
                            int w = 0;
                            if (!string.IsNullOrEmpty(command.Text2))
                            {
                                w = 1;
                                if (!string.IsNullOrEmpty(command.Text3))
                                {
                                    w = 2;
                                    if (!string.IsNullOrEmpty(command.Text4))
                                    {
                                        w = 3;
                                        if (!string.IsNullOrEmpty(command.Text5))
                                        {
                                            w = 4;
                                        }
                                    }
                                }
                            }

                            if (w > 0)
                            {
                                if (currentListOption[curList] < w)
                                {
                                    currentListOption[curList]++;
                                    listLeftOff[curList] = curSlot; // Save current position.

                                    // Jump to the appropriate choice's command list.
                                    switch (currentListOption[curList])
                                    {
                                        case 1: curList = command.Data1; break;
                                        case 2: curList = command.Data2; break;
                                        case 3: curList = command.Data3; break;
                                        case 4: curList = command.Data4; break;
                                    }

                                    curSlot = 0; // Start at the beginning of the new list.
                                }
                                else
                                {
                                    currentListOption[curList] = 0; // Reset for next time.
                                }
                            }

                            break;
                        }
                        case (byte) EventCommand.ConditionalBranch:
                        {
                            // Handle conditional branches (simplified logic).
                            if (currentListOption[curList] == 0)
                            {
                                // First visit: Execute the "if" branch.
                                listLeftOff[curList] = curSlot;
                                curList = command.ConditionalBranch.CommandList;
                                curSlot = 0;
                            }
                            else if (currentListOption[curList] == 1)
                            {
                                // Second visit: Execute the "else" branch (if it exists).
                                listLeftOff[curList] = curSlot;
                                curList = command.ConditionalBranch.ElseCommandList;
                                curSlot = 0;
                            }

                            //else currentlistoption = 2, so continue on.
                            currentListOption[curList] = (currentListOption[curList] + 1) % 3; //prepare for next visit

                            break;
                        }
                        case (byte) EventCommand.Label:
                        {
                            // Check if this is the target label.
                            if (command.Text1 == label)
                            {
                                return; // Found the label, return to the caller.
                            }

                            break;
                        }
                    }

                    curSlot++; // Move to the next command.
                }
            }

            // Label not found, restore original values.
            curList = tmpCurList;
            curSlot = tmpCurSlot;
            listLeftOff = tmpListLeftOff;
        }

        // Replace FindNpcPath with an A* pathfinding implementation
        public static int FindNpcPath(int mapNum, double mapNpcNum, int targetx, int targety)
        {
            // Validate map and NPC
            if (mapNum < 0 || mapNum >= Data.Map.Length || mapNpcNum < 0 || mapNpcNum >= Data.MapNpc[mapNum].Npc.Length)
                return 4;

            int startX = Data.MapNpc[mapNum].Npc[(int) mapNpcNum].X;
            int startY = Data.MapNpc[mapNum].Npc[(int) mapNpcNum].Y;
            int goalX = targetx < 0 ? 0 : targetx;
            int goalY = targety < 0 ? 0 : targety;

            int maxX = Data.Map[mapNum].MaxX;
            int maxY = Data.Map[mapNum].MaxY;

            // Early out if already at target
            if (startX == goalX && startY == goalY)
                return 4;

            // Node structure for A*
            var openSet = new PriorityQueue<(int x, int y), int>();
            var cameFrom = new Dictionary<(int, int), (int, int)>();
            var gScore = new Dictionary<(int, int), int>();
            var fScore = new Dictionary<(int, int), int>();

            (int x, int y) start = (startX, startY);
            (int x, int y) goal = (goalX, goalY);

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);
            openSet.Enqueue(start, fScore[start]);

            // Directions: Right, Down, Up, Left (to match original return values)
            int[] dx = {1, 0, 0, -1};
            int[] dy = {0, 1, -1, 0};
            int[] dirResult = {(int) Direction.Right, (int) Direction.Down, (int) Direction.Up, (int) Direction.Left};

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.Equals(goal))
                {
                    // Reconstruct path to get the first step
                    var path = new List<(int x, int y)>();
                    var node = current;
                    while (cameFrom.ContainsKey(node))
                    {
                        path.Add(node);
                        node = cameFrom[node];
                    }

                    path.Reverse();
                    if (path.Count == 0)
                        return 4;
                    var firstStep = path[0];
                    for (int d = 0; d < 4; d++)
                    {
                        if (startX + dx[d] == firstStep.x && startY + dy[d] == firstStep.y)
                            return dirResult[d];
                    }

                    return 4;
                }

                for (int d = 0; d < 4; d++)
                {
                    int nx = current.x + dx[d];
                    int ny = current.y + dy[d];
                    if (nx < 0 || ny < 0 || nx > maxX || ny > maxY)
                        continue;
                    if (!IsTileWalkable(mapNum, nx, ny))
                        continue;

                    var neighbor = (nx, ny);
                    int tentativeG = gScore[current] + 1;
                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);
                        if (!openSet.UnorderedItems.Any(item => item.Element.Equals(neighbor)))
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }

            return 4; // No path found

            // Manhattan distance
            static int Heuristic((int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

            // Use Event's IsTileWalkable if available, otherwise treat 0 as walkable
            static bool IsTileWalkable(int mapNum, int x, int y)
            {
                // Use Event.IsTileWalkable if available, otherwise always return true for this stub
                // Replace with actual walkability logic as needed
                return true;
            }
        }

        public static async System.Threading.Tasks.Task SpawnAllMapGlobalEvents()
        {
            // Use Task.Run to avoid blocking the main thread.
            await System.Threading.Tasks.Task.Run(() =>
            {
                for (int i = 0; i < Core.Globals.Constant.MaxMaps; i++)
                {
                    SpawnGlobalEvents(i).ConfigureAwait(false);
                }
            });
        }

        public static async System.Threading.Tasks.Task SpawnGlobalEvents(int mapNum)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                // Check if the map exists and has events.
                if (mapNum < 0 || mapNum >= Data.Map.Length || Data.Map[mapNum].EventCount <= 0)
                {
                    return;
                }

                // Initialize the temporary event map.
                Event.TempEventMap[mapNum].EventCount = 0;
                Array.Resize(ref Event.TempEventMap[mapNum].Event, 1); // Start with size 1, resize as needed.

                for (int i = 0; i < Data.Map[mapNum].EventCount; i++)
                {
                    // Check for valid global events.
                    if (Data.Map[mapNum].Event[i].PageCount > 0 && Data.Map[mapNum].Event[i].Globals == 1)
                    {
                        // Add a new event to the temporary map.
                        Event.TempEventMap[mapNum].EventCount++;
                        Array.Resize(ref Event.TempEventMap[mapNum].Event, Event.TempEventMap[mapNum].EventCount + 1); // +1 for easier indexing
                        ref var tempEvent = ref Event.TempEventMap[mapNum].Event[Event.TempEventMap[mapNum].EventCount];

                        // Set initial event properties.
                        tempEvent.X = Data.Map[mapNum].Event[i].X;
                        tempEvent.Y = Data.Map[mapNum].Event[i].Y;
                        tempEvent.Dir = Data.Map[mapNum].Event[i].Pages[0].GraphicType == 1
                            ? (Data.Map[mapNum].Event[i].Pages[0].GraphicY % 4) switch
                            {
                                0 => (int) Direction.Down,
                                1 => (int) Direction.Left,
                                2 => (int) Direction.Right,
                                _ => (int) Direction.Up //3
                            }
                            : (int) Direction.Down;
                        tempEvent.Active = 0;
                        tempEvent.MoveType = Data.Map[mapNum].Event[i].Pages[0].MoveType;

                        if (tempEvent.MoveType == 2) // Custom Move Route
                        {
                            int moveRouteCount = Data.Map[mapNum].Event[i].Pages[0].MoveRouteCount;
                            tempEvent.MoveRouteCount = moveRouteCount;

                            if (moveRouteCount > 0)
                            {
                                // Copy the move route.
                                tempEvent.MoveRoute = new MoveRoute[moveRouteCount];
                                Array.Copy(Data.Map[mapNum].Event[i].Pages[0].MoveRoute, tempEvent.MoveRoute, moveRouteCount);
                                tempEvent.MoveRouteComplete = 0; // Reset completion status.
                            }
                            else
                            {
                                tempEvent.MoveRouteComplete = 1;
                            }
                        }
                        else
                        {
                            tempEvent.MoveRouteComplete = 1; // Not a move route, so considered complete.
                        }

                        tempEvent.RepeatMoveRoute = Data.Map[mapNum].Event[i].Pages[0].RepeatMoveRoute;
                        tempEvent.IgnoreIfCannotMove = Data.Map[mapNum].Event[i].Pages[0].IgnoreMoveRoute;
                        tempEvent.MoveFreq = Data.Map[mapNum].Event[i].Pages[0].MoveFreq;
                        tempEvent.MoveSpeed = Data.Map[mapNum].Event[i].Pages[0].MoveSpeed;
                        tempEvent.WalkThrough = Data.Map[mapNum].Event[i].Pages[0].WalkThrough;
                        tempEvent.FixedDir = Data.Map[mapNum].Event[i].Pages[0].DirFix;
                        tempEvent.WalkingAnim = Data.Map[mapNum].Event[i].Pages[0].WalkAnim;
                        tempEvent.ShowName = Data.Map[mapNum].Event[i].Pages[0].ShowName;
                    }
                }
            });
        }

        public static void SpawnMapEventsFor(int index, int mapNum)
        {
            // Check for valid map.
            if (mapNum < 0 || mapNum >= Data.Map.Length)
            {
                return;
            }

            // Reset player's event data.
            Data.TempPlayer[index].EventMap.CurrentEvents = 0;
            Array.Resize(ref Data.TempPlayer[index].EventMap.EventPages, 1);

            // Initialize event processing array.
            if (Data.Map[mapNum].EventCount > 0)
            {
                Array.Resize(ref Data.TempPlayer[index].EventProcessing, Data.Map[mapNum].EventCount + 1); //+1 for easier indexing
                Data.TempPlayer[index].EventProcessingCount = Data.Map[mapNum].EventCount;
            }
            else
            {
                Array.Resize(ref Data.TempPlayer[index].EventProcessing, 1); //+1 for easier indexing
                Data.TempPlayer[index].EventProcessingCount = 0;
            }

            if (Data.Map[mapNum].EventCount <= 0) return;

            // Iterate through map events.
            for (int i = 0; i < Data.Map[mapNum].EventCount; i++)
            {
                int p = -1;

                // Check if event and its pages exist
                if (Data.Map[mapNum].Event[i].Pages == null) continue;
                if (Data.Map[mapNum].Event[i].PageCount <= 0) continue;

                // Find the highest-priority page that meets conditions.
                for (int z = 0; z < Data.Map[mapNum].Event[i].PageCount; z++)
                {
                    bool spawnCurrentEvent = true;
                    ref var page = ref Data.Map[mapNum].Event[i].Pages[z]; // Use ref for direct modification.
                    bool variableConditionMet = false;

                    // Check conditions (Variable, Switch, Item, Self Switch).
                    if (page.ChkVariable == 1)
                    {
                        int playerVar = Data.Player[index].Variables[page.VariableIndex];
                        switch (page.VariableCompare)
                        {
                            case 0: variableConditionMet = playerVar == page.VariableCondition; break;
                            case 1: variableConditionMet = playerVar >= page.VariableCondition; break;
                            case 2: variableConditionMet = playerVar <= page.VariableCondition; break;
                            case 3: variableConditionMet = playerVar > page.VariableCondition; break;
                            case 4: variableConditionMet = playerVar < page.VariableCondition; break;
                            case 5: variableConditionMet = playerVar != page.VariableCondition; break;
                        }

                        if (!variableConditionMet)
                            spawnCurrentEvent = false;
                    }

                    if (page.ChkSwitch == 1)
                    {
                        // Using XOR for switch check, handles both expecting true and false efficiently
                        if (!((page.SwitchCompare == 1) ^ (Data.Player[index].Switches[page.SwitchIndex] == 0))) //we want true
                            spawnCurrentEvent = false;
                    }

                    if (page.ChkHasItem == 1 && Player.HasItem(index, page.HasItemIndex) == 0)
                    {
                        spawnCurrentEvent = false;
                    }

                    if (page.ChkSelfSwitch == 1)
                    {
                        int compare = page.SelfSwitchCompare; // 0 or 1, no need to check both values explicitly.
                        bool selfSwitchState;

                        if (Data.Map[mapNum].Event[i].Globals == 1)
                            selfSwitchState = Data.Map[mapNum].Event[i].SelfSwitches[page.SelfSwitchIndex] == compare;
                        else
                            selfSwitchState = false; // Local self switches are not checked when spawning.

                        if (!selfSwitchState)
                            spawnCurrentEvent = false;
                    }


                    if (spawnCurrentEvent)
                    {
                        p = z; // Store the valid page index.
                    }
                }


                // Spawn the event if a valid page was found.
                if (p >= 0)
                {
                    int z = p;

                    Data.TempPlayer[index].EventMap.CurrentEvents++;
                    Array.Resize(ref Data.TempPlayer[index].EventMap.EventPages, Data.TempPlayer[index].EventMap.CurrentEvents + 1);
                    ref var withBlock1 = ref Data.TempPlayer[index].EventMap.EventPages[Data.TempPlayer[index].EventMap.CurrentEvents];

                    ref var eventPage = ref Data.Map[mapNum].Event[i].Pages[z];

                    // Set up the event page data.
                    withBlock1.Dir = eventPage.GraphicType == 1
                        ? (eventPage.GraphicY % 4) switch
                        {
                            0 => (int) Direction.Down,
                            1 => (int) Direction.Left,
                            2 => (int) Direction.Right,
                            _ => (int) Direction.Up
                        }
                        : 0;

                    withBlock1.Graphic = eventPage.Graphic;
                    withBlock1.GraphicType = eventPage.GraphicType;
                    withBlock1.GraphicX = eventPage.GraphicX;
                    withBlock1.GraphicY = eventPage.GraphicY;
                    withBlock1.GraphicX2 = eventPage.GraphicX2;
                    withBlock1.GraphicY2 = eventPage.GraphicY2;
                    withBlock1.MovementSpeed = eventPage.MoveSpeed switch
                    {
                        0 => 2,
                        1 => 3,
                        2 => 4,
                        3 => 6,
                        4 => 12,
                        5 => 24,
                        _ => DefaultMovementSpeed
                    };

                    if (Data.Map[mapNum].Event[i].Globals == 1)
                    {
                        // Use global event's position and direction.
                        withBlock1.X = Event.TempEventMap[mapNum].Event[i].X * 32;
                        withBlock1.Y = Event.TempEventMap[mapNum].Event[i].Y * 32;
                        withBlock1.Dir = Event.TempEventMap[mapNum].Event[i].Dir;
                        withBlock1.MoveRouteStep = Event.TempEventMap[mapNum].Event[i].MoveRouteStep;
                    }
                    else
                    {
                        // Use the event's initial position.
                        withBlock1.X = Data.Map[mapNum].Event[i].X * 32;
                        withBlock1.Y = Data.Map[mapNum].Event[i].Y * 32;
                        withBlock1.MoveRouteStep = 0;
                    }

                    withBlock1.Position = eventPage.Position;
                    withBlock1.EventId = i; // Map event ID.
                    withBlock1.PageId = z;
                    withBlock1.Visible = true; // Always visible when initially spawned.
                    withBlock1.MoveType = eventPage.MoveType;

                    if (withBlock1.MoveType == 2) // Custom move route
                    {
                        withBlock1.MoveRouteCount = eventPage.MoveRouteCount;

                        if (eventPage.MoveRouteCount > 0)
                        {
                            withBlock1.MoveRoute = new MoveRoute[eventPage.MoveRouteCount];
                            Array.Copy(eventPage.MoveRoute, withBlock1.MoveRoute, eventPage.MoveRouteCount);
                            withBlock1.MoveRouteComplete = 0; // Reset completion status
                        }
                        else
                            withBlock1.MoveRouteComplete = 1;
                    }
                    else
                    {
                        withBlock1.MoveRouteComplete = 1;
                    }

                    withBlock1.RepeatMoveRoute = eventPage.RepeatMoveRoute;
                    withBlock1.IgnoreIfCannotMove = eventPage.IgnoreMoveRoute;
                    withBlock1.MoveFreq = eventPage.MoveFreq;
                    withBlock1.MoveSpeed = eventPage.MoveSpeed;
                    withBlock1.WalkingAnim = eventPage.WalkAnim;
                    withBlock1.WalkThrough = eventPage.WalkThrough;
                    withBlock1.ShowName = eventPage.ShowName;
                    withBlock1.FixedDir = eventPage.DirFix;
                }
            }

            // Send spawn event packets to the player.
            var buffer = new PacketWriter();
            buffer.WriteEnum(ServerPackets.SSpawnEvent);
            buffer.WriteInt32(Data.TempPlayer[index].EventMap.CurrentEvents);

            for (int i = 0; i < Data.TempPlayer[index].EventMap.CurrentEvents; i++)
            {
                ref var eventPage = ref Data.TempPlayer[index].EventMap.EventPages[i];

                buffer.WriteInt32(eventPage.EventId); // Map event ID.

                buffer.WriteString(Data.Map[mapNum].Event[eventPage.EventId].Name); // Map event ID
                buffer.WriteInt32(eventPage.Dir);
                buffer.WriteByte(eventPage.GraphicType);
                buffer.WriteInt32(eventPage.Graphic);
                buffer.WriteInt32(eventPage.GraphicX);
                buffer.WriteInt32(eventPage.GraphicX2);
                buffer.WriteInt32(eventPage.GraphicY);
                buffer.WriteInt32(eventPage.GraphicY2);
                buffer.WriteInt32(eventPage.MovementSpeed);
                buffer.WriteInt32(eventPage.X);
                buffer.WriteInt32(eventPage.Y);
                buffer.WriteByte(eventPage.Position);
                buffer.WriteBoolean(eventPage.Visible);
                buffer.WriteInt32(Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].WalkAnim); // Use map event and page IDs
                buffer.WriteInt32(Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].DirFix);
                buffer.WriteInt32(Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].WalkThrough);
                buffer.WriteInt32(Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].ShowName);
            }

            PlayerService.Instance.SendDataTo(index, buffer.GetBytes());
        }

        public static bool TriggerEvent(int playerIndex, int eventId, byte triggerType, int targetX, int targetY)
        {
            // 1. Validate player and map
            if (playerIndex < 0)
                return false;

            int mapNum = GetPlayerMap(playerIndex);
            if (mapNum < 0 || mapNum >= Data.Map.Length)
                return false;

            // 2. Find the relevant event for the player
            var eventMap = Data.TempPlayer[playerIndex].EventMap;
            int localEventIndex = -1;
            for (int i = 0; i < eventMap.CurrentEvents; i++)
            {
                if (eventMap.EventPages[i].EventId == eventId)
                {
                    localEventIndex = i + 1;
                    break;
                }
            }

            if (localEventIndex == -1)
                return false; // Event not found

            ref var eventPage = ref eventMap.EventPages[localEventIndex];
            var mapEvent = Data.Map[mapNum].Event[eventPage.EventId];
            var page = mapEvent.Pages[eventPage.PageId];

            // 3. Check trigger type
            if (page.Trigger != triggerType)
                return false;

            // 4. Calculate intended tile based on player direction (if not walk-through)
            if (page.WalkThrough == 0)
            {
                (int x, int y)? offset = GetOffsetByDirection(GetPlayerDir(playerIndex), GetPlayerX(playerIndex), GetPlayerY(playerIndex), Data.Map[mapNum]);
                if (offset == null)
                    return false;
                (targetX, targetY) = offset.Value;
            }

            // 5. Validate player is at the event's coordinates
            if (targetX * 32 != eventPage.X || targetY * 32 != eventPage.Y)
                return false;

            // 6. Begin event processing if applicable
            if (page.CommandListCount > 0)
            {
                ref var eventProcessing = ref Data.TempPlayer[playerIndex].EventProcessing[localEventIndex];
                if (eventProcessing.Active == 0)
                {
                    eventProcessing.Active = 1;
                    eventProcessing.ActionTimer = General.GetTimeMs();
                    eventProcessing.CurList = 0;
                    eventProcessing.CurSlot = 0;
                    eventProcessing.EventId = eventPage.EventId;
                    eventProcessing.PageId = eventPage.PageId;
                    eventProcessing.WaitingForResponse = 0;
                    eventProcessing.ListLeftOff = new int[page.CommandListCount];
                    // Event successfully triggered and processing started.
                    return true;
                }
            }

            return false;
        }

        // Helper to calculate tile offsets based on player direction and map bounds
        private static (int, int)? GetOffsetByDirection(byte direction, int x, int y, Map map)
        {
            int newX = x, newY = y;
            switch ((Direction) direction)
            {
                case Direction.Up:
                    if (y > 0) newY = y - 1;
                    else return null;
                    break;
                case Direction.Down:
                    if (y < map.MaxY) newY = y + 1;
                    else return null;
                    break;
                case Direction.Left:
                    if (x > 0) newX = x - 1;
                    else return null;
                    break;
                case Direction.Right:
                    if (x < map.MaxX) newX = x + 1;
                    else return null;
                    break;
                case Direction.UpRight:
                    if (x < map.MaxX && y > 0)
                    {
                        newX = x + 1;
                        newY = y - 1;
                    }
                    else return null;

                    break;
                case Direction.UpLeft:
                    if (x > 0 && y > 0)
                    {
                        newX = x - 1;
                        newY = y - 1;
                    }
                    else return null;

                    break;
                case Direction.DownLeft:
                    if (x > 0 && y < map.MaxY)
                    {
                        newX = x - 1;
                        newY = y + 1;
                    }
                    else return null;

                    break;
                case Direction.DownRight:
                    if (x < map.MaxX && y < map.MaxY)
                    {
                        newX = x + 1;
                        newY = y + 1;
                    }
                    else return null;

                    break;
                default:
                    return null;
            }

            return (newX, newY);
        }
    }
}