using Client.Game.Objects;
using Core;
using System.Runtime.InteropServices;
using Client.Net;
using Core.Common;
using Core.Configurations;
using Core.Globals;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

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

		public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        public static void Startup()
        {
            GameState.InMenu = true;
            ClearGameData();
            LoadGame();
        }

        public static void LoadGame()
        {
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
            _ = Network.Start();
            Ui.Load();
            Gui.Init();
            GameState.Ping = -1;
        }

        public static void CheckAnimations()
        {
            GameState.NumAnimations = GetFileCount(DataPath.Animations);
        }

        public static void CheckCharacters()
        {
            GameState.NumCharacters = GetFileCount(DataPath.Characters);
        }

        public static void CheckEmotes()
        {
            GameState.NumEmotes = GetFileCount(DataPath.Emotes);
        }

        public static void CheckTilesets()
        {
            GameState.NumTileSets = GetFileCount(DataPath.Tilesets);
        }

        public static void CheckFogs()
        {
            GameState.NumFogs = GetFileCount(DataPath.Fogs);
        }

        public static void CheckItems()
        {
            GameState.NumItems = GetFileCount(DataPath.Items);
        }

        public static void CheckPanoramas()
        {
            GameState.NumPanoramas = GetFileCount(DataPath.Panoramas);
        }

        public static void CheckPaperdolls()
        {
            GameState.NumPaperdolls = GetFileCount(DataPath.Paperdolls);
        }

        public static void CheckParallax()
        {
            GameState.NumParallax = GetFileCount(DataPath.Parallax);
        }

        public static void CheckPictures()
        {
            GameState.NumPictures = GetFileCount(DataPath.Pictures);
        }

        public static void CheckProjectile()
        {
            GameState.NumProjectiles = GetFileCount(DataPath.Projectiles);
        }

        public static void CheckResources()
        {
            GameState.NumResources = GetFileCount(DataPath.Resources);
        }

        public static void CheckSkills()
        {
            GameState.NumSkills = GetFileCount(DataPath.Skills);
        }

        public static void CheckInterface()
        {
            GameState.NumInterface = GetFileCount(DataPath.Gui);
        }

        public static void CheckGradients()
        {
            GameState.NumGradients = GetFileCount(DataPath.Gradients);
        }

        public static void CheckDesigns()
        {
            GameState.NumDesigns = GetFileCount(DataPath.Designs);
        }

        public static (int Width, int Height) GetResolutionSize(byte resolution)
        {
            return resolution switch
            {
                1 => (1920, 1080),
                2 => (1680, 1050),
                3 => (1600, 900),
                4 => (1440, 900),
                5 => (1440, 1050),
                6 => (1366, 768),
                7 => (1360, 1024),
                8 => (1360, 768),
                9 => (1280, 1024),
                10 => (1280, 800),
                11 => (1280, 768),
                12 => (1280, 720),
                13 => (1120, 864),
                14 => (1024, 768),
                _ => (1280, 720)
            };
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

        public static int GetFileCount(string folderPath)
        {
            // folderPath is expected to be an absolute directory path (e.g., DataPath.Resources)
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
            Sound.MusicCache = new string[Directory.GetFiles(DataPath.Music, "*" + SettingsManager.Instance.MusicExt).Count() + 1];
            string[] files = Directory.GetFiles(DataPath.Music, "*" + SettingsManager.Instance.MusicExt);
            string maxNum = Directory.GetFiles(DataPath.Music, "*" + SettingsManager.Instance.MusicExt).Count().ToString();
            int counter = 0;

            foreach (var fileName in files)
            {
                Sound.MusicCache[counter] = System.IO.Path.GetFileName(fileName);
                counter = counter + 1;
            }
        }

        public static void CacheSound()
        {
            Sound.SoundCache = new string[Directory.GetFiles(DataPath.Sounds, "*" + SettingsManager.Instance.SoundExt).Count() + 1];
            string[] files = Directory.GetFiles(DataPath.Sounds, "*" + SettingsManager.Instance.SoundExt);
            string maxNum = Directory.GetFiles(DataPath.Sounds, "*" + SettingsManager.Instance.SoundExt).Count().ToString();
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
                    Sender.SendRequestAdmin();
                }
            }
        }

        public static void DestroyGame()
        {
            // break out of GameLoop
            GameState.InGame = false;
            GameState.InMenu = false;
            Sound.FreeBass();
            Network.Stop();
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
            long isEq = default;
            Type.Rect tempRec;
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
                            isEq = i;
                            return isEq;
                        }
                    }
                }
            }

            return isEq;
        }

        public static long IsInv(long startX, long startY)
        {
            long isInv = default;
            Type.Rect tempRec;
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
                            isInv = i;
                            return isInv;
                        }
                    }
                }
            }

            return -1;
        }

        public static long IsSkill(long startX, long startY)
        {
            long isSkill = default;
            Type.Rect tempRec;
            long i;

            for (i = 0L; i < Constant.MaxPlayerSkills; i++)
            {
                if (Data.Player[GameState.MyIndex].Skill[(int)i].Num >= 0)
                {
                    tempRec.Top = startY + GameState.SkillTop + (GameState.SkillOffsetY + GameState.SizeY) * (i / GameState.SkillColumns);
                    tempRec.Bottom = tempRec.Top + GameState.SizeY;
                    tempRec.Left = startX + GameState.SkillLeft + (GameState.SkillOffsetX + GameState.SizeX) * (i % GameState.SkillColumns);
                    tempRec.Right = tempRec.Left + GameState.SizeX;

                    if (GameState.CurMouseX >= tempRec.Left & GameState.CurMouseX <= tempRec.Right)
                    {
                        if (GameState.CurMouseY >= tempRec.Top & GameState.CurMouseY <= tempRec.Bottom)
                        {
                            isSkill = i;
                            return isSkill;
                        }
                    }
                }
            }

            return -1;
        }

        public static long IsBank(long startX, long startY)
        {
            byte isBank = default;
            Type.Rect tempRec;

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
                            isBank = i;
                            return isBank;
                        }
                    }
                }

            }

            return -1;

        }

        public static long IsShop(long startX, long startY)
        {
            long isShop = default;
            Type.Rect tempRec;
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
                        isShop = i;
                        return isShop;
                    }
                }
            }

            return -1;
        }

        public static long IsTrade(long startX, long startY)
        {
            long isTrade = default;
            Type.Rect tempRec;
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
                        isTrade = i;
                        return isTrade;
                    }
                }
            }

            return -1;
        }

    }
}