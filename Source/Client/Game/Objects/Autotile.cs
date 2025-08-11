using Core;
using Core.Globals;
using Microsoft.VisualBasic.CompilerServices;
using Type = Core.Globals.Type;

namespace Client
{

    public class Autotile
    {
        public static void ClearAutotiles()
        {
            int x;
            int y;
            int i;

            Data.Autotile = new Type.Autotile[(Data.MyMap.MaxX), (Data.MyMap.MaxY)];

            var loopTo = (int)Data.MyMap.MaxX;
            for (x = 0; x < loopTo; x++)
            {
                var loopTo1 = (int)Data.MyMap.MaxY;
                for (y = 0; y < loopTo1; y++)
                {
                    int layerCount = System.Enum.GetValues(typeof(MapLayer)).Length;
                    Data.Autotile[x, y].Layer = new Type.QuarterTile[layerCount];
                    for (i = 0; i < layerCount; i++)
                    {
                        Data.Autotile[x, y].Layer[i].SrcX = new int[5];
                        Data.Autotile[x, y].Layer[i].SrcY = new int[5];
                        Data.Autotile[x, y].Layer[i].Tile = new Type.Point[5];
                    }
                }
            }
        }

        // \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        // All of this code is for auto tiles and the math behind generating them.
        // \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private static void PlaceAutotile(int layerNum, int x, int y, byte tileQuarter, string autoTileLetter)
        {
            int layerCount = System.Enum.GetValues(typeof(MapLayer)).Length;
            if (layerNum > layerCount)
            {
                layerNum = layerNum - (layerCount);
                {
                    ref var withBlock = ref Data.Autotile[x, y].ExLayer[layerNum].Tile[tileQuarter];
                    switch (autoTileLetter ?? "")
                    {
                        case "a":
                            {
                                withBlock.X = Type.AutoIn[1].X;
                                withBlock.Y = Type.AutoIn[1].Y;
                                break;
                            }
                        case "b":
                            {
                                withBlock.X = Type.AutoIn[2].X;
                                withBlock.Y = Type.AutoIn[2].Y;
                                break;
                            }
                        case "c":
                            {
                                withBlock.X = Type.AutoIn[3].X;
                                withBlock.Y = Type.AutoIn[3].Y;
                                break;
                            }
                        case "d":
                            {
                                withBlock.X = Type.AutoIn[4].X;
                                withBlock.Y = Type.AutoIn[4].Y;
                                break;
                            }
                        case "e":
                            {
                                withBlock.X = Type.AutoNw[1].X;
                                withBlock.Y = Type.AutoNw[1].Y;
                                break;
                            }
                        case "f":
                            {
                                withBlock.X = Type.AutoNw[2].X;
                                withBlock.Y = Type.AutoNw[2].Y;
                                break;
                            }
                        case "g":
                            {
                                withBlock.X = Type.AutoNw[3].X;
                                withBlock.Y = Type.AutoNw[3].Y;
                                break;
                            }
                        case "h":
                            {
                                withBlock.X = Type.AutoNw[4].X;
                                withBlock.Y = Type.AutoNw[4].Y;
                                break;
                            }
                        case "i":
                            {
                                withBlock.X = Type.AutoNe[1].X;
                                withBlock.Y = Type.AutoNe[1].Y;
                                break;
                            }
                        case "j":
                            {
                                withBlock.X = Type.AutoNe[2].X;
                                withBlock.Y = Type.AutoNe[2].Y;
                                break;
                            }
                        case "k":
                            {
                                withBlock.X = Type.AutoNe[3].X;
                                withBlock.Y = Type.AutoNe[3].Y;
                                break;
                            }
                        case "l":
                            {
                                withBlock.X = Type.AutoNe[4].X;
                                withBlock.Y = Type.AutoNe[4].Y;
                                break;
                            }
                        case "m":
                            {
                                withBlock.X = Type.AutoSw[1].X;
                                withBlock.Y = Type.AutoSw[1].Y;
                                break;
                            }
                        case "n":
                            {
                                withBlock.X = Type.AutoSw[2].X;
                                withBlock.Y = Type.AutoSw[2].Y;
                                break;
                            }
                        case "o":
                            {
                                withBlock.X = Type.AutoSw[3].X;
                                withBlock.Y = Type.AutoSw[3].Y;
                                break;
                            }
                        case "p":
                            {
                                withBlock.X = Type.AutoSw[4].X;
                                withBlock.Y = Type.AutoSw[4].Y;
                                break;
                            }
                        case "q":
                            {
                                withBlock.X = Type.AutoSe[1].X;
                                withBlock.Y = Type.AutoSe[1].Y;
                                break;
                            }
                        case "r":
                            {
                                withBlock.X = Type.AutoSe[2].X;
                                withBlock.Y = Type.AutoSe[2].Y;
                                break;
                            }
                        case "s":
                            {
                                withBlock.X = Type.AutoSe[3].X;
                                withBlock.Y = Type.AutoSe[3].Y;
                                break;
                            }
                        case "t":
                            {
                                withBlock.X = Type.AutoSe[4].X;
                                withBlock.Y = Type.AutoSe[4].Y;
                                break;
                            }
                    }
                }
            }
            else
            {
                {
                    ref var withBlock1 = ref Data.Autotile[x, y].Layer[layerNum].Tile[tileQuarter];
                    switch (autoTileLetter ?? "")
                    {
                        case "a":
                            {
                                withBlock1.X = Type.AutoIn[1].X;
                                withBlock1.Y = Type.AutoIn[1].Y;
                                break;
                            }
                        case "b":
                            {
                                withBlock1.X = Type.AutoIn[2].X;
                                withBlock1.Y = Type.AutoIn[2].Y;
                                break;
                            }
                        case "c":
                            {
                                withBlock1.X = Type.AutoIn[3].X;
                                withBlock1.Y = Type.AutoIn[3].Y;
                                break;
                            }
                        case "d":
                            {
                                withBlock1.X = Type.AutoIn[4].X;
                                withBlock1.Y = Type.AutoIn[4].Y;
                                break;
                            }
                        case "e":
                            {
                                withBlock1.X = Type.AutoNw[1].X;
                                withBlock1.Y = Type.AutoNw[1].Y;
                                break;
                            }
                        case "f":
                            {
                                withBlock1.X = Type.AutoNw[2].X;
                                withBlock1.Y = Type.AutoNw[2].Y;
                                break;
                            }
                        case "g":
                            {
                                withBlock1.X = Type.AutoNw[3].X;
                                withBlock1.Y = Type.AutoNw[3].Y;
                                break;
                            }
                        case "h":
                            {
                                withBlock1.X = Type.AutoNw[4].X;
                                withBlock1.Y = Type.AutoNw[4].Y;
                                break;
                            }
                        case "i":
                            {
                                withBlock1.X = Type.AutoNe[1].X;
                                withBlock1.Y = Type.AutoNe[1].Y;
                                break;
                            }
                        case "j":
                            {
                                withBlock1.X = Type.AutoNe[2].X;
                                withBlock1.Y = Type.AutoNe[2].Y;
                                break;
                            }
                        case "k":
                            {
                                withBlock1.X = Type.AutoNe[3].X;
                                withBlock1.Y = Type.AutoNe[3].Y;
                                break;
                            }
                        case "l":
                            {
                                withBlock1.X = Type.AutoNe[4].X;
                                withBlock1.Y = Type.AutoNe[4].Y;
                                break;
                            }
                        case "m":
                            {
                                withBlock1.X = Type.AutoSw[1].X;
                                withBlock1.Y = Type.AutoSw[1].Y;
                                break;
                            }
                        case "n":
                            {
                                withBlock1.X = Type.AutoSw[2].X;
                                withBlock1.Y = Type.AutoSw[2].Y;
                                break;
                            }
                        case "o":
                            {
                                withBlock1.X = Type.AutoSw[3].X;
                                withBlock1.Y = Type.AutoSw[3].Y;
                                break;
                            }
                        case "p":
                            {
                                withBlock1.X = Type.AutoSw[4].X;
                                withBlock1.Y = Type.AutoSw[4].Y;
                                break;
                            }
                        case "q":
                            {
                                withBlock1.X = Type.AutoSe[1].X;
                                withBlock1.Y = Type.AutoSe[1].Y;
                                break;
                            }
                        case "r":
                            {
                                withBlock1.X = Type.AutoSe[2].X;
                                withBlock1.Y = Type.AutoSe[2].Y;
                                break;
                            }
                        case "s":
                            {
                                withBlock1.X = Type.AutoSe[3].X;
                                withBlock1.Y = Type.AutoSe[3].Y;
                                break;
                            }
                        case "t":
                            {
                                withBlock1.X = Type.AutoSe[4].X;
                                withBlock1.Y = Type.AutoSe[4].Y;
                                break;
                            }
                    }
                }
            }

        }

