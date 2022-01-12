﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaFramework;
using LuaInterface;

public class UnityEngine_GameObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.GameObject), typeof(UnityEngine.Object));
		L.RegFunction("CreatePrimitive", CreatePrimitive);
		L.RegFunction("GetComponent", GetComponent);
		L.RegFunction("GetComponentInChildren", GetComponentInChildren);
		L.RegFunction("GetComponentInParent", GetComponentInParent);
		L.RegFunction("GetComponents", GetComponents);
		L.RegFunction("GetComponentsInChildren", GetComponentsInChildren);
		L.RegFunction("GetComponentsInParent", GetComponentsInParent);
		L.RegFunction("TryGetComponent", TryGetComponent);
		L.RegFunction("FindWithTag", FindWithTag);
		L.RegFunction("SendMessageUpwards", SendMessageUpwards);
		L.RegFunction("SendMessage", SendMessage);
		L.RegFunction("BroadcastMessage", BroadcastMessage);
		L.RegFunction("AddComponent", AddComponent);
		L.RegFunction("SetActive", SetActive);
		L.RegFunction("CompareTag", CompareTag);
		L.RegFunction("FindGameObjectWithTag", FindGameObjectWithTag);
		L.RegFunction("FindGameObjectsWithTag", FindGameObjectsWithTag);
		L.RegFunction("Find", Find);
		L.RegFunction("GetChilds", GetChilds);
		L.RegFunction("SetParent", SetParent);
		L.RegFunction("SetGlobalRotate", SetGlobalRotate);
		L.RegFunction("SetRotate", SetRotate);
		L.RegFunction("SetScale", SetScale);
		L.RegFunction("SetGlobalPos", SetGlobalPos);
		L.RegFunction("SetPos", SetPos);
		L.RegFunction("Release", Release);
		L.RegFunction("New", _CreateUnityEngine_GameObject);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("transform", get_transform, null);
		L.RegVar("layer", get_layer, set_layer);
		L.RegVar("activeSelf", get_activeSelf, null);
		L.RegVar("activeInHierarchy", get_activeInHierarchy, null);
		L.RegVar("isStatic", get_isStatic, set_isStatic);
		L.RegVar("tag", get_tag, set_tag);
		L.RegVar("scene", get_scene, null);
		L.RegVar("sceneCullingMask", get_sceneCullingMask, null);
		L.RegVar("gameObject", get_gameObject, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_GameObject(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UnityEngine.GameObject obj = new UnityEngine.GameObject();
				ToLua.PushSealed(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.GameObject obj = new UnityEngine.GameObject(arg0);
				ToLua.PushSealed(L, obj);
				return 1;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<System.Type>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Type[] arg1 = ToLua.ToParamsObject<System.Type>(L, 2, count - 1);
				UnityEngine.GameObject obj = new UnityEngine.GameObject(arg0, arg1);
				ToLua.PushSealed(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.GameObject.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatePrimitive(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.PrimitiveType arg0 = (UnityEngine.PrimitiveType)ToLua.CheckObject(L, 1, typeof(UnityEngine.PrimitiveType));
			UnityEngine.GameObject o = UnityEngine.GameObject.CreatePrimitive(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<System.Type>(L, 2))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = (System.Type)ToLua.ToObject(L, 2);
				UnityEngine.Component o = obj.GetComponent(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.ToString(L, 2);
				UnityEngine.Component o = obj.GetComponent(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentInChildren(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Component o = obj.GetComponentInChildren(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				UnityEngine.Component o = obj.GetComponentInChildren(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponentInChildren");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentInParent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Component o = obj.GetComponentInParent(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				UnityEngine.Component o = obj.GetComponentInParent(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponentInParent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponents(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Component[] o = obj.GetComponents(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				System.Collections.Generic.List<UnityEngine.Component> arg1 = (System.Collections.Generic.List<UnityEngine.Component>)ToLua.CheckObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.Component>));
				obj.GetComponents(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponents");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentsInChildren(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Component[] o = obj.GetComponentsInChildren(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				UnityEngine.Component[] o = obj.GetComponentsInChildren(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponentsInChildren");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentsInParent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Component[] o = obj.GetComponentsInParent(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				System.Type arg0 = ToLua.CheckMonoType(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				UnityEngine.Component[] o = obj.GetComponentsInParent(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.GetComponentsInParent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryGetComponent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			System.Type arg0 = ToLua.CheckMonoType(L, 2);
			UnityEngine.Component arg1 = null;
			bool o = obj.TryGetComponent(arg0, out arg1);
			LuaDLL.lua_pushboolean(L, o);
			ToLua.Push(L, arg1);
			return 2;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindWithTag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.GameObject o = UnityEngine.GameObject.FindWithTag(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMessageUpwards(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				obj.SendMessageUpwards(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.SendMessageOptions>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				UnityEngine.SendMessageOptions arg1 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 3);
				obj.SendMessageUpwards(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<object>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.SendMessageUpwards(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.SendMessageOptions arg2 = (UnityEngine.SendMessageOptions)ToLua.CheckObject(L, 4, typeof(UnityEngine.SendMessageOptions));
				obj.SendMessageUpwards(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SendMessageUpwards");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMessage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				obj.SendMessage(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.SendMessageOptions>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				UnityEngine.SendMessageOptions arg1 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 3);
				obj.SendMessage(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<object>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.SendMessage(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.SendMessageOptions arg2 = (UnityEngine.SendMessageOptions)ToLua.CheckObject(L, 4, typeof(UnityEngine.SendMessageOptions));
				obj.SendMessage(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SendMessage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BroadcastMessage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				obj.BroadcastMessage(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.SendMessageOptions>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				UnityEngine.SendMessageOptions arg1 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 3);
				obj.BroadcastMessage(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<object>(L, 3))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.BroadcastMessage(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.SendMessageOptions arg2 = (UnityEngine.SendMessageOptions)ToLua.CheckObject(L, 4, typeof(UnityEngine.SendMessageOptions));
				obj.BroadcastMessage(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.BroadcastMessage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddComponent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			System.Type arg0 = ToLua.CheckMonoType(L, 2);
			UnityEngine.Component o = obj.AddComponent(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetActive(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.SetActive(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompareTag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.CompareTag(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindGameObjectWithTag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.GameObject o = UnityEngine.GameObject.FindGameObjectWithTag(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindGameObjectsWithTag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.GameObject[] o = UnityEngine.GameObject.FindGameObjectsWithTag(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Find(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.GameObject o = UnityEngine.GameObject.Find(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetChilds(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			UnityEngine.Transform[] o = obj.GetChilds();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetParent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3 && TypeChecker.CheckTypes<UnityEngine.Transform, bool>(L, 2))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 2);
				bool arg1 = LuaDLL.lua_toboolean(L, 3);
				obj.SetParent(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.GameObject, bool>(L, 2))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 2);
				bool arg1 = LuaDLL.lua_toboolean(L, 3);
				obj.SetParent(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetParent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGlobalRotate(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				obj.SetGlobalRotate(arg0);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				obj.SetGlobalRotate(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetGlobalRotate");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRotate(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				obj.SetRotate(arg0);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				obj.SetRotate(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetRotate");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScale(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				obj.SetScale(arg0);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				obj.SetScale(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetScale");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGlobalPos(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				obj.SetGlobalPos(arg0);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				obj.SetGlobalPos(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetGlobalPos");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPos(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				obj.SetPos(arg0);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				obj.SetPos(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.GameObject.SetPos");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			obj.Release();
			return 0;
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
	static int get_transform(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			UnityEngine.Transform ret = obj.transform;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index transform on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_layer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			int ret = obj.layer;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layer on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_activeSelf(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			bool ret = obj.activeSelf;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index activeSelf on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_activeInHierarchy(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			bool ret = obj.activeInHierarchy;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index activeInHierarchy on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isStatic(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			bool ret = obj.isStatic;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isStatic on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tag(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			string ret = obj.tag;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index tag on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_scene(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			UnityEngine.SceneManagement.Scene ret = obj.scene;
			ToLua.PushValue(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index scene on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sceneCullingMask(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			ulong ret = obj.sceneCullingMask;
			LuaDLL.tolua_pushuint64(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index sceneCullingMask on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_gameObject(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			UnityEngine.GameObject ret = obj.gameObject;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index gameObject on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_layer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.layer = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layer on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isStatic(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isStatic = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isStatic on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_tag(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.GameObject obj = (UnityEngine.GameObject)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.tag = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index tag on a nil value");
		}
	}
}

