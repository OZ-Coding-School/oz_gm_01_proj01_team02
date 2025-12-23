using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameObject _root;
    private static PoolManager _pool;
    private static StageManager _stage;
    private static DataManager _data;

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
    public static DataManager Data
    {
        get
        {
            CreateManager(ref _data, "DataManager");
            return _data;
        }
    }

    public static void PlayerisDead() // 플레이어 사망 시 등장할 메서드
    {
        _stage.InitStageClearCount();
        if (_pool != null) _pool.ClearPool();
        SceneManager.LoadScene("TitleScene");
    }

    public static void ClearChapter() // 챕터 클리어시 등장할 메서드
    {
        _stage.InitStageClearCount();
        if (_pool != null) _pool.ClearPool();
        SceneManager.LoadScene("TitleScene"); // -> 클리어 패널이 나오는게 맞음.
        Debug.Log(_stage.currentStage);
    }

}
