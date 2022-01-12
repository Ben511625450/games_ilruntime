﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class DragonBones_SortingModeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(DragonBones.SortingMode));
		L.RegVar("SortByZ", get_SortByZ, null);
		L.RegVar("SortByOrder", get_SortByOrder, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<DragonBones.SortingMode>.Check = CheckType;
		StackTraits<DragonBones.SortingMode>.Push = Push;
	}

	static void Push(IntPtr L, DragonBones.SortingMode arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(DragonBones.SortingMode), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SortByZ(IntPtr L)
	{
		ToLua.Push(L, DragonBones.SortingMode.SortByZ);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SortByOrder(IntPtr L)
	{
		ToLua.Push(L, DragonBones.SortingMode.SortByOrder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		DragonBones.SortingMode o = (DragonBones.SortingMode)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

