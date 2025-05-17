using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableCatalog
{
    public string Key;
    public string Type;
    public string Address;
}

public sealed class ResourceManager : SingletonBehaviour<ResourceManager>
{
    private Dictionary<string, Object> resourcesDict;

    protected override void Awake()
    {
        base.Awake();
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
        var operationHandle = Addressables.LoadAssetAsync<T>(address);
        
        return null;
    }

    public T LoadAssets<T>(string label) where T : Object
    {
        return null;
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