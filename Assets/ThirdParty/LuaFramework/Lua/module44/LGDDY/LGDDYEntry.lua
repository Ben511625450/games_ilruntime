-- Game01Panel.lua
-- Date
-- slot场景控制 对应LuaBehaviour
-- endregion
local g_swlzbNum = "Module44/LGDDY/";

require(g_swlzbNum .. "LGDDY_Network")
require(g_swlzbNum .. "LGDDY_Roll")
require(g_swlzbNum .. "LGDDY_DataConfig")
require(g_swlzbNum .. "LGDDY_Caijin")
require(g_swlzbNum .. "LGDDY_Line")
require(g_swlzbNum .. "LGDDY_Result")
require(g_swlzbNum .. "LGDDY_Rule")
require(g_swlzbNum .. "LGDDY_Audio")
require(g_swlzbNum .. "LGDDY_SmallGame")

LGDDYEntry = {};
local self = LGDDYEntry;
self.transform = nil;
self.gameObject = nil;
self.luaBehaviour = nil;

self.CurrentFreeIndex = 0;
self.CurrentChip = 0;
self.CurrentChipIndex = 0;
self.isAutoGame = false;
self.isFreeGame = false;
self.isSmallGame = false;
self.isRoll = false;
self.menuTweener = nil;

self.currentFreeCount = 0;

self.currentAutoCount = 0;--剩余自动次数
self.freeCount = 0;--剩余免费次数
self.totalFreeCount = 0;

self.smallGameCount = 0;

self.myGold = 0;--显示的自身金币
self.ScatterList = {};

self.clickStartTimer = -1;
self.betRollList = {};
self.betProgresslist = {};
self.isPlayAudio = false;

self.SceneData = {
    freeNumber = 0, --免费次数
    bet = 0, --当前下注
    chipNum = 0, --用户金币
    chipList = {}, --下注列表
    nFreeGameCotTotal = 0, --免费游戏总次数int
    nFreeGameGold = 0, --免费游戏获得金币int
    nSmallCount = 0--当前小游戏次数int
};
self.ResultData = {
    ImgTable = {}, --图标
    LineTypeTable = {}, --连线类型
    m_nWinPeiLv = 0, --当前倍率
    m_nCurrGold = 0, --自身金币
    WinScore = 0, --赢得总分
    TotalWinScore = 0, --赢得总分
    FreeCount = 0, --免费次数
    m_nMultiple = 0, --当前押注
    m_nJackPotValue = 0, --奖金池奖int
    m_bSmallGame = 0--小游戏次数
}
self.SmallResultData = {
    nSmallGameTatolConut = 0, --//小游戏次数int
    nSmallGameConut = 0, --	//小游戏剩余次数int
    nGameTatolGold = 0, --//小游戏总获得金币int
    nIconType = 0, --//游戏图标int
    nIconTypeConut = 0, --//第几个水果int
    nIconType4 = {},
    --[4]//中间四个图标int
    nGameGold = 0, --//游戏本次获得金币int
    nGameEnd = 0, --是否结束
    nLineGold = 0               --//单线金币int
}
function LGDDYEntry:Awake(obj)
    Screen.autorotateToLandscapeLeft = true;
    Screen.autorotateToLandscapeRight = true;
    Screen.autorotateToPortrait = false;
    Screen.autorotateToPortraitUpsideDown = false;
    self.transform = obj.transform;
    self.gameObject = obj.gameObject;
    self.luaBehaviour = self.transform:GetComponent("LuaBehaviour");
    --self.GetGameConfig();
    self.FindComponent();
    self.AddListener();
    LGDDY_Roll.Init(self.RollContent);
    LGDDY_Audio.Init();
    LGDDY_Caijin.Init();
    LGDDY_Caijin.currentCaijin = 0;
    LGDDY_SmallGame.Init(self.SmallGamePanel);
    LGDDY_Network.AddMessage();--添加消息监听
    LGDDY_Network.LoginGame();--开始登录游戏
    GameSetsPanel.CreateHideObj(self.MainContent, false);
    LGDDY_Audio.PlayBGM();
end
--
function LGDDYEntry.OnDestroy()
    LGDDY_Line.Close();
    LGDDY_Network.UnMessage();
end

function LGDDYEntry:Update()
    self.CanShowAuto();
    LGDDY_Roll.Update();
    LGDDY_Caijin.Update();
    LGDDY_Line.Update();
    LGDDY_Result.Update();
    LGDDY_SmallGame.Update();
