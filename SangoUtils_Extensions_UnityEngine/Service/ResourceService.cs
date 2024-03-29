﻿using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_Extensions_UnityEngine.UnityWebRequestNet;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils_Extensions_UnityEngine.Service
{
    public class ResourceService : UnitySingleton<ResourceService>
    {
        private UnityWebResourceLoader_RawImage _resourceRawImageLoader = new UnityWebResourceLoader_RawImage();

        private Dictionary<string, AudioClip> _audioClipDict = new Dictionary<string, AudioClip>();
        private Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();
        private Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();
        private Dictionary<string, TMP_FontAsset> _fontDict = new Dictionary<string, TMP_FontAsset>();

        private Dictionary<string, Texture> _rawImageTextureDict = new Dictionary<string, Texture>();

        public AudioClip LoadAudioClip(string audioClipPath, bool isCache)
        {
            _audioClipDict.TryGetValue(audioClipPath, out AudioClip audioClip);
            if (audioClip == null)
            {
                audioClip = Resources.Load<AudioClip>(audioClipPath);
                if (isCache)
                {
                    if (!_audioClipDict.ContainsKey(audioClipPath))
                    {
                        _audioClipDict.Add(audioClipPath, audioClip);
                    }
                }
            }
            return audioClip;
        }

        public Sprite LoadSprite(string spritePath, bool isCache)
        {
            _spriteDict.TryGetValue(spritePath, out Sprite sprite);
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>(spritePath);
                if (isCache)
                {
                    if (!_spriteDict.ContainsKey(spritePath))
                    {
                        _spriteDict.Add(spritePath, sprite);
                    }
                }
            }
            return sprite;
        }

        public GameObject LoadPrefab(string prefabPath, bool isCache)
        {
            _prefabDict.TryGetValue(prefabPath, out GameObject prefab);
            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>(prefabPath);
                if (isCache)
                {
                    if (!_prefabDict.ContainsKey(prefabPath))
                    {
                        _prefabDict.Add(prefabPath, prefab);
                    }
                }
            }
            return prefab;
        }

        public GameObject InstantiatePrefab(Transform parentTrans, string prefabPath, bool isCache = false)
        {
            GameObject prefab = LoadPrefab(prefabPath, isCache);
            GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
            return instantiatedPrefab;
        }

        public TMP_FontAsset LoadFont(string fontPath, bool isCache)
        {
            _fontDict.TryGetValue(fontPath, out TMP_FontAsset font);
            if (font == null)
            {
                font = Resources.Load<TMP_FontAsset>(fontPath);
                if (isCache)
                {
                    if (!_fontDict.ContainsKey(fontPath))
                    {
                        _fontDict.Add(fontPath, font);
                    }
                }
            }
            return font;
        }

        public uint LoadAndSetRawImageOnlineAsync(RawImage targetRawImage, string urlPath, bool isCahce, Action<object[]> completeCallBack, Action<object[]> canceledCallBack, Action<object[]> erroredCallBack)
        {
            uint packId = 0;
            _rawImageTextureDict.TryGetValue(urlPath, out Texture texture);
            {
                if (texture == null)
                {
                    packId = _resourceRawImageLoader.AddPack(targetRawImage, urlPath, isCahce, AddRawImageTextureCacheCB, completeCallBack, canceledCallBack, erroredCallBack);
                }
                else
                {
                    if (targetRawImage != null)
                    {
                        targetRawImage.texture = texture;
                        completeCallBack?.Invoke(null);
                    }
                }
            }
            return packId;
        }

        public bool RemoveRawImageOnlineAsyncPack(uint packId)
        {
            return _resourceRawImageLoader.RemovePack(packId);
        }

        private void AddRawImageTextureCacheCB(string urlPath, Texture texture)
        {
            if (!_rawImageTextureDict.ContainsKey(urlPath))
            {
                _rawImageTextureDict.Add(urlPath, texture);
            }
        }
    }
}
