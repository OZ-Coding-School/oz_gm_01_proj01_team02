
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
        Refresh();
    }

    private void OnDisable()
    {
        PlayerStatManager.OnStatChanged -= Refresh;
    }

    public void Refresh()
    {
        var stat = PlayerStatManager.Instance;

        //hpText.text = $"HP : {stat.maxHp}";
        //attackText.text = $"ATK : {stat.attack}";
    }
}

