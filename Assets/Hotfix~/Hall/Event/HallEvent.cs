using System.Collections.Generic;
using System.ComponentModel;
using LuaFramework;
using static Hotfix.HallStruct;

namespace Hotfix
{
    public class HallEvent
    {
        /// <summary>
        ///     登录成功
        /// </summary>
        public static event CAction<bool> LogonResultCallBack;

        public static void DispatchLogonResult(bool isSuccess)
        {
            LogonResultCallBack?.Invoke(isSuccess);
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        public static event CAction EnterHall;

        public static void DispatchEnterHall()
        {
            EnterHall?.Invoke();
        }


        /// <summary>
        /// 确认注册
        /// </summary>
        public static event CAction<ACP_SC_LOGIN_REGISTER> LogonRegisterCallBack;

        public static void DispatchRegister(ACP_SC_LOGIN_REGISTER registerInfo)
        {
            LogonRegisterCallBack?.Invoke(registerInfo);
        }

        /// <summary>
        ///     注册验证码
        /// </summary>
        public static event CAction<uint> LogonCodeCallBack;

        public static void DispatchCodeCallBack(uint num)
        {
            LogonCodeCallBack?.Invoke(num);
        }


        /// <summary>
        ///     修改密码
        /// </summary>
        public static event CAction<ACP_SC_LOGIN_FINDPW> LogonFindPWCallBack;

        public static void DispatchLogonFindPW(ACP_SC_LOGIN_FINDPW pwInfo = null)
        {
            LogonFindPWCallBack?.Invoke(pwInfo);
        }

        /// <summary>
        ///     修改密码获取验证码
        /// </summary>
        public static event CAction<int> LogonFindPW_GetCode;

        public static void DispatchLogonFindPW_GetCode(int code)
        {
            LogonFindPW_GetCode?.Invoke(code);
        }

        /// <summary>
        ///     心跳消息
        /// </summary>
        public static event CAction S_CHallHeart;

        public static void DispatchS_CHallHeart()
        {
            S_CHallHeart?.Invoke();
        }

        /// <summary>
        ///     更新金币
        /// </summary>
        public static event CAction ChangeGoldTicket;

        public static void DispatchChangeGoldTicket()
        {
            ChangeGoldTicket?.Invoke();
        }

        /// <summary>
        ///     大厅名字同步
        /// </summary>
        public static event CAction ChangeHallNiKeName;

        public static void DispatchChangeHallNiKeName()
        {
            ChangeHallNiKeName?.Invoke();
        }

        /// <summary>
        ///     更改头像
        /// </summary>
        public static event CAction<int> ChangeHeader;

        public static void DispatchChangeHeader(int faceID)
        {
            ChangeHeader?.Invoke(faceID);
        }

        /// <summary>
        ///     更新签名
        /// </summary>
        public static event CAction<ACP_SC_CHANGE_SIGN> Change_Sign;

        public static void DispatchChange_Sign(ACP_SC_CHANGE_SIGN str = null)
        {
            Change_Sign?.Invoke(str);
        }

        /// <summary>
        ///     更新昵称
        /// </summary>
        public static event CAction<ACP_SC_UpdataNickName> SC_UpdataNickName;

        public static void DispatchSC_UpdataNickName(ACP_SC_UpdataNickName str = null)
        {
            SC_UpdataNickName?.Invoke(str);
        }

        /// <summary>
        /// 查询玩家回调
        /// </summary>
        public static event CAction<ACP_SC_QueryPlayer> OnQueryPlayer;

        public static void DispatchOnQueryPlayer(ACP_SC_QueryPlayer queryPlayer)
        {
            OnQueryPlayer?.Invoke(queryPlayer);
        }

        /// <summary>
        ///     点卡查询
        /// </summary>
        public static event CAction<ACP_SC_DIANKA_QUERY> DIANKA_QUERY;

        public static void DispatchDIANKA_QUERY(ACP_SC_DIANKA_QUERY query)
        {
            DIANKA_QUERY?.Invoke(query);
        }

        /// <summary>
        ///     点卡领取
        /// </summary>
        public static event CAction<ACP_SC_DIANKA_RECEIVE> DIANKA_RECEIVE;

        public static void DispatchDIANKA_RECEIVE(ACP_SC_DIANKA_RECEIVE receive)
        {
            DIANKA_RECEIVE?.Invoke(receive);
        }

        /// <summary>
        ///     点卡赠送
        /// </summary>
        public static event CAction<ACP_SC_DIANKA_GIVE> DIANKA_GIVE;

        public static void DispatchDIANKA_GIVE(ACP_SC_DIANKA_GIVE give)
        {
            DIANKA_GIVE?.Invoke(give);
        }

        /// <summary>
        ///     进入银行
        /// </summary>
        public static event CAction<List<string>> SETBANKPASSRESULT;

        public static void DispatchSETBANKPASSRESULT(List<string> list)
        {
            SETBANKPASSRESULT?.Invoke(list);
        }

        /// <summary>
        /// 银行存取
        /// </summary>
        public static event CAction<ACP_SC_SaveOrGetGold> Bank_Operate_Result;

        public static void DispatchBank_Operate_Result(ACP_SC_SaveOrGetGold list)
        {
            Bank_Operate_Result?.Invoke(list);
        }


        /// <summary>
        ///     转账
        /// </summary>
        public static event CAction<bool> OnTransferComplete;

        public static void DispatchOnTransferComplete(bool isSuccess)
        {
            OnTransferComplete?.Invoke(isSuccess);
        }

        /// <summary>
        ///     查询
        /// </summary>
        public static event CAction<ByteBuffer> SC_QUERY_UP_SCORE_RECORD;

        public static void DispatchSC_QUERY_UP_SCORE_RECORD(ByteBuffer buffer)
        {
            SC_QUERY_UP_SCORE_RECORD?.Invoke(buffer);
        }

        /// <summary>
        ///     赠送卡
        /// </summary>
        public static event CAction<ByteBuffer> ZSCardResult;

        public static void DispatchZSCardResult(ByteBuffer buffer)
        {
            ZSCardResult?.Invoke(buffer);
        }

        /// <summary>
        ///     转账记录
        /// </summary>
        public static event CAction<ByteBuffer> SC_Give_Record_List;

        public static void DispatchSC_Give_Record_List(ByteBuffer buffer)
        {
            SC_Give_Record_List?.Invoke(buffer);
        }

        /// <summary>
        ///     修改银行密码
        /// </summary>
        public static event CAction<ACP_SC_Bank_Change_PW> Bank_Change_PW;

        public static void DispatchBank_Change_PW(ACP_SC_Bank_Change_PW data)
        {
            Bank_Change_PW?.Invoke(data);
        }


        /// <summary>
        ///     绑定验证码
        /// </summary>
        public static event CAction<uint> SC_BindCodeCallBack;

        public static void DispatchSC_BindCodeCallBack(uint data)
        {
            SC_BindCodeCallBack?.Invoke(data);
        }

        /// <summary>
        ///     绑定账号
        /// </summary>
        public static event CAction<ACP_SC_CHANGE_ACCOUNT> SC_CHANGE_ACCOUNT;

        public static void DispatchSC_CHANGE_ACCOUNT(ACP_SC_CHANGE_ACCOUNT str = null)
        {
            SC_CHANGE_ACCOUNT?.Invoke(str);
        }

        public static event CAction<bool> EnterGamePre;

        /// <summary>
        /// 进入选场
        /// </summary>
        public static void DispatchEnterGamePre(bool isEnter)
        {
            EnterGamePre?.Invoke(isEnter);
        }

        /// <summary>
        /// 登录游戏
        /// </summary>
        public static event CAction LoginGame;

        public static void DispatchLoginGame()
        {
            LoginGame?.Invoke();
        }


        /// <summary>
        ///     修改密码
        /// </summary>
        public static event CAction<ACP_SC_CHANGE_PASSWOR> SC_CHANGE_PASSWORD;

        public static void DispatchSC_CHANGE_PASSWORD(ACP_SC_CHANGE_PASSWOR str = null)
        {
            SC_CHANGE_PASSWORD?.Invoke(str);
        }

        /// <summary>
        /// 开始下载游戏
        /// </summary>
        public static event CAction<string> BeginDwonGame;

        public static void DispatchLoginGame(string str)
        {
            BeginDwonGame?.Invoke(str);
        }

        /// <summary>
        /// 下载游戏完成
        /// </summary>
        public static event CAction<AsyncOperation> DwonGameSucceed;

        public static void DispatchLoginGame(AsyncOperation apt)
        {
            DwonGameSucceed?.Invoke(apt);
        }

        public static event CAction OnClosePopBig;

        /// <summary>
        /// 关闭弹框
        /// </summary>
        public static void DispatchOnClosePopBig()
        {
            OnClosePopBig?.Invoke();
        }

        public static event CAction OnShowCodeLogin;

        /// <summary>
        /// 打开登录验证
        /// </summary>
        public static void DispatchOnShowCodeLogin()
        {
            OnShowCodeLogin?.Invoke();
        }

        public static event CAction<HallStruct.ACP_SC_QueryLoginVerify> OnQueryLoginVerifyCallBack;

        /// <summary>
        /// 登录验证返回
        /// </summary>
        /// <param name="acpScQueryLoginVerify"></param>
        public static void DispatchOnQueryLoginVerifyCallBack(ACP_SC_QueryLoginVerify acpScQueryLoginVerify)
        {
            OnQueryLoginVerifyCallBack?.Invoke(acpScQueryLoginVerify);
        }

        public static event CAction<InitBank> OnEnterBank;

        /// <summary>
        /// 初始化银行
        /// </summary>
        /// <param name="bank"></param>
        public static void DispatchOnEnterBank(InitBank bank)
        {
            OnEnterBank?.Invoke(bank);
        }


        public static event CAction<bool> OnMoveHallIcon;

        /// <summary>
        /// 移动大厅icon
        /// </summary>
        /// <param name="isMove"></param>
        public static void DispatchOnMoveHallIcon(bool isMove)
        {
            OnMoveHallIcon?.Invoke(isMove);
        }
    }
}