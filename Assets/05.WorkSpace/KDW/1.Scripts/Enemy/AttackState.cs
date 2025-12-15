using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Attack; }
    }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        //공격 가능 거리 보다 멀어지면
        if (dist > enemy.AttackRange || !enemy.IsAttack)
        {
            enemy.ChangeState(enemy.ChaseState);
            Debug.Log("추적");
        }
    }
    public override void FixedUpdateState()
    {
        enemy.Attack();
    }
}
