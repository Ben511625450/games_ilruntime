using System.Collections.Generic;
using System.Runtime.InteropServices;
using LuaFramework;

namespace Hotfix
{
    public class HallStruct
    {
        #region ACP

        public class ACP_CS_Version
        {
            /// <summary>
            ///     是否上线
            /// </summary>
            public byte Online;

            /// <summary>
            ///     是否更新
            /// </summary>
            public byte Update;

            /// <summary>
            ///     更新地址,32
            /// </summary>
            public string Url;

            /// <summary>
            ///     地址大小
            /// </summary>
            public ushort UrlLength;

            public ACP_CS_Version(ByteBuffer byteBuffer)
            {
                Update = byteBuffer.ReadByte();
                Online = byteBuffer.ReadByte();
                UrlLength = byteBuffer.ReadUInt16();
                Url = byteBuffer.ReadString(UrlLength);
            }
        }

        /// <summary>
        ///     登录大厅成功
        /// </summary>
        public class ACP_SC_LOGIN_SUCCESS
        {
            //512
            public string Account;

            private readonly ushort AccountLength;
            public byte BBindInviteCode;
            public uint BeautifulID;
            public uint DwUser_Id;

            public uint FaceID;

            //512
            public string HeadExt;

            private readonly ushort HeadExtLength;
            public int HeadKey;
            public byte IsAccount;
            public byte IsBattle;
            public byte IsChangeAccount;

            public byte IsChangeHead;
            public byte IsChangeSexOrNickName;
            public byte IsChangeSign;
            public byte IsCustomHeadIcon;
            public byte IsFirstRecharge;
            public byte IsGetDuihuanReward;
            public byte IsGetToday;
            public int IsVIP;
            public uint LuckyCDTime;

            public uint LuckyDrawCount;

            //512
            public string NickName;

            private readonly ushort NickNameLength;

            public uint OnlineTime;

            //512
            public string Password;

            private readonly ushort PasswordLength;
            public short phoneNumberSize;

            public uint ReceiveTimeID;
            public byte ReconnectFloorID;
            public uint ReconnectGameID;
            public int RedpaacketOFF;
            public byte Sex;

            public byte ShareDays;

            //512
            public string Sign;

            private readonly ushort SignLength;

            //512
            public string SzPhoneNumber;

            public ACP_SC_LOGIN_SUCCESS()
            {
            }

