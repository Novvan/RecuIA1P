using System.Collections.Generic;

namespace RecuIA1P.Core.AI.Decision_Tree
{
    public class RandomNode : INode
    {
        private readonly RouletteWheel.RouletteWheel _roulette;
        private readonly Dictionary<INode, float> _items;
        
        public RandomNode(Dictionary <INode,float> items)
        {
            _roulette = new RouletteWheel.RouletteWheel();
            _items = items;
        }

        public RandomNode (RouletteWheel.RouletteWheel roulette, Dictionary<INode, float> items)
        {
            _roulette = roulette;
            _items = items;
        }
        public void Execute()
        {
            var node = RouletteWheel.RouletteWheel.Run(_items);
            node.Execute();
        }
    }
}