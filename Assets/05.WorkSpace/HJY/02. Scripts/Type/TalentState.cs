
[System.Serializable]
public class TalentState
{
    public TalentData data;
    public int level;

    public bool IsMaxLevel => level >= data.maxLevel; // 현재 레벨이 최대 레벨 이상이면 true 반환!
}
