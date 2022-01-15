using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class ExchangePanel : PanelBase
    {
        private Text cardText;

        private Button closeBtn;
        private Text goldText;
        private Transform GrouoItem;

        private Text idText;
        private Button maskCloseBtn;

        private Button ReceiveBtn;
        private Text timeText;


        public ExchangePanel() : base(UIType.Middle, nameof(ExchangePanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_DIANKA_QUERY, new ByteBuffer(), SocketType.Hall);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.DIANKA_QUERY += InitData;
            HallEvent.DIANKA_RECEIVE += OnReceiveData;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.DIANKA_QUERY -= InitData;
            HallEvent.DIANKA_RECEIVE -= OnReceiveData;
        }


        protected override void FindComponent()
        {
            ReceiveBtn = transform.FindChildDepth<Button>("Content/ReceiveBtn");

            idText = transform.FindChildDepth<Text>("Content/right/IDText");
            timeText = transform.FindChildDepth<Text>("Content/right/TimeText");
            cardText = transform.FindChildDepth<Text>("Content/right/CardText");
            goldText = transform.FindChildDepth<Text>("Content/right/GoldText");
            GrouoItem = transform.FindChildDepth("Content/Group/Item");

            closeBtn = transform.FindChildDepth<Button>("Mask");
            maskCloseBtn = transform.FindChildDepth<Button>("Content/Close");
            ReceiveBtn.gameObject.SetActive(false); //优先隐藏
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseExchangePanel);

            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(CloseExchangePanel);

            ReceiveBtn.onClick.RemoveAllListeners();
            ReceiveBtn.onClick.Add(OnClickReceive);
        }

        private void InitData(HallStruct.ACP_SC_DIANKA_QUERY data)
        {
            HallEvent.DIANKA_QUERY -= InitData;
            GrouoItem.gameObject.SetActive(true);
            ReceiveBtn.gameObject.SetActive(data.Count > 0);

            if (data.Count <= 0) return;

            var dt = data.Timer.StampToDatetime();
            idText.text = data.ID.ToString();
            timeText.text = $"{dt:yyyy-MM-dd}";
            cardText.text = data.Card;
            goldText.text = data.Gold.ToString();
        }

        private void OnReceiveData(HallStruct.ACP_SC_DIANKA_RECEIVE data)
        {
            HallEvent.DIANKA_RECEIVE -= OnReceiveData;
            ToolHelper.PopSmallWindow(data.Msg);
            ILGameManager.Instance.QuerySelfGold();
            GrouoItem.gameObject.SetActive(false);
            ReceiveBtn.gameObject.SetActive(false);
            ReceiveBtn.interactable = true;
        }

        private void OnClickReceive()
        {
            ReceiveBtn.interactable = false;
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_DIANKA_RECEIVE, new ByteBuffer(), SocketType.Hall);
        }

        private void CloseExchangePanel()
        {
            UIManager.Instance.Close();
        }
    }
}