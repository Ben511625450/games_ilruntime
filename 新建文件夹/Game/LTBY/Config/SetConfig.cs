using System.Collections.Generic;


namespace Hotfix.LTBY
{
    public class SetConfig
    {
        public static FrameData Frame = new FrameData()
        {
            x = 0,
            y = 0,
            w = 650,
            h = 420,
            Title = "游戏设置",
        };

        public const string Effect = "音效";

        public const string Music = "音乐";
        public const string LowPower1 = "省电";
        public const string LowPower2 = "低帧";
        public const string LowPower3 = "流畅";
        public const string LowPower4 = "华丽";

        public const string OpenText = "开";
        public const string CloseText = "关";

        public static Dictionary<string, string> LowPower = new Dictionary<string, string>()
        {
            {"LowPower1", "省电"},
            {"LowPower2", "低帧"},
            {"LowPower3", "流畅"},
            {"LowPower4", "华丽"},
        };
    }
}