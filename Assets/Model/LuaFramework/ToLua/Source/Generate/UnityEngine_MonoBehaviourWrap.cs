﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using DG.Tweening;
using LuaInterface;

public class UnityEngine_MonoBehaviourWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.MonoBehaviour), typeof(UnityEngine.Behaviour));
		L.RegFunction("IsInvoking", IsInvoking);
		L.RegFunction("CancelInvoke", CancelInvoke);
		L.RegFunction("Invoke", Invoke);
		L.RegFunction("InvokeRepeating", InvokeRepeating);
		L.RegFunction("StartCoroutine", StartCoroutine);
		L.RegFunction("StopCoroutine", StopCoroutine);
		L.RegFunction("StopAllCoroutines", StopAllCoroutines);
		L.RegFunction("print", print);
		L.RegFunction("DOTogglePause", DOTogglePause);
		L.RegFunction("DOSmoothRewind", DOSmoothRewind);
		L.RegFunction("DORewind", DORewind);
		L.RegFunction("DORestart", DORestart);
		L.RegFunction("DOPlayForward", DOPlayForward);
		L.RegFunction("DOPlayBackwards", DOPlayBackwards);
		L.RegFunction("DOPlay", DOPlay);
		L.RegFunction("DOPause", DOPause);
		L.RegFunction("DOGoto", DOGoto);
		L.RegFunction("DOFlip", DOFlip);
		L.RegFunction("DOKill", DOKill);
		L.RegFunction("DOComplete", DOComplete);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("useGUILayout", get_useGUILayout, set_useGUILayout);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInvoking(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				bool o = obj.IsInvoking();
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				bool o = obj.IsInvoking(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.IsInvoking");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CancelInvoke(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				obj.CancelInvoke();
				return 0;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.CancelInvoke(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.CancelInvoke");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Invoke(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			obj.Invoke(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InvokeRepeating(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
			obj.InvokeRepeating(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartCoroutine(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				UnityEngine.Coroutine o = obj.StartCoroutine(arg0);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<System.Collections.IEnumerator>(L, 2))
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				System.Collections.IEnumerator arg0 = (System.Collections.IEnumerator)ToLua.ToObject(L, 2);
				UnityEngine.Coroutine o = obj.StartCoroutine(arg0);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.Coroutine o = obj.StartCoroutine(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.StartCoroutine");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopCoroutine(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<System.Collections.IEnumerator>(L, 2))
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				System.Collections.IEnumerator arg0 = (System.Collections.IEnumerator)ToLua.ToObject(L, 2);
				obj.StopCoroutine(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Coroutine>(L, 2))
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				UnityEngine.Coroutine arg0 = (UnityEngine.Coroutine)ToLua.ToObject(L, 2);
				obj.StopCoroutine(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				obj.StopCoroutine(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.StopCoroutine");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopAllCoroutines(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			obj.StopAllCoroutines();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int print(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			UnityEngine.MonoBehaviour.print(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOTogglePause(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOTogglePause();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOSmoothRewind(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOSmoothRewind();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DORewind(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				int o = obj.DORewind();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DORewind(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.DORewind");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DORestart(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				int o = obj.DORestart();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DORestart(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.DORestart");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOPlayForward(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOPlayForward();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOPlayBackwards(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOPlayBackwards();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOPlay(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOPlay();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOPause(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOPause();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOGoto(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				int o = obj.DOGoto(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				int o = obj.DOGoto(arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.DOGoto");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOFlip(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
			int o = obj.DOFlip();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOKill(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				int o = obj.DOKill();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DOKill(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.DOKill");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DOComplete(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				int o = obj.DOComplete();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DOComplete(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.MonoBehaviour.DOComplete");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_useGUILayout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)o;
			bool ret = obj.useGUILayout;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index useGUILayout on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_useGUILayout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.MonoBehaviour obj = (UnityEngine.MonoBehaviour)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.useGUILayout = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index useGUILayout on a nil value");
		}
	}
}

