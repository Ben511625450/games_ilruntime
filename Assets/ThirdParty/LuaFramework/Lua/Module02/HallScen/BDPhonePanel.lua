BDPhonePanel = {}
local self = BDPhonePanel
self.m_luaBeHaviour = nil
self.transform=nil

function BDPhonePanel.Init(_luaBeHaviour)
    if  self.transform==nil then
        self.transform = HallScenPanel.Pool("BDPhonePanel").transform;
        self.transform:SetParent(HallScenPanel.Compose.transform);
        self.transform.localPosition = Vector3.New(0, 0, 0);
        self.transform.localScale = Vector3.New(1, 1, 1);
        self.transform.localScale = Vector3.New(GameManager.ScreenRate, GameManager.ScreenRate, GameManager.ScreenRate);
    end   
    self.m_luaBeHaviour = _luaBeHaviour
    self.RWBDPhonePanelClose=self.transform:Find("mainPanel/CloseBtn")
    self.RWBDPhonePanelMaskClose=self.transform:Find("zhezhao")

    self.BXXMM=self.transform:Find("mainPanel/Image/BXXMM/InputField"):GetComponent("InputField")
    self.SJHM=self.transform:Find("mainPanel/Image/SJHM/InputField"):GetComponent("InputField")
    self.YZM=self.transform:Find("mainPanel/Image/YZM/InputField"):GetComponent("InputField")

    self.HQTZMBtn=self.transform:Find("mainPanel/Image/HQTZMBtn")

    self.HQTZMImage=self.transform:Find("mainPanel/Image/HQTZMBtn/Image")
    self.HQTZMText=self.transform:Find("mainPanel/Image/HQTZMBtn/Text"):GetComponent("Text")

    self.SureBtn=self.transform:Find("mainPanel/SureBtn")

    self.m_luaBeHaviour:AddClick(self.RWBDPhonePanelClose.gameObject, self.RWBDPhoneClose);
    self.m_luaBeHaviour:AddClick(self.RWBDPhonePanelMaskClose.gameObject, self.RWBDPhoneClose);

    self.m_luaBeHaviour:AddClick(self.HQTZMBtn.gameObject, self.HQTZM);
    self.m_luaBeHaviour:AddClick(self.SureBtn.gameObject, self.BindingPhoneNumeberPanel_YesBtnOnClick);

end

function BDPhonePanel.Open(_luaBeHaviour)
    if  self.transform==nil then
        self.Init(_luaBeHaviour)
    else
        self.transform.gameObject:SetActive(true)
    end
    self.BXXMM.text=""
    self.SJHM.text=""
    self.YZM.text=""
    self.HQTZMImage.gameObject:SetActive(true)
    self.HQTZMText.gameObject:SetActive(false)

end
    
function BDPhonePanel.HQTZM(args)
    HallScenPanel.PlayeBtnMusic()

    if self.SJHM.text=="" then
        MessageBox.CreatGeneralTipsPanel("????????????????????????");
        return
    end
    local NewPhoneNumber = self.SJHM:GetComponent('InputField').text;
    if string.len(NewPhoneNumber) ~= 11 then
        MessageBox.CreatGeneralTipsPanel("?????????11???????????????")
        self.HQTZMBtn:GetComponent('Button').interactable = true;
        return ;
    end
    self.HQTZMBtn:GetComponent('Button').interactable = false;
    -- if string.length(SCPlayerInfo._29szPhoneNumber) >= 11 then
    --     MessageBox.CreatGeneralTipsPanel("?????????????????????")
    --     self.HQTZMBtn:GetComponent('Button').interactable = true;
    --     return ;
    -- else
        MoveNotifyInfoClass.getcodetime = 120;
        MoveNotifyInfoClass.getcodefuntion = BDPhonePanel.UpdataNickNamePanel_GetCodeTimeCallBack;
        self.HQTZMText.text = "120s??????";
        self.HQTZMImage.gameObject:SetActive(false)
        self.HQTZMText.gameObject:SetActive(true)
        coroutine.start(MoveNotifyInfoClass.GetCodeTime)
        Event.AddListener(tostring(MH.SUB_3D_SC_DOWN_GAME_RESOURCE), self.UpdataNickNamePanel_GetCodeBtnCallBack);
        local bf = ByteBuffer.New();
        bf:WriteBytes(1, 1);
        bf:WriteBytes(1, 1);
        bf:WriteBytes(DataSize.String12, NewPhoneNumber);
        Network.Send(MH.MDM_3D_LOGIN, MH.SUB_3D_SC_DOWN_GAME_RESOURCE, bf, gameSocketNumber.HallSocket);
   -- end
