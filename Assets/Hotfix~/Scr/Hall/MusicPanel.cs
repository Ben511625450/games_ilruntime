﻿using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class MusicPanel : PanelBase
    {
        private Button closeBtn;
        private Transform mainPanel;
        private Button maskCloseBtn;

        private Slider MusicSlider;
        private Slider SoundSlider;

        private Toggle Musictog;
        private Toggle Musictog2;

        private Toggle Soundtog;
        private Toggle Soundtog2;


        public MusicPanel() : base(UIType.Middle, nameof(MusicPanel))
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
            closeBtn = mainPanel.FindChildDepth<Button>("CloseBtn");

            MusicSlider = mainPanel.FindChildDepth<Slider>("Image/Image1/Slider");
            SoundSlider = mainPanel.FindChildDepth<Slider>("Image/Image2/Slider");

        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseMusicPanel);

            MusicSlider.onValueChanged.RemoveAllListeners();
            MusicSlider.onValueChanged.Add(OnMusicValueChanged);

            SoundSlider.onValueChanged.RemoveAllListeners();
            SoundSlider.onValueChanged.Add(OnSoundValueChanged);
        }

        private void Init()
        {
            MusicSlider.value = ILMusicManager.Instance.GetMusicValue();
            SoundSlider.value = ILMusicManager.Instance.GetSoundValue();
        }

        private void OnMusicValueChanged(float value)
        {
            ILMusicManager.Instance.SetMusicValue(value);
        }

        private void OnSoundValueChanged(float value)
        {
            ILMusicManager.Instance.SetSoundValue(value);
        }

        private void CloseMusicPanel()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.Close();
        }
    }
}