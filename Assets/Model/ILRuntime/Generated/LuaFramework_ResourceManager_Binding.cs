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
    unsafe class LuaFramework_ResourceManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(LuaFramework.ResourceManager);

            field = type.GetField("bundles", flag);
            app.RegisterCLRFieldGetter(field, get_bundles_0);
            app.RegisterCLRFieldSetter(field, set_bundles_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_bundles_0, AssignFromStack_bundles_0);


        }



        static object get_bundles_0(ref object o)
        {
            return ((LuaFramework.ResourceManager)o).bundles;
        }

        static StackObject* CopyToStack_bundles_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((LuaFramework.ResourceManager)o).bundles;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_bundles_0(ref object o, object v)
        {
            ((LuaFramework.ResourceManager)o).bundles = (System.Collections.Generic.Dictionary<System.String, UnityEngine.AssetBundle>)v;
        }

        static StackObject* AssignFromStack_bundles_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.String, UnityEngine.AssetBundle> @bundles = (System.Collections.Generic.Dictionary<System.String, UnityEngine.AssetBundle>)typeof(System.Collections.Generic.Dictionary<System.String, UnityEngine.AssetBundle>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((LuaFramework.ResourceManager)o).bundles = @bundles;
            return ptr_of_this_method;
        }



    }
}
