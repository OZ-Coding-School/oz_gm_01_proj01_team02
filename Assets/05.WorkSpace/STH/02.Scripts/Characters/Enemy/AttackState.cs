using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STH.Characters.Enemy
{
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

            float dist = enemy.DistToPlayer();

            if (dist > enemy.MeleeAttackRange || Time.time < enemy.NextAttackTime)
            {
                enemy.ChangeState(enemy.ChaseState);
                //Debug.Log("�ٰŸ�����");
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

            float dist = enemy.DistToPlayer();

            if (dist > enemy.RangedAttackRange)
            {
                enemy.ChangeState(enemy.ChaseState);
                Debug.Log("���Ÿ�����");
            }

            enemy.LookTo();

            enemy.RangedAttack();
        }
    }
}