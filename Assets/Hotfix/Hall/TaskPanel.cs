﻿using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class TaskPanel : PanelBase
    {
        private Button closeBtn;
        private Transform image1;
        private Transform image2;
        private Button maskCloseBtn;
        private ScrollRect group;
        private GameObject noTask;

        public TaskPanel() : base(UIType.Middle, nameof(TaskPanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            Init();
        }

        protected override void FindComponent()
        {
            group = transform.FindChildDepth<ScrollRect>($"Group");
            noTask = transform.FindChildDepth("NoTask").gameObject;
            closeBtn = transform.FindChildDepth<Button>("mainPanel/CloseBtn");
            maskCloseBtn = transform.FindChildDepth<Button>("zhezhao");
            for (int i = 0; i < group.content.childCount; i++)
            {
                Button btn = @group.content.GetChild(i).FindChildDepth<Button>($"Button");
                btn.onClick.RemoveAllListeners();
                btn.onClick.Add(() =>
                {
                    ILMusicManager.Instance.PlayBtnSound();
                    UIManager.Instance.ReplaceUI<BindPanel>();
                });
            }
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(() =>
            {
                ILMusicManager.Instance.PlayBtnSound();
                UIManager.Instance.Close();
            });

            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(() =>
            {
                ILMusicManager.Instance.PlayBtnSound();
                UIManager.Instance.Close();
            });
        }

        private void Init()
        {
            bool isGuest = string.IsNullOrEmpty(GameLocalMode.Instance.SCPlayerInfo.SzPhoneNumber);
            noTask.gameObject.SetActive(!isGuest);
            group.gameObject.SetActive(isGuest);
        }
    }
}