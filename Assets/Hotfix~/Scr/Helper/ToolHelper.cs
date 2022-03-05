using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Coffee.UIExtensions;
using DG.Tweening;
using LuaFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Hotfix.Hall;

namespace Hotfix
{
    public static class ToolHelper
    {
        public delegate TKey SelectHandler<TSource, TKey>(TSource source);

        public static string SetText(this Text text, object obj)
        {
            if (text == null) return null;
            text.text = obj.ToString();
            return text.text;
        }

        public static string SetText(this TextMeshProUGUI text, object obj, bool isRich = false)
        {
            if (text == null) return null;
            text.text = isRich ? ToolHelper.ShowRichText(obj) : obj.ToString();
            return text.text;
        }

        public static Transform FindChildDepth(this Transform transform, string childName)
        {
            var child = transform.Find(childName);
            if (child != null) return child;
            for (var i = 0; i < transform.childCount; i++)
            {
                child = transform.GetChild(i).FindChildDepth(childName);
                if (child != null) return child;
            }

            return child;
        }

        public static T FindChildDepth<T>(this Transform transform, string childName) where T : Component
        {
            var child = transform.FindChildDepth(childName);
            if (child != null) return child.GetComponent<T>();
            return default;
        }

        /// <summary>
        ///     弹窗
        /// </summary>
        public static void PopBigWindow(BigMessage message)
        {
            PopComponent.Instance?.ShowBig(message);
        }

        /// <summary>
        ///     弹窗
        /// </summary>
        public static void PopSmallWindow(string message)
        {
            PopComponent.Instance?.ShowSmall(message);
        }

        /// <summary>
        /// 添加热更节点
        /// </summary>
        /// <param name="gameObject">添加节点的物体</param>
        /// <typeparam name="T">添加类型</typeparam>
        /// <returns></returns>
        public static T AddILComponent<T>(this GameObject gameObject) where T : ILHotfixEntity, new()
        {
            T com = new T();
            com.Init(gameObject);
            return com;
        }

        /// <summary>
        /// 添加热更节点
        /// </summary>
        /// <param name="transform">添加节点的物体</param>
        /// <typeparam name="T">添加类型</typeparam>
        /// <returns></returns>
        public static T AddILComponent<T>(this Transform transform) where T : ILHotfixEntity, new()
        {
            return transform.gameObject.AddILComponent<T>();
        }

        /// <summary>
        /// 获取热更节点
        /// </summary>
        /// <param name="gameObject">获取节点的物体</param>
        /// <typeparam name="T">获取类型</typeparam>
        /// <returns></returns>
        public static T GetILComponent<T>(this GameObject gameObject) where T : ILHotfixEntity
        {
            if (gameObject == null) return null;
            ILBehaviour[] behaviours = gameObject.GetComponents<ILBehaviour>();
            string behavioutName = typeof(T).Name;
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (!behavioutName.Equals(behaviours[i].BehaviourName)) continue;
                return (T) behaviours[i].Behaviour;
            }

            return null;
        }

        /// <summary>
        /// 获取热更节点
        /// </summary>
        /// <param name="transform">获取节点的物体</param>
        /// <typeparam name="T">获取类型</typeparam>
        /// <returns></returns>
        public static T GetILComponent<T>(this Transform transform) where T : ILHotfixEntity
        {
            if (transform == null) return null;
            return transform.gameObject.GetILComponent<T>();
        }