        public static void InitAutotiles()
        {
            int x;
            int y;
            int layerNum;
            // Procedure used to cache autotile positions. All positioning is
            // independant from the tileset. Calculations are convoluted and annoying.
            // Maths is not my strong point. Luckily we're caching them so it's a one-off
            // thing when the map is originally loaded. As such optimisation isn't an issue.
            // For simplicity's sake we cache all subtile SOURCE positions in to an array.
            // We also give letters to each subtile for easy rendering tweaks. ;]
            // First, we need to re-size the array

            Data.Autotile = new Type.Autotile[(Data.MyMap.MaxX), (Data.MyMap.MaxY)];
            var loopTo = (int)Data.MyMap.MaxX;
            for (x = 0; x < loopTo; x++)
            {
                var loopTo1 = (int)Data.MyMap.MaxY;
                for (y = 0; y < loopTo1; y++)
                {
                    int layerCount = System.Enum.GetValues(typeof(MapLayer)).Length;
                    Data.Autotile[x, y].Layer = new Type.QuarterTile[layerCount];
                    for (int i = 0; i < layerCount; i++)
                    {
                        Data.Autotile[x, y].Layer[i].SrcX = new int[5];
                        Data.Autotile[x, y].Layer[i].SrcY = new int[5];
                        Data.Autotile[x, y].Layer[i].Tile = new Type.Point[5];
                    }
                }
            }

            // Inner tiles (Top right subtile region)
            // NW - a
            Type.AutoIn[1].X = 32;
            Type.AutoIn[1].Y = 0;
            // NE - b
            Type.AutoIn[2].X = 48;
            Type.AutoIn[2].Y = 0;
            // SW - c
            Type.AutoIn[3].X = 32;
            Type.AutoIn[3].Y = 16;
            // SE - d
            Type.AutoIn[4].X = 48;
            Type.AutoIn[4].Y = 16;
            // Outer Tiles - NW (bottom subtile region)
            // NW - e
            Type.AutoNw[1].X = 0;
            Type.AutoNw[1].Y = 32;
            // NE - f
            Type.AutoNw[2].X = 16;
            Type.AutoNw[2].Y = 32;
            // SW - g
            Type.AutoNw[3].X = 0;
            Type.AutoNw[3].Y = 48;
            // SE - h
            Type.AutoNw[4].X = 16;
            Type.AutoNw[4].Y = 48;
            // Outer Tiles - NE (bottom subtile region)
            // NW - i
            Type.AutoNe[1].X = 32;
            Type.AutoNe[1].Y = 32;
            // NE - g
            Type.AutoNe[2].X = 48;
            Type.AutoNe[2].Y = 32;
            // SW - k
            Type.AutoNe[3].X = 32;
            Type.AutoNe[3].Y = 48;
            // SE - l
            Type.AutoNe[4].X = 48;
            Type.AutoNe[4].Y = 48;
            // Outer Tiles - SW (bottom subtile region)
            // NW - m
            Type.AutoSw[1].X = 0;
            Type.AutoSw[1].Y = 64;
            // NE - n
            Type.AutoSw[2].X = 16;
            Type.AutoSw[2].Y = 64;
            // SW - o
            Type.AutoSw[3].X = 0;
            Type.AutoSw[3].Y = 80;
            // SE - p
            Type.AutoSw[4].X = 16;
            Type.AutoSw[4].Y = 80;
            // Outer Tiles - SE (bottom subtile region)
            // NW - q
            Type.AutoSe[1].X = 32;
            Type.AutoSe[1].Y = 64;
            // NE - r
            Type.AutoSe[2].X = 48;
            Type.AutoSe[2].Y = 64;
            // SW - s
            Type.AutoSe[3].X = 32;
            Type.AutoSe[3].Y = 80;
            // SE - t
            Type.AutoSe[4].X = 48;
            Type.AutoSe[4].Y = 80;

            var loopTo2 = (int)Data.MyMap.MaxX;
            for (x = 0; x < loopTo2; x++)
            {
                var loopTo3 = (int)Data.MyMap.MaxY;
                for (y = 0; y < loopTo3; y++)
                {
                    if (Data.Autotile[x, y].Layer == null)
                        return;

                    int layerCount = System.Enum.GetValues(typeof(MapLayer)).Length;
                    for (layerNum = 0; layerNum < layerCount; layerNum++)
                    {
                        // calculate the subtile positions and place them
                        CalculateAutotile(x, y, layerNum);
                        // cache the rendering state of the tiles and set them
                        CacheRenderState(x, y, layerNum);
                    }
                }
            }

        }

