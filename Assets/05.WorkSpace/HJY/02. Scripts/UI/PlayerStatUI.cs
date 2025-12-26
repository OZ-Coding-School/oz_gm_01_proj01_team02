
using UnityEngine;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
    [Header("Stat Texts")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;

    private void OnEnable()
    {
        PlayerStatManager.OnStatChanged += Refresh;
    }

    private void Start()
    {
        Refresh(); // 기본값을 우선 직접 불러옴
    }

    private void OnDisable()
    {
        PlayerStatManager.OnStatChanged -= Refresh;
    }

    public void Refresh()
    {
        var stat = PlayerStatManager.Instance;
      
        hpText.text = $"{stat.maxHp}";
        attackText.text = $"{stat.attack}";
    }
}

