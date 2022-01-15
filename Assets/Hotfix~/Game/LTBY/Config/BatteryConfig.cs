using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.LTBY
{
    public class BatteryConfig
    {
        public class Frame
        {
            public const float x = 0;
            public const float y = -20;
            public const float w = 900;
            public const float h = 540;
            public const string Title = "请选择大炮";
            public const string Tip = "说明：游戏中点击炮台，可弹出本界面，重新选择大炮！";
        }
        public const float ShowWaterSpoutTime = 0.5f;
        public const int DragonBatteryLevel = 9;		//雷皇龙炮
        public const int ZCMBatteryLevel = 10;			//招财猫炮
        public const string Unit = "倍";
        public const string Qian = "千";
        public const string Wan = "万";
        public const string CountdownText = "倒计时:";
        public const string Day = "天";
        public const string Hour = "时";
        public const string Minute = "分";
        public const string Second = "秒";
        public const string Forever = "永久";
        public const string Level = "级";
        public const string LockText = "未开启";
        public const string VipLockText = "获得炮台\nVip≥";
        public const string MemberLockText = "获得炮台\n会员专属";
        public const string ShootCountdownText = "剩余发射时间：";
        public const string MissileTip1 = "请瞄准奖金鱼发射火箭炮";
        public const string MissileTip2 = "火箭炮只能炸奖金鱼哦";
        public const string Permanent = "获得炮台\n永久会员专属";
        public const int LuckyCatGunLevel = 10;

        public static Dictionary<int, BatteryLevelData> DefaultBattery = new Dictionary<int, BatteryLevelData>()
        {
            {1, new BatteryLevelData(){ level = 1, ratio = 1 } },
            {2, new BatteryLevelData(){ level = 2, ratio = 10 } },
            {3, new BatteryLevelData(){ level = 3, ratio = 100 } },
            {4, new BatteryLevelData(){ level = 4, ratio = 1000 } },
            {5, new BatteryLevelData(){ level = 5, ratio = 10000 } },
        };
        public static Dictionary<int, BatteryData> Battery = new Dictionary<int, BatteryData>()
        {
            {1, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_1", name = "1级大炮", des1 = "入门练手", des2 = "", pivot=new Vector2(0.5f,0.2f)} },
            {2, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_2", name = "2级大炮", des1 = "进阶腾飞", des2 = "百步穿杨", pivot=new Vector2(0.5f,0.2f) } },
            {3, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_3", name = "3级大炮", des1 = "熟练捕鱼", des2 = "指哪打哪", pivot=new Vector2(0.5f,0.2f)} },
            {4, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_4", name = "4级大炮", des1 = "高手在人间", des2 = "财源滚滚",pivot=new Vector2(0.5f,0.2f) } },
            {5, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_5", name = "5级大炮", des1 = "大师在世", des2 = "腰缠万贯",pivot=new Vector2(0.5f,0.2f) } },
            {6, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_6", name = "黄金甲", des1 = "身披黄金甲", des2 = "周围散发着光芒", pivot=new Vector2(0.5f,0.2f)} },
            {7, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_7", name = "圣骑", des1 = "浑身散发", des2 = "着贵族气息", pivot=new Vector2(0.5f,0.2f)} },
            {8, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_8", name = "五福熊猫炮", des1 = "五福临门", des2 = "笑口常开", scale = 1.1f ,pivot=new Vector2(0.5f,0.1f)} },
            {9, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_9", name = "雷皇龙击炮", des1 = "爆发龙王怒袭", des2 = "威力无比", scale = 1.1f,pivot=new Vector2(0.5f,0.3f) } },
            {10, new BatteryData(){ imageBundleName = "res_battery", imageName = "select_battery_10", name = "幸运招财炮", des1 = "幸运的招财猫", des2 = "金币滚滚来", scale = 1.1f ,pivot=new Vector2(0.5f,0.3f)} },
        };

        public static Dictionary<int, Dictionary<int, BatteryWaterSpout>> BatteryWaterSpoutConfig = new Dictionary<int, Dictionary<int, BatteryWaterSpout>>()
        {

            { 9 , new Dictionary<int, BatteryWaterSpout>()
            {
                {1 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu01", spoutWidth = 2 }},
                {2 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu02", spoutWidth = 2 }},
                {3 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu03", spoutWidth = 4 }},
                {4 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu04", spoutWidth = 4 } },
            } },
            { 10 , new Dictionary<int, BatteryWaterSpout>()
            {
                {1 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_zcm_shuizhu01", spoutWidth = 4.5f } },
                {2 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu02", spoutWidth = 2 } },
                {3 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_zcm_shuizhu02", spoutWidth = 5 } },
                {4 , new BatteryWaterSpout(){ bundleName = "res_effect", materialName = "mat_shuizhu04", spoutWidth = 4 } },
            } },
        };
    }

    public class BatteryLevelData
    {
        public int level;
        public int ratio;
    }

    public class BatteryData
    {
        public string imageBundleName;
        public string imageName;
        public string name;
        public string des1;
        public string des2;
        public float scale;
        public Vector2 pivot;
    }
    public class BatteryWaterSpout
    {
        public string bundleName;
        public string materialName;
        public float spoutWidth;
    }
}
