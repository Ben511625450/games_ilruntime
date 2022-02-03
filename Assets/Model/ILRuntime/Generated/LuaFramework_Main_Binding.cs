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
    unsafe class LuaFramework_Main_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(LuaFramework.Main);

            field = type.GetField("reporter", flag);
            app.RegisterCLRFieldGetter(field, get_reporter_0);
            app.RegisterCLRFieldSetter(field, set_reporter_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_reporter_0, AssignFromStack_reporter_0);


        }



        static object get_reporter_0(ref object o)
        {
            return ((LuaFramework.Main)o).reporter;
        }

        static StackObject* CopyToStack_reporter_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((LuaFramework.Main)o).reporter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_reporter_0(ref object o, object v)
        {
            ((LuaFramework.Main)o).reporter = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_reporter_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @reporter = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((LuaFramework.Main)o).reporter = @reporter;
            return ptr_of_this_method;
        }



    }
}
