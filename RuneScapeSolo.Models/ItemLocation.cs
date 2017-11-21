﻿using RuneScapeSolo.Primitives;

namespace RuneScapeSolo.Models
{
    public class ItemLocation
    {
        public string Id { get; set; }

        public Point2D Coordinates { get; set; }

        public int Amount { get; set; }

        public int RespawnTime { get; set; }
    }
}
