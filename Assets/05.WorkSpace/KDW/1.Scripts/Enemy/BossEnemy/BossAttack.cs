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
public class MidBossAttack : BossAttack
{
    public MidBossAttack(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        if (dist > boss.MidAttackRange || Time.time < boss.NextAttackTime)
        {
            boss.ChangeState(boss.ChaseState);
            //Debug.Log("근거리추적");
        }
    }

    public override void FixedUpdateState()
    {
        boss.MidAttack();
    }
}
