using System.Collections.Generic;
using Hotfix.Hall;
using LuaFramework;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    ///     ui类型
    /// </summary>
    public enum UIType
    {
        /// <summary>
        ///     底层
        /// </summary>
        Bottom,

        /// <summary>
        ///     中间层
        /// </summary>
        Middle,

        /// <summary>
        ///     顶层
        /// </summary>
        Top,
        /// <summary>
        /// 提示
        /// </summary>
        TipWindow,
        
        /// <summary>
        /// 提示
        /// </summary>
        SmallTipWindow,
    }

    public class UIManager : SingletonILEntity<UIManager>
    {
        private Transform BottomPanel;
        private Transform CentralPanel;
        private Transform Pools;
        private Transform TopPanel;
        private Transform TipPanel;
        private readonly List<PanelBase> uiList = new List<PanelBase>();
        private readonly Dictionary<string, PanelBase> uiMap = new Dictionary<string, PanelBase>();

        protected override void Awake()
        {
            base.Awake();
            Util.GC();
            Application.targetFrameRate = 60;
            uiList.Clear();
            uiMap.Clear();
            Pools = transform.FindChildDepth("Pools");
            Pools.localPosition = Vector3.one * 10000;
            BottomPanel = transform.FindChildDepth("BottomPanel");
            CentralPanel = transform.FindChildDepth("CentralPanel");
            TopPanel = transform.FindChildDepth("TopPanel");
            TipPanel = transform.FindChildDepth("TipPanel");
            // Util.LoadAsset("module02/Pool/font", "font");
            ILMusicManager.Instance.PlayBackgroundMusic();

            if (Util.isPc)
            {
                GameLocalMode.Instance.SetScreen(UnityEngine.ScreenOrientation.Landscape);
            }

            OpenUI<LogonScenPanel>();
        }

        public const string ScreenOrientation = "ScreenOrientation";
        private string lastOrientation;
        protected override void Update()
        {
            base.Update();
            if (Util.isPc)
            {
                bool hasKey = ES3.KeyExists(ScreenOrientation);
                if (!hasKey) return;
                string orientation = ES3.Load<string>(ScreenOrientation);
                if (lastOrientation == orientation) return;
                if (orientation == UnityEngine.ScreenOrientation.Portrait.ToString())
                {
                    AspectRatioController.Instance.SetAspectRatio(9, 16, true);
                }
                else
                {
                    AspectRatioController.Instance.SetAspectRatio(16, 9, true);
                }

                lastOrientation = orientation;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToolHelper.PopBigWindow(new BigMessage
                {
                    content = "是否退出游戏?",
                    okCall = () =>
                    {
                        Application.Quit();
                    }
                });
            }
        }

        /// <summary>
        /// 更换ui
        /// </summary>
        /// <param name="args">参数</param>
        /// <typeparam name="T">ui</typeparam>
        /// <returns></returns>
        public T ReplaceUI<T>(params object[] args) where T : PanelBase, new()
        {
            Close();
            return OpenUI<T>(args);
        }

        /// <summary>
        /// 打开ui
        /// </summary>
        /// <param name="args">参数</param>
        /// <typeparam name="T">ui</typeparam>
        /// <returns></returns>
        public T OpenUI<T>(params object[] args) where T : PanelBase, new()
        {
            var uiName = typeof(T).Name;
            var go = FindPoolsExist(uiName);
            var t = CreateUI<T>(go, args);
            if (t == null) return null;
            //监听页面被销毁，清除他在iViewList中的保存
            t.OnDestroyFinishHandle = baseView =>
            {
                uiList.Remove(baseView);
                uiMap.Remove(uiName);
            };
            if (t.uitype == UIType.SmallTipWindow) return t;
            uiList.Add(t);
            if (uiMap.ContainsKey(uiName)) uiMap.Remove(uiName);
            uiMap.Add(uiName, t);

            return t;
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="trans">ui主体</param>
        /// <param name="args">参数</param>
        /// <typeparam name="T">ui类</typeparam>
        /// <returns></returns>
        private T CreateUI<T>(Transform trans, params object[] args) where T : PanelBase, new()
        {
            var uiName = typeof(T).Name;
            string path = $"module02/Pool/{uiName.ToLower()}";
            if (trans == null) trans = Object.Instantiate(Util.LoadAsset(path, uiName)).transform;
            if (trans == null) return null;
            trans.gameObject.name = uiName;
            var parent = BottomPanel;
            var t = trans.AddILComponent<T>();
            switch (t.uitype)
            {
                case UIType.Bottom:
                    parent = BottomPanel;
                    break;
                case UIType.Middle:
                    parent = CentralPanel;
                    break;
                case UIType.Top:
                    parent = TopPanel;
                    break;
                case UIType.TipWindow:
                case UIType.SmallTipWindow:
                    parent = TipPanel;
                    break;
            }

            trans.SetParent(parent);
            trans.localPosition = Vector3.one;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
            trans.GetComponent<RectTransform>().anchorMax = Vector2.one;
            trans.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            trans.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            trans.GetComponent<RectTransform>().offsetMin = Vector2.zero;

            t.Create(args);
            return t;
        }

        /// <summary>
        /// 获取ui
        /// </summary>
        /// <typeparam name="T">ui类</typeparam>
        /// <returns></returns>
        public T GetUI<T>() where T : PanelBase
        {
            var uiName = typeof(T).Name;
            if (string.IsNullOrWhiteSpace(uiName))
            {
                DebugHelper.Log("UI名字不能为空");
                return null;
            }

            if (uiMap.ContainsKey(uiName)) return uiMap[uiName] as T;
            return null;
        }

        /// <summary>
        /// 关闭指定ui
        /// </summary>
        /// <typeparam name="T">ui类</typeparam>
        public void CloseUI<T>() where T : PanelBase
        {
            var uiName = typeof(T).Name;
            if (!uiMap.ContainsKey(uiName)) return;
            var ui = uiMap[uiName];
            CloseUI(ui);
        }

        public void CloseUI<T>(T ui) where T : PanelBase
        {
            var keys = uiMap.GetDictionaryKeys();
            for (var i = 0; i < keys.Length; i++)
            {
                if (!keys[i].Equals(ui.Behaviour.BehaviourName)) continue;
                if (uiMap[keys[i]] != ui) continue;
                uiMap.Remove(ui.Behaviour.BehaviourName);
                break;
            }

            for (var i = 0; i < uiList.Count; i++)
            {
                if (!uiList[i].Behaviour.BehaviourName.Equals(ui.Behaviour.BehaviourName)) continue;
                Destroy(ui);
                ui.transform.SetParent(Pools);
                ui.OnDestroyFinishHandle = null;
                uiList.RemoveAt(i);
                break;
            }
        }

        /// <summary>
        /// 关闭顶层UI
        /// </summary>
        public void Close()
        {
            if (uiList.Count <= 1) return;
            var panel = uiList[uiList.Count - 1];
            if (panel == null)
            {
                uiList.RemoveAt(uiList.Count - 1);
                return;
            }

            panel.transform.SetParent(Pools);
            panel.transform.localScale = Vector3.zero;
            Destroy(panel);
        }

        /// <summary>
        /// 关闭所有ui
        /// </summary>
        public void CloseAllUI()
        {
            for (var i = 0; i < uiList.Count; i++)
            {
                if (uiList[i] == null) continue;
                DebugHelper.Log($"关闭{uiList[i].Behaviour.BehaviourName}");
                uiList[i]?.transform.SetParent(Pools);
                Destroy(uiList[i]);
            }

            uiList.Clear();
            uiMap.Clear();
        }

        private Transform FindPoolsExist(string name)
        {
            return Pools.Find(name);
        }
    }
}