using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class CodeLogin : PanelBase
    {
        public CodeLogin() : base(UIType.Top, nameof(CodeLogin))
        {
        }

        private InputField codeInput;
        private Button yesbtn;
        private Button cancelbtn;
        private Button closebtn;

        protected override void FindComponent()
        {
            base.FindComponent();
            codeInput = transform.FindChildDepth<InputField>($"Code");
            yesbtn = transform.FindChildDepth<Button>($"YesBtn");
            cancelbtn = transform.FindChildDepth<Button>($"NoBtn");
            closebtn = transform.FindChildDepth<Button>($"Button_close");
        }

        protected override void AddListener()
        {
            base.AddListener();
            closebtn.onClick.RemoveAllListeners();
            closebtn.onClick.Add(OnCloseWindowCall);
            cancelbtn.onClick.RemoveAllListeners();
            cancelbtn.onClick.Add(OnCloseWindowCall);
            yesbtn.onClick.RemoveAllListeners();
            yesbtn.onClick.Add(OnSendCodeCall);
        }

        private void OnSendCodeCall()
        {
            if (codeInput.text.Length < 4 || codeInput.text.Length > 4)
            {
                ToolHelper.PopSmallWindow($"验证码不正确！");
                ToolHelper.ShowWaitPanel(false);
                UIManager.Instance.Close();
                return;
            }

            HallStruct.LoginCodeAccredit accredit = new HallStruct.LoginCodeAccredit();
            accredit.account = GameLocalMode.Instance.Account.account;
            accredit.code = uint.Parse(codeInput.text);
            HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN,
                DataStruct.LoginStruct.SUB_3D_CS_CODE_LOGIN_VERIFY, accredit.ByteBuffer, SocketType.Hall);
            ToolHelper.ShowWaitPanel(false);
            UIManager.Instance.Close();
        }

        private void OnCloseWindowCall()
        {
            ToolHelper.ShowWaitPanel(false);
            UIManager.Instance.Close();
        }
    }
}