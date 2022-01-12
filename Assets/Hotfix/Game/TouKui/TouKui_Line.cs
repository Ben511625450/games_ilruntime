
using DragonBones;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

namespace Hotfix.TouKui
{
    public class TouKui_Line : ILHotfixEntity
    {
        private class LineData
        {
            public int Index;
            public int Count;
        }
        private HierarchicalStateMachine hsm;
        private List<IState> states;
        private Transform animContent;
        private Transform RollContent;

        private List<List<Transform>> lines;
        private List<List<Transform>> anims;
        private List<LineData> lineDatas;

        private List<int> fudongList;

        protected override void Start()
        {
            base.Start();

            lines = new List<List<Transform>>();
            anims = new List<List<Transform>>();
            lineDatas = new List<LineData>();
            fudongList = new List<int>();

            hsm = new HierarchicalStateMachine(false, gameObject);
            states = new List<IState>();
            states.Add(new IdleState(this, hsm));
            states.Add(new ReceiveResultState(this, hsm));
            states.Add(new ShowSingleState(this, hsm));
            states.Add(new ShowTotalState(this, hsm));
            states.Add(new CloseEffectState(this, hsm));

            hsm.Init(states, nameof(IdleState));
        }
        protected override void Update()
        {
            base.Update();
            hsm?.Update();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            hsm?.CurrentState.OnExit();
        }

        protected override void AddEvent()
        {
            TouKui_Event.ShowResult += TouKui_Event_ShowResult;
            TouKui_Event.StartRoll += TouKui_Event_StartRoll;
            TouKui_Event.ExitSpecialMode += TouKui_Event_ExitSpecialMode;
            HotfixActionHelper.ReconnectGame += EventHelper_ReconnectGame;
        }

        protected override void RemoveEvent()
        {
            TouKui_Event.ShowResult -= TouKui_Event_ShowResult;
            TouKui_Event.StartRoll -= TouKui_Event_StartRoll;
            TouKui_Event.ExitSpecialMode -= TouKui_Event_ExitSpecialMode;
            HotfixActionHelper.ReconnectGame -= EventHelper_ReconnectGame;
        }

        private void EventHelper_ReconnectGame()
        {
            hsm?.ChangeState(nameof(CloseEffectState));
        }
        private void TouKui_Event_ShowResult()
        {
            if (TouKuiEntry.Instance.GameData.ResultData.nWinGold <= 0) return;
            hsm?.ChangeState(nameof(ReceiveResultState));
        }

        private void TouKui_Event_StartRoll()
        {
            hsm?.ChangeState(nameof(CloseEffectState));
        }

