using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Npc
    {
        public static void ProcessMovement(double mapNpcNum)
        {
            if (mapNpcNum < 0 || mapNpcNum >= Constant.MaxMapNpcs)
            {
                return;
            }

            // Check if Npc is walking, and if so process moving them over
            if (Data.MyMapNpc[(int)mapNpcNum].Moving == (byte)MovementState.Walking)
            {
                int x = Core.Data.MyMapNpc[(int)mapNpcNum].X;
                int y = Core.Data.MyMapNpc[(int)mapNpcNum].Y;

                switch (Data.MyMapNpc[(int)mapNpcNum].Dir)
                {
                    case (int)Direction.Up:
                        y -= 1;
                        break;
                    case (int)Direction.Down:
                        y += 1;
                        break;
                    case (int)Direction.Left:
                        x -= 1;
                        break;
                    case (int)Direction.Right:
                        x += 1;
                        break;
                }

                if (x < 0 || y < 0 || x >= (Data.MyMap.MaxX - 1) * 32 || y >= (Data.MyMap.MaxY - 1) * 32)
                {
                    return;
                }

                if (x % 32 == 0 && y % 32 == 0)
                {
                    return;
                }


                Core.Data.MyMapNpc[(int)mapNpcNum].X = x;
                Core.Data.MyMapNpc[(int)mapNpcNum].Y = y;

            }
        }

    }
}
