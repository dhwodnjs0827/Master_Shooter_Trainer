using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressableUtility
{
    private static Dictionary<string, string> catalog = new();
    
    public static void AddressMapping()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        
        for (var i = 0; i < settings.groups.Count; i++)
        {
            // Addressable 그룹 이름 불러오기
            Debug.Log(settings.groups[i].Name);
        }
    }

    public static void LoadCatalog()
    {
        
    }
}
