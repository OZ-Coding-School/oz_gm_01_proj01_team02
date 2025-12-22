using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestedGameManager
{
    private static GameObject _root;
    private static PoolManager _pool;
    private static StageManager _stage;
    public static int clearStage { get; private set; } = 1;

    private static void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }
    }

    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init();
            GameObject obj = new GameObject(name);
            manager = obj.AddComponent<T>();
            Object.DontDestroyOnLoad(obj);
            obj.transform.SetParent(_root.transform);
        }
    }

    public static PoolManager Pool
    {
        get
        {
            CreateManager(ref _pool, "PoolManager");
            return _pool;
        }
    }

    public static StageManager Stage
    {
        get
        {
            CreateManager(ref _stage, "StageManager");
            return _stage;
        }
    }

    public static void StageIncrease()
    {
        clearStage++;
    }

    public static void InitStageClearCount()
    {
        clearStage = 0;
    }

    public static void ClearChapter()
    {
        InitStageClearCount();
        if (_pool != null) _pool.ClearPool();
        SceneManager.LoadScene("TitleScene");
        Debug.Log(clearStage);
    }
}
