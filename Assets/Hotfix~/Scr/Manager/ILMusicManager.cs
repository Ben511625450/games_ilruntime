using System.Collections.Generic;
using Hotfix.Hall;
using LitJson;
using LuaFramework;
using UnityEngine;

namespace Hotfix
{
    public class ILMusicManager : SingletonILEntity<ILMusicManager>
    {
        private Dictionary<string, GameObject> _musicbundles;
        private readonly string SoundSaveKey = "SoundSave";
        private AudioSource audio;
        public bool isPlayMV;
        public bool isPlaySV;
        private SoundSaveData soundSaveData;

        public const string BGM = "bgm3";
        public const string BTN = "anniu3";

        private void GetAudio()
        {
            audio = transform.GetComponent<AudioSource>();
            if (audio != null) return;
            audio = gameObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;
        }

        protected override void Awake()
        {
            base.Awake();
            GetAudio();
            _musicbundles = new Dictionary<string, GameObject>();
            soundSaveData = SaveHelper.Get<SoundSaveData>(SoundSaveKey);
            if (soundSaveData == null)
            {
                soundSaveData = new SoundSaveData
                {
                    soundVolume = 1,
                    musicVolume = 1,
                    isMuteMusic = false,
                    isMuteSound = false
                };
                SaveHelper.Save<SoundSaveData>(SoundSaveKey, soundSaveData);
            }
            isPlayMV = soundSaveData.musicVolume > 0 && !soundSaveData.isMuteMusic;
            isPlaySV = soundSaveData.soundVolume > 0 && !soundSaveData.isMuteSound;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _musicbundles.Clear();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBackgroundMusic()
        {
            GameObject obj = null;
            obj = !_musicbundles.ContainsKey(BGM)
                ? Util.LoadAsset($"module02/Pool/music", $"Music")
                : _musicbundles[BGM];

            if (obj == null)
            {
                DebugHelper.LogError($"没找到music");
                return;
            }

            AudioSource source = obj.transform.FindChildDepth<AudioSource>(BGM);
            if (source == null)
            {
                DebugHelper.LogError($"没找到背景音乐节点{BGM}");
                return;
            }

            PlayMusic(source.clip);
        }

        /// <summary>
        /// 播放按钮音效
        /// </summary>
        public void PlayBtnSound()
        {
            GameObject obj = null;
            obj = !_musicbundles.ContainsKey(BTN)
                ? Util.LoadAsset($"module02/Pool/music", $"Music")
                : _musicbundles[BTN];

            if (obj == null)
            {
                DebugHelper.LogError($"没找到music");
                return;
            }

            AudioSource source = obj.transform.FindChildDepth<AudioSource>(BTN);
            if (source == null)
            {
                DebugHelper.LogError($"没找到音效节点{BTN}");
                return;
            }

            PlaySound(source.clip);
        }

        /// <summary>
        ///     设置音效音量
        /// </summary>
        /// <param name="sv">音效音量</param>
        public void SetSoundValue(float sv)
        {
            GetAudio();
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).GetComponent<AudioSource>().volume = sv;
            }

            soundSaveData.soundVolume = sv;
            isPlaySV = sv != 0;
            soundSaveData.isMuteSound = !isPlaySV;

            SaveHelper.Save(SoundSaveKey, soundSaveData);
        }

        /// <summary>
        ///     获取音效音量
        /// </summary>
        /// <returns></returns>
        public float GetSoundValue()
        {
            soundSaveData = SaveHelper.Get<SoundSaveData>(SoundSaveKey);
            return isPlaySV ? (float) soundSaveData.soundVolume : 0;
        }

        /// <summary>
        ///     设置音乐音量
        /// </summary>
        /// <param name="mv">音乐音量</param>
        public void SetMusicValue(float mv)
        {
            GetAudio();
            audio.volume = mv;
            soundSaveData.musicVolume = mv;
            isPlayMV = mv != 0;
            soundSaveData.isMuteMusic = !isPlayMV;
            SaveHelper.Save(SoundSaveKey, soundSaveData);
        }

        /// <summary>
        ///     获取音乐音量
        /// </summary>
        /// <returns></returns>
        public float GetMusicValue()
        {
            soundSaveData = SaveHelper.Get<SoundSaveData>(SoundSaveKey);
            return isPlayMV ? (float) soundSaveData.musicVolume : 0;
        }

        /// <summary>
        ///     设置音效静音
        /// </summary>
        /// <param name="isMute">是否静音</param>
        public void SetSoundMute(bool isMute)
        {
            GetAudio();
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).GetComponent<AudioSource>().mute = isMute;
            }

            soundSaveData.isMuteSound = isMute;
            isPlaySV = !isMute;
            SaveHelper.Save(SoundSaveKey, soundSaveData);
        }

        /// <summary>
        /// 设置音乐静音
        /// </summary>
        /// <param name="isMute"></param>
        public void SetMusicMute(bool isMute)
        {
            DebugHelper.Log("isMute===" + isMute);

            GetAudio();
            audio.mute = isMute;
            soundSaveData.isMuteMusic = isMute;
            isPlayMV = !isMute;
            SaveHelper.Save(SoundSaveKey, soundSaveData);
        }

        /// <summary>
        ///     播放音乐
        /// </summary>
        /// <param name="clip">音乐文件</param>
        public void PlayMusic(AudioClip clip)
        {
            GetAudio();
            if (clip == null)
            {
                DebugHelper.LogError("音乐片段是错误的");
                return;
            }

            audio.clip = clip;
            audio.volume = (float) soundSaveData.musicVolume;
            audio.mute = soundSaveData.isMuteMusic;
            audio.loop = true;
            audio.Play();
        }

        /// <summary>
        ///     播放音效
        /// </summary>
        /// <param name="clip">音效</param>
        /// <param name="times">次数</param>
        public void PlaySound(AudioClip clip, int times = 1)
        {
            GetAudio();
            if (clip == null)
            {
                DebugHelper.LogError("音乐片段是错误的");
                return;
            }

            var timer = clip.length * times;
            var go = new GameObject($"{clip.name}");
            go.transform.SetParent(audio.transform);
            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = true;
            source.volume = (float) soundSaveData.soundVolume;
            source.mute = soundSaveData.isMuteSound;
            source.Play();
            HallExtend.Destroy(go, timer);
        }

        /// <summary>
        ///     停止播放音乐
        /// </summary>
        public void StopMusic()
        {
            GetAudio();
            audio.Stop();
        }

        /// <summary>
        ///     停止所有音效
        /// </summary>
        public void StopAllSound()
        {
            GetAudio();
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).GetComponent<AudioSource>().Stop();
                HallExtend.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public class SoundSaveData : BaseSave
    {
        public bool isMuteMusic;
        public bool isMuteSound;
        public double musicVolume;
        public double soundVolume;
    }
}