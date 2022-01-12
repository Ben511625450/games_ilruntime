using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class EventHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::EventHelper);
            args = new Type[]{typeof(System.Action<LuaFramework.BytesPack>)};
            method = type.GetMethod("add_OnSocketReceive", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_OnSocketReceive_0);
            args = new Type[]{typeof(System.Action<System.String>)};
            method = type.GetMethod("add_OnEnterGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_OnEnterGame_1);
            args = new Type[]{typeof(System.Action<System.String>)};
            method = type.GetMethod("remove_OnEnterGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_OnEnterGame_2);
            args = new Type[]{typeof(System.Action<LuaFramework.BytesPack>)};
            method = type.GetMethod("remove_OnSocketReceive", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_OnSocketReceive_3);
            args = new Type[]{};
            method = type.GetMethod("DispatchLeaveGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DispatchLeaveGame_4);
            args = new Type[]{typeof(UnityEngine.UI.Button.ButtonClickedEvent), typeof(System.Action)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_5);
            args = new Type[]{typeof(UnityEngine.UI.Toggle.ToggleEvent), typeof(System.Action<System.Boolean>)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_6);
            args = new Type[]{typeof(UnityEngine.UI.Slider.SliderEvent), typeof(System.Action<System.Single>)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_7);


        }


        static StackObject* add_OnSocketReceive_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<LuaFramework.BytesPack> @value = (System.Action<LuaFramework.BytesPack>)typeof(System.Action<LuaFramework.BytesPack>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.OnSocketReceive += value;

            return __ret;
        }

        static StackObject* add_OnEnterGame_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.String> @value = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.OnEnterGame += value;

            return __ret;
        }

        static StackObject* remove_OnEnterGame_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.String> @value = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.OnEnterGame -= value;

            return __ret;
        }

        static StackObject* remove_OnSocketReceive_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<LuaFramework.BytesPack> @value = (System.Action<LuaFramework.BytesPack>)typeof(System.Action<LuaFramework.BytesPack>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.OnSocketReceive -= value;

            return __ret;
        }

        static StackObject* DispatchLeaveGame_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::EventHelper.DispatchLeaveGame();

            return __ret;
        }

        static StackObject* Add_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Button.ButtonClickedEvent @buttonClickedEvent = (UnityEngine.UI.Button.ButtonClickedEvent)typeof(UnityEngine.UI.Button.ButtonClickedEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.Add(@buttonClickedEvent, @action);

            return __ret;
        }

        static StackObject* Add_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @action = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Toggle.ToggleEvent @toggleEvent = (UnityEngine.UI.Toggle.ToggleEvent)typeof(UnityEngine.UI.Toggle.ToggleEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.Add(@toggleEvent, @action);

            return __ret;
        }

        static StackObject* Add_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Single> @action = (System.Action<System.Single>)typeof(System.Action<System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Slider.SliderEvent @sliderEvent = (UnityEngine.UI.Slider.SliderEvent)typeof(UnityEngine.UI.Slider.SliderEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::EventHelper.Add(@sliderEvent, @action);

            return __ret;
        }



    }
}
