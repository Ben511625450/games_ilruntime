using UnityEngine;
namespace Hotfix
{
    public class ILLauncher
    {
        static GameObject ILRumtimeObj;
        public static void Init()
        {
            DebugHelper.Log($"进入热更代码");
            if (ILRumtimeObj != null)
            {
                UnityEngine.Object.Destroy(ILRumtimeObj);
            }
            ILRumtimeObj = new GameObject("ILRuntimeObj");
            UnityEngine.Object.DontDestroyOnLoad(ILRumtimeObj);
            AddComponent();
            DebugHelper.Log($"热更代码初始化完成");
        }
        public static void UnInit()
        {
            DebugHelper.Log($"退出");
            RemoveComponent();
            GameLocalMode.Reset();
        }
        private static void AddComponent()
        {
            ILRumtimeObj.AddILComponent<EventComponent>();
            ILRumtimeObj.AddILComponent<EntityComponent>();
            ILRumtimeObj.AddILComponent<HotfixGameComponent>();
            ILRumtimeObj.AddILComponent<ActionComponent>();
            ILRumtimeObj.AddILComponent<PopComponent>();
            ILRumtimeObj.AddILComponent<LoadGameComponent>();
            ILRumtimeObj.AddILComponent<ILGameManager>();            

        }
        private static void RemoveComponent()
        {
            Object.Destroy(ILRumtimeObj);
        }
    }
}
