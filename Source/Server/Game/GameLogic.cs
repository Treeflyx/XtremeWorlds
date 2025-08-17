using Core;
using Core.Globals;
using Server.Game;
using static Core.Globals.Command;

namespace Server
{

    public class GameLogic
    {
        public static int GetTotalMapPlayers(int mapNum)
        {
            int getTotalMapPlayers = default;
            int n;
            n = 0;

            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (GetPlayerMap(i) == mapNum)
                {
                    n = n + 1;
                }
            }

            getTotalMapPlayers = n;
            return getTotalMapPlayers;
        }

        public static int GetNpcMaxVital(double npcNum, Vital vital)
        {
            int getNpcMaxVital = default;
            // Prevent subscript out of range
            if (npcNum < 0 | npcNum > Core.Globals.Constant.MaxNpcs)
                return getNpcMaxVital;

            switch (vital)
            {
                case Vital.Health:
                    {
                        getNpcMaxVital = Data.Npc[(int)npcNum].Hp;
                        break;
                    }
                case Vital.Stamina:
                    {
                        getNpcMaxVital = (int)Data.Npc[(int)npcNum].Stat[(byte)Stat.Intelligence] * 2;
                        break;
                    }
            }

            return getNpcMaxVital;

        }

        public static int FindPlayer(string name)
        {
            int findPlayer = default;

            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                // Trim and convert both names to uppercase for case-insensitive comparison
                if (Strings.UCase(GetPlayerName(i)) == Strings.UCase(name))
                {
                    findPlayer = i;
                    return findPlayer;
                }
            }

            findPlayer = -1;
            return findPlayer;
        }

        public static string CheckGrammar(string word, byte caps = 0)
        {
            string checkGrammar = default;
            string firstLetter;

            firstLetter = Strings.LCase(Strings.Left(word, 1));

            if (firstLetter == "$")
            {
                checkGrammar = Strings.Mid(word, 2, Strings.Len(word) - 1);
                return checkGrammar;
            }

            if (LikeOperator.LikeString(firstLetter, "*[aeiou]*", CompareMethod.Binary))
            {
                if (Conversions.ToBoolean(caps))
                    checkGrammar = "An " + word;
                else
                    checkGrammar = "an " + word;
            }
            else if (Conversions.ToBoolean(caps))
                checkGrammar = "A " + word;
            else
                checkGrammar = "a " + word;
            return checkGrammar;
        }

    }
}