
using UnityEngine;
using UnityEngine.EventSystems;

public class TalentIcon : MonoBehaviour, IPointerClickHandler
{
    public int index;// InventoryManager.ownedTalents에서 몇 번째 재능인지
    public TalentUI talentUI;// TalentUI 스크립트 참조

    public void OnPointerClick(PointerEventData eventData)
    {
        talentUI.OnClickTalent(index);
    }
}