        /// <summary>
        /// 移除物体上的热更组件
        /// </summary>
        /// <param name="gameObject">需要移除组件的物体</param>
        /// <param name="timer">延时</param>
        /// <typeparam name="T">移除的组件</typeparam>
        /// <returns></returns>
        public static void RemoveILComponent<T>(this GameObject gameObject, float timer = 0) where T : ILHotfixEntity
        {
            if (gameObject == null) return;
            ILBehaviour[] behaviours = gameObject.GetComponents<ILBehaviour>();
            string behavioutName = typeof(T).Name;
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (!behavioutName.Equals(behaviours[i].BehaviourName)) continue;
                if (timer <= 0)
                {
                    Object.Destroy(behaviours[i]);
                }
                else
                {
                    Object.Destroy(behaviours[i], timer);
                }
            }
        }

        /// <summary>
        /// 移除物体上的热更组件
        /// </summary>
        /// <param name="transform">需要移除组件的物体</param>
        /// <param name="timer">延时</param>
        /// <typeparam name="T">移除的组件</typeparam>
        /// <returns></returns>
        public static void RemoveILComponent<T>(this Transform transform, float timer = 0) where T : ILHotfixEntity
        {
            if (transform == null) return;
            transform.gameObject.RemoveILComponent<T>(timer);
        }

        /// <summary>
        ///     从html中通过正则找到ip信息(只支持ipv4地址)
        /// </summary>
        /// <param name="pageHtml"></param>
        /// <returns></returns>
        public static string GetIPFromHtml(string pageHtml)
        {
            //验证ipv4地址
            var reg =
                @"(?:(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))\.){3}(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))";
            var ip = "";
            var m = Regex.Match(pageHtml, reg);
            if (m.Success) ip = m.Value;

            return ip;
        }

        /// <summary>
        ///     查找列表中元素的索引
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="match">匹配函数</param>
        /// <typeparam name="T">元素类型</typeparam>
        /// <returns>索引</returns>
        public static int FindListIndex<T>(this List<T> list, CPredicate<T> match)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (match(list[i])) return i;
            }

            return -1;
        }

        /// <summary>
        ///     列表查找元素
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="match">匹配函数</param>
        /// <typeparam name="T">元素类型</typeparam>
        /// <returns>查找到的元素</returns>
        public static T FindItem<T>(this List<T> list, CPredicate<T> match)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (match(list[i])) return list[i];
            }

            return default;
        }

        /// <summary>
        ///     列表查找所有符合条件元素
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="match">匹配函数</param>
        /// <typeparam name="T">元素类型</typeparam>
        /// <returns>查找到的元素列表</returns>
        public static List<T> FindAllItem<T>(this List<T> list, CPredicate<T> match)
        {
            var newlist = new List<T>();
            for (var i = 0; i < list.Count; i++)
            {
                if (match(list[i])) newlist.Add(list[i]);
            }

            return newlist.Count == 0 ? default(List<T>) : newlist;
        }

        /// <summary>
        ///     显示富文本字体
        /// </summary>
        /// <param name="obj">需要处理的字符串</param>
        /// <param name="isThousand">是否千分位</param>
        /// <returns></returns>
        public static string ShowRichText(object obj, bool isThousand = false)
        {
            if (obj == null) return "";
            var text = obj.ToString();
            if (isThousand) text = long.Parse(text).ToString("N0");
            if (string.IsNullOrEmpty(text)) return "";
            var chars = text.ToCharArray();
            var t = new StringBuilder();
            for (var i = 0; i < chars.Length; i++)
            {
                t.Append($"<sprite name=\"{chars[i]}\" tint=1>");
            }

            return t.ToString();
        }

        /// <summary>
        /// 显示千分位字体
        /// </summary>
        /// <param name="obj">需要处理的字符串</param>
        /// <returns></returns>
        public static string ShowThousandText(object obj)
        {
            if (obj == null) return "";
            string text = obj.ToString();
            text = long.Parse(text).ToString("N0");
            return text;
        }

        /// <summary>
        ///     跑分工具
        /// </summary>
        /// <param name="orginGoal">开始分数</param>
        /// <param name="targetGoal">目标分数</param>
        /// <param name="durTimer">持续时间</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="pointCount">显示小数位数</param>
        public static Tweener RunGoal(float orginGoal, float targetGoal, float durTimer,
            CAction<string> changeCall = null, uint pointCount = 0)
        {
            return DOTween.To(delegate(float value) { changeCall?.Invoke(value.ToString($"f{pointCount}")); },
                orginGoal, targetGoal, durTimer).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 延迟操作
        /// </summary>
        /// <param name="timer">延迟时间</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static Tweener DelayRun(float timer, CAction callback = null)
        {
            Tweener tweener = DOTween.To(value => { }, 0, 1, timer).SetEase(Ease.Linear).OnComplete(() =>
            {
                callback?.Invoke();
            });
            tweener.SetAutoKill();
            return tweener;
        }

        /// <summary>
        ///     实例子物体
        /// </summary>
        /// <param name="obj">实例对象父物体</param>
        /// <param name="childIndex">索引</param>
        /// <returns></returns>
        public static GameObject InstantiateChild(this GameObject obj, int childIndex)
        {
            GameObject child;
            if (obj.transform.childCount <= childIndex)
            {
                child = Object.Instantiate(obj.transform.GetChild(0).gameObject, obj.transform);
                child.transform.localScale = Vector3.one;
            }
            else
            {
                child = obj.transform.GetChild(childIndex).gameObject;
            }

            return child;
        }

        /// <summary>
        ///     升序排列
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="handler">排列函数</param>
        public static void OrderBy<T, TKey>(this List<T> array, SelectHandler<T, TKey> handler)
            where TKey : IComparable<TKey>
        {
            for (var i = 0; i < array.Count - 1; i++)
            {
                for (var j = i + 1; j < array.Count; j++)
                {
                    if (handler(array[i]).CompareTo(handler(array[j])) > 0)
                    {
                        var temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        ///     降序排列
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="handler">排列函数</param>
        public static void OrderByDescending<T, TKey>(this List<T> array, SelectHandler<T, TKey> handler)
            where TKey : IComparable
        {
            for (var i = 0; i < array.Count - 1; i++)
            {
                for (var j = i + 1; j < array.Count; j++)
                {
                    if (handler(array[i]).CompareTo(handler(array[j])) < 0)
                    {
                        var temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }

        public static void OrderBy<T>(this List<T> list, CPredicate<T, T> handler)
        {
            for (var i = 0; i < list.Count - 1; i++)
            {
                for (var j = i + 1; j < list.Count; j++)
                {
                    if (handler(list[i], list[j]))
                    {
                        var temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
        }

        public static void OrderByDescending<T>(this List<T> list, CPredicate<T, T> handler)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (!handler(list[i], list[j]))
                    {
                        var temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
        }

        public static bool Write(string key, string val)
        {
            return HandleConfig.Write(key, val);
        }

        public static void ShowWaitPanel(bool isShow = true, string content = null)
        {
            if (isShow)
            {
                WaitPanel waitPanel = UIManager.Instance.GetUI<WaitPanel>();
                if (waitPanel != null)
                {
                    if (content != null)
                    {
                        waitPanel.SetContent(content);
                    }
                }
                else
                {
                    if (content != null)
                    {
                        UIManager.Instance.OpenUI<WaitPanel>(content);
                    }
                    else
                    {
                        UIManager.Instance.OpenUI<WaitPanel>();
                    }
                }
            }
            else
            {
                UIManager.Instance.CloseUI<WaitPanel>();
            }
        }

        /// <summary>
        ///     复制内容
        /// </summary>
        /// <param name="strID"></param>
        public static void SetText(string strID)
        {
            UniClipboard.SetText(strID);
        }

        /// <summary>
        /// 时间戳Timestamp转换成日期
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <param name="isMinSeconds"></param>
        /// <returns></returns>
        public static DateTime StampToDatetime(this long TimeStamp, bool isMinSeconds = false)
        {
            var startTime =
                TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)); //当地时区
            //返回转换后的日期
            return isMinSeconds ? startTime.AddMilliseconds(TimeStamp) : startTime.AddSeconds(TimeStamp);
        }


        /// <summary>
        /// 根据给定的数值转换成汉字大写格式
        /// </summary>
        /// <param name="numberStr">数值</param>
        /// <returns></returns>
        public static string ConvertNumberToChinese(string numberStr)
        {
            double.TryParse(numberStr, out double pSourceNumber);
            string result;
            if (pSourceNumber <= 0)
                result = "零";
            else
            {
                result = Num(pSourceNumber);
            }

            // result += result.IndexOf("点", StringComparison.Ordinal) < 0 ? "正" : "";
            return result;
        }
        /// <summary>
        /// 根据给定的数值转换成汉字大写格式
        /// </summary>
        /// <param name="pSourceNumber">数值</param>
        /// <returns></returns>
        public static string ConvertNumberToChinese(double pSourceNumber)
        {
            string result;
            if (pSourceNumber <= 0)
                result = "零";
            else
            {
                result = Num(pSourceNumber);
            }

            result += result.IndexOf("点", StringComparison.Ordinal) < 0 ? "正" : "";
            return result;
        }

        public static string Num(double num)
        {
            try
            {
                string str = string.Empty;
                if (num.ToString().IndexOf('.') >= 0)
                {
                    string[] strNum = num.ToString().Split('.');
                    str = NumToBig(strNum[0]);
                    str += "点";
                    str += NumToSmall(strNum[1]);
                }
                else
                    str = NumToBig(num.ToString());

                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 组合成大写数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string NumToBig(string num)
        {
            string str = "", strUnit = "", strZi = "零";
            for (int i = 0; i < num.Length; i++)
            {
                if (str.Length >= 1)
                {
                    if (!str.Substring(str.Length - 1, 1).Equals(strZi))
                        str += GetCapital(num[i]);
                    else if (!num[i].Equals('0'))
                        str += GetCapital(num[i]);
                }
                else
                {
                    str += GetCapital(num[i]);
                }

                if (!str.Substring(str.Length - 1, 1).Equals(strZi)) //不是零后面就要有数字单位
                    str += GetUnitName(num.Length - i);
                else if (num.Length - i == 5 && num.Length > 5) //判断万字的位置
                {
                    if (str.Substring(str.Length - 1, 1).Equals(strZi))
                    {
                        str = str.Substring(0, str.Length - 1);
                        strUnit = strZi;
                    }

                    if (!str.Substring(str.Length - 1, 1).Equals("亿")) //如果是亿，后面就不要跟万字
                        str += GetUnitName(num.Length - i);
                    if (!string.IsNullOrEmpty(strUnit))
                    {
                        if (!strUnit.Equals(strZi))
                            str += strUnit;
                        strUnit = "";
                    }
                }
                else if (num.Length - i == 9) //判断亿字的位置
                {
                    if (!str.Substring(str.Length - 1, 1).Equals(strZi))
                        str += GetUnitName(num.Length - i);
                    else
                        str = str.Substring(0, str.Length - 1) + GetUnitName(num.Length - i);
                }
            }

//最后有零就去掉
            if (str.Substring(str.Length - 1, 1).Equals(strZi))
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        private static string NumToSmall(string num)
        {
            string str = "", strZi = "零";
            for (int i = 0; i < num.Length; i++)
            {
                if (str.Length >= 1)
                {
                    if (!str.Substring(str.Length - 1, 1).Equals(strZi))
                        str += GetCapital(num[i]);
                    else if (!num[i].Equals('0'))
                        str += GetCapital(num[i]);
                }
                else
                    str += GetCapital(num[i]);
            }

//最后有零就去掉
            if (str.Substring(str.Length - 1, 1).Equals(strZi))
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        private static string GetUnitName(int numLength)
        {
            string str = string.Empty;
            switch (numLength)
            {
                case 2:
                case 6:
                case 10:
                    str = "拾";
                    break;
                case 3:
                case 7:
                case 11:
                    str = "佰";
                    break;
                case 4:
                case 8:
                case 12:
                    str = "仟";
                    break;
                case 5:
                case 13:
                    str = "萬";
                    break;
                case 9:
                    str = "亿";
                    break;
                default:
                    break;
            }

            return str;
        }

        private static string GetCapital(char c)
        {
            var result = string.Empty;
            switch (c)
            {
                case '0':
                    result = "零";
                    break;
                case '1':
                    result = "壹";
                    break;
                case '2':
                    result = "贰";
                    break;
                case '3':
                    result = "叁";
                    break;
                case '4':
                    result = "肆";
                    break;
                case '5':
                    result = "伍";
                    break;
                case '6':
                    result = "陆";
                    break;
                case '7':
                    result = "柒";
                    break;
                case '8':
                    result = "捌";
                    break;
                case '9':
                    result = "玖";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 数字转换大写方法
        /// </summary>
        /// <param name="strNumber">数字</param>
        /// <returns>大写字符串</returns>
        public static string Low2Up(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber))
            {
                return "";
            }

            string[] Unit = {"", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千", "万"};
            string[] Case = {"零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"};
            if (int.Parse(strNumber) <= 0) return Case[0];

            strNumber = long.Parse(strNumber).ToString();
            //返回字符串
            var strValue = "";
            var value = 0;
            for (var i = 0; i < strNumber.Length; i++)
            {
                value = int.Parse(strNumber[strNumber.Length - 1 - i].ToString());
                if (value != 0)
                {
                    strValue = strValue.Insert(0, string.Concat(Case[value], Unit[i]));
                }
                else
                {
                    if (i % 4 == 0 && i != 0) //万、亿、万亿
                    {
                        strValue = strValue.Insert(0, Unit[i]);
                    }
                    else
                    {
                        if (strValue.Length <= 0) continue;
                        if (strValue.Substring(0, 1) == Case[0]) continue;
                        strValue = strValue.Insert(0, Case[0]);
                    }
                }
            }

            return strValue;
        }

        /// <summary>
        ///     获取字典中的keys
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <returns></returns>
        public static TKey[] GetDictionaryKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) return new TKey[0];
            var keys = new TKey[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0);
            return keys;
        }

        /// <summary>
        ///     获取字典中的values
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="dictionary">当前字典</param>
        public static TValue[] GetDictionaryValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) return new TValue[0];
            var values = new TValue[dictionary.Count];
            dictionary.Values.CopyTo(values, 0);
            return values;
        }

        public static string ShowNumberText(long numInt)
        {
            string numStr = "";
            long tempNumInt = 0;
            int targNum = 10000;

            if ((numInt / targNum >= 1)) // 大于一万 使用“万”字图片
            {
                tempNumInt = numInt / targNum;
                numStr = $"{tempNumInt}w";

                if (numInt / 100000000 >= 1) //大于一亿 使用“亿”字图片
                {
                    tempNumInt = numInt / 100000000;
                    numStr = $"{tempNumInt}y";
                }

                string front = numStr.Substring(1, numStr.Length - 1); //string.sub(numStr, 1, (#numStr) - 1 ); --数字部分
                string endStr = numStr.Substring(numStr.Length, numStr.Length); //万" “亿” 部分
                if (front.Length > 4) //若数字部分长度大于4 则取前4位
                    front = front.Substring(1, 4);

                if ("." == front.Substring(4, 4)) //若第4位是符号 '.' 则省去
                    front = front.Substring(1, front.Length - 1);

                if (endStr == "w")
                    endStr = "万";

                else if (endStr == "y")
                    endStr = "亿";

                numStr = front + endStr; //将数字部分与 "万" "亿" 连接
            }
            else
            {
                tempNumInt = numInt;
                numStr = tempNumInt.ToString();
            }

            return numStr;
        }


        public static void MuteMusic()
        {
            ILMusicManager.Instance.SetMusicMute(true);
            MusicManager.isPlayMV = false;
            Util.Write("IsPlayAudio", MusicManager.isPlayMV.ToString().ToLower());
            PlayerPrefs.SetString("IsPlayAudio", MusicManager.isPlayMV.ToString().ToLower());
            AppFacade.Instance.GetManager<MusicManager>().SetPlaySM(MusicManager.isPlaySV, MusicManager.isPlayMV);
        }

        public static void MuteSound()
        {
            ILMusicManager.Instance.SetSoundMute(true);
            MusicManager.isPlaySV = false;
            Util.Write("isCanPlaySound", MusicManager.isPlaySV.ToString().ToLower());
            PlayerPrefs.SetString("isCanPlaySound", MusicManager.isPlaySV.ToString().ToLower());
            AppFacade.Instance.GetManager<MusicManager>().SetPlaySM(MusicManager.isPlaySV, MusicManager.isPlayMV);
        }

        public static void PlayMusic()
        {
            ILMusicManager.Instance.SetMusicMute(false);
            MusicManager.isPlayMV = true;
            Util.Write("IsPlayAudio", MusicManager.isPlayMV.ToString().ToLower());
            PlayerPrefs.SetString("IsPlayAudio", MusicManager.isPlayMV.ToString().ToLower());
            AppFacade.Instance.GetManager<MusicManager>().SetPlaySM(MusicManager.isPlaySV, MusicManager.isPlayMV);
        }

        public static void PlaySound()
        {
            ILMusicManager.Instance.SetSoundMute(false);
            MusicManager.isPlaySV = true;
            Util.Write("isCanPlaySound", MusicManager.isPlaySV.ToString().ToLower());
            PlayerPrefs.SetString("isCanPlaySound", MusicManager.isPlaySV.ToString().ToLower());
            AppFacade.Instance.GetManager<MusicManager>().SetPlaySM(MusicManager.isPlaySV, MusicManager.isPlayMV);
        }

        public static void Destroy(Object obj, float timer = 0)
        {
            if (timer <= 0)
            {
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj, timer);
            }
        }

        public static T Instantiate<T>(T original, Transform parent = null) where T : UnityEngine.Object
        {
            if (parent == null)
            {
                return UnityEngine.Object.Instantiate<T>(original);
            }

            return UnityEngine.Object.Instantiate<T>(original, parent);
        }

        /// <summary>
        /// 设置图片颜色模式
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mode"></param>
        public static void SetImageEffect(GameObject go, EffectMode mode)
        {
            Image img = go.GetComponent<Image>();
            if (img == null)
            {
                DebugHelper.LogError($"该节点{go.name}缺少Image组件，无法设置颜色模式");
                return;
            }

            UIEffect e = img.GetComponent<UIEffect>();
            if (e == null)
            {
                DebugHelper.LogError($"该节点{go.name}缺少UIEffect组件，无法设置颜色模式");
                return;
            }

            e.effectFactor = mode == EffectMode.Grayscale ? 1 : 0;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isMilliseconds"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dt, bool isMilliseconds)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan ts = dt - startTime;
            return isMilliseconds ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }
    }
}