using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaFramework;
using System.IO;

namespace Hotfix.Hall
{
    internal class DownLoadGamePanel : PanelBase
    {
        HallStruct.RoomInfo info;

        Button closeBtn;
        Text downloadValue;
        Text downloadDesc;
        Slider downProgress;

        GameObject downObj;

        private List<IState> states;
        private HierarchicalStateMachine hsm;

        public DownLoadGamePanel() : base(UIType.Top, nameof(DownLoadGamePanel))
        {
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            if (args.Length <= 0) return;
            info = (HallStruct.RoomInfo)args[0];
            Init();
        }

        protected override void Start()
        {
            base.Start();
            states = new List<IState>();
            hsm = new HierarchicalStateMachine(false, gameObject);
            states.Add(new IdleState(this, hsm));
            states.Add(new ConnectState(this, hsm));
            states.Add(new LoadGameState(this, hsm));
            states.Add(new DownloadGameState(this, hsm));
            hsm.Init(states, nameof(IdleState));
        }

        protected override void Update()
        {
            base.Update();
            hsm?.Update();
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            closeBtn = this.transform.FindChildDepth<Button>("Content/Close");
            downloadValue = this.transform.FindChildDepth<Text>("Content/ProgressValue");
            downloadDesc = this.transform.FindChildDepth<Text>("Content/Desc");
            downProgress = this.transform.FindChildDepth<Slider>("Content/Progress");
        }

        protected override void AddListener()
        {
            base.AddListener();
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(() =>
            {
                if (downObj != null)
                {
                    HallExtend.Destroy(downObj);
                }
                HotfixGameComponent.Instance.CloseNetwork(SocketType.Game);
                ILMusicManager.Instance.PlayBtnSound();
                UIManager.Instance.Close();
            });
        }

        private void Init()
        {
            hsm?.ChangeState(nameof(ConnectState));
        }

        private class IdleState : State<DownLoadGamePanel>
        {
            public IdleState(DownLoadGamePanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
        }
        /// <summary>
        /// 连接服务器
        /// </summary>
        private class ConnectState : State<DownLoadGamePanel>
        {
            public ConnectState(DownLoadGamePanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                GameLocalMode.Instance.SCPlayerInfo.ReconnectGameID = 0;
                GameLocalMode.Instance.SCPlayerInfo.ReconnectFloorID = 0;
                owner.downloadDesc.text = $"正在连接……";
                owner.downloadValue.text = $"0%";
                owner.downProgress.value = 0;
                GameLocalMode.Instance.GameHost = owner.info._4dwServerAddr;
                GameLocalMode.Instance.GamePort = owner.info._5wServerPort;
                if (GameLocalMode.Instance.GWData.isUseLoginIP)
                {
                    if (GameLocalMode.Instance.GWData.isUseDefence)
                    {
                        GameLocalMode.Instance.GameHost = Util.isPc ? GameLocalMode.Instance.GWData.DefenceGameIP : GameLocalMode.Instance.GWData.DefencePCGameIP;
                    }
                    else
                    {
                        GameLocalMode.Instance.HallHost = Util.isPc ? GameLocalMode.Instance.GWData.PCGameIP
                            : GameLocalMode.Instance.GWData.GameIP;
                    }
                }
                else
                {
                    if (GameLocalMode.Instance.GWData.isUseDefence)
                    {
                        GameLocalMode.Instance.HallHost = Util.isPc ? GameLocalMode.Instance.GWData.DefenceGameIP : GameLocalMode.Instance.GWData.DefencePCGameIP;
                    }
                }
                HotfixGameComponent.Instance.ConnectGameServer(isSuccess =>
                {
                    if (isSuccess)
                    {
                        InitLua();
                        hsm?.ChangeState(nameof(DownloadGameState));
                        return;
                    }
                    hsm?.ChangeState(nameof(IdleState));
                    UIManager.Instance.Close();
                    ToolHelper.PopBigWindow(new BigMessage()
                    {
                        content = $"连接服务器失败"
                    });
                });
            }

