using LuaFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Hotfix.Hall;

namespace Hotfix
{
    public class PopComponent : SingletonILEntity<PopComponent>
    {
        private Queue<BigMessage> NeedPopBigList = new Queue<BigMessage>();
        private Queue<string> NeedPopSmallList = new Queue<string>();
        GameObject bigObj;
        GameObject smallObj;
        public bool isShowBig = false;
        bool isShowSmall = false;
        protected override void Awake()
        {
            base.Awake();
            NeedPopBigList = new Queue<BigMessage>();
            NeedPopSmallList = new Queue<string>();
            Reset();
        }
        protected override void AddEvent()
        {
            base.AddEvent();
            EventComponent.Instance.AddListener(HallEvent.OnClosePopBig, HallEvent_OnClosePopBig);
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            EventComponent.Instance.RemoveListener(HallEvent.OnClosePopBig, HallEvent_OnClosePopBig);
        }
        private void HallEvent_OnClosePopBig(params object[] args)
        {
            isShowBig = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Reset();
        }
        protected override void Update()
        {
            base.Update();
            if (NeedPopBigList.Count > 0)
            {
                PopBig();
            }
            if (NeedPopSmallList.Count > 0)
            {
                PopSmall();
            }
        }
        private void Reset()
        {
            NeedPopBigList.Clear();
            NeedPopSmallList.Clear();
            smallObj = null;
            bigObj = null;
            isShowBig = false;
            isShowSmall = false;
        }
        private void PopBig()
        {
            if (isShowBig) return;
            isShowBig = true;
            BigMessage big = PopBigMessage();
            if (big == null)
            {
                isShowBig = true; 
                return;
            }
            LoadBig(big);
        }
        private void PopSmall()
        {
            string content = PopSmallMessage();
            if (string.IsNullOrWhiteSpace(content)) return;
            LoadSmall(content);
        }
        public string PopSmallMessage()
        {
            return NeedPopSmallList.Count > 0 ? NeedPopSmallList.Dequeue() : null;
        }
        public BigMessage PopBigMessage()
        {
            return NeedPopBigList.Count > 0 ? NeedPopBigList.Dequeue() : null;
        }
        private void LoadBig(BigMessage message)
        {
            UIManager.Instance.OpenUI<TishiPanel_Version3>(message);
        }
        private void LoadSmall(string content)
        {
            UIManager.Instance.OpenUI<TipPanel>(content);
        }
        public void ShowBig(BigMessage msg)
        {
            NeedPopBigList.Enqueue(msg);
        }
        public void ShowSmall(string msg)
        {
            NeedPopSmallList.Enqueue(msg);
        }
    }
    public class BigMessage
    {
        public string content;
        public CAction okCall;
        public CAction cancelCall;
    }
}
