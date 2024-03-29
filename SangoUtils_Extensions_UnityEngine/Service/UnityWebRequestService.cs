﻿using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_Extensions_UnityEngine.UnityWebRequestNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Service
{
    public class UnityWebRequestService : UnitySingleton<UnityWebRequestService>
    {
        private UnityWebRequestClient _httpClient = new UnityWebRequestClient();
        private Dictionary<int, BaseUnityWebRequestRequest> _requestDict = new Dictionary<int, BaseUnityWebRequestRequest>();

        public void OnInit()
        {
            _httpClient.Init();
        }

        protected void OnUpdate()
        {
            _httpClient.UpdateHttpRequest();
        }

        public void OnDispose()
        {
            _httpClient.Clear();
        }

        public void HttpGet<T>(int httpId, Dictionary<string, string> getParameDic) where T : class
        {
            string getParameStr;
            if (getParameDic == null || getParameDic.Count == 0)
                getParameStr = null;
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool isFirst = true;
                foreach (var item in getParameDic)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append('&');
                    }
                    stringBuilder.Append(Uri.EscapeDataString(item.Key));
                    stringBuilder.Append('=');
                    stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                getParameStr = stringBuilder.ToString();
            }
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, getParameStr);
        }

        public void HttpGet<T>(int httpId, string getParameStr) where T : class
        {
            //Protocol: getParameStr = url?parame1Name=parame1Value&parame2Name=parame2Value
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, getParameStr);
        }

        public void HttpGet<T>(int httpId) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, null);
        }

        public void HttpPost<T>(int httpId, Dictionary<string, string> postParameDic) where T : class
        {
            string postParameStr;
            if (postParameDic == null || postParameDic.Count == 0)
                postParameStr = null;
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool isFirst = true;
                foreach (var item in postParameDic)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append('&');
                    }
                    stringBuilder.Append(Uri.EscapeDataString(item.Key));
                    stringBuilder.Append('=');
                    stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                postParameStr = stringBuilder.ToString();
            }
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void HttpPost<T>(int httpId, string postParameStr) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void HttpPost<T>(int httpId, object postParame) where T : class
        {
            string postParameStr = JsonSerializer.Serialize(postParame);
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void HttpPost<T>(int httpId) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, null);
        }

        public void HttpPut(int httpId, string putParameStr)
        {
            _httpClient.SendHttpRequest<string>(httpId, UnityWebRequestType.Put, putParameStr);
        }

        public void HttpPut(int httpId, object putParame)
        {
            string putParameStr = JsonSerializer.Serialize(putParame);
            _httpClient.SendHttpRequest<string>(httpId, UnityWebRequestType.Put, putParameStr);
        }

        public void HttpBroadcast<T>(T data, int requestId, int resCode) where T : class
        {
            if (_requestDict.TryGetValue(requestId, out BaseUnityWebRequestRequest request))
            {
                request.OnOperationResponsed<T>(data, resCode);
            }
        }

        public void AddRequest(BaseUnityWebRequestRequest req)
        {
            if (!_requestDict.ContainsKey(req.HttpId))
            {
                _requestDict.Add(req.HttpId, req);
            }
            else
            {
                Debug.LogError("Already has this request.");
            }
        }

        public T GetRequest<T>(int httpId) where T : BaseUnityWebRequestRequest, new()
        {
            if (_requestDict.ContainsKey(httpId))
            {
                return (T)_requestDict[httpId];
            }
            else
            {
                T request = new T();
                request.OnInit(httpId);
                return request;
            }
        }

        public void RemoveRequest(BaseUnityWebRequestRequest req)
        {
            if (_requestDict.ContainsKey(req.HttpId))
            {
                _requestDict.Remove(req.HttpId);
            }
            else
            {
                Debug.LogError("Already remove this request.");
            }

        }

        public void HttpResource(UnityWebResourceBasePack httpBaseResourcePack)
        {
            _httpClient.SendHttpResourceRequest(httpBaseResourcePack);
        }
    }
}
