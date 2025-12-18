using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdleState : EnemyState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Idle; }
    }
}
public class MeleeIdle : IdleState
{
    public MeleeIdle(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        if (dist > enemy.MeleeIdleRange && Time.time < enemy.NextAttackTime)
        {
            Debug.Log("대기 -> 추적");
            enemy.ChangeState(enemy.ChaseState);
            return;
        }
        
        if(Time.time >= enemy.NextAttackTime)
        {
            Debug.Log("대기 -> 공격");
            enemy.ChangeState(enemy.AttackState);
            return;
        }

        enemy.LookTo();

        enemy.Idle();

    }
}
