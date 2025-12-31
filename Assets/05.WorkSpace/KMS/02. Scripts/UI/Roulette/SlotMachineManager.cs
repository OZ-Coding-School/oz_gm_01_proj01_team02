using System.Collections;
using System.Collections.Generic;
using STH.Characters.Player;
using STH.ScriptableObjects.Base;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Skill Data")]
    public List<SkillData> skillDataList;

    private List<SkillData> rollingDeck;
    private HashSet<SkillData> usedResultSkills;
    private SkillData[] finalThreeSlots; 
    


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


    private float imageHeight = 100.0f;
    private PlayerController player;
    private int repeatCount = 20;


    void OnEnable()
    {
        Time.timeScale = 0.0f;

        player = FindObjectOfType<PlayerController>();
        resultSkills = new SkillData[slotColumns.Length];
        usedResultSkills = new HashSet<SkillData>();

        displayResultImage.sprite = null;

        
        FinalThreeSlots();
        PlaySlotMachine();
    }

    private void FinalThreeSlots()
    {
         List<SkillData> candidates = new List<SkillData>();
        foreach (var s in skillDataList)
        {
            // 액티브는 플레이어가 가진 스킬 제외
            if (s.GetType().Name == "StatSkillData" || !player.Skills.Contains(s))
                candidates.Add(s);
        }

        finalThreeSlots = new SkillData[3];
        List<SkillData> temp = new List<SkillData>(candidates);
        List<SkillData> selectedSoFar = new List<SkillData>();

        for (int i = 0; i < 3; i++)
        {
            if (temp.Count == 0) break;

            List<SkillData> filtered = temp.FindAll(s =>
            s.GetType().Name != "StatSkillData" || !selectedSoFar.Contains(s));

            if (filtered.Count == 0) break;

            int idx = Random.Range(0, filtered.Count);
            finalThreeSlots[i] = filtered[idx];

            if (filtered[idx].GetType().Name == "StatSkillData")
               {
                    selectedSoFar.Add(filtered[idx]);
                     temp.RemoveAll(s => s == filtered[idx]);
               }
            
            
               temp.Remove(filtered[idx]);
        }
    }

    public void PlaySlotMachine()
    {
        slotMachineUI.SetActive(true);

    

        for (int i = 0; i < slotButtons.Length; i++)
        {
            slotButtons[i].interactable = false;
            SetRandomIcons(i);
            StartCoroutine(StartSlot(i, repeatCount));
        }
    }

    private void SetRandomIcons(int columnIndex)
    {
        Image[] images = slotImages[columnIndex].images;

        for (int i = 0; i < images.Length; i++)
        {

           SkillData randomSkill = skillDataList[Random.Range(0, skillDataList.Count)];

           images[i].sprite = randomSkill.icon;
        }
    }

    IEnumerator StartSlot(int index, int repeatCount)
    {  
        
        RectTransform column = slotColumns[index];

        for (int i = 0; i < repeatCount; i++)
        {
            column.localPosition -= new Vector3(0, imageHeight, 0);

        if  (column.localPosition.y < 0.0f) // 맨 아래 도달하면
            {
                column.localPosition += new Vector3(0, imageHeight * slotImages[index].images.Length, 0);
            }
                yield return new WaitForSecondsRealtime(0.02f);
        }

        SkillData finalSkill;

        if (index >= slotColumns.Length -3)
            finalSkill = finalThreeSlots[index - (slotColumns.Length - 3)];
        else
            finalSkill = GetFinalSkill();

        if (finalSkill == null)
        {
            Debug.LogError($"finalSkill is null at column {index}");
            slotButtons[index].interactable = true;
            yield break; // 여기서 코루틴 종료
        }
        resultSkills[index] = finalSkill;
        usedResultSkills.Add(finalSkill);

        int centerIndex = GetCenterIndex(index);
        slotImages[index].images[centerIndex].sprite = finalSkill.icon;

        slotButtons[index].interactable = true;
    }

    private SkillData GetFinalSkill()
    {
        List<SkillData> candidates = new List<SkillData>();

        foreach (SkillData skill in skillDataList)
        {
            Debug.Log($"{skill.name} 타입: {skill.GetType().Name}");
            if (skill.GetType().Name == "StatSkillData")
            {
               
                Debug.Log($"{skill.name} → 패시브 후보");
                candidates.Add(skill);
            }

            else if (!usedResultSkills.Contains(skill) && !player.Skills.Contains(skill))
                {
                    Debug.Log($"{skill.name} → 액티브 후보");
                    candidates.Add(skill);
                }
            
        }

         return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : null;
    
    }

    

    private int GetCenterIndex(int index)
    {
        float yPos = slotColumns[index].localPosition.y;
        float h = slotImages[index].images[0].rectTransform.rect.height;
        int centerIndex = Mathf.RoundToInt(Mathf.Abs(yPos) / h);
        return Mathf.Clamp(centerIndex, 0, slotImages[index].images.Length - 1);
    }


    public void ClickBtn(int index)
    {
        
        SkillData skill = resultSkills[index];
        if (skill == null) return;
        skill.Apply(player);

        displayResultImage.sprite = skill.icon;
        StartCoroutine(DisableAfterDelay(1.0f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        slotMachineUI.SetActive(false);
        gameObject.SetActive(false);
        hpBar.SetActive(true);
        Time.timeScale = 1.0f;
    }




    
}
