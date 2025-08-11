using System;
using Client.Net;
using Core;
using Core.Globals;
using Core.Net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using static Core.Globals.Type;
using EventCommand = Core.Globals.EventCommand;
using Type = Core.Globals.Type;

namespace Client
{
    public class Event
    {
        #region Globals

        // Temp event storage
        public static Type.Event TmpEvent;

        public static bool IsEdit;

        public static int CurPageNum;
        public static int CurCommand;
        public static int GraphicSelX;
        public static int GraphicSelY;
        public static int GraphicSelX2;
        public static int GraphicSelY2;

        public static int EventTileX;
        public static int EventTileY;

        public static int EditorEvent;

        public static int GraphicSelType;
        public static int TempMoveRouteCount;
        public static Type.MoveRoute[]? TempMoveRoute;
        public static bool IsMoveRouteCommand;
        public static int[]? ListOfEvents;

        public static int EventReplyId;
        public static int EventReplyPage;
        public static int EventChatFace;

        public static int RenameType;
        public static int RenameIndex;
        public static int EventChatTimer;

        public static bool EventChat;
        public static string EventText;
        public static bool ShowEventLbl;
        public static string[] EventChoices = new string[Constant.MaxEventChoices];
        public static bool[] EventChoiceVisible = new bool[Constant.MaxEventChoices];
        public static int EventChatType;
        public static int AnotherChat;

        // constants
        public static string[] Switches = new string[Constant.MaxSwitches];
        public static string[] Variables = new string[Constant.MaxVariables];

        public static bool EventCopy;
        public static bool EventPaste;
        public static Type.EventList[]? EventList;
        public static Type.Event CopyEvent;
        public static Type.EventPage CopyEventPage;

        public static bool InEvent;
        public static bool HoldPlayer;

        public static Type.Picture Picture;

        #endregion

        #region EventEditor

        public static void CopyEvent_Map(int X, int Y)
        {
            int count;
            int i;

            count = Data.MyMap.EventCount;
            if (count == 0)
                return;

            var loopTo = count;
            for (i = 0; i < loopTo; i++)
            {
                if (Data.MyMap.Event[i].X == X & Data.MyMap.Event[i].Y == Y)
                {
                    CopyEvent = Data.MyMap.Event[i];
                    return;
                }
            }
        }

        public static void PasteEvent_Map(int x, int y)
        {
            int count;
            int i;
            var EventNum = default(int);

            count = Data.MyMap.EventCount;

            if (count > 0)
            {
                var loopTo = count;
                for (i = 0; i < loopTo; i++)
                {
                    if (Data.MyMap.Event[i].X == x & Data.MyMap.Event[i].Y == y)
                    {
                        EventNum = i;
                    }
                }
            }

            // couldn't find one - create one
            if (EventNum == 0)
            {
                AddEvent(x, y, true);
                EventNum = count;
            }

            // copy it
            Data.MyMap.Event[EventNum] = CopyEvent;

            // set position
            Data.MyMap.Event[EventNum].X = x;
            Data.MyMap.Event[EventNum].Y = y;
        }

        public static void DeleteEvent(int X, int Y)
        {
            int i;
            int lowIndex = -1;

            if (GameState.MyEditorType != EditorType.Map)
                return;

            // First pass: find all events to delete and shift others down
            var loopTo = Data.MyMap.EventCount;
            for (i = 0; i < loopTo; i++)
            {
                if (Data.MyMap.Event.Length <= i)
                    break;

                if (Data.MyMap.Event[i].X == X & Data.MyMap.Event[i].Y == Y)
                {
                    // Clear the event
                    ClearEvent(i);
                    lowIndex = i;
                    break;
                }
            }

            if (lowIndex != -1)
            {
                for (i = lowIndex; i < Data.MyMap.EventCount; i++)
                {
                    if (Information.UBound(Data.MyMap.Event) > 2)
                    {
                        Data.MyMap.Event[i] = Data.MyMap.Event[i + 1];
                    }
                }

                for (i = lowIndex; i < Data.MyMap.EventCount; i++)
                {
                    if (Information.UBound(Data.MapEvents) > 2)
                    {
                        Data.MapEvents[i] = Data.MapEvents[i + 1];
                    }
                }

                TmpEvent = default;
            }
        }


        public static void AddEvent(int X, int Y, bool cancelLoad = false)
        {
            int count;
            int pageCount;
            int i;

            count = Data.MyMap.EventCount;

            // make sure there's not already an event
            if (count > 0)
            {
                var loopTo = count;
                for (i = 0; i < loopTo; i++)
                {
                    if (Data.MyMap.Event[i].X == X & Data.MyMap.Event[i].Y == Y)
                    {
                        // already an event - edit it
                        if (!cancelLoad)
                            EventEditorInit(i);
                        return;
                    }
                }
            }

            // increment count
            if (count == 0)
            {
                count = 1;
            }
            else
            {
                count++;
            }

            Data.MyMap.EventCount = count;
            Array.Resize(ref Data.MyMap.Event, count + 1);
            Array.Resize(ref Data.MapEvents, count + 1);
            ClearEvent(count);
            // set the new event
            Data.MyMap.Event[count - 1].X = X;
            Data.MyMap.Event[count - 1].Y = Y;
            // give it a new page
            pageCount = Data.MyMap.Event[count - 1].PageCount + 1;
            Data.MyMap.Event[count - 1].PageCount = pageCount;
            Array.Resize(ref Data.MyMap.Event[count - 1].Pages, pageCount);
            // load the editor
            if (!cancelLoad)
                EventEditorInit(count - 1);
        }

        public static void ClearEvent(int eventNum)
        {
            ref var withBlock = ref Data.MyMap.Event[eventNum];
            withBlock.Name = "";
            withBlock.PageCount = 1;
            withBlock.Pages = new Type.EventPage[1];
            Array.Resize(ref withBlock.Pages[0].CommandList, 1);
            Array.Resize(ref withBlock.Pages[0].CommandList[0].Commands, 1);
            withBlock.Pages[0].CommandList[0].Commands[0].Index = -1;
            withBlock.Globals = 0;
            withBlock.X = 0;
            withBlock.Y = 0;
        }

        public static void EventEditorInit(int EventNum)
        {
            EditorEvent = EventNum;
            TmpEvent = Data.MyMap.Event[EventNum];
            GameState.InitEventEditor = true;
            if (TmpEvent.Pages[0].CommandListCount == 0)
            {
                Array.Resize(ref TmpEvent.Pages[0].CommandList, 1);
                TmpEvent.Pages[0].CommandListCount = 0;
                TmpEvent.Pages[0].CommandList[0].CommandCount = 0;
                Array.Resize(ref TmpEvent.Pages[0].CommandList[0].Commands, TmpEvent.Pages[0].CommandList[0].CommandCount);
            }
        }

        public static void EventEditorLoadPage(int pageNum)
        {
            if (Event.TmpEvent.Pages == null)
                return;

            if (pageNum < 0 || pageNum >= TmpEvent.Pages.Length || TmpEvent.Pages == null)
            {
                // Invalid page number, return or throw an exception
                return;
            }

            ref var withBlock = ref TmpEvent.Pages[pageNum];
            GraphicSelX = withBlock.GraphicX;
            GraphicSelY = withBlock.GraphicY;
            GraphicSelX2 = withBlock.GraphicX2;
            GraphicSelY2 = withBlock.GraphicY2;
            Editor_Event.Instance.cmbGraphic.SelectedIndex = withBlock.GraphicType;
            Editor_Event.Instance.cmbHasItem.SelectedIndex = withBlock.HasItemIndex;
            if (withBlock.HasItemAmount == 0)
            {
                Editor_Event.Instance.nudCondition_HasItem.Value = 1;
            }
            else
            {
                Editor_Event.Instance.nudCondition_HasItem.Value = withBlock.HasItemAmount;
            }

            Editor_Event.Instance.cmbMoveFreq.SelectedIndex = withBlock.MoveFreq;
            Editor_Event.Instance.cmbMoveSpeed.SelectedIndex = withBlock.MoveSpeed;
            Editor_Event.Instance.cmbMoveType.SelectedIndex = withBlock.MoveType;
            Editor_Event.Instance.cmbPlayerVar.SelectedIndex = withBlock.VariableIndex;
            Editor_Event.Instance.cmbPlayerSwitch.SelectedIndex = withBlock.SwitchIndex;
            Editor_Event.Instance.cmbSelfSwitchCompare.SelectedIndex = withBlock.SelfSwitchCompare;
            Editor_Event.Instance.cmbPlayerSwitchCompare.SelectedIndex = withBlock.SwitchCompare;
            Editor_Event.Instance.cmbPlayervarCompare.SelectedIndex = withBlock.VariableCompare;
            Editor_Event.Instance.chkGlobal.Checked = Conversions.ToBoolean(TmpEvent.Globals);
            Editor_Event.Instance.cmbTrigger.SelectedIndex = withBlock.Trigger;
            Editor_Event.Instance.chkDirFix.Checked = Conversions.ToBoolean(withBlock.DirFix);
            Editor_Event.Instance.chkHasItem.Checked = Conversions.ToBoolean(withBlock.ChkHasItem);
            Editor_Event.Instance.chkPlayerVar.Checked = Conversions.ToBoolean(withBlock.ChkVariable);
            Editor_Event.Instance.chkPlayerSwitch.Checked = Conversions.ToBoolean(withBlock.ChkSwitch);
            Editor_Event.Instance.chkSelfSwitch.Checked = Conversions.ToBoolean(withBlock.ChkSelfSwitch);
            Editor_Event.Instance.chkWalkAnim.Checked = Conversions.ToBoolean(withBlock.WalkAnim);
            Editor_Event.Instance.chkWalkThrough.Checked = Conversions.ToBoolean(withBlock.WalkThrough);
            Editor_Event.Instance.chkShowName.Checked = Conversions.ToBoolean(withBlock.ShowName);
            Editor_Event.Instance.nudPlayerVariable.Value = withBlock.VariableCondition;
            Editor_Event.Instance.nudGraphic.Value = withBlock.Graphic;

            if (withBlock.ChkSelfSwitch == 0)
            {
                Editor_Event.Instance.cmbSelfSwitch.Enabled = false;
                Editor_Event.Instance.cmbSelfSwitchCompare.Enabled = false;
            }
            else
            {
                Editor_Event.Instance.cmbSelfSwitch.Enabled = true;
                Editor_Event.Instance.cmbSelfSwitchCompare.Enabled = true;
            }

            if (withBlock.ChkSwitch == 0)
            {
                Editor_Event.Instance.cmbPlayerSwitch.Enabled = false;
                Editor_Event.Instance.cmbPlayerSwitchCompare.Enabled = false;
            }
            else
            {
                Editor_Event.Instance.cmbPlayerSwitch.Enabled = true;
                Editor_Event.Instance.cmbPlayerSwitchCompare.Enabled = true;
            }

            if (withBlock.ChkVariable == 0)
            {
                Editor_Event.Instance.cmbPlayerVar.Enabled = false;
                Editor_Event.Instance.nudPlayerVariable.Enabled = false;
                Editor_Event.Instance.cmbPlayervarCompare.Enabled = false;
            }
            else
            {
                Editor_Event.Instance.cmbPlayerVar.Enabled = true;
                Editor_Event.Instance.nudPlayerVariable.Enabled = true;
                Editor_Event.Instance.cmbPlayervarCompare.Enabled = true;
            }

            if (Editor_Event.Instance.cmbMoveType.SelectedIndex == 2)
            {
                Editor_Event.Instance.btnMoveRoute.Enabled = true;
            }
            else
            {
                Editor_Event.Instance.btnMoveRoute.Enabled = false;
            }

            Editor_Event.Instance.cmbPositioning.SelectedIndex = int.Parse(withBlock.Position.ToString());
            EventListCommands();
        }

