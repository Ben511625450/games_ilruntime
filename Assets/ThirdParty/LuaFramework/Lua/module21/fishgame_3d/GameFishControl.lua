local VECTOR3DIS                                        = VECTOR3DIS                                       
local VECTOR3ZERO                                       = VECTOR3ZERO                                      
local VECTOR3ONE                                        = VECTOR3ONE                                       
local COLORNEW                                          = COLORNEW                                         
local QUATERNION_EULER                                  = QUATERNION_EULER                                 
local QUATERNION_LOOKROTATION                           = QUATERNION_LOOKROTATION                          
local C_Quaternion_Zero                                 = C_Quaternion_Zero                                
local C_Vector3_Zero                                    = C_Vector3_Zero                                   
local C_Vector3_One                                     = C_Vector3_One                                    
local C_Color_One                                       = C_Color_One                                      
local V_Vector3_Value                                   = V_Vector3_Value                                  
local V_Color_Value                                     = V_Color_Value                                    
local ImageAnimaClassType                               = ImageAnimaClassType                              
local ImageClassType                                    = ImageClassType                                   
local GAMEOBJECT_NEW                                    = GAMEOBJECT_NEW                                   
local BoxColliderClassType                              = BoxColliderClassType                             
local ParticleSystemClassType                           = ParticleSystemClassType                          
local UTIL_ADDCOMPONENT                                 = UTIL_ADDCOMPONENT                                
local MATH_SQRT                                         = MATH_SQRT                                        
local MATH_SIN                                          = MATH_SIN                                         
local MATH_COS                                          = MATH_COS
local MATH_ATAN                                         = MATH_ATAN
local MATH_TAN                                          = MATH_TAN                                         
local MATH_FLOOR                                        = MATH_FLOOR                                       
local MATH_ABS                                          = MATH_ABS                                         
local MATH_RAD                                          = MATH_RAD                                         
local MATH_RAD2DEG                                      = MATH_RAD2DEG                                     
local MATH_DEG                                          = MATH_DEG                                         
local MATH_DEG2RAD                                      = MATH_DEG2RAD                                     
local MATH_RANDOM                                       = MATH_RANDOM  
local MATH_PI                                           = MATH_PI                                     

local G_GlobalGame_EventID                              = G_GlobalGame_EventID                             
local G_GlobalGame_KeyValue                             = G_GlobalGame_KeyValue                            
local G_GlobalGame_GameConfig                           = G_GlobalGame_GameConfig                          
local G_GlobalGame_GameConfig_FishInfo                  = G_GlobalGame_GameConfig_FishInfo                 
local G_GlobalGame_GameConfig_Bullet                    = G_GlobalGame_GameConfig_Bullet                   
local G_GlobalGame_GameConfig_SceneConfig               = G_GlobalGame_GameConfig_SceneConfig              
local G_GlobalGame_GameConfig_AnimaStyleInfo            = G_GlobalGame_GameConfig_AnimaStyleInfo           
local G_GlobalGame_Enum_FishType                        = G_GlobalGame_Enum_FishType                       
local G_GlobalGame_Enum_FISH_Effect                     = G_GlobalGame_Enum_FISH_Effect                    
local G_GlobalGame_Enum_GoldType                        = G_GlobalGame_Enum_GoldType                       
local G_GlobalGame_Enum_EffectType                      = G_GlobalGame_Enum_EffectType                     
local G_GlobalGame_SoundDefine                          = G_GlobalGame_SoundDefine                         
local G_GlobalGame_ConstDefine                          = G_GlobalGame_ConstDefine                         
local G_GlobalGame_FunctionsLib                         = G_GlobalGame_FunctionsLib                        
local G_GlobalGame_FunctionsLib_FUNC_GetPrefabName      = G_GlobalGame_FunctionsLib_FUNC_GetPrefabName     
local G_GlobalGame_FunctionsLib_FUNC_CacheGO            = G_GlobalGame_FunctionsLib_FUNC_CacheGO           
local G_GlobalGame_FunctionsLib_FUNC_AddAnimate         = G_GlobalGame_FunctionsLib_FUNC_AddAnimate        
local G_GlobalGame_FunctionsLib_FUNC_GetFishPrefabName  = G_GlobalGame_FunctionsLib_FUNC_GetFishPrefabName 
local G_GlobalGame_FunctionsLib_FUNC_GetFishStyleInfo   = G_GlobalGame_FunctionsLib_FUNC_GetFishStyleInfo  
local G_GlobalGame_FunctionsLib_GetFishIDByGameObject   = G_GlobalGame_FunctionsLib_GetFishIDByGameObject  
local G_GlobalGame_FunctionsLib_FUNC_GetEulerAngle      = G_GlobalGame_FunctionsLib_FUNC_GetEulerAngle 
local G_GlobalGame_FunctionsLib_CreateFishName          = G_GlobalGame_FunctionsLib_CreateFishName