end
function LGDDYEntry.ResetRoll()
    self.AutoContent.gameObject:SetActive(false);
    self.FreeContent.gameObject:SetActive(false);
    self.startState.gameObject:SetActive(true);
    self.stopState.gameObject:SetActive(false);
    LGDDYEntry.addChipBtn.interactable = true;
    LGDDYEntry.reduceChipBtn.interactable = true;
    LGDDYEntry.MaxChipBtn.interactable = true;
    LGDDYEntry.startBtn.interactable = true;
end
function LGDDYEntry.CanShowAuto()
    if self.clickStartTimer >= 0 then
        self.clickStartTimer = self.clickStartTimer + Time.deltaTime;
        if self.clickStartTimer >= 0.5 and not self.isPlayAudio and self.clickStartTimer < 2 then
            self.isPlayAudio = true;
            LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.SmallStart);
        end
        if self.clickStartTimer >= 2 then
            self.clickStartTimer = -1;
            self.isPlayAudio = false;
            LGDDY_Audio.ClearAuido(LGDDY_Audio.SoundList.SmallStart);
            self.OnClickAutoCall();
        end
    end
end

function LGDDYEntry.FindComponent()
    self.MainContent = self.transform:Find("MainPanel");

    self.RollContent = self.MainContent:Find("Content/RollContent");--转动区域
    self.ChipNum = self.MainContent:Find("Content/Bottom/Chip/Num"):GetComponent("TextMeshProUGUI");--下注金额
    self.WinNum = self.MainContent:Find("Content/Bottom/Win/Num"):GetComponent("TextMeshProUGUI");--本次获得金币
    self.WinDesc = self.MainContent:Find("Content/Bottom/Win/WinDesc"):GetComponent("TextMeshProUGUI");--本次获得金币
    self.WinDesc.text = "点击开始，赢取奖励";

    self.MosaicNum = self.MainContent:Find("Content/Mosaic/MosaicNum"):GetComponent("TextMeshProUGUI");--彩金

    self.reduceChipBtn = self.MainContent:Find("Content/Bottom/Chip/Reduce"):GetComponent("Button");--减注
    self.addChipBtn = self.MainContent:Find("Content/Bottom/Chip/Add"):GetComponent("Button");--加注
    self.chipProgress = self.MainContent:Find("Content/Bottom/Chip/Mask/Bet/Progress"):GetComponent("Image");
    self.chipPoint = self.MainContent:Find("Content/Bottom/Chip/Mask/Bet/Point");
    self.chipPan = self.MainContent:Find("Content/Bottom/Chip/Mask/Bet/Pan");
    self.chipBet = self.MainContent:Find("Content/Bottom/Chip/Mask/Bet");

    self.btnGroup = self.MainContent:Find("Content/ButtonGroup");
    self.startBtn = self.btnGroup:Find("StartBtn"):GetComponent("Button");--开始按钮
    self.startState = self.startBtn.transform:Find("Start");
    self.stopState = self.startBtn.transform:Find("Stop");
    self.FreeContent = self.startBtn.transform:Find("Free");--免费状态
    self.freeText = self.FreeContent.transform:Find("Num"):GetComponent("TextMeshProUGUI");--免费次数
    self.AutoContent = self.startBtn.transform:Find("AutoState");
    self.autoText = self.AutoContent.transform:Find("AutoNum"):GetComponent("TextMeshProUGUI");--自动次数
    self.autoInfinite = self.AutoContent.transform:Find("Infinite")--无限次数

    self.MaxChipBtn = self.btnGroup:Find("MaxChip"):GetComponent("Button");--最大下注
    self.closeAutoMenu = self.btnGroup:Find("CloseAutoMenu"):GetComponent("Button");--关闭自动开始界面
    self.autoSelectList = self.closeAutoMenu.transform:Find("AutoSelect");--自动开始次数选择
    self.closeAutoMenu.gameObject:SetActive(false);

    self.rulePanel = self.MainContent:Find("Content/Rule");--规则界面
    self.ruleList = self.rulePanel:Find("Content/RuleList"):GetComponent("ScrollRect");--规则子界面
    self.leftShowBtn = self.rulePanel:Find("Content/LeftBtn"):GetComponent("Button");
    self.rightShowBtn = self.rulePanel:Find("Content/RightBtn"):GetComponent("Button");
    self.closeRuleBtn = self.rulePanel:Find("Content/BackBtn"):GetComponent("Button");

    self.menuBtn = self.MainContent:Find("Content/Menu"):GetComponent("Button");--菜单按钮
    self.closeGame = self.MainContent:Find("Content/QuitGameBtn"):GetComponent("Button");
    self.showRuleBtn = self.MainContent:Find("Content/RuleBtn"):GetComponent("Button");

    self.resultEffect = self.MainContent:Find("Content/Result");--中奖后特效
    self.normalWinEffect = self.resultEffect:Find("NormalReward");
    self.normalWinNum = self.normalWinEffect:Find("Num"):GetComponent("TextMeshProUGUI");
    self.midWinEffect = self.resultEffect:Find("MidReward");
    self.midWinNum = self.midWinEffect:Find("Num"):GetComponent("TextMeshProUGUI");
    self.bigWinEffect = self.resultEffect:Find("BigReward");
    self.bigWinNum = self.bigWinEffect:Find("Num"):GetComponent("TextMeshProUGUI");
    self.EnterFreeEffect = self.MainContent:Find("Content/EnterFree");
    self.FreeResultEffect = self.resultEffect:Find("FreeResult");
    self.FreeTotalCount = self.FreeResultEffect:Find("FreeCount"):GetComponent("TextMeshProUGUI");
    self.FreeResultNum = self.FreeResultEffect:Find("TotalWin"):GetComponent("TextMeshProUGUI");

    self.enterSmallEffect = self.resultEffect:Find("EnterSmallGame");
    self.SmallResultEffect = self.resultEffect:Find("SmallGameResult");
    self.SmallLineWinNum = self.SmallResultEffect:Find("LineWinNum"):GetComponent("TextMeshProUGUI");
    self.SmallWinNum = self.SmallResultEffect:Find("SmallWinNum"):GetComponent("TextMeshProUGUI");

    self.SmallRuleEffect = self.resultEffect:Find("SmallRule");

    self.userInfo = self.MainContent:Find("Content/Bottom/UserInfo");
    self.headIcon = self.userInfo:Find("Head"):GetComponent("Image");
    self.nickName = self.userInfo:Find("UserName"):GetComponent("Text");
    self.selfGold = self.userInfo:Find("GoldNum"):GetComponent("TextMeshProUGUI");--自身金币

    self.icons = self.MainContent:Find("Content/Icons");--图标库
    self.effectList = self.MainContent:Find("Content/EffectList");--动画库
    self.effectPool = self.MainContent:Find("Content/EffectPool");--动画缓存库
    self.CSGroup = self.MainContent:Find("Content/CSContent");--显示财神
    self.LineGroup = self.MainContent:Find("Content/LineGroup");--连线
    self.soundList = self.MainContent:Find("Content/SoundList");--声音库
    self.SmallGamePanel = self.MainContent:Find("Content/SmallGamePanel");--小游戏
    self.settingPanel = self.MainContent:Find("Content/Setting");--设置界面

    self.quitPanel = self.MainContent:Find("Content/QuitPanel");

    self.headIcon.sprite = HallScenPanel.GetHeadIcon();
    self.nickName.text = SCPlayerInfo._05wNickName;
