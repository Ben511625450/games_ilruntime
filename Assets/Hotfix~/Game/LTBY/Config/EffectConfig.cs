using System.Collections.Generic;

namespace Hotfix.LTBY
{
    public class EffectConfig
    {
        public static Dictionary<int, string> FlyText = new Dictionary<int, string>()
        {
            {1, "金币大爆发"},
            {2, "免费刮刮卡奖金"},
            {3, "百万大奖奖金"},
            {4, "千万巨奖奖金"},
            {5, "亿万豪奖奖金"},
        };

        public static Dictionary<string, DropItemData> DropItem = new Dictionary<string, DropItemData>()
        {
            {
                "Drill",
                new DropItemData()
                {
                    imageBundleName = "res_battery", 
                    imageName = new Dictionary<int, string>()
                    {
                        {300, "tsp_cyhjp"}
                    },
                    scale = 1,
                }
            },
            {
                "Electric",
                new DropItemData()
                {
                    imageBundleName = "res_battery",
                    imageName = new Dictionary<int, string>()
                    {
                        {400, "tsp_dcpd"}
                    },
                    scale = 1,
                }
            },
            {
                "ScratchCard",
                new DropItemData()
                {
                    imageBundleName = "res_view",
                    imageName = new Dictionary<int, string>()
                    {
                        {1, "gk_icon"}
                    },
                    scale = 1,
                }
            },
            {
                "Missile",
                new DropItemData()
                {
                    imageBundleName = "res_view",
                    imageName = new Dictionary<int, string>()
                    {
                        {600, "special_item_5"},
                        {601, "special_item_6"},
                        {602, "special_item_7"},
                        {603, "special_item_8"},
                    },
                    scale = 1,
                }
            },
            {
                "Summon",
                new DropItemData()
                {
                    imageBundleName = "res_view", 
                    imageName = new Dictionary<int, string>()
                    {
                        {700, "yxjm_hl"}
                    },
                    scale = 1.5f,
                }
            },
            {
                "FreeBattery",
                new DropItemData()
                {
                    imageBundleName = "res_battery", 
                    imageName = new Dictionary<int, string>()
                    {
                        {800, "pao_free"}
                    },
                    scale = 1.5f,
                }
            },
        };

        public static Dictionary<int, List<int>> DialConfig = new Dictionary<int, List<int>>()
        {
            {1, new List<int>() {2, 3, 5, 6, 7, 8, 9, 10,}},
            {2, new List<int>() {2, 3, 7, 8, 10,}},
            {3, new List<int>() {2, 3, 5, 10, 20}}
        };

        public static Dictionary<int, List<int>> ElectriBallMul = new Dictionary<int, List<int>>()
        {
            {1, new List<int>() {1, 2, 3}},
            {2, new List<int>() {1, 2, 3, 4}},
        };

        public const int UseFireworkScore = 40000000;
        public const int UseHundredMillionScore = 100000000;
    }

    public class DropItemData
    {
        public string imageBundleName;
        public Dictionary<int, string> imageName;
        public float scale;
    }
}