using Unity.VisualScripting;
using UnityEngine;
using STH.Characters.Enemy;

namespace STH.Characters.Enemy
{
    public abstract class ChaseState : EnemyState
    {
        public ChaseState(EnemyController enemy) : base(enemy) { }

        public override StateEnums StateType
        {
            get { return StateEnums.Chase; }
        }
        public override void FixedUpdateState()
        {
            //Ÿ���� �����ϸ�
            if (enemy.Target != null)
            {
                //Ÿ���� ����
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

            //�ٰŸ� ���� ���� ������ �Ÿ� �ȿ� �������� ���� ���·� ��ȯ
            if (dist <= enemy.MeleeAttackRange && Time.time >= enemy.NextAttackTime)
            {
                enemy.ChangeState(enemy.AttackState);
                Debug.Log("�ٰŸ�����");
                return;
            }
            else if (dist <= enemy.MeleeIdleRange && Time.time < enemy.NextAttackTime && !enemy.IsAttack)
            {
                Debug.Log("����");
                Debug.Log(enemy.IsAttack);
                enemy.ChangeState(enemy.IdleState);
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

            //���Ÿ� ���� ���� ������ �Ÿ� �ȿ� �������� ���� ���·� ��ȯ
            if (dist <= enemy.RangedAttackRange)
            {
                enemy.ChangeState(enemy.AttackState);
                Debug.Log("���Ÿ�����");
                return;
            }
        }
    }
}
