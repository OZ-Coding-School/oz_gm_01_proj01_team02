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
            CreateManager(ref _testgamemanager, "TestGameManager");
            return _testgamemanager;
        }
    }

    public static void PlayerisDead() // 플레이어 사망 시 등장할 메서드
    {
        GameOver();
        _stage.InitStageClearCount();
        if (_pool != null) _pool.ClearPool();
        SceneManager.LoadScene("TitleScene");
    }

    public static void ClearChapter() // 챕터 클리어시 등장할 메서드
    {
        GameOver();
        _stage.InitStageClearCount();
        if (_pool != null) _pool.ClearPool();
        Debug.Log(_stage.currentStage);
    }

    public static void GameOver()
    {
        foreach (var i in Data.collectedItem)
        {
            Debug.Log(i.Key);
            Debug.Log(i.Value);
        }
        Stage.OnClearPanel(Test.coin, Test.exp, Stage.currentStage, int.Parse(Stage.chapter[7].ToString()));
    }

}
