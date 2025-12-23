using UnityEngine;

public abstract class BossState 
{
    protected BossController boss;

    protected BossState(BossController boss)
    {
        this.boss = boss;
    }

    public abstract StateEnums StateType { get; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdateState() { }

}
