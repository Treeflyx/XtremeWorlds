using System.Xml.Serialization;

namespace Core.Configurations;

[XmlRoot("Locales")]
public sealed class LocaleData
{
    [XmlElement("Load")]
    public List<LocaleItem> Load { get; set; } = [];

    [XmlElement("MainMenu")]
    public List<LocaleItem> MainMenu { get; set; } = [];

    [XmlElement("Game")]
    public List<LocaleItem> Game { get; set; } = [];

    [XmlElement("Chat")]
    public List<LocaleItem> Chat { get; set; } = [];

    [XmlElement("ItemDescription")]
    public List<LocaleItem> ItemDescription { get; set; } = [];

    [XmlElement("SkillDescription")]
    public List<LocaleItem> SkillDescription { get; set; } = [];

    [XmlElement("Crafting")]
    public List<LocaleItem> Crafting { get; set; } = [];

    [XmlElement("Trade")]
    public List<LocaleItem> Trade { get; set; } = [];

    [XmlElement("Events")]
    public List<LocaleItem> Events { get; set; } = [];

    [XmlElement("Quest")]
    public List<LocaleItem> Quest { get; set; } = [];

    [XmlElement("Character")]
    public List<LocaleItem> Character { get; set; } = [];

    [XmlIgnore]
    public IEnumerable<LocaleItem> AllItems =>
        Load.Concat(MainMenu)
            .Concat(Game)
            .Concat(Chat)
            .Concat(ItemDescription)
            .Concat(SkillDescription)
            .Concat(Crafting)
            .Concat(Trade)
            .Concat(Events)
            .Concat(Quest)
            .Concat(Character);

