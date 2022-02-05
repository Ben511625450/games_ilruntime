using LitJson;
using LuaFramework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class LogonScenPanel : PanelBase
    {
        private Transform LoginGroup;
        private Button LoginBtn;
        private Button GuestBtn;

        private Transform LoginBg;
        private Transform FindPwdBg;
        private Transform RegisterBg;

        List<IState> states;
        HierarchicalStateMachine hsm;

        public LogonScenPanel() : base(UIType.Bottom, nameof(LogonScenPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            var startSence = GameObject.FindGameObjectWithTag(LaunchTag._01StartTag);
            if (startSence != null) HallExtend.Destroy(startSence);
            GameLocalMode.Instance.GetAccount();
            hsm = new HierarchicalStateMachine(false, gameObject);
            states = new List<IState>();
            states.Add(new IdleState(this, hsm));
            states.Add(new GetHttpConfigerState(this, hsm));
            states.Add(new CheckAutoLogin(this, hsm));
            states.Add(new LoginState(this, hsm));
            states.Add(new ResigterState(this, hsm));
            states.Add(new FindPasswordState(this, hsm));
            hsm?.Init(states, nameof(GetHttpConfigerState));
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            LoginGroup = transform.FindChildDepth("LoginGroup");
            LoginBtn = LoginGroup.FindChildDepth<Button>("LoginBtn"); //账号登录
            GuestBtn = LoginGroup.FindChildDepth<Button>("GuestBtn"); //游客登录

            LoginBg = transform.FindChildDepth("LoginBg");
            RegisterBg = transform.FindChildDepth("RegisterBg");
            FindPwdBg = transform.FindChildDepth("FindPwdBg");
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            EventComponent.Instance.AddListener(HallEvent.LogonResultCallBack,LogonBtnCallBack);
            EventComponent.Instance.AddListener(HallEvent.OnShowCodeLogin,HallEventOnOnShowCodeLogin);
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            EventComponent.Instance.RemoveListener(HallEvent.LogonResultCallBack,LogonBtnCallBack);
            EventComponent.Instance.RemoveListener(HallEvent.OnShowCodeLogin,HallEventOnOnShowCodeLogin);
        }

        protected override void Update()
        {
            base.Update();
            hsm?.Update();
        }

        protected override void AddListener()
        {
            base.AddListener();
            LoginBtn.onClick.RemoveAllListeners();
            LoginBtn.onClick.Add(ClickLoginCall);

            GuestBtn.onClick.RemoveAllListeners();
            GuestBtn.onClick.Add(ClickGuestCall);
        }

        /// <summary>
        ///     游客登录
        /// </summary>
        private void ClickGuestCall()
        {
            if (GameLocalMode.Instance.M_HttpData == null)
            {
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = "请重启游戏获取登录配置！",
                    okCall = () => { Application.Quit(0); },
                    cancelCall = () => { Application.Quit(0); }
                });
                return;
            }

            HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
            {
                if (!isSuccess)
                {
                    ToolHelper.PopSmallWindow($"网络连接失败");
                    return;
                }

                ToolHelper.ShowWaitPanel(true, $"正在登录……");
                ILGameManager.Instance.SendLoginMasseage(GameLocalMode.Instance.Account);
            });
        }

        /// <summary>
        ///     点击登录
        /// </summary>
        private void ClickLoginCall()
        {
            if (GameLocalMode.Instance.M_HttpData == null)
            {
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = "请重启游戏获取登录配置！",
                    okCall = () => { Application.Quit(0); },
                    cancelCall = () => { Application.Quit(0); }
                });
                return;
            }

            hsm?.ChangeState(nameof(LoginState));
        }

        /// <summary>
        /// 登录大厅成功
        /// </summary>
        private void LogonBtnCallBack(params object[] args)
        {
            bool isSuccess = (bool) args[0];
            ToolHelper.ShowWaitPanel(false);
            if (!isSuccess)
            {
                GameLocalMode.Instance.AccountList.isAuto = false;
                return;
            }

            GameLocalMode.Instance.Account.account = GameLocalMode.Instance.SCPlayerInfo.Account;
            GameLocalMode.Instance.Account.password = GameLocalMode.Instance.SCPlayerInfo.Password;
            GameLocalMode.Instance.AccountList.isAuto = true;
            GameLocalMode.Instance.AccountList.LoginType =
                GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber == "" ? 1 : 2;
            GameLocalMode.Instance.SaveAccount();
            UIManager.Instance.CloseUI<LogonScenPanel>();
            UIManager.Instance.OpenUI<HallScenPanel>();
        }


        private void HallEventOnOnShowCodeLogin(params object[] args)
        {
            UIManager.Instance.OpenUI<CodeLogin>();
        }

        /// <summary>
        /// 闲置状态
        /// </summary>
        private class IdleState : State<LogonScenPanel>
        {
            public IdleState(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                owner.LoginGroup.gameObject.SetActive(true);
                owner.LoginBg.gameObject.SetActive(false);
                owner.RegisterBg.gameObject.SetActive(false);
                owner.FindPwdBg.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        private class CheckAutoLogin : State<LogonScenPanel>
        {
            public CheckAutoLogin(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            bool _isComplete;

            public override void OnEnter()
            {
                base.OnEnter();
                _isComplete = false;
            }

            public override void Update()
            {
                base.Update();
                if (_isComplete) return;
                _isComplete = true;
                if (!GameLocalMode.Instance.AccountList.isAuto ||
                    GameLocalMode.Instance.AccountList.LoginType == 0) //普通登录
                {
                    GameLocalMode.Instance.AccountList.isAuto = false;
                    ToolHelper.ShowWaitPanel(false);
                    hsm.ChangeState(nameof(IdleState));
                    return;
                }

                //开始自动登录
                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (!isSuccess)
                    {
                        hsm?.ChangeState(nameof(IdleState));
                        return;
                    }

                    ToolHelper.ShowWaitPanel(true, $"正在登录……");
                    ILGameManager.Instance.SendLoginMasseage(GameLocalMode.Instance.Account);
                });
            }
        }

        /// <summary>
        /// 获取网络配置状态
        /// </summary>
        private class GetHttpConfigerState : State<LogonScenPanel>
        {
            public GetHttpConfigerState(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private int reqIPIndex = 0;
            private int reqLoginCount = 0;
            private int reqLoginIndex = 0;

            public override void OnEnter()
            {
                base.OnEnter();
                reqLoginIndex = 0;
                reqLoginCount = 0;
                ToolHelper.ShowWaitPanel(true, $"获取基础配置中，请稍后…");
                GetHttpConfiger();
            }

            /// <summary>
            /// 请求httpConfiger
            /// </summary>
            private void GetHttpConfiger()
            {
                DebugHelper.Log($"获取httpconfiger");
                FormData form = new FormData();
                form.AddField("time", System.DateTime.Now.ToString("yyyyMMddHHmmSS"));
                string url = AppConst.WebUrl.Replace("/android", "");
                url = url.Replace("/ios", "");
                url = url.Replace("/iOS", "");
                url = url.Replace("/win", "");
                HttpManager.Instance.GetText($"{url}HttpConfiger.json", form, (isSuccess, result) =>
                {
                    if (!isSuccess)
                    {
                        GetHttpConfiger();
                        return;
                    }

                    string msg = Util.DecryptDES(result, MD5Helper.DESKey);
                    DebugHelper.LogError(msg);
                    GameLocalMode.Instance.GWData = JsonMapper.ToObject<HttpDataConfiger>(msg);
                    {
                        Main main = Object.FindObjectOfType<Main>();
                        if (main != null)
                        {
                            main.reporter.SetActive(GameLocalMode.Instance.GWData.ShowDebug);
                        }
                    }
                    reqIPIndex = 0;
                    ReqIP();
                });
            }

            /// <summary>
            /// 请求IP
            /// </summary>
            private void ReqIP()
            {
                ToolHelper.ShowWaitPanel(true, $"获取其他配置中，请稍后…");
                DebugHelper.Log($"获取IP");
                FormData form = new FormData();
                form.AddField("time", System.DateTime.Now.ToString("yyyyMMddHHmmSS"));
                HttpManager.Instance.GetText(GameLocalMode.Instance.GWData.IPUrls[reqIPIndex], form, (isSuccess, msg) =>
                {
                    if (!isSuccess)
                    {
                        reqIPIndex++;
                        if (reqIPIndex >= GameLocalMode.Instance.GWData.IPUrls.Count) reqIPIndex = 0;
                        ReqIP();
                        return;
                    }

                    string ip = ToolHelper.GetIPFromHtml(msg);
                    if (ip == null)
                    {
                        reqIPIndex++;
                        if (reqIPIndex >= GameLocalMode.Instance.GWData.IPUrls.Count) reqIPIndex = 0;
                        ReqIP();
                        return;
                    }

                    GameLocalMode.Instance.IP = ip;
                    DebugHelper.Log($"IP:{GameLocalMode.Instance.IP}");
                    reqLoginIndex = 0;
                    reqLoginCount = 0;
                    ReqLoginConfig();

                    GameLocalMode.Instance.HallHost = GameLocalMode.Instance.GWData.isUseLoginIP
                        ? GameLocalMode.Instance.GWData.LoginIP
                        : GameLocalMode.Instance.M_HttpData.login_ip;
                    // GameLocalMode.Instance.HallPort = $"{28101}";
                    // hsm?.ChangeState(nameof(CheckAutoLogin));
                });
            }

            private void ReqLoginConfig()
            {
                DebugHelper.Log($"获取loginConfig");
                FormData form = new FormData();
                form.AddField("clientkey", "107");
                form.AddField("account", GameLocalMode.Instance.Account.account);
                form.AddField("clientID", "0");
                form.AddField("sendIp", "192.168.1.1");
                form.AddField("machine", "37342d44342d33352d44362d30442d39");
                form.AddField("param", $"{GameLocalMode.Instance.PlatformID}");
                form.AddField("md5", "a9e4553ae02ff86d37578525aae3f163");
                GameLocalMode.Instance.M_HttpData = null;
                HttpManager.Instance.GetText(
                    $"http://{GameLocalMode.Instance.GWData.Urls[reqLoginIndex]}/LoginIpHandler.ashx", form,
                    (isSuccess, msg) =>
                    {
                        DebugHelper.Log(msg);
                        if (!isSuccess)
                        {
                            reqLoginIndex++;
                            reqLoginCount++;
                            if (reqLoginCount >= 10)
                            {
                                ToolHelper.PopBigWindow(new BigMessage()
                                {
                                    content = "获取登录配置失败！",
                                    okCall = () => { }
                                });
                                return;
                            }

                            if (reqLoginIndex >= GameLocalMode.Instance.GWData.Urls.Count) reqLoginIndex = 0;
                            ReqLoginConfig();
                            return;
                        }

                        GameLocalMode.Instance.M_HttpData = JsonMapper.ToObject<HttpData>(msg);
                        ToolHelper.ShowWaitPanel(false);
                        GameLocalMode.Instance.HallHost = GameLocalMode.Instance.GWData.isUseLoginIP
                            ? GameLocalMode.Instance.GWData.LoginIP
                            : GameLocalMode.Instance.M_HttpData.login_ip;
                        GameLocalMode.Instance.HallPort = GameLocalMode.Instance.M_HttpData.login_port;
                        // GameLocalMode.Instance.HallPort = $"{28101}";
                        hsm?.ChangeState(CheckVersion() ? nameof(CheckAutoLogin) : nameof(IdleState));
                    });
            }

            /// <summary>
            /// 检查版本
            /// </summary>
            /// <returns></returns>
            private bool CheckVersion()
            {
                double.TryParse(Application.version, out double version);
                if (version >= GameLocalMode.Instance.GWData.version) return true;

                void OpenUrl()
                {
                    if (Util.isPc || Util.isEditor)
                    {
                        Application.OpenURL(GameLocalMode.Instance.GWData.PCUrl);
                    }
                    else if (Util.isAndroidPlatform)
                    {
                        Application.OpenURL(GameLocalMode.Instance.GWData.AndroidUrl);
                    }
                    else if (Util.isApplePlatform)
                    {
                        Application.OpenURL(GameLocalMode.Instance.GWData.iOSUrl);
                    }
                    Util.Quit();
                }
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = "请下载更新最新游戏版本！",
                    okCall = OpenUrl,
                    cancelCall = OpenUrl
                });
                return false;
            }
        }

        #region 登录

        /// <summary>
        /// 登录状态
        /// </summary>
        private class LoginState : State<LogonScenPanel>
        {
            private InputField LoginIDInput;
            private InputField LoginPassword;
            private Button LoginSureBtn;
            private Button LoginFindPwdBtn;
            private Button LoginRegistBtn;
            public Button LoginCloseBtn;

            private bool isInit;

            public LoginState(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                owner.LoginGroup.gameObject.SetActive(false);
                LoginIDInput.text = GameLocalMode.Instance.Account.account;
                LoginPassword.text = GameLocalMode.Instance.Account.account == ""
                    ? ""
                    : $"{GameLocalMode.Instance.Account.account}md5";
                owner.LoginBg.gameObject.SetActive(true);
            }

            public override void OnExit()
            {
                base.OnExit();
                owner.LoginBg.gameObject.SetActive(false);
            }

            private void Init()
            {
                if (isInit) return;
                LoginIDInput = owner.LoginBg.FindChildDepth<InputField>("ID/InputField"); //ID输入
                LoginPassword = owner.LoginBg.FindChildDepth<InputField>("Password/InputField"); //密码输入
                LoginSureBtn = owner.LoginBg.FindChildDepth<Button>("LoginSureBtn"); //确认登录
                LoginFindPwdBtn = owner.LoginBg.FindChildDepth<Button>("FindPwdBtn"); //找回密码
                LoginRegistBtn = owner.LoginBg.FindChildDepth<Button>("RegistBtn"); //注册
                LoginCloseBtn = owner.LoginBg.FindChildDepth<Button>("LoginBgCloseBtn"); //关闭登录
                LoginSureBtn.onClick.RemoveAllListeners();
                LoginSureBtn.onClick.Add(LogonBtnOnClick);

                LoginFindPwdBtn.onClick.RemoveAllListeners();
                LoginFindPwdBtn.onClick.Add(FindPwdBtnOnClick);

                LoginRegistBtn.onClick.RemoveAllListeners();
                LoginRegistBtn.onClick.Add(RegisterBtnOnClick);

                LoginCloseBtn.onClick.RemoveAllListeners();
                LoginCloseBtn.onClick.Add(ClickLoginBgClose);
                isInit = true;
            }

            /// <summary>
            ///     点击关闭登录
            /// </summary>
            private void ClickLoginBgClose()
            {
                hsm?.ChangeState(nameof(IdleState));
            }

            /// <summary>
            ///     点击登录
            /// </summary>
            private void LogonBtnOnClick()
            {
                if (string.IsNullOrWhiteSpace(LoginIDInput.text))
                {
                    ToolHelper.PopSmallWindow("账号或密码错误");
                    return;
                }

                if (string.IsNullOrWhiteSpace(LoginPassword.text))
                {
                    ToolHelper.PopSmallWindow("账号或密码错误");
                    return;
                }

                LoginSureBtn.interactable = false;
                if (string.IsNullOrWhiteSpace(GameLocalMode.Instance.Account.account))
                {
                    GameLocalMode.Instance.Account = new AccountMSG();
                    GameLocalMode.Instance.Account.account = LoginIDInput.text;
                    GameLocalMode.Instance.Account.password = MD5Helper.MD5String(LoginPassword.text);
                }
                else
                {
                    if (GameLocalMode.Instance.Account.account != LoginIDInput.text)
                    {
                        GameLocalMode.Instance.Account = new AccountMSG();
                        GameLocalMode.Instance.Account.account = LoginIDInput.text;
                        GameLocalMode.Instance.Account.password = MD5Helper.MD5String(LoginPassword.text);
                    }

                    if (!$"{LoginIDInput.text}md5".Equals(LoginPassword.text) &&
                        GameLocalMode.Instance.Account.password != MD5Helper.MD5String(LoginPassword.text))
                    {
                        GameLocalMode.Instance.Account.password = MD5Helper.MD5String(LoginPassword.text);
                    }
                }

                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (LoginSureBtn != null) LoginSureBtn.interactable = true;
                    if (!isSuccess)
                    {
                        ToolHelper.PopSmallWindow($"网络连接失败");
                        return;
                    }

                    ToolHelper.ShowWaitPanel(true, $"正在登录……");
                    ILGameManager.Instance.SendLoginMasseage(GameLocalMode.Instance.Account);
                });
            }

            /// <summary>
            ///     找回密码事件
            /// </summary>
            private void FindPwdBtnOnClick()
            {
                hsm?.ChangeState(nameof(FindPasswordState));
            }

            /// <summary>
            ///     注册界面
            /// </summary>
            private void RegisterBtnOnClick()
            {
                hsm?.ChangeState(nameof(ResigterState));
            }
        }

        #endregion

        #region 注册

        /// <summary>
        /// 注册状态
        /// </summary>
        private class ResigterState : State<LogonScenPanel>
        {
            public ResigterState(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private bool isInit;
            private Button RegisterCloseBtn;
            private InputField RegisterCodeInput;

            private Button RegisterGetCodeBtn;

            // private Transform codeImg;
            private Text codeTimeTxt;

            private InputField RegisterIDInput;
            private InputField RegisterMobileNumInput;
            private InputField RegisterPassword2Input;
            private InputField RegisterPasswordInput;
            private Button RegisterSureBtn;
            private int codeAllTime = 60;
            private float timer;
            private bool isStart;
            private Transform timeUnderLine;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                owner.LoginGroup.gameObject.SetActive(false);
                RegisterIDInput.text = "";
                RegisterMobileNumInput.text = "";
                RegisterPasswordInput.text = "";
                RegisterPassword2Input.text = "";
                RegisterCodeInput.text = "";
                EventComponent.Instance.AddListener(HallEvent.LogonRegisterCallBack, RegisterS2CCallBack);
                EventComponent.Instance.AddListener(HallEvent.LogonCodeCallBack, GetCodeBtnCallBack);
                owner.RegisterBg.gameObject.SetActive(true);
            }

            public override void OnExit()
            {
                base.OnExit();
                EventComponent.Instance.RemoveListener(HallEvent.LogonRegisterCallBack, RegisterS2CCallBack);
                EventComponent.Instance.RemoveListener(HallEvent.LogonCodeCallBack, GetCodeBtnCallBack);
                owner.RegisterBg.gameObject.SetActive(false);
            }

            public override void Update()
            {
                base.Update();
                StartCodeTimer();
            }

            /// <summary>
            /// 获取验证码倒计时
            /// </summary>
            private void StartCodeTimer()
            {
                if (!isStart) return;
                timer -= Time.deltaTime;
                RegisterText(timer);
                if (timer > 0) return;
                isStart = false;
                CountDownComplete();
            }

            private void Init()
            {
                if (isInit) return;
                RegisterIDInput = owner.RegisterBg.FindChildDepth<InputField>("ID/InputField"); //ID输入
                RegisterMobileNumInput = owner.RegisterBg.FindChildDepth<InputField>("MobileNum/InputField"); //号码输入
                RegisterPasswordInput = owner.RegisterBg.FindChildDepth<InputField>("Password/InputField"); //密码输入
                RegisterPassword2Input = owner.RegisterBg.FindChildDepth<InputField>("Password2/InputField"); //确认密码
                RegisterCodeInput = owner.RegisterBg.FindChildDepth<InputField>("Code/InputField"); //验证码
                RegisterGetCodeBtn = owner.RegisterBg.FindChildDepth<Button>("GetCodeBtn"); //获取验证码
                RegisterSureBtn = owner.RegisterBg.FindChildDepth<Button>("RegistBtn"); //确认
                RegisterCloseBtn = owner.RegisterBg.FindChildDepth<Button>("QuitBtn"); //关闭注册

                // codeImg = RegisterGetCodeBtn.transform.FindChildDepth("Image");
                codeTimeTxt = RegisterGetCodeBtn.transform.FindChildDepth<Text>($"BtnShow");
                timeUnderLine = RegisterGetCodeBtn.transform.FindChildDepth($"Image");
                RegisterGetCodeBtn.transform.FindChildDepth($"Time").gameObject.SetActive(false);
                RegisterSureBtn.onClick.RemoveAllListeners();
                RegisterSureBtn.onClick.Add(() => { RegisterSureBtnOnClick(RegisterSureBtn); });

                RegisterCloseBtn.onClick.RemoveAllListeners();
                RegisterCloseBtn.onClick.Add(RegisterCloseBtnOnClick);

                RegisterGetCodeBtn.onClick.RemoveAllListeners();
                RegisterGetCodeBtn.onClick.Add(() => { RegisterGetCodeBtnOnClick(RegisterGetCodeBtn); });
                isInit = true;
            }

            /// <summary>
            ///     确认注册
            /// </summary>
            private void RegisterSureBtnOnClick(Button args)
            {
                string IDInput = RegisterIDInput.text;
                string MobileNumInput = RegisterMobileNumInput.text;
                string PasswordInput = RegisterPasswordInput.text;
                string RePasswordInput = RegisterPassword2Input.text;
                string CodeInput = RegisterCodeInput.text;

                DebugHelper.Log("MobileNumInput:" + MobileNumInput.Length);
                if (MobileNumInput.Length != 11)
                {
                    ToolHelper.PopSmallWindow("手机号输入有误");
                    return;
                }

                if (PasswordInput.Length < 6 || PasswordInput.Length > 12)
                {
                    ToolHelper.PopSmallWindow("密码格式有误");
                    return;
                }

                if (RePasswordInput != PasswordInput)
                {
                    ToolHelper.PopSmallWindow("两次密码不一致");
                    return;
                }

                if (CodeInput.Length < 4 || CodeInput.Length > 8)
                {
                    ToolHelper.PopSmallWindow("验证码输入有误");
                    return;
                }

                args.interactable = false;
                HallStruct.REQ_CS_Register register = new HallStruct.REQ_CS_Register();
                register.platform = GameLocalMode.Instance.PlatformID;
                register.channelID = GameLocalMode.Instance.Platform;
                register.iD = GameLocalMode.Instance.PlatformID;
                register.addMultiplyID =
                    (uint) (GameLocalMode.Instance.PlatformID * GameLocalMode.Instance.ResigterPlatMultiply +
                            GameLocalMode.Instance.ResigterPlatAdd);
                register.multiplyID =
                    (ushort) (GameLocalMode.Instance.PlatformID * GameLocalMode.Instance.ResigterPlatMultiply);
                register.addID = (byte) (GameLocalMode.Instance.PlatformID * GameLocalMode.Instance.ResigterPlatAdd);
                register.account = IDInput;
                register.password = MD5Helper.MD5String(PasswordInput);
                register.mechinaCode = GameLocalMode.Instance.MechinaCode;
                register.phoneNum = MobileNumInput;
                register.phoneCode = uint.Parse(CodeInput);
                register.iP = GameLocalMode.Instance.IP;

                DebugHelper.Log(JsonMapper.ToJson(register));
                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (args != null) args.interactable = true;
                    if (!isSuccess) return;
                    HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                        DataStruct.LoginStruct.SUB_3D_CS_REGISTER, register.ByteBuffer, SocketType.Hall);
                });
            }

            /// <summary>
            ///     注册回调
            /// </summary>
            private void RegisterS2CCallBack(params object[] args)
            {
                HallStruct.ACP_SC_LOGIN_REGISTER registerInfo = (HallStruct.ACP_SC_LOGIN_REGISTER) args[0];
                ToolHelper.ShowWaitPanel(false);
                if (registerInfo.Length > 0)
                {
                    ToolHelper.PopSmallWindow(registerInfo.Error);
                }
                else
                {
                    ToolHelper.PopSmallWindow("注册成功");
                    RegisterCloseBtnOnClick();
                }

                RegisterSureBtn.interactable = true;
            }

            /// <summary>
            ///     关闭注册
            /// </summary>
            private void RegisterCloseBtnOnClick()
            {
                hsm?.ChangeState(nameof(IdleState));
            }

            /// <summary>
            ///     获取注册验证码
            /// </summary>
            private void RegisterGetCodeBtnOnClick(Button args)
            {
                timeUnderLine.gameObject.SetActive(false);
                var phonenum = RegisterMobileNumInput.text;
                if (phonenum == "" || phonenum.Length != 11)
                {
                    ToolHelper.PopSmallWindow("请输入正确手机号码");
                    codeTimeTxt.text = "获取验证码";
                    timeUnderLine.gameObject.SetActive(true);
                    return;
                }

                args.interactable = false;

                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (!isSuccess) return;
                    var Code = new HallStruct.REQ_CS_CODE_RegisterS(1, 1, 1, phonenum);
                    HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN, 29, Code._ByteBuffer,
                        SocketType.Hall);
                });
                timer = codeAllTime;
                // codeImg.gameObject.SetActive(false);
                RegisterText(timer);
                // codeTimeTxt.gameObject.SetActive(true);
                isStart = true;
            }

            /// <summary>
            ///     注册验证码返回
            /// </summary>
            /// <param name="args"></param>
            private void GetCodeBtnCallBack(params object[] args)
            {
                uint num = (uint) args[0];
                DebugHelper.Log("------注册验证码返回--------------");
                var str = num.ToString();
                HotfixGameComponent.Instance.CloseNetwork(SocketType.Hall);
                ToolHelper.ShowWaitPanel(false);
                if (str == "0") ToolHelper.PopSmallWindow("手机号已绑定");
            }

            /// <summary>
            ///     注册时间
            /// </summary>
            /// <param name="num"></param>
            private void RegisterText(float num)
            {
                codeTimeTxt.text = $"{Mathf.Ceil(num)}s重发";
            }

            private void CountDownComplete()
            {
                codeTimeTxt.text = "获取验证码";
                timeUnderLine.gameObject.SetActive(true);
                // codeTimeTxt.gameObject.SetActive(false);
                // codeImg.gameObject.SetActive(true);
                RegisterGetCodeBtn.interactable = true;
            }
        }

        #endregion

        #region 找回密码

        /// <summary>
        /// 找回密码状态
        /// </summary>
        private class FindPasswordState : State<LogonScenPanel>
        {
            public FindPasswordState(LogonScenPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private bool isInit;

            private InputField FindPwdCodeInput;
            private Button FindPwdGetCodeBtn;
            private InputField FindPwdMobileNumInput;
            private InputField FindPwdPassword2Input;
            private InputField FindPwdPasswordInput;
            private Button FindPwdSureBtn;
            private Button FindPwdBackBtn;

            // private Transform findBtnImg;
            private Text findBtnTimeTxt;
            private int codeAllTime = 60;

            private float timer;
            private bool isStart;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                owner.LoginGroup.gameObject.SetActive(false);
                FindPwdMobileNumInput.text = "";
                FindPwdPasswordInput.text = "";
                FindPwdPassword2Input.text = "";
                FindPwdCodeInput.text = "";
                owner.FindPwdBg.gameObject.SetActive(true);
                EventComponent.Instance.AddListener(HallEvent.LogonFindPWCallBack,UpdatePwdSuccess);
                EventComponent.Instance.AddListener(HallEvent.LogonFindPW_GetCode,UpdatePwdCodeSC);
            }

            public override void OnExit()
            {
                base.OnExit();
                EventComponent.Instance.RemoveListener(HallEvent.LogonFindPWCallBack,UpdatePwdSuccess);
                EventComponent.Instance.RemoveListener(HallEvent.LogonFindPW_GetCode,UpdatePwdCodeSC);
                owner.FindPwdBg.gameObject.SetActive(false);
            }

            public override void Update()
            {
                base.Update();
                StartCodeTimer();
            }

            private void Init()
            {
                if (isInit) return;
                FindPwdMobileNumInput = owner.FindPwdBg.FindChildDepth<InputField>("MobileNum/InputField"); //号码输入
                FindPwdPasswordInput = owner.FindPwdBg.FindChildDepth<InputField>("Password/InputField"); //密码输入
                FindPwdPassword2Input = owner.FindPwdBg.FindChildDepth<InputField>("Password2/InputField"); //密码2输入
                FindPwdCodeInput = owner.FindPwdBg.FindChildDepth<InputField>("Code/InputField"); //验证码输入
                FindPwdGetCodeBtn = owner.FindPwdBg.FindChildDepth<Button>("GetCodeBtn"); //获取验证码
                FindPwdSureBtn = owner.FindPwdBg.FindChildDepth<Button>("SureBtn"); //确认
                FindPwdBackBtn = owner.FindPwdBg.FindChildDepth<Button>("BackBtn"); //关闭

                // findBtnImg = FindPwdGetCodeBtn.transform.FindChildDepth("Image");
                findBtnTimeTxt = FindPwdGetCodeBtn.transform.FindChildDepth<Text>("Image");

                FindPwdSureBtn.onClick.RemoveAllListeners();
                FindPwdSureBtn.onClick.Add(() => { FindPwdSureBtnOnClick(FindPwdSureBtn); });

                FindPwdBackBtn.onClick.RemoveAllListeners();
                FindPwdBackBtn.onClick.Add(FindPwdCloseBtnOnClick);

                FindPwdGetCodeBtn.onClick.RemoveAllListeners();
                FindPwdGetCodeBtn.onClick.Add(() => { FindPwdGetCodeBtnOnClick(FindPwdGetCodeBtn); });
                isInit = true;
            }

            /// <summary>
            /// 获取验证码倒计时
            /// </summary>
            private void StartCodeTimer()
            {
                if (!isStart) return;
                timer -= Time.deltaTime;
                FindPWText(timer);
                if (timer > 0) return;
                isStart = false;
                CountDownComplete();
            }

            /// <summary>
            ///     确认修改密码
            /// </summary>
            /// <param name="args"></param>
            private void FindPwdSureBtnOnClick(Button args)
            {
                var iphone = FindPwdMobileNumInput.text;
                var pw = FindPwdPasswordInput.text;
                var pw2 = FindPwdPassword2Input.text;
                var code = FindPwdCodeInput.text;
                if (iphone.Length != 11)
                {
                    ToolHelper.PopSmallWindow("手机号输入有误");
                    return;
                }

                if (pw != pw2)
                {
                    ToolHelper.PopSmallWindow("两次密码不一致");
                    return;
                }

                if (pw.Length < 6 || pw.Length > 12)
                {
                    ToolHelper.PopSmallWindow("密码格式有误");
                    return;
                }

                if (code.Length < 4 || code.Length > 8)
                {
                    ToolHelper.PopSmallWindow("验证码输入有误");
                    return;
                }

                args.interactable = false;

                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (args != null)
                    {
                        args.interactable = true;
                    }

                    if (!isSuccess) return;

                    HallStruct.REQ_CS_FindPassword findPassword = new HallStruct.REQ_CS_FindPassword();
                    findPassword.phoneNumber = iphone;
                    findPassword.password = MD5Helper.MD5String(pw);
                    findPassword.code = uint.Parse(code);
                    findPassword.platform = GameLocalMode.Instance.PlatformID;
                    HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN, 19, findPassword.ByteBuffer,
                        SocketType.Hall);
                });
            }

            /// <summary>
            ///     修改密码返回
            /// </summary>
            /// <param name="args"></param>
            private void UpdatePwdSuccess(params object[] args)
            {
                HotfixGameComponent.Instance.CloseNetwork(SocketType.Hall);
                FindPwdSureBtn.interactable = true;
                if (args.Length <= 0)
                {
                    GameLocalMode.Instance.Account.account = FindPwdMobileNumInput.text;
                    GameLocalMode.Instance.Account.password = MD5Helper.MD5String(FindPwdPasswordInput.text);
                    GameLocalMode.Instance.SaveAccount();
                    ToolHelper.PopSmallWindow("重置密码成功，请妥善保管");
                    FindPwdCloseBtnOnClick();
                }
                else
                {
                    HallStruct.ACP_SC_LOGIN_FINDPW pwInfo = (HallStruct.ACP_SC_LOGIN_FINDPW) args[0];
                    ToolHelper.PopSmallWindow(pwInfo.Error);
                }

                ToolHelper.ShowWaitPanel(false);
            }


            /// <summary>
            /// 找回密码验证码返回
            /// </summary>
            /// <param name="args"></param>
            private void UpdatePwdCodeSC(params object[] args)
            {
                int code = (int) args[0];
                HotfixGameComponent.Instance.CloseNetwork(SocketType.Hall);
                ToolHelper.ShowWaitPanel(false);
                if (code <= 0) ToolHelper.PopSmallWindow("手机号未注册");
            }

            /// <summary>
            ///     修改密码时间
            /// </summary>
            /// <param name="num"></param>
            private void FindPWText(float num)
            {
                findBtnTimeTxt.text = $"{Mathf.Ceil(num)}s重发";
            }

            /// <summary>
            ///     关闭修改密码
            /// </summary>
            private void FindPwdCloseBtnOnClick()
            {
                hsm?.ChangeState(nameof(LoginState));
            }

            /// <summary>
            ///     获取找回密码验证码
            /// </summary>
            private void FindPwdGetCodeBtnOnClick(Button args)
            {
                FindPwdGetCodeBtn.GetComponent<Text>().enabled = false;
                var phonenum = FindPwdMobileNumInput.text;

                if (phonenum == "" || phonenum.Length != 11)
                {
                    ToolHelper.PopSmallWindow("请输入正确手机号码");
                    FindPwdGetCodeBtn.GetComponent<Text>().enabled = true;
                    return;
                }

                args.interactable = false;

                HotfixGameComponent.Instance.ConnectHallServer(isSuccess =>
                {
                    if (!isSuccess) return;
                    var Code = new HallStruct.REQ_CS_FindPasswordCode(phonenum);
                    HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN, 22, Code._ByteBuffer,
                        SocketType.Hall);
                });
                timer = codeAllTime;
                // findBtnImg.gameObject.SetActive(false);
                findBtnTimeTxt.gameObject.SetActive(true);
                FindPWText(timer);
                isStart = true;
            }

            private void CountDownComplete()
            {
                findBtnTimeTxt.text = "获取验证码";
                // findBtnImg.gameObject.SetActive(true);
                // findBtnTimeTxt.gameObject.SetActive(false);
                FindPwdGetCodeBtn.interactable = true;
                FindPwdGetCodeBtn.GetComponent<Text>().enabled = true;
            }
        }

        #endregion
    }
}