            public ACP_SC_LOGIN_SUCCESS(ByteBuffer byteBuffer)
            {
                DwUser_Id = byteBuffer.ReadUInt32();
                BeautifulID = byteBuffer.ReadUInt32();
                Sex = byteBuffer.ReadByte();
                IsCustomHeadIcon = byteBuffer.ReadByte();
                AccountLength = byteBuffer.ReadUInt16();
                Account = byteBuffer.ReadString(AccountLength);
                NickNameLength = byteBuffer.ReadUInt16();
                NickName = byteBuffer.ReadString(NickNameLength);
                PasswordLength = byteBuffer.ReadUInt16();
                Password = byteBuffer.ReadString(PasswordLength);
                HeadExtLength = byteBuffer.ReadUInt16();
                HeadExt = byteBuffer.ReadString(HeadExtLength);
                SignLength = byteBuffer.ReadUInt16();
                Sign = byteBuffer.ReadString(SignLength);
                ReceiveTimeID = byteBuffer.ReadUInt32();
                OnlineTime = byteBuffer.ReadUInt32();
                LuckyDrawCount = byteBuffer.ReadUInt32();
                IsBattle = byteBuffer.ReadByte();
                IsGetDuihuanReward = byteBuffer.ReadByte();
                ShareDays = byteBuffer.ReadByte();
                IsGetToday = byteBuffer.ReadByte();
                IsFirstRecharge = byteBuffer.ReadByte();
                IsChangeSexOrNickName = byteBuffer.ReadByte();
                LuckyCDTime = byteBuffer.ReadUInt32();
                FaceID = byteBuffer.ReadUInt32();
                IsChangeHead = byteBuffer.ReadByte();
                IsChangeSign = byteBuffer.ReadByte();
                IsChangeAccount = byteBuffer.ReadByte();
                HeadKey = byteBuffer.ReadInt32();
                BBindInviteCode = byteBuffer.ReadByte();
                IsAccount = byteBuffer.ReadByte();
                IsVIP = byteBuffer.ReadInt32();
                RedpaacketOFF = byteBuffer.ReadInt32();
                phoneNumberSize = (short) byteBuffer.ReadUInt16();
                SzPhoneNumber = byteBuffer.ReadString(phoneNumberSize);
                ReconnectGameID = byteBuffer.ReadUInt32();
                ReconnectFloorID = byteBuffer.ReadByte();
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class ACP_SC_ROOM_INFO
        {
            public ACP_SC_ROOM_INFO(ByteBuffer buffer)
            {
                RoomLength = buffer.ReadUInt32();
                SubInfo = new List<RoomInfo>();
                for (int i = 0; i < RoomLength; i++)
                {
                    RoomInfo roomSubInfo = new RoomInfo();
                    roomSubInfo._1byFloorID = buffer.ReadByte();
                    roomSubInfo._2wGameID = buffer.ReadUInt16();
                    roomSubInfo._3wRoomID = buffer.ReadUInt16();
                    roomSubInfo._4dwServerAddr = NetworkHelper.Int2IP(buffer.ReadUInt32());
                    roomSubInfo._5wServerPort = buffer.ReadUInt16();
                    roomSubInfo._6iLessGold = buffer.ReadInt32();
                    roomSubInfo._7dwOnLineCount = buffer.ReadUInt32();
                    roomSubInfo._8NameLenght = buffer.ReadUInt16();
                    roomSubInfo._9Name = buffer.ReadString(roomSubInfo._8NameLenght);
                    SubInfo.Add(roomSubInfo);
                }
            }

            public uint RoomLength { get; set; }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public List<RoomInfo> SubInfo;
        }

        /// <summary>
        /// 房间列表
        /// </summary>
        public class RoomInfo
        {
            public byte _1byFloorID;
            public ushort _2wGameID;
            public ushort _3wRoomID;
            public string _4dwServerAddr;
            public ushort _5wServerPort;
            public int _6iLessGold;
            public uint _7dwOnLineCount;
            public ushort _8NameLenght;
            public string _9Name;
            public string _10BL;

            public RoomInfo()
            {
            }

            public RoomInfo(ByteBuffer byteBuffer)
            {
                _1byFloorID = byteBuffer.ReadByte();
                _2wGameID = byteBuffer.ReadUInt16();
                _3wRoomID = byteBuffer.ReadUInt16();
                _4dwServerAddr = NetworkHelper.Int2IP(byteBuffer.ReadUInt32());
                _5wServerPort = byteBuffer.ReadUInt16();
                _6iLessGold = byteBuffer.ReadInt32();
                _7dwOnLineCount = byteBuffer.ReadUInt32();
                _8NameLenght = byteBuffer.ReadUInt16();
                _9Name = byteBuffer.ReadString(_8NameLenght);
                _10BL = "1";
            }
        }


        /// <summary>
        ///     登录大厅失败
        /// </summary>
        public class ACP_SC_LOGIN_FAILE
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_LOGIN_FAILE(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadInt32();
                Error = byteBuffer.ReadString(1024 * 2);
            }
        }

        /// <summary>
        ///     注册返回
        /// </summary>
        public class ACP_SC_LOGIN_REGISTER
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_LOGIN_REGISTER(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadInt32();
                Error = byteBuffer.ReadString(1024 * 2);
            }
        }

        /// <summary>
        ///     找回密码
        /// </summary>
        public class ACP_SC_LOGIN_FINDPW
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_LOGIN_FINDPW(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadUInt16();
                Error = byteBuffer.ReadString(Length);
            }
        }

        /// <summary>
        ///     服务器验证码返回
        /// </summary>
        public class ACP_SC_CODE
        {
            public uint Code;

            public ACP_SC_CODE(ByteBuffer byteBuffer)
            {
                Code = byteBuffer.ReadUInt32();
            }
        }

        /// <summary>
        /// 查询玩家返回
        /// </summary>
        public class ACP_SC_QueryPlayer
        {
            public ACP_SC_QueryPlayer(ByteBuffer buffer)
            {
                isReal = buffer.ReadByte() == 1;
                num = buffer.ReadUInt32();
                num1 = buffer.ReadUInt32();
                nickNameLength = buffer.ReadUInt16();
                nickName = buffer.ReadString(nickNameLength);
                sex = buffer.ReadByte();
                isCustomHead = buffer.ReadByte() == 1;
                headExtensionLength = buffer.ReadUInt16();
                headExtension = buffer.ReadString(headExtensionLength);
                signLength = buffer.ReadUInt16();
                sign = buffer.ReadString(signLength);
                goldLength = buffer.ReadUInt16();
                goldNum = buffer.ReadString(goldLength);
                dwPrize = buffer.ReadUInt32();
                byFloorID = buffer.ReadByte();
                wGameID = buffer.ReadUInt16();
                wRoomID = buffer.ReadUInt16();
                wWindow = buffer.ReadByte();
            }

