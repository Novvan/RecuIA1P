using RecuIA1P.Core.Interfaces;
using UnityEngine;

namespace RecuIA1P.Core.AI.Steering_Behaviours
{
    public class AIEvasion : ISteering
    {
        private Transform _self;
        private Transform _target;
        
        private readonly IVel _vel;
        private readonly float _timePrediction;
        public AIEvasion(Transform self, Transform target, IVel vel, float timePrediction)
        {
            _self = self;
            _target = target;
            
            _vel = vel;
            _timePrediction = timePrediction;
        }
        
        public Transform SetSelf
        {
            set => _self = value;
        }

        public Transform SetTarget
        {
            set => _target = value;
        }

        public Vector3 GetDir()
        {
            var directionMultiplier = (_vel.Vel * _timePrediction);
            var distance = Vector3.Distance(_target.position, _self.position);

            if (directionMultiplier >= distance) directionMultiplier = distance / 2;
            
            var finalPosition = _target.position + _target.forward * directionMultiplier;
            var direction = (_self.position - finalPosition).normalized;
            
            return direction;
        }
    }
}