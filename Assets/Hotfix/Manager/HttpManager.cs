﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using LuaFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace Hotfix
{
    public class HttpManager : SingletonILEntity<HttpManager>
    {
        /// <summary>
        ///     获取文本
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="callback">回调(正常为true，反之false)</param>
        public void GetText(string url, CAction<bool, string> callback = null)
        {
            ILGameManager.Instance.Behaviour.StartCoroutine(_GetText(url, callback));
        }

        /// <summary>
        ///     获取文本
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="data">表单数据</param>
        /// <param name="callback">回调(正常为true，反之false)</param>
        public void GetText(string url, FormData data, CAction<bool, string> callback = null)
        {
            var builder = new StringBuilder(url).Append("?");
            for (var i = 0; i < data.FieldNames.Count; i++)
            {
                if (i != 0) builder.Append("&");

                builder.Append(data.FieldNames[i]).Append("=").Append(data.FieldValues[i]);
            }

            ILGameManager.Instance.Behaviour.StartCoroutine(_GetText(builder.ToString(), callback));
        }

        private IEnumerator _GetText(string url, CAction<bool, string> callback)
        {
            yield return new WaitForEndOfFrame();
            var request = UnityWebRequest.Get(url);
            DebugHelper.Log(request.url);
            request.timeout = AppConst.csConfiger.DnsCheckOutTime;
            yield return request.SendWebRequest();
            if (request.result==UnityWebRequest.Result.ConnectionError || !string.IsNullOrEmpty(request.error))
            {
                callback?.Invoke(false, request.error);
                yield break;
            }

            callback?.Invoke(true, request.downloadHandler.text);
        }
    }
}