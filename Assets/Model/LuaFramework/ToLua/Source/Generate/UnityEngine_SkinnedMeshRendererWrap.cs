﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using DG.Tweening;
using LuaInterface;

public class UnityEngine_SkinnedMeshRendererWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.SkinnedMeshRenderer), typeof(UnityEngine.Renderer));
		L.RegFunction("GetBlendShapeWeight", GetBlendShapeWeight);
		L.RegFunction("SetBlendShapeWeight", SetBlendShapeWeight);
		L.RegFunction("BakeMesh", BakeMesh);
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
		L.RegFunction("New", _CreateUnityEngine_SkinnedMeshRenderer);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("quality", get_quality, set_quality);
		L.RegVar("updateWhenOffscreen", get_updateWhenOffscreen, set_updateWhenOffscreen);
		L.RegVar("forceMatrixRecalculationPerRender", get_forceMatrixRecalculationPerRender, set_forceMatrixRecalculationPerRender);
		L.RegVar("rootBone", get_rootBone, set_rootBone);
		L.RegVar("bones", get_bones, set_bones);
		L.RegVar("sharedMesh", get_sharedMesh, set_sharedMesh);
		L.RegVar("skinnedMotionVectors", get_skinnedMotionVectors, set_skinnedMotionVectors);
		L.RegVar("localBounds", get_localBounds, set_localBounds);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_SkinnedMeshRenderer(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UnityEngine.SkinnedMeshRenderer obj = new UnityEngine.SkinnedMeshRenderer();
				ToLua.Push(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.SkinnedMeshRenderer.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBlendShapeWeight(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			float o = obj.GetBlendShapeWeight(arg0);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBlendShapeWeight(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			obj.SetBlendShapeWeight(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BakeMesh(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				UnityEngine.Mesh arg0 = (UnityEngine.Mesh)ToLua.CheckObject(L, 2, typeof(UnityEngine.Mesh));
				obj.BakeMesh(arg0);
				return 0;
			}
			else if (count == 3)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				UnityEngine.Mesh arg0 = (UnityEngine.Mesh)ToLua.CheckObject(L, 2, typeof(UnityEngine.Mesh));
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.BakeMesh(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.BakeMesh");
			}
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				int o = obj.DORewind();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DORewind(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.DORewind");
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
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				int o = obj.DORestart();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DORestart(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.DORestart");
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				int o = obj.DOGoto(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				int o = obj.DOGoto(arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.DOGoto");
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
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
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
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				int o = obj.DOKill();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DOKill(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.DOKill");
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
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				int o = obj.DOComplete();
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)ToLua.CheckObject<UnityEngine.SkinnedMeshRenderer>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				int o = obj.DOComplete(arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.SkinnedMeshRenderer.DOComplete");
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
	static int get_quality(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.SkinQuality ret = obj.quality;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index quality on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_updateWhenOffscreen(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool ret = obj.updateWhenOffscreen;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index updateWhenOffscreen on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_forceMatrixRecalculationPerRender(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool ret = obj.forceMatrixRecalculationPerRender;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index forceMatrixRecalculationPerRender on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rootBone(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Transform ret = obj.rootBone;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index rootBone on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bones(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Transform[] ret = obj.bones;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index bones on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sharedMesh(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Mesh ret = obj.sharedMesh;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index sharedMesh on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_skinnedMotionVectors(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool ret = obj.skinnedMotionVectors;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index skinnedMotionVectors on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localBounds(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Bounds ret = obj.localBounds;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index localBounds on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_quality(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.SkinQuality arg0 = (UnityEngine.SkinQuality)ToLua.CheckObject(L, 2, typeof(UnityEngine.SkinQuality));
			obj.quality = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index quality on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_updateWhenOffscreen(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.updateWhenOffscreen = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index updateWhenOffscreen on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_forceMatrixRecalculationPerRender(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.forceMatrixRecalculationPerRender = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index forceMatrixRecalculationPerRender on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rootBone(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
			obj.rootBone = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index rootBone on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bones(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Transform[] arg0 = ToLua.CheckObjectArray<UnityEngine.Transform>(L, 2);
			obj.bones = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index bones on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sharedMesh(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Mesh arg0 = (UnityEngine.Mesh)ToLua.CheckObject(L, 2, typeof(UnityEngine.Mesh));
			obj.sharedMesh = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index sharedMesh on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_skinnedMotionVectors(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.skinnedMotionVectors = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index skinnedMotionVectors on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_localBounds(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.SkinnedMeshRenderer obj = (UnityEngine.SkinnedMeshRenderer)o;
			UnityEngine.Bounds arg0 = ToLua.ToBounds(L, 2);
			obj.localBounds = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index localBounds on a nil value");
		}
	}
}

