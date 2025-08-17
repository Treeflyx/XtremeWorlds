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
                if (GetPlayerName(i).ToUpperInvariant() == name.ToUpperInvariant())
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
            string firstLetter = word.Substring(0, 1).ToLowerInvariant();

            if (firstLetter == "$")
            {
                checkGrammar = word.Substring(1);
                return checkGrammar;
            }

            // Simple vowel check for English grammar
            string vowels = "aeiou";
            bool startsWithVowel = vowels.Contains(firstLetter);
            bool isCaps = caps != 0;

            if (startsWithVowel)
                checkGrammar = (isCaps ? "An " : "an ") + word;
            else
                checkGrammar = (isCaps ? "A " : "a ") + word;
            return checkGrammar;
        }

    }
}