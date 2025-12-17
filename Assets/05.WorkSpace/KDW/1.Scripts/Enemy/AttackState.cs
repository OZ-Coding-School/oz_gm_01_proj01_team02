using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : EnemyState
{
    public AttackState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Attack; }
    }
}
//근거리 적 공격 상태
public class MeleeAttack : AttackState
{
    public MeleeAttack(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        if (dist > enemy.MeleeAttackRange || Time.time < enemy.NextAttackTime)
        {
            enemy.ChangeState(enemy.ChaseState);
            Debug.Log("근거리추적");
        }
    }

    public override void FixedUpdateState()
    {
        enemy.MeleeAttack();
    }
}
//원거리 적 공격 상태
public class RangedAttack : AttackState
{
    public RangedAttack(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        if (dist > enemy.RangedAttackRange)
        {
            enemy.ChangeState(enemy.ChaseState);
            Debug.Log("원거리추적");
        }

        enemy.LookTo();

        enemy.RangedAttack();
    }
}
