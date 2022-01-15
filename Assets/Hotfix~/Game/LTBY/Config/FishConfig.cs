using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.LTBY
{
    /// <summary>
    /// 鱼配置
    /// </summary>
    public class FishDataConfig
    {
        public int fishOrginType;
        public int fishType;
        public string ResName;
        public string fishName;
        public string fishScore;
        public bool specialFish;
        public string nameSuffix;
        public string des;
        public float previewCameraSize;
        public Vector2 previewOffset;
        public Vector3 previewRotation;
        public int previewPlayAnim;
        public bool previewForbidTouch;
        public bool isDialFish;
        public string broadcast;
        public string broadcastBg;
        public string broadcastBgBundle;
        public float fixedZ;
        public Color hitColor;
        public Color hitColor2D;
        public bool hitChange;
        public bool swinScale;
        public bool isLightFish;
        public float effectScale;
        public bool isCoinOutburstFish;
        public int explosionPointLevel;
        public bool isWhaleFish;
        public string fishHitSound;
        public string fishDeadSound;
        public string fishAppearSound;
        public bool isGlobefish;
        public List<Vector3> roadEulerAngle;
        public string fishDieSound;
        public bool is2D;
        public bool isAwardFish;
        public List<int> killList;
    }

    public class FishConfig
    {
        public const int fishCount = 36;

        public static readonly List<FishDataConfig> Fish = new List<FishDataConfig>()
        {
            new FishDataConfig()
            {
                fishType = 36,
                fishOrginType = 127,
                ResName = "fish_127",
                fishName = "金蟾",
                fishScore = "150",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n海底的土豪，从头到脚都有黄金作饰品，以被人围观而感到自豪。",
                previewCameraSize = 8.5f,
                hitColor = new Color(190, 122, 122, 255),
                explosionPointLevel = 2,
                fishHitSound = "FishSound14",
            },
            new FishDataConfig()
            {
                fishType = 35,
                fishOrginType = 125,
                ResName = "fish_125",
                fishName = "锤头金鲨",
                fishScore = "120",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n自称素食主义者，喜欢品尝海草和藏在海草里的小鱼。",
                previewCameraSize = 10,
                previewOffset = {x = 0, y = 1},
                hitColor = new Color(177, 120, 120, 255),
                hitColor2D = new Color(208, 101, 86, 255),
                explosionPointLevel = 2,
                fishDeadSound = "FishSound12",
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 34,
                fishOrginType = 124,
                ResName = "fish_124",
                fishName = "大金鲨",
                fishScore = "100",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n是大白鲨的亲戚，行事却十分高调，喜欢炫耀自己的战绩与收获。",
                previewCameraSize = 10,
                previewOffset = {x = 0, y = 0.5f},
                hitColor = new Color(194, 114, 114, 255),
                hitColor2D = new Color(238, 117, 96, 255),
                explosionPointLevel = 2,
                fishDeadSound = "FishSound15",
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 33,
                fishOrginType = 121,
                ResName = "fish_121",
                fishName = "金炸弹鱼",
                fishScore = "55",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n个头比一般的炸弹鱼要大，口头禅是人生苦短，常模仿金蟾打扮自己。",
                previewCameraSize = 7,
                hitColor = new Color(194, 120, 120, 255),
                hitColor2D = new Color(246, 150, 98, 255),
                effectScale = 0.6f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 32,
                fishOrginType = 131,
                ResName = "fish_131",
                fishName = "宝藏章鱼",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n手持宝箱，被捕捉后大概率触发“金币大爆发”，连续多次爆发金币",
                previewCameraSize = 6,
                hitColor = new Color(199, 112, 112, 255),
                hitColor2D = new Color(249, 139, 123, 255),
                isCoinOutburstFish = true,
                fishHitSound = "FishSound3",
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 31,
                fishOrginType = 136,
                ResName = "fish_136",
                fishName = "黄金巨龙",
                fishScore = "最高500倍",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n传说中的瑞兽，象征着吉祥和尊贵，可上天入海，见首不见尾。捕获后<color=#FFCB25FF>最高可获得500倍奖励</color>",
                previewCameraSize = 30,
                previewOffset = {x = 0, y = 5},
                previewRotation = {x = 60, y = 165, z = 0},
                previewPlayAnim = 15,
                previewForbidTouch = true,
                broadcast = "黄金巨龙来了！！！",
                fixedZ = 190,
                hitColor = new Color(197, 166, 84, 255),
                fishAppearSound = "FishSound16",
                roadEulerAngle = new List<Vector3>()
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 236, 0),
                    new Vector3(0, 180, 0),
                    new Vector3(0, 55, 0)
                },
            },
            new FishDataConfig()
            {
                fishType = 30,
                fishOrginType = 145,
                ResName = "fish_145",
                fishName = "聚宝盆",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n传说中的宝物，拥有吸财纳物的功能，<color=#FFCB25FF>可使放入其中的财宝不断倍增，场次越高，累计财宝越多！</color>",
                previewCameraSize = 5,
                previewOffset = {x = 0, y = -0.9f},
                previewForbidTouch = true,
                broadcast = "聚宝盆来了！！！",
                broadcastBg = "JBP_BG",
                broadcastBgBundle = "res_effect2",
                previewRotation = {x = -90, y = 0, z = 0},
                fishAppearSound = "FishAppearSound145",
                fishHitSound = "FishSound3",
                hitColor = new Color(242, 116, 198, 255),
                fixedZ = -4,
            },
            new FishDataConfig()
            {
                fishType = 29,
                fishOrginType = 142,
                ResName = "fish_142",
                fishName = "大奖章鱼",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n神秘的大奖章鱼，捕捉后会触发3个“连环转盘”。<color=#FFCB25FF>最高可获得2000倍奖励</color>",
                previewCameraSize = 10,
                broadcast = "大奖章鱼来了！！！",
                hitColor = new Color(199, 112, 112, 1),
                previewRotation = {x = -70, y = 90, z = 0},
                isDialFish = true,
                fishHitSound = "FishSound3",
            },
            // new FishDataConfig()
            // {
            //     fishType = 31,
            //     fishOrginType = 143,
            //     ResName = "fish_143_3",
            //     fishName = "一网打尽(飞鱼)",
            //     fishScore = "随机",
            //     specialFish = true,
            //     nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
            //     des = "鱼简介：\n一群命运共同体的鱼，捕获任意一条标记有“一网打尽”的鱼，其它的将同时被捕获。",
            //     hitColor = new Color(225,0,0,255),
            //     hitColor2D = new Color(246,94,90,255),
            //     previewCameraSize = 8.5f,
            //     previewRotation = { x = -90,y = 0,z = 0},
            // },
            // new FishDataConfig()
            // {
            //     fishType = 30,
            //     fishOrginType = 143,
            //     ResName = "fish_143_2",
            //     fishName = "一网打尽(乌龟)",
            //     fishScore = "随机",
            //     specialFish = true,
            //     nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
            //     des = "鱼简介：\n一群命运共同体的鱼，捕获任意一条标记有“一网打尽”的鱼，其它的将同时被捕获。",
            //     hitColor = new Color(199,13,13,255),
            //     hitColor2D = new Color(255,124,98,255),
            //     previewCameraSize = 8.5f,
            //     previewRotation = { x = -90,y = 0,z = 0},
            // },
            // new FishDataConfig()
            // {
            //     fishType = 29,
            //     fishOrginType = 143,
            //     ResName = "fish_143_1",
            //     fishName = "一网打尽(青蛙)",
            //     fishScore = "随机",
            //     specialFish = true,
            //     nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
            //     des = "鱼简介：\n一群命运共同体的鱼，捕获任意一条标记有“一网打尽”的鱼，其它的将同时被捕获。",
            //     hitColor = new Color(107,28,28,255),
            //     hitColor2D = new Color(255,96,49,255),
            //     previewCameraSize = 8.5f,
            //     previewRotation = { x = -90,y = 0,z = 0},
            // },
            new FishDataConfig()
            {
                fishType = 143,
                fishOrginType = 143,
                ResName = "fish_143",
                fishName = "一网打尽",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n一群命运共同体的鱼，捕获任意一条标记有“一网打尽”的鱼，其它的将同时被捕获。",
                hitColor = new Color(107, 128, 128, 255),
                hitColor2D = new Color(255, 96, 49, 255),
                previewCameraSize = 8.5f,
                previewRotation = {x = -90, y = 0, z = 0},
            },
            new FishDataConfig()
            {
                fishType = 28,
                fishOrginType = 132,
                ResName = "fish_132",
                fishName = "闪电海鳗",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n有海中雷震子之称，难以捕获。被捕获后，有几率触发闪电链，附近的小鱼都要遭殃了。",
                previewCameraSize = 7,
                broadcast = "海鳗来了！！！",
                hitColor = new Color(124,118, 118, 255),
                hitColor2D = new Color(233, 102, 68, 255),
                isLightFish = true,
                is2D = true,
                killList = new List<int>() {1, 2, 3, 4, 5, 6, 7}
            },
            new FishDataConfig()
            {
                fishType = 27,
                fishOrginType = 139,
                ResName = "fish_139",
                fishName = "闪电蝴蝶",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n在鱼阵中出现，被捕获后附近的蝴蝶鱼都要遭殃。",
                previewCameraSize = 4.5f,
                hitColor = new Color(124, 118, 118, 255),
                hitColor2D = new Color(255, 152, 105, 255),
                isLightFish = true,
                is2D = true,
                killList = new List<int>() {2}
            },
            new FishDataConfig()
            {
                fishType = 26,
                fishOrginType = 140,
                ResName = "fish_140",
                fishName = "闪电蜻蜓",
                fishScore = "随机",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n在鱼阵中出现，被捕获后附近的蜻蜓鱼都要遭殃。",
                previewCameraSize = 4.5f,
                hitColor = new Color(124, 118, 118, 255),
                hitColor2D = new Color(249, 116, 81, 255),
                isLightFish = true,
                is2D = true,
                killList = new List<int>() {1}
            },
            new FishDataConfig()
            {
                fishType = 25,
                fishOrginType = 109,
                ResName = "fish_109",
                fishName = "愤怒河豚",
                fishScore = "10-100",
                specialFish = true,
                nameSuffix = "<color=#5EFF6DFF>（特殊）</color>",
                des = "鱼简介：\n性格倔强，掉落金币后，会奋发图强，不仅体型会变大，分值也会变高，直到它体力不支为止。",
                previewCameraSize = 6,
                hitColor = new Color(118, 124, 124, 255),
                swinScale = true,
                effectScale = 0.4f,
                fishHitSound = "FishSound4",
                fishDeadSound = "FishSound5",
                isGlobefish = true,
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 24,
                fishOrginType = 128,
                ResName = "fish_128",
                fishName = "深海鲸",
                fishScore = "300",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n深海霸主,体积庞大，听说是身价上亿的海中富豪。",
                previewCameraSize = 25,
                previewOffset = {x = 0, y = 0},
                broadcast = "深海鲸来了！！！",
                fixedZ = 130,
                hitColor = new Color(188, 157, 157, 255),
                hitChange = true,
                explosionPointLevel = 3,
                isWhaleFish = true,
            },
            new FishDataConfig()
            {
                fishType = 23,
                fishOrginType = 129,
                ResName = "fish_129",
                fishName = "美人鱼",
                fishScore = "100-150",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n体态优美，身形匀称，童话世界中的精灵美人鱼。",
                previewCameraSize = 10,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(201, 135, 135, 255),
                explosionPointLevel = 2,
                hitChange = true,
                fishDeadSound = "FishSound2",
                fishHitSound = "FishSound7",
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 22,
                fishOrginType = 126,
                ResName = "fish_126",
                fishName = "虎鲸",
                fishScore = "90",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n体型巨大，晒太阳时总让人以为是座小岛，小鱼也喜欢躲在他肚子下乘凉。",
                previewCameraSize = 10,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(182, 120, 120, 255),
                hitColor2D = new Color(255, 186, 163, 255),
                explosionPointLevel = 2,
                fishHitSound = "FishSound8",
                fishDeadSound = "FishSound6",
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 21,
                fishOrginType = 123,
                ResName = "fish_123",
                fishName = "大白鲨",
                fishScore = "80",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n在海洋中相当有地位，行事十分低调，关于他的传奇故事数不胜数。",
                previewCameraSize = 8,
                hitColor = new Color(255, 120, 120, 255),
                hitColor2D = new Color(242, 142, 128, 255),
                explosionPointLevel = 2,
                fishHitSound = "FishSound9",
                fishDeadSound = "FishSound10",
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 20,
                fishOrginType = 113,
                ResName = "fish_113",
                fishName = "龙虾",
                fishScore = "68",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n适应力非常强，甚至能爬上岸，最大的乐趣就是进行冒险。",
                previewCameraSize = 8,
                hitColor = new Color(198, 120, 120, 255),
                hitColor2D = new Color(244, 153, 70, 255),
                effectScale = 1,
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 19,
                fishOrginType = 122,
                ResName = "fish_122",
                fishName = "刺鳐鱼",
                fishScore = "60",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n外形看起来十分吓人，实际上非常温顺，而且胆子很小。",
                previewCameraSize = 6,
                hitColor = new Color(150, 120, 120, 255),
                effectScale = 1,
                hitChange = true,
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 18,
                fishOrginType = 119,
                ResName = "fish_119",
                fishName = "灯笼鱼",
                fishScore = "58",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n喜欢在黑暗中惊吓别人，但捉迷藏从来没有赢过。",
                previewCameraSize = 6,
                hitColor = new Color(107, 120, 120, 255),
                hitColor2D = new Color(236, 109, 63, 255),
                effectScale = 0.6f,
                fishHitSound = "FishSound1",
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 17,
                fishOrginType = 120,
                ResName = "fish_120",
                fishName = "炸弹鱼",
                fishScore = "50",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n出生开始头顶就有一小团火苗，因此也让他无时无刻焦急不安。",
                previewCameraSize = 4,
                hitColor = new Color(204, 120, 120, 255),
                hitColor2D = new Color(255, 171, 126, 255),
                effectScale = 0.6f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 16,
                fishOrginType = 117,
                ResName = "fish_117",
                fishName = "剑鱼",
                fishScore = "40",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n攻击性很强，一旦加起速来没有谁能把他拦住。",
                previewCameraSize = 6,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(255, 120, 120, 255),
                hitColor2D = new Color(238, 132, 89, 255),
                effectScale = 1,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 15,
                fishOrginType = 114,
                ResName = "fish_114",
                fishName = "地鲶鱼",
                fishScore = "35",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n出生开始就留着胡子，年龄不大，辈分可不小。",
                previewCameraSize = 5,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(242, 120, 120, 255),
                hitColor2D = new Color(238, 91, 37, 255),
                effectScale = 0.8f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 14,
                fishOrginType = 115,
                ResName = "fish_115",
                fishName = "寒冰鱼",
                fishScore = "30",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n冰冷的外表，不喜欢接触其他人，是一个难以靠近的家伙。",
                previewCameraSize = 5.5f,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(210, 120, 120, 255),
                hitColor2D = new Color(253, 163, 123, 255),
                effectScale = 0.8f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 13,
                fishOrginType = 110,
                ResName = "fish_110",
                fishName = "孔雀鱼",
                fishScore = "20",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n孔雀鱼对于任何事物似乎都不感兴趣，当然这只有在他吃饱之后。",
                previewCameraSize = 4.5f,
                hitColor = new Color(186, 120, 120, 255),
                hitColor2D = new Color(244, 111, 63, 255),
                effectScale = 0.6f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 12,
                fishOrginType = 118,
                ResName = "fish_118",
                fishName = "飞鱼",
                fishScore = "18",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n擅长跳跃与滑行，梦想是成为一名优雅的水上芭蕾舞者。",
                previewCameraSize = 5.5f,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(225, 120, 120, 255),
                hitColor2D = new Color(246, 94, 90, 255),
                effectScale = 1,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 11,
                fishOrginType = 116,
                ResName = "fish_116",
                fishName = "地狱火鱼",
                fishScore = "15",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n脾气暴躁，风风火火，一旦生气不计后果，从来不会顾忌身边有谁。",
                previewCameraSize = 6,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(171, 120, 120, 255),
                hitColor2D = new Color(248, 148, 104, 255),
                effectScale = 0.9f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 10,
                fishOrginType = 112,
                ResName = "fish_112",
                fishName = "乌龟",
                fishScore = "12",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n海洋中的活化石，喜欢讲远古的传说，但没有人知道是真是假。",
                previewCameraSize = 4,
                previewOffset = {x = 0, y = 0},
                hitColor = new Color(199, 120, 120, 255),
                hitColor2D = new Color(255, 124, 98, 255),
                effectScale = 0.6f,
                is2D = false,
            },
            new FishDataConfig()
            {
                fishType = 9,
                fishOrginType = 111,
                ResName = "fish_111",
                fishName = "小青蛙",
                fishScore = "10",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n小青蛙一直是海里的广播员，只要他知道的事情大家也会第一时间知道。",
                previewCameraSize = 8,
                hitColor = new Color(107, 120, 120, 255),
                hitColor2D = new Color(255, 96, 49, 255),
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 8,
                fishOrginType = 107,
                ResName = "fish_107",
                fishName = "七彩鱼",
                fishScore = "9",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n优哉游哉，与世无争，但是聪明过人，只是一点脾气都没有。",
                previewCameraSize = 4,
                hitColor = new Color(173, 120, 120, 255),
                hitColor2D = new Color(253, 106, 73, 255),
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 7,
                fishOrginType = 106,
                ResName = "fish_106",
                fishName = "狮头鱼",
                fishScore = "8",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n骄傲自大，总认为自己是海底的霸主，常常自找苦头。",
                previewCameraSize = 4,
                hitColor = new Color(118, 120, 120, 255),
                hitColor2D = new Color(251, 126, 44, 255),
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 6,
                fishOrginType = 105,
                ResName = "fish_105",
                fishName = "大眼金鱼",
                fishScore = "7",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n天生长着一对大眼睛，一脸无辜的样子让人不忍心捕捉。",
                previewCameraSize = 3,
                hitColor = new Color(182, 120, 120, 255),
                hitColor2D = new Color(251, 115, 115, 255),
                effectScale = 0.4f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 5,
                fishOrginType = 108,
                ResName = "fish_108",
                fishName = "小丑鱼",
                fishScore = "6",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n机智聪明、调皮好动，加上可爱的外表，一直深受大家喜爱。",
                previewCameraSize = 3,
                hitColor = new Color(234, 120, 120, 255),
                hitColor2D = new Color(255, 109, 109, 255),
                effectScale = 0.4f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 4,
                fishOrginType = 104,
                ResName = "fish_104",
                fishName = "热带黄鱼",
                fishScore = "5",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n同样喜欢晒太阳，但聪明的她总会选择隐秘的地方享受。",
                previewCameraSize = 3,
                hitColor = new Color(251, 120, 120, 255),
                hitColor2D = new Color(255, 103, 103, 255),
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 3,
                fishOrginType = 103,
                ResName = "fish_103",
                fishName = "比目鱼",
                fishScore = "4",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n总会在有阳光的海域发呆，往往因此成为被捕捉的目标。",
                previewCameraSize = 3.5f,
                hitColor = new Color(204, 120, 120, 255),
                hitColor2D = new Color(255, 58, 58, 255),
                effectScale = 0.4f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 2,
                fishOrginType = 101,
                ResName = "fish_101",
                fishName = "蝴蝶鱼",
                fishScore = "3",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n身形小巧，自认为很可爱，常说是海中的小公主。",
                previewCameraSize = 4,
                hitColor = new Color(238, 107, 107, 255),
                hitColor2D = new Color(238, 107, 107, 255),
                effectScale = 0.4f,
                is2D = true,
            },
            new FishDataConfig()
            {
                fishType = 1,
                fishOrginType = 102,
                ResName = "fish_102",
                fishName = "蜻蜓鱼",
                fishScore = "2",
                specialFish = false,
                nameSuffix = "<color=#5EFF6DFF>（普通）</color>",
                des = "鱼简介：\n总是成群结队，长得海草一样的绿，喜欢躲在草里玩捉迷藏。",
                previewCameraSize = 4,
                hitColor = new Color(199, 120, 120, 255),
                hitColor2D = new Color(199, 56, 56, 255),
                effectScale = 0.3f,
                is2D = true,
            },
        };

        //检测是否是龙
        public static bool CheckIsDragon(int fishType)
        {
            FishDataConfig fish = Fish.FindItem(p => p.fishOrginType == fishType);
            if (fish == null) return false;
            return fish.fishOrginType == 136 || fish.fishOrginType == 144;
        }

        public static bool CheckIsElectricDragon(int fishType)
        {
            FishDataConfig fish = Fish.FindItem(p => p.fishOrginType == fishType);
            if (fish == null) return false;
            return fish.fishOrginType == 144;
        }

        //检测是否一网打击鱼
        public static bool CheckIsAcedFish(int fishType)
        {
            FishDataConfig fish = Fish.FindItem(p => p.fishOrginType == fishType);
            if (fish == null) return false;
            return fish.fishOrginType == 143;
        }

        //检测是否使用转盘
        public static bool CheckUseWheel(int score)
        {
            if (score >= 15 && score < 80) return true;
            return false;
        }

        //检测是否是聚宝盆
        public static bool CheckIsTreasureBowl(int fishType)
        {
            FishDataConfig fish = Fish.FindItem(p => p.fishOrginType == fishType);
            if (fish == null) return false;
            return fish.fishOrginType == 145;
        }

        //检测是否使用spine动画的奖励
        public static int CheckUseSpine(int score)
        {
            if (score >= 80 && score < 110) return 1;

            else if (score >= 110 && score < 120) return 2;

            else if (score >= 120 && score < 150) return 3;

            else if (score >= 150) return 4;

            return 0;
        }

        public const int AcedFishType = 143;

        public const string AwardFishText = "奖金鱼";

        //public static TreasureBow TreasureBowlConfig = new TreasureBow() 
        //{
        //    Scale = new Dictionary<int, float>()
        //    {
        //        [0] = 2,
        //        [1] = 2,
        //        [2] = 2.8f,
        //        [3] = 3.3f,
        //    }
        //};

        public class TreasureBow
        {
            public Dictionary<int, float> Scale;
            public Dictionary<int, bool> ShowRotate;
            public Dictionary<int, int> captureEffectType;
            public Dictionary<int, int> StageNum;
        }

        public static TreasureBow TreasureBowlConfig = new TreasureBow()
        {
            Scale = new Dictionary<int, float>()
            {
                {0, 2.1f},
                {1, 2.1f},
                {2, 2.4f},
                {3, 2.7f},
                {4, 3f},
                {5, 3.3f},
            },
            ShowRotate = new Dictionary<int, bool>()
            {
                {0, false},
                {1, false},
                {2, false},
                {3, true},
                {4, false},
                {5, true},
            },
            captureEffectType = new Dictionary<int, int>()
            {
                {0, 1},
                {1, 1},
                {2, 1},
                {3, 2},
                {4, 2},
                {5, 3},
            },
            StageNum = new Dictionary<int, int>()
            {
                {1, 0},
                {2, 3},
                {3, 5},
            }
        };

        public static string GetFishImageBundle(int fishId)
        {
            if (fishId == 144) return "res_electroDragon";
            else if (fishId == 145) return "res_TreasureBowl";
            else if (fishId == 136) return "res_TreasureBowl";
            else return "res_fishmodel";
        }


        public static Dictionary<int, Vector3> TreasureBowlRotation = new Dictionary<int, Vector3>()
        {
            //面向右侧
            {1, new Vector3() {x = 21.024f, y = 152.668f, z = -15.053f}},
            //面向左侧
            {2, new Vector3() {x = 20.6f, y = 206.8f, z = 13.1f}},
        };

        public static List<Dictionary<string, int>> LoadFishList = new List<Dictionary<string, int>>()
        {
            new Dictionary<string, int>() {{"fishType", 2}, {"num", 10}, {"maxNum", 20}},
            new Dictionary<string, int>() {{"fishType", 1}, {"num", 10}, {"maxNum", 30}},
            new Dictionary<string, int>() {{"fishType", 3}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 4}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 6}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 7}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 8}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 5}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 26}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 13}, {"num", 1}, {"maxNum", 5}},

            new Dictionary<string, int>() {{"fishType", 9}, {"num", 5}, {"maxNum", 10}},
            new Dictionary<string, int>() {{"fishType", 10}, {"num", 5}, {"maxNum", 10}},
            new Dictionary<string, int>() {{"fishType", 21}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 15}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 14}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 11}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 16}, {"num", 5}, {"maxNum", 10}},
            new Dictionary<string, int>() {{"fishType", 12}, {"num", 5}, {"maxNum", 15}},
            new Dictionary<string, int>() {{"fishType", 19}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 18}, {"num", 1}, {"maxNum", 5}},

            new Dictionary<string, int>() {{"fishType", 38}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 20}, {"num", 1}, {"maxNum", 5}},
            new Dictionary<string, int>() {{"fishType", 22}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 40}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 41}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 23}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 42}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 25}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 24}, {"num", 0}, {"maxNum", 3}},

            new Dictionary<string, int>() {{"fishType", 39}, {"num", 0}, {"maxNum", 2}},
            new Dictionary<string, int>() {{"fishType", 29}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 33}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 32}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 37}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 28}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 27}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 34}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 31}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 36}, {"num", 0}, {"maxNum", 1}},
            new Dictionary<string, int>() {{"fishType", 35}, {"num", 0}, {"maxNum", 1}},
        };
    }
}