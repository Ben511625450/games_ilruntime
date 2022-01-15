using LuaFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GameSetBtns: ILHotfixEntity
    {
        Button SetsBtn;

        Transform RecFrameMask;
        Button CloseRecFrameMask;
        Button BgMusicBtn;
        Button HelpBtn;
        Button ReturnBtn;

        Transform MusicPanel;
        Button CloseMusicPanel;
        ToggleGroup BgMusic;
        Toggle BgMusicDK;

        ToggleGroup BgSound;
        Toggle BgSoundDK;

        protected override void Awake()
        {
            base.Awake();
            AddListener();
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            SetsBtn = this.transform.FindChildDepth<Button>("SetsBtn");

            RecFrameMask = this.transform.FindChildDepth("RecFrameMask");
            CloseRecFrameMask = RecFrameMask.FindChildDepth<Button>("Close");
            BgMusicBtn = RecFrameMask.FindChildDepth<Button>("RecFrameBtns/BgMusicBtn");
            HelpBtn = RecFrameMask.FindChildDepth<Button>("RecFrameBtns/HelpBtn");
            ReturnBtn = RecFrameMask.FindChildDepth<Button>("RecFrameBtns/ReturnBtn");

            RecFrameMask.gameObject.SetActive(false);

            MusicPanel = this.transform.FindChildDepth("MusicPanel");
            CloseMusicPanel = MusicPanel.FindChildDepth<Button>("mainPanel/CloseBtn");
            BgMusic = MusicPanel.FindChildDepth<ToggleGroup>("BgMusic");
            BgMusicDK = BgMusic.transform.FindChildDepth<Toggle>("dk");
            BgSound = MusicPanel.FindChildDepth<ToggleGroup>("BgSound");
            BgSoundDK = BgSound.transform.FindChildDepth<Toggle>("dk");

            MusicPanel.gameObject.SetActive(false);
        }

        private void AddListener() 
        {
            SetsBtn.onClick.RemoveAllListeners();
            SetsBtn.onClick.Add(SetsBtnOnClick);

            CloseRecFrameMask.onClick.RemoveAllListeners();
            CloseRecFrameMask.onClick.Add(CloseRecFrameMaskOnClick);

            BgMusicBtn.onClick.RemoveAllListeners();
            BgMusicBtn.onClick.Add(BgMusicBtnOnClick);

            HelpBtn.onClick.RemoveAllListeners();
            HelpBtn.onClick.Add(HelpBtnOnClick);

            ReturnBtn.onClick.RemoveAllListeners();
            ReturnBtn.onClick.Add(ReturnBtnOnClick);

            CloseMusicPanel.onClick.RemoveAllListeners();
            CloseMusicPanel.onClick.Add(CloseMusicPanelOnClick);

            BgMusicDK.onValueChanged.RemoveAllListeners();
            BgMusicDK.onValueChanged.Add(delegate (bool value)
            {
                if (value)
                {
                    ToolHelper.PlayMusic();
                    HotfixActionHelper.DispatchLoadGameMusic(value);
                }
                else
                {
                    ToolHelper.MuteMusic();
                    HotfixActionHelper.DispatchLoadGameMusic(value);
                }
                BgMusicDK.isOn = value;
            });

            BgSoundDK.onValueChanged.RemoveAllListeners();
            BgSoundDK.onValueChanged.Add(delegate (bool value)
            {
                if (value)
                {
                    ToolHelper.PlaySound();
                    HotfixActionHelper.DispatchLoadGameSound(value);
                }
                else
                {
                    ToolHelper.MuteSound();
                    HotfixActionHelper.DispatchLoadGameSound(value);
                }
                BgSoundDK.isOn = value;
            });
        }

        private void CloseMusicPanelOnClick()
        {
            MusicPanel.gameObject.SetActive(false);
        }

        private void ReturnBtnOnClick()
        {
            HotfixActionHelper.DispatchLoadGameExit();
        }

        private void HelpBtnOnClick()
        {
            RecFrameMask.gameObject.SetActive(false);
            HotfixActionHelper.DispatchLoadGameRule();
        }

        private void BgMusicBtnOnClick()
        {
            RecFrameMask.gameObject.SetActive(false);
            MusicPanel.gameObject.SetActive(true);
        }

        private void CloseRecFrameMaskOnClick()
        {
            RecFrameMask.gameObject.SetActive(false);
        }

        private void SetsBtnOnClick()
        {
            if (RecFrameMask.gameObject.activeSelf)
            {
                RecFrameMask.gameObject.SetActive(false);
            }
            else
            {
                RecFrameMask.gameObject.SetActive(true);
            }
        }
    }
}