        private void TouKui_Event_ExitSpecialMode()
        {
            hsm?.ChangeState(nameof(CloseEffectState));
        }
        private void FindComponent()
        {
            RollContent = TouKuiEntry.Instance.MainContent.FindChildDepth("Content/RollContent"); //转动区域
            animContent = TouKuiEntry.Instance.MainContent.FindChildDepth("Content/AnimContent"); //显示库
        }
        /// <summary>
        /// 创建动画
        /// </summary>
        /// <param name="effectName">动画名</param>
        /// <returns></returns>
        private GameObject CreatHitEffect(string effectName)
        {
            //创建动画，先从对象池中获取
            Transform go = TouKuiEntry.Instance.effectPool.Find(effectName);
            if (go != null) return go.gameObject;

            go = TouKuiEntry.Instance.effectList.Find(effectName);
            GameObject _go = GameObject.Instantiate(go.gameObject);
            return _go;
        }
        /// <summary>
        /// 回收特效动画
        /// </summary>
        /// <param name="effect">特效动画</param>
        private void CollectEffect(GameObject effect)
        {
            if (effect == null) return;
            effect.transform.SetParent(TouKuiEntry.Instance.effectPool);
            effect.SetActive(false);
        }
        private void GetFudongList()
        {
            fudongList.Clear();
            if (TouKuiEntry.Instance.GameData.ResultData.cbType == 1)
            {
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
            }
            else if (TouKuiEntry.Instance.GameData.ResultData.cbType == 2)
            {
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
            }
            else if (TouKuiEntry.Instance.GameData.ResultData.cbType == 3)
            {
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 2) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 2) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
            }
            else if (TouKuiEntry.Instance.GameData.ResultData.cbType == 3)
            {
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add(TouKuiEntry.Instance.GameData.ResultData.nStartRow * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 2);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 1) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 2);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 2) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 2) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 1);
                fudongList.Add((TouKuiEntry.Instance.GameData.ResultData.nStartRow + 2) * 5 + TouKuiEntry.Instance.GameData.ResultData.nStartCol + 2);
            }
        }
        private class IdleState : State<TouKui_Line>
        {
            public IdleState(TouKui_Line owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
        }
        private class ReceiveResultState : State<TouKui_Line>
        {
            public ReceiveResultState(TouKui_Line owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            List<int> scatterlist = new List<int>();
            bool isComplete;
            public override void OnEnter()
            {
                base.OnEnter();
                isComplete = false;
                scatterlist.Clear();
                owner.lineDatas.Clear();
                owner.lines.Clear();
                owner.anims.Clear();
                owner.GetFudongList();
                TouKui_Struct.CMD_3D_SC_Result result = TouKuiEntry.Instance.GameData.ResultData;
                for (int i = 0; i < result.cbHitIcon.Count; i++)
                {
                    if (result.cbHitIcon[i][0] > 0)//筛选中奖的位置
                    {
                        LineData data = new LineData();
                        data.Index = i;
                        List<byte> hits = result.cbHitIcon[i].FindAllItem(delegate (byte b)
                          {
                              return b > 0;
                          });
                        data.Count = hits.Count;
                        owner.lineDatas.Add(data);
                    }
                }
                for (int i = 0; i < owner.RollContent.childCount; i++)
                {
                    Transform tran = owner.RollContent.GetChild(i).GetComponent<ScrollRect>().content;
                    for (int j = 0; j < tran.childCount; j++)
                    {
                        Image img = tran.GetChild(j).FindChildDepth<Image>("Icon");
                        img.color = Color.gray;
                    }
                }
            }
            public override void Update()
            {
                base.Update();
                if (isComplete) return;
                isComplete = true;
                if (owner.lineDatas.Count <= 0)
                {
                    for (int i = 0; i < TouKuiEntry.Instance.GameData.ResultData.cbIcon.Count; i++)
                    {
                        if (TouKuiEntry.Instance.GameData.ResultData.cbIcon[i] == 11)//有scatter
                        {
                            scatterlist.Add(i);
                        }
                    }
                    if (scatterlist.Count >= 3)//中了scatter奖
                    {
                        for (int i = 0; i < scatterlist.Count; i++)
                        {
                            int row = scatterlist[i] / 5;
                            int col = scatterlist[i] % 5;
                            Transform animTrans = owner.animContent.GetChild(col).GetChild(0).GetChild(row);
                            if (animTrans.childCount <= 0)
                            {
                                GameObject effectObj = owner.CreatHitEffect("Item12");
                                effectObj.transform.SetParent(animTrans);
                                effectObj.transform.localPosition = Vector3.zero;
                                effectObj.transform.localRotation = Quaternion.identity;
                                effectObj.transform.localScale = Vector3.one;
                                effectObj.name = "Item12";
                                effectObj.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        hsm?.ChangeState(nameof(IdleState));
                    }
                }
                else
                {
                    hsm?.ChangeState(nameof(ShowTotalState));
                }
            }
        }
        /// <summary>
        /// 单显 轮播
        /// </summary>
        private class ShowSingleState : State<TouKui_Line>
        {
            public ShowSingleState(TouKui_Line owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            private int currentShowIndex = 0;
            private float timer;
            public override void OnEnter()
            {
                base.OnEnter();
                currentShowIndex = 0;
                timer = 0;
                for (int i = 0; i < owner.RollContent.childCount; i++)
                {
                    Transform tran = owner.RollContent.GetChild(i).GetComponent<ScrollRect>().content;
                    for (int j = 0; j < tran.childCount; j++)
                    {
                        Image img = tran.GetChild(j).FindChildDepth<Image>("Icon");
                        img.enabled = true;
                    }
                }
                for (int i = 0; i < owner.anims.Count; i++)
                {
                    for (int j = 0; j < owner.anims[i].Count; j++)
                    {
                        owner.anims[i][j].gameObject.SetActive(false);
                    }
                }
                for (int i = 0; i < owner.transform.childCount; i++)
                {
                    owner.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < owner.lines[currentShowIndex].Count; i++)
                {
                    owner.lines[currentShowIndex][i].GetComponent<Image>().enabled = false;
                }
                for (int i = 0; i < owner.anims[currentShowIndex].Count; i++)
                {
                    owner.anims[currentShowIndex][i].gameObject.SetActive(true);
                }
                owner.transform.GetChild(0).gameObject.SetActive(true);
                owner.transform.GetChild(0).GetComponent<UnityArmatureComponent>().dbAnimation.Play((owner.lineDatas[currentShowIndex].Index + 1).ToString());
                ShowFudong();
            }
            private void ShowFudong()
            {
                bool isShow = false;
                List<int> lines = TouKui_DataConfig.Lines[owner.lineDatas[currentShowIndex].Index];//中奖线
                for (int i = 0; i < lines.Count; i++)
                {
                    if (owner.fudongList.Contains(lines[i] - 1))
                    {
                        isShow = true;
                        break;
                    }
                }
                TouKui_Event.DispatchShowFuDongWin(isShow);
            }
            public override void Update()
            {
                base.Update();
                timer += Time.deltaTime;
                if (timer >= TouKui_DataConfig.cyclePlayLineTime)
                {
                    timer = 0;
                    for (int i = 0; i < owner.lines[currentShowIndex].Count; i++)
                    {
                        owner.lines[currentShowIndex][i].GetComponent<Image>().enabled = true;
                    }
                    for (int i = 0; i < owner.anims[currentShowIndex].Count; i++)
                    {
                        owner.anims[currentShowIndex][i].gameObject.SetActive(false);
                    }
                    currentShowIndex++;
                    if (currentShowIndex >= owner.lines.Count)
                    {
                        currentShowIndex = 0;
                    }

                    owner.transform.GetChild(0).gameObject.SetActive(true);
                    owner.transform.GetChild(0).GetComponent<UnityArmatureComponent>().dbAnimation.Play((owner.lineDatas[currentShowIndex].Index + 1).ToString());
                    for (int i = 0; i < owner.lines[currentShowIndex].Count; i++)
                    {
                        owner.lines[currentShowIndex][i].GetComponent<Image>().enabled = false;
                    }
                    for (int i = 0; i < owner.anims[currentShowIndex].Count; i++)
                    {
                        owner.anims[currentShowIndex][i].gameObject.SetActive(true);
                    }
                    ShowFudong();
                }
            }
        }
        /// <summary>
        /// 总显
        /// </summary>
        private class ShowTotalState : State<TouKui_Line>
        {
            public ShowTotalState(TouKui_Line owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            float timer;
            public override void OnEnter()
            {
                base.OnEnter();
                timer = 0;
                owner.lines.Clear();
                owner.anims.Clear();
                bool isShow = false;
                for (int i = 0; i < owner.lineDatas.Count; i++)
                {
                    List<int> lines = TouKui_DataConfig.Lines[owner.lineDatas[i].Index];//中奖线
                    GameObject child = owner.transform.gameObject.InstantiateChild(i);
                    child.transform.localScale = Vector2.one * 100;//龙骨动画本身就是100*100
                    child.SetActive(true);
                    child.transform.GetComponent<UnityArmatureComponent>().dbAnimation.Play((owner.lineDatas[i].Index + 1).ToString());//播放线动画
                    List<Transform> showLine = new List<Transform>();
                    List<Transform> showAnim = new List<Transform>();
                    for (int j = 0; j < owner.lineDatas[i].Count; j++)
                    {
                        int hitIndex = lines[j] - 1;
                        int row = hitIndex / 5;
                        int col = hitIndex % 5;
                        if (owner.fudongList.Contains(hitIndex))
                        {
                            isShow = true;
                        }
                        Transform hitTrans = owner.RollContent.GetChild(col).GetComponent<ScrollRect>().content.GetChild(row);
                        Transform animTrans = owner.animContent.GetChild(col).GetChild(0).GetChild(row);
                        Image icon = hitTrans.FindChildDepth<Image>("Icon");
                        icon.enabled = false;
                        //获取中奖的动画图标
                        if (animTrans.childCount <= 0)
                        {
                            string effectName = TouKui_DataConfig.IconTable[TouKuiEntry.Instance.GameData.ResultData.cbIcon[hitIndex]];
                            if (TouKuiEntry.Instance.GameData.ResultData.cbIcon[hitIndex] == 10)
                            {
                                if (TouKuiEntry.Instance.GameData.isFreeGame)
                                {
                                    effectName = TouKui_DataConfig.IconTable[11 + (int)TouKuiEntry.Instance.GameData.CurrentMode];
                                }
                                else if (TouKuiEntry.Instance.GameData.isNormalFreeGame)
                                {
                                    effectName = TouKui_DataConfig.IconTable[11 + (int)TouKuiEntry.Instance.GameData.CurrentNormalMode];
                                }
                            }
                            GameObject effectObj = owner.CreatHitEffect(effectName);
                            effectObj.transform.SetParent(animTrans);
                            effectObj.transform.position = icon.transform.position;
                            effectObj.transform.localRotation = Quaternion.identity;
                            effectObj.transform.localScale = Vector3.one;
                            effectObj.name = hitTrans.gameObject.name;
                            effectObj.SetActive(true);
                        }
                        showAnim.Add(animTrans.GetChild(0));

                        showLine.Add(icon.transform);
                    }
                    owner.lines.Add(showLine);
                    owner.anims.Add(showAnim);
                }
                TouKui_Event.DispatchShowFuDongWin(isShow);
            }
            public override void Update()
            {
                base.Update();
                timer += Time.deltaTime;
                if (timer >= TouKui_DataConfig.lineAllShowTime)
                {
                    timer = 0;
                    hsm?.ChangeState(nameof(ShowSingleState));
                }
            }
        }
        /// <summary>
        /// 关闭所有icon动画
        /// </summary>
        private class CloseEffectState : State<TouKui_Line>
        {
            public CloseEffectState(TouKui_Line owner, HierarchicalStateMachine hsm) : base(owner, hsm)
            {
            }
            bool isComplete;
            public override void OnEnter()
            {
                base.OnEnter();
                isComplete = false;
                for (int i = 0; i < owner.RollContent.childCount; i++)
                {
                    Transform tran = owner.RollContent.GetChild(i).GetComponent<ScrollRect>().content;
                    for (int j = 0; j < tran.childCount; j++)
                    {
                        Image img = tran.GetChild(j).FindChildDepth<Image>("Icon");
                        img.color = Color.white;
                        img.enabled = true;
                    }
                }
                for (int i = 0; i < owner.animContent.childCount; i++)
                {
                    if (owner.animContent.GetChild(i).childCount <= 0) continue;
                    Transform child = owner.animContent.GetChild(i).GetChild(0);
                    for (int j = 0; j < child.childCount; j++)
                    {
                        for (int k = 0; k < child.GetChild(j).childCount; k++)
                        {
                            owner.CollectEffect(child.GetChild(j).GetChild(k).gameObject);
                        }
                    }
                }
                for (int i = 0; i < owner.transform.childCount; i++)
                {
                    owner.transform.GetChild(i).gameObject.SetActive(false);
                }
                TouKui_Event.DispatchShowFuDongWin(false);
            }
            public override void Update()
            {
                base.Update();
                if (isComplete) return;
                isComplete = true;
                hsm?.ChangeState(nameof(IdleState));
            }
        }
    }
}
