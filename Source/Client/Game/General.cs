using Client.Game.Objects;
using Core;
using Core.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using static Core.Global.Command;

namespace Client
{

    public class General
    {
        public static GameClient Client = new GameClient();
        public static GameState State = new GameState();
        public static RandomUtility Random = new RandomUtility();
        public static Gui Gui = new Gui();
        
        public static byte[] AesKey = new byte[32];
        public static byte[] AesIV = new byte[16];

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		public const int SW_RESTORE = 9;

		[DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        public static void SetWindowFocus(IntPtr hWnd)
        {
            // Restore window if minimized
            General.ShowWindow(hWnd, General.SW_RESTORE);

            // Bring the window to front
            General.SetForegroundWindow(hWnd);
        }

        public static void Startup()
        {
            GameState.InMenu = true;
            ClearGameData();
            LoadGame();
        }

        public static void LoadGame()
        {
            SettingsManager.Load();
            LocalesManager.Initialize();
            CheckAnimations();
            CheckCharacters();
            CheckEmotes();
            CheckTilesets();
            CheckFogs();
            CheckItems();
            CheckPanoramas();
            CheckPaperdolls();
            CheckParallax();
            CheckPictures();
            CheckProjectile();
            CheckResources();
            CheckSkills();
            CheckInterface();
            CheckGradients();
            CheckDesigns();
            Sound.InitializeBass();
            _ = NetworkConfig.InitNetwork();
            Ui.Load();
            Gui.Init();
            GameState.Ping = -1;
        }

        public static void CheckAnimations()
        {
            GameState.NumAnimations = GetFileCount(Core.Path.Animations);
        }

        public static void CheckCharacters()
        {
            GameState.NumCharacters = GetFileCount(Core.Path.Characters);
        }

        public static void CheckEmotes()
        {
            GameState.NumEmotes = GetFileCount(Core.Path.Emotes);
        }

        public static void CheckTilesets()
        {
            GameState.NumTileSets = GetFileCount(Core.Path.Tilesets);
        }

        public static void CheckFogs()
        {
            GameState.NumFogs = GetFileCount(Core.Path.Fogs);
        }

        public static void CheckItems()
        {
            GameState.NumItems = GetFileCount(Core.Path.Items);
        }

        public static void CheckPanoramas()
        {
            GameState.NumPanoramas = GetFileCount(Core.Path.Panoramas);
        }

        public static void CheckPaperdolls()
        {
            GameState.NumPaperdolls = GetFileCount(Core.Path.Paperdolls);
        }

        public static void CheckParallax()
        {
            GameState.NumParallax = GetFileCount(Core.Path.Parallax);
        }

        public static void CheckPictures()
        {
            GameState.NumPictures = GetFileCount(Core.Path.Pictures);
        }

        public static void CheckProjectile()
        {
            GameState.NumProjectiles = GetFileCount(Core.Path.Projectiles);
        }

        public static void CheckResources()
        {
            GameState.NumResources = GetFileCount(Core.Path.Resources);
        }

        public static void CheckSkills()
        {
            GameState.NumSkills = GetFileCount(Core.Path.Skills);
        }

        public static void CheckInterface()
        {
            GameState.NumInterface = GetFileCount(Core.Path.Gui);
        }

        public static void CheckGradients()
        {
            GameState.NumGradients = GetFileCount(Core.Path.Gradients);
        }

        public static void CheckDesigns()
        {
            GameState.NumDesigns = GetFileCount(Core.Path.Designs);
        }

        public static void GetResolutionSize(byte resolution, ref int width, ref int height)
        {
            switch (resolution)
            {
                case 1:
                    {
                        width = 1920;
                        height = 1080;
                        break;
                    }
                case 2:
                    {
                        width = 1680;
                        height = 1050;
                        break;
                    }
                case 3:
                    {
                        width = 1600;
                        height = 900;
                        break;
                    }
                case 4:
                    {
                        width = 1440;
                        height = 900;
                        break;
                    }
                case 5:
                    {
                        width = 1440;
                        height = 1050;
                        break;
                    }
                case 6:
                    {
                        width = 1366;
                        height = 768;
                        break;
                    }
                case 7:
                    {
                        width = 1360;
                        height = 1024;
                        break;
                    }
                case 8:
                    {
                        width = 1360;
                        height = 768;
                        break;
                    }
                case 9:
                    {
                        width = 1280;
                        height = 1024;
                        break;
                    }
                case 10:
                    {
                        width = 1280;
                        height = 800;
                        break;
                    }
                case 11:
                    {
                        width = 1280;
                        height = 768;
                        break;
                    }
                case 12:
                    {
                        width = 1280;
                        height = 720;
                        break;
                    }
                case 13:
                    {
                        width = 1120;
                        height = 864;
                        break;
                    }
                case 14:
                    {
                        width = 1024;
                        height = 768;
                        break;
                    }
            }
        }

        public static void ClearGameData()
        {
            Map.ClearMap();
            Map.ClearMapNpcs();
            Map.ClearMapItems();
            Database.ClearNpcs();
            MapResource.ClearResources();
            Item.ClearItems();
            Shop.ClearShops();
            Database.ClearSkills();
            Animation.ClearAnimations();
            Projectile.ClearProjectile();
            Database.ClearJobs();
            Moral.ClearMorals();
            Bank.ClearBanks();
            Party.ClearParty();

            for (int i = 0; i < Constant.MaxPlayers; i++)
                Player.ClearPlayer(i);

            Animation.ClearAnimInstances();
            Autotile.ClearAutotiles();

            // clear chat
            for (int i = 0; i < Constant.ChatLines; i++)
                Data.Chat[i].Text = "";
        }

        public static int GetFileCount(string folderName)
        {
            string folderPath = System.IO.Path.Combine(Core.Path.Graphics, folderName);
            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath, "*.png").Length; // Adjust for other formats if needed
            }
            else
            {
                Console.WriteLine($"Folder not found: {folderPath}");
                return 0;
            }
        }

