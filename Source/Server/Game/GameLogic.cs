using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Server.Game;
using static Core.Global.Command;

namespace Server
{

    public class GameLogic
    {
        public static int GetTotalMapPlayers(int mapNum)
        {
            int getTotalMapPlayersRet = default;
            int n;
            n = 0;

            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (GetPlayerMap(i) == mapNum)
                {
                    n = n + 1;
                }
            }

            getTotalMapPlayersRet = n;
            return getTotalMapPlayersRet;
        }

        public static int GetNpcMaxVital(double npcNum, Core.Vital vital)
        {
            int getNpcMaxVitalRet = default;
            // Prevent subscript out of range
            if (npcNum < 0 | npcNum > Core.Constant.MaxNpcs)
                return getNpcMaxVitalRet;

            switch (vital)
            {
                case Core.Vital.Health:
                    {
                        getNpcMaxVitalRet = Core.Data.Npc[(int)npcNum].Hp;
                        break;
                    }
                case Core.Vital.Stamina:
                    {
                        getNpcMaxVitalRet = (int)Core.Data.Npc[(int)npcNum].Stat[(byte)Core.Stat.Intelligence] * 2;
                        break;
                    }
            }

            return getNpcMaxVitalRet;

        }

        public static int FindPlayer(string name)
        {
            int findPlayerRet = default;

            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                // Trim and convert both names to uppercase for case-insensitive comparison
                if (Strings.UCase(GetPlayerName(i)) == Strings.UCase(name))
                {
                    findPlayerRet = i;
                    return findPlayerRet;
                }
            }

            findPlayerRet = -1;
            return findPlayerRet;
        }

        public static string CheckGrammar(string word, byte caps = 0)
        {
            string checkGrammarRet = default;
            string firstLetter;

            firstLetter = Strings.LCase(Strings.Left(word, 1));

            if (firstLetter == "$")
            {
                checkGrammarRet = Strings.Mid(word, 2, Strings.Len(word) - 1);
                return checkGrammarRet;
            }

            if (LikeOperator.LikeString(firstLetter, "*[aeiou]*", CompareMethod.Binary))
            {
                if (Conversions.ToBoolean(caps))
                    checkGrammarRet = "An " + word;
                else
                    checkGrammarRet = "an " + word;
            }
            else if (Conversions.ToBoolean(caps))
                checkGrammarRet = "A " + word;
            else
                checkGrammarRet = "a " + word;
            return checkGrammarRet;
        }

    }
}