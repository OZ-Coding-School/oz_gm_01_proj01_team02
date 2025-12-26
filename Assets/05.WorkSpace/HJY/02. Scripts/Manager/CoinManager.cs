
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int Coin => TestGameManager.Instance.coin;

    private const string COIN_SAVE_KEY = "PERMANENT_COIN";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoin();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 코인 사용 
    public bool UseCoin(int amount)
    {
        if (TestGameManager.Instance.coin < amount)
            return false;

        // private set이라서 코인 읽기만 가능하고 수정이 불가능함
        TestGameManager.Instance.GetCoin(-amount);

        SaveCoin();
        return true;
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_SAVE_KEY, TestGameManager.Instance.coin);
        PlayerPrefs.Save();
    }

    public void LoadCoin()
    {
        if (PlayerPrefs.HasKey(COIN_SAVE_KEY))
        {
            int savedCoin = PlayerPrefs.GetInt(COIN_SAVE_KEY);
            int currentCoin = TestGameManager.Instance.coin;

            // 차이만큼 보정해라.
            TestGameManager.Instance.GetCoin(savedCoin - currentCoin);
        }
    }
}