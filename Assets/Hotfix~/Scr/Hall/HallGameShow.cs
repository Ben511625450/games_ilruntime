using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LuaFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Hotfix.Hall
{
    public class HallGameShow : ILHotfixEntity
    {
        private enum DragDirection
        {
            None,
            Left,
            Right,
        }

        private GameObject h_Prefab;
        private GameObject s_Prefab;

        private ScrollRect showRect;

        public static Dictionary<string, HttpGame> IconList = new Dictionary<string,HttpGame>()
        {
            {DefaultPlatform,new HttpGame()
            {
                hasFix = true,
                group =new List<string>(){"6","16","18","51","22","27","28","39","41","45","54","56","58","59","62","-64"} 
            }}
        };

        public const string DefaultPlatform = "games_Main";
        public List<int> noShowList = new List<int>();
        public List<int> showList = new List<int>();

        private bool isShowLocal = false;

        private GameObject iconGroup;
        private EventTriggerHelper trigger;
        private Vector3 startPos;
        private Vector3 endPos;
        private bool isMove;

        private Button leftBtn;
        private Button rightBtn;
        private Button backBtn;
        private Transform mutipleContent;
        private const float spaceDistance = 450;
        public Dictionary<string, ScrollRect> contentDic = new Dictionary<string, ScrollRect>();

        public static event CAction<bool, string> OpenSubPlatform;

        public static void DispatchOpenSubPlatform(bool isOpen, string platformName)
        {
            OpenSubPlatform?.Invoke(isOpen, platformName);
        }

        private ScrollRect currentShowContent;

        protected override void Awake()
        {
            base.Awake();
            AddListener();
            showList.Clear();
            noShowList.Clear();
            iconGroup = Util.LoadAsset("module02/Pool/gameimg", $"GameImg");
            if (!isShowLocal) IconList = GameLocalMode.Instance.GWData.GameList;
            if (IconList.Count == 1) //单平台处理
            {
                mutipleContent.gameObject.SetActive(false);
                showRect.gameObject.SetActive(true);
                showRect.movementType = ScrollRect.MovementType.Elastic;
                if (!IconList.ContainsKey(DefaultPlatform))
                {
                    DebugHelper.LogError($"没有找到：{DefaultPlatform} 的配置");
                    return;
                }
                
                HttpGame list = IconList[DefaultPlatform];
                GameLocalMode.Instance.CurrentSelectPlatform = DefaultPlatform;
                currentShowContent = showRect;
                InitHallIcon(list, showRect, iconGroup.transform.FindChildDepth(DefaultPlatform));
                DispatchOpenSubPlatform(true, DefaultPlatform);
            }
            else
            {
                contentDic.Clear();
                mutipleContent.gameObject.SetActive(true);
                showRect.gameObject.SetActive(false);
                foreach (var httpGame in IconList)
                {
                    Transform child = transform.FindChildDepth(httpGame.Key);
                    if (child == null) continue;
                    GameObject o;
                    (o = child.gameObject).SetActive(false);
                    ScrollRect rect = child.GetComponent<ScrollRect>();
                    contentDic.Add(o.name, rect);
                    rect.movementType = ScrollRect.MovementType.Elastic;
                }
                DispatchOpenSubPlatform(false, null);
            }

        }

        protected override void AddEvent()
        {
            base.AddEvent();
            OpenSubPlatform += OnOpenSubPlatform;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            OpenSubPlatform -= OnOpenSubPlatform;
        }

        private void OnOpenSubPlatform(bool isOpen, string platformName)
        {
            currentShowContent?.gameObject.SetActive(isOpen);
            mutipleContent.gameObject.SetActive(!isOpen);
            backBtn.gameObject.SetActive(isOpen && platformName != DefaultPlatform);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ScrollRect[] list = contentDic.GetDictionaryValues();
            for (int i = 0; i < list.Length; i++)
            {
                Transform con = list[i].content.GetChild(i);
                for (int j = con.childCount - 1; j >= 0; j--)
                {
                    HallExtend.Destroy(con.GetChild(j).gameObject);
                }
            }
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            // trigger = EventTriggerHelper.Get(gameObject);
            h_Prefab = transform.FindChildDepth($"prefeb_H").gameObject;
            s_Prefab = transform.FindChildDepth($"prefeb_S").gameObject;

            showRect = transform.FindChildDepth<ScrollRect>(DefaultPlatform);
            // fixContent = transform.FindChildDepth($"Fixed/Mask");

            leftBtn = transform.FindChildDepth<Button>($"LeftBtn");
            rightBtn = transform.FindChildDepth<Button>($"RightBtn");
            mutipleContent = transform.FindChildDepth($"MutipleContent");
            backBtn = transform.FindChildDepth<Button>($"BackBtn");
        }

        private void AddListener()
        {
            // trigger.onBeginDrag = OnBeginDrag;
            // trigger.onEndDrag = OnEndDrag;
            // leftBtn.onClick.RemoveAllListeners();
            // leftBtn.onClick.Add(OnClickLeftCall);
            // rightBtn.onClick.RemoveAllListeners();
            // rightBtn.onClick.Add(OnClickRightCall);

            for (int i = 0; i < mutipleContent.childCount; i++)
            {
                Button btn = mutipleContent.GetChild(i).GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.Add(() => { OnClickSelectPlatformCall(btn.gameObject.name); });
            }

            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.Add(OnClickBackCall);
        }

        private void OnClickBackCall()
        {
            DispatchOpenSubPlatform(false, null);
        }

        private void OnClickSelectPlatformCall(string gameObjectName)
        {
            DispatchOpenSubPlatform(true, gameObjectName);
            GameLocalMode.Instance.CurrentSelectPlatform = gameObjectName;
            if (currentShowContent != null) currentShowContent.gameObject.SetActive(false);
            currentShowContent = contentDic[gameObjectName];
            Transform showIconContent = iconGroup.transform.FindChildDepth(gameObjectName);
            var list = IconList[gameObjectName];
            currentShowContent.gameObject.SetActive(true);
            InitHallIcon(list, currentShowContent, showIconContent);
        }

        IEnumerator DelayRun(float timer, Action action = null)
        {
            if(timer<=0) yield return new WaitForEndOfFrame();
            else  yield return new WaitForSeconds(timer);
            action?.Invoke();
        }
        private void InitHallIcon(HttpGame httpGame, ScrollRect itemRect, Transform showIconContent)
        {
            List<int> _list = new List<int>();
            for (int i = 0; i < httpGame.group.Count; i++)
            {
                int id = int.Parse(httpGame.group[i]);
                if (id > 0)
                {
                    showList.Add(id);
                }
                else
                {
                    noShowList.Add(Mathf.Abs(id));
                }

                _list.Add(Mathf.Abs(id));
            }

            int count = _list.Count;
            Transform fix = itemRect.transform.Find($"FixItem");
            for (int i = 0; i < count; i++)
            {
                Transform parent = null;
                if (httpGame.hasFix )
                {
                    parent = i == 0 ? fix : itemRect.content.GetChild(i - 1);
                }
                else
                {
                    parent = itemRect.content.GetChild(i);
                }

                if (parent.childCount > 0)
                {
                    for (int j = parent.childCount - 1; j >= 0; j--)
                    {
                        HallExtend.Destroy(parent.GetChild(j).gameObject);
                    }
                }

                CreateItem(showIconContent, parent, _list, i);
            }

            for (int i = count; i < itemRect.content.childCount; i++)
            {
                itemRect.content.GetChild(i).gameObject.SetActive(false);
            }
            Behaviour.StartCoroutine(DelayRun(0.1f, () => { currentShowContent.horizontalNormalizedPosition = 0; }));
        }

        private void CreateItem(Transform showIconContent, Transform parent, List<int> _list, int i)
        {
            GameObject go = Object.Instantiate(s_Prefab, parent, false);
            parent.gameObject.SetActive(true);
            // go.transform.localPosition = new Vector3((i - count / 2) * spaceDistance, 0, 0);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMax = Vector2.one;
            rect.anchorMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            HallIconItem item = go.AddILComponent<HallIconItem>();
            int id = _list[i];
            item.SetItem(id, noShowList.Contains(id), showIconContent);
        }
    }
}