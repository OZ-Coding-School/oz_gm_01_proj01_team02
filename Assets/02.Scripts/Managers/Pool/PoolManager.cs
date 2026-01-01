using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager pool_instance { get; private set; }

    public Dictionary<string, object> pools = new Dictionary<string, object>();


    private void Awake()
    {
        if(pool_instance == null)
        {
            pool_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void CreatePool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) 
        {
            return; 
        }

        
        string key = prefab.name;
        if (pools.ContainsKey(key)) return;
        if (parent == null) parent = this.transform;

        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent));
        
    }

    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null)
        {
            return null; 
        }

        if (!pools.TryGetValue(prefab.name, out var box))
        {
            return null;
        }

        var pool = box as ObjectPool<T>;

        if (pool != null) 
        {
            return pool.Dequeue(); 
        }
        else return null;

    }

    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        if (instance == null) 
        {
            return; 
        }
        if(!pools.TryGetValue(instance.gameObject.name, out var box))
        {
            Destroy(instance.gameObject);
            return;
        }

        var pool = box as ObjectPool<T>;
        if (pool != null) 
        {
            pool.Enqueue(instance); 
        }
        else return;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
       
        if (scene.name == "TitleScene(kms)")
        {
            ClearPool();
        }
    }

    public void ClearPool()
    {
        foreach (var entry in pools)
        {
            if (entry.Value is IObjectPool pool) 
            {
                pool.ReturnAllObjects();
            }
        }
        pools.Clear();

        Debug.Log("모든 풀이 초기화 되었음.");
    }

}
