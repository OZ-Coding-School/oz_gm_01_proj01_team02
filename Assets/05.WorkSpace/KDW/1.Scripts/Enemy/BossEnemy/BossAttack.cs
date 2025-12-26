using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : BossState
{
    public BossAttack(BossController boss) : base(boss) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Attack; }
    }
}
public class DashBossAttack : BossAttack
{
    public DashBossAttack(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        if (dist > boss.DashAttackRange || Time.time < boss.NextAttackTime)
        {
            boss.ChangeState(boss.ChaseState);
            //Debug.Log("근거리추적");
        }
    }

    public override void FixedUpdateState()
    {
        boss.DashAttack();
    }
}
public class DandelionBossAttack : BossAttack
{
    public DandelionBossAttack(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        if (dist > boss.RangedAttackRange)
        {
            boss.ChangeState(boss.ChaseState);
            Debug.Log("원거리추적");
        }

        boss.LookTo();

        boss.DandelionShotAttack();
    }
}