end

function LGDDYEntry.AddListener()
    --添加监听事件
    self.reduceChipBtn.onClick:RemoveAllListeners();
    self.reduceChipBtn.onClick:AddListener(self.ReduceChipCall);
    self.addChipBtn.onClick:RemoveAllListeners();
    self.addChipBtn.onClick:AddListener(self.AddChipCall);
    local et = self.startBtn.gameObject:AddComponent(typeof(LuaFramework.EventTriggerListener));
    et.onDown = self.OnDownStartBtnCall;
    et.onUp = self.OnUpStartBtnCall;

    self.menuBtn.onClick:RemoveAllListeners();
    self.menuBtn.onClick:AddListener(self.ClickMenuCall);
    self.closeGame.onClick:RemoveAllListeners();
    self.closeGame.onClick:AddListener(self.CloseGameCall);
    self.showRuleBtn.onClick:RemoveAllListeners();--显示规则
    self.showRuleBtn.onClick:AddListener(self.ShowRultPanel);

    self.MaxChipBtn.onClick:RemoveAllListeners();
    self.MaxChipBtn.onClick:AddListener(self.SetMaxChipCall);
    self.closeAutoMenu.onClick:RemoveAllListeners();--关闭自动
    self.closeAutoMenu.onClick:AddListener(function()
        self.closeAutoMenu.gameObject:SetActive(false);
    end)
    for i = 1, #LGDDY_DataConfig.autoList do
        --自动次数选择
        local child = nil;
        if i > self.autoSelectList.childCount then
            child = newObject(self.autoSelectList:GetChild(0)).transform;
            child:SetParent(self.autoSelectList);
            child.localPosition = Vector3.New(0, 0, 0);
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.New(1, 1, 1);
        else
            child = self.autoSelectList:GetChild(i - 1);
        end
        if LGDDY_DataConfig.autoList[i] > 1000 then
            child:Find("Infinite").gameObject:SetActive(false);
            child:Find("Num").gameObject:SetActive(true);
            child:Find("Num"):GetComponent("TextMeshProUGUI").text = "<size=30>∞</size>";
        else
            child:Find("Infinite").gameObject:SetActive(false);
            child:Find("Num").gameObject:SetActive(true);
            child:Find("Num"):GetComponent("TextMeshProUGUI").text = tostring(LGDDY_DataConfig.autoList[i]);
        end
        child.gameObject.name = tostring(LGDDY_DataConfig.autoList[i]);
        child:GetComponent("Button").onClick:RemoveAllListeners();
        child:GetComponent("Button").onClick:AddListener(function()
            self.currentAutoCount = LGDDY_DataConfig.autoList[i];
            self.OnClickAutoItemCall();
        end)
        child.gameObject:SetActive(true);
    end
