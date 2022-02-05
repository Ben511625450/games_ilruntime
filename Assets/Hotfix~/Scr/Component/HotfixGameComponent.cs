using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hotfix.Hall;
using LitJson;
using LuaFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hotfix
{
    public enum SocketType
    {
        Hall = 0,
        Login = 1,
        Game = 2
    }

    public class HotfixGameComponent : Singleton<HotfixGameComponent>
    {
        private string currentEnterGame;
        private float currentHallHeartTimer;
        private float currentGameHeartTimer;
        public bool connectHallSuccess; //是否链接成功
        public bool connectGameSuccess; //是否链接成功
        private bool isHallReconnect;
        private bool isGameReconnect;
        private int reHallConnectCount;
        private int reGameConnectCount;

        private bool isShowReconnectTip;

        private float _resetTime;
        private bool _canReqHallHeart;
        private bool _canReqGameHeart;
        private HierarchicalStateMachine hsm;
        private bool isConnectGameNet;
        private bool isConnectHallNet;
        private bool isRealGameHeart;
        private bool isRealHallHeart;
        private bool isUseILRuntime;
        private List<IState> states;
        public event CAction<ushort, BytesPack> GameSceneInfoAction;
        public event CAction<ushort, BytesPack> LoginGameAction;
        public event CAction<ushort, BytesPack> GameFrameAction;
        public event CAction<ushort, BytesPack> GameAction;
        public event CAction<ushort, BytesPack> RoomBreakLineAction;
        public event CAction<ushort, BytesPack> OnSitAction;

        public event CAction<ushort, GameUserData> UserEnterAction;
        public event CAction<ushort, GameUserData> UserExitAction;
        public event CAction<ushort, GameUserData> UserStatusAction;
        public event CAction<ushort, GameUserData> UserScoreAction;

        protected override void Start()
        {
            base.Start();
            isUseILRuntime = false;
            _canReqGameHeart = true;
            _canReqHallHeart = true;
            hsm = new HierarchicalStateMachine(false, gameObject);
            states = new List<IState>();
            states.Add(new IdleState(this, hsm));
            states.Add(new EnterState(this, hsm));
            hsm.Init(states, nameof(IdleState));
        }

        protected override void Update()
        {
            base.Update();
            hsm?.Update();
            ReqHallHeart();
            ReqGameHeart();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            isUseILRuntime = false;
            CloseNetwork(SocketType.Game);
            CloseNetwork(SocketType.Hall);
        }

        protected override void OnApplicationFocus(bool isFocus)
        {
            base.OnApplicationFocus(isFocus);
            if (!isFocus)
            {
                _resetTime = Time.realtimeSinceStartup;
                _canReqGameHeart = false;
                _canReqHallHeart = false;
                DebugHelper.LogError($"切后台");
                return;
            }

            _canReqGameHeart = true;
            _canReqHallHeart = true;
            DebugHelper.LogError($"切回");
            if (!(Time.realtimeSinceStartup - _resetTime >= AppConst.ResetGameTimer)) return;
            if (Util.isPc || Util.isEditor) return;
            DebugHelper.LogError($"重置游戏");
            isUseILRuntime = false;
            isConnectGameNet = false;
            isConnectHallNet = false;
            CloseNetwork(SocketType.Game);
            CloseNetwork(SocketType.Hall);
            GameLocalMode.Instance.AccountList.isAuto = false;
            GameLocalMode.Instance.SaveAccount();
            if (GameLocalMode.Instance.IsInGame) EventHelper.DispatchLeaveGame();
            UIManager.Instance.CloseAllUI();
            UIManager.Instance.OpenUI<LogonScenPanel>();
        }

        protected override void AddEvent()
        {
            EventHelper.OnSocketReceive += EventHelper_OnSocketReceive;
            EventHelper.OnEnterGame += EventHelper_OnEnterGame;
            EventHelper.LeaveGame += EventHelper_LeaveGame1;
            HotfixActionHelper.LeaveGame += EventHelper_LeaveGame;
        }

        protected override void RemoveEvent()
        {
            EventHelper.OnEnterGame -= EventHelper_OnEnterGame;
            EventHelper.OnSocketReceive -= EventHelper_OnSocketReceive;
            EventHelper.LeaveGame -= EventHelper_LeaveGame1;
            HotfixActionHelper.LeaveGame -= EventHelper_LeaveGame;
        }

        private void EventHelper_LeaveGame1()
        {
            HotfixActionHelper.DispatchLeaveGame();
        }


        private void EventHelper_LeaveGame()
        {
            isUseILRuntime = false;
            isConnectGameNet = false;

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteUInt32(GameLocalMode.Instance.SCPlayerInfo.DwUser_Id);
            Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO, DataStruct.PersonalStruct.SUB_3D_CS_SELECT_GOLD_MSG,
                buffer, SocketType.Hall);
            //EventHelper.DispatchLeaveGame();
        }

        private void EventHelper_OnSocketReceive(BytesPack pack)
        {
            if (pack.session.Id < 0) return;
            DebugHelper.Log(
                $"<color=red>收到消息: mid:{pack.mid} sid:{pack.sid} size:{pack.bytes.Length} idx={pack.session.Id}</color>");
            switch (pack.mid)
            {
                case DataStruct.LoginStruct.MDM_3D_LOGIN: //登录消息
                    DispatchLoginHall(pack);
                    break;
                case DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO: //个人信息
                    DispatchPresonInfo(pack);
                    break;
                case DataStruct.GoldMineStruct.MDM_3D_GOLDMINE: //聚宝盆消息
                    DispatchGoldMine(pack);
                    break;
                case DataStruct.BankStruct.MDM_GP_USER: //银行消息
                    DispatchBankInfo(pack);
                    break;
                case DataStruct.LoginGameStruct.MDM_GR_LOGON: //游戏登录
                    DispatchLoginGame(pack);
                    break;
                case DataStruct.GameSceneStruct.MDM_ScenInfo: //场景消息
                    DispatchGameSceneInfo(pack);
                    break;
                case DataStruct.FrameStruct.MDM_3D_FRAME: //框架消息
                    DispatchGameFrameInfo(pack);
                    break;
                case DataStruct.UserDestDataStruct.MDM_3D_TABLE_USER_DATA: //游戏桌子数据
                    DispatchTableUserData(pack);
                    break;
                case DataStruct.GameStruct.MDM_GF_GAME: //游戏消息
                    DispatchGameDataInfo(pack);
                    break;
                case DataStruct.ClientHallHeart.MDM_3D_HEARTCOFIG: //心跳消息
                    isRealHallHeart = true;
                    break;
            }
        }

        private void EventHelper_OnEnterGame(string obj)
        {
            currentEnterGame = obj;
            hsm.ChangeState(nameof(EnterState));
        }

        public async void Connect(string ip, int port, int id, int timeOut = 7000, Action<string> callBack = null)
        {
            await AsyncCloseNetwork((SocketType) id);
            DebugHelper.Log($"ip:{ip} port:{port}");
            var session1 = new Session(ip, port, id, timeOut, (state, session) =>
            {
                session.run = state == "Yes";
                ActionComponent.Instance?.Add(() =>
                {
                    DebugHelper.Log($"session:{id}connect net:{state}");
                    callBack?.Invoke(state);
                });
            });
        }

        /// <summary>
        /// 获取socket状态
        /// </summary>
        /// <param name="socketType">socket类型</param>
        /// <returns></returns>
        public bool State(SocketType socketType)
        {
            NetworkManager.DicSession.TryGetValue((int) socketType, out var session);
            return session != null && session.run && session.Id > 0;
        }


        /// <summary>
        ///     发送消息
        /// </summary>
        /// <returns></returns>
        public bool Send(ushort mid, ushort sid, ByteBuffer byteBuffer, SocketType socketType)
        {
            var haskey = NetworkManager.DicSession.TryGetValue((int) socketType, out var session);
            if (!haskey) return false;
            if (session == null) return false;
            if (byteBuffer == null) byteBuffer = new ByteBuffer();
            DebugHelper.Log($"<color=green>发送消息 mid={mid},sid={sid},id={(int) socketType}</color>");
            var issend = session.Send(mid, sid, byteBuffer);
            byteBuffer?.Close();
            return issend;
        }

        /// <summary>
        ///     发送大厅心跳
        /// </summary>
        private async void ReqHallHeart()
        {
            if (!_canReqHallHeart) return;
            if (!isConnectHallNet) return;
            currentHallHeartTimer -= Time.deltaTime;
            if (currentHallHeartTimer > 0) return;
            currentHallHeartTimer = 7;
            if (!isRealHallHeart)
            {
                if (!isConnectHallNet) return;
                DebugHelper.LogError($"currentHallHeartTimer:{currentHallHeartTimer}");
                await AsyncCloseNetwork(SocketType.Hall);
                ReconnectHall();
                return;
            }

            isRealHallHeart = false;
            var heartMessage = new HallStruct.REQHeartMessage();
            Send(DataStruct.ClientHallHeart.MDM_3D_HEARTCOFIG, DataStruct.ClientHallHeart.SUB_3D_CS_HEART,
                heartMessage._ByteBuffer, SocketType.Hall);
        }

        /// <summary>
        ///     发送游戏心跳
        /// </summary>
        private async void ReqGameHeart()
        {
            if (!_canReqGameHeart) return;
            if (!isConnectGameNet) return;
            currentGameHeartTimer -= Time.deltaTime;
            if (currentGameHeartTimer > 0) return;
            currentGameHeartTimer = 7;
            if (!isRealGameHeart)
            {
                if (!isConnectGameNet) return;
                await AsyncCloseNetwork(SocketType.Game);
                ReconnectGame();
                return;
            }

            isRealGameHeart = false;
            var byteBuffer = new ByteBuffer();
            Send(DataStruct.FrameStruct.MDM_3D_FRAME, DataStruct.FrameStruct.SUB_3D_CS_GAME_HEART, byteBuffer,
                SocketType.Game);
        }

        /// <summary>
        ///     链接大厅服务器
        /// </summary>
        /// <param name="fun">链接成功的回调</param>
        public void ConnectHallServer(CAction<bool> fun)
        {
            DebugHelper.Log($"------------ConnectHallServer---{GameLocalMode.Instance.HallHost}---------");
            if (string.IsNullOrWhiteSpace(GameLocalMode.Instance.HallHost)) return;

            void CallBack(string _state)
            {
                ActionComponent.Instance.Add(() =>
                {
                    currentHallHeartTimer = 7;
                    if (_state == "Yes")
                    {
                        isRealHallHeart = true;
                        connectHallSuccess = true;
                        reHallConnectCount = 0;
                        isHallReconnect = false;
                        isConnectHallNet = true;
                    }
                    else
                    {
                        isRealHallHeart = false;
                        isConnectHallNet = false;
                        if (connectHallSuccess)
                        {
                            connectHallSuccess = false;
                            ReconnectHall();
                        }

                        ToolHelper.PopSmallWindow($"网络连接失败");
                    }

                    fun?.Invoke(connectHallSuccess);
                });
            }

            if (State(SocketType.Hall))
            {
                fun?.Invoke(true);
                ToolHelper.ShowWaitPanel(false);
                return;
            }

            ToolHelper.ShowWaitPanel(true, $"连接服务器中…");
            var ip = GameLocalMode.Instance.HallHost;
            var port = int.Parse(GameLocalMode.Instance.HallPort);
            var id = (int) SocketType.Hall;
            Connect(ip, port, id, 5000, CallBack);
        }

        /// <summary>
        /// 重连大厅
        /// </summary>
        public void ReconnectHall()
        {
            reHallConnectCount++;
            ToolHelper.ShowWaitPanel(true, $"正在重连中：{reHallConnectCount}…");
            isHallReconnect = true;
            ConnectHallServer(isSuccess =>
            {
                ToolHelper.ShowWaitPanel(false);
                if (isSuccess)
                {
                    ToolHelper.ShowWaitPanel(true, $"正在重连……");
                    ILGameManager.Instance.SendLoginMasseage(GameLocalMode.Instance.Account);
                }
                else
                {
                    if (reHallConnectCount >= 5)
                    {
                        ToolHelper.PopBigWindow(new BigMessage()
                        {
                            content = "网络错误！重连大厅失败",
                            okCall = () =>
                            {
                                GameLocalMode.Instance.AccountList.isAuto = false;
                                GameLocalMode.Instance.SaveAccount();
                                UIManager.Instance.CloseAllUI();
                                UIManager.Instance.OpenUI<LogonScenPanel>();
                            },
                            cancelCall = () =>
                            {
                                GameLocalMode.Instance.AccountList.isAuto = false;
                                GameLocalMode.Instance.SaveAccount();
                                UIManager.Instance.CloseAllUI();
                                UIManager.Instance.OpenUI<LogonScenPanel>();
                            }
                        });
                        return;
                    }

                    HallExtend.Instance.DelayRun(reHallConnectCount, () => { ReconnectHall(); });
                }
            });
        }

        /// <summary>
        /// 连接游戏服务器
        /// </summary>
        /// <param name="callback">回调</param>
        public void ConnectGameServer(CAction<bool> callback)
        {
            DebugHelper.Log("------------ConnectGameServer-------------");
            if (string.IsNullOrWhiteSpace(GameLocalMode.Instance.GameHost)) return;

            void CallBack(string _state)
            {
                currentGameHeartTimer = 7;
                if (_state == "Yes")
                {
                    isRealGameHeart = true;
                    connectGameSuccess = true;
                    Send(301, 5, new ByteBuffer(), SocketType.Game);
                    reGameConnectCount = 0;
                    isGameReconnect = false;
                    isConnectGameNet = true;
                }
                else
                {
                    isRealGameHeart = false;
                    connectGameSuccess = false;
                    isConnectGameNet = false;
                }

                ToolHelper.ShowWaitPanel(false);
                callback?.Invoke(connectGameSuccess);
            }

            if (State(SocketType.Game))
            {
                callback?.Invoke(true);
                ToolHelper.ShowWaitPanel(false);
                return;
            }

            ToolHelper.ShowWaitPanel(true, $"连接服务器中…");
            var ip = GameLocalMode.Instance.GameHost;
            var port = GameLocalMode.Instance.GamePort;
            var id = (int) SocketType.Game;
            Connect(ip, port, id, 5000, CallBack);
        }

        /// <summary>
        /// 重连游戏
        /// </summary>
        public void ReconnectGame()
        {
            ToolHelper.PopBigWindow(new BigMessage()
            {
                content = "重连游戏失败，即将回到大厅",
                okCall = EventHelper.DispatchLeaveGame,
                cancelCall = EventHelper.DispatchLeaveGame
            });
            return;
            //判断大厅是否断线
            if (!State(SocketType.Hall)) //断线,直接离开游戏
            {
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = "重连游戏失败，即将回到大厅",
                    okCall = EventHelper.DispatchLeaveGame,
                    cancelCall = EventHelper.DispatchLeaveGame
                });
                return;
            }

            reGameConnectCount++;
            ToolHelper.ShowWaitPanel(true, $"重连服务器中…");
            isGameReconnect = true;
            ConnectGameServer(isSuccess =>
            {
                ToolHelper.ShowWaitPanel(false);
                if (isSuccess)
                {
                    //重连成功 TODO
                }
                else
                {
                    if (reGameConnectCount >= 5)
                    {
                        ToolHelper.PopBigWindow(new BigMessage()
                        {
                            content = "网络错误！重连游戏失败",
                            okCall = () => { HotfixActionHelper.DispatchLeaveGame(); },
                            cancelCall = () => { HotfixActionHelper.DispatchLeaveGame(); }
                        });
                        return;
                    }

                    HallExtend.Instance.DelayRun(reGameConnectCount, () => { ReconnectGame(); });
                }
            });
        }

        private void DispatchLoginHall(BytesPack pack)
        {
            ByteBuffer buffer = new ByteBuffer(pack.bytes);
            switch (pack.sid)
            {
                case DataStruct.LoginStruct.SUB_3D_SC_LOGIN_SUCCESS:
                    GameLocalMode.Instance.SCPlayerInfo = new HallStruct.ACP_SC_LOGIN_SUCCESS(buffer);
                    DebugHelper.LogError($"{LitJson.JsonMapper.ToJson(GameLocalMode.Instance.SCPlayerInfo)}");
                    EventComponent.Instance.DispatchListener(HallEvent.LogonResultCallBack,true);
                    isShowReconnectTip = false;
                    if (GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID == 0 ||
                        GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID == 0) return;
                    GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID =
                        GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID % 1000 / 10;
                    break;
                case DataStruct.LoginStruct.SUB_3D_SC_ROOM_INFO_BEGIN:
                    if (GameLocalMode.Instance.AllSCGameRoom == null)
                    {
                        GameLocalMode.Instance.AllSCGameRoom = new List<HallStruct.RoomInfo>();
                    }

                    GameLocalMode.Instance.AllSCGameRoom.Clear();
                    break;
                case DataStruct.LoginStruct.SUB_3D_SC_ROOM_INFO_END:
                    //判断有无重连游戏
                    if (GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID == 0 ||
                        GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID == 0) return;
                    HallStruct.RoomInfo info = GameLocalMode.Instance.AllSCGameRoom.FindItem(p =>
                        p._2wGameID == GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID &&
                        p._1byFloorID == GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID);
                    if (info == null)
                    {
                        ToolHelper.PopSmallWindow($"重连游戏未开放");
                        return;
                    }

                    if (isShowReconnectTip) return;
                    isShowReconnectTip = true;
                    ToolHelper.PopBigWindow(new BigMessage()
                    {
                        content = $"您还有游戏未结束!",
                        okCall = () => { UIManager.Instance.OpenUI<DownLoadGamePanel>(info); }
                    });
                    break;
                case DataStruct.LoginStruct.SUB_3D_SC_ROOM_INFO:
                    ReadRoomInfo(buffer);
                    break;
                case DataStruct.LoginStruct.SUB_3D_SC_LOGIN_FAILE:
                {
                    var logFile = new HallStruct.ACP_SC_LOGIN_FAILE(buffer);
                    DebugHelper.LogError(logFile.Error);
                    ToolHelper.PopSmallWindow(logFile.Error);
                    EventComponent.Instance.DispatchListener(HallEvent.LogonResultCallBack,false);
                    // HallEvent.DispatchLogonResult(false);
                    break;
                }
                case DataStruct.LoginStruct.SUB_3D_SC_ACCOUNT_OFFLINE:
                    isUseILRuntime = false;
                    isConnectGameNet = false;
                    isConnectHallNet = false;
                    CloseNetwork(SocketType.Game);
                    CloseNetwork(SocketType.Hall);
                    GameLocalMode.Instance.AccountList.isAuto = false;
                    GameLocalMode.Instance.SaveAccount();
                    ToolHelper.PopBigWindow(new BigMessage()
                    {
                        content = "你的帐号在另一地方登录!",
                        okCall = () =>
                        {
                            if (GameLocalMode.Instance.IsInGame)
                            {
                                EventHelper.DispatchLeaveGame();
                            }

                            UIManager.Instance.CloseAllUI();
                            UIManager.Instance.OpenUI<LogonScenPanel>();
                        },
                        cancelCall = () =>
                        {
                            if (GameLocalMode.Instance.IsInGame)
                            {
                                EventHelper.DispatchLeaveGame();
                            }

                            UIManager.Instance.CloseAllUI();
                            UIManager.Instance.OpenUI<LogonScenPanel>();
                        }
                    });
                    break;
                case DataStruct.LoginStruct.SUB_3D_SC_REGISTER:
                {
                    var registerInfo = new HallStruct.ACP_SC_LOGIN_REGISTER(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.LogonRegisterCallBack, registerInfo);
                    break;
                }
                case DataStruct.LoginStruct.SUB_3D_SC_CODE:
                {
                    var codeInfo = new HallStruct.ACP_SC_CODE(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.LogonCodeCallBack, codeInfo.Code);
                    break;
                }
                case DataStruct.LoginStruct.SUB_SC_RES_MODIFY_USER_PASSWD_RESULT:
                {
                    HallStruct.ACP_SC_LOGIN_FINDPW PWInfo;
                    if (pack.bytes.Length > 0)
                    {
                        PWInfo = new HallStruct.ACP_SC_LOGIN_FINDPW(buffer);
                        EventComponent.Instance.DispatchListener(HallEvent.LogonFindPWCallBack, PWInfo);
                    }
                    else
                    {
                        EventComponent.Instance.DispatchListener(HallEvent.LogonFindPWCallBack);
                    }

                    break;
                }
                case DataStruct.LoginStruct.SUB_3D_SC_RESET_PASSWORD_CODE:
                {
                    var codeInfo = new HallStruct.ACP_SC_CODE(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.LogonFindPW_GetCode, (int) codeInfo.Code);
                    break;
                }
                case DataStruct.LoginStruct.SUB_3D_SC_LOGIN_CODE_RESULT:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.OnShowCodeLogin);
                    break;
                }
                case DataStruct.LoginStruct.SUB_SC_RES_MODIFY_USER_PASSWD_CHECK_CODE:
                {
                    var code = buffer.ReadInt32();
                    EventComponent.Instance.DispatchListener(HallEvent.LogonFindPW_GetCode, code);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 获取房间列表消息
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private void ReadRoomInfo(ByteBuffer buffer)
        {
            if (GameLocalMode.Instance.AllSCGameRoom == null)
            {
                GameLocalMode.Instance.AllSCGameRoom = new List<HallStruct.RoomInfo>();
            }

            HallStruct.ACP_SC_ROOM_INFO _SC_ROOM_INFO = new HallStruct.ACP_SC_ROOM_INFO(buffer);
            if (GameLocalMode.Instance.AllSCGameRoom.Count <= 0)
            {
                GameLocalMode.Instance.AllSCGameRoom.AddRange(_SC_ROOM_INFO.SubInfo);
            }
            else
            {
                for (int i = 0; i < _SC_ROOM_INFO.SubInfo.Count; i++) //筛选房间列表
                {
                    HallStruct.RoomInfo info = _SC_ROOM_INFO.SubInfo[i];
                    // DebugHelper.LogError($"Room:{JsonMapper.ToJson(info)}");
                    int index = GameLocalMode.Instance.AllSCGameRoom.FindListIndex(p =>
                        p._2wGameID == info._2wGameID && p._1byFloorID == info._1byFloorID);
                    if (index >= 0)
                    {
                        GameLocalMode.Instance.AllSCGameRoom[index] = info;
                    }
                    else
                    {
                        GameLocalMode.Instance.AllSCGameRoom.Add(info);
                    }
                }
            }
        }

        /// <summary>
        ///     个人信息
        /// </summary>
        private void DispatchPresonInfo(BytesPack pack)
        {
            var buffer = new ByteBuffer(pack.bytes);

            switch (pack.sid)
            {
                case DataStruct.PersonalStruct.SUB_3D_SC_USER_PROP:
                {
                    OnReceiveProp(buffer);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_SELECT_GOLD_MSG_RES:
                {
                    OnSelectPlayerGold(buffer);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_ChangeHeader:
                {
                    OnUpdatePlayerHeadImg(buffer);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_USER_INFO_SELECT:
                {
                    HallStruct.ACP_SC_QueryPlayer queryPlayer = new HallStruct.ACP_SC_QueryPlayer(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.OnQueryPlayer, queryPlayer);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_SIGN when pack.bytes.Length > 0:
                {
                    var sign = new HallStruct.ACP_SC_CHANGE_SIGN(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.Change_Sign, sign);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_SIGN:
                    EventComponent.Instance.DispatchListener(HallEvent.Change_Sign);
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_NICKNAME when pack.bytes.Length > 0:
                {
                    var nickName = new HallStruct.ACP_SC_UpdataNickName(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.SC_UpdataNickName, nickName);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_NICKNAME:
                    EventComponent.Instance.DispatchListener(HallEvent.SC_UpdataNickName);
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_ACCOUNT when pack.bytes.Length > 0:
                {
                    var acc = new HallStruct.ACP_SC_CHANGE_ACCOUNT(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.SC_CHANGE_ACCOUNT, acc);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_ACCOUNT:
                    EventComponent.Instance.DispatchListener(HallEvent.SC_CHANGE_ACCOUNT);
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_PASSWORD when pack.bytes.Length > 0:
                {
                    var acc = new HallStruct.ACP_SC_CHANGE_PASSWOR(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.SC_CHANGE_PASSWORD, acc);
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_CHANGE_PASSWORD:
                    EventComponent.Instance.DispatchListener(HallEvent.SC_CHANGE_PASSWORD);
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_DIANKA_QUERY:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.DIANKA_QUERY,
                        new HallStruct.ACP_SC_DIANKA_QUERY(buffer));
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_DIANKA_GIVE:
                    EventComponent.Instance.DispatchListener(HallEvent.ZSCardResult,
                        new HallStruct.ACP_SC_ZSCardResult(buffer));
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_DIANKA_RECEIVE:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.DIANKA_RECEIVE,
                        new HallStruct.ACP_SC_DIANKA_RECEIVE(buffer));
                    break;
                }
                case DataStruct.PersonalStruct.SUB_3D_SC_QUERY_UP_SCORE_RECORD:
                    EventComponent.Instance.DispatchListener(HallEvent.SC_QUERY_UP_SCORE_RECORD,
                        new HallStruct.ACP_SC_QueryID(buffer));
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_QUERYLOGINVERIFY_RES:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.OnQueryLoginVerifyCallBack,
                        new HallStruct.ACP_SC_QueryLoginVerify(buffer));
                }
                    break;
                case DataStruct.PersonalStruct.SUB_3D_SC_STRONG_BOX_COME_IN:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.OnEnterBank, new HallStruct.InitBank(buffer));
                }
                    break;
            }
        }

        /// <summary>
        ///     更新玩家头像
        /// </summary>
        /// <param name="buffer"></param>
        private void OnUpdatePlayerHeadImg(ByteBuffer buffer)
        {
            int changeResult = buffer.ReadByte();
            if (changeResult == 0)
            {
                ToolHelper.PopSmallWindow("更改头像失败");
            }
            else
            {
                EventComponent.Instance.DispatchListener(HallEvent.ChangeHeader, (uint) changeResult);
                ToolHelper.PopSmallWindow("更改头像成功");
                UIManager.Instance.Close();
            }
        }

        /// <summary>
        ///     查询玩家金币
        /// </summary>
        /// <param name="buffer"></param>
        private void OnSelectPlayerGold(ByteBuffer buffer)
        {
            var select = new HallStruct.ACP_SC_SELECT_GOLD(buffer);
            var tmeNum = -1;
            for (var i = 0; i < GameLocalMode.Instance.AllSCUserProp.Count; i++)
                if (GameLocalMode.Instance.AllSCUserProp[i].User_Id == select.User_Id)
                    tmeNum = i;

            if (tmeNum >= 0)
                GameLocalMode.Instance.ChangProp(select.Self_Gold, Prop_Id.E_PROP_GOLD);

            EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
        }

        /// <summary>
        ///     更新金币
        /// </summary>
        /// <param name="buffer"></param>
        private void OnReceiveProp(ByteBuffer buffer)
        {
            var USER_PROP = new HallStruct.Acp_SC_USER_PROP(buffer);
            if (GameLocalMode.Instance.AllSCUserProp == null)
            {
                GameLocalMode.Instance.AllSCUserProp = new List<HallStruct.Acp_SC_USER_PROP>();
                GameLocalMode.Instance.AllSCUserProp.Add(USER_PROP);
            }
            else
            {
                var tmeNum = GameLocalMode.Instance.AllSCUserProp.FindListIndex(p => p.User_Id == USER_PROP.User_Id);
                if (tmeNum >= 0)
                    GameLocalMode.Instance.AllSCUserProp[tmeNum] = USER_PROP;
                else
                    GameLocalMode.Instance.AllSCUserProp.Add(USER_PROP);
            }
            EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
        }


        /// <summary>
        ///     转账
        /// </summary>
        /// <param name="pack"></param>
        private void DispatchGoldMine(BytesPack pack)
        {
            var buffer = new ByteBuffer(pack.bytes);

            switch (pack.sid)
            {
                //转账
                case DataStruct.GoldMineStruct.SUB_3D_SC_TRANSFERACCOUNTS when pack.bytes.Length == 0:
                    EventComponent.Instance.DispatchListener(HallEvent.OnTransferComplete, true);
                    break;
                case DataStruct.GoldMineStruct.SUB_3D_SC_TRANSFERACCOUNTS when pack.bytes.Length == 100:
                    ToolHelper.PopSmallWindow("网络错误，请重新操作");
                    EventComponent.Instance.DispatchListener(HallEvent.OnTransferComplete, false);
                    break;
                case DataStruct.GoldMineStruct.SUB_3D_SC_TRANSFERACCOUNTS:
                {
                    int num = buffer.ReadUInt16();
                    var msg = buffer.ReadString(num);
                    EventComponent.Instance.DispatchListener(HallEvent.OnTransferComplete, false);
                    ToolHelper.PopSmallWindow(msg);
                    break;
                }
                //获取记录
                case DataStruct.GoldMineStruct.SUB_2D_SC_GIVE_RECORD_LIST:
                    EventComponent.Instance.DispatchListener(HallEvent.SC_Give_Record_List,
                        new HallStruct.ACP_SC_Bank_AllRecord(buffer));
                    break;
                case DataStruct.GoldMineStruct.SUB_3D_SC_UPDATEBANKERSAVEGOLD:
                {
                    HallStruct.ACP_SC_UPDATEBANKERSAVEGOLD updatebankersavegold =
                        new HallStruct.ACP_SC_UPDATEBANKERSAVEGOLD(buffer);
                    long gold = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG) + updatebankersavegold.gold;
                    GameLocalMode.Instance.ChangProp(gold, Prop_Id.E_PROP_STRONG);
                    EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
                    break;
                }
                case DataStruct.GoldMineStruct.SUB_3D_SC_WITHDRAW:
                {
                    HallStruct.ACP_SC_WITHDRAW recall = new HallStruct.ACP_SC_WITHDRAW(buffer);
                    long gold = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_STRONG);
                    if (recall.recallUserID == GameLocalMode.Instance.SCPlayerInfo.DwUser_Id)
                    {
                        GameLocalMode.Instance.ChangProp(gold - recall.recallGold, Prop_Id.E_PROP_STRONG);
                    }
                    else
                    {
                        GameLocalMode.Instance.ChangProp(gold + recall.recallGold, Prop_Id.E_PROP_STRONG);
                    }

                    EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
                    ToolHelper.PopSmallWindow(recall.recallMsg);
                    break;
                }
            }
        }


        /// <summary>
        ///     银行
        /// </summary>
        /// <param name="pack"></param>
        private void DispatchBankInfo(BytesPack pack)
        {
            DebugHelper.Log("接收银行操作");

            var buffer = new ByteBuffer(pack.bytes);
            switch (pack.sid)
            {
                case DataStruct.BankStruct.SUB_GP_BANKOPENACCOUNTRESULT:
                {
                    DebugHelper.Log("=====接收银行操作128=====");
                    buffer.ReadInt32();
                    var selfGold = buffer.ReadInt64();
                    var bankGold = buffer.ReadInt64();
                    DebugHelper.Log($"selfGold:{selfGold}----------bankGold:{bankGold}");
                    GameLocalMode.Instance.ChangProp(selfGold, Prop_Id.E_PROP_GOLD);
                    GameLocalMode.Instance.ChangProp(bankGold, Prop_Id.E_PROP_STRONG);
                    if (ILGameManager.isOpenBank) UIManager.Instance.OpenUI<BankPanel>();

                    EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
                    break;
                }
                case DataStruct.BankStruct.SUB_GP_SETBANKPASSRESULT:
                {
                    var list = new List<string>();
                    list.Add(buffer.ReadInt64Str());
                    list.Add(buffer.ReadInt64Str());
                    list.Add(buffer.ReadInt64Str());
                    list.Add(buffer.ReadString(1024));
                    EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
                    EventComponent.Instance.DispatchListener(HallEvent.SETBANKPASSRESULT, list);
                    break;
                }
                case DataStruct.BankStruct.SUB_GP_USER_BANK_OPERATE_RESULT:
                {
                    var data = new HallStruct.ACP_SC_SaveOrGetGold(buffer);
                    EventComponent.Instance.DispatchListener(HallEvent.Bank_Operate_Result, data);
                    EventComponent.Instance.DispatchListener(HallEvent.ChangeGoldTicket);
                    break;
                }
                case DataStruct.BankStruct.SUB_GP_MODIFY_BANK_PASSWD_CHECK_CODE_RESULT:
                {
                    EventComponent.Instance.DispatchListener(HallEvent.Bank_Change_PW,
                        new HallStruct.ACP_SC_Bank_Change_PW(buffer));
                    break;
                }
            }
        }


        /// <summary>
        ///     登录
        /// </summary>
        /// <param name="pack"></param>
        private void DispatchLoginGame(BytesPack pack)
        {
            if (!isUseILRuntime) return;
            var buffer = new ByteBuffer(pack.bytes);
            switch (pack.sid)
            {
                case DataStruct.LoginGameStruct.SUB_GR_LOGON_SUCCESS: //登录游戏成功
                    var gameUserData = new GameUserData(buffer);
                    GameLocalMode.Instance.UserGameInfo = gameUserData;
                    break;
                case DataStruct.LoginGameStruct.SUB_GR_LOGON_ERROR: //登录游戏失败
                    var error = buffer.ReadString(pack.bytes.Length);
                    var haskey = NetworkManager.DicSession.TryGetValue((int) SocketType.Game, out var session);
                    if (haskey && session != null)
                        if (session.run)
                            session.Dispose();

                    ToolHelper.PopBigWindow(new BigMessage
                    {
                        content = error,
                        okCall = QuitGame,
                        cancelCall = QuitGame
                    });
                    break;
                case DataStruct.LoginGameStruct.SUB_GR_LOGON_FINISH: //登录游戏完成
                    if (GameLocalMode.Instance.UserGameInfo.ChairId == 65535)
                    {
                        var byteBuffer = new ByteBuffer();
                        Send(DataStruct.UserDataStruct.MDM_GR_USER, DataStruct.UserDataStruct.SUB_GR_USER_SIT_AUTO,
                            byteBuffer, SocketType.Game); //入座
                    }
                    else
                    {
                        var _PLAYERPREPARE = new REQ_PLAYERPREPARE {OnLookerTag = 0};
                        Send(DataStruct.GameSceneStruct.MDM_ScenInfo, DataStruct.GameSceneStruct.SUB_GF_INFO,
                            _PLAYERPREPARE.ByteBuffer, SocketType.Game); //准备
                    }

                    break;
            }

            LoginGameAction?.Invoke((ushort) pack.sid, pack);
        }

        /// <summary>
        ///     退出游戏
        /// </summary>
        private void QuitGame()
        {
            isUseILRuntime = false;
            isConnectGameNet = false;
            var byteBuffer = new ByteBuffer();
            Send(DataStruct.UserDataStruct.MDM_GR_USER, DataStruct.UserDataStruct.SUB_GR_USER_LEFT_GAME_REQ, byteBuffer,
                SocketType.Game); //入座
        }

        public Task AsyncCloseNetwork(SocketType socketType)
        {
            switch (socketType)
            {
                case SocketType.Hall:
                    connectHallSuccess = false;
                    isConnectHallNet = false;
                    break;
                case SocketType.Game:
                    connectGameSuccess = false;
                    isConnectGameNet = false;
                    break;
            }

            string netType = socketType == SocketType.Hall ? "大厅" : "游戏";
            ActionComponent.Instance?.Add(() => { ToolHelper.ShowWaitPanel(true, $"正在重置{netType}网络"); });
            return Task.Run(() =>
            {
                Session session = null;
                NetworkManager.DicSession.TryGetValue((int) socketType, out session);
                if (session != null)
                {
                    session.CloseFunc = null;
                    session.CallBack = null;
                    session?.Dispose();
                }

                ActionComponent.Instance?.Add(() => { ToolHelper.ShowWaitPanel(false); });
                if (session == null) return;
                DebugHelper.LogError($"释放session：{session.Id}");
                session.Id = -1;
            });
        }

        public async void CloseNetwork(SocketType socketType)
        {
            await AsyncCloseNetwork(socketType);
        }

        /// <summary>
        ///     场景消息
        /// </summary>
        /// <param name="pack"></param>
        private void DispatchGameSceneInfo(BytesPack pack)
        {
            if (!isUseILRuntime) return;
            var buffer = new ByteBuffer(pack.bytes);
            switch (pack.sid)
            {
                case DataStruct.GameSceneStruct.SUB_GF_OPTION:
                    var option = new CMD_GF_Option(buffer);
                    DebugHelper.Log($"收到场景信息={option.GameStatus}==={option.AllowLookon}");
                    break;
                case DataStruct.GameSceneStruct.SUB_GF_SCENE:
                    GameSceneInfoAction?.Invoke((ushort) pack.sid, pack);
                    break;
                case DataStruct.GameSceneStruct.SUB_GF_MESSAGE:
                    var message = new CMD_GF_SystemMessage(buffer);
                    PopComponent.Instance.ShowSmall(message.message);
                    break;
            }
        }

        private void DispatchGameFrameInfo(BytesPack pack)
        {
            var buffer = new ByteBuffer(pack.bytes);
            switch (pack.sid)
            {
                case DataStruct.FrameStruct.SUB_3D_SC_AUTO_SIT:
                    if (pack.bytes.Length > 0)
                    {
                        var message = buffer.ReadString(pack.bytes.Length);
                        ToolHelper.PopBigWindow(new BigMessage
                        {
                            content = message,
                            okCall = () =>
                            {
                                //TODO 退出游戏
                                HotfixActionHelper.DispatchLeaveGame();
                            },
                            cancelCall = () =>
                            {
                                //TODO 退出游戏
                                HotfixActionHelper.DispatchLeaveGame();
                            }
                        });
                    }
                    else
                    {
                        var playerprepare = new REQ_PLAYERPREPARE {OnLookerTag = 0};
                        var playerprepareBuffer = new ByteBuffer();
                        playerprepareBuffer.WriteByte(playerprepare.OnLookerTag);
                        Send(DataStruct.GameSceneStruct.MDM_ScenInfo, DataStruct.GameSceneStruct.SUB_GF_INFO,
                            playerprepareBuffer, SocketType.Game); //准备
                    }

                    OnSitAction?.Invoke((ushort) pack.sid, pack);
                    break;
                case DataStruct.FrameStruct.SUB_3D_SC_ROOM_INFO_OFFLINE: //断线
                    RoomBreakLineAction?.Invoke((ushort) pack.sid, pack);
                    break;
                case DataStruct.FrameStruct.SUB_3D_SC_GAME_HEART: //心跳返回
                    DebugHelper.LogError($"游戏心跳");
                    isRealGameHeart = true;
                    break;
            }

            GameFrameAction?.Invoke((ushort) pack.sid, pack);
        }

        private void DispatchTableUserData(BytesPack pack)
        {
            if (!isUseILRuntime) return;
            var byteBuffer = new ByteBuffer(pack.bytes);
            var userData = new GameUserData(byteBuffer);
            switch (pack.sid)
            {
                case DataStruct.UserDestDataStruct.SUB_3D_TABLE_USER_ENTER: //玩家进入
                    UserEnterAction?.Invoke((ushort) pack.sid, userData);
                    break;
                case DataStruct.UserDestDataStruct.SUB_3D_TABLE_USER_LEAVE: //玩家离开
                    UserExitAction?.Invoke((ushort) pack.sid, userData);
                    break;
                case DataStruct.UserDestDataStruct.SUB_3D_TABLE_USER_SCORE: //玩家分数
                    UserScoreAction?.Invoke((ushort) pack.sid, userData);
                    break;
                case DataStruct.UserDestDataStruct.SUB_3D_TABLE_USER_STATUS: //玩家状态
                    UserStatusAction?.Invoke((ushort) pack.sid, userData);
                    break;
            }
        }

        /// <summary>
        ///     游戏消息
        /// </summary>
        /// <param name="pack"></param>
        private void DispatchGameDataInfo(BytesPack pack)
        {
            GameAction?.Invoke((ushort) pack.sid, pack);
        }

        private class IdleState : State<HotfixGameComponent>
        {
            public IdleState(HotfixGameComponent owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
        }

        private class EnterState : State<HotfixGameComponent>
        {
            private bool isComplete;

            public EnterState(HotfixGameComponent owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                isComplete = false;
            }

            public override void Update()
            {
                base.Update();
                if (isComplete) return;
                isComplete = true;
                //检测游戏是否使用ILRuntime
                DebugHelper.Log($"进入游戏:{owner.currentEnterGame}");
                var gameData = GameConfig.GetGameData(owner.currentEnterGame);
                if (gameData == null) return;
                owner.isRealGameHeart = true;
                owner.isUseILRuntime = true;
                owner.isConnectGameNet = true;
                LoadGameComponent.Instance.LoadGameScript(gameData);
                hsm?.ChangeState(nameof(IdleState));
            }
        }
    }
}