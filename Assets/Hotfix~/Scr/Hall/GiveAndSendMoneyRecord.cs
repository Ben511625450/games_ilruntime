using System.Collections.Generic;
using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class GiveAndSendMoneyRecord : PanelBase
    {
        private readonly List<HallStruct.ACP_SC_Bank_Annal> annalList = new List<HallStruct.ACP_SC_Bank_Annal>();

        private Button CloseBtn;
        private ScrollRect giveScorll;


        private bool isReqData;

        private Transform LeftPanel;
        private int loadtype; //0转出--1转入
        private Transform mainPanel;
        private Transform recordItem;

        private Transform RightPanel;
        private ScrollRect sendScorll;
        private int starPoint;
        private Transform titlehandId1;
        private Transform titlehandId2;
        private Button turnInBtn;
        private Button turnOutBtn;


        public GiveAndSendMoneyRecord() : base(UIType.Middle, nameof(GiveAndSendMoneyRecord))
        {

        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            starPoint = 0;
            loadtype = 0;
            setGiveShow(false);
            ReqData();
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HallEvent.SC_Give_Record_List += AllRecord;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HallEvent.SC_Give_Record_List -= AllRecord;
        }

        protected override void FindComponent()
        {
            mainPanel = transform.FindChildDepth("mainPanel");

            LeftPanel = mainPanel.FindChildDepth("LeftPanel");
            turnOutBtn = LeftPanel.FindChildDepth<Button>("turnOutBtn");
            turnInBtn = LeftPanel.FindChildDepth<Button>("turnInBtn");

            RightPanel = mainPanel.FindChildDepth("RightPanel");
            giveScorll = RightPanel.FindChildDepth<ScrollRect>("ScrollViewsend");
            sendScorll = RightPanel.FindChildDepth<ScrollRect>("ScrollViewsendgive");
            titlehandId1 = RightPanel.FindChildDepth("titlehand/id1");
            titlehandId2 = RightPanel.FindChildDepth("titlehand/id2");
            recordItem = transform.FindChildDepth("recorditem");

            CloseBtn = mainPanel.FindChildDepth<Button>("CloseBtn");
        }

        protected override void AddListener()
        {
            CloseBtn.onClick.RemoveAllListeners();
            CloseBtn.onClick.Add(ClosepGiveAndSendMoneyRecord);

            turnOutBtn.onClick.RemoveAllListeners();
            turnOutBtn.onClick.Add(turnOutBtnOnClick);

            turnInBtn.onClick.RemoveAllListeners();
            turnInBtn.onClick.Add(turnInBtnOnClick);
        }

        private void ReqData()
        {
            if (isReqData) return;

            isReqData = true;

            var giveAndSend = new HallStruct.REQ_CS_GiveAndSend(starPoint, 49, loadtype);

            HotfixGameComponent.Instance.Send(DataStruct.GoldMineStruct.MDM_3D_GOLDMINE,
                DataStruct.GoldMineStruct.SUB_2D_CS_GIVE_RECORD_LIST, giveAndSend._ByteBuffer, SocketType.Hall);

            starPoint += 49;
        }

        private void AllRecord(ByteBuffer buffer)
        {
            var length = buffer.ReadInt32();
            if (length <= 0)
            {
                DebugHelper.Log("没有赠送记录");
                return;
            }

            var isInit = false;
            var isSendReadPacket = false;
            for (var i = 0; i < length; i++)
            {
                var annal = new HallStruct.ACP_SC_Bank_Annal(buffer);
                if (isInit == false)
                {
                    if (annal.rid == GameLocalMode.Instance.SCPlayerInfo.BeautifulID)
                    {
                        StartRecord();
                        isSendReadPacket = true;
                    }
                    else
                    {
                        StartRecord();
                        isSendReadPacket = false;
                    }

                    isInit = true;
                }

                if (annal.sid == GameLocalMode.Instance.SCPlayerInfo.BeautifulID)
                {
                    Recording(annal);
                }
                else
                {
                    annal.rid = annal.sid;
                    Recording(annal);
                }
            }

            DebugHelper.Log($"送礼记录返回,共:{length}条记录");
            if (isSendReadPacket)
                EndRecord();
            else
                EndRecord();
        }

        private void StartRecord()
        {
            annalList.Clear();
            DebugHelper.Log("==============开始接收列表================");
        }

        private void Recording(HallStruct.ACP_SC_Bank_Annal annal)
        {
            annalList.Add(annal);
        }

        private void EndRecord()
        {
            DebugHelper.Log("==============接收完成================");

            if (annalList.Count <= 0) return;

            annalList.OrderBy((a, b) => a.time > b.time);

            DebugHelper.Log("==============创建列表================");

            if (loadtype == 1)
                CreatGiveItem(annalList.Count);
            else
                CreatGetItem(annalList.Count);
        }

        private void CreatGiveItem(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Transform child;
                if (i < giveScorll.content.childCount)
                {
                    child = giveScorll.content.GetChild(i);
                }
                else
                {
                    child = Object.Instantiate(recordItem);
                    child.SetParent(giveScorll.content);
                    child.localPosition = Vector3.zero;
                    child.localScale = Vector3.one;
                }

                child.gameObject.SetActive(true);
                var str = ((long)annalList[i].time).StampToDatetime();
                child.FindChildDepth<Text>("name").text = annalList[i].rNick;
                child.FindChildDepth<Text>("id").text = annalList[i].rid.ToString();
                child.FindChildDepth<Text>("gold").text = annalList[i].num;
                child.FindChildDepth<Text>("Group/time").text = $"{str:yyyy-MM-dd HH:mm}";
                child.FindChildDepth<Text>("Group/time").resizeTextForBestFit = true;
                var myindex = annalList[i].id;
                if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
                {
                    var recallBtn = child.FindChildDepth<Button>("Group/recall");
                    recallBtn.gameObject.SetActive(true);
                    recallBtn.onClick.RemoveAllListeners();
                    recallBtn.onClick.Add(() =>
                    {
                        DebugHelper.Log("撤回" + myindex);
                        var buffer = new ByteBuffer();
                        buffer.WriteInt(myindex);
                        HotfixGameComponent.Instance.Send(DataStruct.GoldMineStruct.MDM_3D_GOLDMINE,
                            DataStruct.GoldMineStruct.SUB_3D_CS_WITHDRAW, buffer, SocketType.Hall);
                    });
                }
                else
                {
                    child.FindChildDepth("Group/recall").gameObject.SetActive(false);
                }
            }
        }

        private void CreatGetItem(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Transform child;
                if (i < sendScorll.content.childCount)
                {
                    child = sendScorll.content.GetChild(i);
                }
                else
                {
                    child = Object.Instantiate(recordItem);
                    child.SetParent(sendScorll.content);
                    child.localPosition = Vector3.zero;
                    child.localScale = Vector3.one;
                }

                child.gameObject.SetActive(true);
                var str = ((long)annalList[i].time).StampToDatetime();
                child.FindChildDepth<Text>("name").text = annalList[i].rNick;
                child.FindChildDepth<Text>("id").text = annalList[i].rid.ToString();
                child.FindChildDepth<Text>("gold").text = annalList[i].num;
                child.FindChildDepth<Text>("Group/time").text = $"{str:yyyy-MM-dd HH:mm}";
                child.FindChildDepth<Text>("Group/time").resizeTextForBestFit = true;
                var myindex = annalList[i].id;
                if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
                {
                    var recallBtn = child.FindChildDepth<Button>("Group/recall");
                    recallBtn.gameObject.SetActive(true);
                    recallBtn.onClick.RemoveAllListeners();
                    recallBtn.onClick.Add(() =>
                    {
                        DebugHelper.Log($"撤回{myindex}");
                        var buffer = new ByteBuffer();
                        buffer.WriteInt(myindex);
                        HotfixGameComponent.Instance.Send(DataStruct.GoldMineStruct.MDM_3D_GOLDMINE,
                            DataStruct.GoldMineStruct.SUB_3D_CS_WITHDRAW, buffer, SocketType.Hall);
                    });
                }
                else
                {
                    child.FindChildDepth("Group/recall").gameObject.SetActive(false);
                }
            }

            loadtype = 1;
        }

        private void turnInBtnOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();
            setGiveShow(true);
        }

        private void turnOutBtnOnClick()
        {
            ILMusicManager.Instance.PlayBtnSound();
            setGiveShow(false);
        }

        private void setGiveShow(bool isShow)
        {
            sendScorll.gameObject.SetActive(!isShow);
            giveScorll.gameObject.SetActive(isShow);
            titlehandId1.gameObject.SetActive(isShow);
            titlehandId2.gameObject.SetActive(!isShow);
            //turnOutBtn.transform.FindChildDepth("Light").gameObject.SetActive(!isShow);
            //turnInBtn.transform.FindChildDepth("Light").gameObject.SetActive(isShow);
            turnOutBtn.interactable = isShow;
            turnInBtn.interactable = !isShow;
        }

        private void ClosepGiveAndSendMoneyRecord()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.Close();
        }
    }
}