        public static void CacheRenderState(int x, int y, int layerNum)
        {
            int quarterNum;

            if (x < 0 | x >= Data.MyMap.MaxX | y < 0 | y >= Data.MyMap.MaxY)
                return;

            ref var withBlock = ref Data.MyMap.Tile[x, y];

            // check if the tile can be rendered
            if (withBlock.Layer[layerNum].Tileset <= 0 | withBlock.Layer[layerNum].Tileset > GameState.NumTileSets)
            {
                Data.Autotile[x, y].Layer[layerNum].RenderState = GameState.RenderStateNone;
                return;
            }

            // check if it needs to be rendered as an autotile
            if (withBlock.Layer[layerNum].AutoTile == GameState.AutotileNone | withBlock.Layer[layerNum].AutoTile == GameState.AutotileFake)
            {
                // default to... default
                Data.Autotile[x, y].Layer[layerNum].RenderState = GameState.RenderStateNormal;
            }
            else
            {
                Data.Autotile[x, y].Layer[layerNum].RenderState = GameState.RenderStateAutotile;
                // cache tileset positioning
                for (quarterNum = 0; quarterNum <= 4; quarterNum++)
                {
                    Data.Autotile[x, y].Layer[layerNum].SrcX[quarterNum] = Data.MyMap.Tile[x, y].Layer[layerNum].X * 32 + Data.Autotile[x, y].Layer[layerNum].Tile[quarterNum].X;
                    Data.Autotile[x, y].Layer[layerNum].SrcY[quarterNum] = Data.MyMap.Tile[x, y].Layer[layerNum].Y * 32 + Data.Autotile[x, y].Layer[layerNum].Tile[quarterNum].Y;
                }
            }
        }

