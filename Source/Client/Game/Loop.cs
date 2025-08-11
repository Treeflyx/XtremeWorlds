using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using Client.Game.UI;
using Client.Net;
using Core.Configurations;
using Core.Globals;
using static Core.Globals.Command;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Type = Core.Globals.Type;

namespace Client
{

    public class Loop
    {
        // Declare private fields
        private static int _i;
        private static int _tmr1000;
        private static int _tick;
        private static int _fogTmr;
        private static int _chatTmr;
        private static int _tmpFps;
        private static int _tmpLps;
        private static int _walkTimer;
        private static int _frameTime;
        private static int _tmrWeather;
        private static int _barTmr;
        private static int _tmr25;
        private static int _tmr500;
        private static int _tmr250;
        private static int _tmrConnect;
        private static int _tickFps;
        private static int _fadeTmr;
        private static int _renderTmr;
        private static int[] _animationTmr = new int[2];

        public static void Game()
        {
            _tick = General.GetTickCount();
            GameState.ElapsedTime = _tick - _frameTime; // Set the time difference for time-based movement

            _frameTime = _tick;

            if (GameLogic.GameStarted())
            {
                if (_tmr1000 < _tick)
                {
                    Sender.GetPing();
                    _tmr1000 = _tick + 1000;
                }

                if (_tmr25 < _tick)
                {
                    Sound.PlayMusic(Data.MyMap.Music);
                    _tmr25 = _tick + 25;
                }

                if (GameState.ShowAnimTimer < _tick)
                {
                    GameState.ShowAnimLayers = !GameState.ShowAnimLayers;
                    GameState.ShowAnimTimer = _tick + 500;
                }

                for (int layer = 0; layer <= 1; layer++)
                {
                    if (_animationTmr[layer] < _tick)
                    {
                        for (byte x = 0, loopTo = Data.MyMap.MaxX; x < loopTo; x++)
                        {
                            for (byte y = 0, loopTo1 = Data.MyMap.MaxY; y < loopTo1; y++)
                            {
                                if (GameLogic.IsValidMapPoint(x, y))
                                {
                                    if (Data.MyMap.Tile[x, y].Type == TileType.Animation)
                                    {                                      
                                        _animationTmr[layer] = _tick + Animation.PlayAnimation(Data.Animation[Data.MyMap.Tile[x, y].Data1].Sprite[layer], layer, Data.MyMap.Tile[x, y].Data1, x, y);
                                    }

                                    if (Data.MyMap.Tile[x, y].Type2 == TileType.Animation)
                                    {
                                        _animationTmr[layer] = _tick + Animation.PlayAnimation(Data.Animation[Data.MyMap.Tile[x, y].Data1_2].Sprite[layer], layer, Data.MyMap.Tile[x, y].Data1_2, x, y);
                                    }
                                }
                            }
                        }
                        ;


                    }
                }

                for (_i = 0; _i < byte.MaxValue; _i++)
                {
                    Animation.CheckAnimInstance(_i);
                }

                if (_tick > Event.EventChatTimer)
                {
                    if (string.IsNullOrEmpty(Event.EventText))
                    {
                        if (Event.EventChat)
                        {
                            Event.EventChat = false;
                        }
                    }
                }

                // screenshake
                if (GameState.ShakeTimerEnabled)
                {
                    if (GameState.ShakeTimer < _tick)
                    {
                        if (GameState.ShakeCount < 10)
                        {
                            if (GameState.LastDir == 0)
                            {
                                GameState.LastDir = 1;
                            }
                            else
                            {
                                GameState.LastDir = 0;
                            }
                        }
                        else
                        {
                            GameState.ShakeCount = 0;
                            GameState.ShakeTimerEnabled = false;
                        }

                        GameState.ShakeCount += 1;

                        GameState.ShakeTimer = _tick + 50;
                    }
                }

                // check if we need to end the CD icon
                if (GameState.NumSkills > 0)
                {
                    for (_i = 0; _i < Constant.MaxPlayerSkills; _i++)
                    {
                        if (Data.Player[GameState.MyIndex].Skill[_i].Num >= 0)
                        {
                            if (Data.Player[GameState.MyIndex].Skill[_i].Cd > 0)
                            {
                                if (Data.Player[GameState.MyIndex].Skill[_i].Cd + Data.Skill[(int)Data.Player[GameState.MyIndex].Skill[_i].Num].CdTime * 1000 < _tick)
                                {
                                    Data.Player[GameState.MyIndex].Skill[_i].Cd = 0;
                                }
                            }
                        }
                    }
                }

                // check if we need to unlock the player's skill casting restriction
                if (GameState.SkillBuffer >= 0)
                {
                    if (GameState.SkillBufferTimer + Data.Skill[(int)Data.Player[GameState.MyIndex].Skill[GameState.SkillBuffer].Num].CastTime * 1000 < _tick)
                    {
                        GameState.SkillBuffer = -1;
                        GameState.SkillBufferTimer = 0;
                    }
                }
                
                // Process input before rendering, otherwise input will be behind by 1 frame
                if (_walkTimer < _tick)
                {
                    if (GameState.CanMoveNow)
                    {
                        Player.CheckMovement(); // Check if player is trying to move
                        Player.CheckAttack();   // Check to see if player is trying to attack
                    }
                    
                    // Process player movements
                    for (_i = 0; _i < Constant.MaxPlayers; _i++)
                    {
                        if (IsPlaying(_i))
                        {
                            Player.ProcessMovement(_i);                            
                        }
                    }

                    // Process npc movements
                    for (_i = 0; _i < Constant.MaxMapNpcs; _i++)
                    {
                        Npc.ProcessMovement(_i);
                        
                    }

                    var loopTo2 = GameState.CurrentEvents;
                    for (_i = 0; _i < loopTo2; _i++)
                    {
                        Event.ProcessEventMovement(_i);
                    }

                    _walkTimer = _tick + 10;
                }

                // chat timer
                if (_chatTmr < _tick)
                {
                    // scrolling
                    if (GameState.ChatButtonUp)
                    {
                        GameLogic.ScrollChatBox(0);
                    }

                    if (GameState.ChatButtonDown)
                    {
                        GameLogic.ScrollChatBox(1);
                    }

                    _chatTmr = _tick + 50;
                }

                // fog scrolling
                if (_fogTmr < _tick)
                {
                    if (GameState.CurrentFogSpeed > 0)
                    {
                        // move
                        GameState.FogOffsetX = GameState.FogOffsetX - 1;
                        GameState.FogOffsetY = GameState.FogOffsetY - 1;

                        // reset
                        if (GameState.FogOffsetX < -255)
                            GameState.FogOffsetX = 1;

                        if (GameState.FogOffsetY < -255)
                            GameState.FogOffsetY = 1;

                        _fogTmr = _tick + 255 - GameState.CurrentFogSpeed;
                    }
                }

                if (_tmr500 < _tick)
                {
                    // animate waterfalls
                    switch (GameState.WaterfallFrame)
                    {
                        case 0:
                            {
                                GameState.WaterfallFrame = 1;
                                break;
                            }
                        case 1:
                            {
                                GameState.WaterfallFrame = 2;
                                break;
                            }
                        case 2:
                            {
                                GameState.WaterfallFrame = 0;
                                break;
                            }
                    }

                    // animate autotiles
                    switch (GameState.AutoTileFrame)
                    {
                        case 0:
                            {
                                GameState.AutoTileFrame = 1;
                                break;
                            }
                        case 1:
                            {
                                GameState.AutoTileFrame = 2;
                                break;
                            }
                        case 2:
                            {
                                GameState.AutoTileFrame = 0;
                                break;
                            }
                    }

                    // animate textbox
                    if (GameState.ChatShowLine == "|")
                    {
                        GameState.ChatShowLine = "";
                    }
                    else
                    {
                        GameState.ChatShowLine = "|";
                    }

                    _tmr500 = _tick + 500;
                }

                // elastic bars
                if (_barTmr < _tick)
                {
                    GameLogic.SetBarWidth(ref GameState.BarWidthGuiHpMax, ref GameState.BarWidthGuiHp);
                    GameLogic.SetBarWidth(ref GameState.BarWidthGuiSpMax, ref GameState.BarWidthGuiSp);
                    GameLogic.SetBarWidth(ref GameState.BarWidthGuiExpMax, ref GameState.BarWidthGuiExp);
                    for (_i = 0; _i < Constant.MaxMapNpcs; _i++)
                    {
                        if (Data.MyMapNpc[_i].Num >= 0)
                        {
                            GameLogic.SetBarWidth(ref GameState.BarWidthNpcHpMax[_i], ref GameState.BarWidthNpcHp[_i]);
                        }
                    }

                    for (_i = 0; _i < Constant.MaxPlayers; _i++)
                    {
                        if (IsPlaying(_i) & GetPlayerMap(_i) == GetPlayerMap(GameState.MyIndex))
                        {
                            GameLogic.SetBarWidth(ref GameState.BarWidthPlayerHpMax[_i], ref GameState.BarWidthPlayerHp[_i]);
                            GameLogic.SetBarWidth(ref GameState.BarWidthPlayerSpMax[_i], ref GameState.BarWidthPlayerSp[_i]);
                        }
                    }

                    // reset timer
                    _barTmr = _tick + 10;
                }

                // Change map animation
                if (_tmr250 < _tick)
                {
                    for (int i = 0; i < Constant.MaxPlayers; i++)
                    {
                        if (IsPlaying(i))
                        {
                            if (GetPlayerMap(i) == GetPlayerMap(GameState.MyIndex))
                            {
                                // Check if completed walking over to the next tile
                                if (Data.Player[i].Steps == 3)
                                {
                                    Data.Player[i].Steps = 0;
                                }
                                else
                                {
                                    Data.Player[i].Steps++;
                                }                              
                            }
                        }
                    }

                    for (int i = 0; i < Constant.MaxMapNpcs; i++)
                    {
                        if (Data.MyMapNpc[i].Num >= 0)
                        {
                            // Check if completed walking over to the next tile
                            if (Data.MyMapNpc[i].Steps == 3)
                            {
                                Data.MyMapNpc[i].Steps = 0;
                            }
                            else
                            {
                                Data.MyMapNpc[i].Steps++;
                            }
                        }
                    }

                    var loopTo = GameState.CurrentEvents;
                    for (_i = 0; _i < loopTo; _i++)
                    {
                        if (Data.MapEvents[_i].WalkAnim == 1)
                        {
                            // Check if completed walking over to the next tile
                            if (Data.MyMapNpc[_i].Steps == 3)
                            {
                                Data.MyMapNpc[_i].Steps = 0;
                            }
                            else
                            {
                                Data.MyMapNpc[_i].Steps++;
                            }
                        }
                    }

                    GameState.MapAnim = !GameState.MapAnim;
                    _tmr250 = _tick + 250;
                }

                if (Sound.FadeInSwitch == true)
                {
                    Sound.FadeIn();
                }

                if (Sound.FadeOutSwitch == true)
                {
                    Sound.FadeOut();
                }
            }
            else
            {
                if (_tmr500 < _tick)
                {
                    // animate textbox
                    if (GameState.ChatShowLine == "|")
                    {
                        GameState.ChatShowLine = "";
                    }
                    else
                    {
                        GameState.ChatShowLine = "|";
                    }

                    _tmr500 = _tick + 500;
                }

                if (_tmr25 < _tick)
                {
                    Sound.PlayMusic(SettingsManager.Instance.MenuMusic);
                    _tmr25 = _tick + 25;
                }
            }

            if (_tmrWeather < _tick)
            {
                Weather.ProcessWeather();
                _tmrWeather = _tick + 50;
            }

            if (_fadeTmr < _tick)
            {
                if (GameState.FadeType != 2)
                {
                    if (GameState.FadeType == 1)
                    {
                        if (GameState.FadeAmount == 255)
                        {

                        }
                        else
                        {
                            GameState.FadeAmount = GameState.FadeAmount + 5;
                        }
                    }
                    else if (GameState.FadeType == 0)
                    {
                        if (GameState.FadeAmount == 0)
                        {
                            GameState.UseFade = false;
                        }
                        else
                        {
                            GameState.FadeAmount = GameState.FadeAmount - 5;
                        }
                    }
                }
                _fadeTmr = _tick + 30;
            }

            Gui.ResizeGui();
        }
    }
}