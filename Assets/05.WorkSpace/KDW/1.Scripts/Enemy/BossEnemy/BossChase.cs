using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossChase : BossState
{
    public BossChase(BossController boss) : base(boss) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Chase; }
    }
    public override void FixedUpdateState()
    {
        //타겟이 존재하면
        if (boss.Target != null)
        {
            //타겟을 추적
            boss.Chase();
        }
    }
}
public class DashBossChase : BossChase
{
    public DashBossChase(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        //근거리 적의 공격 가능한 거리 안에 들어왔으면 공격 상태로 전환
        if (dist <= boss.DashAttackRange && Time.time >= boss.NextAttackTime)
        {
            boss.ChangeState(boss.AttackState);
            Debug.Log("근거리공격");
            return;
        }
        else if (dist <= boss.IdleRange && Time.time < boss.NextAttackTime && !boss.IsAttack)
        {
            Debug.Log("멈춰");
            Debug.Log(boss.IsAttack);
            boss.ChangeState(boss.IdleState);
            return;
        }
    }
}
public class DandelionBossChase : BossChase
{
    public DandelionBossChase(BossController boss) : base(boss) { }

    public override void UpdateState()
    {
        if (boss.Target == null) return;

        float dist = boss.DistToPlayer();

        //보스의 공격 가능한 거리 안에 들어왔으면 공격 상태로 전환
        if (dist <= boss.RangedAttackRange)
        {
            boss.ChangeState(boss.AttackState);
            Debug.Log("원거리공격");
            return;
        }
    }
}
