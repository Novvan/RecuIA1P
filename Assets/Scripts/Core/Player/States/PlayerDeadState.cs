using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Player.States
{
    public class PlayerDeadState<T> : State<T>
    {
        private readonly T _transitionToIdle;
        private readonly Action<Vector3> _onMoveCommand;
        private readonly float _respawnDelay;
        private float _counter;

        public PlayerDeadState(T transitionToIdle, Action<Vector3> onMoveCommand, float respawnDelay)
        {
            _transitionToIdle = transitionToIdle;
            _respawnDelay = respawnDelay;
            _onMoveCommand = onMoveCommand;
        }

        public override void Awake()
        {
            ResetTimer();  
            _onMoveCommand?.Invoke(Vector3.zero);
        }

        public override void Execute()
        {
            _counter -= Time.deltaTime;

            if (!(_counter <= 0)) return;
            
            ParentStateMachine.Transition(_transitionToIdle);
            ResetTimer();
        }

        private void ResetTimer() => _counter = _respawnDelay;
    }
}