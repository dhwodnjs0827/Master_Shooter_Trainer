using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class AddressableCatalog
{
    public string Key;
    public string Type;
    public string Address;
}

public sealed class ResourceManager : SingletonBehaviour<ResourceManager>
{
    private Dictionary<string, Object> resourcePool;

    protected override void Awake()
    {
        base.Awake();
        resourcePool = new Dictionary<string, Object>();
        AddressableUtility.AddressMapping();
        Init();
    }

    private void Init()
    {
        var init = Addressables.InitializeAsync();
        init.Completed += (handler) =>
        {
            Debug.Log("초기화 완료");
        };

        var asyncOperationHandle = Addressables.DownloadDependenciesAsync("InitDownload");
        asyncOperationHandle.Completed += (op) =>
        {
            Debug.Log("다운로드 완료");
        };
    }
    
    public T LoadAsset<T>(string address) where T : Object
    {
        if (resourcePool.TryGetValue(address, out var obj))
        {
            return (T)obj;
        }
        else
        {
            var asset = GetAsset<T>(address);
            if (asset != null)
            {
                resourcePool.Add(address, asset);
            }
            return asset;
        }
    }

    public List<T> LoadAssets<T>(string label) where T : Object
    {
        List<T> retList = new List<T>();
        var keys = resourcePool.Keys.ToList().FindAll(obj => obj.Contains(label));
        if(keys.Count > 0)
        {
            foreach(var newKey in keys)
            {
                retList.Add((T)resourcePool[newKey]);
            }
        }
        else
        {
            retList = GetAssets<T>(label);
        }
        return retList;
    }

    private T GetAsset<T>(string address) where T : Object
    {
        var path = address;
        try
        {
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                var obj = Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
                return obj.GetComponent<T>();
            }
            else
            {
                return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return null;
    }

    private List<T> GetAssets<T>(string label) where T : Object
    {
        var list = Addressables.LoadAssetsAsync<T>(label, (obj) => {
            Debug.Log(obj.ToString());
        }).WaitForCompletion();
        List<T> assets = list != null ? new List<T>(list) : new List<T>(); 
        return assets;
    }

    #region Resources

    private readonly Dictionary<string, object> resourcePools = new(); // 리소스 캐싱용

    /// <summary>
    /// Resources.Load()와 사용법 같음
    /// </summary>
    public T Load<T>(string resourcePath) where T : Object
    {
        if (resourcePools.TryGetValue(resourcePath, out var value))
        {
            // 캐싱된 리소스 반환
            return value as T;
        }

        var resource = Resources.Load<T>(resourcePath);
        if (resource != null)
        {
            // 처음 사용하는 리소스면 캐싱
            resourcePools.Add(resourcePath, resource);
        }

        return resource;
    }

    /// <summary>
    /// Resources.LoadAll()와 사용법 같음
    /// </summary>
    public T[] LoadAll<T>(string resourcePath) where T : Object
    {
        if (resourcePools.TryGetValue(resourcePath, out var value))
        {
            return value as T[];
        }

        var resource = Resources.LoadAll<T>(resourcePath);
        if (resource != null)
        {
            resourcePools.Add(resourcePath, resource);
        }

        return resource;
    }

    #endregion
}