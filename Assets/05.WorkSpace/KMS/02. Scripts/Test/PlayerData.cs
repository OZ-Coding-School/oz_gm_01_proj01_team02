using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("전역 데이터")]
    public int totalCoin;
    public int totalExp;
    public int level = 1;
    public int[] nextExp = { 10, 20, 30, 40, 50};


    [Header("Selected Map")]
    [SerializeField] MapData _selectedMap;
    public MapData selectedMap => _selectedMap;

    public event Action<MapData> OnSelectedMapChanged;

    public void SetSelectedMap(MapData data)
    {
        if (_selectedMap == data) return;

        _selectedMap = data;
        OnSelectedMapChanged?.Invoke(data);
    }
}
