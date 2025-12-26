using System.Collections.Generic;
using STH.Characters.Player;
using STH.ScriptableObjects.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AngelUIManagers : MonoBehaviour
{

        [Header("Passive Skill")]
        public List<SkillData> skillData;
        public Button passiveButton;
        public Text passiveText;

    
        [Header("Heal")]
        public Button healButton;
        public Sprite healSprite;
        public float healAmount = 500.0f;

        private SkillData selectedPassive;
        private PlayerController player;
        private PlayerHealth playerHp;

        


        

    
        
    void OnEnable()
    {
        Time.timeScale = 0.0f;
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.Log("Player 없음");
            return;
        }

        playerHp = player.GetComponent<PlayerHealth>();
        if (playerHp == null)
        {
            Debug.Log("PlayerHealth 없음");
            return;
        }
        

        SetupPassive();
        SetupHeal();
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    private void SetupPassive()
    {
        if (skillData == null || skillData.Count == 0)
        {
            Debug.Log("SkillData 없음");
            return;
        }

        selectedPassive = skillData[Random.Range(0, skillData.Count)];

        passiveButton.image.sprite = selectedPassive.icon;
        passiveText.text = selectedPassive.skillName;

        passiveButton.onClick.RemoveAllListeners();
        passiveButton.onClick.AddListener(OnClickPassive);
 

    }

    private void OnClickPassive()
    {
        selectedPassive.Apply(player);
        Close();
    }


    private void SetupHeal()
    {
        if (healButton == null) return;

        healButton.image.sprite = healSprite;

        healButton.onClick.RemoveAllListeners();
        healButton.onClick.AddListener(OnClickHeal);
    }

    private void OnClickHeal()
    {
        HealPlayer();
        Close();
    }

    private void HealPlayer()
    {
        playerHp.Heal(healAmount);
    }


    private void Close()
    {
        gameObject.SetActive(false);
    }


}
