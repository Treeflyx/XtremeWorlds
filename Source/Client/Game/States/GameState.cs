using Core;
using System.Reflection.Metadata;
using Core.Globals;
using Color = Microsoft.Xna.Framework.Color;
using Constant = Core.Globals.Constant;
using Point = Microsoft.Xna.Framework.Point;
using Type = Core.Globals.Type;

namespace Client
{

    public class GameState
    {
        public GameState()
        {
        }

        public static int ResolutionHeight;
        public static int ResolutionWidth;

        public static bool MapAnim;

        // Global dialogue index
        public static string? DiaHeader;
        public static string? DiaBody;
        public static string? DiaBody2;
        public static long DiaIndex;
        public static long DiaData1;
        public static long DiaData2;
        public static long DiaData3;
        public static long DiaData4;
        public static long DiaData5;
        public static string? DiaDataString;
        public static byte DiaStyle;

        // shop
        public static long ShopSelectedSlot;
        public static long ShopSelectedItem;
        public static bool ShopIsSelling;

        // right click menu
        public static long PlayerMenuIndex;

        // description
        public static byte DescType;
        public static long DescItem;
        public static byte DescLastType;
        public static long DescLastItem;
        public static Type.Text[]? Description;

        // New char
        public static long NewCharSprite;
        public static long NewCharJob;
        public static long NewCnarGender;

        // chars
        public static string?[] CharName = new string?[Constant.MaxChars];
        public static long[] CharSprite = new long[(Constant.MaxChars)];
        public static long[] CharAccess = new long[(Constant.MaxChars)];
        public static long[] CharJob = new long[(Constant.MaxChars)];
        public static byte CharNum;

        // elastic bars
        public static long[] BarWidthNpcHp = new long[(Constant.MaxMapNpcs)];
        public static long[] BarWidthPlayerHp = new long[Constant.MaxPlayers];
        public static long[] BarWidthPlayerSp = new long[Constant.MaxPlayers];
        public static long[] BarWidthNpcHpMax = new long[(Constant.MaxMapNpcs)];
        public static long[] BarWidthPlayerHpMax = new long[Constant.MaxPlayers];
        public static long[] BarWidthPlayerSpMax = new long[Constant.MaxPlayers];
        public static long BarWidthGuiHp;
        public static long BarWidthGuiSp;
        public static long BarWidthGuiExp;
        public static long BarWidthGuiHpMax;
        public static long BarWidthGuiSpMax;
        public static long BarWidthGuiExpMax;

        public static int CurrentEvents;

        // Directional blocking
        public static byte[] DirArrowX = new byte[4];
        public static byte[] DirArrowY = new byte[4];

        public static bool UseFade;
        public static int FadeType;
        public static int FadeAmount;
        public static int FlashTimer;

        // Targetting
        public static int MyTarget;
        public static int MyTargetType;

        // Chat bubble
        public static int ChatBubbleindex;

    public static string? ChatShowLine;

    public static string?[] MapNames = new string?[Constant.MaxMaps];

        // chat
        public static bool InSmallChat;
        public static long ActChatHeight;
        public static long ActChatWidth;
        public static bool ChatButtonUp;
        public static bool ChatButtonDown;
        public static long ChatScroll;
        public static long ChatHighIndex;

        public static int EditorTileX;
        public static int EditorTileY;
        public static int EditorTileWidth;
        public static int EditorTileHeight;
        public static int EditorWarpMap;
        public static int EditorWarpX;
        public static int EditorWarpY;
        public static int EditorShop;
        public static int EditorAnimation;
        public static byte EditorAttribute;
        public static Point EditorTileSelStart;
        public static Point EditorTileSelEnd;
        public static bool CopyMap;
        public static byte TmpMaxX;
        public static byte TmpMaxY;

        // Player variables
        public static int MyIndex; // Index of actual player

        public static bool InBank;

        public static int SkillBuffer;
        public static int SkillBufferTimer;
        public static int StunDuration;
        public static int NextlevelExp;

        // Stops movement when updating a map
        public static bool CanMoveNow;

        // Controls main gameloop
        public static bool InGame;
        public static bool InMenu;

        public static bool MapData;
        public static bool PlayerData;

        // Draw map name location
        public static float DrawLocX = 10f;
        public static float DrawLocY = 0f;
        public static Color DrawMapNameColor;

        // Game direction vars
        public static bool DirUp;
        public static bool DirDown;
        public static bool DirLeft;
        public static bool DirRight;

        // Used to freeze controls when getting a new map
        public static bool GettingMap;

