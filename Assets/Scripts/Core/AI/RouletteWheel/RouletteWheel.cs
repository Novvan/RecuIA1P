using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RecuIA1P.Core.AI.RouletteWheel
{
    public class RouletteWheel 
    {
        public static T Run <T>(Dictionary <T,float> items)
        {
            var max = items.Sum(item => item.Value);

            var random = Random.Range(0, max);

            foreach (var item in items)
            {
                random -= item.Value;
                
                if (random <= 0)
                    return item.Key;
            }
            
            return default;
        }
    }
}