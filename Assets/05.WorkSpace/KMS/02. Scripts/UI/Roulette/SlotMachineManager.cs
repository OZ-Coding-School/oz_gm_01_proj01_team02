using System.Collections;
using System.Collections.Generic;
using STH.Characters.Player;
using STH.ScriptableObjects.Base;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Skill Data")]
    public List<SkillData> skillDataList;


    [Header("UI")]
    public GameObject slotMachineUI;
    public GameObject hpBar;
    public Image displayResultImage;


    [Header("Slot Columns")]
    public RectTransform[] slotColumns;

    [System.Serializable]
    public class SlotColumn
    {
        public Image[] images;
    }
    public SlotColumn[] slotImages;

    [Header("Buttons")]
    public Button[] slotButtons;
    private SkillData[] resultSkills;
    private int itemCnt = 3;


    private float imageHeight = 50.0f;


    

    void OnEnable()
    {
        Time.timeScale = 0.0f;
        displayResultImage.sprite = null;

        resultSkills = new SkillData[slotColumns.Length];

        PlaySlotMachine();
    }

    public void PlaySlotMachine()
    {
        int fixedRepeatCount = 20;
        for (int i = 0; i < slotButtons.Length; i++)
        {
            slotButtons[i].interactable = false;
            SetRandomIcons(i);
            StartCoroutine(StartSlot(i, fixedRepeatCount));
        }
    }

    private void SetRandomIcons(int columnIndex)
    {
        Image[] images = slotImages[columnIndex].images;

        for (int i = 0; i < images.Length; i++)
        {
            int randomIndex = Random.Range(0, skillDataList.Count);
            images[i].sprite = skillDataList[randomIndex].icon;
        }
    }

    IEnumerator StartSlot(int index, int repeatCount)
    {
        RectTransform column = slotColumns[index];

        for (int i = 0; i < repeatCount; i++)
        {
            column.localPosition -= new Vector3(0, 50f, 0);

        if (column.localPosition.y < -imageHeight) // 맨 아래 도달하면
              column.localPosition += new Vector3(0, imageHeight * slotImages[index].images.Length, 0);

          yield return new WaitForSecondsRealtime(0.02f);
        }

    // 중앙 이미지 선택
        int finalIndex = GetCenterIndex(index);
        Sprite finalSprite = slotImages[index].images[finalIndex].sprite;
        SkillData selectedSkill = skillDataList.Find(s => s.icon == finalSprite);

        resultSkills[index] = selectedSkill;
        slotButtons[index].interactable = true;
    }

    int GetCenterIndex(int index)
    {
        float yPos = slotColumns[index].localPosition.y;
        float imageHeight = slotImages[index].images[0].rectTransform.rect.height;
        int centerIndex = Mathf.RoundToInt(Mathf.Abs(yPos) / imageHeight);
        return Mathf.Clamp(centerIndex, 0, slotImages[index].images.Length - 1);
    }

    public void ClickBtn(int index)
    {
        
        SkillData skill = resultSkills[index];
        if (skill == null) return;
        PlayerController player = FindObjectOfType<PlayerController>();
        skill.Apply(player);

        displayResultImage.sprite = skill.icon;
        StartCoroutine(DisableAfterDelay(1.0f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        slotMachineUI.SetActive(false);
        hpBar.SetActive(true);
        Time.timeScale = 1.0f;
    }




    
}
