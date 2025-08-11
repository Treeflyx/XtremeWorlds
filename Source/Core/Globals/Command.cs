namespace Core.Globals;

public static class Command
{
    private static readonly int EquipmentCount = Enum.GetNames<Equipment>().Length;

    public static string GetAccountLogin(int index)
    {
        return Data.Account[index].Login;
    }

    public static int GetPlayerExp(int index)
    {
        return Data.Player[index].Exp;
    }

    public static int GetPlayerRawStat(int index, Stat stat)
    {
        return Data.Player[index].Stat[(int) stat];
    }

    public static string GetPlayerName(int index)
    {
        return Data.Player[index].Name;
    }

    public static int GetPlayerInvValue(int index, int invslot)
    {
        return Data.Player[index].Inv[invslot].Value;
    }

    public static int GetPlayerPoints(int index)
    {
        return Data.Player[index].Points;
    }

    public static int GetPlayerVital(int index, Vital vital)
    {
        return Data.Player[index].Vital[(int) vital];
    }

    public static int GetPlayerSprite(int index)
    {
        return Data.Player[index].Sprite;
    }

    public static int GetPlayerJob(int index)
    {
        return Data.Player[index].Job;
    }

    public static int GetPlayerMap(int index)
    {
        return Data.Player[index].Map;
    }

    public static int GetPlayerLevel(int index)
    {
        return Data.Player[index].Level;
    }

    public static int GetPlayerEquipment(int index, Equipment equipmentSlot)
    {
        return Data.Player[index].Equipment[(int) equipmentSlot];
    }

    public static int GetPlayerSkill(int index, int skillSlot)
    {
        return Data.Player[index].Skill[skillSlot].Num;
    }

    public static int GetPlayerSkillCd(int index, int skillSlot)
    {
        return Data.Player[index].Skill[skillSlot].Cd;
    }

    public static void SetPlayerLogin(int index, string login)
    {
        Data.Account[index].Login = login;
    }

    public static string GetPlayerPassword(int index)
    {
        return Data.Account[index].Password;
    }

    public static void SetPlayerPassword(int index, string password)
    {
        Data.Account[index].Password = password;
    }

    public static int GetPlayerMaxVital(int index, Vital vital)
    {
        return vital switch
        {
            Vital.Health => (int) Math.Round(100d + (Data.Player[index].Level + GetPlayerStat(index, Stat.Vitality) / 2d) * 2d),
            Vital.Mana => (int) Math.Round(50d + (Data.Player[index].Level + GetPlayerStat(index, Stat.Intelligence) / 2d) * 2d),
            Vital.Stamina => (int) Math.Round(50d + (Data.Player[index].Level + GetPlayerStat(index, Stat.Spirit) / 2d) * 2d),
            _ => 0
        };
    }

    public static int GetPlayerStat(int index, Stat stat)
    {
        int statValue = Data.Player[index].Stat[(int) stat];

        for (var i = 0; i < EquipmentCount; i++)
        {
            if (Data.Player[index].Equipment[i] >= 0 && Data.Item[Data.Player[index].Equipment[i]].AddStat[(int) stat] > 0)
            {
                statValue += Data.Item[Data.Player[index].Equipment[i]].AddStat[(int) stat];
            }
        }

        return statValue;
    }

    public static byte GetPlayerAccess(int index)
    {
        return Data.Player[index].Access;
    }

    public static int GetPlayerX(int index)
    {
        return (int) Math.Floor((double) Data.Player[index].X / 32);
    }

    public static int GetPlayerY(int index)
    {
        return (int) Math.Floor((double) Data.Player[index].Y / 32);
    }

    public static int GetPlayerRawX(int index)
    {
        return Data.Player[index].X;
    }

    public static int GetPlayerRawY(int index)
    {
        return Data.Player[index].Y;
    }

    public static byte GetPlayerDir(int index)
    {
        return Data.Player[index].Dir;
    }

    public static bool GetPlayerPk(int index)
    {
        return Data.Player[index].Pk;
    }

    public static void SetPlayerVital(int index, Vital vital, int value)
    {
        Data.Player[index].Vital[(int) vital] = value;

        if (GetPlayerVital(index, vital) > GetPlayerMaxVital(index, vital))
        {
            Data.Player[index].Vital[(int) vital] = GetPlayerMaxVital(index, vital);
        }

        if (GetPlayerVital(index, vital) < 0)
        {
            Data.Player[index].Vital[(int) vital] = 0;
        }
    }

