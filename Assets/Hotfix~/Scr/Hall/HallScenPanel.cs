using LuaFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Hotfix.Hall
{
    public class HallScenPanel : PanelBase
    {
        private Button headBtn;
        private Image headIcon;
        private Text id;
        private Text nickName;
        private Text selfGold;
        private Text bankGold;

        private Button gwBtn;
        private Button taskBtn;
        private Button exchangeBtn;
        private Button bankBtn;
        private Button setBtn;
        private Button shopBtn;
        private Text version;
        private Transform midNode;

        private HallGameShow showContent;
        private Transform otherGroup;

        private Image backgroundImg;
        private Sprite defaultSprite;
        public HallScenPanel() : base(UIType.Bottom, nameof(HallScenPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            HallStruct.REQQueryLoginVerify queryLoginVerify = new HallStruct.REQQueryLoginVerify();
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_LOGINVERIFY, queryLoginVerify.ByteBuffer, SocketType.Hall);
            HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN,
                DataStruct.LoginStruct.SUB_3D_CS_REQ_SERVER_LIST, new ByteBuffer(), SocketType.Hall);
        }

        protected override void Awake()
        {
            base.Awake();
            backgroundImg = transform.GetComponent<Image>();
            defaultSprite = backgroundImg.sprite;
            version.text = $"V {Application.version}.{AppConst.valueConfiger.Version}";
            nickName.text = $"{GameLocalMode.Instance.SCPlayerInfo.NickName}";
            id.text = $"ID:{GameLocalMode.Instance.SCPlayerInfo.BeautifulID}";
            headIcon.sprite = ILGameManager.Instance.GetHeadIcon();
            showContent = midNode.AddILComponent<HallGameShow>();
            HallEvent.DispatchEnterGamePre(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(showContent);
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            Transform userInfo = transform.FindChildDepth($"Top/UserInfo");
            headBtn = userInfo.FindChildDepth<Button>($"HeadMask");
            headIcon = headBtn.transform.FindChildDepth<Image>($"HeadIcon");
            nickName = userInfo.FindChildDepth<Text>($"NickName");
            id = userInfo.FindChildDepth<Text>($"ID");

            Transform moneyInfo = transform.FindChildDepth($"Top/MoneyInfo");
            selfGold = moneyInfo.FindChildDepth<Text>($"SelfGoldNum");
            bankGold = moneyInfo.FindChildDepth<Text>($"BankGoldNum");

            otherGroup = transform.FindChildDepth($"Top/OtherBtnGroup");
            gwBtn = otherGroup.FindChildDepth<Button>($"GWBtn");
            gwBtn.gameObject.SetActive(false);
            Transform floorNode = transform.FindChildDepth($"Floor");
            taskBtn = floorNode.FindChildDepth<Button>($"taskBtn");
            exchangeBtn = floorNode.FindChildDepth<Button>($"exchangeBtn");
            bankBtn = floorNode.FindChildDepth<Button>($"BankBtn");
            setBtn = floorNode.FindChildDepth<Button>($"SetsBtn");
            shopBtn = floorNode.FindChildDepth<Button>($"shopBtn");
            version = floorNode.FindChildDepth<Text>($"Version");

            midNode = transform.FindChildDepth($"Mid");
        }

        protected override void AddListener()
        {
            base.AddListener();
            headBtn.onClick.RemoveAllListeners();
            headBtn.onClick.Add(OnClickHeadCall);
            gwBtn.onClick.RemoveAllListeners();
            gwBtn.onClick.Add(OnClickGWCall);
            taskBtn.onClick.RemoveAllListeners();
            taskBtn.onClick.Add(OnClickTaskCall);
            exchangeBtn.onClick.RemoveAllListeners();
            exchangeBtn.onClick.Add(OnClickExchangeCall);
            bankBtn.onClick.RemoveAllListeners();
            bankBtn.onClick.Add(OnClickBankCall);
            setBtn.onClick.RemoveAllListeners();
            setBtn.onClick.Add(OnClickSettingCall);
            shopBtn.onClick.RemoveAllListeners();
            shopBtn.onClick.Add(OnClickShopCall);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.ChangeGoldTicket += HallEventOnChangeGoldTicket;
            HallEvent.EnterGamePre += HallEventOnEnterGamePre;
            HallEvent.OnQueryLoginVerifyCallBack += HallEventOnOnQueryLoginVerifyCallBack;
            HallEvent.ChangeHeader += ChangeHead;
            HallEvent.ChangeHallNiKeName += HallEventOnChangeHallNiKeName;
            HallGameShow.OpenSubPlatform += HallGameShowOnOpenSubPlatform;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.ChangeGoldTicket -= HallEventOnChangeGoldTicket;
            HallEvent.EnterGamePre -= HallEventOnEnterGamePre;
            HallEvent.OnQueryLoginVerifyCallBack -= HallEventOnOnQueryLoginVerifyCallBack;
            HallEvent.ChangeHeader -= ChangeHead;
            HallEvent.ChangeHallNiKeName -= HallEventOnChangeHallNiKeName;
            HallGameShow.OpenSubPlatform -= HallGameShowOnOpenSubPlatform;
        }

        private void HallEventOnOnQueryLoginVerifyCallBack(HallStruct.ACP_SC_QueryLoginVerify obj)
        {
            GameLocalMode.Instance.IsSetLoginValidation = obj.isOn;
            DebugHelper.Log(LitJson.JsonMapper.ToJson(obj));
        }


        private void ChangeHead(int faceID)
        {
            headIcon.sprite = ILGameManager.Instance.GetHeadIcon((uint) faceID);
        }

        private void HallEventOnChangeHallNiKeName()
        {
            nickName.text = GameLocalMode.Instance.SCPlayerInfo.NickName;
        }

        private void HallGameShowOnOpenSubPlatform(bool isOpen,string platformName)
        {
            if (platformName == HallGameShow.DefaultPlatform) return;
            // gwBtn.gameObject.SetActive(!isOpen);
        }

        private void HallEventOnChangeGoldTicket()
        {
            selfGold.text = $"{GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD)}";
        }

        private void HallEventOnEnterGamePre(bool isEnter)
        {
            midNode.gameObject.SetActive(!isEnter);
            otherGroup.gameObject.SetActive(!isEnter);
        }

        /// <summary>
        /// 点击商店
        /// </summary>
        private void OnClickShopCall()
        {
            UIManager.Instance.ReplaceUI<RechargePanel>();
        }

        /// <summary>
        /// 点击设置
        /// </summary>
        private void OnClickSettingCall()
        {
            UIManager.Instance.ReplaceUI<SetPanel>();
        }

        /// <summary>
        /// 点击银行
        /// </summary>
        private void OnClickBankCall()
        {
            UIManager.Instance.ReplaceUI<BankPanel>();
        }

        /// <summary>
        /// 点击兑换
        /// </summary>
        private void OnClickExchangeCall()
        {
            if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
            {
                ToolHelper.PopSmallWindow($"VIP不能进行该操作");
                return;
            }
            UIManager.Instance.ReplaceUI<ExchangePanel>();
        }

        /// <summary>
        /// 点击任务
        /// </summary>
        private void OnClickTaskCall()
        {
            UIManager.Instance.ReplaceUI<TaskPanel>();
        }

        /// <summary>
        /// 点击官网
        /// </summary>
        private void OnClickGWCall()
        {
            UIManager.Instance.ReplaceUI<GWPanel>();
        }

        /// <summary>
        /// 点击头像
        /// </summary>
        private void OnClickHeadCall()
        {
            UIManager.Instance.ReplaceUI<PersonalInfoPanel>();
        }
    }
}