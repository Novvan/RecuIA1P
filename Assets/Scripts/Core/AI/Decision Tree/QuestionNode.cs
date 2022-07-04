using System;

namespace RecuIA1P.Core.AI.Decision_Tree
{
    public class QuestionNode : INode
    {
        private readonly Func<bool> _question;
        
        private readonly INode _trueNode;
        private readonly INode _falseNode;

        public QuestionNode (Func <bool> question, INode trueNode, INode falseNode)
        {
            _question = question;
            
            _trueNode  = trueNode;
            _falseNode = falseNode;
        }

        public void Execute()
        {
            if (_question())
                _trueNode.Execute();
            
            else
                _falseNode.Execute();
        }
    }
}