local CFish = GameRequire("Fish");
local CREATE_FISH = CFish.Create;
local CFishGroup = GameRequire("FishGroup");
--local _CGameFishControl = class("_CGameFishControl");
local _CGameFishControl = class();

function _CGameFishControl:ctor(_sceneControl)
    self._fishMap = {
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                };
    self._fishInScreenMap = {
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,};
--    self._fishMap = {};
--    self._fishInScreenMap = {};
    self._screenFishCount = 0;
    
    --?????????
    self._cacheFishMap = {};
    for i=G_GlobalGame_Enum_FishType.FISH_KIND_1,G_GlobalGame_Enum_FishType.FISH_KIND_COUNT  do
        self._cacheFishMap[i] = {
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                nil,nil,nil,nil,nil,nil,nil,nil,nil,
                };
        --self._cacheFishMap[i] = {};
    end
    
    self._isRefreshFish = true;
    self._sceneControl  = _sceneControl;
    self._isPauseFish   = false;

    --??????
    self._fishGroupMap = {nil,nil,nil};
    --self._fishGroupMap = {};

    --????????????
    self._fishFalseDieMap = {
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,
            nil,nil,nil,nil,nil,nil,nil,nil,nil,};
    --self._fishFalseDieMap = {};

    --update ??????????????????
    self._updateIndex     = 0;
    self._maxUpdateCount  = 2;
    self._updateRecords   = {0,0,0,0};
--    self.fishCount = 0;
end

function _CGameFishControl:Init(transform)
    self.transform  =  transform;

    --????????????
    G_GlobalGame:RegEvent(G_GlobalGame_EventID.FishHurt,self,self.OnEventFishHurt);

    --???????????????
    G_GlobalGame:SetKeyHandler(G_GlobalGame_KeyValue.GetFishById,handler(self,self.OnKVGetFish));
    G_GlobalGame:SetKeyHandler(G_GlobalGame_KeyValue.CreateUIFish,handler(self,self.OnKVCreateUIFish));
end

--????????? ?????????
function _CGameFishControl:OnKVGetFish(_fishId)
    return self:GetFish(_fishId);
end

--??????????????????
function _CGameFishControl:_isCanCreateFish()
    if self._isRefreshFish and not self._isPauseFish then
        return  true;
    end
    return false;
end

--?????????
function _CGameFishControl:CreateFish(_type,_id,_sendData,_flag)
	--logError("?????????????????????--------------------------------------------")
    _flag = _flag or G_GlobalGame.Enum_FISH_FLAG.Common_Fish;
    if not self:_isCanCreateFish() then
        if _flag==G_GlobalGame.Enum_FISH_FLAG.Common_Fish then
            return ;
        end
    end
    local vec = self._cacheFishMap[_type];
    local fish;
    local _handler = function(_sendData,fish)

        fish:SetFishFlag(_flag);
		
        fish:SetFishID(_id);

        fish:RegEvent(self,self.OnEventFish);

        self._fishMap[_id]=fish;
        if _flag==G_GlobalGame.Enum_FISH_FLAG.YC_Fish then

        else

            fish:SetParentBySame(self.transform);


	
            fish:Init(_sendData);

            if not self:_isCanCreateFish() then

                --??????????????????????????????
                fish:Disappear();
            end   
    
        end
		
        _sendData = nil;
    end
    if #vec<1 then
    --if true then
        local initData = clone(_sendData);