    public static LocaleData CreateDefaultEnglish()
    {
        return new LocaleData
        {
            Load =
            [
                new LocaleItem("Loading", "Loading..."),
                new LocaleItem("Graphics", "Initializing Graphics.."),
                new LocaleItem("Network", "Initializing Network..."),
                new LocaleItem("Starting", "Starting Game...")
            ],
            MainMenu =
            [
                new LocaleItem("ServerStatus", "Server Status:"),
                new LocaleItem("ServerOnline", "Online"),
                new LocaleItem("ServerReconnect", "Reconnecting..."),
                new LocaleItem("ServerOffline", "Offline"),
                new LocaleItem("ButtonPlay", "Play"),
                new LocaleItem("ButtonRegister", "Register"),
                new LocaleItem("ButtonCredits", "Credits"),
                new LocaleItem("ButtonExit", "Exit"),
                new LocaleItem("NewsHeader", "Latest News"),
                new LocaleItem("News",
                    """
                    Welcome To the XtremeWorlds.
                    This is a free open-source C# game engine!
                    For help or support please visit our site at
                    https://xtremeworlds.com/.
                    """),
                new LocaleItem("Login", "Login"),
                new LocaleItem("LoginName", "Name: "),
                new LocaleItem("LoginPass", "Password: "),
                new LocaleItem("LoginCheckBox", "Save Password?"),
                new LocaleItem("LoginButton", "Submit"),
                new LocaleItem("NewCharacter", "Create Character"),
                new LocaleItem("NewCharacterName", "Name: "),
                new LocaleItem("NewCharacterClass", "Class: "),
                new LocaleItem("NewCharacterGender", "Gender: "),
                new LocaleItem("NewCharacterMale", "Male"),
                new LocaleItem("NewCharacterFemale", "Female"),
                new LocaleItem("NewCharacterSprite", "Sprite"),
                new LocaleItem("NewCharacterButton", "Submit"),
                new LocaleItem("UseCharacter", "Character Selection"),
                new LocaleItem("UseCharacterNew", "New Character"),
                new LocaleItem("UseCharacterUse", "Use Character"),
                new LocaleItem("UseCharacterDel", "Delete Character"),
                new LocaleItem("Register", "Registration"),
                new LocaleItem("RegisterName", "Username: "),
                new LocaleItem("RegisterPass1", "Password: "),
                new LocaleItem("RegisterPass2", "Retype Password: "),
                new LocaleItem("Credits", "Credits"),
                new LocaleItem("StringLegal", "You cannot use high ASCII characters In your name, please re-enter."),
                new LocaleItem("SendLogin", "Connected, sending login information..."),
                new LocaleItem("SendNewCharacter", "Connected, sending character data..."),
                new LocaleItem("SendRegister", "Connected, sending registration information..."),
                new LocaleItem("ConnectToServer", "Connecting to Server...( {0} )")
            ],
            Game =
            [
                new LocaleItem("Time", "Time: "),
                new LocaleItem("Fps", "Fps: "),
                new LocaleItem("Lps", "Lps: "),
                new LocaleItem("Ping", "Ping: "),
                new LocaleItem("PingSync", "Sync"),
                new LocaleItem("PingLocal", "Local"),
                new LocaleItem("MapReceive", "Receiving map..."),
                new LocaleItem("DataReceive", "Receiving game data..."),
                new LocaleItem("MapCurMap", "Map # {0}"),
                new LocaleItem("MapCurLoc", "Loc() x: {0} y: {1}"),
                new LocaleItem("MapLoc", "Cur Loc x: {0} y: {1}"),
                new LocaleItem("Fullscreen", "Please restart the client for the changes to take effect."),
                new LocaleItem("InvalidMap", "Invalid map index."),
                new LocaleItem("AccessDenied", "Access Denied!")
            ],
            Chat =
            [
                new LocaleItem("Emote", "Usage : /emote [1-11]"),
                new LocaleItem("Info", "Usage : /info [player]"),
                new LocaleItem("Party", "Usage : /party [player]"),
                new LocaleItem("Trade", "Usage : /trade [player]"),
                new LocaleItem("PlayerMsg", "Usage : ![player] [message]"),
                new LocaleItem("InvalidCmd", "Not a valid command!"),
                new LocaleItem("Help1", "Social Commands : "),
                new LocaleItem("Help2", "'[message] = Global Message"),
                new LocaleItem("Help3", "-[message] = Party Message"),
                new LocaleItem("Help4", "![player] [message] = Player Message"),
                new LocaleItem("Help5", "@[message] = Admin Message"),
                new LocaleItem("Help6",
                    """
                    Available Commands: /help, /info, 
                    /fps, /lps, /stats, /trade, 
                    /party, /join, /leave
                    """),
                new LocaleItem("AdminGblMsg", "''msghere = Global Admin Message"),
                new LocaleItem("AdminPvtMsg", "= msghere = Private Admin Message"),
                new LocaleItem("Admin1", "Social Commands:"),
                new LocaleItem("Admin2",
                    """
                    Available Commands: /admin, /who, /access, /loc, 
                    /warpmeto, /warptome, /warpto, 
                    /sprite, /mapreport, /kick, 
                    /ban, /respawn, /welcome,
                    /editmap, /edititem, /editresource,
                    /editskill, /editshop /editprojectile,
                    /editnpc, /editjob, /editscript, /acp
                    """),
                new LocaleItem("Welcome", "Usage : /welcome [message]"),
                new LocaleItem("Access", "Usage : /access [player] [access]"),
                new LocaleItem("Sprite", "Usage : /sprite [index]"),
                new LocaleItem("Kick", "Usage : /kick [player]"),
                new LocaleItem("Ban", "Usage : /ban [player]"),
                new LocaleItem("WarpMeTo", "Usage : /warpmeto [player]"),
                new LocaleItem("WarpToMe", "Usage : /warptome [player]"),
                new LocaleItem("WarpTo", "Usage : /warpto [map index]")
            ],
            ItemDescription =
            [
                new LocaleItem("NotAvailable", "Not Available"),
                new LocaleItem("None", "None"),
                new LocaleItem("Seconds", "Seconds"),
                new LocaleItem("Currency", "Currency"),
                new LocaleItem("CommonEvent", "Event"),
                new LocaleItem("Potion", "Potion"),
                new LocaleItem("Skill", "Skill"),
                new LocaleItem("Weapon", "Weapon"),
                new LocaleItem("Armor", "Armor"),
                new LocaleItem("Helmet", "Helmet"),
                new LocaleItem("Shield", "Shield"),
                new LocaleItem("Shoes", "Shoes"),
                new LocaleItem("Gloves", "Gloves"),
                new LocaleItem("Amount", "Amount : "),
                new LocaleItem("Restore", "Restore Amount : "),
                new LocaleItem("Damage", "Damage : "),
                new LocaleItem("Defense", "Defense : ")
            ],
            SkillDescription =
            [
                new LocaleItem("No", "No"),
                new LocaleItem("None", "None"),
                new LocaleItem("Warp", "Warp"),
                new LocaleItem("Tiles", "Tiles"),
                new LocaleItem("SelfCast", "Self-Cast"),
                new LocaleItem("Gain", "Regen : "),
                new LocaleItem("GainHp", "Regen HP"),
                new LocaleItem("GainMp", "Regen MP"),
                new LocaleItem("Lose", "Syphon : "),
                new LocaleItem("LoseHp", "Syphon HP"),
                new LocaleItem("LoseMp", "Syphon MP")
            ],
            Crafting =
            [
                new LocaleItem("NotEnough", "Not enough materials!"),
                new LocaleItem("NotSelected", "Nothing selected")
            ],
            Trade =
            [
                new LocaleItem("Request", "{0} is requesting to trade."),
                new LocaleItem("Timeout", "You took too long to decide. Please try again."),
                new LocaleItem("Value", "Total Value : {0}g"),
                new LocaleItem("StatusOther", "Other player confirmed offer."),
                new LocaleItem("StatusSelf", "You confirmed the offer.")
            ],
            Events = [new LocaleItem("OptContinue", "- Continue -")],
            Quest =
            [
                new LocaleItem("Cancel", "Cancel Started"),
                new LocaleItem("Started", "Quest Started"),
                new LocaleItem("Completed", "Quest Completed"),
                new LocaleItem("Slay", "Defeat {0}/{1} {2}."),
                new LocaleItem("Collect", "Collect {0}/{1} {2}."),
                new LocaleItem("Talk", "Go talk To {0}."),
                new LocaleItem("Reach", "Go To {0}."),
                new LocaleItem("TurnIn", "Give {0} the {1} {2}/{3} they requested."),
                new LocaleItem("Kill", "Defeat {0}/{1} Players In Battle."),
                new LocaleItem("Gather", "Gather {0}/{1} {2}."),
                new LocaleItem("Fetch", "Fetch {0} X {1} from {2}.")
            ],
            Character =
            [
                new LocaleItem("PName", "Name: "),
                new LocaleItem("JobType", "Job: "),
                new LocaleItem("Level", "Lv: "),
                new LocaleItem("Exp", "Exp: "),
                new LocaleItem("StatsLabel", "Stats:"),
                new LocaleItem("Strength", "Strength: "),
                new LocaleItem("Endurance", "Endurance: "),
                new LocaleItem("Vitality", "Vitality: "),
                new LocaleItem("Intelligence", "Intelligence: "),
                new LocaleItem("Luck", "Luck: "),
                new LocaleItem("Spirit", "Spirit: "),
                new LocaleItem("Points", "Points Available: "),
                new LocaleItem("SkillLabel", "Skills:")
            ]
        };
    }
}