using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("전역 데이터")]
    public int totalCoin;
    public int adventureExp;
    public int adventureLevel = 1;
    public int maxAdventureLevel = 20;
    
    public int[] nextAdventureExp = { 100, 150, 200, 300, 400, 600, 800, 1000, 1300, 1600, 2000, 2400, 2800, 3200, 3600, 4200, 4800, 5400, 6000};


    [Header("Selected Map")]
    [SerializeField] MapData _selectedMap;
    public MapData selectedMap => _selectedMap;

    public event Action<MapData> OnSelectedMapChanged;


    public void AddCoin(int amount)
    {
        totalCoin += amount;
    }
    public void SetSelectedMap(MapData data)
    {
        if (_selectedMap == data) return;

        _selectedMap = data;
        OnSelectedMapChanged?.Invoke(data);
    }

    public void AddadventrueExp(int amount)
    {
        if (adventureLevel >= maxAdventureLevel)
        return;

        adventureExp += amount;

        while (adventureLevel < maxAdventureLevel && adventureExp >= nextAdventureExp[adventureLevel - 1])
        {
            adventureExp -= nextAdventureExp[adventureLevel -1];
            adventureLevel++;
            
        }

        

    }
}
