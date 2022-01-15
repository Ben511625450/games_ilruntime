﻿using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.LTBY
{
    public class GameViewConfig
    {
        public class Text
        {
            public const string Wait = "等待加入...";
            public const string Reconnect = "断线重连中...";
            public const string OpenAutoShoot = "开启自动射击";
            public const string OpenLockFish = "开启自动锁鱼";
            public const string OpenRageShoot = "开启狂暴射击";
            public const string OpenMultiShoot = "开启散弹射击";
            public const string SyncServerData = "正在同步服务器数据...";
            public const string RageShootInvalid = "免费游戏期间，狂暴技能无效";
        };

        public class ShowGameUi
        {
            public const float Time = 0.3f;
            public const float Dis = 200;
        }

        public static Dictionary<int, BatteryPosition> Position = new Dictionary<int, BatteryPosition>()
        {
            {
                0,
                new BatteryPosition()
                {
                    ScoreFrame = new Vector3(-75, -15, 0), NameFrame = new Vector3(-75, 10, 0),
                    BatteryFrame = new Vector3(105, 0, 0),
                }
            },
            {
                1,
                new BatteryPosition()
                {
                    ScoreFrame = new Vector3(80, -15, 0), NameFrame = new Vector3(80, 10, 0),
                    BatteryFrame = new Vector3(-105, 0, 0),
                }
            },
            {
                2,
                new BatteryPosition()
                {
                    ScoreFrame = new Vector3(-75, 25, 0), NameFrame = new Vector3(-75, 0, 0),
                    BatteryFrame = new Vector3(105, 5, 0),
                }
            },
            {
                3,
                new BatteryPosition()
                {
                    ScoreFrame = new Vector3(80, 25, 0), NameFrame = new Vector3(80, 0, 0),
                    BatteryFrame = new Vector3(-105, 5, 0),
                }
            }
        };

        public class UserEventId
        {
            public const int HideIcon = 0;
            public const int CrazySkill = 1;
            public const int UserPool = 2;
            public const int MultiShoot = 3;
        }
    }

    public class BatteryPosition
    {
        public Vector3 ScoreFrame;
        public Vector3 NameFrame;
        public Vector3 BatteryFrame;
    }
}