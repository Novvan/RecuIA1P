using System;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.States
{
    public class EnemyIdleState<T> : State<T>
    {
        private readonly INode _rootNode;
        private readonly Func<bool> _attemptSeePlayer;
        private readonly float _idleTime;

        private readonly Action _onIdleCommand;
        private readonly Action<bool> _setIdleCommand;
        private float _counter;

        public EnemyIdleState(INode rootNode, Action onIdleCommand, Action <bool> setIdleCommand, Func<bool> attemptSeePlayer, float idleTime)
        {
            _rootNode = rootNode;

            _onIdleCommand = onIdleCommand;
            _setIdleCommand = setIdleCommand;
            
            _attemptSeePlayer = attemptSeePlayer;
            
            _idleTime = idleTime;
        }
        
        public override void Awake()
        {
            ResetCounter();
            
            _onIdleCommand?.Invoke();
            _setIdleCommand?.Invoke(true);
        }
        
        public override void Execute()
        {
            _counter -= Time.deltaTime;
            var seePlayer = _attemptSeePlayer.Invoke();

            if (!(_counter <= 0) && !seePlayer) return;
            
            Debug.Log("Idle");
            _setIdleCommand?.Invoke(false);
            _rootNode.Execute();
                
            ResetCounter();
        }

        private void ResetCounter()
        {
            _counter = _idleTime;
        }
    }
}
