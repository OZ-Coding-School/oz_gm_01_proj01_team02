using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetLootingBox : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemText;

    // 스프라이트 이름이랑 아이템 변수명을 맞춰야함
    public void SetData(Sprite sprite, int count)
    {
        itemIcon.sprite = sprite;
        itemText.text = "X" + count.ToString();
    }
}
