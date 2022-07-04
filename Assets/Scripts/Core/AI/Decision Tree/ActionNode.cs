
using System;

namespace RecuIA1P.Core.AI.Decision_Tree
{
    public class ActionNode : INode
    {
        private readonly Action _actionToExecute;
        
        public ActionNode(Action action) => _actionToExecute = action;

        public void Execute() => _actionToExecute();
    }
}