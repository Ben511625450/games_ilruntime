using System.Collections.Generic;

namespace Hotfix.LTBY
{
    public class LockFishConfig
    {
        public static FrameData Frame = new FrameData() 
        {
            x = 0, y = 0, w = 880, h = 540,
            Title = "锁定设置",
            Des1 = "<color=#FFCB25FF>勾选锁定鱼种，设置锁定范围。</color>点击",
            Des2 = "若有勾选鱼种，将会自动锁定勾选的鱼进行自动射击；若",
            Des3 = "全不勾选，则需要手动点击鱼进行锁定捕捉。<color=#FFCB25FF>自动锁定开启后，点击鱼切换锁定目标。</color>",
            BtnAll = "全选",
            BtnNone = "全不选",
        };

        public static Dictionary<int, bool> LockFishList = new Dictionary<int, bool>();

        public static void SetLockFishList(Dictionary<int,bool> list)
        {
            LockFishList = list;
        }
        public static void ClearFishList()
        {
            LockFishList.Clear();
        }

        public static bool IsLockFishListEmpty()
        {
            bool empty = true;
            int[] keys = LockFishList.GetDictionaryKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                if (!LockFishList[keys[i]]) continue;
                empty = false;
                break;
            }
            return empty;
        }
    }
}
