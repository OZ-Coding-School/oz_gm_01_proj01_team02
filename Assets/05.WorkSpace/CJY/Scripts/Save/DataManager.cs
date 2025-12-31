using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayData
{
    public int _coin;
    public int _exp;
    public int _stage;
    public int _chapter;

    // ↓↓↓사용 안하고 있었던듯?↓↓↓
    public Dictionary<int, object> _equipment = new Dictionary<int, object>(); //-> 장비가 완성되야 추가가능
}

public class EquipmentsData
{
    public Dictionary<string, int> equipmentsData = new Dictionary<string, int>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public PlayData playData = new();
    public EquipmentsData equipmentData = new();

    private string path;
    private string fileName = "/save";
    private string keyWord = "evzxkjnv158ezxcvf^%2agasdf687gb3%g7nbvauoi3@8g8fabn^%zxncvkg18aaetx";

    public Dictionary<string, int> collectedItem = new Dictionary<string, int>(); // 획득한 아이템 이름과 갯수.(코인, 경험치 포함)
    // 인벤토리와 연동하면 될듯.
    public List<string> collectedItemName = new List<string>();

    private void Awake()
    {
        instance = this;

        path = Application.persistentDataPath + fileName;
        Debug.Log(path);
    }
    

    public void Save()
    {
        string data = JsonUtility.ToJson(playData);

        File.WriteAllText(path, EncryptAndDecrypt(data));
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            Save();
        }

        string data = File.ReadAllText(path);
        playData = JsonUtility.FromJson<PlayData>(EncryptAndDecrypt(data));
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }


    public void AddData(int coin, int exp, int stage, int chapter)
    {
        playData._coin = coin;
        playData._exp = exp;
        playData._stage = stage;
        playData._chapter = chapter;

        Save();
        Load();
    }

    public int GetItemCount()
    {
        int count = 2;
        count += playData._equipment.Count;

        return count;
    }


    public Dictionary<string, int> GetInGameEquipmentInfo()
    {
        return collectedItem;
    }
}
