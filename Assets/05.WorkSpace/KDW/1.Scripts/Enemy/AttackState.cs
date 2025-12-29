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
//�ٰŸ� �� ���� ����
public class MeleeAttack : AttackState
{
    public MeleeAttack(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        if (enemy.Target.IsDead)
        {
            Debug.Log("Target is dead, losing target");
            enemy.TargetLost();
            return;
        }

        float dist = enemy.DistToPlayer();
        if (dist > enemy.MeleeAttackRange || Time.time < enemy.NextAttackTime)
        {
            enemy.ChangeState(enemy.ChaseState);
            return;
        }
    }

    public override void FixedUpdateState()
    {
        enemy.MeleeAttack();
    }
}
//���Ÿ� �� ���� ����
public class RangedAttack : AttackState
{
    public RangedAttack(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        if (enemy.Target.IsDead)
        {
            Debug.Log("Target is dead, losing target");
            enemy.TargetLost();
            return;
        }

        float dist = enemy.DistToPlayer();
        if (dist > enemy.RangedAttackRange)
        {
            enemy.ChangeState(enemy.ChaseState);
            return;
        }

        enemy.LookTo();

        enemy.RangedAttack();
    }
}