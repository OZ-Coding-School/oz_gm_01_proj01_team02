using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevilPanelController : MonoBehaviour
{   
    [Header("Buff Pool")]
    public List<AngelBuffData> buffPool;
    
    [Header("UI")]
    public Image buffIconImage;
    public Text buffNameText;

    private AngelBuffData selectedBuff;    
    

    void OnEnable()
    {
        SelectRandomBuff();        
    }

    void SelectRandomBuff()
    {
        int randomIndex = Random.Range(0, buffPool.Count);
        selectedBuff = buffPool[randomIndex];

        buffIconImage.sprite = selectedBuff.icon;
        buffNameText.text = selectedBuff.displayName;
    }

    public void AcceptDevilDeal()
    {
        TestGameManager.Instance.DecreaseMaxHp(200);

        gameObject.SetActive(false);
    }

    public void RejectDevilDeal()
    {
        gameObject.SetActive(false);
    }
}
