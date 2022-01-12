JZSF_Roll = {}

local self = JZSF_Roll;
self.rollList = {};
self.rollIndex = 0;
self.StopIndex = 0;
self.currentState = 0;--0:待机，1：正常转动,2:停止
self.timer = 0;
self.startTimer = 0;
self.stopTimer = 0;
self.isstop = false;
self.scatterCount = 0;
self.addSpeedEffectIndex = 0;
self.scatter = true;
function JZSF_Roll.Init(obj)
    --初始化转动
    self.rollList = {};
    self.currentState = 0;
    self.isstop = false;
    self.scatterCount = 0;
    JZSFEntry.ScatterList = {};
    for i = 1, obj.transform.childCount do
        local rect = obj.transform:GetChild(i - 1):GetComponent("ScrollRect");
        local temp1 = newObject(rect.content:GetChild(0):GetChild(0).gameObject);
        temp1.transform:SetParent(rect.content:GetChild(0));
        temp1.transform.localPosition = Vector3.New(0, 170, 0);
        temp1.transform.localRotation = Quaternion.identity;
        temp1.transform.localScale = Vector3.New(1, 1, 1);
        temp1.gameObject.name = "Temp";
        local temp2 = newObject(rect.content:GetChild(0):GetChild(0).gameObject);
        temp2.transform:SetParent(rect.content:GetChild(rect.content.childCount - 1));
        temp2.transform.localPosition = Vector3.New(0, -170, 0);
        temp2.transform.localRotation = Quaternion.identity;
        temp2.transform.localScale = Vector3.New(1, 1, 1);
        temp2.gameObject.name = "Temp";
        rect.verticalNormalizedPosition = 0;
        rect.elasticity = JZSF_DataConfig.rollReboundRate;
        rect.content.transform:GetComponent("GridLayoutGroup").enabled=false

        table.insert(self.rollList, rect);
    end
    for i=1,obj.transform.childCount do
        self.ChangeIconRoll(i)
    end

end
function JZSF_Roll.StartRoll()
    JZSF_Audio.PlaySound(JZSF_Audio.SoundList.StartRoll);

    self.rollIndex = 0;
    self.StopIndex = 0;
    self.timer = JZSF_DataConfig.rollInterval;
    self.stopTimer = 0;
    self.startTimer = 0;
    self.currentState = 1;
    self.scatterCount = 0;
    self.scatter = true;
    self.isstop = false;
    JZSFEntry.ScatterList = {};
    log("开始转动")
end
function JZSF_Roll.StopRoll()
    if self.isstop then
        return ;
    end
    if self.rollIndex == #self.rollList then
        self.currentState = 2;
        self.isstop = true;
    end
end
function JZSF_Roll.Update()
    if self.currentState == 0 then
        --待机状态
        return ;
    elseif self.currentState == 1 then
        --正常旋转
        for i = 1, self.rollIndex do
            self.rollList[i].verticalNormalizedPosition = self.rollList[i].verticalNormalizedPosition + Time.deltaTime * JZSF_DataConfig.rollSpeed;--旋转
            if self.rollList[i].verticalNormalizedPosition >= 1 then
                self.rollList[i].verticalNormalizedPosition = 0;
                self.ChangeIconRoll(i);
            end
        end

        if self.rollIndex < #self.rollList then
            --计算转动间隔
            self.timer = self.timer + Time.deltaTime;
            if self.timer >= 0 then
                self.timer = 0;
                self.rollIndex = self.rollIndex + 1;
                --TODO 赋值转动icon
                self.ChangeIconRoll(self.rollIndex);
            end
        end
        if self.startTimer <= JZSF_DataConfig.rollTime then
            --计算旋转时间，时间到就停止
            self.startTimer = self.startTimer + Time.deltaTime;
            if self.startTimer >= JZSF_DataConfig.rollTime then
                self.currentState = 2;
            end
        end
    elseif self.currentState == 2 then
        for i = 1, self.rollIndex do
            if self.StopIndex < i then
                self.rollList[i].verticalNormalizedPosition = self.rollList[i].verticalNormalizedPosition + Time.deltaTime * JZSF_DataConfig.rollSpeed;--旋转
                if self.rollList[i].verticalNormalizedPosition >= 1 then
                    self.rollList[i].verticalNormalizedPosition = 0;
                    self.ChangeIconRoll(i);
                end
            end
        end
        if self.StopIndex < #self.rollList then
            --计算转动间隔
            self.stopTimer = self.stopTimer + Time.deltaTime;
            if self.stopTimer >= JZSF_DataConfig.rollInterval then
                if self.addSpeedEffectIndex > 0 then
                    if self.stopTimer < JZSF_DataConfig.addSpeedTime then
                        return ;
                    end
                end
                self.stopTimer = 0;
                self.StopIndex = self.StopIndex + 1;
                --TODO 换正式结果图片
                local hasScatter = self.ChangeResultIcon(self.StopIndex);
                self.rollList[self.StopIndex].verticalNormalizedPosition = JZSF_DataConfig.rollDistance;
                self.AddSpeed();
                coroutine.start(function(stopIndex)
                    coroutine.wait(0.3)
                    self.SingleRollStop(stopIndex);
                    if stopIndex == #self.rollList then
                        self.currentState = 0;
                        coroutine.wait(0.7);
                        JZSFEntry.OnStop();
                    end
                end, self.StopIndex);
            end
        end
    end
