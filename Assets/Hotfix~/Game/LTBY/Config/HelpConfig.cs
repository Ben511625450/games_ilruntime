using System.Collections.Generic;

namespace Hotfix.LTBY
{
    public class HelpConfig
    {
        public class HelpContent
        {
            public string special;
            public string normal;
            public string name;
            public string des;
            public string imageBundleName;
            public string imageName;
            public float imageX;
            public float imageY;
            public string Text;
        }

        public static FrameData Frame = new FrameData()
        {
            x = 0, y = -20, w = 910, h = 540,
            Title = "游戏帮助",
            Flag1 = "鱼种图鉴",
            Flag2 = "操作说明",
            Flag3 = "联系客服",
        };

        public static Dictionary<string, string> Flag = new Dictionary<string, string>()
        {
            {"Flag1", "鱼种图鉴"},
            {"Flag2", "操作说明"},
            {"Flag3", "联系客服"},
        };


        public const string ScoreText = "分值:";

        public static HelpContent Content1 = new HelpContent()
        {
            special = "特殊鱼",
            normal = "普通鱼",
        };

        public static List<HelpContent> Content2 = new List<HelpContent>()
        {
            new HelpContent()
            {
                name = "自动射击",
                des = "点击“            ”按键,可以自动发射子弹,但需要玩家自己控制方向。",
                imageBundleName = "res_view",
                imageName = "yxjm_jn_zd_1",
                imageX = 97,
                imageY = 0,
            },
            new HelpContent()
            {
                name = "锁定功能",
                des =
                    "长按“             ”图标,打开锁定设置界面,勾选锁定鱼种,可设置锁定范围。\n点击图标,可开启锁定功能;开启该功能后,可以锁定并持续攻击目标。\n<color=#FFCB25FF>若锁定设置中有勾选,将会自动锁定勾选的鱼进行自动射击;</color>自动锁定的过\n程中,点击鱼可手动切换锁定目标。\n<color=#FFCB25FF>若锁定设置中全不勾选,则需要手动点击鱼进行锁定捕捉。</color>",
                imageBundleName = "res_view",
                imageName = "yxjm_jn_xz_1",
                imageX = 97,
                imageY = 90,
            },
            //new HelpContent(){
            //	name = "狂暴功能",
            //	des = "<color=#FFCB25FF>会员专属技能,充值任意点券即可获得会员。</color>\n点击“             ”图标可开启狂暴技能，可以加快发射子弹的速度。",
            //	imageBundleName = "res_view",
            //	imageName = "yxjm_p3_1",
            //	imageX = 97,
            //	imageY = -20,
            //},
            //new HelpContent(){
            //	name = "散射功能",
            //	des = "<color=#FFCB25FF>刺激战场专属技能，三弹齐发威力无比。</color>\n点击“             ”图标可开启散射技能，可以同时发射三发子弹。\n",
            //	imageBundleName = "res_view",
            //	imageName = "yxjm_ss",
            //	imageX = 97,
            //	imageY = 0,
            //},
        };

        public static HelpContent Content3 = new HelpContent()
        {
            //Text = "客服电话 ：4000-234-000\n客服 QQ  ：4000234000"
            // Text = "客服 QQ  ：4000234000"
            Text = "客服 QQ  ：4000000000"
        };
    }
}