using System.Collections;
using System.Collections.Generic;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using UnityEngine;
using UnityEngine.UI;

public class DevilUIManager : MonoBehaviour
{   
    [Header("Buff Pool")]
    public List<SkillData> skillData;
    
    [Header("UI")]
    public Image buffIconImage;
    public Text buffNameText;

    private SkillData selectedBuff;
    private PlayerController player;    
    

    void OnEnable()
    {
        Time.timeScale = 0.0f;
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.Log("Player 없음");
        }
        SelectRandomBuff();        
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    void SelectRandomBuff()
    {
        int randomIndex = Random.Range(0, skillData.Count);
        selectedBuff = skillData[randomIndex];

        buffIconImage.sprite = selectedBuff.icon;
        buffNameText.text = selectedBuff.skillName;
    }

    public void AcceptDevilDeal()
    {
        PlayerStatManager statManager = PlayerStatManager.Instance;
        if (statManager != null)
        {
            statManager.permanentmaxHpBonus -= 200.0f;
            statManager.RecalculateStats();

            selectedBuff.Apply(player);
            gameObject.SetActive(false);
        }
        
    }

    public void RejectDevilDeal()
    {
        gameObject.SetActive(false);
    }
}
