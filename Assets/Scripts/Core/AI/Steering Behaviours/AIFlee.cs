using UnityEngine;

namespace RecuIA1P.Core.AI.Steering_Behaviours
{
    public class AIFlee : ISteering
    {
        private Transform _self;
        private Transform _target;

        public AIFlee(Transform self, Transform target)
        {
            _self = self;
            _target = target;
        }
        public Transform SetSelf
        {
            set => _self = value;
        }

        public Transform SetTarget
        {
            set => _target = value;
        }
        
        public Vector3 GetDir() => (_self.position - _target.position).normalized;
    }
}