end

-- ????????????????????????
function BDPhonePanel.UpdataNickNamePanel_GetCodeTimeCallBack()

    self.HQTZMText.text = MoveNotifyInfoClass.getcodetime .. "s??????"
    if MoveNotifyInfoClass.getcodetime <= 0 then
        self.HQTZMImage.gameObject:SetActive(true)
        self.HQTZMText.gameObject:SetActive(false)
        self.HQTZMText.text = ""
        self.HQTZMBtn:GetComponent('Button').interactable = true;
    end
end

-- ????????????????????????????????????
function BDPhonePanel.UpdataNickNamePanel_GetCodeBtnCallBack(data)
    Event.RemoveListener(tostring(MH.SUB_3D_SC_DOWN_GAME_RESOURCE));
    local str = tostring(data[1]);
    if str == "0" then
        MessageBox.CreatGeneralTipsPanel("?????????????????????")
        return ;
    end
end

function BDPhonePanel.BindingPhoneNumeberPanel_YesBtnOnClick()
    HallScenPanel.PlayeBtnMusic()

    -- ??????????????????????????????????????????
    self.SureBtn:GetComponent('Button').interactable = false;
    local NewPhoneNumber = self.SJHM.text;
    local Code = self.YZM.text;
    local BXXMM = self.BXXMM.text;


    if not (PersonalInfoSystem.Phone(NewPhoneNumber)) then
        MessageBox.CreatGeneralTipsPanel("????????????????????????")
        self.SureBtn:GetComponent('Button').interactable = true;
        return
    end ;
    if not (PersonalInfoSystem.PhoneMa(Code)) then
        MessageBox.CreatGeneralTipsPanel("??????????????????????????????")
        self.SureBtn:GetComponent('Button').interactable = true;
        return
    end ;
    if BXXMM=="" then
        MessageBox.CreatGeneralTipsPanel("???????????????")
        self.SureBtn:GetComponent('Button').interactable = true;
        return
    end
    local data = {
        [1] = NewPhoneNumber;
        [2] = Code;
        [3] = MD5Helper.MD5String(BXXMM);
    }
    local buffer = SetC2SInfo(CS_ChangeAccount, data);
    Network.Send(MH.MDM_3D_PERSONAL_INFO, MH.SUB_3D_CS_CHANGE_ACCOUNT, buffer, gameSocketNumber.HallSocket);
    self.SureBtn:GetComponent('Button').interactable = true;        
end

-- ???????????????????????????????????????
function BDPhonePanel.UpdataPhoneNumberCallBack(buffer, size)
    if size == 0 then
        MessageBox.CreatGeneralTipsPanel("????????????");
        MoveNotifyInfoClass.getcodefuntion = nil;
        PersonalInfoSystem.SaveAccount(true);
        self.RWBDPhoneClose();
    else
        MessageBox.CreatGeneralTipsPanel("????????????????????????" .. buffer:ReadString(size));
    end
    -- self.BindingPhoneNumeberPanel_NoBtnOnClick();
end

function BDPhonePanel.RWBDPhoneClose(args)
    if args~=nil then
        HallScenPanel.PlayeBtnMusic()   
    end
    destroy(self.transform.gameObject)
    self.m_luaBeHaviour = nil
    self.transform=nil
end