            public bool isReal;
            public uint num;
            public uint num1;
            private ushort nickNameLength;
            public string nickName;
            public byte sex;
            public bool isCustomHead;
            private ushort headExtensionLength;
            public string headExtension;
            private ushort signLength;
            public string sign;
            private ushort goldLength;
            public string goldNum;
            public uint dwPrize;
            public byte byFloorID;
            public ushort wGameID;
            public ushort wRoomID;
            public byte wWindow;
        }

        public class Acp_SC_USER_PROP
        {
            public uint User_Id;
            public uint dwProp_Id;
            public byte byPropType;
            public byte byHighLow;
            public byte bySex;
            public byte byExpireType;
            public long dwAmount;

            public uint dwRemainTime;

            //startTime
            public ushort wStartYear;
            public ushort wStartMonth;
            public ushort wStartDayOfWeek;
            public ushort wStartDay;
            public ushort wStartHour;
            public ushort wStartMinute;
            public ushort wStartSecond;

            public ushort wStartMilliseconds;

            //stopTime
            public ushort wStopYear;
            public ushort wStopMonth;
            public ushort wStopDayOfWeek;
            public ushort wStopDay;
            public ushort wStopHour;
            public ushort wStopMinute;
            public ushort wStopSecond;
            public ushort wStopMilliseconds;
            public byte tag;
            public long Bank_Gold;


            public Acp_SC_USER_PROP(ByteBuffer buffer)
            {
                User_Id = buffer.ReadUInt32();
                dwProp_Id = buffer.ReadUInt32();
                byPropType = buffer.ReadByte();
                byHighLow = buffer.ReadByte();
                bySex = buffer.ReadByte();
                byExpireType = buffer.ReadByte();
                dwAmount = buffer.ReadUInt64();
                dwRemainTime = buffer.ReadUInt32();
                wStartYear = buffer.ReadUInt16();
                wStartMonth = buffer.ReadUInt16();
                wStartDayOfWeek = buffer.ReadUInt16();
                wStartDay = buffer.ReadUInt16();
                wStartHour = buffer.ReadUInt16();
                wStartMinute = buffer.ReadUInt16();
                wStartSecond = buffer.ReadUInt16();
                wStartMilliseconds = buffer.ReadUInt16();
                wStopYear = buffer.ReadUInt16();
                wStopMonth = buffer.ReadUInt16();
                wStopDayOfWeek = buffer.ReadUInt16();
                wStopDay = buffer.ReadUInt16();
                wStopHour = buffer.ReadUInt16();
                wStopMinute = buffer.ReadUInt16();
                wStopSecond = buffer.ReadUInt16();
                wStopMilliseconds = buffer.ReadUInt16();
                tag = buffer.ReadByte();
            }
        }

        /// <summary>
        ///     查询玩家金币消息
        /// </summary>
        public class ACP_SC_SELECT_GOLD
        {
            public long Bank_Gold;
            public long Self_Gold;

            public uint User_Id;

            public ACP_SC_SELECT_GOLD()
            {
            }

            public ACP_SC_SELECT_GOLD(ByteBuffer buffer)
            {
                //cbResult
                buffer.ReadByte();
                //User_Id
                User_Id = buffer.ReadUInt32();
                //dwProp_Id
                buffer.ReadUInt32();
                //dwAmount
                Self_Gold = buffer.ReadUInt64();
                //dwRemainTime
                buffer.ReadUInt32();
                //startTime
                // wYear
                buffer.ReadUInt16();
                //wMonth
                buffer.ReadUInt16();
                //wDayOfWeek
                buffer.ReadUInt16();
                //wDay
                buffer.ReadUInt16();
                //wHour
                buffer.ReadUInt16();
                //wMinute
                buffer.ReadUInt16();
                //wSecond
                buffer.ReadUInt16();
                //wMilliseconds
                buffer.ReadUInt16();
                //stopTime
                // wYear
                buffer.ReadUInt16();
                //wYear
                buffer.ReadUInt16();
                //wMonth
                buffer.ReadUInt16();
                //wDayOfWeek
                buffer.ReadUInt16();
                //wDay
                buffer.ReadUInt16();
                //wHour
                buffer.ReadUInt16();
                //wMinute
                buffer.ReadUInt16();
                //wSecond
                buffer.ReadUInt16();
                //wMilliseconds
                buffer.ReadByte();
            }
        }

