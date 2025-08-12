using Core.Database;
using static Core.Globals.Type;

namespace Core.Globals;

public static class Data
{
    public static Job[] Job = new Job[Constant.MaxJobs];
    public static Moral[] Moral = new Moral[Constant.MaxMorals];
    public static Item[] Item = new Item[Constant.MaxItems];
    public static Npc[] Npc = new Npc[Constant.MaxNpcs];
    public static Shop[] Shop = new Shop[Constant.MaxShops];
    public static Skill[] Skill = new Skill[Constant.MaxSkills];
    public static MapResource[] MapResource = new MapResource[Constant.MaxResources];
    public static MapResourceCache[] MyMapResource = new MapResourceCache[Constant.MaxResources];
    public static Animation[] Animation = new Animation[Constant.MaxAnimations];
    public static Map[] Map = new Map[Constant.MaxMaps];
    public static Map MyMap;
    public static Tile[,]? TempTile;
    public static MapItem[,] MapItem = new MapItem[Constant.MaxMaps, Constant.MaxMapItems];
    public static MapItem[] MyMapItem = new MapItem[Constant.MaxMapItems];
    public static MapData[] MapNpc = new MapData[Constant.MaxMaps];
    public static MapNpc[] MyMapNpc = new MapNpc[Constant.MaxMapNpcs];
    public static Bank[] Bank = new Bank[Constant.MaxPlayers];
    public static TempPlayer[] TempPlayer = new TempPlayer[Constant.MaxPlayers];
    public static Account[] Account = new Account[Constant.MaxPlayers];
    public static Player[] Player = new Player[Constant.MaxPlayers];
    public static Projectile[] Projectile = new Projectile[Constant.MaxProjectiles];
    public static MapProjectile[,] MapProjectile = new MapProjectile[Constant.MaxMaps, Constant.MaxProjectiles];
    public static PlayerInv[] TradeYourOffer = new PlayerInv[Constant.MaxInv];
    public static PlayerInv[] TradeTheirOffer = new PlayerInv[Constant.MaxInv];
    public static Party[] Party = new Party[Constant.MaxParty];
    public static Party MyParty;
    public static Resource[] Resource = new Resource[Constant.MaxResources];
    public static CharacterNameList? Char;
    public static ChatBubble[] ChatBubble = new ChatBubble[byte.MaxValue];
    public static Script Script = new();

    public static Quest[] Quests = new Quest[Constant.MaxQuests];
    public static Event[] Events = new Event[Constant.MaxEvents];
    public static Guild[] Guilds = new Guild[Constant.MaxGuilds];
    public static Weather Weather = new();

    public static ActionMsg[] ActionMsg = new ActionMsg[byte.MaxValue];
    public static Blood[] Blood = new Blood[byte.MaxValue];
    public static Chat[] Chat = new Chat[Constant.ChatLines];
    public static Tile[,]? MapTile;
    public static TileHistory[]? TileHistory;
    public static Autotile[,]? Autotile;
    public static MapEvent[]? MapEvents;
}