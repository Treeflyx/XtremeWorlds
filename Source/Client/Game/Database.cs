using Client.Net;
using Core;
using Core.Globals;
using Type = Core.Globals.Type;

namespace Client
{

    public class Database
    {
        #region Blood

        public static void ClearBlood()
        {
            for (int i = 0; i < byte.MaxValue; i++)
                Data.Blood[i].Timer = 0;
        }

        #endregion

        #region Npc

        public static void ClearNpcs()
        {
            Data.Npc = new Type.Npc[Constant.MaxNpcs];

            for (int i = 0; i < Constant.MaxNpcs; i++)
                ClearNpc(i);

        }

        public static void ClearNpc(int index)
        {
            int statCount = Enum.GetValues(typeof(Stat)).Length;
            Data.Npc[index].AttackSay = "";
            Data.Npc[index].Name = "";
            Data.Npc[index] = default;
            Data.Npc[index].Stat = new byte[statCount];
            Data.Npc[index].DropChance = new int[6];
            Data.Npc[index].DropItem = new int[6];
            Data.Npc[index].DropItemValue = new int[6];
            Data.Npc[index].Skill = new byte[7];
            GameState.NpcLoaded[index] = 0;
        }

        public static void StreamNpc(int npcNum)
        {
            if (npcNum >= 0 && string.IsNullOrEmpty(Data.Npc[npcNum].Name) && GameState.NpcLoaded[npcNum] == 0)
            {
                GameState.NpcLoaded[(int)npcNum] = 1;
                Sender.SendRequestNpc(npcNum);
            }
        }

        #endregion

        #region Jobs
        public static void ClearJobs()
        {
            for (int i = 0; i < Constant.MaxJobs; i++)
                ClearJob(i);
        }

        public static void ClearJob(int index)
        {
            var statCount = System.Enum.GetValues(typeof(Stat)).Length;
            Data.Job[index] = default;
            Data.Job[index].Stat = new int[statCount];
            Data.Job[index].Name = "";
            Data.Job[index].Desc = "";
            Data.Job[index].StartItem = new int[Constant.MaxStartItems];
            Data.Job[index].StartValue = new int[Constant.MaxStartItems];
            Data.Job[index].MaleSprite = 1;
            Data.Job[index].FemaleSprite = 1;
        }
        #endregion

        #region Skills

        public static void ClearSkills()
        {
            int i;

            for (i = 0; i < Constant.MaxSkills; i++)
                ClearSkill(i);

        }

        public static void ClearSkill(int index)
        {
            Data.Skill[index] = default;
            Data.Skill[index].Name = "";
            Data.Skill[index].JobReq = -1;
            GameState.SkillLoaded[index] = 0;
        }

        public static void StreamSkill(int skillNum)
        {
            if (skillNum >= 0 && string.IsNullOrEmpty(Data.Skill[skillNum].Name) && GameState.SkillLoaded[skillNum] == 0)
            {
                GameState.SkillLoaded[skillNum] = 1;
                Sender.SendRequestSkill(skillNum);
            }
        }
        #endregion
    }
}