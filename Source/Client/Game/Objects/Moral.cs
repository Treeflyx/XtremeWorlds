using Client.Net;
using Core;
using Core.Globals;
using Microsoft.VisualBasic.CompilerServices;
using Type = Core.Globals.Type;

namespace Client
{

    public class Moral
    {
        #region Database

        public static void ClearMoral(int index)
        {
            Data.Moral[index] = default;

            Data.Moral[index].Name = "";
            GameState.MoralLoaded[index] = 0;
        }

        public static void ClearMorals()
        {
            int i;

            Data.Moral = new Type.Moral[(Constant.MaxMorals)];

            for (i = 0; i < Constant.MaxMorals; i++)
                ClearMoral(i);
        }

        public static void StreamMoral(int moralNum)
        {
            if (moralNum >= 0 & string.IsNullOrEmpty(Data.Moral[moralNum].Name) && GameState.MoralLoaded[moralNum] == 0)
            {
                GameState.MoralLoaded[moralNum] = 1;
                Sender.SendRequestMoral(moralNum);
            }
        }

        #endregion

        #region Incoming Packets

        #endregion
    }
}