        /// <summary>
        /// 查询id返回
        /// </summary>
        public class ACP_SC_QueryID
        {
            public class QueryIDSubData
            {
                public QueryIDSubData(ByteBuffer buffer)
                {
                    mIndex = buffer.ReadInt();
                    mSenderId = buffer.ReadInt();
                    mRecverId = buffer.ReadInt();
                    mGold = buffer.ReadInt64();
                    mTime = buffer.ReadInt();
                }

                public int mIndex;
                public int mSenderId;
                public int mRecverId;
                public long mGold;
                public int mTime;
            }

            public ACP_SC_QueryID(ByteBuffer buffer)
            {
                cbRes = buffer.ReadByte() != 0;
                cbCount = buffer.ReadByte();
                subDatas = new List<QueryIDSubData>();
                for (int i = 0; i < cbCount; i++)
                {
                    QueryIDSubData subData = new QueryIDSubData(buffer);
                    subDatas.Add(subData);
                }

                m_balance = buffer.ReadInt64();
            }

            public bool cbRes;
            public byte cbCount;
            public List<QueryIDSubData> subDatas;
            public long m_balance;
        }

        /// <summary>
        ///     签名
        /// </summary>
        public class ACP_SC_CHANGE_SIGN
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_CHANGE_SIGN(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadUInt16();
                Error = byteBuffer.ReadString(Length);
            }
        }

        /// <summary>
        ///     昵称
        /// </summary>
        public class ACP_SC_UpdataNickName
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_UpdataNickName(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadUInt16();
                Error = byteBuffer.ReadString(Length);
            }
        }

        /// <summary>
        ///     点卡查询
        /// </summary>
        public class ACP_SC_DIANKA_QUERY
        {
            public string Card;
            public int Count;

            public int Gold;

            public int ID;

            public long Timer;

            public ACP_SC_DIANKA_QUERY(ByteBuffer byteBuffer)
            {
                Count = byteBuffer.ReadInt32();
                Timer = byteBuffer.ReadInt64();
                ID = byteBuffer.ReadInt32();
                Gold = byteBuffer.ReadInt32();
                Card = byteBuffer.ReadString(40);
            }
        }

        /// <summary>
        ///     点卡领取
        /// </summary>
        public class ACP_SC_DIANKA_RECEIVE
        {
            public string Card;

            public int Gold;

            public string Msg;

            public ACP_SC_DIANKA_RECEIVE(ByteBuffer byteBuffer)
            {
                Gold = byteBuffer.ReadInt32();
                Card = byteBuffer.ReadString(40);
                Msg = byteBuffer.ReadString(100);
            }
        }

        /// <summary>
        ///     点卡赠送
        /// </summary>
        public class ACP_SC_DIANKA_GIVE
        {
            public string Card;

            public int Gold;

            public string Msg;

            public ACP_SC_DIANKA_GIVE(ByteBuffer byteBuffer)
            {
                Gold = byteBuffer.ReadInt32();
                Card = byteBuffer.ReadString(40);
                Msg = byteBuffer.ReadString(100);
            }
        }

        /// <summary>
        ///     银行存取
        /// </summary>
        public class ACP_SC_SaveOrGetGold
        {
            public byte cbDrawOut;
            public byte cbSuccess;
            public string szInfoDiscrib;
            public long szInsureScore;

            public ACP_SC_SaveOrGetGold(ByteBuffer byteBuffer)
            {
                szInsureScore = byteBuffer.ReadInt64();
                cbSuccess = byteBuffer.ReadByte();
                cbDrawOut = byteBuffer.ReadByte();
                szInfoDiscrib = byteBuffer.ReadString(128);
            }
        }


        /// <summary>
        ///     银行修改密码
        /// </summary>
        public class ACP_SC_Bank_Change_PW
        {
            public int cbSuccess;
            public string szInfoDiscrib;

            public ACP_SC_Bank_Change_PW(ByteBuffer byteBuffer)
            {
                cbSuccess = byteBuffer.ReadInt32();
                szInfoDiscrib = byteBuffer.ReadString(128);
            }
        }

        /// <summary>
        ///     转账记录
        /// </summary>
        public class ACP_SC_Bank_Annal
        {
            public int id; // 礼物唯一索引UINT32
            public int isgive; // 是否已领UINT32
            public string num; // 道具个数UINT32
            public int rid; // 接收者ID
            public string rNick; //接收则昵称
            public int sid; // 发送者ID
            public string sNick; //发送者昵称
            public int time; // 赠送时间UINT32
            public int type; // 道具idUINT32

            public ACP_SC_Bank_Annal(ByteBuffer byteBuffer)
            {
                id = byteBuffer.ReadInt32();
                sid = byteBuffer.ReadInt32();
                rid = byteBuffer.ReadInt32();
                type = byteBuffer.ReadInt32();
                num = byteBuffer.ReadInt64Str();
                time = byteBuffer.ReadInt32();
                isgive = byteBuffer.ReadInt32();
                sNick = byteBuffer.ReadString();
                rNick = byteBuffer.ReadString();
            }
        }


        /// <summary>
        ///     绑定界面验证码返回
        /// </summary>
        public class ACP_SC_BindGetCodeCallBack
        {
            public uint index; // 礼物唯一索引UINT32

            public ACP_SC_BindGetCodeCallBack(ByteBuffer byteBuffer)
            {
                index = byteBuffer.ReadUInt32();
            }
        }

        /// <summary>
        ///     修改账号
        /// </summary>
        public class ACP_SC_CHANGE_ACCOUNT
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_CHANGE_ACCOUNT(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadUInt16();
                Error = byteBuffer.ReadString(Length);
            }
        }

        /// <summary>
        ///     修改密码
        /// </summary>
        public class ACP_SC_CHANGE_PASSWOR
        {
            //1024
            public string Error;
            public int Length;

            public ACP_SC_CHANGE_PASSWOR(ByteBuffer byteBuffer)
            {
                Length = byteBuffer.ReadUInt16();
                Error = byteBuffer.ReadString(Length);
            }
        }

        /// <summary>
        /// 登录验证查询返回
        /// </summary>
        public class ACP_SC_QueryLoginVerify
        {
            public ACP_SC_QueryLoginVerify()
            {
            }

            public ACP_SC_QueryLoginVerify(ByteBuffer buffer)
            {
                state = buffer.ReadByte();
                type = buffer.ReadByte();
                isOn = buffer.ReadByte() != 0;
            }

            public byte state;
            public byte type;
            public bool isOn;
        }
        /// <summary>
        /// 刷新银行
        /// </summary>
        public class ACP_SC_UPDATEBANKERSAVEGOLD
        {
            
            public long gold;

            public ACP_SC_UPDATEBANKERSAVEGOLD(ByteBuffer buffer)
            {
                gold = buffer.ReadUInt64();
            }

            public ACP_SC_UPDATEBANKERSAVEGOLD()
            {
                
            }
        }
        /// <summary>
        /// 撤回
        /// </summary>
        public class ACP_SC_WITHDRAW
        {
            public ACP_SC_WITHDRAW()
            {
                
            }
            public ACP_SC_WITHDRAW(ByteBuffer buffer)
            {
                recallResult = buffer.ReadByte() == 1;
                recallGold = buffer.ReadUInt64();
                recallUserID = buffer.ReadUInt32();
                recallMsg = buffer.ReadString(100);
                recallMsg = recallMsg.Trim();
            }

            public bool recallResult;
            public long recallGold;
            public uint recallUserID;
            public string recallMsg;
        }

        /// <summary>
        /// 初始化银行
        /// </summary>
        public class InitBank
        {
            public InitBank(ByteBuffer buffer)
            {
                count = buffer.ReadByte();
                bankGold = buffer.ReadInt64();
            }

            public byte count;
            public long bankGold;
        }

        #endregion

        #region REQ

        /// <summary>
        ///     登录
        /// </summary>
        public class REQ_CS_LOGIN
        {
            //256
            public string headUrl;

            //16
            public string nickName;

            //32
            public string account;
            public byte addID;
            public uint addMultiplyID;
            private ByteBuffer buffer;
            public byte channelID;

            public ushort iD;

            //16
            public string ip;

            //50
            public string mechinaCode;

            public ushort multiplyID;

            //33
            public string password;
            public byte platform;

            public REQ_CS_LOGIN()
            {
            }

            public REQ_CS_LOGIN(byte Platform, byte ChannelID, ushort ID, uint AddMultiplyID, ushort MultiplyID,
                byte AddID, string Account, string Password, string MechinaCode, string IP, string HeadURL,
                string NickName)
            {
                platform = Platform;
                channelID = ChannelID;
                iD = ID;
                addMultiplyID = AddMultiplyID;
                multiplyID = MultiplyID;
                addID = AddID;
                account = Account;
                password = Password;
                mechinaCode = MechinaCode;
                ip = IP;
                headUrl = HeadURL;
                nickName = NickName;
            }

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteByte(platform);
                    buffer.WriteByte(channelID);
                    buffer.WriteUInt16(iD);
                    buffer.WriteUInt32(addMultiplyID);
                    buffer.WriteUInt16(multiplyID);
                    buffer.WriteByte(addID);
                    buffer.WriteBytes(32, account);
                    buffer.WriteBytes(33, password);
                    buffer.WriteBytes(50, mechinaCode);
                    buffer.WriteBytes(16, ip);
                    buffer.WriteBytes(256, headUrl);
                    buffer.WriteBytes(16, nickName);
                    return buffer;
                }
            }
        }

        /// <summary>
        ///     心跳
        /// </summary>
        public class REQHeartMessage
        {
            private ByteBuffer buffer;

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    return buffer;
                }
            }
        }

        /// <summary>
        ///     注册验证码请求
        /// </summary>
        public class REQ_CS_CODE_RegisterS
        {
            private ByteBuffer buffer;

            private readonly byte iD;

            //12
            private readonly string phoneNumber;
            private readonly byte platformID;
            private readonly ushort temp;

            public REQ_CS_CODE_RegisterS()
            {
            }

            public REQ_CS_CODE_RegisterS(byte PlatformID, byte ID, ushort Temp, string PhoneNumber)
            {
                platformID = PlatformID;
                iD = ID;
                temp = Temp;
                phoneNumber = PhoneNumber;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteByte(platformID);
                    buffer.WriteByte(iD);
                    buffer.WriteUInt16(temp);
                    buffer.WriteBytes(12, phoneNumber);
                    return buffer;
                }
            }
        }

        /// <summary>
        ///     注册
        /// </summary>
        public class REQ_CS_Register
        {
            //32
            public string account;
            public byte addID;
            public uint addMultiplyID;
            public ByteBuffer buffer;
            public byte channelID;

            public ushort iD;

            //16
            public string iP;

            //50
            public string mechinaCode;

            public ushort multiplyID;

            //33
            public string password;

            public uint phoneCode;

            //12
            public string phoneNum;
            public byte platform;

            public REQ_CS_Register()
            {
            }

            public REQ_CS_Register(byte Platform, byte ChannelID, ushort ID, uint AddMultiplyID, ushort MultiplyID,
                byte AddID, string Account, string Password, string MechinaCode, string PhoneNum, uint PhoneCode,
                string IP)
            {
                platform = Platform;
                channelID = ChannelID;
                iD = ID;
                addMultiplyID = AddMultiplyID;
                multiplyID = MultiplyID;
                addID = AddID;
                account = Account;
                password = Password;
                mechinaCode = MechinaCode;
                phoneNum = PhoneNum;
                phoneCode = PhoneCode;
                iP = IP;
            }

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteByte(platform);
                    buffer.WriteByte(channelID);
                    buffer.WriteUInt16(iD);
                    buffer.WriteUInt32(addMultiplyID);
                    buffer.WriteUInt16(multiplyID);
                    buffer.WriteByte(addID);
                    buffer.WriteBytes(32, account);
                    buffer.WriteBytes(33, password);
                    buffer.WriteBytes(50, mechinaCode);
                    buffer.WriteBytes(12, phoneNum);
                    buffer.WriteUInt32(phoneCode);
                    buffer.WriteBytes(16, iP);
                    return buffer;
                }
            }
        }

        /// <summary>
        ///     找回密码验证码
        /// </summary>
        public class REQ_CS_FindPasswordCode
        {
            private ByteBuffer buffer;

            //12
            private readonly string phoneNumber;

            public REQ_CS_FindPasswordCode()
            {
            }

            public REQ_CS_FindPasswordCode(string phoneNumber)
            {
                this.phoneNumber = phoneNumber;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(12, phoneNumber);
                    return buffer;
                }
            }
        }

        /// <summary>
        ///     找回密码
        /// </summary>
        public class REQ_CS_FindPassword
        {
            private ByteBuffer buffer;

            public uint code;

            //33
            public string password;

            //12
            public string phoneNumber;
            public uint platform;

            public REQ_CS_FindPassword()
            {
            }

            public REQ_CS_FindPassword(string PhoneNumber, string Password, uint Code, uint Platform)
            {
                phoneNumber = PhoneNumber;
                password = Password;
                code = Code;
                platform = Platform;
            }

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(12, phoneNumber);
                    buffer.WriteBytes(33, password);
                    buffer.WriteUInt32(code);
                    buffer.WriteUInt32(platform);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 存取金币
        /// </summary>
        public class REQ_CS_SAVEORGETGOLD
        {
            private ByteBuffer buffer;

            //12
            public long gold;
            public uint id;

            public uint mark;

            //33
            public string pwd;

            public REQ_CS_SAVEORGETGOLD()
            {
            }

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteUInt32(id);
                    buffer.WriteUInt32(mark);
                    buffer.WriteLong(gold);
                    buffer.WriteBytes(33, pwd);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 转账
        /// </summary>
        public class REQ_CS_TRANSFERACCOUNTS
        {
            private ByteBuffer buffer;
            private readonly long gold;
            private readonly uint id;

            public REQ_CS_TRANSFERACCOUNTS()
            {
            }

            public REQ_CS_TRANSFERACCOUNTS(uint ID, long Gold)
            {
                gold = Gold;
                id = ID;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteUInt32(id);
                    buffer.WriteLong(gold);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 查询玩家
        /// </summary>
        public class REQ_CS_QueryPlayer
        {
            public REQ_CS_QueryPlayer()
            {
            }

            public uint id;
            private ByteBuffer buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteUInt32(id);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 打开银行
        /// </summary>
        public class REQ_CS_OpenBank
        {
            private ByteBuffer buffer;
            private readonly uint id;

            public REQ_CS_OpenBank()
            {
            }

            public REQ_CS_OpenBank(uint ID)
            {
                id = ID;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteUInt32(id);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        public class REQ_CS_ModifyInfo
        {
            private ByteBuffer buffer;
            private readonly string sign;

            public REQ_CS_ModifyInfo()
            {
            }

            public REQ_CS_ModifyInfo(string Sign)
            {
                sign = Sign;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(50, sign);
                    return buffer;
                }
            }
        }


        /// <summary>
        /// 修改昵称
        /// </summary>
        public class REQ_CS_ModifyNiKeName
        {
            private ByteBuffer buffer;
            private readonly string nickName;
            private readonly byte sex;

            public REQ_CS_ModifyNiKeName()
            {
            }

            public REQ_CS_ModifyNiKeName(string NickName, byte Sex)
            {
                nickName = NickName;
                sex = Sex;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(32, nickName);
                    buffer.WriteByte(sex);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 点卡赠送
        /// </summary>
        public class REQ_CS_DIANKA_GIVE
        {
            private ByteBuffer buffer;
            private readonly int count;
            private readonly int id;
            private readonly int user_id;

            public REQ_CS_DIANKA_GIVE()
            {
            }

            public REQ_CS_DIANKA_GIVE(int ID, int User_Id, int Count)
            {
                id = ID;
                user_id = User_Id;
                count = Count;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteInt(id);
                    buffer.WriteInt(user_id);
                    buffer.WriteInt(count);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 银行查询
        /// </summary>
        public class REQ_CS_Bank_Query
        {
            private ByteBuffer buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteUInt32(dwUser_Id);
                    return buffer;
                }
            }

            public uint dwUser_Id;

            public REQ_CS_Bank_Query()
            {
            }

            public REQ_CS_Bank_Query(uint _dwUser_Id)
            {
                dwUser_Id = _dwUser_Id;
            }
        }

        /// <summary>
        /// 初始化银行 打开银行传递密码
        /// </summary>
        public class REQ_CS_Bank_Init
        {
            private ByteBuffer buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(33, password);
                    return buffer;
                }
            }

            public string password;

            public REQ_CS_Bank_Init()
            {
            }

            public REQ_CS_Bank_Init(string _password)
            {
                password = _password;
            }
        }

        /// <summary>
        ///     银行修改密码验证码
        /// </summary>
        public class REQ_CS_Bank_Code
        {
            private ByteBuffer buffer;
            public string phoneNum;

            public REQ_CS_Bank_Code()
            {
            }

            public REQ_CS_Bank_Code(string PhoneNum)
            {
                phoneNum = PhoneNum;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(12, phoneNum);
                    return buffer;
                }
            }
        }


        /// <summary>
        ///     银行修改密码
        /// </summary>
        public class REQ_CS_Bank_Reset_Password
        {
            private ByteBuffer buffer;
            private readonly int code;
            private readonly string phoneNum;
            private readonly byte platformID;
            private readonly string pw;

            public REQ_CS_Bank_Reset_Password()
            {
            }

            public REQ_CS_Bank_Reset_Password(string PhoneNum, string PW, int Code, byte PlatformID)
            {
                phoneNum = PhoneNum;
                pw = PW;
                code = Code;
                platformID = PlatformID;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(12, phoneNum);
                    buffer.WriteBytes(33, pw);
                    buffer.WriteInt(code);
                    buffer.WriteByte(platformID);
                    return buffer;
                }
            }
        }


        /// <summary>
        ///     银行修改密码
        /// </summary>
        public class REQ_CS_GiveAndSend
        {
            private ByteBuffer buffer;
            private readonly int id;
            private readonly int loadType;
            private readonly int starPoint;

            public REQ_CS_GiveAndSend()
            {
            }

            public REQ_CS_GiveAndSend(int StarPoint, int ID, int LoadType)
            {
                starPoint = StarPoint;
                id = ID;
                loadType = LoadType;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteInt(starPoint);
                    buffer.WriteInt(id);
                    buffer.WriteInt(loadType);
                    return buffer;
                }
            }
        }


        /// <summary>
        ///     绑定验证码
        /// </summary>
        public class REQ_CS_BindGetCode
        {
            private ByteBuffer buffer;
            private readonly string num1;
            private readonly string num2;
            private readonly string phone;

            public REQ_CS_BindGetCode()
            {
            }

            public REQ_CS_BindGetCode(string Num1, string Num2, string Phone)
            {
                num1 = Num1;
                num2 = Num2;
                phone = Phone;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(1, num1);
                    buffer.WriteBytes(1, num2);
                    buffer.WriteBytes(12, phone);
                    return buffer;
                }
            }
        }


        /// <summary>
        ///     绑定账号
        /// </summary>
        public class REQ_CS_ChangeAccount
        {
            private ByteBuffer buffer;
            private readonly int code;
            private readonly string phone;
            private readonly string pw;

            public REQ_CS_ChangeAccount()
            {
            }

            public REQ_CS_ChangeAccount(string Phone, int Code, string PW)
            {
                phone = Phone;
                code = Code;
                pw = PW;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(12, phone);
                    buffer.WriteInt(code);
                    buffer.WriteBytes(33, pw);
                    return buffer;
                }
            }
        }


        /// <summary>
        ///     修改密码
        /// </summary>
        public class REQ_CS_ChangePassword
        {
            private ByteBuffer buffer;
            private readonly string newPW;
            private readonly string pw;

            public REQ_CS_ChangePassword()
            {
            }

            public REQ_CS_ChangePassword(string PW, string NewPW)
            {
                pw = PW;
                newPW = NewPW;
            }

            public ByteBuffer _ByteBuffer
            {
                get
                {
                    buffer?.Close();
                    buffer = new ByteBuffer();
                    buffer.WriteBytes(33, pw);
                    buffer.WriteBytes(33, newPW);
                    return buffer;
                }
            }
        }

        public class LoginCodeAccredit
        {
            public LoginCodeAccredit()
            {
            }

            public string account;
            public uint code;
            private ByteBuffer _buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    _buffer?.Close();
                    _buffer = new ByteBuffer();
                    _buffer.WriteBytes(32, account);
                    _buffer.WriteUInt32(code);
                    return _buffer;
                }
            }
        }

        public class REQSyncGold
        {
            public int chairId;
            private ByteBuffer _buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    _buffer?.Close();
                    _buffer = new ByteBuffer();
                    _buffer.WriteInt(chairId);
                    return _buffer;
                }
            }
        }

        /// <summary>
        /// 查询登录验证是否开启或者修改登录验证
        /// </summary>
        public class REQQueryLoginVerify
        {
            public REQQueryLoginVerify()
            {
            }

            /// <summary>
            /// 是否为查询 0查询 1修改
            /// </summary>
            public bool isChange;

            public bool isSetOn;
            private ByteBuffer _buffer;

            public ByteBuffer ByteBuffer
            {
                get
                {
                    _buffer?.Close();
                    _buffer = new ByteBuffer();
                    _buffer.WriteByte(isChange ? 1 : 0);
                    _buffer.WriteByte(isSetOn ? 1 : 0);
                    return _buffer;
                }
            }
        }

        #endregion
    }
}