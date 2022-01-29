using System.Collections;
using System.Collections.Generic;
using Hotfix.Hall;
using LuaFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hotfix
{
    internal class ILGameManager : SingletonILEntity<ILGameManager>
    {
        public static string ScenSeverName; //场景名称
        public static bool startCheckNetHall; //是否链接成功
        public static bool isLogin = false;
        public static bool IsInGame = false;

        public static List<string> LoginList = new List<string>();
        public static int LoginIndex;

        public static bool isOpenBank;

        private string _AccText;
        private string _PwText;
        private string _selfIP;
        private int ipIndex = 0;


        private float rqHeartTimer;
        private readonly float rqHeartTimerMax = 7f;
        public GameObject RootObj { get; private set; }
        private Transform rootContent;

        public Transform HeadIcons { get; set; }
        private GameObject gameSetPanelObj;

        protected override void Awake()
        {
            base.Awake();
            gameObject.AddILComponent<HttpManager>();
            gameObject.AddILComponent<ILMusicManager>();
            LoadScene($"module02", (isdone, apt) =>
            {
                if (!isdone) return;
                RootObj = GameObject.FindGameObjectWithTag(LaunchTag._02hallTag);
                if (RootObj == null) RootObj = GameObject.Find("HallScenPanel");
                if (RootObj == null)
                {
                    DebugHelper.LogError($"没有找到大厅根节点");
                    return;
                }
            
                RootObj.AddILComponent<UIManager>();
                rootContent = RootObj.transform.FindChildDepth($"Content");
            });
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            HotfixActionHelper.OnEnterGame += HotfixActionHelper_OnEnterGame;
            HotfixActionHelper.LeaveGame += HotfixActionHelper_LeaveGame;
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            HotfixActionHelper.OnEnterGame -= HotfixActionHelper_OnEnterGame;
            HotfixActionHelper.LeaveGame -= HotfixActionHelper_LeaveGame;
        }

        private void HotfixActionHelper_LeaveGame()
        {
            GameLocalMode.Instance.IsInGame = false;
            ILMusicManager.Instance.Init();
            rootContent.gameObject.SetActive(true);
            AppFacade.Instance.GetManager<MusicManager>().KillAllSoundEffect();
            AudioSource source = AppFacade.Instance.GetManager<MusicManager>().transform.GetComponent<AudioSource>();
            if (source != null)
            {
                HallExtend.Destroy(source);
            }

            ILMusicManager.Instance.PlayBackgroundMusic();
            DebugHelper.Log($"退出游戏");
            HotfixGameComponent.Instance.Send(DataStruct.UserDataStruct.MDM_GR_USER,
                DataStruct.UserDataStruct.SUB_GR_USER_LEFT_GAME_REQ, new ByteBuffer(), SocketType.Game);
            HotfixGameComponent.Instance.CloseNetwork(SocketType.Game);
            DebugHelper.Log($"退出游戏：{GameLocalMode.Instance.CurrentGame}");
            SceneManager.UnloadSceneAsync(GameLocalMode.Instance.CurrentGame);
            Util.Unload(GameLocalMode.Instance.CurrentGame);
        }

        private void HotfixActionHelper_OnEnterGame()
        {
            GameLocalMode.Instance.IsInGame = true;
            AppFacade.Instance.GetManager<MusicManager>().OnInitialize();
            rootContent.gameObject.SetActive(false);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="callback">加载完成回调</param>
        public void LoadScene(string sceneName, CAction<bool, AsyncOperation> callback)
        {
            Behaviour.StartCoroutine(_LoadScene(sceneName, callback));
        }

        private IEnumerator _LoadScene(string sceneName, CAction<bool, AsyncOperation> callback)
        {
            yield return new WaitForEndOfFrame();
            ResourceManager resMgr = AppFacade.Instance.GetManager<ResourceManager>();
            if (!resMgr.bundles.ContainsKey(sceneName))
            {
                yield return null;
                var filePath = $"{PathHelp.AppHotfixResPath}{sceneName}/{sceneName}.unity3d";
                AssetBundleCreateRequest abcr = MD5Helper.SamplyDESDNAsync(filePath, MD5Helper.FileKey);
                yield return abcr;
                resMgr.bundles.Add(sceneName, abcr.assetBundle);
            }

            var apt = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!apt.isDone)
            {
                yield return new WaitForEndOfFrame();
                callback?.Invoke(false, apt);
            }

            callback?.Invoke(true, null);
        }

        /// <summary>
        /// 发送登录
        /// </summary>
        /// <param name="account">账号信号</param>
        public void SendLoginMasseage(AccountMSG account)
        {
            var headimgurl = "";
            var nickname = "";

            HallStruct.REQ_CS_LOGIN login = new HallStruct.REQ_CS_LOGIN()
            {
                platform = GameLocalMode.Instance.Platform,
                channelID = GameLocalMode.Instance.GameQuDao,
                iD = GameLocalMode.Instance.PlatformID,
                addMultiplyID = (uint) (GameLocalMode.Instance.PlatformID * GameLocalMode.Instance.LoginPlatMultiply +
                                        GameLocalMode.Instance.LoginPlatAdd),
                multiplyID = (ushort) (GameLocalMode.Instance.PlatformID * GameLocalMode.Instance.LoginPlatMultiply),
                addID = (byte) (GameLocalMode.Instance.PlatformID + GameLocalMode.Instance.LoginPlatAdd),
                account = account.account,
                password = account.password,
                mechinaCode = GameLocalMode.Instance.MechinaCode,
                ip = GameLocalMode.Instance.IP,
                headUrl = headimgurl,
                nickName = nickname,
            };
            DebugHelper.Log(LitJson.JsonMapper.ToJson(login));
            HotfixGameComponent.Instance.Send(DataStruct.LoginStruct.MDM_3D_LOGIN,
                DataStruct.LoginStruct.SUB_3D_CS_LOGIN, login.ByteBuffer, SocketType.Hall);
        }

        /// <summary>
        /// 查询自己的金币
        /// </summary>
        public void QuerySelfGold()
        {
            DebugHelper.Log("查询自己的金币");
            var buffer = new ByteBuffer();
            buffer.WriteUInt32(GameLocalMode.Instance.SCPlayerInfo.DwUser_Id); // 玩家ID
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_SELECT_GOLD_MSG, buffer, SocketType.Hall);
        }

        /// <summary>
        /// 获取自己头像
        /// </summary>
        /// <returns></returns>
        public Sprite GetHeadIcon(uint faceId = 0)
        {
            HeadIcons = HeadIcons == null
                ? Util.LoadAsset("module02/Pool/headicons", "HeadIcons").transform
                : HeadIcons;
            uint id = 0;
            if (faceId > 0)
            {
                id = faceId - 1;
                return HeadIcons.transform.GetChild((int) id).GetComponent<Image>().sprite;
            }

            bool isExist = (GameLocalMode.Instance.SCPlayerInfo != null &&
                            GameLocalMode.Instance.SCPlayerInfo.FaceID > 0);
            if(isExist) id = GameLocalMode.Instance.SCPlayerInfo.FaceID - 1;
            return isExist ? HeadIcons.transform.GetChild((int) id).GetComponent<Image>().sprite : null;
        }

        public GameObject GetGameSetPanel(Transform parent)
        {
            string path = $"module02/Pool/gamesetspanel";
            gameSetPanelObj = gameSetPanelObj == null ? Util.LoadAsset(path, "GameSetsPanel") : gameSetPanelObj;
            GameObject go = ToolHelper.Instantiate(gameSetPanelObj);
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.transform.GetComponent<RectTransform>().anchorMax = Vector2.one;
            go.transform.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            go.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            go.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;

            go.transform.AddILComponent<GameSetBtns>();
            return go;
        }
    }
}