        public static void EventEditorOK()
        {
            // copy the event data from the temp event
            Data.MyMap.Event[EditorEvent] = TmpEvent;
            TmpEvent = default;

            // unload the form
            Editor_Event.Instance.Dispose();
        }

        public static void EventListCommands()
        {
            int i;
            int curlist;
            int X;
            string indent = "";
            int[] listleftoff;
            int[] conditionalstage;

            if (TmpEvent.Pages == null)
                return;

            Editor_Event.Instance.lstCommands.Items.Clear();

            if (TmpEvent.Pages[CurPageNum].CommandListCount > 0)
            {
                listleftoff = new int[TmpEvent.Pages[CurPageNum].CommandListCount];
                conditionalstage = new int[TmpEvent.Pages[CurPageNum].CommandListCount];
                curlist = 0;
                X = 0;
                Array.Resize(ref EventList, X + 1);
                newlist:
                var loopTo = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount;
                for (i = 0; i < loopTo; i++)
                {
                    if (listleftoff[curlist] > 0)
                    {
                        if ((TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[listleftoff[curlist]].Index == (int) EventCommand.ConditionalBranch | TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[listleftoff[curlist]].Index == (int) EventCommand.ShowChoices) & conditionalstage[curlist] != 0)
                        {
                            i = listleftoff[curlist];
                        }
                        else if (listleftoff[curlist] >= i)
                        {
                            i = listleftoff[curlist] + 1;
                        }
                    }

                    if (i < TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount)
                    {
                        if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Index == (int) EventCommand.ConditionalBranch)
                        {
                            X = X + 1;
                            Array.Resize(ref EventList, X + 1);
                            switch (conditionalstage[curlist])
                            {
                                case 0:
                                {
                                    EventList[X].CommandList = curlist;
                                    EventList[X].CommandNum = i;
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Condition)
                                    {
                                        case 0:
                                        {
                                            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2)
                                            {
                                                case 0:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] == " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                                case 1:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] >= " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                                case 2:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] <= " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                                case 3:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] > " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                                case 4:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] < " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                                case 5:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Variable [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1] + 1 + "] != " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        case 1:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Switch [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Switches[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + 1] + "] == " + "True");
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Switch [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + ". " + Switches[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + 1] + "] == " + "False");
                                            }

                                            break;
                                        }
                                        case 2:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Has Item [" + Data.Item[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1].Name + "] x" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2);
                                            break;
                                        }
                                        case 3:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Job Is [" + Strings.Trim(Data.Job[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1].Name) + "]");
                                            break;
                                        }
                                        case 4:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player Knows Skill [" + Strings.Trim(Data.Skill[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1].Name) + "]");
                                            break;
                                        }
                                        case 5:
                                        {
                                            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2)
                                            {
                                                case 0:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is == " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                                case 1:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is >= " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                                case 2:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is <= " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                                case 3:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is > " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                                case 4:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is < " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                                case 5:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Level is NOT " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1);
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        case 6:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 0)
                                            {
                                                switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1)
                                                {
                                                    case 0:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [A] == " + "True");
                                                        break;
                                                    }
                                                    case 1:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [B] == " + "True");
                                                        break;
                                                    }
                                                    case 2:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [C] == " + "True");
                                                        break;
                                                    }
                                                    case 3:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [D] == " + "True");
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 1)
                                            {
                                                switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1)
                                                {
                                                    case 0:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [A] == " + "False");
                                                        break;
                                                    }
                                                    case 1:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [B] == " + "False");
                                                        break;
                                                    }
                                                    case 2:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [C] == " + "False");
                                                        break;
                                                    }
                                                    case 3:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Self Switch [D] == " + "False");
                                                        break;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                        case 7:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 0)
                                            {
                                                switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3)
                                                {
                                                    case 0:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] not started.");
                                                        break;
                                                    }
                                                    case 1:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] is started.");
                                                        break;
                                                    }
                                                    case 2:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] is completed.");
                                                        break;
                                                    }
                                                    case 3:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] can be started.");
                                                        break;
                                                    }
                                                    case 4:
                                                    {
                                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] can be ended. (All tasks complete)");
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Quest [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1 + "] in progress and on task #" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data3);
                                            }

                                            break;
                                        }
                                        case 8:
                                        {
                                            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1)
                                            {
                                                case (int) Sex.Male:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's Gender is Male");
                                                    break;
                                                }
                                                case (int) Sex.Female:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Player's  Gender is Female");
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        case 9:
                                        {
                                            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.Data1)
                                            {
                                                case (int) TimeOfDay.Day:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Time of Day is Day");
                                                    break;
                                                }
                                                case (int) TimeOfDay.Night:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Time of Day is Night");
                                                    break;
                                                }
                                                case (int) TimeOfDay.Dawn:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Time of Day is Dawn");
                                                    break;
                                                }
                                                case (int) TimeOfDay.Dusk:
                                                {
                                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Conditional Branch: Time of Day is Dusk");
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    indent = indent + "       ";
                                    listleftoff[curlist] = i;
                                    conditionalstage[curlist] = 1;
                                    curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.CommandList;
                                    goto newlist;
                                }
                                case 1:
                                {
                                    EventList[X].CommandList = curlist;
                                    EventList[X].CommandNum = 0;
                                    Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "Else");
                                    listleftoff[curlist] = i;
                                    conditionalstage[curlist] = 2;
                                    curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].ConditionalBranch.ElseCommandList;
                                    goto newlist;
                                }
                                case 2:
                                {
                                    EventList[X].CommandList = curlist;
                                    EventList[X].CommandNum = 0;
                                    Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "End Branch");
                                    indent = Strings.Mid(indent, 1, Strings.Len(indent) - 7);
                                    listleftoff[curlist] = i;
                                    conditionalstage[curlist] = 0;
                                    break;
                                }
                            }
                        }
                        else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Index == (int) EventCommand.ShowChoices)
                        {
                            X = X + 1;
                            switch (conditionalstage[curlist])
                            {
                                case 0:
                                {
                                    Array.Resize(ref EventList, X + 1);
                                    EventList[X].CommandList = curlist;
                                    EventList[X].CommandNum = i;
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Choices - Prompt: " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20));
                                    indent = indent + "       ";
                                    listleftoff[curlist] = i;
                                    conditionalstage[curlist] = 1;
                                    goto newlist;
                                }
                                case 1:
                                {
                                    if (!string.IsNullOrEmpty(Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text2)))
                                    {
                                        Array.Resize(ref EventList, X + 1);
                                        EventList[X].CommandList = 7;
                                        EventList[X].CommandNum = 0;
                                        Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "When [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text2) + "]");
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 2;
                                        curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1;
                                        goto newlist;
                                    }
                                    else
                                    {
                                        X = X - 1;
                                        Array.Resize(ref EventList, X + 1);
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 2;
                                        goto newlist;
                                    }

                                    break;
                                }
                                case 2:
                                {
                                    if (!string.IsNullOrEmpty(Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text3)))
                                    {
                                        Array.Resize(ref EventList, X + 1);
                                        EventList[X].CommandList = curlist;
                                        EventList[X].CommandNum = 0;
                                        Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "When [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text3) + "]");
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 3;
                                        curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2;
                                        goto newlist;
                                    }
                                    else
                                    {
                                        X = X - 1;
                                        Array.Resize(ref EventList, X + 1);
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 3;
                                        curlist = curlist;
                                        goto newlist;
                                    }

                                    break;
                                }
                                case 3:
                                {
                                    if (!string.IsNullOrEmpty(Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text4)))
                                    {
                                        Array.Resize(ref EventList, X + 1);
                                        EventList[X].CommandList = curlist;
                                        EventList[X].CommandNum = 0;
                                        Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "When [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text4) + "]");
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 4;
                                        curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3;
                                        goto newlist;
                                    }
                                    else
                                    {
                                        X = X - 1;
                                        Array.Resize(ref EventList, X + 1);
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 4;
                                        curlist = curlist;
                                        goto newlist;
                                    }

                                    break;
                                }
                                case 4:
                                {
                                    if (!string.IsNullOrEmpty(Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text5)))
                                    {
                                        Array.Resize(ref EventList, X + 1);
                                        EventList[X].CommandList = curlist;
                                        EventList[X].CommandNum = 0;
                                        Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "When [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text5) + "]");
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 5;
                                        curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4;
                                        goto newlist;
                                    }
                                    else
                                    {
                                        X = X - 1;
                                        Array.Resize(ref EventList, X + 1);
                                        listleftoff[curlist] = i;
                                        conditionalstage[curlist] = 5;
                                        curlist = curlist;
                                        goto newlist;
                                    }

                                    break;
                                }
                                case 5:
                                {
                                    Array.Resize(ref EventList, X + 1);
                                    EventList[X].CommandList = curlist;
                                    EventList[X].CommandNum = 0;
                                    Editor_Event.Instance.lstCommands.Items.Add(Strings.Mid(indent, 1, Strings.Len(indent) - 4) + " : " + "Branch End");
                                    indent = Strings.Mid(indent, 1, Strings.Len(indent) - 7);
                                    listleftoff[curlist] = i;
                                    conditionalstage[curlist] = 0;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            X = X + 1;
                            Array.Resize(ref EventList, X + 1);
                            EventList[X].CommandList = curlist;
                            EventList[X].CommandNum = i;
                            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Index)
                            {
                                case (byte) EventCommand.AddText:
                                {
                                    // Build the preview text safely as a string (avoid VB Operators.ConcatenateObject which returns object)
                                    string textPreview = Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20);
                                    string colorStr = Convert.ToString(GetColorString(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1));
                                    string chatType;
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2)
                                    {
                                        case 0:
                                            chatType = "Player";
                                            break;
                                        case 1:
                                            chatType = "Map";
                                            break;
                                        case 2:
                                            chatType = "Global";
                                            break;
                                        default:
                                            chatType = "Unknown";
                                            break;
                                    }
                                    Editor_Event.Instance.lstCommands.Items.Add($"{indent}@>Add Text - {textPreview}... - Color: {colorStr} - Chat Type: {chatType}");
                                    break;
                                }
                                case (byte) EventCommand.ShowText:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Text - " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20));
                                    break;
                                }
                                case (byte) EventCommand.ModifyVariable:
                                {
                                    string variableValue = Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1];
                                    if (variableValue == "")
                                        variableValue = ": None";
                                    else
                                        variableValue = ": " + variableValue;

                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2)
                                    {
                                        case 0:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Variable [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + variableValue + "] == " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3);
                                            break;
                                        }
                                        case 1:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Variable [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + variableValue + "] + " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3);
                                            break;
                                        }
                                        case 2:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Variable [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + variableValue + "] - " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3);
                                            break;
                                        }
                                        case 3:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Variable [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + variableValue + "] Random Between " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + " and " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4);
                                            break;
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.ModifySwitch:
                                {
                                    string switchValue = Variables[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1];
                                    if (switchValue == "")
                                        switchValue = ": None";
                                    else
                                        switchValue = ": " + switchValue;

                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Switch [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + switchValue + "] == False");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Switch [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + switchValue + "] == True");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.ModifySelfSwitch:
                                {
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1)
                                    {
                                        case 0:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [A] to Off");
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [A] to On");
                                            }

                                            break;
                                        }
                                        case 1:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [B] to Off");
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [B] to On");
                                            }

                                            break;
                                        }
                                        case 2:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [C] to Off");
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [C] to On");
                                            }

                                            break;
                                        }
                                        case 3:
                                        {
                                            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [D] to Off");
                                            }
                                            else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Self Switch [D] to On");
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.ExitEventProcess:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Exit Event Processing");
                                    break;
                                }
                                case (byte) EventCommand.ChangeItems:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Item Amount of [" + Data.Item[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "] to " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3);
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Give Player " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + " " + Data.Item[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "(s)");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 2)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Take " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + " " + Data.Item[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "(s) from Player.");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.RestoreHealth:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Restore Player HP");
                                    break;
                                }
                                case (byte) EventCommand.RestoreMana:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Restore Player MP");
                                    break;
                                }
                                case (byte) EventCommand.RestoreStamina:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Restore Player SP");
                                    break;
                                }
                                case (byte) EventCommand.LevelUp:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Level Up Player");
                                    break;
                                }
                                case (byte) EventCommand.ChangeLevel:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Level to " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1);
                                    break;
                                }
                                case (byte) EventCommand.ChangeSkills:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Teach Player Skill [" + Strings.Trim(Data.Skill[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name) + "]");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Remove Player Skill [" + Strings.Trim(Data.Skill[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name) + "]");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.ChangeJob:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Job to " + Strings.Trim(Data.Job[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name));
                                    break;
                                }
                                case (byte) EventCommand.ChangeSprite:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Sprite to " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1);
                                    break;
                                }
                                case (byte) EventCommand.ChangeSex:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Sex to Male.");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Sex to Female.");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.SetPlayerKillable:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player PK to No.");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player PK to Yes.");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.WarpPlayer:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Warp Player To Map: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Tile(" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + ") while retaining direction.");
                                    }
                                    else
                                    {
                                        switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4 - 1)
                                        {
                                            case (int) Direction.Up:
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Warp Player To Map: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Tile(" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + ") facing upward.");
                                                break;
                                            }
                                            case (int) Direction.Down:
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Warp Player To Map: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Tile(" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + ") facing downward.");
                                                break;
                                            }
                                            case (int) Direction.Left:
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Warp Player To Map: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Tile(" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + ") facing left.");
                                                break;
                                            }
                                            case (int) Direction.Right:
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Warp Player To Map: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Tile(" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + ") facing right.");
                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.SetMoveRoute:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 <= Data.MyMap.EventCount)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Move Route for Event #" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " [" + Data.MyMap.Event[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "]");
                                    }
                                    else
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Move Route for COULD NOT FIND EVENT!");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.PlayAnimation:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Play Animation " + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + " [" + Data.Animation[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "]" + " On Player");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 1)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Play Animation " + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + " [" + Data.Animation[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "]" + " On Event " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + " [" + Strings.Trim(Data.MyMap.Event[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3].Name) + "]");
                                    }
                                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 == 2)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Play Animation " + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + 1) + " [" + Data.Animation[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "]" + " On Tile (" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3 + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4 + ")");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.PlayBgm:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Play BGM [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1 + "]");
                                    break;
                                }
                                case (byte) EventCommand.FadeOutBgm:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Fadeout BGM");
                                    break;
                                }
                                case (byte) EventCommand.PlaySound:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Play Sound [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1 + "]");
                                    break;
                                }
                                case (byte) EventCommand.StopSound:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Stop Sound");
                                    break;
                                }
                                case (byte) EventCommand.OpenBank:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Open Bank");
                                    break;
                                }
                                case (byte) EventCommand.OpenShop:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Open Shop [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ". " + Data.Shop[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name + "]");
                                    break;
                                }
                                case (byte) EventCommand.SetAccessLevel:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Player Access [" + Editor_Event.Instance.cmbSetAccess.Items[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 - 1]);
                                    break;
                                }
                                case (byte) EventCommand.GiveExperience:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Give Player " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " Experience.");
                                    break;
                                }
                                case (byte) EventCommand.ShowChatBubble:
                                {
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1)
                                    {
                                        case (int) TargetType.Player:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Chat Bubble - " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20) + "... - On Player");
                                            break;
                                        }
                                        case (int) TargetType.Npc:
                                        {
                                            if (Data.MyMap.Npc[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2] <= 0)
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Chat Bubble - " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20) + "... - On Npc [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + 1).ToString() + ". ]");
                                            }
                                            else
                                            {
                                                Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Chat Bubble - " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20) + "... - On Npc [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + 1).ToString() + ". " + Data.Npc[Data.MyMap.Npc[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2]].Name + "]");
                                            }

                                            break;
                                        }
                                        case (int) TargetType.Event:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Chat Bubble - " + Strings.Mid(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1, 1, 20) + "... - On Event [" + (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2 + 1).ToString() + ". " + Data.MyMap.Event[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2].Name + "]");
                                            break;
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.Label:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Label: [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1) + "]");
                                    break;
                                }
                                case (byte) EventCommand.GoToLabel:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Jump to Label: [" + Strings.Trim(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Text1) + "]");
                                    break;
                                }
                                case (byte) EventCommand.SpawnNpc:
                                {
                                    if (Data.MyMap.Npc[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1] <= 0)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Spawn Npc: [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ". " + "]");
                                    }
                                    else
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Spawn Npc: [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ". " + Data.Npc[Data.MyMap.Npc[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1]].Name + "]");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.FadeIn:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Fade In");
                                    break;
                                }
                                case (byte) EventCommand.FadeOut:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Fade Out");
                                    break;
                                }
                                case (byte) EventCommand.FlashScreen:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Flash White");
                                    break;
                                }
                                case (byte) EventCommand.SetFog:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Fog [Fog: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + " Speed: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + " Opacity: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3.ToString() + "]");
                                    break;
                                }
                                case (byte) EventCommand.SetWeather:
                                {
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1)
                                    {
                                        case (int) WeatherType.None:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Weather [None]");
                                            break;
                                        }
                                        case (int) WeatherType.Rain:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Weather [Rain - Intensity: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + "]");
                                            break;
                                        }
                                        case (int) WeatherType.Snow:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Weather [Snow - Intensity: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + "]");
                                            break;
                                        }
                                        case (int) WeatherType.Sandstorm:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Weather [Sand Storm - Intensity: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + "]");
                                            break;
                                        }
                                        case (int) WeatherType.Storm:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Weather [Storm - Intensity: " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + "]");
                                            break;
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.SetScreenTint:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Set Map Tint RGBA [" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2.ToString() + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data3.ToString() + "," + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4.ToString() + "]");
                                    break;
                                }
                                case (byte) EventCommand.Wait:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Wait " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + " Ms");
                                    break;
                                }
                                case (byte) EventCommand.ShowPicture:
                                {
                                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2)
                                    {
                                        case 0:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Picture " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ": Pic=" + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2) + " Top Left, X: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4) + " Y: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data5));
                                            break;
                                        }
                                        case 1:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Picture " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ": Pic=" + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2) + " Center Screen, X: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4) + " Y: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data5));
                                            break;
                                        }
                                        case 2:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Picture " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ": Pic=" + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2) + " On Event, X: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4) + " Y: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data5));
                                            break;
                                        }
                                        case 3:
                                        {
                                            Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Show Picture " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString() + ": Pic=" + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data2) + " On Player, X: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data4) + " Y: " + Conversion.Str(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data5));
                                            break;
                                        }
                                    }

                                    break;
                                }
                                case (byte) EventCommand.HidePicture:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Hide Picture " + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1.ToString());
                                    break;
                                }
                                case (byte) EventCommand.WaitMovementCompletion:
                                {
                                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 <= Data.MyMap.EventCount)
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Wait for Event #" + TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1 + " [" + Strings.Trim(Data.MyMap.Event[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i].Data1].Name) + "] to complete move route.");
                                    }
                                    else
                                    {
                                        Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Wait for COULD NOT FIND EVENT to complete move route.");
                                    }

                                    break;
                                }
                                case (byte) EventCommand.HoldPlayer:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Hold Player [Do not allow player to move.]");
                                    break;
                                }
                                case (byte) EventCommand.ReleasePlayer:
                                {
                                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@>" + "Release Player [Allow player to turn and move again.]");
                                    break;
                                }

                                default:
                                {
                                    // Ghost
                                    X = X - 1;
                                    if (X == -1)
                                    {
                                        EventList = new Type.EventList[1];
                                    }
                                    else
                                    {
                                        Array.Resize(ref EventList, X + 1);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }

                if (curlist > 1)
                {
                    X = X + 1;
                    Array.Resize(ref EventList, X + 1);
                    EventList[X].CommandList = curlist;
                    EventList[X].CommandNum = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount;
                    Editor_Event.Instance.lstCommands.Items.Add(indent + "@> ");
                    curlist = TmpEvent.Pages[CurPageNum].CommandList[curlist].ParentList;
                    goto newlist;
                }
            }

            Editor_Event.Instance.lstCommands.Items.Add(indent + "@> ");

            var z = default(int);
            X = 0;
            var loopTo1 = Editor_Event.Instance.lstCommands.Items.Count;
            for (i = 0; i < loopTo1; i++)
            {
                if (X > z)
                    z = X;
            }
        }

        public static void AddCommand(int Index)
        {
            int curlist;
            var i = default(int);
            var X = default(int);
            int curslot;
            int p;
            Type.CommandList oldCommandList;

            if (Editor_Event.Instance.lstCommands.SelectedIndex == -1 || EventList == null)
            {
                curlist = 0;
            }
            else
            {
                curlist = EventList[Editor_Event.Instance.lstCommands.SelectedIndex].CommandList;
            }

            TmpEvent.Pages[CurPageNum].CommandListCount += 1;
            Array.Resize(ref TmpEvent.Pages[CurPageNum].CommandList, TmpEvent.Pages[CurPageNum].CommandListCount);
            TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount += 1;
            p = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount;
            Array.Resize(ref TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands, p);

            if (Editor_Event.Instance.lstCommands.SelectedIndex + 1 == Editor_Event.Instance.lstCommands.Items.Count)
            {
                curslot = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount - 1;
            }
            else
            {
                oldCommandList = TmpEvent.Pages[CurPageNum].CommandList[curlist];
                TmpEvent.Pages[CurPageNum].CommandList[curlist].ParentList = oldCommandList.ParentList;

                var loopTo = p;
                for (i = 0; i < loopTo; i++)
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i] = oldCommandList.Commands[i];

                i = EventList[Editor_Event.Instance.lstCommands.SelectedIndex].CommandNum;
                if (i <= TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount)
                {
                    var loopTo1 = i;
                    for (X = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount; X < loopTo1; X++)
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[X + 1] = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[X];

                    curslot = EventList[Editor_Event.Instance.lstCommands.SelectedIndex].CommandNum;
                }
                else
                {
                    curslot = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount;
                }
            }

            switch (Index)
            {
                case (int) EventCommand.AddText:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtAddText_Text.Text;
                    if (Editor_Event.Instance.optAddText_Player.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optAddText_Map.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else if (Editor_Event.Instance.optAddText_Global.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                    }

                    break;
                }
                case (int) EventCommand.ConditionalBranch:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandListCount += 1;
                    Array.Resize(ref TmpEvent.Pages[CurPageNum].CommandList, TmpEvent.Pages[CurPageNum].CommandListCount);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.CommandList = TmpEvent.Pages[CurPageNum].CommandListCount;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.ElseCommandList = TmpEvent.Pages[CurPageNum].CommandListCount;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.CommandList].ParentList = curlist;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.ElseCommandList].ParentList = curlist;

                    if (Editor_Event.Instance.optCondition0.Checked == true)
                        X = 0;
                    if (Editor_Event.Instance.optCondition1.Checked == true)
                        X = 1;
                    if (Editor_Event.Instance.optCondition2.Checked == true)
                        X = 2;
                    if (Editor_Event.Instance.optCondition3.Checked == true)
                        X = 3;
                    if (Editor_Event.Instance.optCondition4.Checked == true)
                        X = 4;
                    if (Editor_Event.Instance.optCondition5.Checked == true)
                        X = 5;
                    if (Editor_Event.Instance.optCondition6.Checked == true)
                        X = 6;
                    if (Editor_Event.Instance.optCondition8.Checked == true)
                        X = 8;
                    if (Editor_Event.Instance.optCondition9.Checked == true)
                        X = 9;

                    switch (X)
                    {
                        case 0: // Player Var
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 0;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_PlayerVarIndex.SelectedIndex;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_PlayerVarCompare.SelectedIndex;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data3 = (int) Math.Round(Editor_Event.Instance.nudCondition_PlayerVarCondition.Value);
                            break;
                        }
                        case 1: // Player Switch
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 1;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_PlayerSwitch.SelectedIndex;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondtion_PlayerSwitchCondition.SelectedIndex;
                            break;
                        }
                        case 2: // Has Item
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 2;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_HasItem.SelectedIndex;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = (int) Math.Round(Editor_Event.Instance.nudCondition_HasItem.Value);
                            break;
                        }
                        case 3: // Job Is
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 3;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_JobIs.SelectedIndex;
                            break;
                        }
                        case 4: // Learnt Skill
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 4;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_LearntSkill.SelectedIndex;
                            break;
                        }
                        case 5: // Level Is
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 5;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = (int) Math.Round(Editor_Event.Instance.nudCondition_LevelAmount.Value);
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_LevelCompare.SelectedIndex;
                            break;
                        }
                        case 6: // Self Switch
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 6;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_SelfSwitch.SelectedIndex;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_SelfSwitchCondition.SelectedIndex;
                            break;
                        }
                        case 7:
                        {
                            break;
                        }

                        case 8: // Gender
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 8;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_Gender.SelectedIndex;
                            break;
                        }
                        case 9: // Time
                        {
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 9;
                            TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_Time.SelectedIndex;
                            break;
                        }
                    }

                    break;
                }

                case (int) EventCommand.ShowText:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    string tmptxt = "";
                    // TextArea has no Lines property; split Text manually to mimic previous behavior
                    var rawText = Editor_Event.Instance.txtShowText.Text ?? string.Empty;
                    var splitLines = rawText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    for (i = 0; i < splitLines.Length; i++)
                    {
                        tmptxt += splitLines[i];
                    }
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = tmptxt;
                    break;
                }

                case (int) EventCommand.ShowChoices:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtChoicePrompt.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text2 = Editor_Event.Instance.txtChoices1.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text3 = Editor_Event.Instance.txtChoices2.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text4 = Editor_Event.Instance.txtChoices3.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text5 = Editor_Event.Instance.txtChoices4.Text;
                    TmpEvent.Pages[CurPageNum].CommandListCount += 3;
                    Array.Resize(ref TmpEvent.Pages[CurPageNum].CommandList, TmpEvent.Pages[CurPageNum].CommandListCount + 1);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = TmpEvent.Pages[CurPageNum].CommandListCount - 3;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = TmpEvent.Pages[CurPageNum].CommandListCount - 2;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = TmpEvent.Pages[CurPageNum].CommandListCount - 1;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = TmpEvent.Pages[CurPageNum].CommandListCount;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandListCount - 3].ParentList = curlist;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandListCount - 2].ParentList = curlist;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandListCount - 1].ParentList = curlist;
                    TmpEvent.Pages[CurPageNum].CommandList[TmpEvent.Pages[CurPageNum].CommandListCount].ParentList = curlist;
                    break;
                }

                case (int) EventCommand.ModifyVariable:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbVariable.SelectedIndex;

                    if (Editor_Event.Instance.optVariableAction0.Checked == true)
                        i = 0;
                    if (Editor_Event.Instance.optVariableAction1.Checked == true)
                        i = 1;
                    if (Editor_Event.Instance.optVariableAction2.Checked == true)
                        i = 2;
                    if (Editor_Event.Instance.optVariableAction3.Checked == true)
                        i = 3;

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = i;
                    if (i == 3)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData3.Value);
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudVariableData4.Value);
                    }
                    else if (i == 0)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData0.Value);
                    }
                    else if (i == 1)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData1.Value);
                    }
                    else if (i == 2)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData2.Value);
                    }

                    break;
                }

                case (int) EventCommand.ModifySwitch:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSwitch.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbPlayerSwitchSet.SelectedIndex;
                    break;
                }

                case (int) EventCommand.ModifySelfSwitch:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetSelfSwitch.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbSetSelfSwitchTo.SelectedIndex;
                    break;
                }

                case (int) EventCommand.ExitEventProcess:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.ChangeItems:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeItemIndex.SelectedIndex;
                    if (Editor_Event.Instance.optChangeItemSet.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optChangeItemAdd.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else if (Editor_Event.Instance.optChangeItemRemove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                    }

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudChangeItemsAmount.Value);
                    break;
                }

                case (int) EventCommand.RestoreHealth:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.RestoreMana:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.RestoreStamina:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.LevelUp:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.ChangeLevel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudChangeLevel.Value);
                    break;
                }

                case (int) EventCommand.ChangeSkills:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeSkills.SelectedIndex;
                    if (Editor_Event.Instance.optChangeSkillsAdd.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optChangeSkillsRemove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }

                    break;
                }

                case (int) EventCommand.ChangeJob:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeJob.SelectedIndex;
                    break;
                }

                case (int) EventCommand.ChangeSprite:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudChangeSprite.Value);
                    break;
                }

                case (int) EventCommand.ChangeSex:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    if (Editor_Event.Instance.optChangeSexMale.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Sex.Male;
                    }
                    else if (Editor_Event.Instance.optChangeSexFemale.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Sex.Female;
                    }

                    break;
                }

                case (int) EventCommand.SetPlayerKillable:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetPK.SelectedIndex;
                    break;
                }

                case (int) EventCommand.WarpPlayer:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudWPMap.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudWPX.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudWPY.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = Editor_Event.Instance.cmbWarpPlayerDir.SelectedIndex;
                    break;
                }

                case (int) EventCommand.SetMoveRoute:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = ListOfEvents[Editor_Event.Instance.cmbEvent.SelectedIndex];
                    if (Editor_Event.Instance.chkIgnoreMove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }

                    if (Editor_Event.Instance.chkRepeatRoute.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = 1;
                    }
                    else
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = 0;
                    }

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRouteCount = TempMoveRouteCount;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRoute = TempMoveRoute;
                    break;
                }

                case (int) EventCommand.PlayAnimation:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbPlayAnim.SelectedIndex;
                    if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 0)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 1)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = Editor_Event.Instance.cmbPlayAnimEvent.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 2 == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudPlayAnimTileX.Value);
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudPlayAnimTileY.Value);
                    }

                    break;
                }

                case (int) EventCommand.PlayBgm:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Sound.MusicCache[Editor_Event.Instance.cmbPlayBGM.SelectedIndex];
                    break;
                }

                case (int) EventCommand.FadeOutBgm:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.PlaySound:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Sound.SoundCache[Editor_Event.Instance.cmbPlaySound.SelectedIndex];
                    break;
                }

                case (int) EventCommand.StopSound:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.OpenBank:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.OpenShop:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbOpenShop.SelectedIndex;
                    break;
                }

                case (int) EventCommand.SetAccessLevel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetAccess.SelectedIndex + 1;
                    break;
                }

                case (int) EventCommand.GiveExperience:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudGiveExp.Value);
                    break;
                }

                case (int) EventCommand.ShowChatBubble:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtChatbubbleText.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChatBubbleTargetType.SelectedIndex + 1;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbChatBubbleTarget.SelectedIndex;
                    break;
                }

                case (int) EventCommand.Label:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtLabelName.Text;
                    break;
                }

                case (int) EventCommand.GoToLabel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtGoToLabel.Text;
                    break;
                }

                case (int) EventCommand.SpawnNpc:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSpawnNpc.SelectedIndex;
                    break;
                }

                case (int) EventCommand.FadeIn:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.FadeOut:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.FlashScreen:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.SetFog:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudFogData0.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudFogData1.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudFogData2.Value);
                    break;
                }

                case (int) EventCommand.SetWeather:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.CmbWeather.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudWeatherIntensity.Value);
                    break;
                }

                case (int) EventCommand.SetScreenTint:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudMapTintData0.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudMapTintData1.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudMapTintData2.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudMapTintData3.Value);
                    break;
                }

                case (int) EventCommand.Wait:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudWaitAmount.Value);
                    break;
                }

                case (int) EventCommand.ShowPicture:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudShowPicture.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbPicLoc.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudPicOffsetX.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudPicOffsetY.Value);
                    break;
                }

                case (int) EventCommand.HidePicture:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.WaitMovementCompletion:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = ListOfEvents[Editor_Event.Instance.cmbMoveWait.SelectedIndex];
                    break;
                }

                case (int) EventCommand.HoldPlayer:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }

                case (int) EventCommand.ReleasePlayer:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index = (byte) Index;
                    break;
                }
            }

            EventListCommands();
        }

        public static void EditEventCommand()
        {
            int i;
            var X = default(int);
            int curlist;
            int curslot;

            i = Editor_Event.Instance.lstCommands.SelectedIndex + 1;
            if (i == -1)
                return;

            if (i > Information.UBound(EventList))
                return;

            Editor_Event.Instance.fraConditionalBranch.Visible = false;

            curlist = EventList[i].CommandList;
            curslot = EventList[i].CommandNum;

            if (curlist > TmpEvent.Pages[CurPageNum].CommandListCount)
                return;

            if (TmpEvent.Pages[CurPageNum].CommandList == null)
                return;

            if (curslot > TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount)
                return;

            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index)
            {
                case (byte) EventCommand.AddText:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtAddText_Text.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    // Editor_Event.Instance.scrlAddText_Color.Value = tmpEvent.Pages(curPageNum).CommandList(curlist).Commands(curslot).Data1
                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2)
                    {
                        case 0:
                        {
                            Editor_Event.Instance.optAddText_Player.Checked = true;
                            break;
                        }
                        case 1:
                        {
                            Editor_Event.Instance.optAddText_Map.Checked = true;
                            break;
                        }
                        case 2:
                        {
                            Editor_Event.Instance.optAddText_Global.Checked = true;
                            break;
                        }
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraAddText.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ConditionalBranch:
                {
                    IsEdit = true;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraConditionalBranch.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    Editor_Event.Instance.ClearConditionFrame();

                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition)
                    {
                        case 0:
                        {
                            Editor_Event.Instance.optCondition0.Checked = true;
                            break;
                        }
                        case 1:
                        {
                            Editor_Event.Instance.optCondition1.Checked = true;
                            break;
                        }
                        case 2:
                        {
                            Editor_Event.Instance.optCondition2.Checked = true;
                            break;
                        }
                        case 3:
                        {
                            Editor_Event.Instance.optCondition3.Checked = true;
                            break;
                        }
                        case 4:
                        {
                            Editor_Event.Instance.optCondition4.Checked = true;
                            break;
                        }
                        case 5:
                        {
                            Editor_Event.Instance.optCondition5.Checked = true;
                            break;
                        }
                        case 6:
                        {
                            Editor_Event.Instance.optCondition6.Checked = true;
                            break;
                        }
                        case 7:
                        {
                            break;
                        }

                        case 8:
                        {
                            Editor_Event.Instance.optCondition8.Checked = true;
                            break;
                        }
                        case 9:
                        {
                            Editor_Event.Instance.optCondition9.Checked = true;
                            break;
                        }
                    }

                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition)
                    {
                        case 0:
                        {
                            Editor_Event.Instance.cmbCondition_PlayerVarIndex.Enabled = true;
                            Editor_Event.Instance.cmbCondition_PlayerVarCompare.Enabled = true;
                            Editor_Event.Instance.nudCondition_PlayerVarCondition.Enabled = true;
                            Editor_Event.Instance.cmbCondition_PlayerVarIndex.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            Editor_Event.Instance.cmbCondition_PlayerVarCompare.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2;
                            Editor_Event.Instance.nudCondition_PlayerVarCondition.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data3;
                            break;
                        }
                        case 1:
                        {
                            Editor_Event.Instance.cmbCondition_PlayerSwitch.Enabled = true;
                            Editor_Event.Instance.cmbCondtion_PlayerSwitchCondition.Enabled = true;
                            Editor_Event.Instance.cmbCondition_PlayerSwitch.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            Editor_Event.Instance.cmbCondtion_PlayerSwitchCondition.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2;
                            break;
                        }
                        case 2:
                        {
                            Editor_Event.Instance.cmbCondition_HasItem.Enabled = true;
                            Editor_Event.Instance.nudCondition_HasItem.Enabled = true;
                            Editor_Event.Instance.cmbCondition_HasItem.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            Editor_Event.Instance.nudCondition_HasItem.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2;
                            break;
                        }
                        case 3:
                        {
                            Editor_Event.Instance.cmbCondition_JobIs.Enabled = true;
                            Editor_Event.Instance.cmbCondition_JobIs.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            break;
                        }
                        case 4:
                        {
                            Editor_Event.Instance.cmbCondition_LearntSkill.Enabled = true;
                            Editor_Event.Instance.cmbCondition_LearntSkill.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            break;
                        }
                        case 5:
                        {
                            Editor_Event.Instance.cmbCondition_LevelCompare.Enabled = true;
                            Editor_Event.Instance.nudCondition_LevelAmount.Enabled = true;
                            Editor_Event.Instance.nudCondition_LevelAmount.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            Editor_Event.Instance.cmbCondition_LevelCompare.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2;
                            break;
                        }
                        case 6:
                        {
                            Editor_Event.Instance.cmbCondition_SelfSwitch.Enabled = true;
                            Editor_Event.Instance.cmbCondition_SelfSwitchCondition.Enabled = true;
                            Editor_Event.Instance.cmbCondition_SelfSwitch.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            Editor_Event.Instance.cmbCondition_SelfSwitchCondition.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2;
                            break;
                        }
                        case 7:
                        {
                            break;
                        }

                        case 8:
                        {
                            Editor_Event.Instance.cmbCondition_Gender.Enabled = true;
                            Editor_Event.Instance.cmbCondition_Gender.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            break;
                        }
                        case 9:
                        {
                            Editor_Event.Instance.cmbCondition_Time.Enabled = true;
                            Editor_Event.Instance.cmbCondition_Time.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1;
                            break;
                        }
                    }

                    break;
                }
                case (byte) EventCommand.ShowText:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtShowText.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraShowText.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ShowChoices:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtChoicePrompt.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    Editor_Event.Instance.txtChoices1.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text2;
                    Editor_Event.Instance.txtChoices2.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text3;
                    Editor_Event.Instance.txtChoices3.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text4;
                    Editor_Event.Instance.txtChoices4.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text5;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraShowChoices.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ModifyVariable:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbVariable.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2)
                    {
                        case 0:
                        {
                            Editor_Event.Instance.optVariableAction0.Checked = true;
                            Editor_Event.Instance.nudVariableData0.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                            break;
                        }
                        case 1:
                        {
                            Editor_Event.Instance.optVariableAction1.Checked = true;
                            Editor_Event.Instance.nudVariableData1.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                            break;
                        }
                        case 2:
                        {
                            Editor_Event.Instance.optVariableAction2.Checked = true;
                            Editor_Event.Instance.nudVariableData2.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                            break;
                        }
                        case 3:
                        {
                            Editor_Event.Instance.optVariableAction3.Checked = true;
                            Editor_Event.Instance.nudVariableData3.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                            Editor_Event.Instance.nudVariableData4.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4;
                            break;
                        }
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlayerVariable.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ModifySwitch:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbSwitch.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.cmbPlayerSwitchSet.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlayerSwitch.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ModifySelfSwitch:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbSetSelfSwitch.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.cmbSetSelfSwitchTo.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSetSelfSwitch.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeItems:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbChangeItemIndex.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 0)
                    {
                        Editor_Event.Instance.optChangeItemSet.Checked = true;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 1)
                    {
                        Editor_Event.Instance.optChangeItemAdd.Checked = true;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 2)
                    {
                        Editor_Event.Instance.optChangeItemRemove.Checked = true;
                    }

                    Editor_Event.Instance.nudChangeItemsAmount.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeItems.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeLevel:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudChangeLevel.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeLevel.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeSkills:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbChangeSkills.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 0)
                    {
                        Editor_Event.Instance.optChangeSkillsAdd.Checked = true;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 1)
                    {
                        Editor_Event.Instance.optChangeSkillsRemove.Checked = true;
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeSkills.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeJob:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbChangeJob.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeJob.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeSprite:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudChangeSprite.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeSprite.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ChangeSex:
                {
                    IsEdit = true;
                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 == 0)
                    {
                        Editor_Event.Instance.optChangeSexMale.Checked = true;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 == 1)
                    {
                        Editor_Event.Instance.optChangeSexFemale.Checked = true;
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangeGender.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetPlayerKillable:
                {
                    IsEdit = true;

                    Editor_Event.Instance.cmbSetPK.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraChangePK.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.WarpPlayer:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudWPMap.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.nudWPX.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.nudWPY.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    Editor_Event.Instance.cmbWarpPlayerDir.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlayerWarp.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetMoveRoute:
                {
                    IsEdit = true;
                    Editor_Event.Instance.fraMoveRoute.Visible = true;
                    Editor_Event.Instance.lstMoveRoute.Items.Clear();
                    ListOfEvents = new int[Data.MyMap.EventCount];
                    ListOfEvents[0] = EditorEvent;
                    var loopTo = Data.MyMap.EventCount;
                    for (i = 0; i < loopTo; i++)
                    {
                        if (i != EditorEvent)
                        {
                            Editor_Event.Instance.cmbEvent.Items.Add(Strings.Trim(Data.MyMap.Event[i].Name));
                            X = X + 1;
                            ListOfEvents[X] = i;
                            if (i == TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1)
                                Editor_Event.Instance.cmbEvent.SelectedIndex = X;
                        }
                    }

                    IsMoveRouteCommand = true;
                    Editor_Event.Instance.chkIgnoreMove.Checked = Conversions.ToBoolean(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2);
                    Editor_Event.Instance.chkRepeatRoute.Checked = Conversions.ToBoolean(TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3);
                    TempMoveRouteCount = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRouteCount;
                    TempMoveRoute = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRoute;
                    var loopTo1 = TempMoveRouteCount;
                    for (i = 0; i < loopTo1; i++)
                    {
                        switch (TempMoveRoute[i].Index)
                        {
                            case 1:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Up");
                                break;
                            }
                            case 2:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Down");
                                break;
                            }
                            case 3:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Left");
                                break;
                            }
                            case 4:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Right");
                                break;
                            }
                            case 5:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Randomly");
                                break;
                            }
                            case 6:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Towards Player");
                                break;
                            }
                            case 7:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Move Away From Player");
                                break;
                            }
                            case 8:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Step Forward");
                                break;
                            }
                            case 9:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Step Back");
                                break;
                            }
                            case 10:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Wait 100ms");
                                break;
                            }
                            case 11:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Wait 500ms");
                                break;
                            }
                            case 12:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Wait 1000ms");
                                break;
                            }
                            case 13:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Up");
                                break;
                            }
                            case 14:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Down");
                                break;
                            }
                            case 15:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Left");
                                break;
                            }
                            case 16:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Right");
                                break;
                            }
                            case 17:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn 90 Degrees To the Right");
                                break;
                            }
                            case 18:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn 90 Degrees To the Left");
                                break;
                            }
                            case 19:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Around 180 Degrees");
                                break;
                            }
                            case 20:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Randomly");
                                break;
                            }
                            case 21:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Towards Player");
                                break;
                            }
                            case 22:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Away from Player");
                                break;
                            }
                            case 23:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed 8x Slower");
                                break;
                            }
                            case 24:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed 4x Slower");
                                break;
                            }
                            case 25:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed 2x Slower");
                                break;
                            }
                            case 26:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed to Normal");
                                break;
                            }
                            case 27:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed 2x Faster");
                                break;
                            }
                            case 28:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Speed 4x Faster");
                                break;
                            }
                            case 29:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Frequency Lowest");
                                break;
                            }
                            case 30:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Frequency Lower");
                                break;
                            }
                            case 31:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Frequency Normal");
                                break;
                            }
                            case 32:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Frequency Higher");
                                break;
                            }
                            case 33:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Frequency Highest");
                                break;
                            }
                            case 34:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn On Walking Animation");
                                break;
                            }
                            case 35:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Off Walking Animation");
                                break;
                            }
                            case 36:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn On Fixed Direction");
                                break;
                            }
                            case 37:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Off Fixed Direction");
                                break;
                            }
                            case 38:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn On Walk Through");
                                break;
                            }
                            case 39:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Turn Off Walk Through");
                                break;
                            }
                            case 40:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Position Below Characters");
                                break;
                            }
                            case 41:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Position Same as Characters");
                                break;
                            }
                            case 42:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Position Above Characters");
                                break;
                            }
                            case 43:
                            {
                                Editor_Event.Instance.lstMoveRoute.Items.Add("Set Graphic");
                                break;
                            }
                        }
                    }

                    Editor_Event.Instance.fraMoveRoute.Visible = true;
                    Editor_Event.Instance.fraDialogue.Visible = false;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.PlayAnimation:
                {
                    IsEdit = true;
                    Editor_Event.Instance.lblPlayAnimX.Visible = false;
                    Editor_Event.Instance.lblPlayAnimY.Visible = false;
                    Editor_Event.Instance.nudPlayAnimTileX.Visible = false;
                    Editor_Event.Instance.nudPlayAnimTileY.Visible = false;
                    Editor_Event.Instance.cmbPlayAnimEvent.Visible = false;
                    Editor_Event.Instance.cmbPlayAnim.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.cmbPlayAnimEvent.Items.Clear();
                    var loopTo2 = Data.MyMap.EventCount;
                    for (i = 0; i < loopTo2; i++)
                        Editor_Event.Instance.cmbPlayAnimEvent.Items.Add(i + 1 + ". " + Data.MyMap.Event[i].Name);
                    Editor_Event.Instance.cmbPlayAnimEvent.SelectedIndex = 0;
                    if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 0)
                    {
                        Editor_Event.Instance.cmbAnimTargetType.SelectedIndex = 0;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 1)
                    {
                        Editor_Event.Instance.cmbAnimTargetType.SelectedIndex = 1;
                        Editor_Event.Instance.cmbPlayAnimEvent.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    }
                    else if (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 == 2)
                    {
                        Editor_Event.Instance.cmbAnimTargetType.SelectedIndex = 2;
                        Editor_Event.Instance.nudPlayAnimTileX.MaxValue = Data.MyMap.MaxX;
                        Editor_Event.Instance.nudPlayAnimTileY.MaxValue = Data.MyMap.MaxY;
                        Editor_Event.Instance.nudPlayAnimTileX.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                        Editor_Event.Instance.nudPlayAnimTileY.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4;
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlayAnimation.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }

                case (byte) EventCommand.PlayBgm:
                {
                    IsEdit = true;
                    var loopTo3 = Information.UBound(Sound.MusicCache);
                    for (i = 0; i < loopTo3; i++)
                    {
                        if ((Sound.MusicCache[i] ?? "") == (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 ?? ""))
                        {
                            Editor_Event.Instance.cmbPlayBGM.SelectedIndex = i;
                        }
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlayBGM.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.PlaySound:
                {
                    IsEdit = true;
                    var loopTo4 = Information.UBound(Sound.SoundCache);
                    for (i = 0; i < loopTo4; i++)
                    {
                        if ((Sound.SoundCache[i] ?? "") == (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 ?? ""))
                        {
                            Editor_Event.Instance.cmbPlaySound.SelectedIndex = i;
                        }
                    }

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraPlaySound.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.OpenShop:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbOpenShop.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraOpenShop.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetAccessLevel:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbSetAccess.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 - 1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSetAccess.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.GiveExperience:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudGiveExp.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraGiveExp.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ShowChatBubble:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtChatbubbleText.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    Editor_Event.Instance.cmbChatBubbleTargetType.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 - 1;
                    if (Editor_Event.Instance.cmbChatBubbleTarget.Items.Count > -1)
                        Editor_Event.Instance.cmbChatBubbleTarget.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;

                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraShowChatBubble.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.Label:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtLabelName.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraCreateLabel.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.GoToLabel:
                {
                    IsEdit = true;
                    Editor_Event.Instance.txtGoToLabel.Text = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraGoToLabel.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SpawnNpc:
                {
                    IsEdit = true;
                    Editor_Event.Instance.cmbSpawnNpc.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSpawnNpc.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetFog:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudFogData0.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.nudFogData1.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.nudFogData2.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSetFog.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetWeather:
                {
                    IsEdit = true;
                    Editor_Event.Instance.CmbWeather.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.nudWeatherIntensity.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSetWeather.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.SetScreenTint:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudMapTintData0.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.nudMapTintData1.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;
                    Editor_Event.Instance.nudMapTintData2.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    Editor_Event.Instance.nudMapTintData3.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraMapTint.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.Wait:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudWaitAmount.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraSetWait.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    break;
                }
                case (byte) EventCommand.ShowPicture:
                {
                    IsEdit = true;
                    Editor_Event.Instance.nudShowPicture.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1;

                    Editor_Event.Instance.cmbPicLoc.SelectedIndex = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2;

                    Editor_Event.Instance.nudPicOffsetX.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3;
                    Editor_Event.Instance.nudPicOffsetY.Value = TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraShowPic.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    Map.DrawPicture();
                    break;
                }
                case (byte) EventCommand.WaitMovementCompletion:
                {
                    IsEdit = true;
                    Editor_Event.Instance.fraDialogue.Visible = true;
                    Editor_Event.Instance.fraMoveRouteWait.Visible = true;
                    Editor_Event.Instance.fraCommands.Visible = false;
                    Editor_Event.Instance.cmbMoveWait.Items.Clear();
                    ListOfEvents = new int[Data.MyMap.EventCount];
                    ListOfEvents[0] = EditorEvent;
                    Editor_Event.Instance.cmbMoveWait.Items.Add("This Event");
                    Editor_Event.Instance.cmbMoveWait.SelectedIndex = 0;
                    var loopTo5 = Data.MyMap.EventCount;
                    for (i = 0; i < loopTo5; i++)
                    {
                        if (i != EditorEvent)
                        {
                            Editor_Event.Instance.cmbMoveWait.Items.Add(Strings.Trim(Data.MyMap.Event[i].Name));
                            X = X + 1;
                            ListOfEvents[X] = i;
                            if (i == TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1)
                                Editor_Event.Instance.cmbMoveWait.SelectedIndex = X;
                        }
                    }

                    break;
                }
            }
        }

        public static void DeleteEventCommand()
        {
            int i;
            int curlist;
            int curslot;
            int p;
            Type.CommandList oldCommandList;

            i = Editor_Event.Instance.lstCommands.SelectedIndex;
            if (i == -1)
                return;

            if (i > Information.UBound(EventList))
                return;

            curlist = EventList[i].CommandList;
            curslot = EventList[i].CommandNum;

            if (curlist > TmpEvent.Pages[CurPageNum].CommandListCount)
                return;

            if (TmpEvent.Pages[CurPageNum].CommandList == null)
                return;

            if (curslot >= TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount)
                return;

            if (TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount != i + 1)
            {
                TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount--;
                p = TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount;
                oldCommandList = TmpEvent.Pages[CurPageNum].CommandList[curlist];

                if (p <= 0)
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands = new Type.EventCommand[1];
                }
                else
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands = new Type.EventCommand[p];
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].ParentList = oldCommandList.ParentList;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount = p;

                    // Move all commands down by 1  
                    for (i = Editor_Event.Instance.lstCommands.SelectedIndex + 1; i <= p; i++)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[i - 1] = oldCommandList.Commands[i];
                    }
                }
            }
            else
            {
                // If we are deleting the last command in the list, set only the last command  
                TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount--;
                Array.Resize(ref TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands, TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount);
            }

            EventListCommands();
        }

        public static void ClearEventCommands()
        {
            TmpEvent.Pages[CurPageNum].CommandList = new Type.CommandList[1];
            TmpEvent.Pages[CurPageNum].CommandListCount = 0;
            EventListCommands();
        }

        public static void EditCommand()
        {
            int i;
            int curlist;
            int curslot;

            i = Editor_Event.Instance.lstCommands.SelectedIndex;
            if (i == -1)
                return;

            if (i > Information.UBound(EventList))
                return;

            curlist = EventList[i].CommandList;
            curslot = EventList[i].CommandNum;

            if (curlist > TmpEvent.Pages[CurPageNum].CommandListCount)
                return;

            if (curslot > TmpEvent.Pages[CurPageNum].CommandList[curlist].CommandCount)
                return;

            switch (TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Index)
            {
                case (byte) EventCommand.AddText:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtAddText_Text.Text;
                    // tmpEvent.Pages(curPageNum).CommandList(curlist).Commands(curslot).Data1 = Editor_Event.Instance.scrlAddText_Color.Value
                    if (Editor_Event.Instance.optAddText_Player.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optAddText_Map.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else if (Editor_Event.Instance.optAddText_Global.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                    }

                    break;
                }
                case (byte) EventCommand.ConditionalBranch:
                {
                    if (Editor_Event.Instance.optCondition0.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 0;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_PlayerVarIndex.SelectedIndex;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_PlayerVarCompare.SelectedIndex;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data3 = (int) Math.Round(Editor_Event.Instance.nudCondition_PlayerVarCondition.Value);
                    }
                    else if (Editor_Event.Instance.optCondition1.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 1;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_PlayerSwitch.SelectedIndex;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondtion_PlayerSwitchCondition.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition2.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 2;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_HasItem.SelectedIndex;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = (int) Math.Round(Editor_Event.Instance.nudCondition_HasItem.Value);
                    }
                    else if (Editor_Event.Instance.optCondition3.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 3;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_JobIs.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition4.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 4;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_LearntSkill.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition5.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 5;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = (int) Math.Round(Editor_Event.Instance.nudCondition_LevelAmount.Value);
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_LevelCompare.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition6.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 6;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_SelfSwitch.SelectedIndex;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data2 = Editor_Event.Instance.cmbCondition_SelfSwitchCondition.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition8.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 8;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_Gender.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.optCondition9.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Condition = 9;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].ConditionalBranch.Data1 = Editor_Event.Instance.cmbCondition_Time.SelectedIndex;
                    }

                    break;
                }
                case (byte) EventCommand.ShowText:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtShowText.Text;
                    break;
                }
                case (byte) EventCommand.ShowChoices:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtChoicePrompt.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text2 = Editor_Event.Instance.txtChoices1.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text3 = Editor_Event.Instance.txtChoices2.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text4 = Editor_Event.Instance.txtChoices3.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text5 = Editor_Event.Instance.txtChoices4.Text;
                    break;
                }
                case (byte) EventCommand.ModifyVariable:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbVariable.SelectedIndex;
                    if (Editor_Event.Instance.optVariableAction0.Checked == true)
                        i = 0;
                    if (Editor_Event.Instance.optVariableAction1.Checked == true)
                        i = 1;
                    if (Editor_Event.Instance.optVariableAction2.Checked == true)
                        i = 2;
                    if (Editor_Event.Instance.optVariableAction3.Checked == true)
                        i = 3;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = i;
                    if (i == 0)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData0.Value);
                    }
                    else if (i == 1)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData1.Value);
                    }
                    else if (i == 2)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData2.Value);
                    }
                    else if (i == 3)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudVariableData3.Value);
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudVariableData4.Value);
                    }

                    break;
                }
                case (byte) EventCommand.ModifySwitch:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSwitch.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbPlayerSwitchSet.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.ModifySelfSwitch:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetSelfSwitch.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbSetSelfSwitchTo.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.ChangeItems:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeItemIndex.SelectedIndex;
                    if (Editor_Event.Instance.optChangeItemSet.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optChangeItemAdd.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else if (Editor_Event.Instance.optChangeItemRemove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                    }

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudChangeItemsAmount.Value);
                    break;
                }
                case (byte) EventCommand.ChangeLevel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudChangeLevel.Value);
                    break;
                }
                case (byte) EventCommand.ChangeSkills:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeSkills.SelectedIndex;
                    if (Editor_Event.Instance.optChangeSkillsAdd.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.optChangeSkillsRemove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }

                    break;
                }
                case (byte) EventCommand.ChangeJob:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChangeJob.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.ChangeSprite:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudChangeSprite.Value);
                    break;
                }
                case (byte) EventCommand.ChangeSex:
                {
                    if (Editor_Event.Instance.optChangeSexMale.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = 0;
                    }
                    else if (Editor_Event.Instance.optChangeSexFemale.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = 1;
                    }

                    break;
                }
                case (byte) EventCommand.SetPlayerKillable:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetPK.SelectedIndex;
                    break;
                }

                case (byte) EventCommand.WarpPlayer:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudWPMap.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudWPX.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudWPY.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = Editor_Event.Instance.cmbWarpPlayerDir.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.SetMoveRoute:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = ListOfEvents[Editor_Event.Instance.cmbEvent.SelectedIndex];
                    if (Editor_Event.Instance.chkIgnoreMove.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                    }
                    else
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }

                    if (Editor_Event.Instance.chkRepeatRoute.Checked == true)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = 1;
                    }
                    else
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = 0;
                    }

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRouteCount = TempMoveRouteCount;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].MoveRoute = TempMoveRoute;
                    break;
                }
                case (byte) EventCommand.PlayAnimation:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbPlayAnim.SelectedIndex;
                    if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 0)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 0;
                    }
                    else if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 1)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 1;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = Editor_Event.Instance.cmbPlayAnimEvent.SelectedIndex;
                    }
                    else if (Editor_Event.Instance.cmbAnimTargetType.SelectedIndex == 2)
                    {
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = 2;
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudPlayAnimTileX.Value);
                        TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudPlayAnimTileY.Value);
                    }

                    break;
                }
                case (byte) EventCommand.PlayBgm:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Sound.MusicCache[Editor_Event.Instance.cmbPlayBGM.SelectedIndex];
                    break;
                }
                case (byte) EventCommand.PlaySound:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Sound.SoundCache[Editor_Event.Instance.cmbPlaySound.SelectedIndex];
                    break;
                }
                case (byte) EventCommand.OpenShop:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbOpenShop.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.SetAccessLevel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSetAccess.SelectedIndex + 1;
                    break;
                }
                case (byte) EventCommand.GiveExperience:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudGiveExp.Value);
                    break;
                }
                case (byte) EventCommand.ShowChatBubble:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtChatbubbleText.Text;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbChatBubbleTargetType.SelectedIndex + 1;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbChatBubbleTarget.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.Label:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtLabelName.Text;
                    break;
                }
                case (byte) EventCommand.GoToLabel:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Text1 = Editor_Event.Instance.txtGoToLabel.Text;
                    break;
                }
                case (byte) EventCommand.SpawnNpc:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.cmbSpawnNpc.SelectedIndex;
                    break;
                }
                case (byte) EventCommand.SetFog:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudFogData0.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudFogData1.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudFogData2.Value);
                    break;
                }
                case (byte) EventCommand.SetWeather:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = Editor_Event.Instance.CmbWeather.SelectedIndex;
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudWeatherIntensity.Value);
                    break;
                }
                case (byte) EventCommand.SetScreenTint:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudMapTintData0.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = (int) Math.Round(Editor_Event.Instance.nudMapTintData1.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudMapTintData2.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudMapTintData3.Value);
                    break;
                }
                case (byte) EventCommand.Wait:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudWaitAmount.Value);
                    break;
                }
                case (byte) EventCommand.ShowPicture:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = (int) Math.Round(Editor_Event.Instance.nudShowPicture.Value);

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data2 = Editor_Event.Instance.cmbPicLoc.SelectedIndex;

                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data3 = (int) Math.Round(Editor_Event.Instance.nudPicOffsetX.Value);
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data4 = (int) Math.Round(Editor_Event.Instance.nudPicOffsetY.Value);
                    break;
                }
                case (byte) EventCommand.WaitMovementCompletion:
                {
                    TmpEvent.Pages[CurPageNum].CommandList[curlist].Commands[curslot].Data1 = ListOfEvents[Editor_Event.Instance.cmbMoveWait.SelectedIndex];
                    break;
                }
            }

            EventListCommands();
        }

        #endregion

        #region Incoming Packets

        public static void Packet_SpawnEvent(ReadOnlyMemory<byte> data)
        {
            int id;
            var buffer = new PacketReader(data);

            GameState.CurrentEvents = buffer.ReadInt32();
            Array.Resize(ref Data.MapEvents, GameState.CurrentEvents);

            for (int i = 0; i < GameState.CurrentEvents; i++)
            {
                id = buffer.ReadInt32();

                if (id >= GameState.CurrentEvents)
                    break;

                ref var withBlock = ref Data.MapEvents[id];
                withBlock.Name = buffer.ReadString();
                withBlock.Dir = buffer.ReadInt32();
                withBlock.ShowDir = withBlock.Dir;
                withBlock.GraphicType = buffer.ReadByte();
                withBlock.Graphic = buffer.ReadInt32();
                withBlock.GraphicX = buffer.ReadInt32();
                withBlock.GraphicX2 = buffer.ReadInt32();
                withBlock.GraphicY = buffer.ReadInt32();
                withBlock.GraphicY2 = buffer.ReadInt32();
                withBlock.MovementSpeed = buffer.ReadInt32();
                withBlock.Moving = 0;
                withBlock.X = buffer.ReadInt32();
                withBlock.Y = buffer.ReadInt32();
                withBlock.Position = buffer.ReadByte();
                withBlock.Visible = buffer.ReadBoolean();
                withBlock.WalkAnim = buffer.ReadInt32();
                withBlock.DirFix = buffer.ReadInt32();
                withBlock.WalkThrough = buffer.ReadInt32();
                withBlock.ShowName = buffer.ReadInt32();
            }
        }

        public static void Packet_EventMove(ReadOnlyMemory<byte> data)
        {
            int id;
            int x;
            int y;
            int dir;
            int showDir;
            int movementSpeed;
            var buffer = new PacketReader(data);

            id = buffer.ReadInt32();
            x = buffer.ReadInt32();
            y = buffer.ReadInt32();
            dir = buffer.ReadInt32();
            showDir = buffer.ReadInt32();
            movementSpeed = buffer.ReadInt32();

            if (id > GameState.CurrentEvents)
                return;

            {
                ref var withBlock = ref Data.MapEvents[id];
                withBlock.X = x;
                withBlock.Y = y;
                withBlock.Dir = dir;
                withBlock.Moving = 1;
                withBlock.ShowDir = showDir;
                withBlock.MovementSpeed = movementSpeed;
            }
        }

        public static void Packet_EventDir(ReadOnlyMemory<byte> data)
        {
            int i;
            byte dir;
            var buffer = new PacketReader(data);
            i = buffer.ReadInt32();
            dir = (byte) buffer.ReadInt32();

            if (i > GameState.CurrentEvents)
                return;

            {
                ref var withBlock = ref Data.MapEvents[i];
                withBlock.Dir = dir;
                withBlock.ShowDir = dir;
                withBlock.Moving = 0;
            }
        }

        public static void Packet_SwitchesAndVariables(ReadOnlyMemory<byte> data)
        {
            int i;
            var buffer = new PacketReader(data);

            for (i = 0; i < Constant.MaxSwitches; i++)
                Switches[i] = buffer.ReadString();

            for (i = 0; i < Constant.MaxVariables; i++)
                Variables[i] = buffer.ReadString();
        }

        public static void Packet_MapEventData(ReadOnlyMemory<byte> data)
        {
            int i;
            int x;
            int y;
            int z;
            int w;
            var buffer = new PacketReader(data);

            Data.MyMap.EventCount = buffer.ReadInt32();

            if (Data.MyMap.EventCount > 0)
            {
                Data.MyMap.Event = new Type.Event[Data.MyMap.EventCount];
                var loopTo = Data.MyMap.EventCount;
                for (i = 0; i < loopTo; i++)
                {
                    {
                        ref var withBlock = ref Data.MyMap.Event[i];
                        withBlock.Name = buffer.ReadString();
                        withBlock.Globals = buffer.ReadByte();
                        withBlock.X = buffer.ReadInt32();
                        withBlock.Y = buffer.ReadInt32();
                        withBlock.PageCount = buffer.ReadInt32();
                    }

                    if (Data.MyMap.Event[i].PageCount > 0)
                    {
                        Data.MyMap.Event[i].Pages = new Type.EventPage[Data.MyMap.Event[i].PageCount];
                        var loopTo1 = Data.MyMap.Event[i].PageCount;
                        for (x = 0; x < loopTo1; x++)
                        {
                            {
                                ref var withBlock1 = ref Data.MyMap.Event[i].Pages[x];
                                withBlock1.ChkVariable = buffer.ReadInt32();
                                withBlock1.VariableIndex = buffer.ReadInt32();
                                withBlock1.VariableCondition = buffer.ReadInt32();
                                withBlock1.VariableCompare = buffer.ReadInt32();
                                withBlock1.ChkSwitch = buffer.ReadInt32();
                                withBlock1.SwitchIndex = buffer.ReadInt32();
                                withBlock1.SwitchCompare = buffer.ReadInt32();
                                withBlock1.ChkHasItem = buffer.ReadInt32();
                                withBlock1.HasItemIndex = buffer.ReadInt32();
                                withBlock1.HasItemAmount = buffer.ReadInt32();
                                withBlock1.ChkSelfSwitch = buffer.ReadInt32();
                                withBlock1.SelfSwitchIndex = buffer.ReadInt32();
                                withBlock1.SelfSwitchCompare = buffer.ReadInt32();
                                withBlock1.GraphicType = buffer.ReadByte();
                                withBlock1.Graphic = buffer.ReadInt32();
                                withBlock1.GraphicX = buffer.ReadInt32();
                                withBlock1.GraphicY = buffer.ReadInt32();
                                withBlock1.GraphicX2 = buffer.ReadInt32();
                                withBlock1.GraphicY2 = buffer.ReadInt32();

                                withBlock1.MoveType = buffer.ReadByte();
                                withBlock1.MoveSpeed = buffer.ReadByte();
                                withBlock1.MoveFreq = buffer.ReadByte();
                                withBlock1.MoveRouteCount = buffer.ReadInt32();
                                withBlock1.IgnoreMoveRoute = buffer.ReadInt32();
                                withBlock1.RepeatMoveRoute = buffer.ReadInt32();

                                if (withBlock1.MoveRouteCount > 0)
                                {
                                    Data.MyMap.Event[i].Pages[x].MoveRoute = new Type.MoveRoute[withBlock1.MoveRouteCount];
                                    var loopTo2 = withBlock1.MoveRouteCount;
                                    for (y = 0; y < loopTo2; y++)
                                    {
                                        withBlock1.MoveRoute[y].Index = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data1 = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data2 = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data3 = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data4 = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data5 = buffer.ReadInt32();
                                        withBlock1.MoveRoute[y].Data6 = buffer.ReadInt32();
                                    }
                                }

                                withBlock1.WalkAnim = buffer.ReadInt32();
                                withBlock1.DirFix = buffer.ReadInt32();
                                withBlock1.WalkThrough = buffer.ReadInt32();
                                withBlock1.ShowName = buffer.ReadInt32();
                                withBlock1.Trigger = buffer.ReadByte();
                                withBlock1.CommandListCount = buffer.ReadInt32();
                                withBlock1.Position = buffer.ReadByte();
                            }

                            if (Data.MyMap.Event[i].Pages[x].CommandListCount > 0)
                            {
                                Data.MyMap.Event[i].Pages[x].CommandList = new Type.CommandList[Data.MyMap.Event[i].Pages[x].CommandListCount];
                                var loopTo3 = Data.MyMap.Event[i].Pages[x].CommandListCount;
                                for (y = 0; y < loopTo3; y++)
                                {
                                    Data.MyMap.Event[i].Pages[x].CommandList[y].CommandCount = buffer.ReadInt32();
                                    Data.MyMap.Event[i].Pages[x].CommandList[y].ParentList = buffer.ReadInt32();
                                    if (Data.MyMap.Event[i].Pages[x].CommandList[y].CommandCount > 0)
                                    {
                                        Data.MyMap.Event[i].Pages[x].CommandList[y].Commands = new Type.EventCommand[Data.MyMap.Event[i].Pages[x].CommandList[y].CommandCount];
                                        var loopTo4 = Data.MyMap.Event[i].Pages[x].CommandList[y].CommandCount;
                                        for (z = 0; z < loopTo4; z++)
                                        {
                                            {
                                                ref var withBlock2 = ref Data.MyMap.Event[i].Pages[x].CommandList[y].Commands[z];
                                                withBlock2.Index = buffer.ReadInt32();
                                                withBlock2.Text1 = buffer.ReadString();
                                                withBlock2.Text2 = buffer.ReadString();
                                                withBlock2.Text3 = buffer.ReadString();
                                                withBlock2.Text4 = buffer.ReadString();
                                                withBlock2.Text5 = buffer.ReadString();
                                                withBlock2.Data1 = buffer.ReadInt32();
                                                withBlock2.Data2 = buffer.ReadInt32();
                                                withBlock2.Data3 = buffer.ReadInt32();
                                                withBlock2.Data4 = buffer.ReadInt32();
                                                withBlock2.Data5 = buffer.ReadInt32();
                                                withBlock2.Data6 = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.CommandList = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.Condition = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.Data1 = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.Data2 = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.Data3 = buffer.ReadInt32();
                                                withBlock2.ConditionalBranch.ElseCommandList = buffer.ReadInt32();
                                                withBlock2.MoveRouteCount = buffer.ReadInt32();

                                                if (withBlock2.MoveRouteCount > 0)
                                                {
                                                    withBlock2.MoveRoute = new Type.MoveRoute[withBlock2.MoveRouteCount];
                                                    var loopTo5 = withBlock2.MoveRouteCount;
                                                    for (w = 0; w < loopTo5; w++)
                                                    {
                                                        withBlock2.MoveRoute[w].Index = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data1 = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data2 = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data3 = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data4 = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data5 = buffer.ReadInt32();
                                                        withBlock2.MoveRoute[w].Data6 = buffer.ReadInt32();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Packet_EventChat(ReadOnlyMemory<byte> data)
        {
            int i;
            int choices;
            var buffer = new PacketReader(data);
            EventReplyId = buffer.ReadInt32();
            EventReplyPage = buffer.ReadInt32();
            EventChatFace = buffer.ReadInt32();
            EventText = buffer.ReadString();
            if (string.IsNullOrEmpty(EventText))
                EventText = " ";
            EventChat = true;
            ShowEventLbl = true;
            choices = buffer.ReadInt32();
            InEvent = true;
            for (i = 0; i < Constant.MaxEventChoices; i++)
            {
                EventChoices[i] = "";
                EventChoiceVisible[i] = false;
            }

            EventChatType = 0;
            if (choices == 0)
            {
            }
            else
            {
                EventChatType = 1;
                var loopTo = choices;
                for (i = 0; i < loopTo; i++)
                {
                    EventChoices[i] = buffer.ReadString();
                    EventChoiceVisible[i] = true;
                }
            }

            AnotherChat = buffer.ReadInt32();
        }

        public static void Packet_EventStart(ReadOnlyMemory<byte> data)
        {
            InEvent = true;
        }

        public static void Packet_EventEnd(ReadOnlyMemory<byte> data)
        {
            InEvent = false;
        }

        public static void Packet_Picture(ReadOnlyMemory<byte> data)
        {
            var buffer = new PacketReader(data);
            int picIndex;
            int spriteType;
            int xOffset;
            int yOffset;
            int eventid;

            eventid = buffer.ReadInt32();
            picIndex = buffer.ReadByte();

            if (picIndex == 0)
            {
                Picture.Index = 0;
                Picture.EventId = 0;
                Picture.SpriteType = 0;
                Picture.XOffset = 0;
                Picture.YOffset = 0;
                return;
            }

            spriteType = buffer.ReadByte();
            xOffset = buffer.ReadByte();
            yOffset = buffer.ReadByte();

            Picture.Index = (byte) picIndex;
            Picture.EventId = eventid;
            Picture.SpriteType = (byte) spriteType;
            Picture.XOffset = (byte) xOffset;
            Picture.YOffset = (byte) yOffset;
        }

        public static void Packet_HidePicture(ReadOnlyMemory<byte> data)
        {
            var buffer = new PacketReader(data);

            Picture = default;
        }

        public static void Packet_HoldPlayer(ReadOnlyMemory<byte> data)
        {
            var buffer = new PacketReader(data);
            if (buffer.ReadInt32() == 0)
            {
                HoldPlayer = true;
            }
            else
            {
                HoldPlayer = false;
            }
        }

        public static void Packet_PlayBGM(ReadOnlyMemory<byte> data)
        {
            string music;
            var buffer = new PacketReader(data);

            music = buffer.ReadString();
            Data.MyMap.Music = music;
        }

        public static void Packet_FadeOutBGM(ReadOnlyMemory<byte> data)
        {
            Sound.CurrentMusic = "";
            Sound.FadeOutSwitch = true;
        }

        public static void Packet_PlaySound(ReadOnlyMemory<byte> data)
        {
            string sound;
            var buffer = new PacketReader(data);
            int x;
            int y;

            sound = buffer.ReadString();
            x = buffer.ReadInt32();
            y = buffer.ReadInt32();

            Sound.PlaySound(sound, x, y);
        }

        public static void Packet_StopSound(ReadOnlyMemory<byte> data)
        {
            Sound.StopSound();
        }

        public static void Packet_SpecialEffect(ReadOnlyMemory<byte> data)
        {
            int effectType;
            var buffer = new PacketReader(data);
            effectType = buffer.ReadInt32();

            switch (effectType)
            {
                case GameState.EffectTypeFadein:
                {
                    GameState.UseFade = true;
                    GameState.FadeType = 1;
                    GameState.FadeAmount = 0;
                    break;
                }
                case GameState.EffectTypeFadeout:
                {
                    GameState.UseFade = true;
                    GameState.FadeType = 0;
                    GameState.FadeAmount = 255;
                    break;
                }
                case GameState.EffectTypeFlash:
                {
                    GameState.FlashTimer = General.GetTickCount() + 150;
                    break;
                }
                case GameState.EffectTypeFog:
                {
                    GameState.CurrentFog = buffer.ReadInt32();
                    GameState.CurrentFogSpeed = buffer.ReadInt32();
                    GameState.CurrentFogOpacity = buffer.ReadInt32();
                    break;
                }
                case GameState.EffectTypeWeather:
                {
                    GameState.CurrentWeather = buffer.ReadInt32();
                    GameState.CurrentWeatherIntensity = buffer.ReadInt32();
                    break;
                }
                case GameState.EffectTypeTint:
                {
                    Data.MyMap.MapTint = true;
                    GameState.CurrentTintR = buffer.ReadInt32();
                    GameState.CurrentTintG = buffer.ReadInt32();
                    GameState.CurrentTintB = buffer.ReadInt32();
                    GameState.CurrentTintA = buffer.ReadInt32();
                    break;
                }
            }
        }

        #endregion

        #region Outgoing Packets

        public static void RequestSwitchesAndVariables()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CRequestSwitchesAndVariables);

            Network.Send(packetWriter);
        }

        public static void SendSwitchesAndVariables()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CSwitchesAndVariables);

            for (var i = 0; i < Constant.MaxSwitches; i++)
            {
                packetWriter.WriteString(Switches[i]);
            }

            for (var i = 0; i < Constant.MaxVariables; i++)
            {
                packetWriter.WriteString(Variables[i]);
            }

            Network.Send(packetWriter);
        }

        #endregion

        #region Misc

        public static void ProcessEventMovement(int id)
        {
            if (GameState.MyEditorType == EditorType.Map)
                return;

            if (id >= Data.MyMap.EventCount)
                return;

            if (id >= Data.MapEvents.Length)
                return;

            if (Data.MapEvents[id].Moving == 1)
            {
                // Check if completed walking over to the next tile
                if (Data.MapEvents[id].Moving > 0)
                {
                    if (Data.MapEvents[id].Dir == (int) Direction.Right | Data.MapEvents[id].Dir == (int) Direction.Down)
                    {
                        switch (Data.MapEvents[(int) id].Dir)
                        {
                            case (int) Direction.Up:
                            {
                                Data.MapEvents[(int) id].Y = (byte) (Data.MapEvents[(int) id].Y - 1);

                                break;
                            }
                            case (int) Direction.Down:
                            {
                                Data.MapEvents[(int) id].Y = (byte) (Data.MapEvents[(int) id].Y + 1);
                                break;
                            }
                            case (int) Direction.Left:
                            {
                                Data.MapEvents[(int) id].X = (byte) (Data.MapEvents[(int) id].X - 1);
                                break;
                            }
                            case (int) Direction.Right:
                            {
                                Data.MyMapNpc[(int) id].X = (byte) (Data.MyMapNpc[(int) id].X + 1);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static object GetColorString(int color)
        {
            object getColorString = default;

            switch (color)
            {
                case 0:
                {
                    getColorString = "Black";
                    break;
                }
                case 1:
                {
                    getColorString = "Blue";
                    break;
                }
                case 2:
                {
                    getColorString = "Green";
                    break;
                }
                case 3:
                {
                    getColorString = "Cyan";
                    break;
                }
                case 4:
                {
                    getColorString = "Red";
                    break;
                }
                case 5:
                {
                    getColorString = "Magenta";
                    break;
                }
                case 6:
                {
                    getColorString = "Brown";
                    break;
                }
                case 7:
                {
                    getColorString = "Grey";
                    break;
                }
                case 8:
                {
                    getColorString = "Dark Grey";
                    break;
                }
                case 9:
                {
                    getColorString = "Bright Blue";
                    break;
                }
                case 10:
                {
                    getColorString = "Bright Green";
                    break;
                }
                case 11:
                {
                    getColorString = "Bright Cyan";
                    break;
                }
                case 12:
                {
                    getColorString = "Bright Red";
                    break;
                }
                case 13:
                {
                    getColorString = "Pink";
                    break;
                }
                case 14:
                {
                    getColorString = "Yellow";
                    break;
                }
                case 15:
                {
                    getColorString = "White";
                    break;
                }

                default:
                {
                    getColorString = "Black";
                    break;
                }
            }

            return getColorString;
        }

        public static void ClearEventChat()
        {
            int i;

            if (AnotherChat == 1)
            {
                for (i = 0; i < Constant.MaxEventChoices; i++)
                    EventChoiceVisible[i] = false;
                EventText = "";
                EventChatType = 1;
                EventChatTimer = General.GetTickCount() + 100;
            }
            else if (AnotherChat == 2)
            {
                for (i = 0; i < Constant.MaxEventChoices; i++)
                    EventChoiceVisible[i] = false;
                EventText = "";
                EventChatType = 1;
                EventChatTimer = General.GetTickCount() + 100;
            }
            else
            {
                EventChat = false;
            }
        }

        #endregion
    }
}