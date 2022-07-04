using System;
using RecuIA1P.Core.AI.Finite_State_Machine;

namespace RecuIA1P.Core.Enemy.States
{
    public class EnemyDeadState<T> : State<T>
    {
        private readonly Action _onDead;
        
        public EnemyDeadState(Action onDead)
        {
            _onDead = onDead;
        }

        public override void Awake()
        {
            _onDead?.Invoke();
        }
    }
}