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
    private static TestGameManager _testgamemanager;

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

    public static TestGameManager Test
    {
        get
        {
            if (_testgamemanager != null) return _testgamemanager;
            _testgamemanager = Object.FindObjectOfType<TestGameManager>();

            if (_testgamemanager == null)
            {
                CreateManager(ref _testgamemanager, "TestGameManager");
            }
            return _testgamemanager;
        }
    }


    public static void PlayerisDead()
    {
        GameOver();
    }

    public static void ClearChapter()
    {
        GameOver();
    }

    public static void GameOver()
    {
        Stage.OnClearPanel(Test.coin, Test.exp, Stage.currentStage, int.Parse(Stage.chapter[7].ToString()));
    }

}
