using System;
using DG.Tweening;
using Spine.Unity;
using System.Collections.Generic;
using LuaFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Hotfix.LTBY
{
    public class EffectParamData
    {
        public List<EffectParamData> fishList;
        public int chairId;
        public int level;
        public Vector3 pos;
        public bool playSound;
        public long score;
        public Vector3 worldPos;
        public Vector3 aimPos;
        public bool showText;
        public Action callBack;
        public Vector3 angles;
        public int useMul;
        public int baseScore;
        public string showType;
        public long showScore;
        public int fishType;
        public int multiple;
        public int bulletType;
        public int ratio;
        public List<int> mulList;
        public bool isElectricDragon;
        public List<int> multiples;
        public int id;
        public int count;
        public float time;
        public int mul;
        public string itemType;
        public string EffectName;
        public Vector3 position;
        public float lifeTime;
        public Action func;
        public string key;
        public string value;
        public EffectParamData award_cnt;
        public EffectParamData award;
        public int delay;
        public float scale;
        public long earn;
        public int fish_value;
        public int accum_money;
        public bool display_multiple;
    }

    public class EffectData
    {
        public Stack<EffectClass> Pool;
        public int maxNum;
    }

    /// <summary>
    /// 特效管理
    /// </summary>
    public class LTBY_EffectManager : SingletonNew<LTBY_EffectManager>
    {
        private int EffectId;
        Dictionary<int, Dictionary<int, EffectClass>> EffectList = new Dictionary<int, Dictionary<int, EffectClass>>();

        public Transform UiLayer;
        public Transform UiTopLayer;
        public Transform FishLayer;
        private Dictionary<int, SpecialBatterySpineAward> DrillList;
        private Dictionary<int, Dictionary<int, MissileSpineAward>> MissileList;
        private Dictionary<int, SpecialBatterySpineAward> ElectricList;
        private Dictionary<int, SpecialBatterySpineAward> FreeBatteryList;
        private Dictionary<int, ElectricDragonSpineAward> ElectricDragonSpineList;
        private Dictionary<int, EffectClass> DragonBallList;
        private FishAppear FishAppear;

        /// <summary>
        /// 特效的对象池
        /// </summary>
        private Dictionary<string, EffectData> EffectPoolItems = new Dictionary<string, EffectData>()
        {
            {nameof(FlyCoin), new EffectData() {maxNum = 60, Pool = new Stack<EffectClass>()}},
            {nameof(EffectLight), new EffectData() {maxNum = 10, Pool = new Stack<EffectClass>()}},
            {nameof(EffectText), new EffectData() {maxNum = 20, Pool = new Stack<EffectClass>()}},
            {nameof(LightAccount), new EffectData() {maxNum = 10, Pool = new Stack<EffectClass>()}},
            {nameof(DropItem), new EffectData() {maxNum = 5, Pool = new Stack<EffectClass>()}},
            {nameof(FishAppear), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(SpineAwardLevel), new EffectData() {maxNum = 4, Pool = new Stack<EffectClass>()}},
            {nameof(SpineAwardFullScreen), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(SpecialBatterySpineAward), new EffectData() {maxNum = 2, Pool = new Stack<EffectClass>()}},
            {nameof(MissileSpineAward), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(PoolTreasure), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(Wheel), new EffectData() {maxNum = 5, Pool = new Stack<EffectClass>()}},
            {nameof(CoinOutburst), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(FlyText), new EffectData() {maxNum = 4, Pool = new Stack<EffectClass>()}},
            {nameof(ExplosionPoint), new EffectData() {maxNum = 6, Pool = new Stack<EffectClass>()}},
            {nameof(GetItem), new EffectData() {maxNum = 5, Pool = new Stack<EffectClass>()}},
            {nameof(DragonDeadEffect), new EffectData() {maxNum = 2, Pool = new Stack<EffectClass>()}},
            {nameof(DragonSpineAward), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(FreeLotteryDropItem), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(SummonAppear), new EffectData() {maxNum = 2, Pool = new Stack<EffectClass>()}},
            {nameof(FreeDropItem), new EffectData() {maxNum = 5, Pool = new Stack<EffectClass>()}},
            {nameof(Aced), new EffectData() {maxNum = 4, Pool = new Stack<EffectClass>()}},
            {nameof(AcedLink), new EffectData() {maxNum = 20, Pool = new Stack<EffectClass>()}},
            {nameof(DialFish), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(SpineDialFish), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(Firework), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(ElectricDragonDeadEffect), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(DragonBallEffect), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(ElectricDragonSpineAward), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(DoubleDragonEffect), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(PandaDance), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(HundredMillionMoney), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(TreasureBowlEffect2), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(TreasureBowlEffect1), new EffectData() {maxNum = 1, Pool = new Stack<EffectClass>()}},
            {nameof(BaoJinBi), new EffectData() {maxNum = 3, Pool = new Stack<EffectClass>()}},
        };

        private T GetEffectFromPool<T>() where T : EffectClass, new()
        {
            string name = typeof(T).Name;
            if (!EffectPoolItems.ContainsKey(name))
            {
                DebugHelper.LogError($"没有注册此特效:{name}");
                return null;
            }

            EffectData list = EffectPoolItems[name];
            int curLength = list.Pool.Count;
            T result = null;
            if (curLength > 0)
            {
                result = list.Pool.Pop() as T;
            }
            else
            {
                result = System.Activator.CreateInstance<T>();
            }

            return result;
        }

        private void RemoveEffectToPool<T>(T effect) where T : EffectClass
        {
            string name = typeof(T).Name;
            if (!EffectPoolItems.ContainsKey(name))
            {
                DebugHelper.LogError($"没有注册此特效:{name}");
                effect.OnDestroy();
                return;
            }

            EffectData list = EffectPoolItems[name];
            int curLength = list.Pool.Count;
            if (curLength >= list.maxNum)
            {
                effect.OnDestroy();
            }
            else
            {
                list.Pool.Push(effect);
                effect.OnStored();
            }
        }

        public override void Init(Transform iLEntity = null)
        {
            base.Init(iLEntity);
            UiLayer = LTBYEntry.Instance.GetUiLayer();
            UiTopLayer = LTBYEntry.Instance.GetUiTopLayer();
            FishLayer = LTBYEntry.Instance.GetFishLayer();

            EffectList = new Dictionary<int, Dictionary<int, EffectClass>>();

            DrillList = new Dictionary<int, SpecialBatterySpineAward>();

            MissileList = new Dictionary<int, Dictionary<int, MissileSpineAward>>();

            ElectricList = new Dictionary<int, SpecialBatterySpineAward>();

            FreeBatteryList = new Dictionary<int, SpecialBatterySpineAward>();

            ElectricDragonSpineList = new Dictionary<int, ElectricDragonSpineAward>();

            DragonBallList = new Dictionary<int, EffectClass>();

            EffectId = 0;

            FishAppear = null;

            RegisterEvent();
        }

        public override void Release()
        {
            base.Release();
            DestroyAllEffect();
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
        }

        private void UnregisterEvent()
        {
        }

        //----------------------effect创建与销毁 start--------------------------------
        public T CreateEffect<T>(int chairId, params object[] args) where T : EffectClass, new()
        {
            if (!EffectList.ContainsKey(chairId))
            {
                EffectList.Add(chairId, new Dictionary<int, EffectClass>());
            }

            T effect = GetEffectFromPool<T>();
            int id = AllocateEffectId();
            effect.OnCreate(id, chairId, args);
            EffectList[chairId].Add(id, effect);
            return effect;
        }

        public void DestroyEffect<T>(int chairId, int id) where T : EffectClass
        {
            if (!EffectList.ContainsKey(chairId)) return;
            if (!EffectList[chairId].ContainsKey(id)) return;
            RemoveEffectToPool<T>(EffectList[chairId][id] as T);
            EffectList[chairId].Remove(id);
        }
        //----------------------effect创建与销毁 end--------------------------------

        //----------------------Missile创建与销毁 start--------------------------------
        public MissileSpineAward CreateMissile(int chairId, int propId)
        {
            if (!MissileList.ContainsKey(chairId))
            {
                MissileList.Add(chairId, new Dictionary<int, MissileSpineAward>());
            }

            MissileSpineAward effect = GetEffectFromPool<MissileSpineAward>();
            int id = AllocateEffectId();
            effect.OnCreate(id, chairId, propId);
            MissileList[chairId].Add(id,effect);
            return effect;
        }

        public void DestroyMissile(int chairId, int id = -1)
        {
            if (id >= 0)
            {
                if (!MissileList.ContainsKey(chairId)) return;
                if (!MissileList[chairId].ContainsKey(id)) return;
                if (MissileList[chairId][id] != null)
                {
                    RemoveEffectToPool(MissileList[chairId][id]);
                }

                MissileList[chairId].Remove(id);
            }
            else
            {
                if (!MissileList.ContainsKey(chairId)) return;
                var MissileValues = MissileList[chairId].GetDictionaryValues();
                for (int i = 0; i < MissileValues.Length; i++)
                {
                    RemoveEffectToPool(MissileValues[i]);
                }

                MissileList.Remove(chairId);
            }
        }

        //----------------------Missile创建与销毁 end--------------------------------
        //----------------------免费子弹创建与销毁 start--------------------------------
        public SpecialBatterySpineAward CreateFreeBattery(int chairId)
        {
            SpecialBatterySpineAward effect = GetEffectFromPool<SpecialBatterySpineAward>();
            effect.OnCreate(chairId, "FreeBattery");
            FreeBatteryList.Add(chairId, effect);
            return effect;
        }

        public SpecialBatterySpineAward GetFreeBattery(int chairId)
        {
            return !FreeBatteryList.ContainsKey(chairId) ? FreeBatteryList[chairId] : null;
        }

        public void DestroyFreeBattery(int chairId)
        {
            if (!FreeBatteryList.ContainsKey(chairId)) return;
            RemoveEffectToPool(FreeBatteryList[chairId]);
            FreeBatteryList.Remove(chairId);
        }
        //----------------------免费子弹创建与销毁 end-------------------------------- 

        //----------------------钻头炮创建与销毁 start--------------------------------
        public SpecialBatterySpineAward CreateDrill(int chairId)
        {
            SpecialBatterySpineAward effect = GetEffectFromPool<SpecialBatterySpineAward>();
            effect.OnCreate(chairId, "Drill");
            DrillList.Add(chairId, effect);
            return effect;
        }

        public SpecialBatterySpineAward GetDrill(int chairId)
        {
            return DrillList.ContainsKey(chairId) ? DrillList[chairId] : null;
        }

        public void DestroyDrill(int chairId)
        {
            if (!DrillList.ContainsKey(chairId)) return;
            RemoveEffectToPool(DrillList[chairId]);
            DrillList.Remove(chairId);
        }
        //----------------------钻头炮创建与销毁 end--------------------------------

        //----------------------电磁炮创建与销毁 start--------------------------------
        public SpecialBatterySpineAward CreateElectric(int chairId)
        {
            SpecialBatterySpineAward effect = GetEffectFromPool<SpecialBatterySpineAward>();
            effect.OnCreate(chairId, "Electric");
            ElectricList.Add(chairId, effect);
            return effect;
        }

        public SpecialBatterySpineAward GetElectric(int chairId)
        {
            return ElectricList.ContainsKey(chairId) ? ElectricList[chairId] : null;
        }

        public void DestroyElectric(int chairId)
        {
            if (!ElectricList.ContainsKey(chairId)) return;
            RemoveEffectToPool(ElectricList[chairId]);
            ElectricList.Remove(chairId);
        }
        //----------------------电磁炮创建与销毁 end--------------------------------

        //----------------------雷皇龙创建与销毁 start--------------------------------
        public ElectricDragonSpineAward CreateElectricDragonSpineAward(int chairId, params object[] args)
        {
            DebugHelper.LogError("创建ElectricDragonSpineAward  effectManager:308");
            ElectricDragonSpineAward effect = GetEffectFromPool<ElectricDragonSpineAward>();

            int id = AllocateEffectId();
            effect.OnCreate(id, chairId, args);
            ElectricDragonSpineList.Add(chairId, effect);
            return effect;
        }

        public ElectricDragonSpineAward GetElectricDragonSpineAward(int chairId)
        {
            return ElectricDragonSpineList.ContainsKey(chairId) ? ElectricDragonSpineList[chairId] : null;
        }


        public void DestroyElectricDragonSpineAward(int chairId)
        {
            if (!ElectricDragonSpineList.ContainsKey(chairId)) return;
            RemoveEffectToPool(ElectricDragonSpineList[chairId]);
            ElectricDragonSpineList.Remove(chairId);
        }
        //----------------------雷皇龙创建与销毁 end--------------------------------

        //----------------------龙珠创建与销毁 start--------------------------------
        public DragonBallEffect CreateDragonBallEffect(int chairId, params object[] args)
        {
            DragonBallEffect effect = GetEffectFromPool<DragonBallEffect>();
            int id = AllocateEffectId();
            effect.OnCreate(id, chairId, args);
            DragonBallList.Add(chairId, effect);
            return effect;
        }

        public void DestroyDragonBallEffect(int chairId)
        {
            if (DragonBallList.ContainsKey(chairId))
            {
                RemoveEffectToPool(DragonBallList[chairId]);
            }

            DragonBallList.Remove(chairId);
        }

        public void DestroyAllDragonBallEffect()
        {
            var DragonBallKeys = DragonBallList.GetDictionaryKeys();
            for (int i = 0; i < DragonBallKeys.Length; i++)
            {
                DestroyDragonBallEffect(DragonBallKeys[i]);
            }
        }

        //----------------------龙珠创建与销毁 end--------------------------------
        //----------------------特殊鱼出场特效创建与销毁 start--------------------------------
        public FishAppear CreateFishAppear(FishDataConfig config)
        {
            if (FishAppear != null) return FishAppear;
            FishAppear = GetEffectFromPool<FishAppear>();
            FishAppear.OnCreate(config);
            return FishAppear;
        }

        public void DestroyFishAppear()
        {
            if (LTBY_EffectManager.Instance.FishAppear == null) return;
            RemoveEffectToPool(LTBY_EffectManager.Instance.FishAppear);
            LTBY_EffectManager.Instance.FishAppear = null;
        }

        //----------------------特殊鱼出场特效创建与销毁 end--------------------------------
        private int AllocateEffectId()
        {
            EffectId++;
            if (EffectId >= 10000)
            {
                EffectId = 0;
            }

            return EffectId;
        }

        public void DestroyPlayerEffect(int chairId)
        {
            if (EffectList.ContainsKey(chairId))
            {
                var effectValues = EffectList[chairId].GetDictionaryValues();
                for (int i = 0; i < effectValues.Length; i++)
                {
                    effectValues[i].OnDestroy();
                }

                EffectList.Remove(chairId);
            }

            DestroyElectric(chairId);
            DestroyDrill(chairId);
            DestroyMissile(chairId);
            DestroyFreeBattery(chairId);
            DestroyElectricDragonSpineAward(chairId);
            DestroyDragonBallEffect(chairId);
        }

        private void DestroyAllEffect()
        {
            var effectKeys = EffectList.GetDictionaryKeys();
            for (int i = 0; i < effectKeys.Length; i++)
            {
                DestroyPlayerEffect(effectKeys[i]);
            }

            DestroyFishAppear();
            var MissileKeys = MissileList.GetDictionaryKeys();
            for (int i = 0; i < MissileKeys.Length; i++)
            {
                DestroyMissile(MissileKeys[i]);
            }

            var DrillKeys = DrillList.GetDictionaryKeys();
            for (int i = 0; i < DrillKeys.Length; i++)
            {
                DestroyDrill(DrillKeys[i]);
            }

            var ElectricListKeys = ElectricList.GetDictionaryKeys();
            for (int i = 0; i < ElectricListKeys.Length; i++)
            {
                DestroyElectric(ElectricListKeys[i]);
            }

            var FreeBatteryListKeys = FreeBatteryList.GetDictionaryKeys();
            for (int i = 0; i < FreeBatteryListKeys.Length; i++)
            {
                DestroyFreeBattery(FreeBatteryListKeys[i]);
            }

            var ElectricDragonSpineListKeys = ElectricDragonSpineList.GetDictionaryKeys();
            for (int i = 0; i < ElectricDragonSpineListKeys.Length; i++)
            {
                DestroyElectricDragonSpineAward(ElectricDragonSpineListKeys[i]);
            }


            //清除缓存池中的数据
            var EffectPoolItemsValues = EffectPoolItems.GetDictionaryValues();
            for (int i = 0; i < EffectPoolItemsValues.Length; i++)
            {
                if (EffectPoolItemsValues[i].Pool == null) continue;
                while (EffectPoolItemsValues[i].Pool.Count > 0)
                {
                    EffectPoolItemsValues[i].Pool.Pop().OnDestroy();
                }

                EffectPoolItemsValues[i].Pool.Clear();
            }

            System.GC.Collect();
        }

        /// <summary>
        /// 切换房间时,把所有特效全部放到对象池中
        /// </summary>
        public void StoreAllEffect()
        {
            var EffectListValues = EffectList.GetDictionaryValues();
            for (int i = 0; i < EffectListValues.Length; i++)
            {
                var itemValues = EffectListValues[i].GetDictionaryValues();
                for (int j = 0; j < itemValues.Length; j++)
                {
                    RemoveEffectToPool(itemValues[j]);
                }

                EffectListValues[i].Clear();
            }


            DestroyFishAppear();
            var MissileListKeys = MissileList.GetDictionaryKeys();
            for (int i = 0; i < MissileListKeys.Length; i++)
            {
                DestroyMissile(MissileListKeys[i]);
            }

            var DrillListKeys = DrillList.GetDictionaryKeys();
            for (int i = 0; i < DrillListKeys.Length; i++)
            {
                DestroyDrill(DrillListKeys[i]);
            }

            var ElectricListKeys = ElectricList.GetDictionaryKeys();
            for (int i = 0; i < ElectricListKeys.Length; i++)
            {
                DestroyElectric(ElectricListKeys[i]);
            }

            var FreeBatteryListKeys = FreeBatteryList.GetDictionaryKeys();
            for (int i = 0; i < FreeBatteryListKeys.Length; i++)
            {
                DestroyFreeBattery(FreeBatteryListKeys[i]);
            }

            var ElectricDragonSpineListKeys = ElectricDragonSpineList.GetDictionaryKeys();
            for (int i = 0; i < ElectricDragonSpineListKeys.Length; i++)
            {
                DestroyElectricDragonSpineAward(ElectricDragonSpineListKeys[i]);
            }
        }

        public void CreateBoostScoreBoardEffects(long score, params object[] args)
        {
            EffectParamData data = args.Length > 0 ? (EffectParamData)args[0] : new EffectParamData();
            int chairId = data.chairId;
            Vector3 pos = data.pos;
            float scale = data.scale == 0 ? 1 : data.scale;
            float delay = data.delay; //delay这里不用赋默认值，由各个特效自己负责自己的默认播放时间
            if (score >= EffectConfig.UseFireworkScore)
            {
                CreateEffect<Firework>(chairId, pos, scale, delay);
            }

            if (!LTBY_GameView.GameInstance.IsSelf(chairId)) return;
            if (CheckCanUseHundredMillion(score))
            {
                CreateEffect<HundredMillionMoney>(chairId);
            }
        }

        public bool CheckCanUseHundredMillion(long score)
        {
            return score >= EffectConfig.UseHundredMillionScore;
        }
    }

    public class EffectClass
    {
        protected string name;
        protected int id;
        protected int chairId;
        protected Transform effect;
        protected Image imageBG;
        protected Transform fishImage;
        protected Transform text;
        protected string prefab;
        protected float scale;

        protected Transform item;
        protected Transform account;
        protected string prefabName;

        protected List<int> timerKey;
        protected List<int> actionKey;

        protected SkeletonGraphic skeleton;
        protected NumberRoller num;
        protected Vector3 numPosition;
        protected long score;
        protected ILBehaviour IlBehaviour;

        public virtual void OnCreate(int _id, int _chairId, params object[] args)
        {
        }

        public virtual void OnCreate(int _chairId, params object[] args)
        {
        }

        public virtual void OnCreate(params object[] args)
        {
        }

        public virtual void OnStored()
        {
            if (this.actionKey != null)
            {
                for (int i = 0; i < actionKey.Count; i++)
                {
                    LTBY_Extend.Instance.StopAction(actionKey[i]);
                }

                this.actionKey.Clear();
            }

            if (this.timerKey != null)
            {
                for (int i = 0; i < timerKey.Count; i++)
                {
                    LTBY_Extend.Instance.StopTimer(timerKey[i]);
                }

                this.timerKey.Clear();
            }
        }

        public virtual void OnDestroy()
        {
            OnStored();
        }

        public virtual void SettleAccount()
        {
        }

        public virtual Vector3 GetNumWorldPos()
        {
            return this.numPosition;
        }

        public virtual void AddScore(long _score, float roll = 0)
        {
        }

        public virtual void OnSettle()
        {
        }
    }

    public class FishAppear : EffectClass
    {
        public override void OnCreate(params object[] args)
        {
            base.OnCreate(args);
            this.name = nameof(FishAppear);
            LTBY_Audio.Instance.Play(SoundConfig.FishAppear);
            if (effect == null)
            {
                this.effect =
                    LTBY_PoolManager.Instance.GetUiItem("LTBY_FishAppear", LTBY_EffectManager.Instance.UiLayer);
            }

            this.effect.gameObject.SetActive(true);
            this.effect.localPosition = new Vector3(0, 0, 0);
            FishDataConfig config = args.Length >= 1 ? args[0] as FishDataConfig : new FishDataConfig();
            if (config == null) config = new FishDataConfig();
            string BGBundle = string.IsNullOrEmpty(config.broadcastBgBundle) ? "res_effect" : config.broadcastBgBundle;
            string BGName = string.IsNullOrEmpty(config.broadcastBg) ? "ty_gc_di" : config.broadcastBg;
            Sprite appearBG = LTBY_Extend.Instance.LoadSprite(BGBundle, BGName);
            if (imageBG == null)
            {
                this.imageBG = this.effect.FindChildDepth<Image>("Node/yu");
            }

            this.imageBG.sprite = appearBG;
            this.imageBG.SetNativeSize();

            if (fishImage == null)
            {
                this.fishImage = this.effect.FindChildDepth("Node/yu/yu");
            }

            fishImage.gameObject.SetActive(false);
            string imageBundle = FishConfig.GetFishImageBundle(config.fishOrginType);
            this.fishImage.GetComponent<Image>().sprite =
                LTBY_Extend.Instance.LoadSprite(imageBundle, $"fish_{config.fishOrginType}");
            fishImage.GetComponent<RectTransform>().sizeDelta = Vector2.one * 120;
            fishImage.GetComponent<RectTransform>().anchorMax = Vector2.one * 0.5f;
            fishImage.GetComponent<RectTransform>().anchorMin = Vector2.one * 0.5f;
            fishImage.localPosition = new Vector3(-112, 10);
            fishImage.gameObject.SetActive(true);
            if (text == null)
            {
                this.text = this.effect.FindChildDepth("Node/yu/text");
            }

            this.text.GetComponent<Text>().text = config.broadcast;

            this.timerKey = new List<int>();
            int key = LTBY_Extend.Instance.DelayRun(4, () => LTBY_EffectManager.Instance.DestroyFishAppear());
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_FishAppear", this.effect);
            this.effect = null;
        }
    }

    public class EffectText : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(EffectText);
            this.id = _id;
            this.chairId = _chairId;
            score = args.Length >= 1 ? (long) args[0] : 0;
            Vector3 worldPos = args.Length >= 2 ? (Vector3) args[1] : Vector3.zero;
            int useMul = args.Length >= 3 ? (int) args[2] : 0;
            bool isElectric = args.Length >= 4 && (bool) args[3];
            int useMul2 = args.Length >= 5 ? (int) args[4] : 0;
            if (isElectric)
            {
                this.prefab = LTBY_GameView.GameInstance.IsSelf(_chairId)
                    ? "LTBY_TextElectricSelf"
                    : "LTBY_TextElectricOther";
            }
            else
            {
                this.prefab = LTBY_GameView.GameInstance.IsSelf(_chairId) ? "LTBY_TextSelf" : "LTBY_TextOther";
            }


            this.text = LTBY_PoolManager.Instance.GetUiItem(this.prefab, LTBY_EffectManager.Instance.UiLayer);
            this.text.position = worldPos;
            this.scale = 1;
            this.actionKey = new List<int>();

            if (useMul > 0)
            {
                this.text.GetComponent<TextMeshProUGUI>().text = $"+{Mathf.Floor((float) score / useMul)}";
                this.text.FindChildDepth("Mul").gameObject.SetActive(true);
                this.text.FindChildDepth<Text>("Mul").text = useMul2 > 0 ? $"x{useMul}x{useMul2}" : $"x{useMul}";

                this.text.localScale = new Vector3(0, 0, 0);
                Sequence tween = DOTween.Sequence();
                tween.Append(this.text?.DOScale(new Vector3(this.scale, this.scale, 1), 0.4f).SetEase(Ease.OutElastic));
                tween.Append(this.text?.DOScale(new Vector3(0, 0, 1), 0.4f).SetEase(Ease.InBack).SetDelay(0.2f));
                tween.OnComplete(()=>
                {
                    LTBY_EffectManager.Instance.DestroyEffect<EffectText>(this.chairId, this.id);
                });
                int key = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(key);
            }
            else
            {
                this.text.GetComponent<TextMeshProUGUI>().text = $"+{score}";
                this.text.FindChildDepth("Mul").gameObject.SetActive(false);
                Sequence tween = DOTween.Sequence();
                tween.Append(this.text?.DOScale(new Vector3(this.scale * 1.2f, this.scale * 1.2f, 1), 0.2f)
                    .SetEase(Ease.Linear));
                tween.Append(this.text?.DOScale(new Vector3(this.scale, this.scale, 1), 0.2f).SetEase(Ease.Linear));
                Sequence tween1 = DOTween.Sequence();
                tween.Append(tween1);
                tween1.Append(this.text?.DOBlendableLocalMoveBy(new Vector3(0, 50, 1), 0.4f).SetEase(Ease.Linear));
                CanvasGroup group = text.GetComponent<CanvasGroup>();
                if (group == null) group = text.gameObject.AddComponent<CanvasGroup>();

                tween1.Insert(0, group.DOFade(0, 0.4f));
                tween1.SetDelay(0.2f);
                tween.OnComplete(() =>
                {
                    group.alpha = 1;
                    LTBY_EffectManager.Instance.DestroyEffect<EffectText>(this.chairId, this.id);
                });
                int key = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(key);
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            if (string.IsNullOrEmpty(this.prefab)) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.prefab, this.text);
            this.prefab = null;
        }
    }

    public class EffectLight : EffectClass
    {
        private Transform light;
        private LineRenderer lineRenderer;
        private float detail;
        private int lightningWarp;
        private Vector3 startPos;

        private Vector3 endPos;
        private int hideFrame;
        private List<Vector3> rendererPosList;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(EffectLight);
            this.id = _id;
            this.chairId = _chairId;
            this.detail = 2;
            this.lightningWarp = 4;
            this.startPos = args.Length >= 1 ? (Vector3) args[0] : Vector3.zero;
            this.endPos = args.Length >= 2 ? (Vector3) args[1] : Vector3.zero;
            float existTime = args.Length >= 3 ? (float) args[2] : 0;

            this.hideFrame = 1;
            if (this.light == null)
            {
                this.light = LTBY_PoolManager.Instance.GetGameItem("LTBY_Light", LTBY_EffectManager.Instance.FishLayer);
            }

            this.light.gameObject.SetActive(false);

            if (this.lineRenderer == null)
            {
                this.lineRenderer = this.light.GetComponent<LineRenderer>();
            }

            this.lineRenderer.useWorldSpace = true;
            this.rendererPosList = new List<Vector3>();

            this.timerKey = new List<int>();
            if (IlBehaviour == null)
            {
                IlBehaviour = this.light.gameObject.AddComponent<ILBehaviour>();
            }

            IlBehaviour.UpdateEvent = Update;
            // int key = LTBY_Extend.Instance.StartTimer(delegate() { Update(); });
            // this.timerKey.Add(key);

            int _key = LTBY_Extend.Instance.DelayRun(existTime <= 0 ? 1.5f : existTime,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<EffectLight>(this.chairId, this.id); });
            this.timerKey.Add(_key);
        }

        private void CollectLinePos(Vector3 _startPos, Vector3 destPos, float displace)
        {
            if (displace < this.detail)
            {
                this.rendererPosList.Add(_startPos);
            }
            else
            {
                float midX = (_startPos.x + destPos.x) * 0.5f;
                float midY = (_startPos.y + destPos.y) * 0.5f;
                float midZ = (_startPos.z + destPos.z) * 0.5f;

                midX += (UnityEngine.Random.Range(0f, 1f) - 0.5f) * displace;
                midY += (UnityEngine.Random.Range(0f, 1f) - 0.5f) * displace;
                midZ += (UnityEngine.Random.Range(0f, 1f) - 0.5f) * displace;

                Vector3 midPos = new Vector3(midX, midY, midZ);

                CollectLinePos(_startPos, midPos, displace * 0.5f);
                CollectLinePos(midPos, destPos, displace * 0.5f);
            }
        }

        private void Update()
        {
            if (this.hideFrame > 0)
            {
                this.hideFrame--;
                if (this.hideFrame == 0)
                {
                    this.light.gameObject.SetActive(true);
                }
            }

            this.rendererPosList.Clear();
            CollectLinePos(this.startPos, this.endPos, this.lightningWarp);

            this.rendererPosList.Add(this.endPos);

            this.lineRenderer.positionCount = this.rendererPosList.Count;
            for (int i = 0; i < rendererPosList.Count; i++)
            {
                lineRenderer.SetPosition(i, rendererPosList[i]);
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.light == null) return;
            this.light.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (light == null) return;
            if (IlBehaviour != null)
            {
                LTBY_Extend.Destroy(IlBehaviour);
            }

            IlBehaviour = null;
            LTBY_PoolManager.Instance.RemoveGameItem("LTBY_Light", this.light);
            this.light = null;
        }
    }

    public class LightAccount : EffectClass
    {
        private string hadSpecialBattery;
        private Transform image;
        private List<long> scoreList;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(LightAccount);
            this.id = _id;
            this.chairId = _chairId;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            EffectParamData data = args.Length >= 1 ? (EffectParamData) args[0] : new EffectParamData();

            this.hadSpecialBattery = LTBY_BatteryManager.Instance.HadSpecialBattery(this.chairId);
            int bulletType = data.bulletType;
            if (PropConfig.CheckIsDragonBall(bulletType))
            {
                this.hadSpecialBattery = "DragonBall";
            }

            if (!string.IsNullOrEmpty(this.hadSpecialBattery))
            {
                SpecialShow(data);
            }
            else
            {
                NormalShow(data);
            }
        }

        private void SpecialShow(EffectParamData data)
        {
            int multiple = data.multiple;
            EffectClass _effect = null;
            int useMul = 0;
            switch (this.hadSpecialBattery)
            {
                case "Drill":
                {
                    _effect = LTBY_EffectManager.Instance.GetDrill(this.chairId);
                    if (_effect == null)
                    {
                        _effect = LTBY_EffectManager.Instance.CreateDrill(this.chairId);
                    }

                    break;
                }
                case "Electric":
                {
                    _effect = LTBY_EffectManager.Instance.GetElectric(this.chairId);
                    if (_effect == null)
                    {
                        _effect = LTBY_EffectManager.Instance.CreateElectric(this.chairId);
                    }

                    break;
                }
                case "FreeBattery":
                {
                    _effect = LTBY_EffectManager.Instance.GetFreeBattery(this.chairId);
                    if (_effect == null)
                    {
                        _effect = LTBY_EffectManager.Instance.CreateFreeBattery(this.chairId);
                    }

                    useMul = multiple;
                    break;
                }
                case "DragonBall":
                {
                    _effect = LTBY_EffectManager.Instance.GetElectricDragonSpineAward(this.chairId);
                    if (_effect == null)
                    {
                        _effect = LTBY_EffectManager.Instance.CreateElectricDragonSpineAward(this.chairId, multiple);
                    }

                    break;
                }
            }

            if (_effect == null) return;
            Vector3 aimPos = _effect.GetNumWorldPos();

            float delay = 0;
            List<EffectParamData> fishList = data.fishList;
            for (int i = 0; i < fishList.Count; i++)
            {
                int index = i;
                delay += 0.05f;
                int key = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    if (fishList.Count <= index || fishList[index] == null) return;
                    EffectParamData _data = new EffectParamData()
                    {
                        score = fishList[index].score,
                        worldPos = fishList[index].pos,
                        aimPos = aimPos,
                        showText = true,
                        callBack = () => { _effect.AddScore(fishList[index].score); },
                        useMul = useMul,
                    };
                    LTBY_EffectManager.Instance.CreateEffect<FlyCoin>(this.chairId, _data);
                });
                this.timerKey.Add(key);
            }

            delay++;

            int _key = LTBY_Extend.Instance.DelayRun(delay,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<LightAccount>(this.chairId, this.id); });
            this.timerKey.Add(_key);
        }

        private void NormalShow(EffectParamData data)
        {
            int fishType = data.fishType;
            List<EffectParamData> fishList = data.fishList;
            if (account == null)
            {
                this.account =
                    LTBY_PoolManager.Instance.GetUiItem("LTBY_SpineLight", LTBY_EffectManager.Instance.UiLayer);
            }

            this.account.gameObject.SetActive(true);

            this.skeleton = this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            if (this.skeleton != null)
            {
                this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.account.localScale = new Vector3(1, 1, 1);

            this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
            if (num == null)
            {
                Transform numtrans = this.account.FindChildDepth("Num");
                this.num = numtrans.gameObject.GetILComponent<NumberRoller>();
                if (num == null)
                {
                    this.num = numtrans.gameObject.AddILComponent<NumberRoller>();
                }
            }

            this.num.Init();
            this.num.text = "0";
            this.numPosition = this.num.transform.position;

            if (image == null)
            {
                this.image = this.account.FindChildDepth("Image");
            }

            this.image.GetComponent<Image>().sprite =
                LTBY_Extend.Instance.LoadSprite("res_fishmodel", $"fish_{fishType}");

            this.account.gameObject.SetActive(false);

            float delay = 0.5f;

            int key = LTBY_Extend.Instance.DelayRun(delay + 0.5f, ()=>
            {
                this.account?.gameObject.SetActive(true);

                if (!LTBY_GameView.GameInstance.IsSelf(this.chairId)) return;
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward3);
                LTBY_Audio.Instance.Play(SoundConfig.SpineAwardLight);
            });
            this.timerKey.Add(key);

            this.scoreList = new List<long>();

            long scoreNum = 0;
            for (int i = 0; i < fishList.Count; i++)
            {
                delay += 0.05f;
                int index = i;
                int _key = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    if (fishList.Count <= index || fishList[index] == null) return;
                    EffectParamData flyCoinData = new EffectParamData()
                    {
                        score = fishList[index].score,
                        worldPos = fishList[index].pos,
                        aimPos = this.numPosition,
                        showText = true,
                        callBack = ()=>
                        {
                            this.num.RollBy(fishList[index].score, 0.5f);
                        }
                    };
                    LTBY_EffectManager.Instance.CreateEffect<FlyCoin>(this.chairId, flyCoinData);
                    this.scoreList.Add(fishList[index].score);
                });
                this.timerKey.Add(_key);

                scoreNum += fishList[i].score;
            }

            delay += 2;

            if (scoreNum >= EffectConfig.UseFireworkScore)
            {
                int _key = LTBY_Extend.Instance.DelayRun(delay, () =>
                 {
                     EffectParamData _data = new EffectParamData()
                     {
                         chairId = this.chairId,
                         pos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                         scale = 0.5f
                     };
                     LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(scoreNum, _data);
                 });
                this.timerKey.Add(_key);
                delay += 5;
            }
            else
            {
                delay++;
            }

            int kkey = LTBY_Extend.Instance.DelayRun(delay, ()=>
            {
                float baseX = this.account.position.x;
                float baseY = this.account.position.y;
                Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
                float aimX = aimPos.x;
                float aimY = aimPos.y;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(DOTween.To(value=>
                {
                    if (this.account == null) return;
                    float t = value * 0.01f;
                    this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                    float _scale = 1 - t;
                    this.account.localScale = new Vector3(_scale, _scale, _scale);
                }, 0, 100, 0.3f).SetEase(Ease.InBack));
                sequence.OnComplete(()=>
                {
                    this.account?.gameObject.SetActive(false);
                    for (int i = 0; i < this.scoreList.Count; i++)
                    {
                        int index = i;
                        Tween tween = LTBY_Extend.DelayRun(index * 0.15f).OnComplete(()=>
                        {
                            if (scoreList.Count <= index) return;
                            LTBY_GameView.GameInstance.AddScore(this.chairId, scoreList[index]);
                            LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, scoreList[index], true);
                            if (index == this.scoreList.Count - 1)
                            {
                                LTBY_EffectManager.Instance.DestroyEffect<LightAccount>(this.chairId, this.id);
                            }
                        });
                        int _tkey = LTBY_Extend.Instance.RunAction(tween);
                        this.actionKey.Add(_tkey);
                    }
                });
                int _key = LTBY_Extend.Instance.RunAction(sequence);
                actionKey.Add(_key);
            });
            timerKey.Add(kkey);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.account == null) return;
            this.account.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_SpineLight", this.account);
            this.account = null;
        }
    }

    public class DragonDeadEffect : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(DragonDeadEffect);
            this.id = _id;
            this.chairId = _chairId;
            this.timerKey = new List<int>();
            float delay = 1;
            int key = LTBY_Extend.Instance.DelayRun(delay, ()=> { LTBYEntry.Instance.ShakeFishLayer(4); });
            this.timerKey.Add(key);
            List<Vector3> poslist = args.Length >= 1 ? (List<Vector3>) args[0] : new List<Vector3>();
            for (int i = 0; i < poslist.Count; i++)
            {
                int index = i;
                int kkey = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    if (poslist.Count <= index) return;
                    EffectParamData paramData = new EffectParamData()
                    {
                        level = 2,
                        pos = poslist[index],
                        playSound = true
                    };
                    LTBY_EffectManager.Instance.CreateEffect<ExplosionPoint>(this.chairId, paramData);
                });
                this.timerKey.Add(kkey);
                delay += 0.28f;
            }

            delay += 0.5f;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                int kkey = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    this.effect = this.effect != null ? this.effect : LTBY_PoolManager.Instance.GetUiItem("LTBY_DragonDeadEffect",
                            LTBY_EffectManager.Instance.UiLayer);
                    this.effect?.gameObject.SetActive(true);
                    this.effect.localPosition = new Vector3(0, 0, 0);
                    LTBY_Audio.Instance.Play(SoundConfig.DragonEffect1);
                });
                this.timerKey.Add(kkey);
            }

            delay++;

            int _key = LTBY_Extend.Instance.DelayRun(delay, () =>
            {
                EffectParamData data = args[1] as EffectParamData;
                if (data == null) data = new EffectParamData();
                LTBY_EffectManager.Instance.CreateEffect<DragonSpineAward>(this.chairId, data.score, data.baseScore);
                LTBY_EffectManager.Instance.DestroyEffect<DragonDeadEffect>(this.chairId, this.id);
            });
            this.timerKey.Add(_key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_DragonDeadEffect", this.effect);
            this.effect = null;
        }
    }

    public class ElectricDragonDeadEffect : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(ElectricDragonDeadEffect);
            this.id = _id;
            this.chairId = _chairId;
            this.timerKey = new List<int>();

            float delay = 1;
            int key = LTBY_Extend.Instance.DelayRun(delay, () =>{ LTBYEntry.Instance.ShakeFishLayer(4); });
            this.timerKey.Add(key);
            List<Vector3> poslist = args.Length >= 1 ? (List<Vector3>) args[0] : new List<Vector3>();
            EffectParamData data = args.Length >= 2 ? (EffectParamData) args[1] : new EffectParamData();
            for (int i = 0; i < poslist.Count; i++)
            {
                int index = i;
                int _key = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    if (poslist.Count <= index) return;
                    EffectParamData param = new EffectParamData()
                    {
                        level = 2,
                        pos = poslist[index],
                        playSound = true,
                    };
                    LTBY_EffectManager.Instance.CreateEffect<ExplosionPoint>(this.chairId, param);
                });
                this.timerKey.Add(_key);
                delay += 0.28f;
            }

            delay += 0.5f;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                int _key = LTBY_Extend.Instance.DelayRun(delay, ()=>
                {
                    this.effect = this.effect!= null ? this.effect : LTBY_PoolManager.Instance.GetUiItem("LTBY_ElectricDragonDeadEffect",
                            LTBY_EffectManager.Instance.UiLayer);
                    this.effect?.gameObject.SetActive(true);
                    this.effect.localPosition = new Vector3(0, 0, 0);
                    LTBY_Audio.Instance.Play(SoundConfig.DragonEffect1);
                });
                this.timerKey.Add(_key);
                delay++;
                int _delayKey = LTBY_Extend.Instance.DelayRun(delay - 0.4f, ()=>
                {
                    LTBY_EffectManager.Instance.CreateEffect<DoubleDragonEffect>(_chairId);
                    if (!LTBY_GameView.GameInstance.IsPlayerRunInBackground(this.chairId))
                    {
                        if (data == null) return;
                        LTBY_EffectManager.Instance.CreateDragonBallEffect(this.chairId, data.worldPos,
                            data.multiples[0], data.multiples[1]);
                    }
                });
                this.timerKey.Add(_delayKey);
            }
            else
            {
                int _delayKey = LTBY_Extend.Instance.DelayRun(delay - 0.4f, ()=>
                {
                    if (!LTBY_GameView.GameInstance.IsPlayerRunInBackground(this.chairId))
                    {
                        if (data == null) return;
                        LTBY_EffectManager.Instance.CreateDragonBallEffect(this.chairId, data.worldPos,
                            data.multiples[0], data.multiples[1]);
                    }
                });
                this.timerKey.Add(_delayKey);
            }

            int delayKey = LTBY_Extend.Instance.DelayRun(delay, () =>
                {
                    LTBY_EffectManager.Instance.DestroyEffect<ElectricDragonDeadEffect>(this.chairId, this.id);
                });
            this.timerKey.Add(delayKey);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_ElectricDragonDeadEffect", this.effect);
            this.effect = null;
        }
    }

    public class ExplosionPoint : EffectClass
    {
        int level;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(ExplosionPoint);
            this.id = _id;
            this.chairId = _chairId;
            EffectParamData data = args.Length >= 1 ? (EffectParamData) args[0] : new EffectParamData();
            this.level = data.level;
            Vector3 pos = data.pos;
            bool playSound = data.playSound;
            Vector3 angles = data.angles;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId) && playSound)
            {
                LTBY_Audio.Instance.Play(SoundConfig.AllAudio[$"ExplosionPoint{this.level}"]);
            }

            this.prefab = $"LTBY_ExplosionPoint{this.level}";
            this.effect = LTBY_PoolManager.Instance.GetUiItem(this.prefab, LTBY_EffectManager.Instance.UiLayer);
            this.effect.position = pos;

            float delay = 1;

            if (this.level > 1)
            {
                LTBYEntry.Instance.ShakeFishLayer(this.level - 1);
            }

            if (this.level == 3)
            {
                delay = 4;
            }

            if (args.Length >= 4)
            {
                this.effect.localEulerAngles = angles;
            }

            this.timerKey = new List<int>();
            int key = LTBY_Extend.Instance.DelayRun(delay,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<ExplosionPoint>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.prefab, this.effect);
            this.effect = null;
        }
    }

    /// <summary>
    /// 飞金币
    /// </summary>
    public class FlyCoin : EffectClass
    {
        private List<Transform> coinList;
        private Vector3 position;
        private Vector3 aimPos;
        private Action callBack;
        private bool showText;
        private int useMul;
        private int coinNum;
        private float deltaAngle;
        private float initAngle;
        private string coinPrefab;

        /// <summary>
        /// 创建飞行硬币
        /// </summary>
        /// <param name="_id">id</param>
        /// <param name="_chairId">椅子号</param>
        /// <param name="args">参数</param>
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            EffectParamData data = args.Length >= 1 ? args[0] as EffectParamData : new EffectParamData();
            if (data == null) data = new EffectParamData();
            this.name = nameof(FlyCoin);
            this.id = _id;
            this.chairId = _chairId;
            this.coinList = new List<Transform>();
            this.position = data.worldPos;
            this.aimPos = data.aimPos;
            this.score = data.score;
            this.callBack = data.callBack;
            this.showText = data.showText;
            this.useMul = data.useMul;

            this.coinNum = 2;
            this.deltaAngle = 360f / this.coinNum;
            this.initAngle = UnityEngine.Random.Range(0, 360);
            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            this.coinPrefab = LTBY_GameView.GameInstance.IsSelf(this.chairId) ? "LTBY_CoinSelf" : "LTBY_CoinOther";
            int _num = this.coinNum;

            while (_num > 0)
            {
                Transform coin =
                    LTBY_PoolManager.Instance.GetUiItem(this.coinPrefab, LTBY_EffectManager.Instance.UiLayer);
                var transform = coin.transform;
                transform.localScale = Vector3.one;
                transform.position = this.position;
                coinList.Add(coin);
                AppearAction(coin);
                _num--;
            }

            int key = LTBY_Extend.Instance.DelayRun(0.5f, ()=>
            {
                for (int i = 0; i < coinList.Count; i++)
                {
                    int index = i;
                    int _key = LTBY_Extend.Instance.DelayRun(0.1f * (index), () =>
                        {
                            if (coinList.Count <= index)
                            {
                                LTBY_EffectManager.Instance.DestroyEffect<FlyCoin>(this.chairId, this.id);
                                return;
                            }
                            MoveAction(index, coinList[index]);
                        });
                    timerKey.Add(_key);
                }
            });
            timerKey.Add(key);
        }

        /// <summary>
        /// 展示移动
        /// </summary>
        /// <param name="coin">移动的物体</param>
        private void AppearAction(Transform coin)
        {
            float rad = Mathf.Deg2Rad * 1 * (this.deltaAngle * (this.coinList.Count - 1) + this.initAngle);
            float radius = Random.Range(1f, 4f);
            float baseX = this.position.x;
            float baseY = this.position.y;
            float aimX = baseX + radius * Mathf.Cos(rad);
            float aimY = baseY + radius * Mathf.Sin(rad);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value=>
            {
                if (coin == null) return;
                float t = value * 0.01f;
                coin.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
            }, 0, 100, 0.3f).SetEase(Ease.OutSine));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            actionKey.Add(key);
        }

        /// <summary>
        /// 移动金币
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="coin">金币</param>
        private void MoveAction(int index, Transform coin)
        {
            var position1 = coin.position;
            float baseX = position1.x;
            float baseY = position1.y;
            float aimX = aimPos.x;
            float aimY = aimPos.y;
            float offsetX = (aimX - baseX) * 0.5f;
            float offsetY = aimY > baseY ? -15f : 15f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value=>
            {
                if (coin == null) return;
                float t = value * 0.01f;
                float x = aimX + offsetX * (1 - t);
                float y = aimY + offsetY * (1 - t);
                coin.position = new Vector3(baseX + (x - baseX) * t, baseY + (y - baseY) * t, 0);
                float _scale = 1 - 0.4f * t;
                coin.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.7f).SetEase(Ease.OutSine).OnComplete(()=>
            {
                if (index == 0) callBack?.Invoke();
                if (index == coinNum - 1)
                {
                    LTBY_EffectManager.Instance.DestroyEffect<FlyCoin>(this.chairId, this.id);
                }

                coin?.gameObject.SetActive(false);
            }));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            actionKey.Add(key);
        }

        /// <summary>
        /// 回收
        /// </summary>
        public override void OnStored()
        {
            base.OnStored();
            if (coinList == null) return;
            for (int i = 0; i < coinList.Count; i++)
            {
                LTBY_PoolManager.Instance.RemoveUiItem(this.coinPrefab, coinList[i]);
            }

            coinList?.Clear();
        }
    }

    public class GetItem : EffectClass
    {
        public class GetItemData
        {
            public string name;
            public int num;
            public string imageBundleName;
            public string imageName;
        }

        private CAction callBack;
        private Transform itemName;
        private Transform itemNum;
        private Image itemImage;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(GetItem);
            this.id = _id;
            this.chairId = _chairId;
            Vector3 worldPos = args.Length >= 1 ? (Vector3) args[0] : Vector3.zero;
            GetItemData config = args.Length >= 2 ? (GetItemData) args[1] : new GetItemData();
            this.callBack = args.Length >= 3 ? (CAction) args[2] : null;

            this.actionKey = new List<int>();

            this.item = this.item
                ? this.item
                : LTBY_Extend.Instance.LoadPrefab("LTBY_GetItem", LTBY_EffectManager.Instance.UiLayer);
            this.item.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(config.name))
            {
                this.itemName = this.itemName ? this.itemName : this.item.FindChildDepth("Name");
                this.itemName.gameObject.SetActive(true);
                this.itemName.GetComponent<Text>().text = config.name;
            }

            if (config.num > 0)
            {
                this.itemNum = this.itemNum ? this.itemNum : this.item.FindChildDepth("Num");
                this.itemNum.gameObject.SetActive(true);
                this.itemNum.GetComponent<NumberRoller>().text = config.num.ToString();
            }

            this.itemImage = this.itemImage ? this.itemImage : this.item.GetComponent<Image>();
            this.itemImage.sprite = LTBY_Extend.Instance.LoadSprite(config.imageBundleName, config.imageName);
            this.itemImage.SetNativeSize();

            this.item.position = worldPos;

            Vector3 aimPos = LTBY_GameView.GameInstance.GetBatteryWorldPos(_chairId);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value=>
            {
                if (this.item == null) return;
                float t = value * 0.01f;
                this.item.position = new Vector3(worldPos.x + (aimPos.x - worldPos.x) * t,
                    worldPos.y + (aimPos.y - worldPos.y) * t, 0);
                float _scale = 1 - t;
                this.item.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.5f).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(()=>
            {
                callBack?.Invoke();
                LTBY_EffectManager.Instance.DestroyEffect<GetItem>(this.chairId, this.id);
            }));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.item == null) return;
            this.item.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.item == null) return;
            LTBY_Extend.Destroy(this.item.gameObject);
            this.item = null;
        }
    }

    public class FreeLotteryDropItem : EffectClass
    {
        Vector3 aimPos;
        Vector3 worldPos;
        Text FrameName;
        Image OpenImage;
        Transform Open;
        Transform Close;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(FreeLotteryDropItem);
            this.id = _id;
            this.chairId = _chairId;
            EffectParamData data = args.Length >= 1 ? (EffectParamData) args[0] : new EffectParamData();
            this.actionKey = new List<int>();

            this.aimPos = LTBY_GameView.GameInstance.GetBatteryWorldPos(this.chairId);

            if (LTBY_GameView.GameInstance.CheckIsOtherSide(this.chairId))
            {
                this.worldPos = new Vector3(this.aimPos.x, this.aimPos.y - 6, 0);
            }
            else
            {
                this.worldPos = new Vector3(this.aimPos.x, this.aimPos.y + 6, 0);
            }

            this.item = this.item
                ? this.item
                : LTBY_Extend.Instance.LoadPrefab("LTBY_FreeLotteryDropItem", LTBY_EffectManager.Instance.UiLayer);
            this.item.gameObject.SetActive(true);
            this.item.position = this.worldPos;

            this.FrameName = this.FrameName ? this.FrameName : this.item.FindChildDepth<Text>("Open/NameFrame/Name");
            this.FrameName.text = data.award.key;
            this.OpenImage = this.OpenImage ? this.OpenImage : this.item.FindChildDepth<Image>("Open/Image");
            this.OpenImage.sprite = LTBY_Extend.Instance.LoadSprite("res_view", data.award.value);
            this.OpenImage.SetNativeSize();
            this.Open = this.Open ? this.Open : this.item.FindChildDepth("Open");
            this.Close = this.Open ? this.Open : this.item.FindChildDepth("Close");

            float aimX = this.aimPos.x;
            float aimY = this.aimPos.y;
            float baseX = this.worldPos.x;
            float baseY = this.worldPos.y;


            Sequence tween = DOTween.Sequence();
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Append(item.DOBlendableRotateBy(Vector3.one * 8, 0.04f).SetEase(Ease.Linear));
            sequence1.Append(item.DOBlendableRotateBy(Vector3.one * -16, 0.08f).SetEase(Ease.Linear));
            sequence1.Append(item.DOBlendableRotateBy(Vector3.one * 8, 0.04f).SetEase(Ease.Linear));
            sequence1.SetLoops(3);
            sequence1.OnComplete(()=>
            {
                this.Open.gameObject.SetActive(true);
                this.Close.gameObject.SetActive(false);
            });
            tween.Append(sequence1);
            Sequence sequence2 = DOTween.Sequence();
            sequence2.Append(DOTween.To(value =>
            {
                if (this.item == null) return;
                float t = value * 0.01f;
                this.item.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                float _scale = 1 - t;
                this.item.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.5f).SetEase(Ease.OutSine).OnComplete(()=>
            {
                if (data == null) return;
                int propId = data.award_cnt.id;
                int count = data.award_cnt.count;
                if (PropConfig.CheckIsCoin(propId))
                {
                    //DebugHelper.LogError($"玩家抽中金币，数量:{data.count}");
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(_chairId, count);
                }

                LTBY_EffectManager.Instance.DestroyEffect<FreeLotteryDropItem>(this.chairId, this.id);
            }).SetDelay(2));
            sequence2.SetLoops(3);
            sequence2.OnComplete(()=>
            {
                this.Open.gameObject.SetActive(true);
                this.Close.gameObject.SetActive(false);
            });
            tween.Insert(0, sequence2);
            int key = LTBY_Extend.Instance.RunAction(tween);
            this.actionKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.item == null) return;
            this.item.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.item == null) return;
            LTBY_Extend.Destroy(this.item.gameObject);
            this.item = null;
        }
    }

    public class FreeDropItem : EffectClass
    {
        Vector3 worldPos;
        string itemType;
        float time;
        int mul;
        Transform itemTime;
        Transform itemMul;
        Text itemTimeText;
        Text itemMulText;
        Vector3 aimPos;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(FreeDropItem);
            this.id = _id;
            this.chairId = _chairId;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.DropItem);
            }

            EffectParamData data = args.Length >= 1 ? (EffectParamData) args[0] : new EffectParamData();
            this.worldPos = data.worldPos;
            this.worldPos.z = 0;

            this.itemType = data.itemType;
            this.time = data.time;
            this.mul = data.mul;

            this.item = this.item
                ? this.item
                : LTBY_Extend.Instance.LoadPrefab("LTBY_FreeDropItem", LTBY_EffectManager.Instance.UiLayer);
            this.item.localScale = Vector3.one;
            this.item.gameObject.SetActive(true);
            this.item.position = this.worldPos;
            this.itemTime = this.item.FindChildDepth("Time");
            this.itemMul = this.item.FindChildDepth("Mul");
            this.itemTimeText = this.item.FindChildDepth<Text>("Time/Text");
            this.itemMulText = this.item.FindChildDepth<Text>("Mul/Text");

            switch (this.itemType)
            {
                case "FreeAddTime":
                    this.itemTime.gameObject.SetActive(true);
                    this.itemMul.gameObject.SetActive(false);
                    this.itemTimeText.text = $"+{this.time}{BatteryConfig.Second}";
                    this.aimPos = LTBY_BatteryManager.Instance.GetFreeBattery(this.chairId).GetTimeWorldPos();
                    break;
                case "FreeAddMul":
                    this.itemTime.gameObject.SetActive(false);
                    this.itemMul.gameObject.SetActive(true);
                    this.itemMulText.text = $"+{this.mul}{BatteryConfig.Unit} ";
                    this.aimPos = LTBY_BatteryManager.Instance.GetFreeBattery(this.chairId).GetMulWorldPos();
                    break;
            }

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            float aimX = this.aimPos.x;
            float aimY = this.aimPos.y;
            float baseX = this.worldPos.x;
            float baseY = this.worldPos.y;

            float offsetX = (aimX - baseX);
            float offsetY = aimY > baseY ? -15 : 15;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(item.DOBlendableScaleBy(new Vector3(0.7f, 0.7f, 1), 0.2f).SetEase(Ease.Linear));
            sequence.Append(item.DOBlendableScaleBy(new Vector3(-0.5f, -0.5f, 1), 0.2f).SetEase(Ease.Linear));
            sequence.Append(DOTween.To(value=>
            {
                if (this.item == null) return;
                float t = value * 0.01f;
                float x = aimX + offsetX * (1 - t);
                float y = aimY + offsetY * (1 - t);
                this.item.position = new Vector3(baseX + (x - baseX) * t, baseY + (y - baseY) * t, 0);

                float _scale = 1 - 0.5f * t;
                this.item.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.7f).SetDelay(0.6f));
            sequence.OnComplete(()=>
            {
                FreeBatteryClass aimBattery = LTBY_BatteryManager.Instance.GetFreeBattery(this.chairId);
                if (aimBattery == null) return;
                if (this.itemType.Equals("FreeAddTime"))
                {
                    aimBattery.AddTime(this.time);
                }
                else if (this.itemType.Equals("FreeAddMul"))
                {
                    aimBattery.AddMul(this.mul);
                }

                LTBY_EffectManager.Instance.DestroyEffect<FreeDropItem>(this.chairId, this.id);
            });
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.item == null) return;
            this.item.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.item == null) return;
            LTBY_Extend.Destroy(this.item.gameObject);
            this.item = null;
        }
    }

    public class DropItem : EffectClass
    {
        Vector3 worldPos;
        Vector3 angles;
        string itemType;
        int ratio;
        Vector3 aimPos;
        int propId;
        float time;
        int mul;
        float aimAngles;

        Image itemImage;
        Transform itemEffect;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(DropItem);
            this.id = _id;
            this.chairId = _chairId;

            EffectParamData config = args.Length >= 1 ? (EffectParamData) args[0] : new EffectParamData();
            this.worldPos = config.worldPos;
            this.worldPos.z = 0;
            this.angles = config.angles;
            this.itemType = config.itemType;
            this.ratio = config.ratio;
            this.aimPos = LTBY_GameView.GameInstance.GetBatteryWorldPos(this.chairId);
            this.propId = config.id;
            this.time = config.time;
            this.mul = config.mul;

            if (LTBY_GameView.GameInstance.CheckIsOtherSide(this.chairId))
            {
                this.aimPos.y -= 1.35f;
                this.aimAngles = 1080 + 180;
            }
            else
            {
                this.aimPos.y += 1.35f;
                this.aimAngles = 1080;
            }

            float delayCreate = 0.5f;
            if (this.itemType.Equals("ScratchCard") || this.itemType.Equals("Summon"))
            {
                this.aimAngles = 720;
            }
            else if (this.itemType.Equals("Missile"))
            {
                delayCreate = 0;
            }

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            if (delayCreate > 0)
            {
                int key = LTBY_Extend.Instance.DelayRun(delayCreate, CreateItem);
                this.timerKey.Add(key);
            }
            else
            {
                CreateItem();
            }
        }

        private void CreateItem()
        {
            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.DropItem);
            }

            EffectConfig.DropItem.TryGetValue(this.itemType, out DropItemData config);
            if (config == null)
            {
                DebugHelper.LogError($"GC.EffectConfig.DropItem {this.itemType} 没有配置");
            }

            this.item = this.item != null ? this.item : LTBY_Extend.Instance.LoadPrefab("LTBY_DropItem", LTBY_EffectManager.Instance.UiLayer);
            this.item.localScale = Vector3.one;
            this.item.gameObject.SetActive(true);
            this.item.position = this.worldPos;
            this.item.localEulerAngles = this.angles;

            this.itemImage = this.item.FindChildDepth<Image>("Image");
            this.itemEffect = this.item.FindChildDepth("Effect");
            if (config != null)
            {
                this.itemImage.sprite =
                    LTBY_Extend.Instance.LoadSprite(config.imageBundleName, config.imageName[this.propId]);
                this.itemImage.SetNativeSize();
                float _scale = config.scale;
                this.itemImage.transform.localScale = new Vector3(_scale, _scale, _scale);
            }

            this.itemImage.gameObject.SetActive(true);

            float aimX = this.aimPos.x;
            float aimY = this.aimPos.y;
            float baseX = this.worldPos.x;
            float baseY = this.worldPos.y;

            float baseAngles = this.angles.z;
            float _aimAngles = this.aimAngles;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.DOBlendableScaleBy(new Vector3(0.5f, 0.5f, 1), 0.2f).SetEase(Ease.Linear));
            sequence.Append(item.DOBlendableScaleBy(new Vector3(-0.5f, -0.5f, 1), 0.2f).SetEase(Ease.Linear));
            sequence.Append(DOTween.To(value=>
            {
                if (this.item == null) return;
                float t = value * 0.01f;
                this.item.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                this.item.localEulerAngles = new Vector3(0, 0, baseAngles + (_aimAngles - baseAngles) * t);
            }, 0, 100, 0.8f).SetEase(Ease.OutSine).SetDelay(1).OnComplete(OnFinish));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        private void OnFinish()
        {
            this.itemImage.gameObject.SetActive(false);
            this.itemEffect.gameObject.SetActive(false);
            this.itemEffect.gameObject.SetActive(true);
            int key = LTBY_Extend.Instance.DelayRun(0.5f,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<DropItem>(this.chairId, this.id); });
            this.timerKey.Add(key);

            switch (this.itemType)
            {
                case "Drill":
                    LTBY_BatteryManager.Instance.CreateDrill(this.chairId, this.ratio);
                    break;
                case "Electric":
                    LTBY_BatteryManager.Instance.CreateElectric(this.chairId, this.ratio);
                    break;
                case "FreeBattery":
                    LTBY_BatteryManager.Instance.CreateFreeBattery(this.chairId, this.ratio, this.mul, this.time);
                    break;
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.item == null) return;
            this.item.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.item == null) return;
            LTBY_Extend.Destroy(this.item.gameObject);
            this.item = null;
        }
    }

    public class CoinOutburst : EffectClass
    {
        Transform outburst1;
        Transform outburst2;
        int devide;
        int halfDevide;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(CoinOutburst);
            this.id = _id;
            this.chairId = _chairId;

            this.score = args.Length >= 1 ? (long) args[0] : 0;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                SelfEffect();
            }
            else
            {
                OtherEffect();
            }
        }

        private void SelfEffect()
        {
            LTBY_Audio.Instance.Play(SoundConfig.CoinOutburst);

            Vector3 pos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);

            this.effect = this.effect != null ? this.effect : LTBY_PoolManager.Instance.GetUiItem("LTBY_CoinOutburst", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.position = pos;

            this.outburst1 = this.outburst1 != null ? this.outburst1 : this.effect.FindChildDepth("Effect1");
            this.outburst1.gameObject.SetActive(false);
            this.outburst2 = this.outburst2 != null ? this.outburst2 : this.effect.FindChildDepth("Effect2");
            this.outburst2.gameObject.SetActive(false);

            this.devide = 10;
            this.halfDevide = 5;

            CalcShowScore();

            Vector3 worldPos = new Vector3(pos.x, pos.y, 0);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(LTBY_Extend.DelayRun(0).OnComplete(() =>
            {
                this.outburst1?.gameObject.SetActive(true);
                LTBY_EffectManager.Instance.CreateEffect<FlyText>(this.chairId, "Outburst", 1, worldPos, GetCurShowScore());
            }));
            sequence.Append(LTBY_Extend.DelayRun(2).OnComplete(() => this.outburst2.gameObject.SetActive(false)));
            sequence.SetLoops(halfDevide);
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            int _key = LTBY_Extend.Instance.DelayRun(1, () =>
            {
                Sequence sq = DOTween.Sequence();
                Tween sequence1 = LTBY_Extend.DelayRun(0).OnComplete(() =>
                {
                    this.outburst2?.gameObject.SetActive(true);
                    LTBY_EffectManager.Instance.CreateEffect<FlyText>(this.chairId, "Outburst", 1, worldPos, GetCurShowScore());
                });
                sq.Append(sequence1);
                Tween sequence2 = LTBY_Extend.DelayRun(2).OnComplete(() => this.outburst2?.gameObject.SetActive(false));
                sq.Append(sequence2);
                sq.SetLoops(halfDevide).OnComplete(() =>
                    LTBY_EffectManager.Instance.DestroyEffect<CoinOutburst>(this.chairId, this.id));
                int tween = LTBY_Extend.Instance.RunAction(sq);
                this.actionKey.Add(tween);
            });
            this.timerKey.Add(_key);
        }

        int count;
        List<long> scoreList;

        private void CalcShowScore()
        {
            this.count = 0;
            this.scoreList = new List<long>();

            long lastScore = this.score;
            for (int i = 0; i < this.devide - 1; i++)
            {
                long _score = (long) Mathf.Floor(this.score * Random.Range(3, 11f) / 100);

                this.scoreList.Add(_score);
                lastScore -= _score;
            }

            this.scoreList.Insert(Random.Range(1, this.scoreList.Count-1), lastScore);
        }

        private long GetCurShowScore()
        {
            if (this.count >= this.devide) return 0;
            long _score = this.scoreList[this.count];
            this.count++;
            return _score;
        }

        private void OtherEffect()
        {
            Vector3 worldPos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
            LTBY_EffectManager.Instance.CreateEffect<FlyText>(this.chairId, "Outburst", 2.5f, worldPos, this.score);
            LTBY_EffectManager.Instance.DestroyEffect<CoinOutburst>(this.chairId, this.id);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_CoinOutburst", this.effect);
            this.effect = null;
        }
    }

    public class PoolTreasure : EffectClass
    {
        Transform EffectPart;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(PoolTreasure);
            this.id = _id;
            this.chairId = _chairId;
            int level = args.Length >= 1 ? (int) args[0] : 0;

            this.score = args.Length >= 2 ? (long) args[1] : 0;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            LTBY_Audio.Instance.Play(SoundConfig.AllAudio[$"Treasure{level}"]);

            LTBY_Audio.Instance.Play(SoundConfig.TreasureText);

            this.effect =
                LTBY_Extend.Instance.LoadPrefab($"LTBY_PoolTreasure{level}", LTBY_EffectManager.Instance.UiLayer);
            this.effect.position = new Vector3(0, 0, 0);

            Transform numtxt = this.effect.FindChildDepth("CaiDai/Text");
            this.num = numtxt.gameObject.GetILComponent<NumberRoller>();
            if (num == null)
            {
                num = numtxt.gameObject.AddILComponent<NumberRoller>();
            }

            num.Init();
            this.num.text = "0";
            this.EffectPart = this.effect.FindChildDepth("Effect");
            this.EffectPart.gameObject.SetActive(!LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score));

            int key = LTBY_Extend.Instance.DelayRun(1, ()=>
            {
                EffectParamData _data = new EffectParamData()
                {
                    chairId = _chairId,
                    pos = Vector3.zero,
                    scale = 1
                };
                LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
            });

            this.num.transform.localScale = Vector3.one;
            this.num.transform.gameObject.SetActive(false);

            float delay = 0.5f;

            int _key = LTBY_Extend.Instance.DelayRun(delay, ()=>
            {
                this.num?.transform.gameObject.SetActive(true);
                this.num?.RollTo(this.score, 4);
            });
            this.timerKey.Add(_key);

            delay += 4;

            int _key1 = LTBY_Extend.Instance.DelayRun(delay, ()=>
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(num.transform.DOBlendableScaleBy(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.Linear));
                sequence.Append(num.transform.DOBlendableScaleBy(new Vector3(-1, -1, 1), 0.2f).SetEase(Ease.Linear));
                int kkey = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(kkey);
            });
            this.timerKey.Add(_key1);

            delay++;

            int _key2 = LTBY_Extend.Instance.DelayRun(delay, ()=>
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(effect?.DOScale(new Vector3(0, 0, 1), 0.5f).SetEase(Ease.InBack).OnComplete(()=>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<PoolTreasure>(this.chairId, this.id);
                }));
                int kkey = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(kkey);
            });
            this.timerKey.Add(_key2);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            LTBY_Extend.Destroy(this.effect.gameObject);
            this.effect = null;
        }
    }

    public class MissileSpineAward : EffectClass
    {
        int settleAccountActionKey = -1;
        float numScale;
        Transform image;
        Image image2;
        bool isSettleAccount;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(MissileSpineAward);
            this.id = _id;
            this.chairId = _chairId;
            this.score = 0;
            int propId = args.Length >= 1 ? (int) args[0] : 0;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward3);
            }

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();
            this.settleAccountActionKey = -1;

            this.account = this.account
                ? this.account
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_MissileSpineAward", LTBY_EffectManager.Instance.UiLayer);
            this.account.gameObject.SetActive(true);
            this.skeleton = this.skeleton!=null ? this.skeleton : this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            if (this.skeleton != null)
            {
                this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.account.localScale = new Vector3(1, 1, 1);

            this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
            Transform numtxt = this.account.FindChildDepth("Num");
            this.num = numtxt.gameObject.GetILComponent<NumberRoller>() != null
                ? numtxt.gameObject.GetILComponent<NumberRoller>()
                : numtxt.gameObject.AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.text = "0";
            this.numPosition = this.num.transform.position;
            this.numScale = 0.45f;

            this.num.transform.gameObject.SetActive(false);

            this.image = this.image ? this.image : this.account.FindChildDepth("Image");
            this.image.localEulerAngles = new Vector3(0, 0, 0);
            this.image.localScale = new Vector3(1, 1, 1);
            DropItemData config = EffectConfig.DropItem["Missile"];
            this.image2 = this.image2 ? this.image2 : this.image.FindChildDepth<Image>("Image");
            if (!string.IsNullOrEmpty(config.imageName[propId]))
            {
                this.image2.sprite = LTBY_Extend.Instance.LoadSprite(config.imageBundleName, config.imageName[propId]);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOBlendableScaleBy(new Vector3(3, 3, 1), 0.01f).SetEase(Ease.Linear));
            sequence.Append(image.DOBlendableScaleBy(new Vector3(-3, -3, 1), 0.2f).SetEase(Ease.Linear));
            Sequence sequence1 = DOTween.Sequence();
            sequence.Append(sequence);
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, -20), 0.4f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.SetLoops(20);
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            this.isSettleAccount = false;
        }

        public override void AddScore(long _score, float roll = 0)
        {
            base.AddScore(_score, roll);
            if (this.isSettleAccount)
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, _score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, _score);
            }
            else
            {
                this.num.transform.gameObject.SetActive(true);
                this.score += (long) _score;
                if (roll > 0)
                {
                    this.num.RollTo(this.score, roll);
                }
                else
                {
                    this.num.text = this.score.ToString();
                }

                Sequence sequence = DOTween.Sequence();
                sequence.Append(num?.transform?.DOScale(new Vector3(this.numScale + 0.3f, this.numScale + 0.3f, 1), 0.1f)
                    .SetEase(Ease.Linear));
                sequence.Append(num?.transform?.DOScale(new Vector3(this.numScale, this.numScale, 1), 0.1f)
                    .SetEase(Ease.Linear));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
        }

        public void SettleAccount(float delay = 0)
        {
            if (this.isSettleAccount) return;

            Vector3 basePos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);

            this.account.position = basePos;
            this.account.localScale = new Vector3(1, 1, 1);

            float baseX = basePos.x;
            float baseY = basePos.y;

            Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
            float aimX = aimPos.x;
            float aimY = aimPos.y;

            LTBY_Extend.Instance.StopAction(this.settleAccountActionKey);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value=>
            {
                if (this.account == null) return;
                float t = value * 0.01f;
                this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                float _scale = 1 - t;
                this.account.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.3f).SetEase(Ease.InBack).OnComplete(()=>
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                LTBY_EffectManager.Instance.DestroyMissile(this.chairId, this.id);
            }).SetDelay(delay));
            this.settleAccountActionKey = LTBY_Extend.Instance.RunAction(sequence);
        }

        public override void OnStored()
        {
            base.OnStored();
            this.isSettleAccount = true;

            if (this.settleAccountActionKey > 0)
            {
                LTBY_Extend.Instance.StopAction(this.settleAccountActionKey);
                this.settleAccountActionKey = -1;
            }

            if (this.account != null)
            {
                this.account.gameObject.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!this.account) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_MissileSpineAward", this.account);
            this.account = null;
        }
    }

    public class SpecialBatterySpineAward : EffectClass
    {
        List<long> scoreList;
        int settleAccountActionKey = -1;
        string batteryType;
        float numScale;
        Transform image;
        bool isSettleAccount;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(SpecialBatterySpineAward);
            this.chairId = _chairId;
            this.score = 0;
            this.scoreList = new List<long>();

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward3);
            }

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();
            this.settleAccountActionKey = -1;

            this.batteryType = args.Length >= 1 ? args[0].ToString() : "";

            this.prefabName = $"LTBY_{this.batteryType}SpineAward";

            this.account = LTBY_PoolManager.Instance.GetUiItem(this.prefabName, LTBY_EffectManager.Instance.UiLayer);

            skeleton = this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            if (skeleton != null)
            {
                skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.account.localScale = new Vector3(1, 1, 1);

            this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
            Transform numTxt = this.account.FindChildDepth("Num");
            this.num = numTxt.gameObject.GetILComponent<NumberRoller>();
            this.num = this.num == null ? numTxt.gameObject.AddILComponent<NumberRoller>() : this.num;
            this.num.Init();
            this.num.text = "0";
            this.numPosition = this.num.transform.position;
            this.numScale = 0.45f;

            this.num.transform.gameObject.SetActive(false);

            this.image = this.account.FindChildDepth("Image");
            this.image.localEulerAngles = new Vector3(0, 0, 0);
            this.image.localScale = new Vector3(1, 1, 1);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOBlendableScaleBy(new Vector3(3, 3, 1), 0.01f).SetEase(Ease.Linear));
            sequence.Append(image.DOBlendableScaleBy(new Vector3(-3, -3, 1), 0.2f).SetEase(Ease.Linear));
            Sequence sequence1 = DOTween.Sequence();
            sequence.Append(sequence1);
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, -20), 0.4f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.SetLoops(100);
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            this.isSettleAccount = false;
        }

        public override void AddScore(long _score, float roll = 0)
        {
            base.AddScore(_score, roll);
            if (this.isSettleAccount)
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, _score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, _score);
            }
            else
            {
                this.num.transform.gameObject.SetActive(true);
                this.score += (long) _score;
                this.scoreList.Add((long) _score);
                if (roll > 0)
                {
                    this.num.RollTo(this.score, roll);
                }
                else
                {
                    this.num.text = this.score.ToString();
                }

                Sequence sequence = DOTween.Sequence();
                sequence.Append(num?.transform?.DOScale(new Vector3(this.numScale + 0.3f, this.numScale + 0.3f, 1), 0.1f)
                    .SetEase(Ease.Linear));
                sequence.Append(num?.transform?.DOScale(new Vector3(this.numScale, this.numScale, 1), 0.1f)
                    .SetEase(Ease.Linear));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
        }

        public override void SettleAccount()
        {
            base.SettleAccount();
            Vector3 basePos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);

            this.account.position = basePos;
            this.account.localScale = new Vector3(1, 1, 1);

            float baseX = basePos.x;
            float baseY = basePos.y;

            Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
            float aimX = aimPos.x;
            float aimY = aimPos.y;

            LTBY_Extend.Instance.StopAction(this.settleAccountActionKey);

            float delay = 0;

            if (this.score >= EffectConfig.UseFireworkScore)
            {
                EffectParamData _data = new EffectParamData()
                {
                    chairId = chairId,
                    pos = basePos,
                    scale = 0.5f
                };
                LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
                delay += 5;
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value=>
            {
                if (this.account == null) return;
                float t = value * 0.01f;
                this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                float _scale = 1 - t;
                this.account.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.5f).SetEase(Ease.InBack).SetDelay(delay).OnComplete(()=>
            {
                this.isSettleAccount = true;
                this.account?.gameObject.SetActive(false);
                for (int i = 0; i < scoreList.Count; i++)
                {
                    int index = i;
                    Tween sequence1= LTBY_Extend.DelayRun(index * 0.15f).OnComplete(()=>
                    {
                        LTBY_GameView.GameInstance.AddScore(this.chairId, scoreList[index]);
                        LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, scoreList[index], true);
                        if (index != this.scoreList.Count - 1) return;
                        switch (this.batteryType)
                        {
                            case "Drill":
                                LTBY_EffectManager.Instance.DestroyDrill(this.chairId);
                                break;
                            case "Electric":
                                LTBY_EffectManager.Instance.DestroyElectric(this.chairId);
                                break;
                            case "FreeBattery":
                                LTBY_EffectManager.Instance.DestroyFreeBattery(this.chairId);
                                break;
                        }
                    });
                    int key = LTBY_Extend.Instance.RunAction(sequence1);
                    this.actionKey.Add(key);
                }
            }));
            this.settleAccountActionKey = LTBY_Extend.Instance.RunAction(sequence);
        }

        public override void OnStored()
        {
            base.OnStored();
            this.isSettleAccount = true;

            if (this.settleAccountActionKey > 0)
            {
                LTBY_Extend.Instance.StopAction(this.settleAccountActionKey);
                this.settleAccountActionKey = -1;
            }

            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.prefabName, this.account);
            this.account = null;
        }
    }

    public class SpineAwardLevel : EffectClass
    {
        Transform image;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(SpineAwardLevel);
            this.id = _id;
            this.chairId = _chairId;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();
            int level = args.Length >= 1 ? (int) args[0] : 0;
            int fishType = args.Length >= 1 ? (int) args[1] : 0;
            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward2);
                if (SoundConfig.AllAudio.ContainsKey($"SpineAwardText{level}"))
                {
                    LTBY_Audio.Instance.Play(SoundConfig.AllAudio[$"SpineAwardText{level}"]);
                }
            }

            this.prefabName = $"LTBY_SpineAwardLevel{level}";
            this.account = LTBY_PoolManager.Instance.GetUiItem(this.prefabName, LTBY_EffectManager.Instance.UiLayer);

            skeleton = this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            if (skeleton != null)
            {
                skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.account.localScale = new Vector3(1, 1, 1);

            this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
            this.num = this.account.FindChildDepth("Num").gameObject.GetILComponent<NumberRoller>() ??
                       this.account.FindChildDepth("Num").gameObject.AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.text = "0";
            this.numPosition = this.num.transform.position;

            this.num.transform.gameObject.SetActive(false);

            this.image = this.account.FindChildDepth("Image");
            this.image.localScale = Vector3.one;
            this.image.GetComponent<Image>().sprite =
                LTBY_Extend.Instance.LoadSprite("res_fishmodel", $"fish_{fishType}");
            this.image.localEulerAngles = new Vector3(0, 0, 0);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOBlendableScaleBy(new Vector3(3, 3, 1), 0.01f).SetEase(Ease.Linear));
            sequence.Append(image.DOBlendableScaleBy(new Vector3(-3, -3, 1), 0.2f).SetEase(Ease.Linear));
            Sequence sequence1 = DOTween.Sequence();
            sequence.Append(sequence1);
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, -10), 0.4f).SetEase(Ease.Linear));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.SetLoops(10);
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        public override void AddScore(long _score, float roll = 0)
        {
            base.AddScore(_score, roll);
            this.num.transform.gameObject.SetActive(true);

            this.score = _score;

            this.num.RollTo(this.score, 2);

            var position = this.account.position;
            float baseX = position.x;
            float baseY = position.y;
            Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
            float aimX = aimPos.x;
            float aimY = aimPos.y;

            float delay = 4;

            if (this.score >= EffectConfig.UseFireworkScore)
            {
                int key = LTBY_Extend.Instance.DelayRun(0, ()=>
                {
                    EffectParamData _data = new EffectParamData()
                    {
                        chairId = chairId,
                        pos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                        scale = 0.5f
                    };
                    LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
                });
                this.timerKey.Add(key);
            }

            if (LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score))
            {
                delay++;
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value =>
            {
                if (this.account == null) return;
                float t = value * 0.01f;
                this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                float _scale = 1 - t;
                this.account.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                LTBY_EffectManager.Instance.DestroyEffect<SpineAwardLevel>(this.chairId, this.id);
            }).SetDelay(delay));
            int _key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(_key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.prefabName, this.account);
            this.account = null;
        }
    }

    public class SpineAwardFullScreen : EffectClass
    {
        Transform JinBiPart;
        Transform image;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(SpineAwardFullScreen);
            this.id = _id;
            this.chairId = _chairId;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();
            int fishType = args.Length >= 1 ? (int) args[0] : 0;
            this.score = args.Length >= 2 ? (long) args[1] : 0;
            float delay = args.Length >= 3 ? (float) args[2] : 0;

            if (this.score >= EffectConfig.UseFireworkScore)
            {
                int key = LTBY_Extend.Instance.DelayRun(1, ()=>
                {
                    EffectParamData _data = new EffectParamData()
                    {
                        chairId = _chairId,
                        pos = Vector3.zero
                    };
                    LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
                });
                this.timerKey.Add(key);
            }

            int _key = LTBY_Extend.Instance.DelayRun(delay, ()=>
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward2);

                LTBY_Audio.Instance.Play(SoundConfig.SpineAwardText4);

            this.account = this.account != null ? this.account : LTBY_PoolManager.Instance.GetUiItem("LTBY_SpineAwardFullScreen",
                        LTBY_EffectManager.Instance.UiLayer);
                this.account.gameObject.SetActive(true);
                this.account.position = new Vector3(0, 0, 0);

                this.JinBiPart = this.JinBiPart != null ? this.JinBiPart : this.account.FindChildDepth("Particle System/JinBiPart");

                this.JinBiPart.gameObject.SetActive(!LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score));

                this.skeleton = this.skeleton!=null ? this.skeleton : this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
                if (this.skeleton != null)
                {
                    this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
                }

                this.account.localScale = new Vector3(1, 1, 1);

                this.num = this.num!=null ? this.num : this.account.FindChildDepth("Num").GetILComponent<NumberRoller>();
                this.num = this.num!=null ? this.num : this.account.FindChildDepth("Num").AddILComponent<NumberRoller>();
                this.num.Init();
                this.num.text = "0";
                this.num.RollTo(this.score, 3);

                this.image = this.image!=null ? this.image : this.account.FindChildDepth("Image");
                this.image.GetComponent<Image>().sprite =
                    LTBY_Extend.Instance.LoadSprite("res_fishmodel", $"fish_{fishType}");
                this.image.localEulerAngles = new Vector3(0, 0, 0);
                this.image.localScale = Vector3.one;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(image.DOBlendableScaleBy(new Vector3(3, 3, 1), 0.01f).SetEase(Ease.Linear));
                sequence.Append(image.DOBlendableScaleBy(new Vector3(-3, -3, 1), 0.2f).SetEase(Ease.Linear));
                Sequence sequence1 = DOTween.Sequence();
                sequence.Append(sequence1);
                sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
                sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, -10), 0.4f).SetEase(Ease.Linear));
                sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
                sequence1.SetLoops(10);

                int _key1 = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(_key1);

                float delayTime = 4.5f;
                if (LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score))
                {
                    delayTime = 5.8f;
                }

                Sequence sequence2 = DOTween.Sequence();
                sequence2.Append(DOTween.To(value =>
                {
                    if (this.account == null) return;
                    float t = value * 0.01f;
                    float _scale = 1 - t;
                    this.account.localScale = new Vector3(_scale, _scale, _scale);
                }, 0, 100, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<SpineAwardFullScreen>(this.chairId, this.id);
                }).SetDelay(delayTime));
                int _key2 = LTBY_Extend.Instance.RunAction(sequence2);
                this.actionKey.Add(_key2);
            });
            this.timerKey.Add(_key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.account == null) return;
            this.account.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_SpineAwardFullScreen", this.account);
            this.account = null;
        }
    }

    public class DragonSpineAward : EffectClass
    {
        long baseScore;
        bool isSelf;
        Transform effect1;
        Transform effect2;
        Transform numNode;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(DragonSpineAward);
            this.id = _id;
            this.chairId = _chairId;
            this.score = args.Length >= 1 ? (long) args[0] : 0;
            this.baseScore = args.Length >= 2 ? (long) args[1] : 0;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            this.account = this.account != null ? this.account : LTBY_PoolManager.Instance.GetUiItem("LTBY_DragonSpineAward", LTBY_EffectManager.Instance.UiLayer);
            this.account.gameObject.SetActive(true);
            this.skeleton = this.skeleton!= null ? this.skeleton : this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            this.skeleton.AnimationState.SetAnimation(0, "stand01", false);

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.DragonSpineAward);
                this.isSelf = true;
                this.account.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                this.account.position = new Vector3(0, 0, 0);
                this.effect1 = this.account.FindChildDepth("Effect1_1");
                this.effect2 = this.account.FindChildDepth("Effect1_2");
            }
            else
            {
                this.isSelf = false;
                this.account.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
                this.effect1 = this.account.FindChildDepth("Effect2_1");
                this.effect2 = this.account.FindChildDepth("Effect2_2");
            }


            this.numNode = this.numNode!=null ? this.numNode : this.account.FindChildDepth("Num");
            this.numNode.gameObject.SetActive(false);
            this.num = this.num!=null ? this.num : this.numNode.GetILComponent<NumberRoller>();
            this.num = this.num!=null ? this.num : this.numNode.AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.text = "0";

            int key = LTBY_Extend.Instance.DelayRun(2, ()=> { this.skeleton?.AnimationState?.SetAnimation(1, "stand02", true); });
            this.timerKey.Add(key);

            RollNum();
        }

        float numScale;

        private void RollNum()
        {
            this.numScale = 0.65f;
            this.numNode.localScale = new Vector3(this.numScale, this.numScale, this.numScale);
            int count = 0;
            Queue<long> numList = new Queue<long>();
            long bulletRatio = this.score / this.baseScore;
            Queue<float> scaleList = new Queue<float>();

            long ratio = 0;
            if (this.baseScore >= 100)
            {
                count = 1;
                ratio = this.baseScore < 200 ? this.baseScore : 100;
                numList.Enqueue(bulletRatio * ratio);
                scaleList.Enqueue(0.75f);
            }

            if (this.baseScore >= 200)
            {
                count = 2;
                ratio = this.baseScore < 300 ? this.baseScore : 200;
                numList.Enqueue(bulletRatio * ratio);
                scaleList.Enqueue(0.8f);
            }

            if (this.baseScore >= 300)
            {
                count = 3;
                ratio = this.baseScore < 400 ? this.baseScore : 300;
                numList.Enqueue(bulletRatio * ratio);
                scaleList.Enqueue(0.85f);
            }

            if (this.baseScore >= 400)
            {
                count = 4;
                ratio = this.baseScore < 500 ? this.baseScore : 400;
                numList.Enqueue(bulletRatio * ratio);
                scaleList.Enqueue(0.9f);
            }

            if (this.baseScore >= 500)
            {
                count = 5;
                numList.Enqueue(bulletRatio * 500);
                scaleList.Enqueue(1);
            }

            float delay = 0;
            for (int i = 0; i < count; i++)
            {
                int index = i;
                long to = numList.Dequeue();

                float data = scaleList.Dequeue();
                Sequence sequence = DOTween.Sequence();
                sequence.Append(LTBY_Extend.DelayRun(delay + 1).OnComplete(() =>
                {
                    this.numNode.gameObject.SetActive(true);
                    this.num.RollTo(to, 1);
                }));
                sequence.Append(LTBY_Extend.DelayRun(0.6f).OnComplete(() =>
                {
                    LTBY_Audio.Instance.Play(SoundConfig.DragonEffect2);

                    //最后一次加分的时候 播放烟花
                    if (index == count - 1)
                    {
                        //当达到一亿分的时候 隐藏这个爆金币的特效，用一亿分的特效代替
                        if (!(this.isSelf && LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score)))
                        {
                            this.effect2?.gameObject.SetActive(false);
                            this.effect2?.gameObject.SetActive(true);
                        }


                        if (this.score < EffectConfig.UseFireworkScore) return;
                        EffectParamData _data = new EffectParamData()
                        {
                            chairId = this.chairId,
                            pos = this.isSelf ? new Vector3(0, 0, 0) : LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                            scale = this.isSelf ? 1f : 0.5f
                        };
                        LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
                    }
                    else
                    {
                        this.effect2?.gameObject.SetActive(false);
                        this.effect2?.gameObject.SetActive(true);
                    }
                }));
                sequence.Append(LTBY_Extend.DelayRun(0.4f).OnComplete(() =>
                {
                    this.effect1?.gameObject.SetActive(false);
                    this.effect1?.gameObject.SetActive(true);
                }));
                sequence.Append(numNode?.DOScale(new Vector3(data * 1.5f, data * 1.5f, 1), 0.04f)
                    .SetEase(Ease.Linear));
                sequence.Append(numNode?.DOScale(new Vector3(data, data, 1), 0.7f).SetEase(Ease.OutSine));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
                delay += 2;
            }

            if (this.score >= EffectConfig.UseFireworkScore)
            {
                delay += 2;
            }

            if (this.isSelf)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(LTBY_Extend.DelayRun(delay + 2).OnComplete(() =>
                {
                    this.effect1?.gameObject.SetActive(false);
                    this.effect2?.gameObject.SetActive(false);
                }));
                sequence.Append(account?.DOScale(new Vector3(0, 0, 1), 0.4f).SetEase(Ease.InBack).SetDelay(0.2f));
                sequence.OnComplete(() =>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<DragonSpineAward>(this.chairId, this.id);
                });
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
            else
            {
                var position = this.account.position;
                float baseX = position.x;
                float baseY = position.y;
                Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
                float aimX = aimPos.x;
                float aimY = aimPos.y;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(DOTween.To(value =>
                {
                    if (this.account == null) return;
                    float t = value * 0.01f;
                    this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                    float _scale = (1 - t) * 0.7f;
                    this.account.localScale = new Vector3(_scale, _scale, _scale);
                }, 0, 100, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<DragonSpineAward>(this.chairId, this.id);
                }).SetDelay(delay + 2));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            this.skeleton?.AnimationState.ClearTracks();

            if (this.account != null)
            {
                this.account.gameObject.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_DragonSpineAward", this.account);
            this.account = null;
        }
    }

    public class Wheel : EffectClass
    {
        Vector3 aimPos;
        string showType;
        Action callBack;
        Text effectMul;
        Transform nodeText;
        Image image;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(Wheel);
            this.id = _id;
            this.chairId = _chairId;
            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();
            this.showType = data.showType;
            this.score = data.showScore;
            this.callBack = data.callBack;
            this.aimPos = data.aimPos == Vector3.zero
                ? LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId)
                : data.aimPos;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId) && data.playSound)
            {
                LTBY_Audio.Instance.Play(SoundConfig.Wheel);
            }

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_Wheel", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.localScale = new Vector3(0.5f, 0.5f, 1);
            this.effect.position = data.pos;

            this.skeleton =
                this.skeleton !=null? this.skeleton : this.effect.FindChildDepth<SkeletonGraphic>("Node/Skeleton");
            if (this.skeleton!=null)
            {
                this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.effectMul = this.effectMul!=null ? this.effectMul : this.effect.FindChildDepth<Text>("Mul");

            if (data.useMul > 0)
            {
                this.effectMul.gameObject.SetActive(true);
                this.effectMul.text = $"x{data.useMul}";
                this.score /= data.useMul;
            }
            else
            {
                this.effectMul.gameObject.SetActive(false);
            }


            this.num = this.num!=null ? this.num : this.effect.FindChildDepth("Node/Num").GetILComponent<NumberRoller>();
            this.num = this.num!=null ? this.num : this.effect.FindChildDepth("Node/Num").AddILComponent<NumberRoller>();
            this.num.Init();
            this.nodeText = this.nodeText!=null ? this.nodeText : this.effect.FindChildDepth("Node/Text");
            if (this.showType.Equals("Score"))
            {
                this.num.gameObject.SetActive(true);
                this.num.RollFromTo(0, this.score, 0.7f);
                this.nodeText.gameObject.SetActive(false);
            }
            else
            {
                this.nodeText.gameObject.SetActive(true);
                this.num.gameObject.SetActive(false);
            }

            this.num.transform.localEulerAngles = new Vector3(0, 0, 0);

            this.image = this.image!=null ? this.image : this.effect.FindChildDepth<Image>("Node/Image");
            this.image.sprite = LTBY_Extend.Instance.LoadSprite("res_fishmodel", $"fish_{data.fishType}");
            this.image.transform.localEulerAngles = new Vector3(0, 0, 0);

            this.actionKey = new List<int>();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(effect?.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutElastic));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            Sequence sequence1 = DOTween.Sequence();
            sequence1.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, -20), 0.4f).SetEase(Ease.Linear));
            sequence1.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.Linear));
            sequence1.SetLoops(2);
            int _key = LTBY_Extend.Instance.RunAction(sequence1);
            this.actionKey.Add(_key);

            Sequence sequence2 = DOTween.Sequence();
            sequence2.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, 7), 0.2f).SetEase(Ease.Linear));
            sequence2.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, -14), 0.4f).SetEase(Ease.Linear));
            sequence2.Append(image.transform.DOBlendableRotateBy(new Vector3(0, 0, 7), 0.2f).SetEase(Ease.Linear));
            sequence2.SetLoops(2);
            sequence2.OnComplete(MoveAction);
            int _key1 = LTBY_Extend.Instance.RunAction(sequence2);
            this.actionKey.Add(_key);
        }

        private void MoveAction()
        {
            if (this.showType.Equals("Score"))
            {
                var position = this.effect.position;
                float baseX = position.x;
                float baseY = position.y;

                float aimX = this.aimPos.x;
                float aimY = this.aimPos.y;

                float offsetX = (aimX - baseX) * 0.5f;
                float offsetY = aimY > baseY ? -15 : 15;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(DOTween.To(value =>
                {
                    if (this.effect == null) return;
                    float t = value * 0.01f;
                    float x = aimX + offsetX * (1 - t);
                    float y = aimY + offsetY * (1 - t);
                    this.effect.position = new Vector3(baseX + (x - baseX) * t, baseY + (y - baseY) * t, 0);

                    float _scale = 1 - 0.9f * t;
                    this.effect.localScale = new Vector3(_scale, _scale, _scale);
                }, 0, 100, 0.5f).OnComplete(() =>
                {
                    this.callBack?.Invoke();
                    LTBY_EffectManager.Instance.DestroyEffect<Wheel>(this.chairId, this.id);
                }));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
            else
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(effect?.DOScale(new Vector3(0, 0, 1), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.callBack?.Invoke();
                    LTBY_EffectManager.Instance.DestroyEffect<Wheel>(this.chairId, this.id);
                }));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            this.callBack = null;

            if (this.effect != null)
            {
                this.effect.gameObject.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_Wheel", this.effect);
            this.effect = null;
        }
    }

    public class FlyText : EffectClass
    {
        Transform textNode;
        NumberRoller numRoller;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(FlyText);
            this.id = _id;
            this.chairId = _chairId;

            this.actionKey = new List<int>();
            string textType = args.Length >= 1 ? args[0].ToString() : "";
            float interval = args.Length >= 2 ? (float) args[1] : 0;
            Vector3 worldPos = args.Length >= 3 ? (Vector3) args[2] : Vector3.zero;
            try
            {
                this.score = args.Length >= 4 ? (long) args[3] : 0;
            }
            catch (Exception e)
            {
                this.score = 0;
            }

            this.textNode = this.textNode!=null
                ? this.textNode
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_TextNode", LTBY_EffectManager.Instance.UiLayer);
            this.textNode.gameObject.SetActive(true);
            this.textNode.localScale = new Vector3(1, 1, 1);
            this.textNode.position = worldPos;
            this.numRoller = this.numRoller!=null
                ? this.numRoller
                : this.textNode.FindChildDepth("Num").GetILComponent<NumberRoller>();
            this.numRoller = this.numRoller!=null
                ? this.numRoller
                : this.textNode.FindChildDepth("Num").AddILComponent<NumberRoller>();
            this.numRoller.Init();
            this.numRoller.text = this.score.ToString();

            this.text = this.text!=null ? this.text : this.textNode.FindChildDepth("Text");
            switch (textType)
            {
                case "Outburst":
                    this.text.GetComponent<Text>().text = EffectConfig.FlyText[1];
                    break;
                case "ScratchCard":
                    this.text.GetComponent<Text>().text = EffectConfig.FlyText[2];
                    break;
                case "PoolTreasure0":
                    this.text.GetComponent<Text>().text = EffectConfig.FlyText[3];
                    break;
                case "PoolTreasure1":
                    this.text.GetComponent<Text>().text = EffectConfig.FlyText[4];
                    break;
                case "PoolTreasure2":
                    this.text.GetComponent<Text>().text = EffectConfig.FlyText[5];
                    break;
            }

            var position = this.textNode.position;
            float baseX = position.x;
            float baseY = position.y;
            Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
            float aimX = aimPos.x;
            float aimY = aimPos.y;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(textNode?.DOScale(new Vector3(1.2f, 1.2f, 1), 0.1f).SetEase(Ease.Linear));
            sequence.Append(textNode?.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.Linear));
            sequence.Append(LTBY_Extend.DelayRun(interval - 0.65f).SetEase(Ease.Linear));
            sequence.Append(DOTween.To(value =>
            {
                if (this.textNode == null) return;
                float t = value * 0.01f;
                this.textNode.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);

                float _scale = 1 - 0.8f * t;
                this.textNode.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.45f).SetEase(Ease.Linear));
            sequence.OnComplete(() =>
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                LTBY_EffectManager.Instance.DestroyEffect<FlyText>(this.chairId, this.id);
            });
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.textNode == null) return;
            this.textNode.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.textNode == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_TextNode", this.textNode);
            this.textNode = null;
        }
    }

    public class SummonAppear : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            LTBY_Audio.Instance.Play(SoundConfig.Summon);
            this.name = nameof(SummonAppear);
            this.id = _id;
            this.chairId = _chairId;
            Vector3 pos = args.Length > 0 ? (Vector3) args[0] : Vector3.zero;
            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetGameItem("LTBY_SummonAppear", LTBY_EffectManager.Instance.FishLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.position = pos;

            this.timerKey = new List<int>();
            int key = LTBY_Extend.Instance.DelayRun(2,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<SummonAppear>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveGameItem("LTBY_SummonAppear", this.effect);
            this.effect = null;
        }
    }

    public class Aced : EffectClass
    {
        List<EffectParamData> fishList;
        new EffectClass effect;
        int multiple;
        int useMul;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(Aced);
            this.id = _id;
            this.chairId = _chairId;
            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();
            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.AcedLinkText);
            }

            this.fishList = data.fishList;
            this.multiple = data.multiple;

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            float interval = 0.15f;

            float delay = 0;

            for (int i = 0; i < fishList.Count; i++)
            {
                int index = i;
                int key = LTBY_Extend.Instance.DelayRun(delay, () =>
                {
                    if (index + 1 < fishList.Count && fishList[index + 1] != null)
                    {
                        LTBY_EffectManager.Instance.CreateEffect<AcedLink>(this.chairId, this.fishList[index].pos,
                            this.fishList[index + 1].pos);
                    }
                });
                this.timerKey.Add(key);
                delay += interval;
            }

            delay += interval;

            int lastIndex = this.fishList.Count;
            for (int i = 0; i < fishList.Count; i++)
            {
                int index = i;
                EffectParamData v = fishList[index];
                int key = LTBY_Extend.Instance.DelayRun(delay,  ()=>
                {
                    Check();
                    if (v?.callBack != null)
                    {
                        v.callBack.Invoke();
                        v.callBack = null;
                    }

                    EffectParamData effectConfigData = new EffectParamData()
                    {
                        level = 2,
                        pos = v.pos,
                        playSound = false
                    };
                    LTBY_EffectManager.Instance.CreateEffect<ExplosionPoint>(this.chairId, effectConfigData);
                    if (v == null) return;
                    EffectParamData effectConfigData1 = new EffectParamData()
                    {
                        pos = v.pos,
                        showType = "Score",
                        showScore = v.score,
                        fishType = v.fishType,
                        aimPos = this.effect == null ? Vector3.zero : this.effect.GetNumWorldPos(),
                        useMul = this.useMul,
                        playSound = index == 0 || index == lastIndex-1,
                        callBack = () =>
                        {
                            if (this.effect != null)
                            {
                                this.effect.AddScore(v.score);
                            }
                            else
                            {
                                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, v.score, true);
                                LTBY_GameView.GameInstance.AddScore(this.chairId, v.score);
                            }
                        }
                    };
                    LTBY_EffectManager.Instance.CreateEffect<Wheel>(this.chairId, effectConfigData1);
                });
                this.timerKey.Add(key);
                delay += interval;
            }
        }

        private void Check()
        {
            this.effect = null;
            this.useMul = -1;
            string hadSpecialBattery = LTBY_BatteryManager.Instance.HadSpecialBattery(this.chairId);
            if (string.IsNullOrEmpty(hadSpecialBattery)) return;
            if (hadSpecialBattery.Equals("Drill"))
            {
                this.effect = LTBY_EffectManager.Instance.GetDrill(this.chairId);
                if (this.effect == null)
                {
                    this.effect = LTBY_EffectManager.Instance.CreateDrill(this.chairId);
                }
            }
            else if (hadSpecialBattery.Equals("Electric"))
            {
                this.effect = LTBY_EffectManager.Instance.GetElectric(this.chairId);
                if (this.effect == null)
                {
                    this.effect = LTBY_EffectManager.Instance.CreateElectric(this.chairId);
                }
            }
            else if (hadSpecialBattery.Equals("FreeBattery"))
            {
                this.effect = LTBY_EffectManager.Instance.GetFreeBattery(this.chairId);
                if (this.effect == null)
                {
                    this.effect = LTBY_EffectManager.Instance.CreateFreeBattery(this.chairId);
                }

                this.useMul = this.multiple;
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.fishList == null) return;
            for (int i = 0; i < fishList.Count; i++)
            {
                fishList[i]?.func?.Invoke();
            }

            this.fishList.Clear();
        }
    }

    public class AcedLink : EffectClass
    {
        Transform Dot1;
        Transform Dot2;
        LineRenderer line;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(AcedLink);
            this.id = _id;
            this.chairId = _chairId;

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                LTBY_Audio.Instance.Play(SoundConfig.AcedLink);
            }

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetGameItem("LTBY_AcedLink", LTBY_EffectManager.Instance.FishLayer);
            this.effect.gameObject.SetActive(true);
            Vector3 startPos = args.Length > 0 ? (Vector3) args[0] : Vector3.zero;
            Vector3 endPos = args.Length > 0 ? (Vector3) args[1] : Vector3.zero;
            float dis = Vector2.Distance(startPos, endPos) - 4;
            float rad = Mathf.PI - Mathf.Atan2((endPos.y - startPos.y), (endPos.x - startPos.x));
            Vector3 midPos = (startPos + endPos) * 0.5f;

            Vector3 pos1 = new Vector3(midPos.x + dis * Mathf.Cos(rad) * 0.5f, midPos.y - dis * Mathf.Sin(rad) * 0.5f,
                0);
            Vector3 pos2 = new Vector3(midPos.x - dis * Mathf.Cos(rad) * 0.5f, midPos.y + dis * Mathf.Sin(rad) * 0.5f,
                0);

            this.Dot1 = this.Dot1 ? this.Dot1 : this.effect.FindChildDepth("Dot1");
            this.Dot2 = this.Dot2 ? this.Dot2 : this.effect.FindChildDepth("Dot2");
            this.Dot1.position = pos1;
            this.Dot2.position = pos2;

            this.line = this.line ? this.line : this.effect.FindChildDepth<LineRenderer>("Line");
            this.line.useWorldSpace = true;

            this.line.positionCount = 2;
            this.line.SetPosition(0, pos1);
            this.line.SetPosition(1, pos2);

            this.timerKey = new List<int>();
            int key = LTBY_Extend.Instance.DelayRun(1,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<AcedLink>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveGameItem("LTBY_AcedLink", this.effect);
            this.effect = null;
        }
    }

    public class DialFish : EffectClass
    {
        public class DialFishData
        {
            public Vector3 dialAimPos;
            public int dialMul;
            public TextMeshProUGUI mul;
            public Transform mulEffect;
            public int curShowMul;
            public bool beginRoll;
            public int rollState;
            public float dialT;
            public CFunc<float, float> easeFunc;
            public float dialSpeed;
            public float aimRotation;
            public float speedSlope;
            public float pointerSpeed;
            public float pointerSpeedMul;
            public float pointerT;
            public float pointerRotation1;
            public float pointerRotation2;
            public Transform effect;
            public Transform dial;
            public Transform board;
            public Transform pointer;
            public int mulCount;
            public float split;
            public List<Transform> dotList1;
            public List<Transform> dotList2;
            public bool dotShowFlag;
            internal int dotCount;
        }

        string hadSpecialBattery;
        bool isSelf;
        List<DialFishData> dialList;

        Transform shine;
        RectTransform mask;

        Vector3 basePos;
        Vector3 pos;
        int ratio;
        private TextMeshProUGUI result;
        private int resultMul;
        private Transform resultEffect;
        private List<Transform> Dails;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(DialFish);
            this.id = _id;
            this.chairId = _chairId;
            this.timerKey = new List<int>();
            this.actionKey = new List<int>();

            this.hadSpecialBattery = LTBY_BatteryManager.Instance.HadSpecialBattery(this.chairId);

            this.isSelf = LTBY_GameView.GameInstance.IsSelf(this.chairId);

            if (this.isSelf)
            {
                LTBY_Audio.Instance.Play(SoundConfig.DialFishText);
            }

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_DialEffect", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.localScale = new Vector3(1, 1, 1);
            Vector3 _pos = LTBY_GameView.GameInstance.GetBatteryWorldPos(this.chairId);
            if (this.isSelf)
            {
                _pos.y += 5;
                if (!string.IsNullOrEmpty(this.hadSpecialBattery))
                {
                    _pos.x = 0;
                }
            }
            else if (LTBY_GameView.GameInstance.CheckIsOtherSide(this.chairId))
            {
                _pos.y += 6;
            }
            else
            {
                _pos.y += 2;
            }

            this.effect.position = _pos;
            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();
            InitData(data);

            EnterAction();
        }

        private void MoveAction(int i)
        {
            if (this.isSelf)
            {
                LTBY_Audio.Instance.Play(SoundConfig.DialFishGet);
            }
            if (this.dialList.Count <= i) return;
            DialFishData config = this.dialList[i];
            if (config == null) return;
            var position = config.dial.position;
            float baseX = position.x;
            float baseY = position.y;

            float aimX = config.dialAimPos.x;
            float aimY = config.dialAimPos.y;

            float offsetX = (aimX - baseX) * 0.5f;
            float offsetY = aimY > baseY ? -15 : 15;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value =>
            {
                if (config.dial == null) return;
                float t = value * 0.01f;
                float x = aimX + offsetX * (1 - t);
                float y = aimY + offsetY * (1 - t);
                config.dial.position = new Vector3(baseX + (x - baseX) * t, baseY + (y - baseY) * t, 0);

                if (this.isSelf) return;
                float _scale = 1 - t;
                config.dial.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (i == 2)
                {
                    MoveFinish();
                }
            }));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        private void MoveFinish()
        {
            if (this.isSelf)
            {
                this.shine.gameObject.SetActive(true);
                LTBY_Audio.Instance.Play(SoundConfig.DialFishFrame);
            }
            else
            {
                this.shine.gameObject.SetActive(false);
                this.effect.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(value =>
            {
                if (this.mask == null) return;
                float t = value / 100;
                float x = this.mask.sizeDelta.x;
                this.mask.sizeDelta = new Vector2(460 * t, 58);
            }, 0, 100, 0.5f).SetEase(Ease.Linear));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            for (int i = 0; i < 3; i++)
            {
                int index = i;
                int _key = LTBY_Extend.Instance.DelayRun(index * 0.5f, () =>
                {
                    if (this.isSelf)
                    {
                        LTBY_Audio.Instance.Play(SoundConfig.DialFishRoll);
                    }
                    if (this.dialList.Count > index) this.dialList[index].mul.gameObject.SetActive(true);
                    int kkey = LTBY_Extend.Instance.StartTimer(() => UpdateDial(index));
                    this.timerKey.Add(kkey);

                    if (index == 2)
                    {
                        int kkey1 = LTBY_Extend.Instance.StartTimer(() => UpdateMul(index), 0.05f);
                        this.timerKey.Add(kkey1);
                    }
                    else
                    {
                        int kkey1 = LTBY_Extend.Instance.StartTimer(() => UpdateMul(index));
                        this.timerKey.Add(kkey1);
                    }

                    int _key1 = LTBY_Extend.Instance.StartTimer(() => UpdateDot(index), 0.3f);
                    this.timerKey.Add(_key1);
                });
                this.timerKey.Add(_key);
            }
        }

        private void EnterAction()
        {
            this.basePos = this.pos;
            for (int i = 0; i < 3; i++)
            {
                int index = i;
                this.dialList[i].dialAimPos = this.isSelf
                    ? this.dialList[i].dial.position
                    : LTBY_GameView.GameInstance.GetBatteryWorldPos(this.chairId);
                this.dialList[i].dial.position = this.basePos;
                int key = LTBY_Extend.Instance.DelayRun(i * 0.3f + 0.5f, ()=> { MoveAction(index); });
                this.timerKey.Add(key);
            }
        }

        private int GetDialMulIndex(int i, int mul)
        {
            int[] keys = EffectConfig.DialConfig.GetDictionaryKeys();
            for (int j = 0; j < EffectConfig.DialConfig[keys[i]].Count; j++)
            {
                if (EffectConfig.DialConfig[keys[i]][j] == mul)
                {
                    return j;
                }
            }

            return 0;
        }

        private void InitData(EffectParamData data)
        {
            this.pos = data.pos;

            this.dialList = new List<DialFishData>();

            this.mask = this.mask!=null ? this.mask : this.effect.FindChildDepth<RectTransform>("Mask");
            this.mask.sizeDelta = new Vector2(0, 0);

            this.shine = this.mask.FindChildDepth("Shine");
            this.shine.gameObject.SetActive(false);

            if (this.hadSpecialBattery=="FreeBattery")
            {
                this.mask.FindChildDepth("ExtraMul").gameObject.SetActive(true);
                this.mask.FindChildDepth<Text>("ExtraMul").text = $"x{data.multiple}";
            }
            else
            {
                this.mask.FindChildDepth("ExtraMul").gameObject.SetActive(false);
            }

            this.ratio = data.ratio;

            this.result = this.mask.FindChildDepth<TextMeshProUGUI>("MulFrame/Result/Num");
            this.result.gameObject.SetActive(false);
            this.resultMul = 1;
            this.resultEffect = this.mask.FindChildDepth("MulFrame/Result/NumEffect");
            this.resultEffect.gameObject.SetActive(false);

            this.score = data.score;

            int[] dialkeys = EffectConfig.DialConfig.GetDictionaryKeys();
            for (int i = 0; i < 3; i++)
            {
                this.dialList.Add(new DialFishData());

                this.dialList[i].dialMul = GetDialMulIndex(i, data.mulList[i]);

                this.dialList[i].mul = this.mask.FindChildDepth<TextMeshProUGUI>($"MulFrame/Mul{i + 1}/Num");
                this.dialList[i].mul.gameObject.SetActive(false);
                this.dialList[i].mulEffect = this.mask.FindChildDepth($"MulFrame/Mul{i + 1}/NumEffect");
                this.dialList[i].mulEffect.gameObject.SetActive(false);
                this.dialList[i].curShowMul = 1;

                this.dialList[i].beginRoll = true;
                this.dialList[i].rollState = 1;
                this.dialList[i].dialT = 0;
                this.dialList[i].easeFunc = QuadEaseIn;

                if (i == 2)
                {
                    this.dialList[i].dialSpeed = 0.03f;
                    this.dialList[i].aimRotation = -720;
                }
                else
                {
                    this.dialList[i].dialSpeed = 0.04f;
                    this.dialList[i].aimRotation = -360;
                }

                this.dialList[i].speedSlope = -1;

                this.dialList[i].pointerSpeed = 5;
                this.dialList[i].pointerSpeedMul = 0;

                this.dialList[i].pointerT = 0;

                this.dialList[i].pointerRotation1 = 0;
                this.dialList[i].pointerRotation2 = 15;

                this.Dails = Dails != null ? Dails : new List<Transform>();
                if (this.Dails.Count <= i)
                {
                    Dails.Add(this.effect.FindChildDepth($"Dial{i + 1}"));
                }

                Transform dial = this.Dails[i];
                dial.localScale = new Vector3(1, 1, 1);
                dial.localPosition = new Vector3(-300 + (i + 1) * 150, 0, 0);
                Transform board = dial.FindChildDepth("Board");
                board.localEulerAngles = new Vector3(0, 0, 0);
                Transform pointer = dial.FindChildDepth("Pointer");

                this.dialList[i].effect = dial.FindChildDepth("Effect");

                this.dialList[i].dial = dial;
                this.dialList[i].board = board;
                this.dialList[i].pointer = pointer;

                int childCount = board.childCount;
                this.dialList[i].mulCount = childCount;
                for (int j = 0; j < childCount; j++)
                {
                    Text mul = board.FindChildDepth<Text>($"{j + 1}");
                    mul.text = $"x{EffectConfig.DialConfig[dialkeys[i]][j]}";
                    float rad = -2 * Mathf.PI / childCount * j;
                    float x = 38 * Mathf.Cos(rad + Mathf.PI * 0.5f);
                    float y = 38 * Mathf.Sin(rad + Mathf.PI * 0.5f);
                    var transform = mul.transform;
                    transform.localPosition = new Vector3(x, y, 0);
                    transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * rad);
                }

                this.dialList[i].split = 360f / childCount;

                this.dialList[i].dotList1 = new List<Transform>();
                this.dialList[i].dotList2 = new List<Transform>();
                this.dialList[i].dotShowFlag = true;

                int _childCount = dial.FindChildDepth("Dot1").childCount;
                this.dialList[i].dotCount = _childCount;

                for (int j = 0; j < _childCount; j++)
                {
                    Transform dot1 = dial.FindChildDepth($"Dot1/{j + 1}");
                    Transform dot2 = dial.FindChildDepth($"Dot2/{j + 1}");
                    float rad = 2 * Mathf.PI / _childCount * j;
                    float x = 63 * Mathf.Cos(rad);
                    float y = 63 * Mathf.Sin(rad);
                    dot1.localPosition = new Vector3(x, y, 0);
                    dot1.gameObject.SetActive(j % 2 == 0);
                    dot2.localPosition = new Vector3(x, y, 0);
                    dot2.gameObject.SetActive(j % 2 != 0);
                    this.dialList[i].dotList1.Add(dot1);
                    this.dialList[i].dotList2.Add(dot2);
                }
            }
        }

        private void OnFinish(int i)
        {
            Transform _effect = this.dialList[i].effect;

            TextMeshProUGUI mul = this.dialList[i].mul;
            int[] keys = EffectConfig.DialConfig.GetDictionaryKeys();
            int mulNum = EffectConfig.DialConfig[keys[i]][this.dialList[i].dialMul];
            mul.SetText(mulNum, true);
            this.dialList[i].mulEffect.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(mul?.transform?.DOScale(new Vector3(1f * 1.8f, 1f * 1.8f, 0), 0.1f));
            sequence.Append(mul?.transform?.DOScale(new Vector3(1f, 1f, 0), 0.1f));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            this.resultMul *= mulNum;

            if (this.isSelf)
            {
                LTBY_Audio.Instance.Play(SoundConfig.AllAudio[$"DialFishMul{i + 1}"]);
            }

            if (i != 2) return;

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(LTBY_Extend.DelayRun(0.8f).OnComplete(() =>
            {
                if (this.result == null) return;
                this.result.SetText(this.resultMul, true);
                this.result.gameObject.SetActive(true);
                this.resultEffect.gameObject.SetActive(true);
                if (this.isSelf)
                {
                    LTBY_Audio.Instance.Play(SoundConfig.DialFishMul4);
                }

                Sequence _sq = DOTween.Sequence();
                _sq.Append(result?.transform?.DOScale(new Vector3(1f * 1.8f, 1f * 1.8f, 1), 0.15f));
                _sq.Append(result?.transform?.DOScale(new Vector3(1f, 1f, 1), 0.15f));
                int kkey = LTBY_Extend.Instance.RunAction(_sq);
                this.actionKey.Add(kkey);
            }));
            _sequence.Append(LTBY_Extend.DelayRun(1).OnComplete(() =>
            {
                for (int j = 0; j < timerKey.Count; j++)
                {
                    LTBY_Extend.Instance.StopTimer(timerKey[j]);
                }

                this.timerKey.Clear();
                if (this.resultMul >= 500)
                {
                    LTBYEntry.Instance.ShakeFishLayer(1);
                }

                LTBY_EffectManager.Instance.CreateEffect<SpineDialFish>(this.chairId, this.score);
            }));
            CanvasGroup group = this.effect.GetComponent<CanvasGroup>();
            if (group == null) group = this.effect.gameObject.AddComponent<CanvasGroup>();
            _sequence.Append(group.DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                LTBY_EffectManager.Instance.DestroyEffect<DialFish>(this.chairId, this.id);
                group.alpha = 1;
            }));
            int _key = LTBY_Extend.Instance.RunAction(_sequence);
            this.actionKey.Add(_key);
        }

        private void UpdateDot(int i)
        {
            if (this.dialList.Count <= i) return;
            DialFishData config = this.dialList[i];
            if (config == null) return;
            if (config.dotShowFlag)
            {
                for (int j = 0; j < config.dotCount; j++)
                {
                    config.dotList1[j].gameObject.SetActive(j % 2 != 0);
                    config.dotList2[j].gameObject.SetActive(j % 2 == 0);
                }
            }
            else
            {
                for (int j = 0; j < config.dotCount; j++)
                {
                    config.dotList1[j].gameObject.SetActive(j % 2 == 0);
                    config.dotList2[j].gameObject.SetActive(j % 2 != 0);
                }
            }

            config.dotShowFlag = !config.dotShowFlag;
        }

        private void UpdateMul(int i)
        {
            if (this.dialList.Count <= i) return;
            DialFishData config = this.dialList[i];
            if (config == null) return;
            if (!config.beginRoll) return;
            config.curShowMul++;
            if (config.curShowMul > config.mulCount)
            {
                config.curShowMul = 1;
            }

            int[] keys = EffectConfig.DialConfig.GetDictionaryKeys();
            config.mul?.SetText(EffectConfig.DialConfig[keys[i]][config.curShowMul - 1], true);
        }

        private void UpdateDial(int i)
        {
            if (i >= this.dialList.Count) return;
            DialFishData config = this.dialList[i];
            if (config == null) return;
            if (!config.beginRoll) return;
            config.dialT += config.dialSpeed;
            if (config.dialT > 1)
            {
                config.dialT = 0;
                switch (config.rollState)
                {
                    case 1:
                        config.rollState = 2;
                        config.easeFunc = Linear;
                        config.speedSlope = -1;
                        break;
                    case 2:
                        config.rollState = 3;
                        config.easeFunc = QuadEaseOut;
                        config.aimRotation += config.dialMul * config.split;
                        config.speedSlope = -1;
                        break;
                    case 3:
                        config.beginRoll = false;
                        config.pointer.localEulerAngles = new Vector3(0, 0, 0);
                        config.curShowMul = config.dialMul + 1;
                        OnFinish(i);
                        break;
                }
            }
            else
            {
                var t = config.easeFunc(config.dialT);
                config.board.localEulerAngles = new Vector3(0, 0, Lerp(0, config.aimRotation, t));

                if (config.speedSlope < 0)
                {
                    config.speedSlope = t;
                }
                else
                {
                    config.pointerSpeedMul = t - config.speedSlope;
                    config.speedSlope = t;
                }

                config.pointerT += config.pointerSpeed * config.pointerSpeedMul;
                config.pointer.localEulerAngles = new Vector3(0, 0,
                    Lerp(config.pointerRotation1, config.pointerRotation2, config.pointerT));
                if (config.pointerT > 1)
                {
                    config.pointerT = 0;
                }
            }
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private float QuadEaseOut(float time)
        {
            return -1 * time * (time - 2);
        }

        private float Linear(float time)
        {
            return time * 2;
        }

        private float QuadEaseIn(float time)
        {
            return time * time;
        }

        public override void OnStored()
        {
            base.OnStored();
            this.dialList?.Clear();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_DialEffect", this.effect);
            this.effect = null;
        }
    }

    public class SpineDialFish : EffectClass
    {
        bool isSelf;
        Transform EffectSelf;
        Transform EffectOther;
        Transform JinBiPart;
        float curScale;
        Transform SelfGQ;
        Transform OtherGQ;
        private Transform image;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(SpineDialFish);
            this.id = _id;
            this.chairId = _chairId;
            this.isSelf = LTBY_GameView.GameInstance.IsSelf(this.chairId);

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            this.score = args.Length > 0 ? (long) args[0] : 0;

            this.account = this.account!=null
                ? this.account
                : LTBY_PoolManager.Instance.GetUiItem($"LTBY_SpineDialFish", LTBY_EffectManager.Instance.UiLayer);
            this.account.gameObject.SetActive(true);

            this.EffectSelf = this.EffectSelf!=null ? this.EffectSelf : this.account.FindChildDepth("EffectSelf");
            this.EffectOther = this.EffectOther!=null ? this.EffectOther : this.account.FindChildDepth("EffectOther");
            this.JinBiPart = JinBiPart != null ? JinBiPart : this.EffectSelf.FindChildDepth($"JinBiPart");

            if (this.isSelf)
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward2);

                this.account.position = new Vector3(0, 0, 0);
                this.EffectSelf.gameObject.SetActive(false);
                this.EffectSelf.gameObject.SetActive(this.isSelf);
                this.EffectOther.gameObject.SetActive(false);

                this.curScale = 1;
            }
            else
            {
                this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
                this.EffectOther.gameObject.SetActive(false);
                this.EffectOther.gameObject.SetActive(!this.isSelf);
                this.EffectSelf.gameObject.SetActive(false);

                this.curScale = 0.8f;
            }

            this.SelfGQ = this.SelfGQ!=null ? this.SelfGQ : this.account.FindChildDepth("EffectSelf/guaquan");
            this.OtherGQ = this.OtherGQ!=null ? this.OtherGQ : this.account.FindChildDepth("EffectOther/guaquan");

            if (this.score >= EffectConfig.UseFireworkScore)
            {
                this.SelfGQ.gameObject.SetActive(false);
                this.OtherGQ.gameObject.SetActive(false);
                EffectParamData _data = new EffectParamData()
                {
                    chairId = _chairId,
                    pos = this.isSelf ? Vector3.zero : LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                    scale = this.isSelf ? 1 : 0.5f
                };
                LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
            }
            else
            {
                this.SelfGQ.gameObject.SetActive(true);
                this.OtherGQ.gameObject.SetActive(true);
            }

            this.JinBiPart.gameObject.SetActive(!LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score));

            this.account.localScale = new Vector3(this.curScale, this.curScale, this.curScale);

            this.skeleton = this.skeleton!=null ? this.skeleton : this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            if (this.skeleton != null)
            {
                this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
            }

            this.num = this.num!=null ? this.num : this.account.FindChildDepth("Num").GetILComponent<NumberRoller>();
            this.num = this.num!=null ? this.num : this.account.FindChildDepth("Num").AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.text = "0";
            this.num.RollTo(this.score, 3);

            this.image = this.account.FindChildDepth("Image");
            this.image.localEulerAngles = new Vector3(0, 0, 0);
            this.image.localScale = Vector3.one;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOBlendableScaleBy(new Vector3(3, 3), 0.01f));
            sequence.Append(image.DOBlendableScaleBy(new Vector3(-3, -3), 0.01f));
            Sequence sequence1 = DOTween.Sequence();
            sequence.Append(sequence1);
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, -10), 0.4f));
            sequence1.Append(image.DOBlendableRotateBy(new Vector3(0, 0, 10), 0.2f));
            sequence1.SetLoops(10);
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            Tween tween = DOTween.To(value =>
            {
                if (this.account == null) return;
                float t = value * 0.01f;
                float _scale = (1 - t) * this.curScale;
                this.account.localScale = new Vector3(_scale, _scale, _scale);
            }, 0, 100, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                LTBY_EffectManager.Instance.DestroyEffect<SpineDialFish>(this.chairId, this.id);
            }).SetDelay(4);
            int key1 = LTBY_Extend.Instance.RunAction(tween);
            this.actionKey.Add(key1);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.account == null) return;
            this.account.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_SpineDialFish", this.account);
            this.account = null;
        }
    }

    public class Firework : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            LTBY_Audio.Instance.Play(SoundConfig.SpineAwardDialFish);
            this.name = nameof(Firework);
            this.id = _id;
            this.chairId = _chairId;

            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_FireworkEffect", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.position = data.pos;
            float _scale = data.scale != 0 ? data.scale : 1;
            this.effect.localScale = new Vector3(_scale, _scale, _scale);

            this.timerKey = new List<int>();
            int key = LTBY_Extend.Instance.DelayRun(data.delay == 0 ? 5 : data.delay,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<Firework>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_FireworkEffect", this.effect);
            this.effect = null;
        }
    }

    public class PandaDance : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            LTBY_Audio.Instance.Play(SoundConfig.SpineAwardDialFish);
            this.name = nameof(PandaDance);
            this.id = _id;
            this.chairId = _chairId;

            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_PandaDanceEffect", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.position = data.pos;
            float _scale = data.scale != 0 ? data.scale : 1;
            this.effect.localScale = new Vector3(_scale, _scale, _scale);

            this.timerKey = new List<int>();

            int key = LTBY_Extend.Instance.DelayRun(data.delay == 0 ? 3.2f : data.delay,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<PandaDance>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_PandaDanceEffect", this.effect);
            this.effect = null;
        }
    }

    public class DragonBallEffect : EffectClass
    {
        int dragonOwnerId;
        bool isSelf;
        bool isMyDragon;
        private Dictionary<int, List<int>> MulConfig;
        private Transform ball_1;
        private Transform ball_2;
        private Text multiplier_1;
        private Text multiplier_2;
        private ParticleSystem zip_1;
        private ParticleSystem zip_2;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, chairId, args);
            this.name = nameof(DragonBallEffect);
            this.id = _id;
            this.chairId = _chairId;
            this.dragonOwnerId = _chairId;
            this.isSelf = LTBY_GameView.GameInstance.IsSelf(chairId);
            this.isMyDragon = LTBY_GameView.GameInstance.chairId == _chairId;
            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            Vector3 pos = args.Length >= 1 ? (Vector3) args[0] : Vector3.zero;
            int num1 = args.Length >= 2 ? (int) args[1] : 0;
            int num2 = args.Length >= 3 ? (int) args[2] : 0;

            this.MulConfig = EffectConfig.ElectriBallMul;
            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_DragonBalls", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);

            this.ball_1 = this.ball_1!=null ? this.ball_1 : this.effect.FindChildDepth("ElectricBall1");
            this.ball_2 = this.ball_2!=null ? this.ball_2 : this.effect.FindChildDepth($"ElectricBall2");
            this.ball_1.gameObject.SetActive(true);
            this.ball_2.gameObject.SetActive(true);
            this.ball_1.localPosition = new Vector3(0, 0, 0);
            this.ball_2.localPosition = new Vector3(0, 0, 0);

            this.multiplier_1 = multiplier_1 != null ? multiplier_1 : this.ball_1.FindChildDepth<Text>("multiplier");
            this.multiplier_2 = this.multiplier_2!=null ? this.multiplier_2 : this.ball_2.FindChildDepth<Text>("multiplier");
            this.multiplier_1.gameObject.SetActive(false);
            this.multiplier_2.gameObject.SetActive(false);

            this.zip_1 = this.zip_1!=null ? this.zip_1 : this.ball_1.FindChildDepth<ParticleSystem>("ZipEffect");
            this.zip_2 = this.zip_2!=null ? this.zip_2 : this.ball_2.FindChildDepth<ParticleSystem>("ZipEffect");

            float timer = 0;
            float phaseOneTime = 1;
            this.ball_1.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            this.ball_2.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            if (this.isMyDragon)
            {
                this.effect.localPosition = new Vector3(0, 0, 1000);
                LTBY_Audio.Instance.Play(SoundConfig.ElectricDragonDie);
                timer += phaseOneTime;
                List<float> scaleArray = new List<float>() {40, 20, 60, 40, 60};
                LTBY_Audio.Instance.Play(SoundConfig.DragonBallFly);
                Sequence sequence = DOTween.Sequence();
                sequence.Append(ball_1.DOLocalMove(new Vector3(-160, 0, 0), phaseOneTime));
                Sequence sequence1 = DOTween.Sequence();
                sequence.Insert(0, sequence1);
                sequence1.Append(DOTween.To(value =>
                {
                    if (this.ball_1 == null || this.ball_2 == null) return;
                    float t = value * 0.01f;
                    this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[0], scaleArray[1], 0.2f));
                sequence1.Append(DOTween.To(value =>
                {
                    if (this.ball_1 == null || this.ball_2 == null) return;
                    float t = value * 0.01f;
                    this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[1], scaleArray[2], 0.3f));
                sequence1.Append(DOTween.To(value =>
                {
                    if (this.ball_1 == null || this.ball_2 == null) return;
                    float t = value * 0.01f;
                    this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[2], scaleArray[3], 0.3f));
                sequence1.Append(DOTween.To(value =>
                {
                    if (this.ball_1 == null || this.ball_2 == null) return;
                    float t = value * 0.01f;
                    this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[3], scaleArray[4], 0.3f));
                int action_1 = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(action_1);

                Tween tween = ball_2.DOLocalMove(new Vector3(160, 0, 0), phaseOneTime);
                int action_2 = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(action_2);
                timer += 0.3f;
                int delayKey = LTBY_Extend.Instance.DelayRun(timer, ()=> { RollMultipliers(num1, num2); });
                this.timerKey.Add(delayKey);
            }
            else
            {
                this.effect.localPosition = new Vector3(0, 0, 1000);
                int otherId = this.dragonOwnerId;
                Vector3 effectPos = LTBY_GameView.GameInstance.GetBatteryWorldPos(otherId);
                Tween tween = ball_1.DOLocalMove(new Vector3(-60, 0, 0), phaseOneTime);
                int moveBallOne = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(moveBallOne);
                Tween tween1 = ball_2.DOLocalMove(new Vector3(60, 0, 0), phaseOneTime);
                int moveBallTwo = LTBY_Extend.Instance.RunAction(tween1);
                this.actionKey.Add(moveBallTwo);
                float x = effectPos.x;
                float y = LTBY_GameView.GameInstance.CheckIsOtherSide(otherId)
                    ? effectPos.y - 4.5f
                    : effectPos.y + 4.5f;
                float z = this.effect.position.z;
                Tween tween2 = DOTween.To(value =>
                {
                    if (this.effect == null) return;
                    float t = value * 0.01f;
                    this.effect.position = new Vector3(x * t, y * t, z);
                }, 0, 100, phaseOneTime);
                int moveAll = LTBY_Extend.Instance.RunAction(tween2);
                this.actionKey.Add(moveAll);
                timer += phaseOneTime + 0.3f;
                int delayKey = LTBY_Extend.Instance.DelayRun(timer, ()=> { RollMultipliers(num1, num2); });
                this.timerKey.Add(delayKey);
            }
        }

        private void RollMultipliers(int num1 = 1, int num2 = 1)
        {
            // int mul_start_1 = 1;
            // int mul_start_2 = 1;
            int mul_end_1 = num1;
            int mul_end_2 = num2;
            Text mulText_1 = this.multiplier_1;
            Text mulText_2 = this.multiplier_2;
            mulText_1.text = $"X {mul_end_1}";
            mulText_2.text = $"X {mul_end_2}";

            // int index = 1;
            List<int> mulConfig_2 = this.MulConfig[2];
            // Transform ball_1 = this.ball_1;
            // Transform ball_2 = this.ball_2;
            float phaseTwoTime = 1.5f;
            float scaleTime = 0.3f;

            //缩放电球1文本，并播放小闪电
            Sequence sequence = DOTween.Sequence();
            sequence.Append(mulText_1?.transform?.DOScale(new Vector3(0, 0), 0));
            sequence.Append(mulText_1?.transform?.DOScale(new Vector3(1.7f, 1.7f), scaleTime));
            sequence.Append(mulText_1?.transform?.DOScale(new Vector3(1.2f, 1.2f), scaleTime).OnComplete(() => { }));
            int textScaleKey_1 = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(textScaleKey_1);

            //缩放电球1
            List<float> scaleArray = this.isMyDragon ? new List<float>() {60, 80, 60} : new List<float>() {40, 60, 40};
            //球放大缩小比例
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Append(DOTween.To(value =>
            {
                if (this.ball_1 == null) return;
                float t = value * 0.01f;
                this.ball_1.localScale = new Vector3(t, t, t);
                //this.ball_2.localScale = new Vector3(t, t, t);
            }, scaleArray[0], scaleArray[1], scaleTime).OnComplete(() =>
            {
                mulText_1?.gameObject.SetActive(true);
                if (this.isMyDragon)
                {
                    LTBY_Audio.Instance.Play(SoundConfig.RollMultiple);
                    LTBYEntry.Instance.ShakeFishLayer(0.5f, 2);
                }

                this.zip_1.Play();
            }));
            sequence1.Insert(0, DOTween.To(value =>
            {
                if (this.ball_1 == null) return;
                float t = value * 0.01f;
                this.ball_1.localScale = new Vector3(t, t, t);
                //this.ball_2.localScale = new Vector3(t, t, t);
            }, scaleArray[1], scaleArray[2], scaleTime));
            sequence1.SetEase(Ease.InOutSine);
            int scaleBallUp = LTBY_Extend.Instance.RunAction(sequence1);
            this.actionKey.Add(scaleBallUp);

            //电球2等待0.3秒后再开始动作
            int delayKey = LTBY_Extend.Instance.DelayRun(scaleTime + 0.3f, ()=>
            {
                //缩放电球2大小，并播放小闪电
                Sequence seq = DOTween.Sequence();
                seq.Append(mulText_2?.transform?.DOScale(new Vector3(0, 0), 0));
                seq.Append(mulText_2?.transform?.DOScale(new Vector3(1.7f, 1.7f), scaleTime));
                seq.Append(mulText_2?.transform?.DOScale(new Vector3(1.2f, 1.2f), scaleTime).OnComplete(() => { }));
                int textScaleKey_2 = LTBY_Extend.Instance.RunAction(seq);
                this.actionKey.Add(textScaleKey_2);
                //缩放电球2，并在缩回去后震屏
                Sequence seq1 = DOTween.Sequence();
                seq1.Append(DOTween.To(value =>
                {
                    if (this.ball_2 == null) return;
                    float t = value * 0.01f;
                    //this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[0], scaleArray[1], scaleTime).OnComplete(() =>
                {
                    mulText_2?.gameObject.SetActive(true);
                    if (this.isMyDragon)
                    {
                        LTBY_Audio.Instance.Play(SoundConfig.RollMultiple);
                        LTBYEntry.Instance.ShakeFishLayer(0.5f, 2);
                    }

                    this.zip_2.Play();
                }));
                seq1.Insert(0, DOTween.To(value =>
                {
                    if (this.ball_2 == null) return;
                    float t = value * 0.01f;
                    //this.ball_1.localScale = new Vector3(t, t, t);
                    this.ball_2.localScale = new Vector3(t, t, t);
                }, scaleArray[1], scaleArray[2], scaleTime));
                seq1.SetEase(Ease.InOutSine);
                int _scaleBallUp = LTBY_Extend.Instance.RunAction(seq1);
                this.actionKey.Add(_scaleBallUp);
            });
            this.timerKey.Add(delayKey);
            //等待所有动作完成，进入融合阶段
            int _delayKey = LTBY_Extend.Instance.DelayRun(phaseTwoTime, ()=> { Fusion(mul_end_1 * mul_end_2); });
            this.timerKey.Add(_delayKey);
        }

        private void Fusion(int product)
        {
            Text mulText_1 = this.multiplier_1;
            float fusionTime = 0.6f;
            float hoverShakingTime = 0.4f;
            // float smashBackTime = 0.8f;
            float timer = 0;
            // Transform ball_1 = this.ball_1;
            // Transform ball_2 = this.ball_2;
            if (this.isMyDragon)
            {
                LTBY_Audio.Instance.Play(SoundConfig.DragonBallFly);
            }

            //移动电球2到中心并隐藏
            Tween tween = ball_2.DOLocalMove(new Vector2(0, 0), fusionTime)
                .OnComplete(() => { ball_2?.gameObject.SetActive(false); });
            int moveBallTwoKey = LTBY_Extend.Instance.RunAction(tween);
            this.actionKey.Add(moveBallTwoKey);

            //移动电球1到中心并进行融合
            List<int> scaleArray = this.isMyDragon ? new List<int>() {60, 100, 80} : new List<int>() {40, 60, 40};
            Sequence sequence = DOTween.Sequence();
            sequence.Append(ball_1.DOLocalMove(new Vector2(0, 0), fusionTime).OnComplete(() =>
            {
                this.multiplier_1.text = $"X {product}";
                if (this.isMyDragon)
                {
                    LTBY_Audio.Instance.Play(SoundConfig.RollMultiple);
                    LTBYEntry.Instance.ShakeFishLayer(0.5f, 2);
                }
            }));
            Sequence sequence1 = DOTween.Sequence();
            sequence.Append(sequence1);
            sequence1.Append(DOTween.To(value =>
            {
                if (this.ball_1 == null) return;
                float t = value * 0.01f;
                this.ball_1.localScale = new Vector3(t, t, t);
            }, scaleArray[0], scaleArray[1], fusionTime / 2));

            sequence1.Insert(0, ball_1.DOShakePosition(fusionTime / 2, 50, 100, 100, false));
            sequence.Append(DOTween.To(value =>
            {
                if (this.ball_1 == null) return;
                float t = value * 0.01f;
                this.ball_1.localScale = new Vector3(t, t, t);
            }, scaleArray[1], scaleArray[2], fusionTime / 2));

            int moveBallOneKey = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(moveBallOneKey);


            timer += fusionTime;

            //融合之后电球1进行震动并缩放
            int delayTimerKey = LTBY_Extend.Instance.DelayRun(timer, ()=>
            {
                Sequence _seq = DOTween.Sequence();
                _seq.Append(mulText_1?.transform?.DOScale(new Vector2(1.7f, 1.7f), 0.3f));
                _seq.Append(mulText_1?.transform?.DOScale(new Vector2(1.2f, 1.2f), 0.1f).OnComplete(() =>
                {
                    if (!this.isMyDragon) return;
                    Tween tween1 = effect?.DOShakePosition(hoverShakingTime, 50, 100, 100, false);
                    int shakeActionKey = LTBY_Extend.Instance.RunAction(tween1);
                    this.actionKey.Add(shakeActionKey);
                }));
                int scaleTextKey = LTBY_Extend.Instance.RunAction(_seq);
                this.actionKey.Add(scaleTextKey);
            });
            this.timerKey.Add(delayTimerKey);


            timer += hoverShakingTime;
            int delayTimerKey1 = LTBY_Extend.Instance.DelayRun(timer, ()=>
            {
                ball_2.localPosition = new Vector3(0, 0, 0);
                if (this.isMyDragon)
                {
                    LTBYEntry.Instance.ShakeFishLayer(0.8f, 3);
                }

                this.zip_1.Play();
                LTBY_BulletManager.Instance.CreateBulletDragonBall(this.isMyDragon ? this.chairId : this.dragonOwnerId,
                    product);

                OnFinish();
            });
            this.timerKey.Add(delayTimerKey);

            //int nextPhaseTimer = LTBY_Extend.Instance.DelayRun(timer, delegate () { });
            //this.timerKey.Add(nextPhaseTimer);
        }

        private void OnFinish(long _score = 0, int multiplier = 0)
        {
            float _scale = this.isMyDragon ? 80 : 40;
            int delayKey = LTBY_Extend.Instance.DelayRun(0.6f, ()=>
            {
                Tween tween = DOTween.To(value =>
                {
                    if (this.ball_1 == null) return;
                    float t = value * 0.01f;
                    this.ball_1.localScale = new Vector3(t, t, t);
                }, _scale, 0, 0.8f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    LTBY_EffectManager.Instance.DestroyDragonBallEffect(this.chairId);
                });
                int scaleOutKey = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(scaleOutKey);
            });
            this.timerKey.Add(delayKey);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_DragonBalls", this.effect);
            this.effect = null;
        }
    }

    public class ElectricDragonSpineAward : EffectClass
    {
        private int multiple = -1;
        private Text multipleText;
        private bool isSelf;
        private Transform effect1;
        private Transform effect2;
        private Transform numNode;
        private float numScale;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(ElectricDragonSpineAward);
            this.id = _id;
            this.chairId = _chairId;
            this.score = 0;
            this.multiple = args.Length > 0 ? (int) args[0] : 0;
            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            if (LTBY_GameView.GameInstance.IsPlayerRunInBackground(_chairId))
            {
                LTBY_DataReport.Instance.ReportElectricInBackground(_chairId);
                return;
            }

            this.account = this.account != null
                ? this.account
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_ElectricDragonSpineAward",
                    LTBY_EffectManager.Instance.UiLayer);
            this.account.gameObject.SetActive(true);

            this.skeleton = this.skeleton!=null ? this.skeleton : this.account.FindChildDepth<SkeletonGraphic>("Skeleton");
            this.skeleton.AnimationState.SetAnimation(0, "stand", false);

            this.multipleText = this.multipleText!=null ? this.multipleText : this.account.FindChildDepth<Text>("Multiple");
            if (args.Length > 0)
            {
                this.multipleText.gameObject.SetActive(true);
                this.multipleText.text = $"X {this.multiple}";
            }
            else
            {
                this.multipleText.gameObject.SetActive(false);
            }

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                this.isSelf = true;
                this.account.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                this.account.position = new Vector3(0, 0, 0);
                this.effect1 = this.account.FindChildDepth("Effect1_1");
                this.effect2 = this.account.FindChildDepth("Effect1_2");
            }
            else
            {
                this.isSelf = false;
                this.account.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                this.account.position = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
                this.effect1 = this.account.FindChildDepth("Effect2_1");
                this.effect2 = this.account.FindChildDepth("Effect2_2");
            }

            this.effect1.gameObject.SetActive(false);
            this.effect2.gameObject.SetActive(false);
            this.numNode = this.numNode!=null ? this.numNode : this.account.FindChildDepth("Num");
            this.numNode.gameObject.SetActive(true);
            this.numPosition = this.numNode.position;
            this.num = this.num!=null ? this.num : this.numNode.GetILComponent<NumberRoller>();
            this.num = this.num!=null ? this.num : this.numNode.AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.RollFromTo(0, 0, 0.1f);
            this.numScale = 0.65f;
            int delay = LTBY_Extend.Instance.DelayRun(1,
                ()=> { this.skeleton?.AnimationState.SetAnimation(0, "stand1", true); });
            this.timerKey.Add(delay);
        }

        public override void AddScore(long _score, float roll = 0)
        {
            base.AddScore(_score, roll);
            //DebugHelper.LogError($"雷皇龙加分:{score}    effectManager:3424");
            this.numNode.transform.gameObject.SetActive(true);
            this.score += _score;

            this.num.RollTo(this.score, 0.7f);
            //this.num.text = this.score.ToString();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(num?.transform?.DOScale(new Vector2(this.numScale + 0.3f, this.numScale + 0.3f), 0.1f));
            sequence.Append(num?.transform?.DOScale(new Vector2(this.numScale, this.numScale), 0.1f));
            int key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);
        }

        public override void OnSettle()
        {
            base.OnSettle();
            bool _isSelf = LTBY_GameView.GameInstance.IsSelf(this.chairId);

            //只有当播放自己特效的时候 effect2的爆金币特效被一亿金币特效特换
            bool showEffect2 = (!_isSelf) || (!LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score));
            this.effect2.gameObject.SetActive(showEffect2);

            int key = LTBY_Extend.Instance.DelayRun(1.2f,  ()=>
            {
                this.effect1?.gameObject.SetActive(true);
                if (this.score < EffectConfig.UseFireworkScore) return;
                EffectParamData _data = new EffectParamData()
                {
                    chairId = chairId,
                    pos = this.isSelf ? Vector3.zero : LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                    scale = this.isSelf ? 1 : 0.5f
                };
                LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
            });
            this.timerKey.Add(key);

            if (this.isSelf)
            {
                LTBY_Audio.Instance.Play(SoundConfig.SpineAward3);
                LTBYEntry.Instance.ShakeFishLayer(4);
            }

            int key1 = LTBY_Extend.Instance.DelayRun(5.4f, ()=>
            {
                if (this.isSelf)
                {
                    Tween tween = account?.DOScale(new Vector2(0, 0), 0.6f).SetEase(Ease.InBack).SetDelay(0.2f)
                        .OnComplete(() =>
                        {
                            LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                            LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                            LTBY_EffectManager.Instance.DestroyElectricDragonSpineAward(this.chairId);
                            LTBY_DataReport.Instance.Get("雷皇龙", this.score);
                            LTBY_DataReport.Instance.ReportElectricDragonAddScore(this.score, this.multiple,
                                this.chairId);
                        });
                    int _key = LTBY_Extend.Instance.RunAction(tween);
                    this.actionKey.Add(_key);
                }
                else
                {
                    float baseX = this.account.position.x;
                    float baseY = this.account.position.y;
                    Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
                    float aimX = aimPos.x;
                    float aimY = aimPos.y;

                    Tween tween = DOTween.To(value =>
                    {
                        if (this.account == null) return;
                        float t = value * 0.01f;
                        this.account.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                        float _scale = (1 - t) * 0.7f;
                        this.account.localScale = new Vector3(_scale, _scale, _scale);
                    }, 0, 100, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                        LTBY_EffectManager.Instance.DestroyElectricDragonSpineAward(this.chairId);
                    });
                    int _key = LTBY_Extend.Instance.RunAction(tween);
                    this.actionKey.Add(key);
                }
            });
            this.timerKey.Add(key1);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.skeleton != null)
            {
                this.skeleton.AnimationState.ClearTracks();
            }

            if (this.account != null)
            {
                this.account.gameObject.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                //LTBY_DataReport.Instance.ReportDestroyElectricDragon(debug.traceback())
            }

            base.OnDestroy();
            if (this.account == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_ElectricDragonSpineAward", this.account);
            this.account = null;
        }
    }

    public class DoubleDragonEffect : EffectClass
    {
        private float playTime;
        private Animator animator_1;
        private Animator animator_2;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(DoubleDragonEffect);
            this.id = _id;
            this.chairId = _chairId;
            this.timerKey = new List<int>();
            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetGameItem("LTBY_DoubleDragonEffect",
                    LTBY_EffectManager.Instance.FishLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.localPosition = new Vector3(0, 0, 0);
            this.playTime = 10;
            this.animator_1 = this.animator_1!=null ? this.animator_1 : this.effect.FindChildDepth<Animator>("model");
            this.animator_2 = this.animator_2!=null ? this.animator_2 : this.effect.FindChildDepth<Animator>("model2");

            this.animator_1.SetTrigger($"move");
            this.animator_2.SetTrigger($"move");

            int key = LTBY_Extend.Instance.DelayRun(this.playTime,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<DoubleDragonEffect>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveGameItem("LTBY_DoubleDragonEffect", this.effect);
            this.effect = null;
        }
    }

    public class HundredMillionMoney : EffectClass
    {
        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(HundredMillionMoney);
            this.id = _id;
            this.chairId = _chairId;

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_yifenbaojiang", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.effect.position = new Vector3(0, 0, 0);
            float _scale = 1;
            this.effect.localScale = new Vector3(_scale, _scale, _scale);
            bool isTop = args.Length > 0 && (bool) args[0];
            //设置层级
            string sortLayer = isTop ? "sort10" : "Default";
            int sortOrder = isTop ? 2 : 0;
            ParticleSystemRenderer[] particleComps = this.effect.GetComponentsInChildren<ParticleSystemRenderer>();
            if (particleComps != null)
            {
                for (int i = 0; i < particleComps.Length; i++)
                {
                    particleComps[i].sortingLayerName = sortLayer;
                    particleComps[i].sortingOrder = sortOrder;
                }
            }

            this.timerKey = new List<int>();

            LTBY_Audio.Instance.Play(SoundConfig.HundredMillion);

            int key = LTBY_Extend.Instance.DelayRun(6,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<HundredMillionMoney>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_yifenbaojiang", this.effect);
            this.effect = null;
        }
    }

    public class TreasureBowlEffect2 : EffectClass
    {
        SCTreasureFishCatched data;
        private string effectName;
        private Queue<float> totalNums;
        private bool isSelf;
        private Transform effect1;
        private Transform effect2;
        private Transform numNode;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(TreasureBowlEffect2);
            this.id = _id;
            this.chairId = _chairId;
            this.data = args.Length > 0 ? (SCTreasureFishCatched) args[0] : new SCTreasureFishCatched();
            this.score = data.earn;
            int status = (int) args[1];
            this.effectName = $"LTBY_JBPSpineAward{status}";

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            this.totalNums = new Queue<float>();
            float value1 = data.fish_value * data.ratio * data.multiple;
            this.totalNums.Enqueue(value1);
            //float value2 = data.accum_money * data.multiple;
            float value2 = data.accum_money;
            if (value2 > 0)
            {
                this.totalNums.Enqueue(value2 + value1);
            }

            this.effect = this.effect != null ? this.effect : LTBY_PoolManager.Instance.GetUiItem(this.effectName, LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);
            this.skeleton = this.skeleton != null ? this.skeleton : this.effect.FindChildDepth<SkeletonGraphic>("Skeleton");
            this.skeleton.AnimationState.SetAnimation(0, "stand01", false);
            int key = LTBY_Extend.Instance.DelayRun(1.4f,
                ()=> { this.skeleton?.AnimationState.SetAnimation(1, "stand02", true); });
            this.timerKey.Add(key);

            if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
            {
                this.isSelf = true;
                //这里由于三个阶段的spine大小不一样 这里处理一下
                if (status != 3)
                {
                    this.effect.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                }
                else
                {
                    this.effect.localScale = new Vector3(1, 1, 1);
                }

                this.effect.position = new Vector3(0, 0, -40);
                this.effect1 = this.effect.FindChildDepth("Effect1_1");
                this.effect2 = this.effect.FindChildDepth("Effect1_2");
                LTBY_Audio.Instance.Play(SoundConfig.JBPSettleVoice);
            }
            else
            {
                this.isSelf = false;
                this.effect.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                Vector3 pos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
                if (!LTBY_GameView.GameInstance.CheckIsOtherSide(this.chairId))
                {
                    pos = new Vector3(pos.x, pos.y + 1, -40);
                }
                else
                {
                    pos = new Vector3(pos.x, pos.y - 1, -40);
                }

                this.effect.position = pos;
                this.effect1 = this.effect.FindChildDepth("Effect2_1");
                this.effect2 = this.effect.FindChildDepth("Effect2_2");
            }


            this.numNode = this.effect.FindChildDepth("Num");
            this.numNode.gameObject.SetActive(true);
            this.num = this.num!=null ? this.num : this.numNode.GetILComponent<NumberRoller>();
            this.num = this.num!=null ? this.num : this.numNode.AddILComponent<NumberRoller>();
            this.num.Init();
            this.num.text = "0";

            RollNum();
        }

        private void RollNum()
        {
            float delay = 0;
            float numScale = 0.65f;
            int count = this.totalNums.Count;
            for (int i = 0; i < count; i++)
            {
                int index = i;
                int curShowNum = (int) this.totalNums.Dequeue();
                Sequence sequence = DOTween.Sequence();
                sequence.Append(LTBY_Extend.DelayRun(delay + 1).OnComplete(() =>
                {
                    this.numNode?.gameObject.SetActive(true);
                    this.num?.RollTo(curShowNum, 1);
                }));

                sequence.Append(LTBY_Extend.DelayRun(0.9f).OnComplete(() =>
                {
                    if (LTBY_GameView.GameInstance.IsSelf(this.chairId))
                    {
                        LTBY_Audio.Instance.Play(SoundConfig.DragonEffect2);
                    }

                    //最后一次加分的时候 播放烟花
                    if (index == count - 1)
                    {
                        //当达到一亿分的时候 隐藏这个爆金币的特效，用一亿分的特效代替
                        if (!(this.isSelf && LTBY_EffectManager.Instance.CheckCanUseHundredMillion(this.score)))
                        {
                            this.effect2?.gameObject.SetActive(false);
                            this.effect2?.gameObject.SetActive(true);
                        }


                        if (this.score < EffectConfig.UseFireworkScore) return;
                        EffectParamData _data = new EffectParamData()
                        {
                            chairId = chairId,
                            pos = this.isSelf ? Vector3.zero : LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId),
                            scale = this.isSelf ? 0.7f : 0.4f
                        };
                        LTBY_EffectManager.Instance.CreateBoostScoreBoardEffects(this.score, _data);
                    }
                    else
                    {
                        this.effect2?.gameObject.SetActive(false);
                        this.effect2?.gameObject.SetActive(true);
                    }
                }));
                sequence.Append(LTBY_Extend.DelayRun(0.2f).OnComplete(() =>
                {
                    this.effect1?.gameObject.SetActive(false);
                    this.effect1?.gameObject.SetActive(true);
                }));
                sequence.Append(numNode?.DOScale(new Vector2(numScale * 1.5f, numScale * 1.5f), 0.04f));
                sequence.Append(numNode?.DOScale(new Vector2(numScale, numScale), 0.7f).SetEase(Ease.OutSine));
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
                delay += 2;
            }

            if (this.data.earn >= EffectConfig.UseFireworkScore)
            {
                delay += 2;
            }

            if (this.isSelf)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(LTBY_Extend.DelayRun(delay + 2).OnComplete(() =>
                {
                    this.effect1?.gameObject.SetActive(false);
                    this.effect2?.gameObject.SetActive(false);
                }));
                sequence.Append(effect?.DOScale(new Vector2(0, 0), 0.4f).SetEase(Ease.InBack).SetDelay(0.2f));
                sequence.OnComplete(() =>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_GameView.GameInstance.AddScore(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<TreasureBowlEffect2>(this.chairId, this.id);
                });
                int key = LTBY_Extend.Instance.RunAction(sequence);
                this.actionKey.Add(key);
            }
            else
            {
                var position = this.effect.position;
                float baseX = position.x;
                float baseY = position.y;
                Vector3 aimPos = LTBY_GameView.GameInstance.GetCoinWorldPos(this.chairId);
                float aimX = aimPos.x;
                float aimY = aimPos.y;

                Tween tween = DOTween.To(value =>
                {
                    if (this.effect == null) return;
                    float t = value * 0.01f;
                    this.effect.position = new Vector3(baseX + (aimX - baseX) * t, baseY + (aimY - baseY) * t, 0);
                    float _scale = (1 - t) * 0.7f;
                    this.effect.localScale = new Vector3(_scale, _scale, _scale);
                }, 0, 100, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    LTBY_GameView.GameInstance.CreateAddUserScoreItem(this.chairId, this.score);
                    LTBY_EffectManager.Instance.DestroyEffect<TreasureBowlEffect2>(this.chairId, this.id);
                }).SetDelay(delay + 2);
                int key = LTBY_Extend.Instance.RunAction(tween);
                this.actionKey.Add(key);
            }
        }

        public override void OnStored()
        {
            base.OnStored();
            this.skeleton?.AnimationState.ClearTracks();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.effectName, this.effect);
            this.effect = null;
        }
    }

    public class TreasureBowlEffect1 : EffectClass
    {
        SCTreasureFishCatched data;
        private bool isSelf;
        private Transform topEffect;
        private Transform TopRoot;
        private Transform BottomRoot;
        private Text bottomText;
        private Text TopMul;
        private Text BottomMul;
        private float targetScale;

        private Transform BottomEffect { get; set; }

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(TreasureBowlEffect1);
            this.id = _id;
            this.chairId = _chairId;
            this.data = args.Length > 0 ? (SCTreasureFishCatched) args[0] : new SCTreasureFishCatched();
            Vector3 pos = (Vector3) args[1];
            this.isSelf = LTBY_GameView.GameInstance.IsSelf(this.chairId);

            this.actionKey = new List<int>();
            this.timerKey = new List<int>();

            this.effect = this.effect!=null
                ? this.effect
                : LTBY_PoolManager.Instance.GetUiItem("LTBY_JBPSpineAward4", LTBY_EffectManager.Instance.UiLayer);
            this.effect.gameObject.SetActive(true);

            this.topEffect = this.topEffect!=null ? this.topEffect : this.effect.FindChildDepth("TopEffect");
            this.BottomEffect = this.BottomEffect!=null ? this.BottomEffect : this.effect.FindChildDepth("BottomEffect");

            this.skeleton = this.skeleton!=null ? this.skeleton : this.effect.FindChildDepth<SkeletonGraphic>("Skeleton");
            this.TopRoot = TopRoot != null ? TopRoot : this.effect.FindChildDepth("Top");
            this.TopRoot.gameObject.SetActive(false);
            this.TopRoot.FindChildDepth<Text>("topText").text = $"+{data.accum_money}";

            this.BottomRoot = this.BottomRoot!=null ? this.BottomRoot : this.effect.FindChildDepth("Bottom");
            this.BottomRoot.gameObject.SetActive(false);
            this.bottomText = this.bottomText!=null ? this.bottomText : this.BottomRoot.FindChildDepth<Text>("bottomText");
            this.bottomText.text = $"x{data.fish_value}";

            this.TopMul = this.TopMul!=null ? this.TopMul : TopRoot.FindChildDepth<Text>("topText/TopMul");
            this.BottomMul = this.BottomMul!=null ? this.BottomMul : this.BottomRoot.FindChildDepth<Text>("BottomMul");
            string multipleText = data.display_multiple ? $"x{data.multiple}" : "";
            this.TopMul.text = multipleText;
            this.BottomMul.text = multipleText;
            Vector3 startPos = LTBYEntry.Instance.GetUIPosFromWorld(pos);
            this.effect.position = startPos;
            this.targetScale = 1;

            Vector3 targetPos = new Vector3(0, -0.8f, 0);
            if (!this.isSelf)
            {
                targetPos = LTBY_GameView.GameInstance.GetEffectWorldPos(this.chairId);
                if (!LTBY_GameView.GameInstance.CheckIsOtherSide(this.chairId))
                {
                    targetPos = new Vector3(targetPos.x, targetPos.y + 0.2f, targetPos.z);
                }
                else
                {
                    targetPos = new Vector3(targetPos.x, targetPos.y - 0.2f, targetPos.z);
                }

                this.targetScale = 0.5f;
            }

            this.effect.localScale = new Vector3(this.targetScale, this.targetScale, this.targetScale);

            if (this.skeleton != null)
            {
                this.skeleton.AnimationState.ClearTracks();
            }

            this.skeleton?.AnimationState.SetAnimation(0, "stand01", false);
            int key = LTBY_Extend.Instance.DelayRun(1.4f,
                ()=> { this.skeleton?.AnimationState.SetAnimation(0, "stand02", true); });
            this.timerKey.Add(key);

            float startX = startPos.x;
            float startY = startPos.y;
            float aimX = targetPos.x;
            float aimY = targetPos.y;

            float startTime = 1.4f;
            float numScale = 1f;
            Tween tween = DOTween.To(value =>
            {
                if (this.effect == null) return;
                float t = value * 0.001f;
                this.effect.position = new Vector3(startX + t * (aimX - startX), startY + t * (aimY - startY), -40);
            }, 0, 1000, 1.2f).SetEase(Ease.OutSine).SetDelay(startTime);
            key = LTBY_Extend.Instance.RunAction(tween);
            this.actionKey.Add(key);

            startTime += 1;

            if (this.isSelf)
            {
                key = LTBY_Extend.Instance.DelayRun(startTime, ()=>
                {
                    LTBY_Audio.Instance.Play(SoundConfig.JBPCoinDrop);
                    string EffectName = "Effect_Jubaopen_jinbidiaoluo";
                    float lifeTime = 6;
                    Vector3 position = Vector3.zero;
                    LTBY_EffectManager.Instance.CreateEffect<BaoJinBi>(0, EffectName, lifeTime, position);
                });
                this.timerKey.Add(key);
            }

            startTime += 0.4f;
            key = LTBY_Extend.Instance.DelayRun(startTime, ()=>
            {
                this.BottomRoot?.gameObject.SetActive(true);
                this.BottomEffect?.gameObject.SetActive(false);
                if (this.isSelf)
                {
                    LTBY_Audio.Instance.Play(SoundConfig.JBPFlashNum);
                }

                this.BottomEffect?.gameObject.SetActive(true);
            });
            this.timerKey.Add(key);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(BottomRoot?.DOScale(new Vector2(numScale * 1.5f, numScale * 1.5f), 0.2f));
            sequence.Append(BottomRoot?.DOScale(new Vector2(numScale, numScale), 0.7f).SetEase(Ease.OutSine));
            sequence.SetDelay(startTime);
            key = LTBY_Extend.Instance.RunAction(sequence);
            this.actionKey.Add(key);

            startTime += 1.4f;

            key = LTBY_Extend.Instance.DelayRun(startTime, ()=>
            {
                this.TopRoot?.gameObject.SetActive(true);
                this.topEffect?.gameObject.SetActive(false);
                if (this.isSelf)
                {
                    LTBY_Audio.Instance.Play(SoundConfig.JBPFlashNum);
                }

                this.topEffect?.gameObject.SetActive(true);
            });
            this.timerKey.Add(key);

            Sequence sequence1 = DOTween.Sequence();
            sequence1.Append(TopRoot?.DOScale(new Vector2(numScale * 1.5f, numScale * 1.5f), 0.2f));
            sequence1.Append(TopRoot?.DOScale(new Vector2(numScale, numScale), 0.7f).SetEase(Ease.OutSine));
            sequence1.SetDelay(startTime);
            key = LTBY_Extend.Instance.RunAction(sequence1);
            this.actionKey.Add(key);

            startTime += 2;
            key = LTBY_Extend.Instance.DelayRun(startTime, ()=>
            {
                LTBY_EffectManager.Instance.DestroyEffect<TreasureBowlEffect1>(this.chairId, this.id);
                LTBY_EffectManager.Instance.CreateEffect<TreasureBowlEffect2>(this.chairId, this.data, 3);
            });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            this.effect.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem("LTBY_JBPSpineAward4", this.effect);
            this.effect = null;
        }
    }

    public class BaoJinBi : EffectClass
    {
        private string EffectName;
        private Action callBack;

        public override void OnCreate(int _id, int _chairId, params object[] args)
        {
            base.OnCreate(_id, _chairId, args);
            this.name = nameof(BaoJinBi);
            this.id = _id;
            this.chairId = _chairId;
            this.timerKey = new List<int>();
            EffectParamData data = args.Length > 0 ? (EffectParamData) args[0] : new EffectParamData();
            this.EffectName = data.EffectName;
            this.callBack = data.callBack;

            this.effect = this.effect != null ? this.effect : LTBY_PoolManager.Instance.GetUiItem(this.EffectName, LTBY_EffectManager.Instance.UiLayer);
            float _scale = data.scale;
            this.effect.localScale = Mathf.Abs(_scale) > 0 ? new Vector3(_scale, _scale, _scale) : Vector3.one;
            this.effect.position = data.position;
            Vector3 localPos = this.effect.localPosition;

            this.effect.localPosition = new Vector3(localPos.x, localPos.y, 0);
            this.effect.gameObject.SetActive(true);

            int key = LTBY_Extend.Instance.DelayRun(data.lifeTime,
                ()=> { LTBY_EffectManager.Instance.DestroyEffect<BaoJinBi>(this.chairId, this.id); });
            this.timerKey.Add(key);
        }

        public override void OnStored()
        {
            base.OnStored();
            if (this.effect == null) return;
            LTBY_PoolManager.Instance.RemoveUiItem(this.EffectName, this.effect);
            this.effect = null;
        }
    }
}