        private static void CalculateAutotile(int x, int y, int layerNum)
        {
            // Right, so we've split the tile block in to an easy to remember
            // collection of letters. We now need to do the calculations to find
            // out which little lettered block needs to be rendered. We do this
            // by reading the surrounding tiles to check for matches.
            // First we check to make sure an autotile situation is actually there.
            // Then we calculate exactly which situation has arisen.
            // The situations are "inner", "outer", "horizontal", "vertical" and "fill".
            // Exit out if we don't have an autotile

            if (Data.MyMap.Tile[x, y].Layer[layerNum].AutoTile == 0)
                return;

            // Okay, we have autotiling but which one?
            switch (Data.MyMap.Tile[x, y].Layer[layerNum].AutoTile)
            {
                // Normal or animated - same difference
                case GameState.AutotileNormal:
                case GameState.AutotileAnim:
                    // North West Quarter
                    CalculateNW_Normal(layerNum, x, y);
                    // North East Quarter
                    CalculateNE_Normal(layerNum, x, y);
                    // South West Quarter
                    CalculateSW_Normal(layerNum, x, y);
                    // South East Quarter
                    CalculateSE_Normal(layerNum, x, y);
                    break;
                // Cliff
                case GameState.AutotileCliff:
                    {
                        // North West Quarter
                        CalculateNW_Cliff(layerNum, x, y);
                        // North East Quarter
                        CalculateNE_Cliff(layerNum, x, y);
                        // South West Quarter
                        CalculateSW_Cliff(layerNum, x, y);
                        // South East Quarter
                        CalculateSE_Cliff(layerNum, x, y);
                        break;
                    }
                // Waterfalls
                case GameState.AutotileWaterfall:
                    {
                        // North West Quarter
                        CalculateNW_Waterfall(layerNum, x, y);
                        // North East Quarter
                        CalculateNE_Waterfall(layerNum, x, y);
                        // South West Quarter
                        CalculateSW_Waterfall(layerNum, x, y);
                        // South East Quarter
                        CalculateSE_Waterfall(layerNum, x, y);
                        break;
                    }
                    // Anything else
            }

        }

