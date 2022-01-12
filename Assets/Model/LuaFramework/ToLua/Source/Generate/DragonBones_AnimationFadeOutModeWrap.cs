﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class DragonBones_AnimationFadeOutModeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(DragonBones.AnimationFadeOutMode));
		L.RegVar("None", get_None, null);
		L.RegVar("SameLayer", get_SameLayer, null);
		L.RegVar("SameGroup", get_SameGroup, null);
		L.RegVar("SameLayerAndGroup", get_SameLayerAndGroup, null);
		L.RegVar("All", get_All, null);
		L.RegVar("Single", get_Single, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<DragonBones.AnimationFadeOutMode>.Check = CheckType;
		StackTraits<DragonBones.AnimationFadeOutMode>.Push = Push;
	}

	static void Push(IntPtr L, DragonBones.AnimationFadeOutMode arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(DragonBones.AnimationFadeOutMode), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_None(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.None);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SameLayer(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.SameLayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SameGroup(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.SameGroup);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SameLayerAndGroup(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.SameLayerAndGroup);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_All(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.All);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Single(IntPtr L)
	{
		ToLua.Push(L, DragonBones.AnimationFadeOutMode.Single);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		DragonBones.AnimationFadeOutMode o = (DragonBones.AnimationFadeOutMode)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

