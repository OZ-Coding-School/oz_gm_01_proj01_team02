using UnityEngine;

namespace STH.Characters.Enemy
{
    public abstract class EnemyState
    {
        protected EnemyController enemy;

        protected EnemyState(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public abstract StateEnums StateType { get; }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void UpdateState() { }

        public virtual void FixedUpdateState() { }
    }
}