end
function JZSF_Roll.AddSpeed()
    if self.scatterCount >= 2 then
        self.addSpeedEffectIndex = self.StopIndex;
        if self.addSpeedEffectIndex >= #self.rollList or not self.scatter then
            self.addSpeedEffectIndex = 0;
            return ;
        end
        --TODO 播放加速音效,延时销毁
        --JZSF_Audio.PlaySound(JZSF_Audio.SoundList.Speed, JZSF_DataConfig.addSpeedTime);
    end
end
function JZSF_Roll.SingleRollStop(stopIndex)
    --TODO 音效
    for i=1,3 do
        JZSF_Audio.PlaySound(JZSF_Audio.SoundList.RS);
    end
    for i=1,3 do
        if stopIndex==5 then
            JZSF_Audio.PlaySound(JZSF_Audio.SoundList.RS5); 
        end
    end

    -- if hasScatter then
    --     JZSF_Audio.PlaySound("Scatter" .. self.scatterCount);
    -- end
end
function JZSF_Roll.ChangeIconRoll(index)
    local iconParent = self.rollList[index].content;
    for i = 0, iconParent.childCount - 1 do
        local iconIndex = math.random(1, 12);
        local changeIcon = JZSFEntry.icons:Find(JZSF_DataConfig.IconTable[iconIndex]):GetComponent("Image").sprite;
        iconParent:GetChild(i):Find("Icon"):GetComponent("Image").sprite = changeIcon;
        local tempIcon = iconParent:GetChild(i):Find("Temp");
        if tempIcon ~= nil then
            tempIcon:GetComponent("Image").sprite = changeIcon;
            tempIcon:GetComponent("Image"):SetNativeSize();
        end
        iconParent:GetChild(i):Find("Icon"):GetComponent("Image"):SetNativeSize();
    end
end
function JZSF_Roll.ChangeResultIcon(index)
    local iconParent = self.rollList[index].content;
    local hasScatter = false;
    for i = 0, iconParent.childCount - 1 do
        local img = iconParent:GetChild(i):Find("Icon"):GetComponent("Image");
        local iconIndex = 1;
        if i == iconParent.childCount - 1 then
            iconIndex = math.random(1, 12);
        else
            iconIndex = JZSFEntry.ResultData.ImgTable[i * 5 + index];
            if iconIndex == 12 then
                if self.scatter then
                    if index == 1 then
                        hasScatter = true;
                        self.scatterCount = self.scatterCount + 1;
                        table.insert(JZSFEntry.ScatterList, img);
                    else
                        if self.scatterCount == index - 1 then
                            hasScatter = true;
                            self.scatterCount = self.scatterCount + 1;
                            table.insert(JZSFEntry.ScatterList, img);
                        end
                    end
                end
            elseif iconIndex == 13 then
                if JZSFEntry.isFreeGame then
                    iconIndex = 13 + JZSFEntry.CurrentFreeIndex;
                end
            end
        end
        local changeIcon = JZSFEntry.icons:Find(JZSF_DataConfig.IconTable[iconIndex]):GetComponent("Image").sprite;
        img.sprite = changeIcon;
        if i ~= iconParent.childCount - 1 then
            iconParent:GetChild(i).gameObject.name = JZSF_DataConfig.IconTable[iconIndex];
        end
        img:SetNativeSize();
        local tempIcon = iconParent:GetChild(i):Find("Temp");
        if tempIcon ~= nil then
            iconIndex = math.random(1, 12);
            changeIcon = JZSFEntry.icons:Find(JZSF_DataConfig.IconTable[iconIndex]):GetComponent("Image").sprite;
            tempIcon:GetComponent("Image").sprite = changeIcon;
            tempIcon:GetComponent("Image"):SetNativeSize();
        end
    end
    self.scatter = hasScatter;
    return hasScatter;
end