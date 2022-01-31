using System;
using System.Collections.Generic;
using LuaFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class BankPanel : PanelBase
    {
        private Button closeBtn;

        private Transform leftPanel;
        private Button leftGetBtn;
        private Button leftQueryBtn;
        private Button leftSaveBtn;
        private Button leftTransferBtn;
        private Button leftZSKBtn;

        private Button maskCloseBtn;

        private Transform mainPanel;
        private Transform rightPanel;
        private Transform getSavePanel;
        private Transform queryPanel;
        private Transform transferPanel;

        private List<IState> states;
        private HierarchicalStateMachine hsm;

        public BankPanel() : base(UIType.Middle, nameof(BankPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            ILGameManager.isOpenBank = true;
            HallEvent.DispatchChangeGoldTicket();
            states = new List<IState>();
            hsm = new HierarchicalStateMachine(false, gameObject);
            states.Add(new IdleState(this, hsm));
            states.Add(new GetState(this, hsm));
            states.Add(new SaveState(this, hsm));
            states.Add(new QueryState(this, hsm));
            states.Add(new GiveState(this, hsm));
            states.Add(new GiveCardState(this, hsm));
            hsm.Init(states, nameof(IdleState));
            ILGameManager.isOpenBank = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            hsm?.CurrentState.OnExit();
        }

        protected override void Update()
        {
            base.Update();
            hsm?.Update();
        }

        protected override void FindComponent()
        {
            closeBtn = transform.FindChildDepth<Button>("mainPanel/CloseBtn");
            maskCloseBtn = transform.FindChildDepth<Button>("Mask");

            mainPanel = transform.FindChildDepth("mainPanel");

            leftPanel = mainPanel.FindChildDepth("Left");
            leftSaveBtn = leftPanel.FindChildDepth<Button>("SaveBtn");
            leftGetBtn = leftPanel.FindChildDepth<Button>("GetBtn");
            leftTransferBtn = leftPanel.FindChildDepth<Button>("TransferBtn");
            leftQueryBtn = leftPanel.FindChildDepth<Button>("QueryBtn");
            leftQueryBtn.gameObject.SetActive(GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1);
            leftZSKBtn = leftPanel.FindChildDepth<Button>("ZSKBtn");
            leftZSKBtn.gameObject.SetActive(GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1);

            rightPanel = mainPanel.FindChildDepth("Right");
            getSavePanel = rightPanel.FindChildDepth("GetAndSave");
            transferPanel = rightPanel.FindChildDepth("Transfer");
            queryPanel = rightPanel.FindChildDepth("QueryPanel");
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseBankePanel);
            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(CloseBankePanel);

            leftSaveBtn.onClick.RemoveAllListeners();
            leftSaveBtn.onClick.Add(() =>
            {
                if (hsm.CurrentStateName.Equals(nameof(SaveState))) return;
                hsm?.ChangeState(nameof(SaveState));
            });

            leftGetBtn.onClick.RemoveAllListeners();
            leftGetBtn.onClick.Add(() =>
            {
                if (hsm.CurrentStateName.Equals(nameof(GetState))) return;
                hsm?.ChangeState(nameof(GetState));
            });

            leftTransferBtn.onClick.RemoveAllListeners();
            leftTransferBtn.onClick.Add(() =>
            {
                if (hsm.CurrentStateName.Equals(nameof(GiveState))) return;
                hsm?.ChangeState(nameof(GiveState));
            });

            leftQueryBtn.onClick.RemoveAllListeners();
            leftQueryBtn.onClick.Add(() =>
            {
                if (hsm.CurrentStateName.Equals(nameof(QueryState))) return;
                hsm?.ChangeState(nameof(QueryState));
            });

            leftZSKBtn.onClick.RemoveAllListeners();
            leftZSKBtn.onClick.Add(() =>
            {
                if (hsm.CurrentStateName.Equals(nameof(GiveCardState))) return;
                hsm?.ChangeState(nameof(GiveCardState));
            });
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.OnEnterBank += HallEventOnOnEnterBank;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.OnEnterBank -= HallEventOnOnEnterBank;
        }

        private void HallEventOnOnEnterBank(HallStruct.InitBank bankInfo)
        {
            if (bankInfo.count > 200)
            {
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = "银行因安全问题已被锁定,请重置银行密码。",
                    okCall = ()=>{UIManager.Instance.CloseUI<BankPanel>();},
                    cancelCall = ()=>{UIManager.Instance.CloseUI<BankPanel>();},
                });
                return;
            }

            GameLocalMode.Instance.ChangProp(bankInfo.bankGold, Prop_Id.E_PROP_STRONG);
            HallEvent.DispatchChangeGoldTicket();
            if (bankInfo.count == 0) return;
            ToolHelper.PopBigWindow(new BigMessage()
            {
                content = $"银行密码输入错误,还能输入<color=#ffff00ff>{bankInfo.count}</color>次",
                okCall = ()=>{UIManager.Instance.CloseUI<BankPanel>();},
                cancelCall = ()=>{UIManager.Instance.CloseUI<BankPanel>();},
            });
        }

        private void CloseBankePanel()
        {
            UIManager.Instance.Close();
        }

        private void QueryBank()
        {
            HallStruct.REQ_CS_Bank_Query bankQuery = new HallStruct.REQ_CS_Bank_Query();
            bankQuery.dwUser_Id = GameLocalMode.Instance.SCPlayerInfo.DwUser_Id;
            HotfixGameComponent.Instance.Send(DataStruct.BankStruct.MDM_GP_USER,
                DataStruct.BankStruct.SUB_GP_USER_BANK_Query_RESULT, bankQuery.ByteBuffer, SocketType.Hall);
        }
        /// <summary>
        /// 闲置
        /// </summary>
        private class IdleState : State<BankPanel>
        {
            public IdleState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private bool _isComplete;

            public override void OnEnter()
            {
                base.OnEnter();
                owner.getSavePanel.gameObject.SetActive(false);
                owner.transferPanel.gameObject.SetActive(false);
                owner.queryPanel.gameObject.SetActive(false);
                _isComplete = false;
            }

            public override void Update()
            {
                base.Update();
                if (_isComplete) return;
                _isComplete = true;
                InputPwdGetGold();
                hsm?.ChangeState(nameof(SaveState));
            }

            /// <summary>
            ///     初始化进入
            /// </summary>
            private void InputPwdGetGold()
            {
                var str = "123456";
                HallStruct.REQ_CS_Bank_Init bankInit = new HallStruct.REQ_CS_Bank_Init();
                bankInit.password = MD5Helper.MD5String(str);
                HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                    DataStruct.PersonalStruct.SUB_3D_CS_OPEN_BANK, bankInit.ByteBuffer, SocketType.Hall);
                owner.QueryBank();
            }
        }

        /// <summary>
        /// 存款
        /// </summary>
        private class SaveState : State<BankPanel>
        {
            public SaveState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private Text selfGoldNum;
            private Text bankGoldNum;
            private InputField saveNum;
            private Button allSaveBtn;
            private Text upperDesc;
            private InputField bankPassword;
            private Button changeBankPasswordBtn;
            private Button saveBtn;
            private Button getBtn;
            private Button resetBtn;

            private GameObject selectObj;
            private bool isInit;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                AddListener();
                saveNum.text = "";
                upperDesc.text = "";
                bankPassword.text = "";
                selectObj.gameObject.SetActive(true);
                owner.leftSaveBtn.interactable = false;
                owner.getSavePanel.gameObject.SetActive(true);
                allSaveBtn.transform.Find("Cun").gameObject.SetActive(true);
                allSaveBtn.transform.Find("Qu").gameObject.SetActive(false);
                getBtn.gameObject.SetActive(false);
                saveBtn.gameObject.SetActive(true);
                HallEvent.ChangeGoldTicket += HallEventOnChangeGoldTicket;
                HallEvent.Bank_Operate_Result += GetOrSaveCallBack;
                HallEvent.DispatchChangeGoldTicket();
            }

            public override void OnExit()
            {
                base.OnExit();
                selectObj.gameObject.SetActive(false);
                owner.leftSaveBtn.interactable = true;
                owner.getSavePanel.gameObject.SetActive(false);
                HallEvent.ChangeGoldTicket -= HallEventOnChangeGoldTicket;
                HallEvent.Bank_Operate_Result -= GetOrSaveCallBack;
            }

            private void Init()
            {
                if (isInit) return;
                selectObj = owner.leftSaveBtn.transform.FindChildDepth($"Select").gameObject;
                selfGoldNum = owner.getSavePanel.FindChildDepth<Text>("Gold/Text");
                bankGoldNum = owner.getSavePanel.FindChildDepth<Text>("SaveGold/Text");
                allSaveBtn = owner.getSavePanel.FindChildDepth<Button>("AllBtn");
                Transform getGroup = owner.getSavePanel.FindChildDepth("Group");
                saveNum = getGroup.FindChildDepth<InputField>("SetGold/InputField");
                upperDesc = getGroup.FindChildDepth<Text>("UpperGold/Desc");
                bankPassword = getGroup.FindChildDepth<InputField>("BankPw/InputField");
                saveBtn = owner.getSavePanel.FindChildDepth<Button>("SaveBtn");
                getBtn = owner.getSavePanel.FindChildDepth<Button>("GetBtn");
                resetBtn = owner.getSavePanel.FindChildDepth<Button>("RestBtn");
                changeBankPasswordBtn = owner.getSavePanel.FindChildDepth<Button>("ChangePWBtn");
                isInit = true;
            }

            private void AddListener()
            {
                saveBtn.onClick.RemoveAllListeners();
                saveBtn.onClick.Add(SaveGoldBtnOnClick);

                resetBtn.onClick.RemoveAllListeners();
                resetBtn.onClick.Add(OnClickResetCall);

                changeBankPasswordBtn.onClick.RemoveAllListeners();
                changeBankPasswordBtn.onClick.Add(OpenChangeBankPWPanel);

                saveNum.onValueChanged.RemoveAllListeners();
                saveNum.onValueChanged.Add(SetSaveGoldEndValue);

                allSaveBtn.onClick.RemoveAllListeners();
                allSaveBtn.onClick.Add(OnClickAllCall);
            }

            private void OnClickAllCall()
            {
                saveNum.text = selfGoldNum.text;
                upperDesc.text = ToolHelper.Low2Up(saveNum.text);
            }

            private void OnClickResetCall()
            {
                upperDesc.text = "";
                saveNum.text = "";
                bankPassword.text = "";
                saveBtn.interactable = true;
            }

            /// <summary>
            /// 存取金币服务器返回
            /// </summary>
            /// <param name="data"></param>
            private void GetOrSaveCallBack(HallStruct.ACP_SC_SaveOrGetGold data)
            {
                if (data.cbDrawOut != 0) return;
                SaveGoldBtnCallBack(data);
            }

            /// <summary>
            ///     存入金币
            /// </summary>
            /// <param name="data"></param>
            private void SaveGoldBtnCallBack(HallStruct.ACP_SC_SaveOrGetGold data)
            {
                saveBtn.interactable = true;
                saveNum.text = "";
                upperDesc.text = "";
                bankPassword.text = "";
                if (data.cbSuccess == 0)
                {
                    GameLocalMode.Instance.ChangProp(data.szInsureScore, Prop_Id.E_PROP_STRONG);
                    bankGoldNum.text = data.szInsureScore.ToString();
                    ILGameManager.Instance.QuerySelfGold();
                }
                else
                {
                    ToolHelper.PopSmallWindow(data.szInfoDiscrib);
                }
            }

            private void OpenChangeBankPWPanel()
            {
                UIManager.Instance.OpenUI<UpdateBankPWPanel>();
            }

            private void HallEventOnChangeGoldTicket()
            {
                selfGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD).ToString();
                bankGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG).ToString();
            }


            /// <summary>
            /// 存入金币
            /// </summary>
            private void SaveGoldBtnOnClick()
            {
                var str = saveNum.text;
                long gold = long.Parse(str);
                if (string.IsNullOrEmpty(str) || gold < 1)
                {
                    ToolHelper.PopSmallWindow("请检查你输入的金币是否正确");
                    return;
                }

                if (long.Parse(selfGoldNum.text) < gold)
                {
                    ToolHelper.PopSmallWindow("你持有的金币数量不足,请重新输入!");
                    return;
                }

                saveBtn.interactable = false;
                var id = GameLocalMode.Instance.SCPlayerInfo.DwUser_Id;
                uint mark = 0;
                var pw = bankPassword.text;
                HallStruct.REQ_CS_SAVEORGETGOLD saveGold = new HallStruct.REQ_CS_SAVEORGETGOLD()
                {
                    id = id,
                    mark = mark,
                    gold = gold,
                    pwd = MD5Helper.MD5String(pw)
                };
                HotfixGameComponent.Instance.Send(DataStruct.BankStruct.MDM_GP_USER,
                    DataStruct.BankStruct.SUB_GP_USER_BANK_OPERATE, saveGold.ByteBuffer, SocketType.Hall);
                OnClickResetCall();
            }

            private void SetSaveGoldEndValue(string value)
            {
                if (value.Length > 13)
                {
                    saveNum.text = "";
                    ToolHelper.PopSmallWindow("数值过大");
                    return;
                }

                if (string.IsNullOrWhiteSpace(value) || long.Parse(value) <= 0)
                {
                    saveNum.text = "";
                }
                else
                {
                    saveNum.text = value;
                    upperDesc.text = ToolHelper.Low2Up(saveNum.text);
                }
            }
        }

        /// <summary>
        /// 取款
        /// </summary>
        private class GetState : State<BankPanel>
        {
            public GetState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private Text selfGoldNum;
            private Text bankGoldNum;
            private InputField getNum;
            private Button allSaveBtn;
            private Text upperDesc;
            private InputField bankPassword;
            private Button changeBankPasswordBtn;
            private Button saveBtn;
            private Button getBtn;
            private Button resetBtn;

            private GameObject selectObj;
            private bool isInit;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                AddListener();
                getNum.text = "";
                upperDesc.text = "";
                bankPassword.text = "";
                owner.leftGetBtn.interactable = false;
                selectObj.gameObject.SetActive(true);
                owner.getSavePanel.gameObject.SetActive(true);
                allSaveBtn.transform.Find("Cun").gameObject.SetActive(false);
                allSaveBtn.transform.Find("Qu").gameObject.SetActive(true);
                getBtn.gameObject.SetActive(true);
                saveBtn.gameObject.SetActive(false);
                HallEvent.ChangeGoldTicket += HallEventOnChangeGoldTicket;
                HallEvent.Bank_Operate_Result += GetOrSaveCallBack;
                HallEvent.DispatchChangeGoldTicket();
            }

            public override void OnExit()
            {
                base.OnExit();
                selectObj.gameObject.SetActive(false);
                owner.leftGetBtn.interactable = true;
                owner.getSavePanel.gameObject.SetActive(false);
                HallEvent.ChangeGoldTicket -= HallEventOnChangeGoldTicket;
                HallEvent.Bank_Operate_Result -= GetOrSaveCallBack;
            }

            private void Init()
            {
                if (isInit) return;
                selectObj = owner.leftGetBtn.transform.FindChildDepth($"Select").gameObject;
                selfGoldNum = owner.getSavePanel.FindChildDepth<Text>("Gold/Text");
                bankGoldNum = owner.getSavePanel.FindChildDepth<Text>("SaveGold/Text");
                allSaveBtn = owner.getSavePanel.FindChildDepth<Button>("AllBtn");
                Transform getGroup = owner.getSavePanel.FindChildDepth("Group");
                getNum = getGroup.FindChildDepth<InputField>("SetGold/InputField");
                upperDesc = getGroup.FindChildDepth<Text>("UpperGold/Desc");
                bankPassword = getGroup.FindChildDepth<InputField>("BankPw/InputField");
                saveBtn = owner.getSavePanel.FindChildDepth<Button>("SaveBtn");
                getBtn = owner.getSavePanel.FindChildDepth<Button>("GetBtn");
                resetBtn = owner.getSavePanel.FindChildDepth<Button>("RestBtn");
                changeBankPasswordBtn = owner.getSavePanel.FindChildDepth<Button>("ChangePWBtn");
                isInit = true;
            }

            private void AddListener()
            {
                getBtn.onClick.RemoveAllListeners();
                getBtn.onClick.Add(GetGoldBtnOnClick);

                resetBtn.onClick.RemoveAllListeners();
                resetBtn.onClick.Add(OnClickResetCall);

                changeBankPasswordBtn.onClick.RemoveAllListeners();
                changeBankPasswordBtn.onClick.Add(OpenChangeBankPWPanel);

                getNum.onValueChanged.RemoveAllListeners();
                getNum.onValueChanged.Add(SetGetGoldEndValue);

                allSaveBtn.onClick.RemoveAllListeners();
                allSaveBtn.onClick.Add(OnClickAllCall);
            }

            private void OnClickAllCall()
            {
                getNum.text = bankGoldNum.text;
                upperDesc.text = ToolHelper.Low2Up(getNum.text);
            }

            private void OnClickResetCall()
            {
                upperDesc.text = "";
                getNum.text = "";
                bankPassword.text = "";
                getBtn.interactable = true;
            }

            private void OpenChangeBankPWPanel()
            {
                UIManager.Instance.OpenUI<UpdateBankPWPanel>();
            }

            private void HallEventOnChangeGoldTicket()
            {
                if (selfGoldNum != null)
                    selfGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD).ToString();
                if (bankGoldNum != null)
                    bankGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG).ToString();
            }

            private void GetOrSaveCallBack(HallStruct.ACP_SC_SaveOrGetGold data)
            {
                if (data.cbDrawOut == 0) return;
                GetGoldBtnCallBack(data);
            }

            /// <summary>
            /// 取出金币
            /// </summary>
            /// <param name="data"></param>
            private void GetGoldBtnCallBack(HallStruct.ACP_SC_SaveOrGetGold data)
            {
                getBtn.interactable = true;
                getNum.text = "";
                upperDesc.text = "";
                bankPassword.text = "";
                if (data.cbSuccess == 0)
                {
                    GameLocalMode.Instance.ChangProp(data.szInsureScore, Prop_Id.E_PROP_STRONG);
                    bankGoldNum.text = data.szInsureScore.ToString();
                    ILGameManager.Instance.QuerySelfGold();
                }
                else
                {
                    ToolHelper.PopSmallWindow(data.szInfoDiscrib);
                }
            }

            /// <summary>
            /// 存入金币
            /// </summary>
            private void GetGoldBtnOnClick()
            {
                var str = getNum.text;
                long gold = long.Parse(str);
                if (string.IsNullOrEmpty(str) || gold < 1)
                {
                    ToolHelper.PopSmallWindow("请检查你输入的金币是否正确");
                    return;
                }

                if (long.Parse(bankGoldNum.text) < gold)
                {
                    ToolHelper.PopSmallWindow("你持有的金币数量不足,请重新输入!");
                    return;
                }

                getBtn.interactable = false;
                var id = GameLocalMode.Instance.SCPlayerInfo.DwUser_Id;
                uint mark = 1;
                var pw = bankPassword.text;
                var getGold = new HallStruct.REQ_CS_SAVEORGETGOLD()
                {
                    gold = gold,
                    id = id,
                    mark = mark,
                    pwd = MD5Helper.MD5String(pw)
                };
                HotfixGameComponent.Instance.Send(DataStruct.BankStruct.MDM_GP_USER,
                    DataStruct.BankStruct.SUB_GP_USER_BANK_OPERATE, getGold.ByteBuffer, SocketType.Hall);
                OnClickResetCall();
                getBtn.interactable = true;
            }

            private void SetGetGoldEndValue(string value)
            {
                if (value.Length > 13)
                {
                    getNum.text = "";
                    ToolHelper.PopSmallWindow("数值过大");
                    return;
                }

                if (string.IsNullOrWhiteSpace(value) || long.Parse(value) <= 0)
                {
                    getNum.text = "";
                }
                else
                {
                    getNum.text = value;
                    upperDesc.text = ToolHelper.Low2Up(getNum.text);
                }
            }
        }

        /// <summary>
        /// 赠送
        /// </summary>
        private class GiveState : State<BankPanel>
        {
            public GiveState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private Text selfGoldNum;
            private Text bankGoldNum;
            private Text givePlayerNickName;
            private InputField giveGoldNum;
            private InputField givePlayerId;
            private Text giveGoldUpperNum;
            private Button giveCardBtn;
            private Button giveGoldBtn;

            private Transform transferGroup;

            private Button giveRecordBtn;
            private Button resetBtn;

            private HallStruct.ACP_SC_QueryPlayer queryPlayer;

            private GameObject selectObj;
            private long transferMoney;
            private int rate = 10000;
            private bool isInit;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                givePlayerNickName.text = "";
                giveGoldNum.text = "";
                givePlayerId.text = "";
                giveGoldUpperNum.text = "";
                selectObj.gameObject.SetActive(true);
                owner.leftTransferBtn.interactable = false;
                rate = GameLocalMode.Instance.GWData.MoneyRate;
                HallEvent.ChangeGoldTicket += HallEventOnChangeGoldTicket;
                HallEvent.OnQueryPlayer += HallEventOnQueryPlayer;
                HallEvent.OnTransferComplete += GiveGoldCallBack;
                HallEvent.DispatchChangeGoldTicket();
                owner.transferPanel.gameObject.SetActive(true);
            }

            public override void OnExit()
            {
                base.OnExit();
                selectObj.gameObject.SetActive(false);
                owner.leftTransferBtn.interactable = true;
                owner.transferPanel.gameObject.SetActive(false);
                HallEvent.ChangeGoldTicket -= HallEventOnChangeGoldTicket;
                HallEvent.OnQueryPlayer -= HallEventOnQueryPlayer;
                HallEvent.OnTransferComplete -= GiveGoldCallBack;
            }

            private void Init()
            {
                if (isInit) return;
                selectObj = owner.leftTransferBtn.transform.FindChildDepth($"Select").gameObject;
                selfGoldNum = owner.transferPanel.FindChildDepth<Text>("Gold/Text");
                bankGoldNum = owner.transferPanel.FindChildDepth<Text>("SaveGold/Text");
                givePlayerId = owner.transferPanel.FindChildDepth<InputField>("GiveGoldID/InputField");
                givePlayerNickName = owner.transferPanel.FindChildDepth<Text>("GiveGoldNiKename/titlename");
                giveGoldNum = owner.transferPanel.FindChildDepth<InputField>("GiveGoldNum/InputField");
                resetBtn = owner.transferPanel.FindChildDepth<Button>("GiveGoldNum/ResetBtn");
                giveGoldUpperNum = owner.transferPanel.FindChildDepth<Text>("UPGoldNum/uppernum");
                transferGroup = owner.transferPanel.FindChildDepth("TransferGroup");
                Transform BtnGroup = owner.transferPanel.FindChildDepth("BtnGroup");
                giveGoldBtn = BtnGroup.FindChildDepth<Button>("GiveGoldBtn");
                giveCardBtn = BtnGroup.FindChildDepth<Button>("GiveCardBtn");
                giveRecordBtn = BtnGroup.FindChildDepth<Button>("RedRecordBtn");
                AddListener();
                transferGroup.gameObject.SetActive(GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1);
                giveCardBtn.gameObject.SetActive(GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1);
                if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
                {
                    for (int i = 0; i < transferGroup.childCount; i++)
                    {
                        transferGroup.GetChild(i).gameObject.SetActive(false);
                    }

                    for (int i = 0; i < GameLocalMode.Instance.GWData.TransferConfig.Count; i++)
                    {
                        GameObject child = transferGroup.GetChild(i).gameObject;
                        //child.transform.SetParent(transferGroup);
                        child.name = GameLocalMode.Instance.GWData.TransferConfig[i].ToString();
                        int transferData = GameLocalMode.Instance.GWData.TransferConfig[i];
                        child.transform.FindChildDepth<Text>("Text").text = $"{transferData}元";
                        Button btn = child.GetComponent<Button>();
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.Add(() => { OnClickSelectMoneyCall(transferData); });
                        child.SetActive(true);
                    }
                }

                isInit = true;
            }

            private void AddListener()
            {
                resetBtn.onClick.RemoveAllListeners();
                resetBtn.onClick.Add(OnClickResetBtn);

                giveGoldBtn.onClick.RemoveAllListeners();
                giveGoldBtn.onClick.Add(() => { TishiGiveGoldYesBtnOnClick(giveGoldBtn); });

                giveCardBtn.onClick.RemoveAllListeners();
                giveCardBtn.onClick.Add(() => { UIManager.Instance.ReplaceUI<ZSCardPanel>(); });

                givePlayerId.onEndEdit.RemoveAllListeners();
                givePlayerId.onEndEdit.Add(ShowGivePlayerNickName);

                giveRecordBtn.onClick.RemoveAllListeners();
                giveRecordBtn.onClick.Add(GiveGoldRecordOnClick);

                giveGoldNum.onEndEdit.RemoveAllListeners();
                giveGoldNum.onEndEdit.AddListener(value => { GiveGoldEndValue(value); });
            }


            /// <summary>
            /// 重置
            /// </summary>
            private void OnClickResetBtn()
            {
                giveGoldNum.text = "";
                giveGoldUpperNum.text = "";
                giveCardBtn.interactable = true;
                transferMoney = 0;
            }

            private void HallEventOnChangeGoldTicket()
            {
                selfGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD).ToString();
                bankGoldNum.text = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG).ToString();
            }

            private void ShowGivePlayerNickName(string id)
            {
                DebugHelper.Log($"查询玩家：{id}");
                if (string.IsNullOrWhiteSpace(id)) return;
                HallStruct.REQ_CS_QueryPlayer queryPlayer = new HallStruct.REQ_CS_QueryPlayer();
                queryPlayer.id = uint.Parse(id);
                HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                    DataStruct.PersonalStruct.SUB_3D_CS_USER_INFO_SELECT, queryPlayer.ByteBuffer, SocketType.Hall);
            }

            private void HallEventOnQueryPlayer(HallStruct.ACP_SC_QueryPlayer obj)
            {
                queryPlayer = obj;
                if (givePlayerNickName == null) return;
                if (!queryPlayer.isReal)
                {
                    ToolHelper.PopSmallWindow($"ID不存在");
                    givePlayerNickName.text = "";
                    return;
                }

                givePlayerNickName.text = queryPlayer.nickName;
            }

            /// <summary>
            /// 点击转账按钮
            /// </summary>
            /// <param name="args"></param>
            private void TishiGiveGoldYesBtnOnClick(Button args)
            {
                if (string.IsNullOrWhiteSpace(GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber))
                {
                    ToolHelper.PopSmallWindow("需要绑定手机号才能使用该功能");
                    return;
                }

                uint.TryParse(givePlayerId.text,out uint cornucopiaID);
                long.TryParse(giveGoldNum.text,out long cornucopiaNum);
                if (string.IsNullOrEmpty(giveGoldNum.text))
                {
                    ToolHelper.PopSmallWindow("请输入正确的转账数额");
                    return;
                }

                if (cornucopiaID == GameLocalMode.Instance.SCPlayerInfo.DwUser_Id)
                {
                    ToolHelper.PopSmallWindow("请勿给自己转账");
                    return;
                }

                if (GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG) < cornucopiaNum)
                {
                    ToolHelper.PopSmallWindow("你的存款不足，请重新输入！");
                    return;
                }

                args.interactable = false;
                owner.transform.localPosition=Vector3.one *10000;
                ToolHelper.PopBigWindow(new BigMessage
                {
                    content =
                        $"接收人:{givePlayerNickName.text}({givePlayerId.text})\n数   额:{giveGoldNum.text}({giveGoldUpperNum.text})",
                    okCall = ()=>
                    {
                        GiveGoldYesBtnOnClick();
                        owner.transform.localPosition = Vector3.zero;
                    },
                    cancelCall = () =>
                    {
                        GiveGoldNoBtnOnClick();
                        owner.transform.localPosition = Vector3.zero;
                    }
                });
                args.interactable = true;
            }

            /// <summary>
            ///     取消转账
            /// </summary>
            private void GiveGoldNoBtnOnClick()
            {
                givePlayerId.text = "";
                givePlayerNickName.text = "";
                giveGoldNum.text = "";
                giveGoldUpperNum.text = "";
                transferMoney = 0;
            }

            /// <summary>
            /// 确认转账
            /// </summary>
            private void GiveGoldYesBtnOnClick()
            {
                uint.TryParse(givePlayerId.text,out uint cornucopiaID);
                long.TryParse(giveGoldNum.text, out long cornucopiaNum);
                transferMoney = 0;

                var transfer = new HallStruct.REQ_CS_TRANSFERACCOUNTS(cornucopiaID, cornucopiaNum);

                HotfixGameComponent.Instance.Send(DataStruct.GoldMineStruct.MDM_3D_GOLDMINE,
                    DataStruct.GoldMineStruct.SUB_3D_CS_TRANSFERACCOUNTS, transfer._ByteBuffer, SocketType.Hall);
            }

            /// <summary>
            /// 选择金额
            /// </summary>
            /// <param name="args"></param>
            private void OnClickSelectMoneyCall(int args)
            {
                ILMusicManager.Instance.PlayBtnSound();
                transferMoney += args * rate;

                if (transferMoney >= long.Parse(bankGoldNum.text)) transferMoney = long.Parse(bankGoldNum.text);

                giveGoldNum.text = transferMoney.ToString();
                giveGoldUpperNum.text = ToolHelper.Low2Up(transferMoney.ToString());
            }

            /// <summary>
            /// 监听输入完毕
            /// </summary>
            /// <param name="value"></param>
            private void GiveGoldEndValue(string value)
            {
                if (value.Length > 13)
                {
                    value = bankGoldNum.text;
                    ToolHelper.PopSmallWindow("数值过大");
                }

                if (string.IsNullOrEmpty(value) || long.Parse(value) < 0)
                {
                    transferMoney = 0;
                }
                else
                {
                    if (long.Parse(value) > long.Parse(bankGoldNum.text))
                    {
                        value = bankGoldNum.text;
                        giveGoldNum.text = bankGoldNum.text;
                        ToolHelper.PopSmallWindow($"输入数量大于银行存款");
                    }
                    transferMoney = long.Parse(value);
                    giveGoldUpperNum.text = ToolHelper.Low2Up(transferMoney.ToString());
                }
            }

            private void GiveGoldCallBack(bool isSuccess)
            {
                if (isSuccess)
                {
                    GiveGoldSuccess();
                }
                else
                {
                    GiveGoldFill();
                }
                transferMoney = 0;
            }

            /// <summary>
            /// 转账成功
            /// </summary>
            private void GiveGoldSuccess()
            {
                owner.QueryBank();
                //self.Gold.text = gameData.GetProp(enum_Prop_Id.E_PROP_STRONG)
                string str =
                    $"{GameLocalMode.Instance.SCPlayerInfo.BeautifulID}赠送给{givePlayerNickName.text}\n金币:{giveGoldNum.text}\n大写:{giveGoldUpperNum.text}\n时间:{DateTime.Now.ToString("yyyyMMddhhmmss")}\n成功!";
                UIManager.Instance.OpenUI<BankInfo>(str);
            }

            /// <summary>
            /// 失败
            /// </summary>
            private void GiveGoldFill()
            {
                givePlayerId.text = "";
                givePlayerNickName.text = "";
                giveGoldNum.text = "";
                giveGoldUpperNum.text = "";
                transferMoney = 0;
            }

            /// <summary>
            /// 转账记录
            /// </summary>
            private void GiveGoldRecordOnClick()
            {
                UIManager.Instance.ReplaceUI<GiveAndSendMoneyRecord>();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private class QueryState : State<BankPanel>
        {
            public QueryState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private InputField queryId;
            private Button queryIdBtn;
            private Transform queryIdObj;
            private Text queryIdText;
            private float queryIDTimer;
            private Text balancegold;
            private long bankGoldNum;
            private Transform recordResult;
            private Transform recordResultGroup;
            private bool isInit;
            private int maxTimer = 10;
            private bool isQueryID;
            private bool isStart;
            private GameObject selectObj;

            public override void OnEnter()
            {
                base.OnEnter();
                Init();
                queryId.text = "";
                queryIdText.text = "";
                balancegold.text = "";
                selectObj.gameObject.SetActive(true);
                owner.leftQueryBtn.interactable = false;
                owner.queryPanel.gameObject.SetActive(true);
                queryIdObj.gameObject.SetActive(true);
                queryIdText.gameObject.SetActive(false);
                HallEvent.SC_QUERY_UP_SCORE_RECORD += RecordCallBack;
                isStart = false;
            }

            public override void OnExit()
            {
                base.OnExit();
                selectObj.gameObject.SetActive(false);
                owner.leftQueryBtn.interactable = true;
                owner.queryPanel.gameObject.SetActive(false);
                HallEvent.SC_QUERY_UP_SCORE_RECORD -= RecordCallBack;
            }

            public override void Update()
            {
                base.Update();
                StartTimer();
            }

            private void Init()
            {
                if (isInit) return;
                selectObj = owner.leftQueryBtn.transform.FindChildDepth($"Select").gameObject;
                queryId = owner.queryPanel.FindChildDepth<InputField>("Content/QueryID/InputField");
                queryIdBtn = owner.queryPanel.FindChildDepth<Button>("Content/QueryID/QueryBtn");
                queryIdObj = queryIdBtn.transform.FindChildDepth("Img");
                queryIdText = queryIdBtn.transform.FindChildDepth<Text>("Text");
                balancegold = owner.queryPanel.FindChildDepth<Text>("Content/QueryBalance/Balance");
                recordResult = owner.queryPanel.FindChildDepth("Content/Group");
                recordResultGroup = recordResult.FindChildDepth("Result/Viewport/Content");
                AddListener();
                isInit = true;
            }

            private void AddListener()
            {
                queryIdBtn.onClick.RemoveAllListeners();
                queryIdBtn.onClick.Add(OnClickQueryIdCall);
            }

            private void InitQueryPanel()
            {
                queryIdObj.gameObject.SetActive(false);
                queryIdText.gameObject.SetActive(true);
                for (var i = 0; i < recordResultGroup.childCount; i++)
                {
                    recordResultGroup.GetChild(i).gameObject.SetActive(false);
                }
            }

            private void StartTimer()
            {
                if (!isStart) return;
                queryIDTimer -= Time.deltaTime;
                queryIdText.text = $"<color=black>{Mathf.Ceil(queryIDTimer)} s</color>";
                if (queryIDTimer > 0) return;
                isStart = false;
                queryIdBtn.interactable = true;
                queryIdObj.gameObject.SetActive(true);
                queryIdText.gameObject.SetActive(false);
            }

            private void OnClickQueryIdCall()
            {
                InitQueryPanel();
                queryIdBtn.interactable = false;
                queryIDTimer = maxTimer;
                queryIdText.text = $"<color=black>{Mathf.Ceil(queryIDTimer)} s</color>";
                queryIdObj.gameObject.SetActive(false);
                queryIdText.gameObject.SetActive(true);
                isQueryID = true;
                if (string.IsNullOrEmpty(queryId.text))
                {
                    ToolHelper.PopSmallWindow("请输入正确的ID!");
                    return;
                }

                recordResult.gameObject.SetActive(true);

                var buffer = new ByteBuffer();
                buffer.WriteInt(int.Parse(queryId.text));
                HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                    DataStruct.PersonalStruct.SUB_3D_CS_QUERY_UP_SCORE_RECORD, buffer, SocketType.Hall);
                isStart = true;
            }


            private void RecordCallBack(ByteBuffer buffer)
            {
                queryId.text = "";
                balancegold.text = "";
                HallStruct.ACP_SC_QueryID queryResult = new HallStruct.ACP_SC_QueryID(buffer);
                DebugHelper.Log($"{LitJson.JsonMapper.ToJson(queryResult)}");
                if (!queryResult.cbRes)
                {
                    DebugHelper.Log("没有数据");
                    ToolHelper.PopSmallWindow("没有查询到该玩家！");
                    return;
                }

                int count = queryResult.cbCount;
                if (count >= 9) count = 9;
                for (var i = 0; i < count; i++)
                {
                    GameObject child = recordResultGroup.gameObject.InstantiateChild(i);
                    HallStruct.ACP_SC_QueryID.QueryIDSubData data = queryResult.subDatas[i];
                    DateTime str = ((long) data.mTime).StampToDatetime();
                    child.transform.FindChildDepth("Order").GetComponent<Text>().text = data.mIndex.ToString();
                    child.transform.FindChildDepth("Receive").GetComponent<Text>().text = data.mRecverId.ToString();
                    child.transform.FindChildDepth("Send").GetComponent<Text>().text = data.mSenderId.ToString();
                    child.transform.FindChildDepth("Number").GetComponent<Text>().text = data.mGold.ToString();
                    child.transform.FindChildDepth("Time").GetComponent<Text>().text =
                        $"{str:MM-dd HH:mm}";
                    child.gameObject.SetActive(i < queryResult.cbCount);
                }

                var m_balance = queryResult.m_balance;
                DebugHelper.Log($"余额:{m_balance}");
                balancegold.text = m_balance.ToString();
            }
        }

        /// <summary>
        /// 赠送卡状态
        /// </summary>
        private class GiveCardState : State<BankPanel>
        {
            public GiveCardState(BankPanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            private GameObject selectObj;
            private bool isComplete;

            public override void OnEnter()
            {
                base.OnEnter();
                selectObj = owner.leftZSKBtn.transform.FindChildDepth($"Select").gameObject;
                selectObj.gameObject.SetActive(true);
                owner.leftZSKBtn.interactable = false;
                isComplete = false;
            }

            public override void OnExit()
            {
                base.OnExit();
                selectObj.gameObject.SetActive(false);
                owner.leftZSKBtn.interactable = true;
            }

            public override void Update()
            {
                base.Update();
                if (isComplete) return;
                isComplete = true;
                UIManager.Instance.ReplaceUI<ZSCardPanel>();
                hsm?.ChangeState(nameof(IdleState));
            }
        }
    }
}