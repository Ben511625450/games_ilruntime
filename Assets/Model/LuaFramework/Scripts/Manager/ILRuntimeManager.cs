using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LuaFramework
{
    public class ILRuntimeManager : Manager
    {
        ILRuntime.Runtime.Enviorment.AppDomain _appdomain;
        System.IO.MemoryStream _fs;
        System.IO.MemoryStream _p;

        public const string AesKey = "ILRuntime";

        public override void OnInitialize()
        {
            base.OnInitialize();
            _appdomain = new ILRuntime.Runtime.Enviorment.AppDomain(ILRuntimeJITFlags.JITOnDemand);
            EventHelper.LeaveGame += EventHelperOnLeaveGame;
        }

        private void EventHelperOnLeaveGame()
        {
            // AppFacade.Instance.GetManager<LuaManager>().CallFunction("GameSetsBtnInfo.LuaGameQuit");
        }

        public void LeaveGame()
        {
            EventHelper.DispatchLeaveGame();
        }

        public void LoadHotFix(byte[] dll, byte[] pdb)
        {
            StartCoroutine(LoadHotFixAssembly(dll, pdb));
        }

        IEnumerator LoadHotFixAssembly(byte[] dll, byte[] pdb)
        {
            yield return new WaitForEndOfFrame();
            _fs = new MemoryStream(AES.AESDecrypt(dll, AesKey));
            _p = new MemoryStream(AES.AESDecrypt(pdb, AesKey));
            try
            {
#if !UNITY_EDITOR
                _p = null;
#endif
                _appdomain?.LoadAssembly(_fs, _p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch
            {
                Debug.LogError("加载热更DLL失败，请确保已经编译过热更DLL");
            }

            InitializeILRuntime();
            SetupCLRRedirection();
#if UNITY_EDITOR
            _appdomain.DebugService.StartDebugService(56000);
#endif
            OnHotFixLoaded();
        }

        private unsafe void SetupCLRRedirection()
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
                {
                    _appdomain.RegisterCLRMethodRedirection(i, AddComponent);
                }

                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    _appdomain.RegisterCLRMethodRedirection(i, GetComponent);
                }
            }
        }

        /// <summary>
        /// 注册解释器和委托
        /// </summary>
        private void InitializeILRuntime()
        {
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(_appdomain);
            _appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
            _appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            _appdomain.DelegateManager.RegisterMethodDelegate<int>();
            _appdomain.DelegateManager.RegisterMethodDelegate<string>();
            _appdomain.DelegateManager.RegisterMethodDelegate<float>();
            _appdomain.DelegateManager.RegisterMethodDelegate<double>();
            _appdomain.DelegateManager.RegisterMethodDelegate<short>();
            _appdomain.DelegateManager.RegisterMethodDelegate<long>();
            _appdomain.DelegateManager.RegisterMethodDelegate<uint>();
            _appdomain.DelegateManager.RegisterMethodDelegate<ushort>();
            _appdomain.DelegateManager.RegisterMethodDelegate<ulong>();
            _appdomain.DelegateManager.RegisterMethodDelegate<decimal>();
            _appdomain.DelegateManager.RegisterMethodDelegate<bool>();
            _appdomain.DelegateManager.RegisterMethodDelegate<byte>();
            _appdomain.DelegateManager.RegisterMethodDelegate<byte[]>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Vector2>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Vector3>();
            _appdomain.DelegateManager.RegisterMethodDelegate<ByteBuffer>();
            _appdomain.DelegateManager.RegisterMethodDelegate<GameObject>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Transform>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Collider>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Collision>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Collider2D>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Collision2D>();
            _appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            _appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            _appdomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Spine.TrackEntry>();
            _appdomain.DelegateManager.RegisterMethodDelegate<LuaFramework.BytesPack>();
            _appdomain.DelegateManager.RegisterMethodDelegate<AssetBundle>();
            _appdomain.DelegateManager.RegisterMethodDelegate<System.String, LuaFramework.Session>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Scene, LoadSceneMode>();
            _appdomain.DelegateManager.RegisterMethodDelegate<GameObject, GameObject>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Object, Object,Object>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Object, Object>();
            _appdomain.DelegateManager.RegisterMethodDelegate<Object>();
            _appdomain.DelegateManager.RegisterFunctionDelegate<Object>();
            _appdomain.DelegateManager.RegisterFunctionDelegate<Object, Object>();
            _appdomain.DelegateManager.RegisterFunctionDelegate<Object, Object,Object>();
            _appdomain.DelegateManager.RegisterMethodDelegate<GameObject, PointerEventData>();
            _appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Object>();
            _appdomain.DelegateManager
                .RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,
                    UnityEngine.SceneManagement.LoadSceneMode>>((act) =>
                {
                    return new
                        UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,
                            UnityEngine.SceneManagement.LoadSceneMode>((arg0, arg1) =>
                        {
                            ((Action<UnityEngine.SceneManagement.Scene,
                                UnityEngine.SceneManagement.LoadSceneMode>) act)(arg0, arg1);
                        });
                });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() => { ((System.Action) act)(); });
            });


            _appdomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
            {
                return new System.AsyncCallback((ar) => { ((Action<System.IAsyncResult>) act)(ar); });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() => { ((Action) act)(); });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Single>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Single>((pNewValue) =>
                {
                    ((Action<System.Single>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Decimal>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Decimal>((pNewValue) =>
                {
                    ((Action<System.Decimal>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Double>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Double>((pNewValue) =>
                {
                    ((Action<System.Double>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Int16>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Int16>((pNewValue) =>
                {
                    ((Action<System.Int16>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Int32>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Int32>((pNewValue) =>
                {
                    ((Action<System.Int32>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Int64>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Int64>((pNewValue) =>
                {
                    ((Action<System.Int64>) act)(pNewValue);
                });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<Color>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<Color>((pNewValue) => { ((Action<Color>) act)(pNewValue); });
            });
            _appdomain.DelegateManager.RegisterDelegateConvertor<Spine.AnimationState.TrackEntryDelegate>((act) =>
            {
                return new Spine.AnimationState.TrackEntryDelegate((trackEntry) =>
                {
                    ((Action<Spine.TrackEntry>) act)(trackEntry);
                });
            });
            _appdomain.DelegateManager.RegisterMethodDelegate<System.String, DragonBones.EventObject>();
            _appdomain.DelegateManager.RegisterDelegateConvertor<DragonBones.ListenerDelegate<DragonBones.EventObject>>(
                (act) =>
                {
                    return new DragonBones.ListenerDelegate<DragonBones.EventObject>((type, eventObject) =>
                    {
                        ((Action<System.String, DragonBones.EventObject>) act)(type, eventObject);
                    });
                });

            _appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>) act)(arg0);
                });
            });


            CLRBindings.Initialize(_appdomain);
        }

        unsafe void OnHotFixLoaded()
        {
            try
            {
                _appdomain?.Invoke("Hotfix.ILLauncher", "Init", null, null);
                DebugTool.LogError($"加载ILRuntime成功");
            }
            catch (Exception e)
            {
                DebugTool.LogError($"加载ILRuntime,{e.Message}");
            }
        }

        unsafe void OnHotFixUnLoaded()
        {
            try
            {
                _appdomain?.Invoke("Hotfix.ILLauncher", "UnInit", null, null);
            }
            catch (Exception e)
            {
                DebugTool.LogError($"卸载ILRuntime,{e.Message}");
            }
        }

        public void EnterGame(string gameName)
        {
            EventHelper.DispatchOnEnterGame(gameName);
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
            EventHelper.LeaveGame -= EventHelperOnLeaveGame;
            OnHotFixUnLoaded();
            _fs?.Dispose();
            _p?.Dispose();
            _fs = null;
            _p = null;
        }

        private void OnDestroy()
        {
            UnInitialize();
        }

        MonoBehaviourAdapter.Adaptor GetComponent(ILType type)
        {
            var arr = GetComponents<MonoBehaviourAdapter.Adaptor>();
            for (int i = 0; i < arr.Length; i++)
            {
                var instance = arr[i];
                if (instance.ILInstance != null && instance.ILInstance.Type == type)
                {
                    return instance;
                }
            }

            return null;
        }

        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.AddComponent(type.TypeForCLR);
                }
                else
                {
                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance =
                        new ILTypeInstance(type as ILType, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                    //接下来创建Adapter实例
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    clrInstance.ILInstance = ilInstance;
                    clrInstance.AppDomain = __domain;
                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance

                    clrInstance.Awake(); //因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.GetComponent(type.TypeForCLR);
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
                            if (clrInstance.ILInstance.Type == type)
                            {
                                res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }
    }
}