using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class PersonalInfoPanel : PanelBase
    {
        private Text bankText;
        private Button ChangeHeadBtn;

        private Button closeBtn;
        private Text goldText;
        private Image headIamge;
        private Button IDCopyBtn;
        private Text IDText;
        private InputField LeaveWordText;
        private Transform mainPanel;
        private Button maskCloseBtn;
        private InputField NickNameText;
        private Button SureBtn;
        private Text tipText;

        public PersonalInfoPanel() : base(UIType.Middle, nameof(PersonalInfoPanel))
        {

        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            UpdataPersonalInfo();
        }
        protected override void FindComponent()
        {
            mainPanel = transform.FindChildDepth("Bg/mainPanel");
            goldText = mainPanel.FindChildDepth<Text>("GoldText/Text");
            bankText = mainPanel.FindChildDepth<Text>("BankText/Text");
            IDText = mainPanel.FindChildDepth<Text>("IDText/Text");
            IDCopyBtn = mainPanel.FindChildDepth<Button>("IDText/Copy");
            NickNameText = mainPanel.FindChildDepth<InputField>("NickNameText/InputField");
            tipText = mainPanel.FindChildDepth<Text>("NickNameText/tipText");
            headIamge = mainPanel.FindChildDepth<Image>("HeadBg/Mask/Icon");
            SureBtn = mainPanel.FindChildDepth<Button>("SureBtn");
            LeaveWordText = mainPanel.FindChildDepth<InputField>("LeaveWordText/InputField");
            ChangeHeadBtn = mainPanel.FindChildDepth<Button>("ChangeHeadBtn");

            closeBtn = transform.FindChildDepth<Button>("Bg/CloseBtn");
            maskCloseBtn = transform.FindChildDepth<Button>("Mask");
        }
        protected override void AddListener()
        {
            IDCopyBtn.onClick.RemoveAllListeners();
            IDCopyBtn.onClick.Add(CopyIDCall);

            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(PersonalCloseBtnOnClick);
            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(PersonalCloseBtnOnClick);

            SureBtn.onClick.RemoveAllListeners();
            SureBtn.onClick.Add(LeaveWordSureBtnOnClick);

            ChangeHeadBtn.onClick.RemoveAllListeners();
            ChangeHeadBtn.onClick.Add(OpenChangeHeadOnClick);

            NickNameText.onEndEdit.RemoveAllListeners();
            NickNameText.onEndEdit.Add(str => { UpdataNickNameOnClick(str); });
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.ChangeGoldTicket += UpdatePanleGoldTextInfo;
            HallEvent.ChangeHeader += ChangeHead;
            HallEvent.Change_Sign += LeaveWordCallBack;
            HallEvent.SC_UpdataNickName += UpdataNickNameCallBack;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.ChangeGoldTicket -= UpdatePanleGoldTextInfo;
            HallEvent.ChangeHeader -= ChangeHead;
            HallEvent.Change_Sign -= LeaveWordCallBack;
            HallEvent.SC_UpdataNickName -= UpdataNickNameCallBack;
        }

        /// <summary>
        ///     修改签名确认
        /// </summary>
        private void LeaveWordSureBtnOnClick()
        {
            var str = LeaveWordText.text;
            if (string.IsNullOrEmpty(str)) return;
            if (str == GameLocalMode.Instance.SCPlayerInfo.Sign) return;

            var modifyInfo = new HallStruct.REQ_CS_ModifyInfo(str);
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_CHANGE_SIGN, modifyInfo._ByteBuffer, SocketType.Hall);
        }

        /// <summary>
        ///     修改签名返回
        /// </summary>
        /// <param name="sign"></param>
        private void LeaveWordCallBack(HallStruct.ACP_SC_CHANGE_SIGN sign)
        {
            if (sign != null)
            {
                ToolHelper.PopBigWindow(new BigMessage
                {
                    content = sign.Error,
                    okCall = () => { creatUpdataSignError(); },
                    cancelCall = () => { creatUpdataSignError(); }
                });
            }
            else
            {
                GameLocalMode.Instance.SCPlayerInfo.Sign = LeaveWordText.text;
                ToolHelper.PopSmallWindow("修改签名成功");
            }
        }

        /// <summary>
        ///     失败签名
        /// </summary>
        private void creatUpdataSignError()
        {
            LeaveWordText.text = GameLocalMode.Instance.SCPlayerInfo.Sign;
        }

        /// <summary>
        ///     关闭个人信息
        /// </summary>
        private void PersonalCloseBtnOnClick()
        {
            UIManager.Instance.Close();
            ILMusicManager.Instance.PlayBtnSound();
        }

        /// <summary>
        ///     复制ID
        /// </summary>
        private void CopyIDCall()
        {
            ILMusicManager.Instance.PlayBtnSound();
            var strID = IDText.text;
            ToolHelper.SetText(strID);
            ToolHelper.PopSmallWindow("复制ID成功");
        }

        /// <summary>
        ///     打开修改头像
        /// </summary>
        private void OpenChangeHeadOnClick()
        {
            UIManager.Instance.OpenUI<ChangeHeadPanel>();
        }

        /// <summary>
        ///     修改昵称
        /// </summary>
        /// <param name="str"></param>
        private void UpdataNickNameOnClick(string str)
        {
            var NewNickName = str;
            NewNickName = string.Format(NewNickName, " ", "");
            if (NewNickName == GameLocalMode.Instance.SCPlayerInfo.NickName) return;
            if (NewNickName.Length == 0 || NewNickName.Length > 7)
            {
                ToolHelper.PopBigWindow(new BigMessage
                {
                    content = "输入的昵称有误，请重新输入",
                    okCall = ()=> 
                    {
                        CreatUpdataNameError();
                    },
                    cancelCall =()=> { CreatUpdataNameError(); } 
                });
                return;
            }

            var nikeName = new HallStruct.REQ_CS_ModifyNiKeName(NewNickName,
                GameLocalMode.Instance.SCPlayerInfo.Sex);
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_CHANGE_NICKNAME, nikeName._ByteBuffer, SocketType.Hall);
        }

        /// <summary>
        ///     失败昵称
        /// </summary>
        private void CreatUpdataNameError()
        {
            NickNameText.text = GameLocalMode.Instance.SCPlayerInfo.NickName;
        }

        /// <summary>
        ///     修改昵称返回
        /// </summary>
        /// <param name="obj"></param>
        private void UpdataNickNameCallBack(HallStruct.ACP_SC_UpdataNickName obj)
        {
            if (obj != null)
            {
                ToolHelper.PopBigWindow(new BigMessage
                {
                    content = obj.Error,
                    okCall = CreatUpdataNameError,
                    cancelCall = CreatUpdataNameError
                });
            }
            else
            {
                GameLocalMode.Instance.SCPlayerInfo.NickName = NickNameText.text;
                HallEvent.DispatchChangeHallNiKeName();
                ToolHelper.PopSmallWindow("修改成功");
            }
        }

        /// <summary>
        ///     更新金币
        /// </summary>
        private void UpdatePanleGoldTextInfo()
        {
            if (goldText == null) return;
            goldText.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD).ToString();
        }

        /// <summary>
        ///     更新个人信息
        /// </summary>
        private void UpdataPersonalInfo()
        {
            NickNameText.text = GameLocalMode.Instance.SCPlayerInfo.NickName;
            IDText.text = GameLocalMode.Instance.SCPlayerInfo.BeautifulID.ToString();
            goldText.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD).ToString();
            headIamge.sprite = ILGameManager.Instance.GetHeadIcon();
            LeaveWordText.text = GameLocalMode.Instance.SCPlayerInfo.Sign;
            tipText.text = GameLocalMode.Instance.GWData.CNameGold;
        }

        private void ChangeHead(int faceID)
        {
            GameLocalMode.Instance.SCPlayerInfo.FaceID = (uint) faceID;
            headIamge.sprite = ILGameManager.Instance.GetHeadIcon();
        }
    }
}