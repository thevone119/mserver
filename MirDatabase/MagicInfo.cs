﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Server.MirEnvir;
using S = ServerPackets;
using System.Data.SQLite;
using System.Data.Common;

namespace Server.MirDatabase
{
    //魔法技能啊
    public class MagicInfo
    {
        public string Name;
        public Spell Spell;//主键
        public byte BaseCost, LevelCost, Icon;
        public byte Level1, Level2, Level3;
        public ushort Need1, Need2, Need3;
        public uint DelayBase = 1800, DelayReduction;
        public ushort PowerBase, PowerBonus;
        //消耗的MP能量,基础能量，额外能量
        public ushort MPowerBase, MPowerBonus;
        public float MultiplierBase = 1.0f, MultiplierBonus;
        //释放距离
        public byte Range = 9;

        public override string ToString()
        {
            return Name;
        }

        public MagicInfo()
        {

        }

        //作废
        public MagicInfo (BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
        {
            Name = reader.ReadString();
            Spell = (Spell)reader.ReadByte();
            BaseCost = reader.ReadByte();
            LevelCost = reader.ReadByte();
            Icon = reader.ReadByte();
            Level1 = reader.ReadByte();
            Level2 = reader.ReadByte();
            Level3 = reader.ReadByte();
            Need1 = reader.ReadUInt16();
            Need2 = reader.ReadUInt16();
            Need3 = reader.ReadUInt16();
            DelayBase = reader.ReadUInt32();
            DelayReduction = reader.ReadUInt32();
            PowerBase = reader.ReadUInt16();
            PowerBonus = reader.ReadUInt16();
            MPowerBase = reader.ReadUInt16();
            MPowerBonus = reader.ReadUInt16();

            if (version > 66)
                Range = reader.ReadByte();
            if (version > 70)
            {
                MultiplierBase = reader.ReadSingle();
                MultiplierBonus = reader.ReadSingle();
            }
        }
        //作废
        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)Spell);
            writer.Write(BaseCost);
            writer.Write(LevelCost);
            writer.Write(Icon);
            writer.Write(Level1);
            writer.Write(Level2);
            writer.Write(Level3);
            writer.Write(Need1);
            writer.Write(Need2);
            writer.Write(Need3);
            writer.Write(DelayBase);
            writer.Write(DelayReduction);
            writer.Write(PowerBase);
            writer.Write(PowerBonus);
            writer.Write(MPowerBase);
            writer.Write(MPowerBonus);
            writer.Write(Range);
            writer.Write(MultiplierBase);
            writer.Write(MultiplierBonus);
        }

        /// <summary>
        /// 加载所有的魔法技能，从数据库中加载
        /// </summary>
        /// <returns></returns>
        public static List<MagicInfo>  loadAll()
        {
            List<MagicInfo> list = new List<MagicInfo>();
            //DbDataReader read = MirConfigDB.ExecuteReader("select Name,BaseCost,LevelCost,Icon,Level1,Level2,Level3,Need1,Need2,Need3,DelayBase,DelayReduction,PowerBase,PowerBonus,MPowerBase,MPowerBonus,Range,Spell,MultiplierBase,MultiplierBonus from MagicInfo");
            DbDataReader read = MirConfigDB.ExecuteReader("select * from MagicInfo");

            while (read.Read())
            {
                MagicInfo magic = new MagicInfo();
                if (read.IsDBNull(read.GetOrdinal("Name")))
                {
                    continue;
                }
                magic.Name = read.GetString(read.GetOrdinal("Name"));
                if (magic.Name == null)
                {
                    continue;
                }
                magic.BaseCost = read.GetByte(read.GetOrdinal("BaseCost"));
                magic.LevelCost = read.GetByte(read.GetOrdinal("LevelCost"));
                magic.Icon = read.GetByte(read.GetOrdinal("Icon"));
                magic.Level1 = read.GetByte(read.GetOrdinal("Level1"));
                magic.Level2 = read.GetByte(read.GetOrdinal("Level2"));
                magic.Level3 = read.GetByte(read.GetOrdinal("Level3"));

                magic.Need1 = (ushort)read.GetInt32(read.GetOrdinal("Need1"));
                magic.Need2 = (ushort)read.GetInt32(read.GetOrdinal("Need2"));
                magic.Need3 = (ushort)read.GetInt32(read.GetOrdinal("Need3"));

                magic.DelayBase = (uint)read.GetInt32(read.GetOrdinal("DelayBase"));
                magic.DelayReduction = (uint)read.GetInt32(read.GetOrdinal("DelayReduction"));

                magic.PowerBase = (ushort)read.GetInt32(read.GetOrdinal("PowerBase"));
                magic.PowerBonus = (ushort)read.GetInt32(read.GetOrdinal("PowerBonus"));
                magic.MPowerBase = (ushort)read.GetInt32(read.GetOrdinal("MPowerBase"));
                magic.MPowerBonus = (ushort)read.GetInt32(read.GetOrdinal("MPowerBonus"));

                magic.Range = read.GetByte(read.GetOrdinal("Range"));
                magic.Spell = (Spell)read.GetByte(read.GetOrdinal("Spell"));

                magic.MultiplierBase = read.GetFloat(read.GetOrdinal("MultiplierBase"));
                magic.MultiplierBonus = read.GetFloat(read.GetOrdinal("MultiplierBonus"));

                list.Add(magic);
            }

            return list;
        }

        //保存到数据库中,update
        //作废
        public void SaveDB2()
        {
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> lp = new List<SQLiteParameter>();
            sb.Append("update MagicInfo set ");

            sb.Append(" Name=@Name, "); lp.Add(new SQLiteParameter("Name", Name));
            sb.Append(" BaseCost=@BaseCost, "); lp.Add(new SQLiteParameter("BaseCost", BaseCost));
            sb.Append(" LevelCost=@LevelCost, "); lp.Add(new SQLiteParameter("LevelCost", LevelCost));
            sb.Append(" Icon=@Icon, "); lp.Add(new SQLiteParameter("Icon", Icon));
            sb.Append(" Level1=@Level1, "); lp.Add(new SQLiteParameter("Level1", Level1));
            sb.Append(" Level2=@Level2, "); lp.Add(new SQLiteParameter("Level2", Level2));
            sb.Append(" Level3=@Level3, "); lp.Add(new SQLiteParameter("Level3", Level3));
            sb.Append(" Need1=@Need1, "); lp.Add(new SQLiteParameter("Need1", Need1));
            sb.Append(" Need2=@Need2, "); lp.Add(new SQLiteParameter("Need2", Need2));
            sb.Append(" Need3=@Need3, "); lp.Add(new SQLiteParameter("Need3", Need3));
            sb.Append(" DelayBase=@DelayBase, "); lp.Add(new SQLiteParameter("DelayBase", DelayBase));
            sb.Append(" DelayReduction=@DelayReduction, "); lp.Add(new SQLiteParameter("DelayReduction", DelayReduction));
            sb.Append(" PowerBase=@PowerBase, "); lp.Add(new SQLiteParameter("PowerBase", PowerBase));
            sb.Append(" PowerBonus=@PowerBonus, "); lp.Add(new SQLiteParameter("PowerBonus", PowerBonus));
            sb.Append(" MPowerBase=@MPowerBase, "); lp.Add(new SQLiteParameter("MPowerBase", MPowerBase));
            sb.Append(" MPowerBonus=@MPowerBonus, "); lp.Add(new SQLiteParameter("MPowerBonus", MPowerBonus));
            sb.Append(" Range=@Range, "); lp.Add(new SQLiteParameter("Range", Range));
            sb.Append(" MultiplierBase=@MultiplierBase, "); lp.Add(new SQLiteParameter("MultiplierBase", MultiplierBase));
            sb.Append(" MultiplierBonus=@MultiplierBonus "); lp.Add(new SQLiteParameter("MultiplierBonus", MultiplierBonus));
            sb.Append(" where  Spell=@Spell"); lp.Add(new SQLiteParameter("Spell", Spell));
            //执行更新
            MirConfigDB.Execute(sb.ToString(),lp.ToArray());
        }
    }

    //用户释放的魔法，这个是捆绑到快捷键的
    public class UserMagic
    {
        public int userid;//添加字段，这个是用户ID
        public Spell Spell;
        public MagicInfo Info;

        public byte Level, Key;
        public ushort Experience;
        public bool IsTempSpell;
        public long CastTime;

        private MagicInfo GetMagicInfo(Spell spell)
        {
            for (int i = 0; i < SMain.Envir.MagicInfoList.Count; i++)
            {
                MagicInfo info = SMain.Envir.MagicInfoList[i];
                if (info.Spell != spell) continue;
                return info;
            }
            return null;
        }

        public UserMagic(Spell spell)
        {
            Spell = spell;
            
            Info = GetMagicInfo(Spell);
        }

        //这个后续作废
        public UserMagic(int back,BinaryReader reader)
        {
            Spell = (Spell) reader.ReadByte();
            Info = GetMagicInfo(Spell);

            Level = reader.ReadByte();
            Key = reader.ReadByte();
            Experience = reader.ReadUInt16();

            if (Envir.LoadVersion < 15) return;
            IsTempSpell = reader.ReadBoolean();

            if (Envir.LoadVersion < 65) return;
            CastTime = reader.ReadInt64();
        }


       public static List<UserMagic> loadByUserid(int userid)
       {
            List<UserMagic> list = new List<UserMagic>();
            DbDataReader read = MirRunDB.ExecuteReader("select * from UserMagic where userid=@userid", new SQLiteParameter("userid", userid));
            while (read.Read())
            {
                if (read.IsDBNull(read.GetOrdinal("Spell")))
                {
                    continue;
                }
                UserMagic magic = new UserMagic((Spell)read.GetByte(read.GetOrdinal("Spell")));
                magic.userid = userid;
                magic.Level = read.GetByte(read.GetOrdinal("Level"));
                magic.Key = read.GetByte(read.GetOrdinal("Key"));
                magic.Experience = (ushort)read.GetInt32(read.GetOrdinal("Experience"));
                magic.CastTime = read.GetInt64(read.GetOrdinal("CastTime"));
                magic.IsTempSpell = read.GetBoolean(read.GetOrdinal("IsTempSpell"));
                list.Add(magic);
            }
            return list;
        }

        //后续作废
        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)Spell);

            writer.Write(Level);
            writer.Write(Key);
            writer.Write(Experience);
            writer.Write(IsTempSpell);
            writer.Write(CastTime);
        }

        //保存到数据库
        public void SaveDB()
        {
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> lp = new List<SQLiteParameter>();
            sb.Append("update UserMagic set ");

            sb.Append(" Level=@Level, "); lp.Add(new SQLiteParameter("Level", Level));
            sb.Append(" Key=@Key, "); lp.Add(new SQLiteParameter("Key", Key));
            sb.Append(" Experience=@Experience, "); lp.Add(new SQLiteParameter("Experience", Experience));
            sb.Append(" IsTempSpell=@IsTempSpell, "); lp.Add(new SQLiteParameter("IsTempSpell", IsTempSpell));
            sb.Append(" CastTime=@CastTime "); lp.Add(new SQLiteParameter("CastTime", CastTime));
            
            sb.Append(" where  Spell=@Spell and userid=@userid"); lp.Add(new SQLiteParameter("Spell", Spell)); lp.Add(new SQLiteParameter("userid", userid));
            //执行更新
            int ucount = MirRunDB.Execute(sb.ToString(), lp.ToArray());

            //没有得更新，则执行插入
            if (ucount <= 0)
            {
                sb.Clear();
                lp.Clear();
                sb.Append("insert into UserMagic(Level,Key,Experience,IsTempSpell,CastTime,Spell,userid) values(@Level,@Key,@Experience,@IsTempSpell,@CastTime,@Spell,@userid) ");

                lp.Add(new SQLiteParameter("Level", Level));
                lp.Add(new SQLiteParameter("Key", Key));
                lp.Add(new SQLiteParameter("Experience", Experience));
                lp.Add(new SQLiteParameter("IsTempSpell", IsTempSpell));
                lp.Add(new SQLiteParameter("CastTime", CastTime));

                lp.Add(new SQLiteParameter("Spell", Spell)); lp.Add(new SQLiteParameter("userid", userid));
                //执行插入
                MirRunDB.Execute(sb.ToString(), lp.ToArray());
            }
        }

        public Packet GetInfo()
        {
            return new S.NewMagic
                {
                    Magic = CreateClientMagic()
                };
        }

        public ClientMagic CreateClientMagic()
        {
            return new ClientMagic
                {
                    Name= Info.Name,
                    Spell = Spell,
                    BaseCost = Info.BaseCost,
                    LevelCost = Info.LevelCost,
                    Icon = Info.Icon,
                    Level1 = Info.Level1,
                    Level2 = Info.Level2,
                    Level3 = Info.Level3,
                    Need1 = Info.Need1,
                    Need2 = Info.Need2,
                    Need3 = Info.Need3,
                    Level = Level,
                    Key = Key,
                    Experience = Experience,
                    IsTempSpell = IsTempSpell,
                    Delay = GetDelay(),
                    Range = Info.Range,
                    CastTime = (CastTime != 0) && (SMain.Envir.Time > CastTime)? SMain.Envir.Time - CastTime: 0
            };
        }

        public int GetDamage(int DamageBase)
        {
            return (int)((DamageBase + GetPower()) * GetMultiplier());
        }

        public float GetMultiplier()
        {
            return (Info.MultiplierBase + (Level * Info.MultiplierBonus));
        }

        public int GetPower()
        {
            return (int)Math.Round((MPower() / 4F) * (Level + 1) + DefPower());
        }

        public int MPower()
        {
            if (Info.MPowerBonus > 0)
            {
                return SMain.Envir.Random.Next(Info.MPowerBase, Info.MPowerBonus + Info.MPowerBase);
            }
            else
                return Info.MPowerBase;
        }
        public int DefPower()
        {
            if (Info.PowerBonus > 0)
            {
                return SMain.Envir.Random.Next(Info.PowerBase, Info.PowerBonus + Info.PowerBase);
            }
            else
                return Info.PowerBase;
        }

        public int GetPower(int power)
        {
            return (int)Math.Round(power / 4F * (Level + 1) + DefPower());
        }

        public long GetDelay()
        {
            return Info.DelayBase - (Level * Info.DelayReduction);
        }
    }
}
