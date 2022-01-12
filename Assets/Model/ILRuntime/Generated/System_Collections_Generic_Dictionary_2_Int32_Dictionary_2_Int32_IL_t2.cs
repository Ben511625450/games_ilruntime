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
    unsafe class System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>.ValueCollection);
            args = new Type[]{typeof(System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>[]), typeof(System.Int32)};
            method = type.GetMethod("CopyTo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CopyTo_0);


        }


        static StackObject* CopyTo_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>[] @array = (System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>[])typeof(System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>.ValueCollection instance_of_this_method = (System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>.ValueCollection)typeof(System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.Dictionary<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>.ValueCollection).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CopyTo(@array, @index);

            return __ret;
        }



    }
}
