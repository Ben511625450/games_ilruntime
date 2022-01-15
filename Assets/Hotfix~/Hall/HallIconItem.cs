using DG.Tweening;
using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class HallIconItem : ILHotfixEntity
    {
        private bool isHide;

        private Transform hideMask;
        private Button enterGameBtn;
        private Transform iconNode;

        private int Id;
        private const float distance = 450;

        protected override void Awake()
        {
            base.Awake();
            AddListener();
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            hideMask = transform.FindChildDepth($"open");
            iconNode = transform.FindChildDepth($"ICO");
            enterGameBtn = iconNode.GetComponent<Button>();
        }

        private void AddListener()
        {
            enterGameBtn.onClick.RemoveAllListeners();
            enterGameBtn.onClick.Add(OnClickEnterGameCall);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            // HallEvent.OnMoveHallIcon += HallEventOnOnMoveHallIcon;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            // HallEvent.OnMoveHallIcon -= HallEventOnOnMoveHallIcon;
        }

        private void HallEventOnOnMoveHallIcon(bool isMove)
        {
            transform.DOKill();
            if (!isMove)
            {
                transform.DOScale(0.6f, 0.1f).SetEase(Ease.Linear);
                return;
            }

            if (transform.localPosition.x + transform.parent.localPosition.x > -50 &&
                transform.localPosition.x + transform.parent.localPosition.x < 50)
            {
                transform.DOScale(1f, 0.1f).SetEase(Ease.Linear);
            }
            else
            {
                transform.DOScale(0.6f, 0.1f).SetEase(Ease.Linear);
            }
        }

        /// <summary>
        /// 移动完成
        /// </summary>
        /// <param name="moveType">移动类型-1左滑 0 没有滑 1 右滑</param>
        private void MoveComplete(int moveType)
        {
            if (moveType == 0) return;
            if (moveType == -1)
            {
                if (transform.GetSiblingIndex() != 0) return;
                DebugHelper.LogError(transform.GetSiblingIndex());
                int count = transform.parent.childCount;
                Vector3 pos = new Vector3(transform.parent.GetChild(count - 1).localPosition.x + distance, 0, 0);
                transform.localPosition = pos;
                transform.SetAsLastSibling();
            }
            else if (moveType == 1)
            {
                int count = transform.parent.childCount;
                if (transform.GetSiblingIndex() != count - 1) return;
                Vector3 pos = new Vector3(transform.parent.GetChild(0).localPosition.x - distance, 0, 0);
                transform.localPosition = pos;
                transform.SetAsFirstSibling();
            }
        }

        private void OnClickEnterGameCall()
        {
            DebugHelper.LogError($"点击{Id}");
            if (Id >= 255)
            {
                ToolHelper.PopSmallWindow($"当前游戏未开放，敬请期待!");
                return;
            }

            if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
            {
                ToolHelper.PopSmallWindow($"VIP 无法进入游戏!");
                return;
            }

            var list = GameLocalMode.Instance.AllSCGameRoom.FindAllItem(p => p._2wGameID == Id);
            if (list == null || list.Count <= 0)
            {
                GameData data= GameConfig.GetGameData(Id);
                list = GameLocalMode.Instance.AllSCGameRoom.FindAllItem(p => p._2wGameID == data.otherClientId);
                if (list == null || list.Count <= 0)
                {
                    ToolHelper.PopSmallWindow($"游戏暂未开放");
                    return;
                }
            }

            HallEvent.DispatchEnterGamePre(true);
            UIManager.Instance.OpenUI<GameRoomListPanel>(Id, list);
        }

        public void SetItem(int i, bool _isHide, Transform iconGroup)
        {
            isHide = _isHide;
            Id = i;
            hideMask.gameObject.SetActive(isHide);
            GameData data = GameConfig.GetGameData(Id);
            if (data == null)
            {
                DebugHelper.LogError($"未找到{Id} 配置");
                return;
            }

            gameObject.name = data.scenName;
            Transform obj = iconGroup.FindChildDepth(data.scenName);
            if (obj == null)
            {
                DebugHelper.LogError($"未找到 {data.scenName} icon");
                return;
            }

            if (transform.localPosition.x + transform.parent.localPosition.x > -50 &&
                transform.localPosition.x + transform.parent.localPosition.x < 50)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.one * 0.6f;
            }
            transform.localScale = Vector3.one;

            GameObject go = Object.Instantiate(obj.gameObject, iconNode);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
        }
    }
}