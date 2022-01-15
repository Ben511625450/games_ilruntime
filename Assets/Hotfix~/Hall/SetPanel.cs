using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class SetPanel : PanelBase
    {
        private Transform accInfo;
        private Button accInfoBtn;
        private Text accInfoName;

        private Button closeBtn;

        private Text logonType;
        private Transform mainPanel;
        private Button maskCloseBtn;
        private Button musicBtn;
        private Text nowSta;

        private Transform phoneInfo;
        private Button phoneInfoBtn;

        private Transform PWInfo;
        private Button PWInfoBtn;

        private Button quitGameBtn;

        private Transform RecFrameBtns;

        public SetPanel() : base(UIType.Middle, nameof(SetPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            Init();
        }

        protected override void FindComponent()
        {
            mainPanel = transform.FindChildDepth("mainPanel");
            maskCloseBtn = mainPanel.FindChildDepth<Button>("Close");
            closeBtn = mainPanel.FindChildDepth<Button>("CloseBtn");

            logonType = mainPanel.FindChildDepth<Text>("logonType/Text");
            nowSta = mainPanel.FindChildDepth<Text>("nowSta/Text");

            RecFrameBtns = mainPanel.FindChildDepth("RecFrameBtns");

            accInfo = RecFrameBtns.FindChildDepth("AccountInfo");
            accInfoName = accInfo.FindChildDepth<Text>("name");
            accInfoBtn = accInfo.FindChildDepth<Button>("SetBtn");

            PWInfo = RecFrameBtns.FindChildDepth("PasswordInfo");
            PWInfoBtn = PWInfo.FindChildDepth<Button>("SetBtn");

            phoneInfo = RecFrameBtns.FindChildDepth("SetPhone");
            phoneInfoBtn = phoneInfo.FindChildDepth<Button>("SetBtn");

            quitGameBtn = RecFrameBtns.FindChildDepth<Button>("quitGameBtn");
            musicBtn = RecFrameBtns.FindChildDepth<Button>("musicBtn");
        }

        protected override void AddListener()
        {
            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(() => { CloseSetPanel(); });
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(() => { CloseSetPanel(closeBtn.transform); });

            accInfoBtn.onClick.RemoveAllListeners();
            accInfoBtn.onClick.Add(ChangeAccOnClick);

            PWInfoBtn.onClick.RemoveAllListeners();
            PWInfoBtn.onClick.Add(SafeBtnOnClick);

            phoneInfoBtn.onClick.RemoveAllListeners();
            phoneInfoBtn.onClick.Add(BDPhonePanel);

            quitGameBtn.onClick.RemoveAllListeners();
            quitGameBtn.onClick.Add(ExitGameCall);

            musicBtn.onClick.RemoveAllListeners();
            musicBtn.onClick.Add(OpenMusicPanel);
        }

        private void Init()
        {
            if (GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber == "")
            {
                logonType.text = "游客登录";
                nowSta.text = GameLocalMode.Instance.IsSetLoginValidation ? $"已开启验证登录" : $"未开启验证登录";
                PWInfo.gameObject.SetActive(false);
                phoneInfo.gameObject.SetActive(true);
                phoneInfo.transform.localPosition = PWInfo.localPosition;
            }
            else
            {
                logonType.text = "账号登录";
                nowSta.text = GameLocalMode.Instance.IsSetLoginValidation ? $"已开启验证登录" : $"未开启验证登录";
                PWInfo.gameObject.SetActive(true);
                phoneInfo.gameObject.SetActive(false);
            }

            accInfoName.text = GameLocalMode.Instance.SCPlayerInfo.NickName;
        }

        private void ChangeAccOnClick()
        {
            DebugHelper.Log($"切换账号");
            ILMusicManager.Instance.PlayBtnSound();
            HotfixGameComponent.Instance.CloseNetwork(SocketType.Hall);
            GameLocalMode.Instance.AllSCUserProp.Clear();
            UIManager.Instance.CloseAllUI();

            var buffer = new ByteBuffer();
            buffer.WriteUInt16(0);
            HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN,
                DataStruct.LoginStruct.SUB_3D_CS_LOGOUT, buffer, SocketType.Hall);
            GameLocalMode.Instance.AccountList.isAuto = false;
            GameLocalMode.Instance.SaveAccount();
            System.GC.Collect();
            UIManager.Instance.OpenUI<LogonScenPanel>();
        }

        private void BDPhonePanel()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.ReplaceUI<BindPanel>();
        }

        private void SafeBtnOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.ReplaceUI<ChangePassWord>();
        }

        private void OpenMusicPanel()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.ReplaceUI<MusicPanel>();
        }

        private void CloseSetPanel(Transform args = null)
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.Close();
        }

        private void ExitGameCall()
        {
            ILMusicManager.Instance.PlayBtnSound();
            ToolHelper.PopBigWindow(new BigMessage
            {
                content = "是否退出游戏?",
                okCall = () => { Application.Quit(); }
            });
        }
    }
}