--        self.fishCount =  self.fishCount + 1;
        fish = CREATE_FISH(_type,handler(initData,_handler),self.transform);
        _sendData = nil;
--        error("New create:" .. self.fishCount);
    else
        fish = table.remove(vec);
        _handler(_sendData,fish);
    end
	--if fish~=nil then logError("???????????????") else logError("???????????????") end 
    return fish;
end


local UI_FishName = "FishName_";

--?????????
function _CGameFishControl:OnKVCreateUIFish(_type)
    local _flag = G_GlobalGame.Enum_FISH_FLAG.Common_Fish;
    local fish = CREATE_FISH(_type);
    fish:SetFishFlag(_flag);
    fish:SetName(UI_FishName .. _type);
    return fish;
end

--????????????
function _CGameFishControl:CreateFishGroup(_fishGroupData)
    local fishGroup = CFishGroup.Create(self.transform,handler(self,self.CreateFish),_fishGroupData)
    fishGroup:RegEvent(self,self.OnEventFishGroup);
    self._fishGroupMap[fishGroup:LID()] = fishGroup;
    return fishGroup;
end

--???????????????
function _CGameFishControl:StopRefreshFish()
    self._isRefreshFish = false;
end

--???????????????
function _CGameFishControl:StartRefreshFish()
    self._isRefreshFish = true;
end

function _CGameFishControl:Pause()
    self._isPauseFish = true;
end

function _CGameFishControl:Resume()
    self._isPauseFish = false;
end

--????????????
function _CGameFishControl:Update(_dt)
    --??????????????????
    local FishMap = self._fishFalseDieMap;
    for id,v in pairs(FishMap) do
        if not v:IsFalseDie() then
            self:_cacheFish(v);
            self._fishFalseDieMap[id] = nil;
        end
    end
    --????????????????????????
    local x,y,x2,y2 = self._sceneControl:GetSceneRectInWorld(true);
    FishMap = self._fishMap;
    local isPauseFish = self._isPauseFish;
    for id,v in pairs(FishMap) do
        v:UpdateEx(_dt,isPauseFish,x,y,x2,y2);
    end
    --??????
    FishMap = self._fishGroupMap;
    for id,v in pairs(FishMap) do
        if v:IsRealStart() then
            v:Update(_dt);
        else 
            --????????????
            v:AsyncUpdate(_dt);
        end
    end
end

--?????????ID?????????
function _CGameFishControl:GetFish(_fishId)
    return self._fishMap[_fishId];
end

--?????????ID???????????????
function _CGameFishControl:GetFalseDieFish(_fishId)
    local fish = self._fishFalseDieMap[_fishId];
    if fish==nil then
        --???????????????????????????????????????????????????????????????????????????????????????
        fish = self:GetFish(_fishId);
        if fish:IsFalseDie() then
            return fish;
        end
    end
    return fish;
end

--??????????????????
function _CGameFishControl:OnEventFish(_eventId,_fish)
    if _eventId == G_GlobalGame_EventID.FishDie then
        self:RemoveFish(_fish);
    elseif _eventId == G_GlobalGame_EventID.FishDead then
        if self._fishInScreenMap[_fish:FishID()] then
            self._screenFishCount =  self._screenFishCount - 1;
            self._fishInScreenMap[_fish:FishID()] = nil;
        end
    elseif _eventId == G_GlobalGame_EventID.BeforeFishEnterScreen then
        self._fishInScreenMap[_fish:FishID()]=_fish;
        self._screenFishCount =  self._screenFishCount + 1;
    elseif _eventId == G_GlobalGame_EventID.BeforeFishLeaveScreen then
        self._fishInScreenMap[_fish:FishID()]=nil;
        self._screenFishCount =  self._screenFishCount - 1;
    end
end


