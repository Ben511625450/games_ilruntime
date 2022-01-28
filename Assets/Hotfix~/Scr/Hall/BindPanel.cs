using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class BindPanel : PanelBase
    {
        private Button closeBtn;
        private InputField code;
        private float codeTime;
        private Transform codeTimeText;
        private Transform codeTipText;
        private Button getCodeBtn;


        private int getCodeTime;
        private bool isGetCode;
        private InputField logonPW;
        private Transform mainPanel;

        private Button maskCloseBtn;

        private InputField phoneNum;

        private Button sureBtn;
        public BindPanel() : base(UIType.Bottom, nameof(BindPanel))
        {

        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            getCodeTime = 120;
            codeTime = 0f;
        }

        protected override void Update()
        {
            base.Update();
            if (!isGetCode) return;
            codeTime += Time.deltaTime;
            if (codeTime < 1) return;
            codeTime = 0f;
            getCodeTime--;
            GetCodeRemTime(getCodeTime);
        }


        protected override void FindComponent()
        {
            mainPanel = transform.FindChildDepth("mainPanel");

            maskCloseBtn = transform.FindChildDepth<Button>("Mask");
            closeBtn = mainPanel.FindChildDepth<Button>("CloseBtn");

            phoneNum = mainPanel.FindChildDepth<InputField>("SJHM/InputField");
            logonPW = mainPanel.FindChildDepth<InputField>("DLMM/InputField");
            code = mainPanel.FindChildDepth<InputField>("YZM/InputField");
            getCodeBtn = mainPanel.FindChildDepth<Button>("HQTZMBtn");
            codeTipText = mainPanel.FindChildDepth("HQTZMBtn/Image");
            codeTimeText = mainPanel.FindChildDepth("HQTZMBtn/Text");
            sureBtn = mainPanel.FindChildDepth<Button>("SureBtn");
        }

        protected override void AddListener()
        {
            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(CloseBtnOnClick);
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseBtnOnClick);

            getCodeBtn.onClick.RemoveAllListeners();
            getCodeBtn.onClick.Add(GetCodeBtnOnClick);

            sureBtn.onClick.RemoveAllListeners();
            sureBtn.onClick.Add(SureBtnOnClick);
        }

        private void GetCodeBtnOnClick()
        {
            if (phoneNum.text == "")
            {
                ToolHelper.PopSmallWindow("手机号码不能为空");
                return;
            }

            if (phoneNum.text.Length != 11)
            {
                ToolHelper.PopSmallWindow("请输入11位手机号码");
                return;
            }

            getCodeBtn.interactable = false;
            getCodeTime = 120;
            isGetCode = true;
            codeTipText.gameObject.SetActive(false);
            codeTimeText.GetComponent<Text>().text = $"{getCodeTime}s重发";
            codeTimeText.gameObject.SetActive(true);
            HallEvent.SC_BindCodeCallBack += GetCodeCallBack;
            var bind = new HallStruct.REQ_CS_BindGetCode("1", "1", phoneNum.text);
            HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN,
                DataStruct.LoginStruct.SUB_3D_SC_DOWN_GAME_RESOURCE, bind._ByteBuffer, SocketType.Hall);
        }

        private void GetCodeRemTime(int Time)
        {
            codeTimeText.GetComponent<Text>().text = $"{Time}s重发";
            if (Time > 0) return;
            isGetCode = false;
            codeTipText.gameObject.SetActive(true);
            codeTimeText.gameObject.SetActive(false);
            getCodeBtn.interactable = true;
            codeTimeText.GetComponent<Text>().text = "";
        }


        private void GetCodeCallBack(uint index)
        {
            HallEvent.SC_BindCodeCallBack -= GetCodeCallBack;
            if (index == 0) ToolHelper.PopSmallWindow("手机号已经绑定");
        }

        /// <summary>
        ///     给服务器发送绑定手机号的信息
        /// </summary>
        private void SureBtnOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();

            sureBtn.interactable = false;
            var NewPhoneNumber = phoneNum.text;
            var Code = code.text;
            var PWText = logonPW.text;


            if (string.IsNullOrEmpty(NewPhoneNumber) || NewPhoneNumber.Length != 11)
            {
                ToolHelper.PopSmallWindow("请输入有效手机号");
                return;
            }

            if (Code.Length < 4 || Code.Length > 8)
            {
                ToolHelper.PopSmallWindow("请确认验证码是否正确");
                return;
            }

            if (string.IsNullOrEmpty(PWText))
            {
                ToolHelper.PopSmallWindow("请输入密码");
                return;
            }

            sureBtn.interactable = false;
            HallEvent.SC_CHANGE_ACCOUNT += BindAccCallBack;
            var acc = new HallStruct.REQ_CS_ChangeAccount(NewPhoneNumber, int.Parse(Code), Md5Helper.GetString(PWText));
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_CHANGE_ACCOUNT, acc._ByteBuffer, SocketType.Hall);
        }

        private void BindAccCallBack(HallStruct.ACP_SC_CHANGE_ACCOUNT data)
        {
            if (data == null)
            {
                ToolHelper.PopSmallWindow("绑定成功");
                Clear();
                UIManager.Instance.Close();
            }
            else
            {
                ToolHelper.PopSmallWindow($"绑定手机号失败:{data.Error}");
            }
        }

        private void CloseBtnOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();
            Clear();
            UIManager.Instance.Close();
        }

        private void Clear()
        {
            phoneNum.text = "";
            logonPW.text = "";
            code.text = "";
        }
    }
}
