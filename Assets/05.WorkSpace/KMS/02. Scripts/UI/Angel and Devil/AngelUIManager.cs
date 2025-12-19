using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AngelUIManagers : MonoBehaviour
{
    
        [Header("Buttons")]
        public Button leftBuffButton;
        public Button healButton;


        [Header("Left Buff UI")]
        public Image leftBuffIcon;
        public Text leftBuffText;

        [Header("Heal UI")]
        public Text healText;


        [Header("Buff Data List")]
        public AngelBuffData[] buffDataList;
        private AngelBuffData currentBuffData;


    
        
    void OnEnable()
    {
        SetupAngelUI();
        Time.timeScale = 0.0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }


    private void SetupAngelUI()
    {
        currentBuffData = GetRandomBuffData();

        leftBuffText.text = currentBuffData.displayName;
        leftBuffIcon.sprite = currentBuffData.icon;

        leftBuffButton.onClick.RemoveAllListeners();
        leftBuffButton.onClick.AddListener(() => ApplyBuff(currentBuffData));

        healButton.onClick.RemoveAllListeners();
        healButton.onClick.AddListener(HealPlayer);

    }

    private AngelBuffData GetRandomBuffData()
    {

        int count = buffDataList.Length;
        return buffDataList[UnityEngine.Random.Range(0, count)];
    }

    private void ApplyBuff(AngelBuffData data)
    {
        Debug.Log($" {data.buffType}");
        Close();
    }

    private void HealPlayer()
    {
        Close();
    }


    private void Close()
    {
        gameObject.SetActive(false);
    }


}