        // Normal autotiling
        private static void CalculateNW_Normal(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            var situation = default(byte);

            // North West
            if (CheckTileMatch(layerNum, x, y, x - 1, y - 1))
                tmpTile[1] = true;

            // North
            if (CheckTileMatch(layerNum, x, y, x, y - 1))
                tmpTile[2] = true;

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[2] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Horizontal
            if (!tmpTile[2] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[2] & !tmpTile[3])
                situation = GameState.AutoVertical;

            // Outer
            if (!tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoOuter;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "e");
                        break;
                    }
                case GameState.AutoOuter:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "a");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "i");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "m");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "q");
                        break;
                    }
            }

        }

        private static void CalculateNE_Normal(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            var situation = default(byte);

            // North
            if (CheckTileMatch(layerNum, x, y, x, y - 1))
                tmpTile[1] = true;

            // North East
            if (CheckTileMatch(layerNum, x, y, x + 1, y - 1))
                tmpTile[2] = true;

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Horizontal
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoVertical;
            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3])
                situation = GameState.AutoOuter;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "j");
                        break;
                    }
                case GameState.AutoOuter:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "b");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "f");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "r");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "n");
                        break;
                    }
            }

        }

        private static void CalculateSW_Normal(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            var situation = default(byte);

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile[1] = true;

            // South West
            if (CheckTileMatch(layerNum, x, y, x - 1, y + 1))
                tmpTile[2] = true;

            // South
            if (CheckTileMatch(layerNum, x, y, x, y + 1))
                tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Horizontal
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoVertical;

            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3])
                situation = GameState.AutoOuter;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "o");
                        break;
                    }
                case GameState.AutoOuter:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "c");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "s");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "g");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "k");
                        break;
                    }
            }

        }

        private static void CalculateSE_Normal(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            var situation = default(byte);

            // South
            if (CheckTileMatch(layerNum, x, y, x, y + 1))
                tmpTile[1] = true;

            // South East
            if (CheckTileMatch(layerNum, x, y, x + 1, y + 1))
                tmpTile[2] = true;

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Horizontal
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoVertical;

            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3])
                situation = GameState.AutoOuter;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "t");
                        break;
                    }
                case GameState.AutoOuter:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "d");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "p");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "l");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "h");
                        break;
                    }
            }

        }

        // Waterfall autotiling
        private static void CalculateNW_Waterfall(int layerNum, int x, int y)
        {
            var tmpTile = default(bool);

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile = true;

            // Actually place the subtile
            if (tmpTile)
            {
                // Extended
                PlaceAutotile(layerNum, x, y, 1, "i");
            }
            else
            {
                // Edge
                PlaceAutotile(layerNum, x, y, 1, "e");
            }

        }

        private static void CalculateNE_Waterfall(int layerNum, int x, int y)
        {
            var tmpTile = default(bool);

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile = true;
            // Actually place the subtile
            if (tmpTile)
            {
                // Extended
                PlaceAutotile(layerNum, x, y, 2, "f");
            }
            else
            {
                // Edge
                PlaceAutotile(layerNum, x, y, 2, "j");
            }

        }

        private static void CalculateSW_Waterfall(int layerNum, int x, int y)
        {
            var tmpTile = default(bool);

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile = true;
            // Actually place the subtile
            if (tmpTile)
            {
                // Extended
                PlaceAutotile(layerNum, x, y, 3, "k");
            }
            else
            {
                // Edge
                PlaceAutotile(layerNum, x, y, 3, "g");
            }

        }

        private static void CalculateSE_Waterfall(int layerNum, int x, int y)
        {
            var tmpTile = default(bool);

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile = true;
            // Actually place the subtile
            if (tmpTile)
            {
                // Extended
                PlaceAutotile(layerNum, x, y, 4, "h");
            }
            else
            {
                // Edge
                PlaceAutotile(layerNum, x, y, 4, "l");
            }

        }

        // Cliff autotiling
        private static void CalculateNW_Cliff(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            byte situation;

            // North West
            if (CheckTileMatch(layerNum, x, y, x - 1, y - 1))
                tmpTile[1] = true;

            // North
            if (CheckTileMatch(layerNum, x, y, x, y - 1))
                tmpTile[2] = true;

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile[3] = true;
            situation = GameState.AutoFill;

            // Calculate Situation - Horizontal
            if (!tmpTile[2] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[2] & !tmpTile[3])
                situation = GameState.AutoVertical;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Inner
            if (!tmpTile[2] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "e");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "i");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "m");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 1, "q");
                        break;
                    }
            }

        }

        private static void CalculateNE_Cliff(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            byte situation;

            // North
            if (CheckTileMatch(layerNum, x, y, x, y - 1))
                tmpTile[1] = true;

            // North East
            if (CheckTileMatch(layerNum, x, y, x + 1, y - 1))
                tmpTile[2] = true;

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile[3] = true;
            situation = GameState.AutoFill;

            // Calculate Situation - Horizontal
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoVertical;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "j");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "f");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "r");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 2, "n");
                        break;
                    }
            }

        }

        private static void CalculateSW_Cliff(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            byte situation;

            // West
            if (CheckTileMatch(layerNum, x, y, x - 1, y))
                tmpTile[1] = true;

            // South West
            if (CheckTileMatch(layerNum, x, y, x - 1, y + 1))
                tmpTile[2] = true;

            // South
            if (CheckTileMatch(layerNum, x, y, x, y + 1))
                tmpTile[3] = true;
            situation = GameState.AutoFill;

            // Calculate Situation - Horizontal
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoVertical;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;
            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "o");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "s");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "g");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 3, "k");
                        break;
                    }
            }

        }

        private static void CalculateSE_Cliff(int layerNum, int x, int y)
        {
            var tmpTile = new bool[4];
            byte situation;

            // South
            if (CheckTileMatch(layerNum, x, y, x, y + 1))
                tmpTile[1] = true;

            // South East
            if (CheckTileMatch(layerNum, x, y, x + 1, y + 1))
                tmpTile[2] = true;

            // East
            if (CheckTileMatch(layerNum, x, y, x + 1, y))
                tmpTile[3] = true;

            situation = GameState.AutoFill;
            // Calculate Situation -  Horizontal
            if (!tmpTile[1] & tmpTile[3])
                situation = GameState.AutoHorizontal;

            // Vertical
            if (tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoVertical;

            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3])
                situation = GameState.AutoFill;

            // Inner
            if (!tmpTile[1] & !tmpTile[3])
                situation = GameState.AutoInner;

            // Actually place the subtile
            switch (situation)
            {
                case GameState.AutoInner:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "t");
                        break;
                    }
                case GameState.AutoHorizontal:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "p");
                        break;
                    }
                case GameState.AutoVertical:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "l");
                        break;
                    }
                case GameState.AutoFill:
                    {
                        PlaceAutotile(layerNum, x, y, 4, "h");
                        break;
                    }
            }

        }

        private static bool CheckTileMatch(int layerNum, int x1, int y1, int x2, int y2)
        {
            bool checkTileMatch = default;
            checkTileMatch = true;

            // if it's off the map then set it as autotile and exit out early
            if (x2 < 0 | x2 > Data.MyMap.MaxX | y2 < 0 | y2 > Data.MyMap.MaxY)
            {
                checkTileMatch = true;
                return checkTileMatch;
            }

            // fakes ALWAYS return true
            if (Data.MyMap.Tile[x2, y2].Layer[layerNum].AutoTile == GameState.AutotileFake)
            {
                checkTileMatch = true;
                return checkTileMatch;
            }

            // check neighbour is an autotile
            if (Data.MyMap.Tile[x2, y2].Layer[layerNum].AutoTile == 0)
            {
                checkTileMatch = false;
                return checkTileMatch;
            }

            // check we're a matching
            if (Data.MyMap.Tile[x1, y1].Layer[layerNum].Tileset != Data.MyMap.Tile[x2, y2].Layer[layerNum].Tileset)
            {
                checkTileMatch = false;
                return checkTileMatch;
            }

            // check tiles match
            if (Data.MyMap.Tile[x1, y1].Layer[layerNum].X != Data.MyMap.Tile[x2, y2].Layer[layerNum].X)
            {
                checkTileMatch = false;
                return checkTileMatch;
            }
            else if (Data.MyMap.Tile[x1, y1].Layer[layerNum].Y != Data.MyMap.Tile[x2, y2].Layer[layerNum].Y)
            {
                checkTileMatch = false;
                return checkTileMatch;
            }

            return checkTileMatch;
        }

    }
}