    public static bool IsDirBlocked(byte blockvar, Direction dir)
    {
        return dir switch
        {
            Direction.UpRight =>
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Up))) != 0 ||
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Right))) != 0,

            Direction.UpLeft =>
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Up))) != 0 ||
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Left))) != 0,

            Direction.DownRight =>
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Down))) != 0 ||
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Right))) != 0,

            Direction.DownLeft =>
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Down))) != 0 ||
                (blockvar & (long) Math.Round(Math.Pow(2d, (double) Direction.Left))) != 0,

            _ => (blockvar & (long) Math.Round(Math.Pow(2d, (byte) dir))) != 0
        };
    }


    public static int GetPlayerNextLevel(int index)
    {
        return (int) Math.Round(50d / 3d * (Math.Pow(GetPlayerLevel(index) + 1, 3d) - 6d * Math.Pow(GetPlayerLevel(index) + 1, 2d) + 17 * (GetPlayerLevel(index) + 1) - 12d));
    }

    public static void SetPlayerGatherSkillLvl(int index, int skillSlot, int lvl)
    {
        Data.Player[index].GatherSkills[skillSlot].SkillLevel = lvl;
    }

    public static void SetPlayerGatherSkillExp(int index, int skillSlot, int exp)
    {
        Data.Player[index].GatherSkills[skillSlot].SkillCurExp = exp;
    }

    public static void SetPlayerGatherSkillMaxExp(int index, int skillSlot, int maxExp)
    {
        Data.Player[index].GatherSkills[skillSlot].SkillNextLvlExp = maxExp;
    }

    public static string GetResourceSkillName(ResourceSkill skillNum)
    {
        return skillNum switch
        {
            ResourceSkill.Herbalism => "Herbalism",
            ResourceSkill.Woodcutting => "Woodcutting",
            ResourceSkill.Mining => "Mining",
            ResourceSkill.Fishing => "Fishing",
            _ => string.Empty
        };
    }

    public static int GetSkillNextLevel(int index, int skillSlot)
    {
        return (int) Math.Round(50d / 3d * (Math.Pow(GetPlayerGatherSkillLvl(index, skillSlot) + 1, 3d) - 6d * Math.Pow(GetPlayerGatherSkillLvl(index, skillSlot) + 1, 2d) + 17 * (GetPlayerGatherSkillLvl(index, skillSlot) + 1) - 12d));
    }

    public static bool IsPlaying(int index)
    {
        return GetPlayerName(index).Length > 0;
    }

    public static int GetPlayerGatherSkillLvl(int index, int skillSlot)
    {
        return Data.Player[index].GatherSkills[skillSlot].SkillLevel;
    }

    public static int GetPlayerGatherSkillExp(int index, int skillSlot)
    {
        return Data.Player[index].GatherSkills[skillSlot].SkillCurExp;
    }

    public static int GetPlayerGatherSkillMaxExp(int index, int skillSlot)
    {
        return Data.Player[index].GatherSkills[skillSlot].SkillNextLvlExp;
    }

    public static void SetPlayerMap(int index, int mapNum)
    {
        Data.Player[index].Map = mapNum;
    }

    public static int GetPlayerInv(int index, int invslot)
    {
        return Data.Player[index].Inv[invslot].Num;
    }

    public static void SetPlayerName(int index, string name)
    {
        Data.Player[index].Name = name;
    }

    public static void SetPlayerJob(int index, int jobNum)
    {
        Data.Player[index].Job = (byte) jobNum;
    }

    public static void SetPlayerPoints(int index, int points)
    {
        Data.Player[index].Points = (byte) points;
    }

    public static void SetPlayerStat(int index, Stat stat, int value)
    {
        Data.Player[index].Stat[(int) stat] = (byte) value;
    }

    public static void SetPlayerInv(int index, int invSlot, int itemNum)
    {
        Data.Player[index].Inv[invSlot].Num = itemNum;
    }

    public static void SetPlayerInvValue(int index, int invslot, int itemValue)
    {
        Data.Player[index].Inv[invslot].Value = itemValue;
    }

    public static void SetPlayerAccess(int index, byte access)
    {
        Data.Player[index].Access = access;
    }

    public static void SetPlayerPk(int index, bool pk)
    {
        Data.Player[index].Pk = pk;
    }

    public static void SetPlayerX(int index, int x)
    {
        Data.Player[index].X = x;
    }

    public static void SetPlayerY(int index, int y)
    {
        Data.Player[index].Y = y;
    }

    public static void SetPlayerSprite(int index, int sprite)
    {
        Data.Player[index].Sprite = sprite;
    }

    public static void SetPlayerExp(int index, int exp)
    {
        Data.Player[index].Exp = exp;
    }

    public static void SetPlayerLevel(int index, int level)
    {
        Data.Player[index].Level = (byte) level;
    }

    public static void SetPlayerDir(int index, int dir)
    {
        Data.Player[index].Dir = (byte) dir;
    }

    public static void SetPlayerEquipment(int index, int itemNum, Equipment equipmentSlot)
    {
        Data.Player[index].Equipment[(int) equipmentSlot] = itemNum;
    }

    public static string IsEditorLocked(int index, EditorType id)
    {
        for (int i = 0; i < Constant.MaxPlayers; i++)
        {
            if (IsPlaying(i))
            {
                if (i != index)
                {
                    if (Data.TempPlayer[i].Editor == id)
                    {
                        if (GetPlayerName(i) != GetPlayerName(index))
                            return GetPlayerName(i);
                    }
                }
            }
        }

        return "";
    }

    public static int FindOpenSkill(int index)
    {
        for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
        {
            if (GetPlayerSkill(index, slot) == -1)
            {
                return slot;
            }
        }

        return -1;
    }

    public static void SetPlayerSkillCd(int index, int skillSlot, int value)
    {
        Data.Player[index].Skill[skillSlot].Cd = value;
    }

    public static bool HasSkill(int index, double skillNum)
    {
        for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
        {
            if (GetPlayerSkill(index, slot) == skillNum)
            {
                return true;
            }
        }

        return false;
    }

    public static void SetPlayerSkill(int index, int skillslot, int skillNum)
    {
        Data.Player[index].Skill[skillslot].Num = skillNum;
    }

    public static int GetBank(int index, int bankslot)
    {
        return Data.Bank[index].Item[bankslot].Num;
    }

    public static void SetBank(int index, byte bankSlot, int itemNum)
    {
        Data.Bank[index].Item[bankSlot].Num = itemNum;
    }

    public static int GetBankValue(int index, int bankSlot)
    {
        return Data.Bank[index].Item[bankSlot].Value;
    }

    public static void SetBankValue(int index, byte bankSlot, int itemValue)
    {
        Data.Bank[index].Item[bankSlot].Value = itemValue;
    }
}