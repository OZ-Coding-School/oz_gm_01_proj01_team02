using UnityEngine;

public abstract class BossIdle : BossState
{
    public BossIdle(BossController boss) : base(boss) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Idle; }
    }
}
public class MidBossIdle : BossIdle
{
    public MidBossIdle(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        if (dist > boss.MidIdleRange && Time.time < boss.NextAttackTime)
        {
            Debug.Log("대기 -> 추적");
            boss.ChangeState(boss.ChaseState);
            return;
        }

        if (Time.time >= boss.NextAttackTime)
        {
            Debug.Log("대기 -> 공격");
            boss.ChangeState(boss.AttackState);
            return;
        }

        boss.LookTo();

        boss.Idle();
    }
}
