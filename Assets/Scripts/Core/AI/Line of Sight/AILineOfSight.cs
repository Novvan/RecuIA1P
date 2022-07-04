using RecuIA1P.Core.ScriptableObjects;
using UnityEngine;

namespace RecuIA1P.Core.AI.Line_of_Sight
{
    [AddComponentMenu("AI/AI Line Of Sight")]
    public class AILineOfSight : MonoBehaviour
    {   
        [Header("Data Bindings")]
        [SerializeField] private LineOfSightDataScriptableObject lineOfSightData;
        
        [Header("Component Bindings")]
        [SerializeField] private Transform lineOfSightOrigin;
        
        public bool SingleTargetInSight(Transform target)
        {
            var difference = target.position - lineOfSightOrigin.position;
            var distance = difference.magnitude;
            
            if (distance > lineOfSightData.range) return false;

            var front = lineOfSightOrigin.forward;

            return IsInVisionAngle(difference, front) && IsInView(difference.normalized, distance, lineOfSightData.obstaclesMask);
        }

        private bool IsInVisionAngle(Vector3 origin, Vector3 target)
        {       
            var angleToTarget = Vector3.Angle(origin, target);
            
            return angleToTarget < (lineOfSightData.angle / 2);
        }
        
        private bool IsInView(Vector3 normalizedDirection, float distance, LayerMask obstacleMask)
        {
            return !Physics.Raycast(lineOfSightOrigin.position, normalizedDirection, distance, obstacleMask);      
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var currentForward  = transform.forward;
            var currentPosition = lineOfSightOrigin.position;
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawRay(currentPosition, Quaternion.Euler(0, lineOfSightData.angle, 0)  * currentForward * lineOfSightData.range);
            Gizmos.DrawRay(currentPosition, Quaternion.Euler(0, -lineOfSightData.angle, 0) * currentForward * lineOfSightData.range);
            
            Gizmos.DrawWireSphere(currentPosition, lineOfSightData.range);
        }
#endif
    }
}