        public static void CacheMusic()
        {
            Sound.MusicCache = new string[Directory.GetFiles(Core.Path.Music, "*" + SettingsManager.Instance.MusicExt).Count() + 1];
            string[] files = Directory.GetFiles(Core.Path.Music, "*" + SettingsManager.Instance.MusicExt);
            string maxNum = Directory.GetFiles(Core.Path.Music, "*" + SettingsManager.Instance.MusicExt).Count().ToString();
            int counter = 0;

            foreach (var fileName in files)
            {
                Sound.MusicCache[counter] = System.IO.Path.GetFileName(fileName);
                counter = counter + 1;
            }
        }

        public static void CacheSound()
        {
            Sound.SoundCache = new string[Directory.GetFiles(Core.Path.Sounds, "*" + SettingsManager.Instance.SoundExt).Count() + 1];
            string[] files = Directory.GetFiles(Core.Path.Sounds, "*" + SettingsManager.Instance.SoundExt);
            string maxNum = Directory.GetFiles(Core.Path.Sounds, "*" + SettingsManager.Instance.SoundExt).Count().ToString();
            int counter = 0;

            foreach (var fileName in files)
            {
                Sound.SoundCache[counter] = System.IO.Path.GetFileName(fileName);
                counter = counter + 1;
            }
        }

        public static void GameInit()
        {
            // Send a request to the server to open the admin menu if the user wants it.
            if (SettingsManager.Instance.OpenAdminPanelOnLogin == true)
            {
                if (GetPlayerAccess(GameState.MyIndex) > 0)
                {
                    NetworkSend.SendRequestAdmin();
                }
            }
        }

        public static void DestroyGame()
        {
            // break out of GameLoop
            GameState.InGame = false;
            GameState.InMenu = false;
            Sound.FreeBass();
            NetworkConfig.DestroyNetwork();
            Environment.Exit(0);
        }

        // Get the shifted version of a digit key (for symbols)
        public static char GetShiftedDigit(char digit)
        {
            switch (digit)
            {
                case '1':
                    {
                        return '!';
                    }
                case '2':
                    {
                        return '@';
                    }
                case '3':
                    {
                        return '#';
                    }
                case '4':
                    {
                        return '$';
                    }
                case '5':
                    {
                        return '%';
                    }
                case '6':
                    {
                        return '^';
                    }
                case '7':
                    {
                        return '&';
                    }
                case '8':
                    {
                        return '*';
                    }
                case '9':
                    {
                        return '(';
                    }
                case '0':
                    {
                        return ')';
                    }

                default:
                    {
                        return digit;
                    }
            }
        }

        public static long IsEq(long startX, long startY)
        {
            long isEqRet = default;
            Core.Type.Rect tempRec;
            long i;

            int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
            for (i = 0L; i < equipmentCount; i++)
            {
                if (GetPlayerEquipment(GameState.MyIndex, (Equipment)i) >= 0)
                {
                    tempRec.Top = startY + GameState.EqTop + GameState.SizeY * (i / GameState.EqColumns);
                    tempRec.Bottom = tempRec.Top + GameState.SizeY;
                    tempRec.Left = startX + GameState.EqLeft + (GameState.EqOffsetX + GameState.SizeX) * (i % GameState.EqColumns);
                    tempRec.Right = tempRec.Left + GameState.SizeX;

                    if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                    {
                        if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                        {
                            isEqRet = i;
                            return isEqRet;
                        }
                    }
                }
            }

            return isEqRet;
        }

