using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Idle; }
    }

    public override void UpdateState()
    {
        if (enemy.Idle())
        {
            enemy.ChangeState(enemy.ChaseState);
        }
    }
}