end

function LGDDYEntry.GetGameConfig()
    --获取远程端配置
    local formdata = FormData.New();
    formdata:AddField("v", os.time());
    local url = string.gsub(AppConst.WebUrl, "/android", "");
    url = string.gsub(url, "/iOS", "");
    url = string.gsub(url, "/ios", "");
    url = string.gsub(url, "/win32", "");
    url = string.gsub(url, "/win", "");
    UnityWebRequestManager.Instance:GetText(url .. "Config/module27.json", 4, formdata, function(state, result)
        log(result);
        local data = json.decode(result);
        LGDDY_DataConfig.rollTime = data.rollTime;
        LGDDY_DataConfig.rollReboundRate = data.rollReboundRate;
        LGDDY_DataConfig.rollInterval = data.rollInterval;
        LGDDY_DataConfig.rollSpeed = data.rollSpeed;
        LGDDY_DataConfig.caijinGoldChangeRate = data.caijinGoldChangeRate;
        LGDDY_DataConfig.winGoldChangeRate = data.winGoldChangeRate;
        LGDDY_DataConfig.selfGoldChangeRate = data.selfGoldChangeRate;
        LGDDY_DataConfig.freeLoadingShowTime = data.freeLoadingShowTime;
        LGDDY_DataConfig.smallGameLoadingShowTime = data.smallGameLoadingShowTime;
        LGDDY_DataConfig.rollDistance = data.rollDistance;
        LGDDY_DataConfig.REQCaiJinTime = data.REQCaiJinTime;
        LGDDY_DataConfig.lineAllShowTime = data.lineAllShowTime;
        LGDDY_DataConfig.cyclePlayLineTime = data.cyclePlayLineTime;
        LGDDY_DataConfig.waitShowLineTime = data.waitShowLineTime;
        LGDDY_DataConfig.autoRewardInterval = data.autoRewardInterval;
        LGDDY_DataConfig.autoNoRewardInterval = data.autoNoRewardInterval;
    end);
end