            private void InitLua()
            {
                GameData data = GameConfig.GetGameData(owner.info._2wGameID);
                if (data.configer.driveType != ScriptType.ILRuntime)
                {
                    AppFacade.Instance.GetManager<LuaManager>().DoFile(data.configer.luaPath.Replace('.', '/'));
                }
            }
        }
        /// <summary>
        /// 加载游戏
        /// </summary>
        private class LoadGameState : State<DownLoadGamePanel>
        {
            public LoadGameState(DownLoadGamePanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            GameData data;
            public override void OnEnter()
            {
                base.OnEnter();
                owner.closeBtn.gameObject.SetActive(false);
                owner.downloadDesc.text = $"正在加载游戏……";
                data = GameConfig.GetGameData(owner.info._2wGameID);
                ILGameManager.Instance.LoadScene(data.scenName, (isdone,apt) =>
                 {
                     if (!isdone)
                     {
                         owner.downProgress.value = apt.progress;
                         owner.downloadValue.text = $"{Mathf.Ceil(apt.progress * 100)}%";
                     }
                     else
                     {
                         owner.downProgress.value = 1;
                         owner.downloadValue.text = $"100%";
                         hsm?.ChangeState(nameof(IdleState));
                         OnLoadComplete();
                     }
                 });
            }

            private void OnLoadComplete()
            {
                UIManager.Instance.Close();
                ILMusicManager.Instance.StopMusic();
                HotfixActionHelper.DispatchOnEnterGame();
                GameLocalMode.Instance.CurrentGame = data.scenName;
                if (data.configer.driveType == ScriptType.Lua)
                {
                    GameObject go = GameObject.FindGameObjectWithTag(LaunchTag._01gameTag);
                    if (go == null)
                    {
                        go = GameObject.Find(data.configer.luaRootName);
                        if (go == null)
                        {
                            DebugHelper.LogError($"没有找到根节点 {data.configer.luaRootName} 0");
                            return;
                        }
                    }
                    else
                    {
                        if (go.name != data.configer.luaRootName)
                        {
                            Transform child = go.transform.FindChildDepth(data.configer.luaRootName);
                            if (child != null)
                            {
                                go = child.gameObject;
                            }
                        }
                    }
                    LuaBehaviour behaviour = go.GetComponent<LuaBehaviour>();
                    if (behaviour == null)
                    {
                        behaviour = go.AddComponent<LuaBehaviour>();
                    }
                }
                else
                {
                    EventHelper.DispatchOnEnterGame(data.scenName);
                }
            }
        }
        /// <summary>
        /// 下载游戏
        /// </summary>
        private class DownloadGameState : State<DownLoadGamePanel>
        {
            public DownloadGameState(DownLoadGamePanel owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            bool isComplete;
            public override void OnEnter()
            {
                base.OnEnter();
                owner.downProgress.value = 0;
                owner.closeBtn.gameObject.SetActive(true);
                isComplete = false;
            }
            public override void Update()
            {
                base.Update();
                if (isComplete) return;
                isComplete = true;
                if (DownLoadFile.IsDown(owner.info._2wGameID))
                {
                    owner.downloadDesc.text = $"正在下载游戏……";
                    owner.downloadValue.text = $"0%";
                    owner.downObj = DownLoadFile.DownloadFileAsync(owner.info._2wGameID, OnDownGameCall);
                }
                else
                {
                    hsm?.ChangeState(nameof(LoadGameState));
                }
            }
            /// <summary>
            /// 下载进度
            /// </summary>
            /// <param name="progress">进度</param>
            /// <param name="t2"></param>
            private void OnDownGameCall(float progress, object t2)
            {
                owner.downProgress.value = progress;
                owner.downloadValue.text = $"{Mathf.Ceil(progress * 100)}%";
                if(progress>=1)
                {
                    hsm?.ChangeState(nameof(LoadGameState));
                }
                else if (progress < 0)
                {
                    owner.downloadDesc.text = $"下载错误";
                }
            }
        }
    }
    public static class DownLoadFile
    {
        /// <summary>
        /// 是否下载游戏
        /// </summary>
        /// <param name="id">游戏id</param>
        /// <returns></returns>
        public static bool IsDown(int id)
        {
            bool isDown = false;
            GameData data = GameConfig.GetGameData(id);
            List<string> files = data.configer.downFiles;
            BaseValueConfigerJson[] configer = AppConst.gameValueConfiger.GetModule(data.scenName);
            for (int i = 0; i < files.Count; i++)
            {
                string path = $"{PathHelp.AppHotfixResPath}{files[i]}";
                if (!File.Exists(path))
                {
                    string dirName = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    isDown = true;
                    break;
                }
                for (int j = 0; j < configer.Length; j++)
                {
                    BaseValueConfigerJson con = configer[j];
                    if (!files[i].ToLower().Equals(con.dirPath.ToLower())) continue;
                    string localMd5 = MD5Helper.MD5File(path);
                    if (!localMd5.Equals(con.md5))
                    {
                        isDown = true;
                        break;
                    }
                }
            }
            return isDown;
        }
        /// <summary>
        /// 下载游戏资源
        /// </summary>
        /// <param name="id">游戏id</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static GameObject  DownloadFileAsync(int id,CAction<float,object> callback)
        {
            GameData data = GameConfig.GetGameData(id);
            List<string> files = data.configer.downFiles;
            UnityWebDownPacketQueue queue = new UnityWebDownPacketQueue();
            BaseValueConfigerJson[] configer = AppConst.gameValueConfiger.GetModule(data.scenName);
            for (int i = 0; i < files.Count; i++)
            {
                for (int j = 0; j < configer.Length; j++)
                {
                    BaseValueConfigerJson con = configer[j];
                    if (!files[i].ToLower().Equals(con.dirPath.ToLower())) continue;
                    string localUlr = $"{PathHelp.AppHotfixResPath}{con.dirPath}";
                    if (File.Exists(localUlr))
                    {
                        string localMd5 = MD5Helper.MD5File(localUlr);
                        if (localMd5.Equals(con.md5)) continue;
                        File.Delete(localUlr);
                    }
                    string webUrl = $"{AppConst.WebUrl}games/{con.dirPath}?md5={con.md5}";
                    DebugHelper.Log($"需要下载的资源地址:{webUrl}");
                    UnityWebPacket unityWeb = new UnityWebPacket();
                    unityWeb.urlPath = webUrl;
                    unityWeb.localPath = localUlr;
                    unityWeb.size = ulong.Parse(con.size);
                    unityWeb.func = (a, b) =>
                    {
                        callback?.Invoke(a, b);
                    };
                    queue.Add(unityWeb);
                }
            }
            GameObject go = new GameObject($"UnityWebRequestAsync");
            UnityWebRequestAsync asyncDown = go.AddComponent<UnityWebRequestAsync>();
            asyncDown.DownloadAsync(queue);
            return go;
        }
    }
}