        public static long IsInv(long startX, long startY)
        {
            long isInvRet = default;
            Core.Type.Rect tempRec;
            long i;

            for (i = 0L; i < Constant.MaxInv; i++)
            {
                if (GetPlayerInv(GameState.MyIndex, (int)i) >= 0)
                {
                    tempRec.Top = startY + GameState.InvTop + (GameState.InvOffsetY) * (i / GameState.InvColumns);
                    tempRec.Bottom = tempRec.Top;
                    tempRec.Left = startX + GameState.InvLeft + (GameState.InvOffsetX) * (i % GameState.InvColumns);
                    tempRec.Right = tempRec.Left;

                    if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                    {
                        if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                        {
                            isInvRet = i;
                            return isInvRet;
                        }
                    }
                }
            }

            return -1;
        }

        public static long IsSkill(long startX, long startY)
        {
            long isSkillRet = default;
            Core.Type.Rect tempRec;
            long i;

            for (i = 0L; i < Constant.MaxPlayerSkills; i++)
            {
                if (Core.Data.Player[GameState.MyIndex].Skill[(int)i].Num >= 0)
                {
                    tempRec.Top = startY + GameState.SkillTop + (GameState.SkillOffsetY + GameState.SizeY) * (i / GameState.SkillColumns);
                    tempRec.Bottom = tempRec.Top + GameState.SizeY;
                    tempRec.Left = startX + GameState.SkillLeft + (GameState.SkillOffsetX + GameState.SizeX) * (i % GameState.SkillColumns);
                    tempRec.Right = tempRec.Left + GameState.SizeX;

                    if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                    {
                        if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                        {
                            isSkillRet = i;
                            return isSkillRet;
                        }
                    }
                }
            }

            return -1;
        }

        public static long IsBank(long startX, long startY)
        {
            byte isBankRet = default;
            Core.Type.Rect tempRec;

            for (byte i = 0; i < Constant.MaxBank; i++)
            {
                if (GetBank(GameState.MyIndex, (byte)i) >= 0)
                {
                    tempRec.Top = startY + GameState.BankTop + (GameState.BankOffsetY + GameState.SizeY) * (i / GameState.BankColumns);
                    tempRec.Bottom = tempRec.Top + GameState.SizeY;
                    tempRec.Left = startX + GameState.BankLeft + (GameState.BankOffsetX + GameState.SizeX) * (i % GameState.BankColumns);
                    tempRec.Right = tempRec.Left + GameState.SizeX;

                    if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                    {
                        if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                        {
                            isBankRet = i;
                            return isBankRet;
                        }
                    }
                }

            }

            return -1;

        }

        public static long IsShop(long startX, long startY)
        {
            long isShopRet = default;
            Core.Type.Rect tempRec;
            long i;

            for (i = 0L; i < Constant.MaxTrades; i++)
            {
                tempRec.Top = startY + GameState.ShopTop + (GameState.ShopOffsetY + GameState.SizeY) * (i / GameState.ShopColumns);
                tempRec.Bottom = tempRec.Top + GameState.SizeY;
                tempRec.Left = startX + GameState.ShopLeft + (GameState.ShopOffsetX + GameState.SizeX) * (i % GameState.ShopColumns);
                tempRec.Right = tempRec.Left + GameState.SizeX;

                if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                {
                    if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                    {
                        isShopRet = i;
                        return isShopRet;
                    }
                }
            }

            return -1;
        }

        public static long IsTrade(long startX, long startY)
        {
            long isTradeRet = default;
            Core.Type.Rect tempRec;
            long i;

            for (i = 0L; i < Constant.MaxInv; i++)
            {
                tempRec.Top = startY + GameState.TradeTop + (GameState.TradeOffsetY + GameState.SizeY) * (i / GameState.TradeColumns);
                tempRec.Bottom = tempRec.Top + GameState.SizeY;
                tempRec.Left = startX + GameState.TradeLeft + (GameState.TradeOffsetX + GameState.SizeX) * (i % GameState.TradeColumns);
                tempRec.Right = tempRec.Left + GameState.SizeX;

                if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                {
                    if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                    {
                        isTradeRet = i;
                        return isTradeRet;
                    }
                }
            }

            return -1;
        }

    }
}