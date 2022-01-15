using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class UpdateBankPWPanel : PanelBase
    {
        private Button backBtn;
        private Button CloseBtn;
        private Button codeBtn;
        private InputField codeText;
        private float codeTime;

        private int getCodeTime;
        private bool isGetCode;

        private Text phoneText;
        private InputField PWText;
        private Button resetBtn;
        private InputField surePWText;
        private Text timetext;


        public UpdateBankPWPanel() : base(UIType.Middle, nameof(UpdateBankPWPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            Init();
            isGetCode = false;
            getCodeTime = 60;
        }


        protected override void AddEvent()
        {
            base.AddEvent();

            HallEvent.LogonFindPW_GetCode += UpdatePwdCodeSC;
            HallEvent.Bank_Change_PW += UpdatePwdSuccess;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.LogonFindPW_GetCode -= UpdatePwdCodeSC;
            HallEvent.Bank_Change_PW -= UpdatePwdSuccess;
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
            phoneText = transform.FindChildDepth<Text>("phone/Text");
            PWText = transform.FindChildDepth<InputField>("pwd/InputField");
            surePWText = transform.FindChildDepth<InputField>("surepwd/InputField");
            codeText = transform.FindChildDepth<InputField>("code/InputField");
            codeBtn = transform.FindChildDepth<Button>("codebtn");
            backBtn = transform.FindChildDepth<Button>("BackBtn");
            resetBtn = transform.FindChildDepth<Button>("ResetBtn");
            CloseBtn = transform.FindChildDepth<Button>("CloseBtn");
            timetext = codeBtn.transform.FindChildDepth<Text>("Time");
        }

        protected override void AddListener()
        {
            codeBtn.onClick.RemoveAllListeners();
            codeBtn.onClick.Add(UpdatePwdCodeBtn);

            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.Add(UpdateBackOnClick);

            CloseBtn.onClick.RemoveAllListeners();
            CloseBtn.onClick.Add(UpdateBackOnClick);

            resetBtn.onClick.RemoveAllListeners();
            resetBtn.onClick.Add(() => { UpdateResetBtnOnClick(resetBtn); });
        }

        private void UpdateResetBtnOnClick(Button args)
        {
            ILMusicManager.Instance.PlayBtnSound();
            args.interactable = false;

            if (PWText.text.Length < 6 || PWText.text.Length > 12)
            {
                ToolHelper.PopSmallWindow("密码格式有误");
                args.interactable = true;
                return;
            }

            if (codeText.text.Length < 4 || codeText.text.Length > 8)
            {
                ToolHelper.PopSmallWindow("验证码输入有误");
                args.interactable = true;
                return;
            }

            if (PWText.text != surePWText.text)
            {
                ToolHelper.PopSmallWindow("两次输入密码不同");
                args.interactable = true;
                return;
            }

            var phoneNum = GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber;
            var _PW = MD5Helper.MD5String(PWText.text);
            var code = int.Parse(codeText.text);
            var PlatformID = (byte) GameLocalMode.Instance.PlatformID;
            var resetPW = new HallStruct.REQ_CS_Bank_Reset_Password(phoneNum, _PW, code, PlatformID);
            HotfixGameComponent.Instance.Send(DataStruct.BankStruct.MDM_GP_USER,
                DataStruct.BankStruct.SUB_GP_MODIFY_BANK_PASSWD, resetPW._ByteBuffer, SocketType.Hall);
        }

        private void UpdatePwdSuccess(HallStruct.ACP_SC_Bank_Change_PW data)
        {
            resetBtn.GetComponent<Button>().interactable = true;
            if (data.cbSuccess == 1)
            {
                ToolHelper.PopSmallWindow("重置密码成功，请妥善保管");
                return;
            }

            ToolHelper.PopSmallWindow(data.szInfoDiscrib);
        }


        private void UpdateBackOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.Close();
        }

        /// <summary>
        ///     获取验证码
        /// </summary>
        private void UpdatePwdCodeBtn()
        {
            ILMusicManager.Instance.PlayBtnSound();
            var phonenum = GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber;
            if (string.IsNullOrEmpty(phonenum))
            {
                ToolHelper.PopSmallWindow("请输入正确手机号码");
                return;
            }

            if (phonenum.Length != 11)
            {
                ToolHelper.PopSmallWindow("请输入正确手机号码");
                return;
            }

            codeBtn.interactable = false;
            codeBtn.transform.FindChildDepth($"Show").gameObject.SetActive(false);
            timetext.gameObject.SetActive(true);
            timetext.text = "60s重发";
            getCodeTime = 60;
            GetCodeRemTime(getCodeTime);
            isGetCode = true;
            var check_Code = new HallStruct.REQ_CS_Bank_Code(phonenum);
            HotfixGameComponent.Instance.Send(DataStruct.BankStruct.MDM_GP_USER,
                DataStruct.BankStruct.SUB_GP_MODIFY_BANK_PASSWD_CHECK_CODE, check_Code._ByteBuffer, SocketType.Hall);
        }

        /// <summary>
        ///     获取验证码返回
        /// </summary>
        /// <param name="code"></param>
        private void UpdatePwdCodeSC(int code)
        {
            if (code > 0) return;
            ToolHelper.PopSmallWindow("找不到该手机号");
            codeBtn.transform.FindChildDepth("Show").gameObject.SetActive(true);
            timetext.gameObject.SetActive(false);
            codeBtn.interactable = true;
        }

        /// <summary>
        ///     验证码时间
        /// </summary>
        /// <param name="num"></param>
        private void GetCodeRemTime(int num)
        {
            timetext.text = num + "s重发";
            if (num > 0) return;
            timetext.gameObject.SetActive(false);
            codeBtn.transform.FindChildDepth("Show").gameObject.SetActive(true);
            isGetCode = false;
            codeBtn.interactable = true;
        }

        private void Init()
        {
            if (GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber == "") return;
            var str = GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber;
            var str1 = str.Substring(0, 3);
            var str2 = str.Substring(7, 4);
            phoneText.text = $"{str1}****{str2}";
        }
    }
}