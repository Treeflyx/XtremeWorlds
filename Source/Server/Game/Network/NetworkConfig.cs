using Core;
using Core.Globals;
using Server.Game;
using Server.Game.Net;
using static Core.Globals.Command;

namespace Server;

public static class NetworkConfig
{
    public static bool IsLoggedIn(int index)
    {
        return Data.Account[index].Login.Length > 0;
    }

    public static bool IsPlaying(int index)
    {
        return Data.TempPlayer[index].InGame;
    }

    public static bool IsMultiLogin(int playerId, string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            return false;
        }

        foreach (var otherPlayerId in PlayerService.Instance.PlayerIds)
        {
            if (otherPlayerId == playerId)
            {
                continue;
            }

            if (!Data.Account[otherPlayerId].Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                PlayerService.Instance.ClientIp(otherPlayerId) == PlayerService.Instance.ClientIp(playerId))
            {
                return true;
            }
        }

        return false;
    }

    public static async Task LoadAccount(GameSession session, string login, byte slot)
    {
        if (!string.IsNullOrEmpty(login))
        {
            foreach (var otherPlayerId in PlayerService.Instance.PlayerIds)
            {
                if (session.Id == otherPlayerId || !Data.Account[otherPlayerId].Login.Equals(login, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                await Player.LeftGame(otherPlayerId);
                break;
            }
        }

        Database.LoadCharacter(session.Id, slot);
        Database.LoadBank(session.Id);

        // Check if character data has been created
        if (Data.Player[session.Id].Name.Length > 0)
        {
            // we have a char!
            Player.HandleUseChar(session);
        }
        else
        {
            NetworkSend.AlertMsg(session, SystemMessage.DatabaseError, Menu.CharacterSelect);
        }
    }

    public static void SendDataToMapBut(int excludePlayerId, int mapNum, byte[] bytes)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (!IsPlaying(playerId) || playerId == excludePlayerId)
            {
                continue;
            }

            if (GetPlayerMap(playerId) == mapNum)
            {
                PlayerService.Instance.SendDataTo(playerId, bytes);
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
}