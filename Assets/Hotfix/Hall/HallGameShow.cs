using System.Collections.Generic;
using DG.Tweening;
using LuaFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private Transform showContent;
        private Transform viewContent;

        public static Dictionary<string, List<string>> IconList = new Dictionary<string, List<string>>()
        {
            {
                "games_hc", new List<string>()
                {
                    "62", "24", "3", "20", "51", "19", "6", "22", "42", "48", "50", "-71", "-72", "-73", "-74"
                }
            },
            {
                "games_339", new List<string>()
                {
                    "6", "51", "54", "42", "48", "47", "41", "3", "46", "19", "55", "53"
                }
            },
            {
                "games_ttcm", new List<string>()
                {
                    "6", "49", "46", "40", "47", "51", "16", "19", "41", "55", "42", "48", "43", "20", "50"
                }
            },
            {
                "games_lldzz", new List<string>()
                {
                    "22", "28", "39", "32", "27", "16", "38", "33", "24", "19", "25", "3", "17", "-13", "6", "20"
                }
            },
            {
                "games_yh", new List<string>()
                {
                    "6", "51", "47", "42", "16", "41", "19", "53", "50", "55"
                }
            },
            {
                "games_dcr", new List<string>()
                {
                    "51", "42", "47", "48", "41", "16", "19", "6", "54", "20"
                }
            },
            {
                "games_hy888", new List<string>()
                {
                    "51", "47", "-64", "-65", "-66", "40", "27", "-67", "-68", "62", "-70", "6", "-13", "18", "43", "19"
                }
            },
        };

        public const string DefaultPlatform = "Main";
        public List<int> noShowList = new List<int>();
        public List<int> showList = new List<int>();

        private bool isShowLocal = false;

        private ScrollRect _rect;
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
        public Dictionary<string, RectTransform> contentDic = new Dictionary<string, RectTransform>();

        public static event CAction<bool, string> OpenSubPlatform;

        public static void DispatchOpenSubPlatform(bool isOpen, string platformName)
        {
            OpenSubPlatform?.Invoke(isOpen, platformName);
        }

        private RectTransform currentShowContent;

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
                viewContent.gameObject.SetActive(true);
                if (!IconList.ContainsKey(DefaultPlatform))
                {
                    DebugTool.LogError($"没有找到：{DefaultPlatform} 的配置");
                    return;
                }

                List<string> list = IconList[DefaultPlatform];
                InitHallIcon(list, showContent, iconGroup.transform.FindChildDepth(DefaultPlatform));
            }
            else
            {
                contentDic.Clear();
                mutipleContent.gameObject.SetActive(true);
                viewContent.gameObject.SetActive(false);
                for (int i = 0; i < viewContent.childCount; i++)
                {
                    Transform child = viewContent.GetChild(i);
                    GameObject o;
                    (o = child.gameObject).SetActive(false);
                    contentDic.Add(o.name, child.GetComponent<RectTransform>());
                }
            }

            DispatchOpenSubPlatform(false, null);
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
            viewContent.gameObject.SetActive(isOpen);
            mutipleContent.gameObject.SetActive(!isOpen);
            backBtn.gameObject.SetActive(isOpen);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = 0; i < viewContent.childCount; i++)
            {
                Transform con = viewContent.GetChild(i);
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
            _rect = transform.GetComponent<ScrollRect>();
            _rect.movementType = ScrollRect.MovementType.Elastic;
            viewContent = _rect.transform.FindChildDepth($"View");
            h_Prefab = transform.FindChildDepth($"prefeb_H").gameObject;
            s_Prefab = transform.FindChildDepth($"prefeb_S").gameObject;

            showContent = transform.FindChildDepth(DefaultPlatform);
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
            _rect.content = currentShowContent;
            Transform showIconContent = iconGroup.transform.FindChildDepth(gameObjectName);
            var list = IconList[gameObjectName];
            currentShowContent.gameObject.SetActive(true);
            InitHallIcon(list, currentShowContent, showIconContent);
            _rect.horizontalNormalizedPosition = 0;
        }

        private void OnClickRightCall()
        {
            leftBtn.interactable = false;
            rightBtn.interactable = false;
            FindNearestPos(DragDirection.Left);
        }

        private void OnClickLeftCall()
        {
            leftBtn.interactable = false;
            rightBtn.interactable = false;
            FindNearestPos(DragDirection.Right);
        }

        private void OnEndDrag(GameObject go, PointerEventData data)
        {
            endPos = _rect.content.localPosition;
            if (endPos.x - startPos.x < -50) //往左滑
            {
                FindNearestPos(DragDirection.Left);
            }
            else if (endPos.x - startPos.x > 50) //往右滑
            {
                FindNearestPos(DragDirection.Right);
            }
            else //滑动力度不大
            {
                FindNearestPos(DragDirection.None);
            }

            isMove = false;
        }

        private void OnBeginDrag(GameObject go, PointerEventData data)
        {
            leftBtn.interactable = false;
            rightBtn.interactable = false;
            isMove = true;
            startPos = _rect.content.localPosition;
            HallEvent.DispatchOnMoveHallIcon(false);
        }

        private void FindNearestPos(DragDirection direction)
        {
            _rect.content.DOKill();
            switch (direction)
            {
                case DragDirection.None:
                {
                    int v = (int) _rect.content.localPosition.x / 450;
                    _rect.content.DOLocalMoveX(450 * v, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        HallEvent.DispatchOnMoveHallIcon(true);
                        leftBtn.interactable = true;
                        rightBtn.interactable = true;
                    });
                    break;
                }
                case DragDirection.Right:
                {
                    int v = (int) _rect.content.localPosition.x / 450;
                    if (isMove)
                    {
                        if ((int) _rect.content.localPosition.x % spaceDistance != 0) v++;
                    }
                    else
                    {
                        v++;
                    }

                    _rect.content.DOLocalMoveX(450 * v, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        HallEvent.DispatchOnMoveHallIcon(true);
                        Vector3 pos = new Vector3(_rect.content.GetChild(0).localPosition.x - spaceDistance, 0, 0);
                        _rect.content.GetChild(_rect.content.childCount - 1).localPosition = pos;
                        _rect.content.GetChild(_rect.content.childCount - 1).SetAsFirstSibling();
                        leftBtn.interactable = true;
                        rightBtn.interactable = true;
                    });
                }
                    break;
                case DragDirection.Left:
                {
                    int v = (int) _rect.content.localPosition.x / 450;
                    if (isMove)
                    {
                        if ((int) _rect.content.localPosition.x % spaceDistance != 0) v--;
                    }
                    else
                    {
                        v--;
                    }

                    _rect.content.DOLocalMoveX(450 * v, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        HallEvent.DispatchOnMoveHallIcon(true);
                        Vector3 pos =
                            new Vector3(
                                _rect.content.GetChild(_rect.content.childCount - 1).localPosition.x + spaceDistance, 0,
                                0);
                        _rect.content.GetChild(0).localPosition = pos;
                        _rect.content.GetChild(0).SetAsLastSibling();
                        leftBtn.interactable = true;
                        rightBtn.interactable = true;
                    });
                }
                    break;
            }
        }

        private void InitHallIcon(List<string> list, Transform content, Transform showIconContent)
        {
            List<int> _list = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                int id = int.Parse(list[i]);
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
            for (int i = 0; i < count; i++)
            {
                // GameObject go = i == 0
                //     ? Object.Instantiate(h_Prefab, fixContent)
                //     : Object.Instantiate(s_Prefab, showContent);
                Transform parent = content.GetChild(i);
                if (parent.childCount > 0)
                {
                    for (int j = parent.childCount - 1; j >= 0; j--)
                    {
                        HallExtend.Destroy(parent.GetChild(j).gameObject);
                    }
                }

                GameObject go = Object.Instantiate(s_Prefab, parent,false);
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

            for (int i = count; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}