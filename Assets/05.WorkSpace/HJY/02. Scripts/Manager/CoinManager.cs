
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    private const string COIN_SAVE_KEY = "PERMANENT_COIN";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCoin(PlayerData playerData)
    {
        PlayerPrefs.SetInt(COIN_SAVE_KEY, playerData.totalCoin);
        PlayerPrefs.Save();
    }

    public void LoadCoin(PlayerData playerData)
    {
        if (PlayerPrefs.HasKey(COIN_SAVE_KEY))
        {
            playerData.totalCoin = PlayerPrefs.GetInt(COIN_SAVE_KEY);
        }
    }
}