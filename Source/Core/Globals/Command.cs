// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast
// ReSharper disable ConvertIfStatementToReturnStatement

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Core.Globals
{
    public static class Command
    {
        // ------------------------------------
        // Constants / helpers
        // ------------------------------------

        private const int TileSize = 32;

        // Faster and safer than Enum.GetNames().Length
        private static readonly int EquipmentCount = Enum.GetValues<Equipment>().Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ClampToByte(int v) => (byte)(v < byte.MinValue ? byte.MinValue : (v > byte.MaxValue ? byte.MaxValue : v));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Clamp(int v, int min, int max) => v < min ? min : (v > max ? max : v);

        /// <summary>Floor division for signed integers (correct for negatives).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FloorDiv(int value, int divisor)
        {
            // Equivalent to Math.Floor(value / (double)divisor) but branchy int math
            var q = value / divisor;
            var r = value % divisor;
            return (r != 0 && ((r ^ divisor) < 0)) ? (q - 1) : q;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidPlayerIndex(int index) => (uint)index < (uint)Data.Player.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidAccountIndex(int index) => (uint)index < (uint)Data.Account.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidBankIndex(int index) => (uint)index < (uint)Data.Bank.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidArrayIndex<T>(T[] arr, int idx) => arr != null && (uint)idx < (uint)arr.Length;

        [Conditional("DEBUG")]
        private static void AssertPlayer(int index)
        {
            Debug.Assert(IsValidPlayerIndex(index), $"Invalid player index {index}");
        }

        [Conditional("DEBUG")]
        private static void AssertAccount(int index)
        {
            Debug.Assert(IsValidAccountIndex(index), $"Invalid account index {index}");
        }

        [Conditional("DEBUG")]
        private static void AssertBank(int index)
        {
            Debug.Assert(IsValidBankIndex(index), $"Invalid bank index {index}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasBit(byte mask, int bit) => ((mask & (1 << bit)) != 0);

        // ------------------------------------
        // Simple getters
        // ------------------------------------

        public static string GetAccountLogin(int index)
        {
            AssertAccount(index);
            return IsValidAccountIndex(index) ? Data.Account[index].Login : string.Empty;
        }

        public static int GetPlayerExp(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Exp : 0;
        }

        public static int GetPlayerRawStat(int index, Stat stat)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var s = (int)stat;
            var arr = Data.Player[index].Stat;
            return IsValidArrayIndex(arr, s) ? arr[s] : 0;
        }

        public static string GetPlayerName(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? (Data.Player[index].Name ?? string.Empty) : string.Empty;
        }

        public static int GetPlayerInvValue(int index, int invslot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var inv = Data.Player[index].Inv;
            return IsValidArrayIndex(inv, invslot) ? inv[invslot].Value : 0;
        }

        public static int GetPlayerPoints(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Points : 0;
        }

        public static int GetPlayerVital(int index, Vital vital)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var v = (int)vital;
            var arr = Data.Player[index].Vital;
            return IsValidArrayIndex(arr, v) ? arr[v] : 0;
        }

        public static int GetPlayerSprite(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Sprite : 0;
        }

        public static int GetPlayerJob(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Job : 0;
        }

        public static int GetPlayerMap(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Map : 0;
        }

        public static int GetPlayerLevel(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Level : 0;
        }

        public static int GetPlayerEquipment(int index, Equipment equipmentSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var e = Data.Player[index].Equipment;
            var i = (int)equipmentSlot;
            return IsValidArrayIndex(e, i) ? e[i] : 0;
        }

        public static int GetPlayerSkill(int index, int skillSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return -1;

            var skills = Data.Player[index].Skill;
            return IsValidArrayIndex(skills, skillSlot) ? skills[skillSlot].Num : -1;
        }

        public static int GetPlayerSkillCd(int index, int skillSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var skills = Data.Player[index].Skill;
            return IsValidArrayIndex(skills, skillSlot) ? skills[skillSlot].Cd : 0;
        }

        public static void SetPlayerLogin(int index, string login)
        {
            AssertAccount(index);
            if (!IsValidAccountIndex(index)) return;
            Data.Account[index].Login = login ?? string.Empty;
        }

        public static string GetPlayerPassword(int index)
        {
            AssertAccount(index);
            return IsValidAccountIndex(index) ? (Data.Account[index].Password ?? string.Empty) : string.Empty;
        }

        public static void SetPlayerPassword(int index, string password)
        {
            AssertAccount(index);
            if (!IsValidAccountIndex(index)) return;
            Data.Account[index].Password = password ?? string.Empty;
        }

        // ------------------------------------
        // Derived stats
        // ------------------------------------

        public static int GetPlayerMaxVital(int index, Vital vital)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            // Use doubles for formula then clamp down to int
            int L() => GetPlayerLevel(index);
            int S(Stat st) => GetPlayerStat(index, st);

            return vital switch
            {
                Vital.Health  => (int)Math.Round(100d + (L() + S(Stat.Vitality)     / 2d) * 2d),
                Vital.Mana    => (int)Math.Round( 50d + (L() + S(Stat.Intelligence) / 2d) * 2d),
                Vital.Stamina => (int)Math.Round( 50d + (L() + S(Stat.Spirit)       / 2d) * 2d),
                _             => 0
            };
        }

        public static int GetPlayerStat(int index, Stat stat)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var sIndex = (int)stat;

            var baseStats = Data.Player[index].Stat;
            var statValue = IsValidArrayIndex(baseStats, sIndex) ? baseStats[sIndex] : 0;

            // Add equipment bonuses (safe guarded)
            var eq = Data.Player[index].Equipment;
            if (eq == null) return statValue;

            for (var i = 0; i < Math.Min(EquipmentCount, eq.Length); i++)
            {
                var itemId = eq[i];
                if (itemId < 0) continue;

                // check item bounds
                if (!IsValidArrayIndex(Data.Item, itemId)) continue;

                var add = Data.Item[itemId].AddStat;
                if (!IsValidArrayIndex(add, sIndex)) continue;

                statValue += add[sIndex];
            }

            return statValue;
        }

        // ------------------------------------
        // Accessors / position / flags
        // ------------------------------------

        public static byte GetPlayerAccess(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Access : (byte)0;
        }

        public static int GetPlayerX(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? FloorDiv(Data.Player[index].X, TileSize) : 0;
        }

        public static int GetPlayerY(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? FloorDiv(Data.Player[index].Y, TileSize) : 0;
        }

        public static int GetPlayerRawX(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].X : 0;
        }

        public static int GetPlayerRawY(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Y : 0;
        }

        public static byte GetPlayerDir(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) ? Data.Player[index].Dir : (byte)0;
        }

        public static bool GetPlayerPk(int index)
        {
            AssertPlayer(index);
            return IsValidPlayerIndex(index) && Data.Player[index].Pk;
        }

        public static void SetPlayerVital(int index, Vital vital, int value)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var vidx = (int)vital;
            var arr = Data.Player[index].Vital;
            if (!IsValidArrayIndex(arr, vidx)) return;

            arr[vidx] = value;

            var max = GetPlayerMaxVital(index, vital);
            if (arr[vidx] > max) arr[vidx] = max;
            if (arr[vidx] < 0)   arr[vidx] = 0;
        }

        /// <summary>Block‑map bit test with diagonal logic. Replaced slow Math.Pow with bit ops.</summary>
        public static bool IsDirBlocked(byte blockvar, Direction dir)
        {
            int m = blockvar; // widen to int for bit math

            return dir switch
            {
                Direction.UpRight  => HasBit((byte)m, (int)Direction.Up)   || HasBit((byte)m, (int)Direction.Right),
                Direction.UpLeft   => HasBit((byte)m, (int)Direction.Up)   || HasBit((byte)m, (int)Direction.Left),
                Direction.DownRight=> HasBit((byte)m, (int)Direction.Down) || HasBit((byte)m, (int)Direction.Right),
                Direction.DownLeft => HasBit((byte)m, (int)Direction.Down) || HasBit((byte)m, (int)Direction.Left),
                _                  => HasBit((byte)m, (int)dir)
            };
        }

        // ------------------------------------
        // XP curves
        // ------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int NextLevelExpFor(int level)
        {
            // E(n+1) = (50/3) * ( (n+1)^3 - 6(n+1)^2 + 17(n+1) - 12 )
            var n1 = level + 1;
            var d = (50d / 3d) * (Math.Pow(n1, 3d) - 6d * Math.Pow(n1, 2d) + 17d * n1 - 12d);
            return (int)Math.Round(d);
        }

        public static int GetPlayerNextLevel(int index)
        {
            return NextLevelExpFor(GetPlayerLevel(index));
        }

        // ------------------------------------
        // Gathering skills
        // ------------------------------------

        public static void SetPlayerGatherSkillLvl(int index, int skillSlot, int lvl)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var gs = Data.Player[index].GatherSkills;
            if (!IsValidArrayIndex(gs, skillSlot)) return;

            gs[skillSlot].SkillLevel = lvl;
        }

        public static void SetPlayerGatherSkillExp(int index, int skillSlot, int exp)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var gs = Data.Player[index].GatherSkills;
            if (!IsValidArrayIndex(gs, skillSlot)) return;

            gs[skillSlot].SkillCurExp = exp;
        }

        public static void SetPlayerGatherSkillMaxExp(int index, int skillSlot, int maxExp)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var gs = Data.Player[index].GatherSkills;
            if (!IsValidArrayIndex(gs, skillSlot)) return;

            gs[skillSlot].SkillNextLvlExp = maxExp;
        }

        public static string GetResourceSkillName(ResourceSkill skillNum)
        {
            return skillNum switch
            {
                ResourceSkill.Herbalism   => "Herbalism",
                ResourceSkill.Woodcutting => "Woodcutting",
                ResourceSkill.Mining      => "Mining",
                ResourceSkill.Fishing     => "Fishing",
                _                         => string.Empty
            };
        }

        public static int GetSkillNextLevel(int index, int skillSlot)
        {
            return NextLevelExpFor(GetPlayerGatherSkillLvl(index, skillSlot));
        }

        public static bool IsPlaying(int index)
        {
            return GetPlayerName(index).Length > 0;
        }

        public static int GetPlayerGatherSkillLvl(int index, int skillSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var gs = Data.Player[index].GatherSkills;
            return IsValidArrayIndex(gs, skillSlot) ? gs[skillSlot].SkillLevel : 0;
        }

        public static int GetPlayerGatherSkillExp(int index, int skillSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var gs = Data.Player[index].GatherSkills;
            return IsValidArrayIndex(gs, skillSlot) ? gs[skillSlot].SkillCurExp : 0;
        }

        public static int GetPlayerGatherSkillMaxExp(int index, int skillSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return 0;

            var gs = Data.Player[index].GatherSkills;
            return IsValidArrayIndex(gs, skillSlot) ? gs[skillSlot].SkillNextLvlExp : 0;
        }

        // ------------------------------------
        // Map / inventory / identity
        // ------------------------------------

        public static void SetPlayerMap(int index, int mapNum)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Map = mapNum;
        }

        public static int GetPlayerInv(int index, int invslot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return -1;

            var inv = Data.Player[index].Inv;
            return IsValidArrayIndex(inv, invslot) ? inv[invslot].Num : -1;
        }

        public static void SetPlayerName(int index, string name)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Name = name ?? string.Empty;
        }

        public static void SetPlayerJob(int index, int jobNum)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Job = Clamp(jobNum, byte.MinValue, byte.MaxValue);
        }

        public static void SetPlayerPoints(int index, int points)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Points = Clamp(points, byte.MinValue, byte.MaxValue);
        }

        public static void SetPlayerStat(int index, Stat stat, int value)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var s = (int)stat;
            var arr = Data.Player[index].Stat;
            if (!IsValidArrayIndex(arr, s)) return;

            arr[s] = Clamp(value, byte.MinValue, byte.MaxValue);
        }

        public static void SetPlayerInv(int index, int invSlot, int itemNum)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var inv = Data.Player[index].Inv;
            if (!IsValidArrayIndex(inv, invSlot)) return;

            inv[invSlot].Num = itemNum;
        }

        public static void SetPlayerInvValue(int index, int invslot, int itemValue)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var inv = Data.Player[index].Inv;
            if (!IsValidArrayIndex(inv, invslot)) return;

            inv[invslot].Value = itemValue;
        }

        public static void SetPlayerAccess(int index, byte access)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Access = access;
        }

        public static void SetPlayerPk(int index, bool pk)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Pk = pk;
        }

        public static void SetPlayerX(int index, int x)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].X = x;
        }

        public static void SetPlayerY(int index, int y)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Y = y;
        }

        public static void SetPlayerSprite(int index, int sprite)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Sprite = sprite;
        }

        public static void SetPlayerExp(int index, int exp)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Exp = exp;
        }

        public static void SetPlayerLevel(int index, int level)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Level = Clamp(level, byte.MinValue, byte.MaxValue);
        }

        public static void SetPlayerDir(int index, int dir)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;
            Data.Player[index].Dir = ClampToByte(dir);
        }

        public static void SetPlayerEquipment(int index, int itemNum, Equipment equipmentSlot)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var e = Data.Player[index].Equipment;
            var i = (int)equipmentSlot;
            if (!IsValidArrayIndex(e, i)) return;

            e[i] = itemNum;
        }

        // ------------------------------------
        // Editor / skills management
        // ------------------------------------

        public static string IsEditorLocked(int index, EditorType id)
        {
            var myName = GetPlayerName(index);

            for (int i = 0; i < Constant.MaxPlayers; i++)
            {
                if (i == index) continue;
                if (!IsPlaying(i)) continue;

                if (Data.TempPlayer[i].Editor == id)
                {
                    var name = GetPlayerName(i);
                    if (!name.Equals(myName, StringComparison.Ordinal))
                        return name;
                }
            }

            return string.Empty;
        }

        public static int FindOpenSkill(int index)
        {
            for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
            {
                if (GetPlayerSkill(index, slot) == -1)
                    return slot;
            }

            return -1;
        }

        public static void SetPlayerSkillCd(int index, int skillSlot, int value)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var skills = Data.Player[index].Skill;
            if (!IsValidArrayIndex(skills, skillSlot)) return;

            skills[skillSlot].Cd = value;
        }

        // Kept signature (double), but compare as int to avoid floating issues.
        public static bool HasSkill(int index, double skillNum)
        {
            var target = (int)Math.Round(skillNum);
            for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
            {
                if (GetPlayerSkill(index, slot) == target)
                    return true;
            }

            return false;
        }

        public static void SetPlayerSkill(int index, int skillslot, int skillNum)
        {
            AssertPlayer(index);
            if (!IsValidPlayerIndex(index)) return;

            var skills = Data.Player[index].Skill;
            if (!IsValidArrayIndex(skills, skillslot)) return;

            skills[skillslot].Num = skillNum;
        }

        // ------------------------------------
        // Bank
        // ------------------------------------

        public static int GetBank(int index, int bankslot)
        {
            AssertBank(index);
            if (!IsValidBankIndex(index)) return -1;

            var items = Data.Bank[index].Item;
            return IsValidArrayIndex(items, bankslot) ? items[bankslot].Num : -1;
        }

        public static void SetBank(int index, byte bankSlot, int itemNum)
        {
            AssertBank(index);
            if (!IsValidBankIndex(index)) return;

            var items = Data.Bank[index].Item;
            if (!IsValidArrayIndex(items, bankSlot)) return;

            items[bankSlot].Num = itemNum;
        }

        public static int GetBankValue(int index, int bankSlot)
        {
            AssertBank(index);
            if (!IsValidBankIndex(index)) return 0;

            var items = Data.Bank[index].Item;
            return IsValidArrayIndex(items, bankSlot) ? items[bankSlot].Value : 0;
        }

        public static void SetBankValue(int index, byte bankSlot, int itemValue)
        {
            AssertBank(index);
            if (!IsValidBankIndex(index)) return;

            var items = Data.Bank[index].Item;
            if (!IsValidArrayIndex(items, bankSlot)) return;

            items[bankSlot].Value = itemValue;
        }
    }
}