        // Used to check if FPS needs to be drawn
        public static bool Bfps;
        public static bool Blps;
        public static bool BLoc;

        // FPS and Time-based movement vars
        public static int ElapsedTime;

        // Mouse cursor tile location
        public static int CurX;
        public static int CurY;
        public static int CurMouseX;
        public static int CurMouseY;
        public static bool Info;

        // Game editors
        public static EditorType MyEditorType;
        public static int EditorIndex;
        public static bool AdminPanel;

        // Spawn
        public static int SpawnNpcNum;
        public static int SpawnNpcDir;

        // Items
        public static int ItemEditorNum;
        public static int ItemEditorValue;

        // Resources
        public static int ResourceEditorNum;

        // Used for map editor heal & trap & slide tiles
        public static int MapEditorHealType;
        public static int MapEditorHealAmount;
        public static int MapEditorSlideDir;

        public static Type.Rect Camera;
        public static Type.Rect TileView;

        // Pinging
        public static int PingStart;
        public static int PingEnd;
        public static int Ping;

        // Indexing
        public static byte ActionMsgIndex;
        public static byte BloodIndex;

    public static byte[]? TempMapData;

        public static bool ShakeTimerEnabled;
        public static int ShakeTimer;
        public static byte ShakeCount;
        public static byte LastDir;

        public static bool ShowAnimLayers;
        public static int ShowAnimTimer;

        // Stream Content
        public static int[] ItemLoaded = new int[Constant.MaxItems];
        public static int[] NpcLoaded = new int[Constant.MaxNpcs];
        public static int[] ResourceLoaded = new int[Constant.MaxResources];
        public static int[] AnimationLoaded = new int[Constant.MaxResources];
        public static int[] SkillLoaded = new int[Constant.MaxSkills];
        public static int[] ShopLoaded = new int[Constant.MaxShops];
        public static int[] MoralLoaded = new int[(Constant.MaxMorals)];
        public static int[] ProjectileLoaded = new int[(Constant.MaxProjectiles)];

        public static int[] AnimEditorFrame = new int[2];
        public static int[] AnimEditorTimer = new int[2];

        public static double CurrentCameraX;
        public static double CurrentCameraY;

        // Number of graphic files
        public static int NumTileSets;
        public static int NumCharacters;
        public static int NumPaperdolls;
        public static int NumItems;
        public static int NumResources;
        public static int NumAnimations;
        public static int NumSkills;
        public static int NumFogs;
        public static int NumEmotes;
        public static int NumPanoramas;
        public static int NumParallax;
        public static int NumPictures;
        public static int NumInterface;
        public static int NumGradients;
        public static int NumDesigns;

        public static bool VbKeyRight;
        public static bool VbKeyLeft;
        public static bool VbKeyUp;
        public static bool VbKeyDown;
        public static bool VbKeyShift;
        public static bool VbKeyControl;
        public static bool VbKeyAlt;
        public static bool VbKeyEnter;

        public static int LastLeftClickTime;
        public const int DoubleClickTImer = 500; // Time in milliseconds for double-click detection
        public static int ClickCount;

        public const int ChatBubbleWidth = 300;

        public const long ChatTimer = 20000L;

        public const int EffectTypeFadein = 1;
        public const int EffectTypeFadeout = 2;
        public const int EffectTypeFlash = 3;
        public const int EffectTypeFog = 4;
        public const int EffectTypeWeather = 5;
        public const int EffectTypeTint = 6;

        // Bank constants
        public const long BankTop = 28L;
        public const long BankLeft = 9L;
        public const long BankOffsetY = 6L;
        public const long BankOffsetX = 6L;
        public const long BankColumns = 10L;

        // Inventory constants
        public const long InvTop = 28L;
        public const long InvLeft = 9L;
        public const long InvOffsetY = 6L;
        public const long InvOffsetX = 6L;
        public const long InvColumns = 5L;

        // Character consts
        public const long EqTop = 315L;
        public const long EqLeft = 10L;
        public const long EqOffsetX = 8L;
        public const long EqColumns = 4L;

        // Skill constants
        public const long SkillTop = 28L;
        public const long SkillLeft = 9L;
        public const long SkillOffsetY = 6L;
        public const long SkillOffsetX = 6L;
        public const long SkillColumns = 5L;

        // Hotbar constants
        public const long HotbarTop = 0L;
        public const long HotbarLeft = 8L;
        public const long HotbarOffsetX = 40L;

