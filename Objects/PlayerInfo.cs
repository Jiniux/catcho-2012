﻿using osuserver2012.Enums;

namespace osuserver2012
{
    public class PlayerInfo
    {
        public long Score { get; set; }
        public long RankedScore { get; set; }
        public Status Status { get; set; }
        public int PlayCount { get; set; }
        public float Accuracy { get; set; }
        public int Rank { get; set; }
        public string Action { get; set; }
        public string ActionMD5 { get; set; }
        public Gamemode Gamemode { get; set; }
        public short Mods { get; set; }
        public int MapID { get; set; }
    }
}