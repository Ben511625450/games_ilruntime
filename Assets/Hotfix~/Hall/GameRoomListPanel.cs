using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;


namespace Hotfix.Hall
{
    public class GameRoomListPanel : PanelBase
    {
        Button closeBtn;
        Transform mainPanel;
        Text roomName;
        Transform mainInfo;
        private Dictionary<string, Transform> _contentInfo;
        Transform RoomNameImg;

        List<HallStruct.RoomInfo> gameRoom;
        string ChooseGame;
        Transform gameRoomListPanel;
        string SvaegameName;
        GameData roomConfig;

        int gameid;
        private Text goldNum;

        public GameRoomListPanel() : base(UIType.Middle, nameof(GameRoomListPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            gameRoom = new List<HallStruct.RoomInfo>();
            if (args.Length <= 0)
            {
                DebugHelper.LogError($"没有游戏ID");
                return;
            }

            Open((int) args[0], (List<HallStruct.RoomInfo>) args[1]);
            ShowPanel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HallEvent.DispatchEnterGamePre(false);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.ChangeGoldTicket += HallEventOnChangeGoldTicket;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.ChangeGoldTicket -= HallEventOnChangeGoldTicket;
        }

        protected override void FindComponent()
        {
            closeBtn = this.transform.FindChildDepth<Button>("CloseBtn");
            mainPanel = this.transform.FindChildDepth("mainPanel");
            roomName = mainPanel.FindChildDepth<Text>("Head/RoomName");
            _contentInfo = new Dictionary<string, Transform>();
            foreach (var listKey in HallGameShow.IconList.Keys)
            {
                Transform info = mainPanel.FindChildDepth(listKey);
                if (info == null) continue;
                _contentInfo.Add(listKey, info);
                info.gameObject.SetActive(false);
            }
            RoomNameImg = this.transform.FindChildDepth("RoomNameImg");
            goldNum = transform.FindChildDepth<Text>($"Gold/Num");
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(() =>
            {
                ILMusicManager.Instance.PlayBtnSound();
                UIManager.Instance.Close();
            });
        }

        private void Open(int id, List<HallStruct.RoomInfo> roomInfos)
        {
            roomConfig = GameConfig.GetGameData(id);
            string name = roomConfig.uiName;
            DebugHelper.Log($"打开{name}房间列表");
            gameRoom = roomInfos;
            gameid = id;
            ChooseGame = name;
            goldNum.text = $"{GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD)}";
            DebugHelper.Log($"{JsonMapper.ToJson(gameRoom)}");
            if (!_contentInfo.ContainsKey(GameLocalMode.Instance.CurrentSelectPlatform)) return;
            mainInfo = _contentInfo[GameLocalMode.Instance.CurrentSelectPlatform];
        }

        private void HallEventOnChangeGoldTicket()
        {
            goldNum.text = $"{GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD)}";
        }
        private void ShowPanel()
        {
            DebugHelper.Log("____GameRoomList.ShowPanel______" + ChooseGame);
            SvaegameName = ChooseGame;
            DebugHelper.Log(roomConfig.scenName);
            DebugHelper.Log(roomConfig.configer);
            roomName.text = roomConfig.configer.gameName; // RoomNameImg.FindChildDepth().GetComponent<Image>().sprite;
            roomName.SetNativeSize();
            Transform content = mainInfo.FindChildDepth($"Content");
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }

            GameData data = GameConfig.GetGameData(gameid);
            for (int i = 0; i < gameRoom.Count; i++)
            {
                int index = i;
                Transform child = content.GetChild(gameRoom[i]._3wRoomID - 2);
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                child.GetComponent<Button>().onClick.Add(() => { StartRoomOnClick(gameRoom[index]); });
                Text tj = child.FindChildDepth<Text>("TJ");
                Text bl = child.FindChildDepth<Text>("BL");
                int gold = gameRoom[i]._6iLessGold;
                if (gold > 100000000)
                {
                    tj.text = $"入场：{gold / 100000000}亿";
                }
                else if (gold > 10000)
                {
                    tj.text = $"入场：{gold / 10000}万";
                }
                else
                {
                    tj.text = $"入场：{gold}";
                }

                string blDesc = data.BL[gameRoom[i]._3wRoomID - 2];
                bl.text = $"倍率:{blDesc}";
                child.gameObject.SetActive(true);
            }
            mainInfo.gameObject.SetActive(true);
        }

        private void StartRoomOnClick(HallStruct.RoomInfo info)
        {
            long gold = 0;
            if (GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID == 0 ||
                GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID == 0)
            {
                gold = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD);
                if (gold < info._6iLessGold)
                {
                    ToolHelper.PopSmallWindow($"你的金币不足，请充值");
                    return;
                }

                UIManager.Instance.OpenUI<DownLoadGamePanel>(info);
                return;
            }

            HallStruct.RoomInfo roomInfo = GameLocalMode.Instance.AllSCGameRoom.FindItem(p =>
                p._2wGameID == GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID &&
                p._1byFloorID == GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID);
            if (roomInfo == null)
            {
                gold = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD);
                if (gold < info._6iLessGold)
                {
                    ToolHelper.PopSmallWindow($"你的金币不足，请充值");
                    return;
                }

                ToolHelper.PopSmallWindow($"重连游戏未开放");
                return;
            }

            gold = GameLocalMode.Instance.GetProp(Prop_Id.E_PROP_GOLD);
            if (gold < info._6iLessGold)
            {
                ToolHelper.PopSmallWindow($"你的金币不足，请充值");
                return;
            }

            if (info._2wGameID == roomInfo._2wGameID && info._3wRoomID == roomInfo._3wRoomID)
            {
                UIManager.Instance.OpenUI<DownLoadGamePanel>(info);
            }
            else
            {
                ToolHelper.PopBigWindow(new BigMessage()
                {
                    content = $"您还有游戏未结束!",
                    okCall=()=>
                    {
                        UIManager.Instance.OpenUI<DownLoadGamePanel>(roomInfo);
                    }
                });
            }
        }
    }
}