        // Shop constants
        public const long ShopTop = 28L;
        public const long ShopLeft = 9L;
        public const long ShopOffsetY = 6L;
        public const long ShopOffsetX = 6L;
        public const long ShopColumns = 7L;

        // Trade
        public const long TradeTop = 0L;
        public const long TradeLeft = 0L;
        public const long TradeOffsetY = 6L;
        public const long TradeOffsetX = 6L;
        public const long TradeColumns = 5L;

        // Gfx Path and variables
        public const string GfxExt = ".png";

        public static bool MapGrid;
        public static bool EyeDropper;
        public static int TileHistoryIndex;
        public static int TileHistoryHighIndex;
        public static bool HideLayers;

        // Sprite, item, skill size constants
        public const int SizeX = 32;
        public const int SizeY = 32;

        // Map
        public const int MaxTileHistory = 500;
        public const byte TileSize = 32; // Tile size is 32x32 pixels

        // Autotiles
        public const byte AutoInner = 1;

        public const byte AutoOuter = 2;
        public const byte AutoHorizontal = 3;
        public const byte AutoVertical = 4;
        public const byte AutoFill = 5;

        // Autotile Type
        public const byte AutotileNone = 0;

        public const byte AutotileNormal = 1;
        public const byte AutotileFake = 2;
        public const byte AutotileAnim = 3;
        public const byte AutotileCliff = 4;
        public const byte AutotileWaterfall = 5;

        // Rendering
        public const int RenderStateNone = 0;

        public const int RenderStateNormal = 0;
        public const int RenderStateAutotile = 2;

        // Map animations
        public static int WaterfallFrame;

        public static int AutoTileFrame;

        public static int NumProjectiles;
        public static bool InitProjectileEditor;

        public static int ResourceIndex;
        public static bool ResourcesInit;

        public static Type.WeatherParticle[] WeatherParticle = new Type.WeatherParticle[Constant.MaxWeatherParticles];

        public static int FogOffsetX;
        public static int FogOffsetY;

        public static int CurrentWeather;
        public static int CurrentWeatherIntensity;
        public static int CurrentFog;
        public static int CurrentFogSpeed;
        public static int CurrentFogOpacity;
        public static int CurrentTintR;
        public static int CurrentTintG;
        public static int CurrentTintB;
        public static int CurrentTintA;
        public static int DrawThunder;

        public static int InShop; // is the player in a shop?
        public static byte ShopAction; // stores the current shop action

        public static int MapEditorTab;
        public static int CurLayer;
        public static int CurAutotileType;
        public static int CurTileset;

        // Thread-safe mirrors of editor tool radio button states (avoids cross-thread UI access)
        public static bool OptInfo;         // Info mode
        public static bool OptBlocked;      // Blocked tile
        public static bool OptWarp;         // Warp attribute
        public static bool OptItem;         // Item attribute
        public static bool OptNpcAvoid;     // NPC avoid attribute
        public static bool OptResource;     // Resource attribute
        public static bool OptNpcSpawn;     // NPC spawn attribute
        public static bool OptShop;         // Shop attribute
        public static bool OptBank;         // Bank attribute
        public static bool OptHeal;         // Heal attribute
        public static bool OptTrap;         // Trap attribute
        public static bool OptAnimation;    // Animation attribute
        public static bool OptNoCrossing;   // No Crossing attribute

        // Editors
        public static bool InitEditor;
        public static bool InitMapEditor;
        public static bool InitItemEditor;
        public static bool InitResourceEditor;
        public static bool InitNpcEditor;
        public static bool InitSkillEditor;
        public static bool InitShopEditor;
        public static bool InitAnimationEditor;
        public static bool InitJobEditor;
        public static bool InitMoralEditor;
        public static bool InitAdminForm;
        public static bool InitMapReport;
        public static bool InitEventEditor;
        public static bool InitScriptEditor;

        // Editor edited items array
        public static bool[] ItemChanged = new bool[Constant.MaxItems];
        public static bool[] NpcChanged = new bool[Constant.MaxNpcs];
        public static bool[] ResourceChanged = new bool[Constant.MaxResources];
        public static bool[] AnimationChanged = new bool[Constant.MaxAnimations];
        public static bool[] SkillChanged = new bool[Constant.MaxSkills];
        public static bool[] ShopChanged = new bool[Constant.MaxShops];
        public static bool[] JobChanged = new bool[(Constant.MaxJobs)];
        public static bool[] MoralChanged = new bool[(Constant.MaxMorals)];
        public static bool[] ProjectileChanged = new bool[Constant.MaxProjectiles];
    }
}