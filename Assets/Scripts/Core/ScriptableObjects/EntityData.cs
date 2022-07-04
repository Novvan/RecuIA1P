using UnityEngine;

namespace RecuIA1P.Core.ScriptableObjects
{
    [CreateAssetMenu (fileName = "NewEntityData", menuName = "ScriptableObjects/Entities/Data", order = 1)]
    public class EntityData : ScriptableObject
    {
        [Header("Movement Settings")] 
        public float walkSpeed = 8f;
        public float runSpeed = 10f;
        public float rotationSmoothTime = 0.12f;

        [Header ("Jump Settings")]
        public LayerMask groundMask;
        public float jumpHeight = 5f;
        public float jumpCooldown = 0.5f;
        public float isGroundedRadius = 0.15f;

        [Header("Others")]
        public float maxHealth = 100;
        public float maxLives = 1f;
        public float respawnDelay = 3f;
    }
}