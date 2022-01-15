using LuaFramework;
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

            //MusicSlider = mainPanel.FindChildDepth<Slider>("Image/Music/Slider");
            //SoundSlider = mainPanel.FindChildDepth<Slider>("Image/Sound/Slider");
            Musictog= mainPanel.FindChildDepth<Toggle>("Image/Image1/BgMusic/dk");
            Musictog2 = mainPanel.FindChildDepth<Toggle>("Image/Image1/BgMusic/gb");
            Soundtog = mainPanel.FindChildDepth<Toggle>("Image/Image2/BgSound/dk");
            Soundtog2 = mainPanel.FindChildDepth<Toggle>("Image/Image2/BgSound/gb");

        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseMusicPanel);

            //MusicSlider.onValueChanged.RemoveAllListeners();
            //MusicSlider.onValueChanged.Add(OnMusicValueChanged);

            //SoundSlider.onValueChanged.RemoveAllListeners();
            //SoundSlider.onValueChanged.Add(OnSoundValueChanged);

            Musictog.onValueChanged.RemoveAllListeners();
            Musictog.onValueChanged.Add((value)=> 
            {
                OnMusicToggleChanged(value);
            });

            Soundtog.onValueChanged.RemoveAllListeners();
            Soundtog.onValueChanged.Add((value) =>
            {
                OnSoundToggleChanged(value);
            });
        }

        private void Init()
        {
            Musictog2.isOn=!ILMusicManager.Instance.isPlayMV;
            Soundtog2.isOn =!ILMusicManager.Instance.isPlaySV;
        }

        private void OnMusicToggleChanged(bool value)
        {
            ILMusicManager.Instance.SetMusicMute(!value);
        }

        private void OnSoundToggleChanged(bool value)
        {
            ILMusicManager.Instance.SetSoundMute(!value);
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