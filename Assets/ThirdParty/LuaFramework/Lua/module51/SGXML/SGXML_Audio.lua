SGXML_Audio = {};

local self = SGXML_Audio;

self.SoundList = {
    BGM = "BGM", --背景音乐
    BTN = "Button", --按钮音乐
    RStart = "RollStart", --每列停止音乐
    RollStop = "RollStop", --每列停止音乐
    Rolling = "Rolling", --每列停止音乐
    LINE = "Reward", --连线
    Bomb = "Bomb", --连线
    Counting = "BigWinCoin", --连线
    SmallEnd = "SmallEnd", --超大倍率音乐
    SmallBingo = "SmallBingo", --超大倍率音乐
    SmallPayout = "SmallPayout", --超大倍率音乐
    SmallRoll = "SmallRoll", --超大倍率音乐
    SmallStart = "SmallStart"--超大倍率音乐
}
self.pool = nil;
function SGXML_Audio.Init()
    self.pool = GameObject.New("AudioPool"):AddComponent(typeof(UnityEngine.AudioSource));
    self.pool.playOnAwake = false;
    self.pool.loop = true;
    self.pool.volume = MusicManager.musicVolume;
    self.pool.mute = not AllSetGameInfo._5IsPlayAudio;
    self.pool.transform:SetParent(SGXMLEntry.MainContent:Find("Content"));
end
function SGXML_Audio.PlayBGM(mode)
    --播放bgm
    self.pool:Stop();
    local rc = AllSetGameInfo._5IsPlayAudio;
    local audioclip = nil;
    if mode == nil then
        audioclip = SGXMLEntry.soundList:Find(self.SoundList.BGM):GetComponent("AudioSource");
    else
        audioclip = SGXMLEntry.soundList:Find(mode):GetComponent("AudioSource");
    end
    self.pool.clip = audioclip.clip;
    self.pool:Play();
end

function SGXML_Audio.PlaySound(soundName, time)
    if soundName == nil then
        error("不存在该音效");
        return ;
    end
    local isPlay = AllSetGameInfo._6IsPlayEffect;
    if not isPlay then
        return ;
    end
    local volumn = 1;
    if PlayerPrefs.HasKey("SoundValue") then
        volumn = tonumber(PlayerPrefs.GetString("SoundValue"));
    end
    local obj = SGXMLEntry.soundList:Find(soundName);
    if obj == nil then
        error("没有找到该音效");
        return ;
    end
    local go = newObject(obj.gameObject);
    go.transform:SetParent(self.pool.transform);
    go.transform.localPosition = Vector3.New(0, 0, 0);
    go.name = soundName;
    local audio = go.transform:GetComponent("AudioSource");
    audio.volume = volumn;
    audio:Play();
    local timer = time;
    if timer == nil then
        timer = audio.clip.length;
    end
    GameObject.Destroy(audio.gameObject, timer);
end
function SGXML_Audio.ClearAuido(soundName)
    local sound = self.pool.transform:Find(soundName);
    if sound ~= nil then
        destroy(sound.gameObject);
    end
end