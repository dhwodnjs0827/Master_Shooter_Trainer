using System.Collections;
using UnityEngine;

public class CoroutineRunner : SingletonBehaviour<CoroutineRunner>
{
    protected override void Awake()
    {
        base.Awake();
        isDontDestroyOnLoad = false;
    }
    
    public static Coroutine Run(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void Stop(IEnumerator coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }
    
    public static void StopAll()
    {
        Instance.StopAllCoroutines();
    }
}
