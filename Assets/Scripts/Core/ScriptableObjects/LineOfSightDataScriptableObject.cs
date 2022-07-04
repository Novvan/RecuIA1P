using UnityEngine;

namespace RecuIA1P.Core.ScriptableObjects
{
    [CreateAssetMenu (fileName = "NewLineOfSightData", menuName = "ScriptableObjects/AI/LineOfSight", order = 1)]
    public class LineOfSightDataScriptableObject : ScriptableObject
    {
        [Header("Settings")]
        public LayerMask obstaclesMask;
        public LayerMask targetMask;

        [Space]
        [Range(0, 50f)] public float range = 10;
        [Range(0, 90f)] public float angle = 45f;
    }
}