function _CGameFishControl:Remove(id)
    --[[
    local fishId = fish._fishId;
    local fishType= fish._fishType;
    self._fishMap[fishId] = nil;
    if fish:IsFalseDie() then
        --?????????
        fish:Hide();
        --??????
        --???????????????
        self._fishFalseDieMap[fishId] = fish;
    else
        self:_cacheFish(fish);
    end
    --]]
end

function _CGameFishControl:_cacheFish(fish)
    local cacheVec= self._cacheFishMap[fish._fishType];
    if fish.fishConfig.cacheCount<#cacheVec then
        fish:Destroy();
--        self.fishCount = self.fishCount -1;
    else
        fish:Cache(); 
        fish:Display();
        cacheVec[#cacheVec + 1] = fish;
    end
--    fish:Cache();
end

function _CGameFishControl:RemoveFish(fish)
    local fishId = fish._fishId;
    local fishType= fish._fishType;
    self._fishMap[fishId] = nil;
    if self._fishInScreenMap[fishId] then
        self._screenFishCount =  self._screenFishCount - 1;
        self._fishInScreenMap[fishId] = nil;
    end
    if fish:IsFalseDie() then
        --?????????
        fish:Hide();
        --??????
        --???????????????
        self._fishFalseDieMap[fishId] = fish;
    else
        self:_cacheFish(fish);
    end
end

--??????????????????
function _CGameFishControl:OnEventFishGroup(_eventId,_fishGroup)
    if _eventId == G_GlobalGame_EventID.NotifyFishGroupOver then
        --??????????????????
        self:ClearFishes();
        --????????????
        self._fishGroupMap[_fishGroup:LID()]=nil;
        --????????????
        self:StartRefreshFish();
    end
end

--????????????
function _CGameFishControl:ClearAll()
    self._fishInScreenMap = {};
    self._screenFishCount = 0;
    self._fishMap = {};
end

--???????????????
function _CGameFishControl:ClearFishes()
    local FishMap = self._fishFalseDieMap;
    local cacheVec;
    CachFishMap = self._cacheFishMap;
    for id,v in pairs(FishMap) do
        self:_cacheFish(v);
        FishMap[id] = nil;
    end
    --self._fishFalseDieMap = {};

    FishMap = self._fishMap;
    
    for id,v in pairs(FishMap) do
        self:_cacheFish(v);
        FishMap[id] = nil;
        self._fishInScreenMap[id] = nil;
    end
    --self._fishMap = {};
    --self._fishInScreenMap = {};
    self._screenFishCount = 0;

    FishMap = self._fishGroupMap;
    for id,v in pairs(FishMap) do
        v:Destroy();
        FishMap[id] = nil;
    end
    --self._fishGroupMap = {};
    return vec;
end

--??????????????????
function _CGameFishControl:ClearFishesByFlag(_flag)
    local FishMap = self._fishMap;
    local cacheVec;
    CachFishMap = self._cacheFishMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() and v:IsFlag(_flag) then
            v:Disappear();
        end
    end
--    return vec;
end

--???????????????????????????
function _CGameFishControl:GetFishesInScreen()
    local vec = {};
    local count=0;
    local FishMap = self._fishInScreenMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() then
            count = count + 1;
            vec[count] = v;
        end
    end
    return vec;
end

--??????????????????????????? ??????????????????
function _CGameFishControl:GetFishesInScreenExceptFish(_fish)
    local vec = {};
    local count=0;
    local FishMap = self._fishInScreenMap;
    local fishId = _fish._fishId;
    for id,v in pairs(FishMap) do
        if not v:IsDie() and fishId~=v._fishId then
            count = count + 1;
            vec[count] = v;
        end
    end
    return vec;
end

--???????????????????????????
function _CGameFishControl:ClearFishesInScreen()
    local FishMap = self._fishInScreenMap;
    for id,v in pairs(FishMap) do
        v:Disappear();
    end
end

--?????????????????????????????????
function _CGameFishControl:GetFishesInScreenWithFishType(_fishType)
    local vec = {};
    local count = 0;
    local FishMap = self._fishInScreenMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() and v._fishType ==  _fishType then
            count = count + 1;
            vec[count] = v;
        end
    end
    return vec;
end

--?????????????????????????????????
function _CGameFishControl:ClearFishesInScreenWithFishType(_fishType)
    local FishMap = self._fishInScreenMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() and v._fishType ==  _fishType then
            v:Disappear();
        end
    end
end

--??????????????? ?????????????????????????????? _distance
function _CGameFishControl:GetFishesInScreenInnerRound(_fish,_distance)
    local position1 = G_GlobalGame:SwitchWorldPosToWorldPosBy3DCamera(_fish:Position(),2);
    local vec = {};
    local count = 0;
    local FishMap = self._fishInScreenMap;
    local _fishId = _fish._fishId;
    local position2;
    for id,v in pairs(FishMap) do
        if v._fishId ~= _fishId then
            if not v:IsDie() then
                position2 = G_GlobalGame:SwitchWorldPosToWorldPosBy3DCamera(v:Position(),2);
                if VECTOR3DIS(position1,position2)<_distance then
                    count = count + 1;
                    vec[count] = v;
                end
            end
        end
    end
    return vec;
end

--???????????? ?????????????????????????????? _distance
function _CGameFishControl:GetFishesInWorldRound(_fish,_distance)
    local position1 = G_GlobalGame:SwitchWorldPosToWorldPosBy3DCamera(_fish:Position(),20);
    local vec = {};
    local count = 0;
    local FishMap = self._fishInScreenMap;
    local _fishId = _fish._fishId;
    local position2;
    for id,v in pairs(FishMap) do
        if v._fishId ~= _fishId then
            if not v:IsDie() then
                position2 = G_GlobalGame:SwitchWorldPosToWorldPosBy3DCamera(v:Position(),20);
                if VECTOR3DIS(position1,position2)<_distance then
                    count = count + 1;
                    vec[count] = v;
                end
            end
        end
    end
    return vec;
end

--?????????????????????
function _CGameFishControl:OnEventFishHurt(_eventId,_eventData)
    if _eventId == G_GlobalGame_EventID.FishHurt then
        if _eventData~=nil then
            local fish =self:GetFish(_eventData.fishId);
            if fish~=nil then
                local pos = G_GlobalGame:SwitchWorldPosToWorldPosBy3DCamera(_eventData.pos,fish:Position().z-0.5);
                fish:Hurt(pos,_eventData.isOwner);
            end
        end
    end
end

--????????????
function _CGameFishControl:OnEventHitLK(_eventId,_eventData)
    if _eventId == G_GlobalGame_EventID.HitLK then
        if _eventData~=nil then
            local fish =self:GetFish(_eventData.fishId);
            if fish~=nil then
                fish:SetMultiple(_eventData.fishMultiple);
            end
        end
    end
end

--????????????????????????????????? ??????nil??????????????????????????????
function _CGameFishControl:GetNextLockFish(_fish)
    local isStart = false;
    if not _fish then
        --???????????????
        isStart = true;
    end
    local _fishId = _fish._fishId;
    local FishMap = self._fishInScreenMap;
    for id,v in pairs(FishMap) do
        if isStart then
            if not v:IsDie() and G_GlobalGame_FunctionsLib.Mod_Fish_IsCanBeLock(v._fishType) then
                return v;
            end
        else
            if v._fishId == _fishId then
                isStart = true;
            end
        end
    end

    for id,v in pairs(FishMap) do
        if not v:IsDie() and G_GlobalGame_FunctionsLib.Mod_Fish_IsCanBeLock(v._fishType) then
            return v;
        end
    end
    return nil;
end

--????????????????????????
function _CGameFishControl:FishQuickMoveOutScreen()
    local FishMap = self._fishMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() then
            v:QuickMove();
        end
    end
end

--?????????
function _CGameFishControl:FishCount()
    local count=0;
    local FishMap = self._fishMap;
    for id,v in pairs(FishMap) do
        if not v:IsDie() then
            count = count+1;
        end
    end
    return count;
end

--?????????????????????
function _CGameFishControl:ScreenFishCount()
    return self._screenFishCount;
end

return _CGameFishControl;