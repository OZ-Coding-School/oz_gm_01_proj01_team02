
[System.Serializable]
public class TalentState
{
    public TalentData data;
    public int level;

    public bool IsMaxLevel => level >= data.maxLevel;
}
