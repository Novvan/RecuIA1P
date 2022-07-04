using UnityEngine;

namespace RecuIA1P.Core.AI.Steering_Behaviours
{
    public interface ISteering 
    {
        Vector3 GetDir();
        
        Transform SetTarget { set; }
    }
}