using UnityEngine;

public abstract class ChaseState : EnemyState
{
    public ChaseState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Chase; }
    }

    public override void UpdateState()
    {
        
    }
    public override void FixedUpdateState()
    {
        //타겟이 존재하면
        if (enemy.Target != null)
        {
            //타겟을 추적
            enemy.Chase();
        }
    }
}
public class MeleeChase : ChaseState
{
    public MeleeChase(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        //근거리 적의 공격 가능한 거리 안에 들어왔으면 공격 상태로 전환
        if (dist <= enemy.MeleeAttackRange)
        {
            enemy.ChangeState(enemy.AttackState);
            Debug.Log("근거리공격");
            return;
        }
    }
}
public class RangedChase : ChaseState
{
    public RangedChase(EnemyController enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        //원거리 적의 공격 가능한 거리 안에 들어왔으면 공격 상태로 전환
        if (dist <= enemy.RangedAttackRange)
        {
            enemy.ChangeState(enemy.AttackState);
            Debug.Log("원거리공격");
            return;
        }
    }
}