function LGDDYEntry.ToCharArray(num)
    --拆解字符串
    local str = tostring(num);
    local list1 = {}
    for i = 1, string.len(str) do
        table.insert(list1, #list1 + 1, string.sub(str, i, i));
    end
    return list1;
end

function LGDDYEntry.FormatNumberThousands(num)
    --对数字做千分位操作
    local function checknumber(value)
        return tonumber(value) or 0
    end
    local formatted = tostring(checknumber(num))
    local k
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        print(formatted, k)
        if k == 0 then
            break
        end

    end
    return formatted
end

function LGDDYEntry.OnDownStartBtnCall()
    self.clickStartTimer = 0;
    self.isPlayAudio = false;
end

function LGDDYEntry.OnUpStartBtnCall()
    if self.clickStartTimer < 2 and self.clickStartTimer ~= -1 then
        self.clickStartTimer = -1;
        self.isPlayAudio = false;
        LGDDY_Audio.ClearAuido(LGDDY_Audio.SoundList.SmallStart);
        self.StartGameCall();
    end
end

function LGDDYEntry.SetSelfGold(str)
    self.selfGold.text = tostring(str);
    LGDDY_SmallGame.SelfGold.text = self.ShowText(str);
end

function LGDDYEntry.ShowText(str)
    --展示tmp字体
    local arr = self.ToCharArray(str);
    local _str = "";
    for i = 1, #arr do
        _str = _str .. string.format("<sprite name=\"%s\">", arr[i]);
    end
    return _str;
end

function LGDDYEntry.ReduceChipCall()
    --减注
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if self.SceneData.chipList == nil or #self.SceneData.chipList <= 0 then
        return ;
    end
    self.CurrentChipIndex = self.CurrentChipIndex - 1;
    if self.CurrentChipIndex <= 0 then
        self.CurrentChipIndex = 1;
        MessageBox.CreatGeneralTipsPanel("下注已达最小值");
        return ;
    end
    self.CurrentChip = self.SceneData.chipList[self.CurrentChipIndex];
    self.ChipNum.text = self.ShowText(self.CurrentChip * LGDDY_DataConfig.ALLLINECOUNT);
    if self.chipprogressTween ~= nil then
        self.chipprogressTween:Kill();
    end
    self.chipprogressTween = self.chipProgress:DOFillAmount(self.betProgresslist[self.CurrentChipIndex], 0.1);
    self.chipPan:DOLocalRotate(Vector3.New(0, 0, -90 - self.betRollList[self.CurrentChipIndex]), 0.1);
    self.chipPoint:DOLocalRotate(Vector3.New(0, 0, self.betRollList[self.CurrentChipIndex]), 0.1);
end

function LGDDYEntry.AddChipCall()
    --加注
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if self.SceneData.chipList == nil or #self.SceneData.chipList <= 0 then
        return ;
    end
    self.CurrentChipIndex = self.CurrentChipIndex + 1;
    if self.CurrentChipIndex > #self.SceneData.chipList then
        self.CurrentChipIndex = #self.SceneData.chipList;
        MessageBox.CreatGeneralTipsPanel("下注已达最大值");
        return ;
    end
    self.CurrentChip = self.SceneData.chipList[self.CurrentChipIndex];
    self.ChipNum.text = self.ShowText(self.CurrentChip * LGDDY_DataConfig.ALLLINECOUNT);
    if self.chipprogressTween ~= nil then
        self.chipprogressTween:Kill();
    end
    self.chipprogressTween = self.chipProgress:DOFillAmount(self.betProgresslist[self.CurrentChipIndex], 0.1);
    self.chipPan:DOLocalRotate(Vector3.New(0, 0, -90 - self.betRollList[self.CurrentChipIndex]), 0.1);
    self.chipPoint:DOLocalRotate(Vector3.New(0, 0, self.betRollList[self.CurrentChipIndex]), 0.1);
end

function LGDDYEntry.StartGameCall()
    --开始游戏
    if self.isFreeGame or self.isSmallGame then
        return ;
    end
    if self.isAutoGame then
        self.OnClickAutoCall();
        return ;
    end
    if self.isRoll then
        --TODO 急停
        --self.AutoContent.gameObject:SetActive(false);
        --self.FreeContent.gameObject:SetActive(false);
        --self.startState.gameObject:SetActive(true);
        --self.stopState.gameObject:SetActive(false);
        --LGDDY_Roll.StopRoll();
        return ;
    end
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    self.AutoContent.gameObject:SetActive(false);
    self.FreeContent.gameObject:SetActive(false);
    self.startState.gameObject:SetActive(false);
    self.stopState.gameObject:SetActive(true);
    LGDDYEntry.addChipBtn.interactable = false;
    LGDDYEntry.reduceChipBtn.interactable = false;
    LGDDYEntry.MaxChipBtn.interactable = false;
    LGDDYEntry.startBtn.interactable = false;
    --TODO 发送开始消息,等待结果开始转动
    LGDDY_Network.StartGame()
    ;
end
function LGDDYEntry.ClickMenuCall()
    --点击显示菜单
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    self.settingPanel.gameObject:SetActive(true);
    local settingCloseBtn = self.settingPanel:Find("Content/Close"):GetComponent("Button");
    settingCloseBtn.onClick:RemoveAllListeners();
    settingCloseBtn.onClick:AddListener(function()
        LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
        self.settingPanel.gameObject:SetActive(false);
    end);
    local musicprogress = self.settingPanel:Find("Content/Music"):GetComponent("Slider");
    local soundprogress = self.settingPanel:Find("Content/Sound"):GetComponent("Slider");
    if not PlayerPrefs.HasKey("MusicValue") then
        PlayerPrefs.SetString("MusicValue", "1");
    end
    if not PlayerPrefs.HasKey("SoundValue") then
        PlayerPrefs.SetString("SoundValue", "1");
    end
    local musicvalue = tonumber(PlayerPrefs.GetString("MusicValue"));
    local soundvalue = tonumber(PlayerPrefs.GetString("SoundValue"));
    if AllSetGameInfo._5IsPlayAudio then
        musicprogress.value = musicvalue;
    else
        musicprogress.value = 0;
    end

    if AllSetGameInfo._6IsPlayEffect then
        soundprogress.value = soundvalue;
    else
        soundprogress.value = 0;
    end
    self.luaBehaviour:AddSliderEvent(musicprogress.gameObject, function(value)
        PlayerPrefs.SetString("MusicValue", tostring(value));
        if value <= 0 then
            AllSetGameInfo._5IsPlayAudio = false;
        else
            AllSetGameInfo._5IsPlayAudio = true;
        end
        Util.Write("IsPlayAudio", tostring(AllSetGameInfo._5IsPlayAudio));
        PlayerPrefs.SetString("IsPlayAudio", tostring(AllSetGameInfo._5IsPlayAudio));
        LGDDY_Audio.pool.volume = value;
        LGDDY_Audio.pool.mute = not AllSetGameInfo._5IsPlayAudio;
        GameManager.SetIsPlayMute(AllSetGameInfo._6IsPlayEffect, AllSetGameInfo._5IsPlayAudio);
    end);
    self.luaBehaviour:AddSliderEvent(soundprogress.gameObject, function(value)
        PlayerPrefs.SetString("SoundValue", tostring(value));
        if value <= 0 then
            AllSetGameInfo._6IsPlayEffect = false;
        else
            AllSetGameInfo._6IsPlayEffect = true;
        end
        Util.Write("isCanPlaySound", tostring(AllSetGameInfo._6IsPlayEffect));
        PlayerPrefs.SetString("isCanPlaySound", tostring(AllSetGameInfo._6IsPlayEffect));
        GameManager.SetIsPlayMute(AllSetGameInfo._6IsPlayEffect, AllSetGameInfo._5IsPlayAudio);
    end);
end
function LGDDYEntry.ResetState()
    self.AutoContent.gameObject:SetActive(false);
    self.FreeContent.gameObject:SetActive(false);
    self.startState.gameObject:SetActive(true);
    self.stopState.gameObject:SetActive(false);
    LGDDYEntry.addChipBtn.interactable = true;
    LGDDYEntry.reduceChipBtn.interactable = true;
    LGDDYEntry.MaxChipBtn.interactable = true;
    LGDDYEntry.startBtn.interactable = true;
    self.isAutoGame = false;
end
function LGDDYEntry.CloseMenuCall()
    --关闭菜单
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if self.menuTweener ~= nil then
        self.menuTweener:Kill();
    end
    self.menuTweener = self.menulist:Find("Content"):DOLocalMove(Vector3.New(0, 1000, 0), 0.5):OnComplete(function()
        if self.menuTweener ~= nil then
            self.menuTweener = nil;
        end
    end);
    if self.menuTweener ~= nil then
        self.menuTweener:SetAutoKill();
    end
end
function LGDDYEntry.CloseGameCall()
    self.quitPanel.gameObject:SetActive(true);
    local backHallBtn = self.quitPanel:Find("Content/BackHall"):GetComponent("Button");
    local closeBtn = self.quitPanel:Find("Content/ContinueGame"):GetComponent("Button");
    local closebtn1 = self.quitPanel:Find("Content/Close"):GetComponent("Button");
    backHallBtn.onClick:RemoveAllListeners();
    backHallBtn.onClick:AddListener(function()
        if not self.isFreeGame and not self.isSmallGame then
            Event.Brocast(MH.Game_LEAVE);
        else
            MessageBox.CreatGeneralTipsPanel("特殊模式中无法离开游戏");
        end
    end);
    closeBtn.onClick:RemoveAllListeners();
    closeBtn.onClick:AddListener(function()
        self.quitPanel.gameObject:SetActive(false);
    end);
    closebtn1.onClick:RemoveAllListeners();
    closebtn1.onClick:AddListener(function()
        self.quitPanel.gameObject:SetActive(false);
    end);
end
function LGDDYEntry.SetMusicCall()
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if AllSetGameInfo._5IsPlayAudio then
        SetInfoSystem.MuteMusic();
        self.musicOn:SetActive(false);
        self.musicOff:SetActive(true);
    else
        SetInfoSystem.PlayMusic();
        self.musicOn:SetActive(true);
        self.musicOff:SetActive(false);
    end
    LGDDY_Audio.pool.mute = not AllSetGameInfo._5IsPlayAudio;
end
function LGDDYEntry.SetSoundCall()
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if AllSetGameInfo._6IsPlayEffect then
        SetInfoSystem.MuteSound();
        self.soundOn:SetActive(false);
        self.soundOff:SetActive(true);
    else
        SetInfoSystem.PlaySound();
        self.soundOn:SetActive(true);
        self.soundOff:SetActive(false);
    end
end

function LGDDYEntry.OnClickAutoCall()
    --点击自动开始
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if self.isAutoGame then
        self.AutoContent.gameObject:SetActive(false);
        self.FreeContent.gameObject:SetActive(false);
        self.startState.gameObject:SetActive(true);
        self.stopState.gameObject:SetActive(false);
        self.StopAutoGame();
        return ;
    end
    self.closeAutoMenu.gameObject:SetActive(true);
end
function LGDDYEntry.OnClickAutoStartCall()
    --点击选择自动次数
    if self.isAutoGame then
        self.StopAutoGame();
        return ;
    end
    self.AutoStartCall();
end
function LGDDYEntry.AutoStartCall()
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    if self.isAutoGame then
        self.StopAutoGame();
        return ;
    end
    self.closeAutoMenu.gameObject:SetActive(true);
end
function LGDDYEntry.OnClickAutoItemCall()
    --点击选择自动次数
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    self.isAutoGame = true;
    self.closeAutoMenu.gameObject:SetActive(false);
    if not self.isFreeGame then
        self.AutoContent.gameObject:SetActive(true);
        self.FreeContent.gameObject:SetActive(false);
        self.startState.gameObject:SetActive(false);
        self.stopState.gameObject:SetActive(false);
    end
    self.MaxChipBtn.interactable = false;
    self.addChipBtn.interactable = false;
    self.reduceChipBtn.interactable = false;
    if self.currentAutoCount > 1000 then
        self.autoText.text = "剩余∞次";
    else
        self.autoText.text = "剩余" .. tostring(self.currentAutoCount) .. "次";
    end
    if not self.isRoll and not self.isFreeGame then
        --没有转动的状态开始自动旋转
        LGDDY_Network.StartGame();
    end
end
function LGDDYEntry.ShowRultPanel()
    --显示规则
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    LGDDY_Rule.ShowRule();
end
function LGDDYEntry.SetMaxChipCall()
    --设置最大下注
    LGDDY_Audio.PlaySound(LGDDY_Audio.SoundList.BTN);
    self.CurrentChipIndex = #self.SceneData.chipList;
    self.CurrentChip = self.SceneData.chipList[self.CurrentChipIndex];
    self.ChipNum.text = self.ShowText(self.CurrentChip * LGDDY_DataConfig.ALLLINECOUNT);
    if self.chipprogressTween ~= nil then
        self.chipprogressTween:Kill();
    end
    self.chipprogressTween = self.chipProgress:DOFillAmount(self.betProgresslist[self.CurrentChipIndex], 0.1);
    self.chipPan:DOLocalRotate(Vector3.New(0, 0, -90 - self.betRollList[self.CurrentChipIndex]), 0.1);
    self.chipPoint:DOLocalRotate(Vector3.New(0, 0, self.betRollList[self.CurrentChipIndex]), 0.1);
end

function LGDDYEntry.CSJLRoll()
    LGDDY_Network.StartGame();
end
function LGDDYEntry.StopAutoGame()
    --停止自动旋转
    self.isAutoGame = false;
    self.currentAutoCount = 0;
end
function LGDDYEntry.FreeGame()
    --免费游戏
    self.isFreeGame = true;
    self.freeText.text = "剩余" .. tostring(self.freeCount + 1) .. "次";
    LGDDY_Network.StartGame();
end
function LGDDYEntry.Roll()
    --开始转动    
    if not self.isFreeGame then
        self.myGold = self.myGold - self.CurrentChip * LGDDY_DataConfig.ALLLINECOUNT;
        LGDDYEntry.SetSelfGold(self.myGold);
    end
    self.WinNum.text = tostring(0);
    LGDDY_Result.HideResult();
    LGDDY_Line.Close();
    LGDDY_Roll.StartRoll();
    self.chipBet:DOLocalMove(Vector3.New(0, -100, 0), 0.5);
end
function LGDDYEntry.OnStop()
    --停止转动
    log("停止")
    LGDDY_Result.ShowResult();
end
function LGDDYEntry.InitPanel()
    --场景消息初始化
    self.myGold = TableUserInfo._7wGold;
    LGDDYEntry.SetSelfGold(self.myGold);
    self.betRollList = {};
    self.betProgresslist = {};
    for i = 1, self.SceneData.chipNum do
        table.insert(self.betRollList, (-90 / (self.SceneData.chipNum - 1)) * (i - 1));
        table.insert(self.betProgresslist, 0.125 + (0.25 / (self.SceneData.chipNum - 1)) * (i - 1));
    end
    for i = 1, #self.SceneData.chipList do
        if self.SceneData.chipList[i] == self.SceneData.bet then
            self.CurrentChipIndex = i;
            self.CurrentChip = self.SceneData.bet;
            self.ChipNum.text = self.ShowText(self.CurrentChip * LGDDY_DataConfig.ALLLINECOUNT);
            if self.chipprogressTween ~= nil then
                self.chipprogressTween:Kill();
            end
            self.chipprogressTween = self.chipProgress:DOFillAmount(self.betProgresslist[self.CurrentChipIndex], 0.1);
            self.chipPan:DOLocalRotate(Vector3.New(0, 0, -90 - self.betRollList[self.CurrentChipIndex]), 0.1);
            self.chipPoint:DOLocalRotate(Vector3.New(0, 0, self.betRollList[self.CurrentChipIndex]), 0.1);
        end
    end
    self.WinNum.text = tostring(0);
    --LGDDY_Caijin.isCanSend = true;
    self.isRoll = false;
    self.isSmallGame = false;
    self.isFreeGame = false;
    self.isAutoGame = false;
    LGDDY_Line.Init();
    LGDDY_Result.Init();
    if self.SceneData.freeNumber > 0 then
        self.isFreeGame = true;
        self.freeCount = self.SceneData.freeNumber;
        self.AutoContent.gameObject:SetActive(false);
        self.FreeContent.gameObject:SetActive(true);
        self.startState.gameObject:SetActive(false);
        self.stopState.gameObject:SetActive(false);
        LGDDY_Result.totalFreeGold = self.SceneData.nFreeGameGold;
        LGDDY_Result.ShowFreeEffect(true);
    elseif self.SceneData.nSmallCount > 0 then
        LGDDY_Result.ShowEnterSmallGameEffect();
    else
        self.AutoContent.gameObject:SetActive(false);
        self.FreeContent.gameObject:SetActive(false);
        self.startState.gameObject:SetActive(true);
        self.stopState.gameObject:SetActive(false);
        LGDDYEntry.startBtn.interactable = true;
    end
end
function LGDDYEntry.FreeRoll()
    --判断是否为免费游戏
    LGDDY_Result.isShow = false;
    if self.isFreeGame then
        self.freeCount = self.freeCount - 1;
        self.currentFreeCount = self.currentFreeCount + 1;
        if self.freeCount < 0 then
            --免费结束,展示免费结算界面
            --LGDDY_Result.ShowFreeResultEffect();
            self.isFreeGame = false;
            self.AutoRoll();
        else
            --还有免费次数
            log("继续免费")
            self.FreeGame();
        end
    else
        self.AutoRoll();
    end
end
function LGDDYEntry.AutoRoll()
    --判断是否为自动游戏
    if self.isAutoGame then
        --如果是自动游戏
        if self.currentAutoCount < 1000 then
            self.currentAutoCount = self.currentAutoCount - 1;
        end
        if self.currentAutoCount < 0 then
            --自动次数使用完了，回到待机状态
            self.AutoContent.gameObject:SetActive(false);
            self.FreeContent.gameObject:SetActive(false);
            self.startState.gameObject:SetActive(true);
            self.stopState.gameObject:SetActive(false);
            LGDDYEntry.addChipBtn.interactable = true;
            LGDDYEntry.reduceChipBtn.interactable = true;
            LGDDYEntry.MaxChipBtn.interactable = true;
            LGDDYEntry.startBtn.interactable = true;
            self.chipBet:DOLocalMove(Vector3.New(0, -15.7, 0), 0.5);
            self.StopAutoGame();
        else
            --还有自动次数
            self.OnClickAutoItemCall();
        end
    else
        --不是自动游戏，直接待机
        self.AutoContent.gameObject:SetActive(false);
        self.FreeContent.gameObject:SetActive(false);
        self.startState.gameObject:SetActive(true);
        self.stopState.gameObject:SetActive(false);
        LGDDYEntry.addChipBtn.interactable = true;
        LGDDYEntry.reduceChipBtn.interactable = true;
        LGDDYEntry.MaxChipBtn.interactable = true;
        LGDDYEntry.startBtn.interactable = true;
        self.chipBet:DOLocalMove(Vector3.New(0, -15.7, 0), 0.5);
        self.StopAutoGame();
    end
end