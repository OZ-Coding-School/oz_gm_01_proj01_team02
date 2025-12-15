using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController enemy) : base(enemy) { }

    public override StateEnums StateType
    {
        get { return StateEnums.Chase; }
    }

    public override void UpdateState()
    {
        if (enemy.Target == null) return;

        float dist = enemy.DistToPlayer();

        //공격 가능한 거리 안에 들어왔으면 공격 상태로 전환
        if (dist <= enemy.AttackRange && enemy.IsAttack)
        {
            enemy.ChangeState(enemy.AttackState);
            Debug.Log("공격");
            return;
        }
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
