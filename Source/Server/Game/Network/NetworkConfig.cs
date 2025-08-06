using Core;
using Microsoft.VisualBasic;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Global.Command;

namespace Server;

public class NetworkConfig
{
    public static bool IsLoggedIn(int index)
    {
        return Data.Account[index].Login.Length > 0;
    }

    public static bool IsPlaying(int index)
    {
        return Data.TempPlayer[index].InGame;
    }

    public static bool IsMultiLogin(int index, string login)
    {
        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (i != index)
            {
                if (login != "" && Data.Account[i].Login.ToLower() != "")
                {
                    if (Data.Account[i].Login.ToLower() != login)
                    {
                        if (PlayerService.Instance.ClientIp(i) == PlayerService.Instance.ClientIp(index))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public static async Task LoadAccount(GameSession session, string login, byte slot)
    {
        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (login != "" && Data.Account[i].Login.ToLower() != "")
            {
                if (Data.Account[i].Login.ToLower() == login)
                {
                    if (session.Id != i)
                    {
                        await Player.LeftGame(i);
                        break;
                    }
                }
            }
        }

        Database.LoadCharacter(session.Id, slot);
        Database.LoadBank(session.Id);

        // Check if character data has been created
        if (Strings.Len(Data.Player[session.Id].Name) > 0)
        {
            // we have a char!                        
            Player.HandleUseChar(session);
        }
        else
        {
            NetworkSend.AlertMsg(session, SystemMessage.DatabaseError, Menu.CharacterSelect);
        }
    }

    public static void SendDataToAll(ReadOnlySpan<byte> data, int head)
    {
        PlayerService.Instance.SendDataToAll(data, head);
    }

    public static void SendDataToAllBut(int index, ReadOnlySpan<byte> data, int head)
    {
        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (i != index)
            {
                PlayerService.Instance.SendDataTo(i, data, head);
            }
        }
    }

    public static void SendDataToMapBut(int index, int mapNum, ReadOnlySpan<byte> data, int head)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (!IsPlaying(playerId) || playerId == index)
            {
                continue;
            }
            
            if (GetPlayerMap(playerId) == mapNum)
            {
                PlayerService.Instance.SendDataTo(playerId, data, head);
            }
        }
    }
    
    public static void SendDataToMapBut(int index, int mapNum, byte[] bytes)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (!IsPlaying(playerId) || playerId == index)
            {
                continue;
            }
            
            if (GetPlayerMap(playerId) == mapNum)
            {
                PlayerService.Instance.SendDataTo(playerId, bytes);
            }
        }
    }

    public static void SendDataToMap(int mapNum, ReadOnlySpan<byte> data, int head)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (!IsPlaying(playerId))
            {
                continue;
            }

            if (GetPlayerMap(playerId) == mapNum)
            {
                PlayerService.Instance.SendDataTo(playerId, data, head);
            }
        }
    }

    public static void SendDataToMap(int mapNum, byte[] bytes)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (!IsPlaying(playerId) || GetPlayerMap(playerId) != mapNum)
            {
                continue;
            }

            PlayerService.Instance.SendDataTo(playerId, bytes);
        }
    }

    public static void SendDataTo(int index, ReadOnlySpan<byte> data, int head)
    {
        PlayerService.Instance.SendDataTo(index, data, head);
    }
}