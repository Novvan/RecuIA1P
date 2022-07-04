using System.Collections.Generic;
using System.Linq;

namespace RecuIA1P.Core.AI.Finite_State_Machine
{
    public class State <T> : IState <T>
    {
        private readonly Dictionary<T, IState<T>> _stateTransitions = new Dictionary<T, IState<T>>();

        public StateMachine<T> ParentStateMachine { get; set; }

        public virtual void Awake() { }

        public virtual void Execute() { }

        public virtual void Sleep() { }

        public void AddTransition (T input, IState<T> transitionState)
        {
            if (!_stateTransitions.ContainsKey(input)) _stateTransitions[input] = transitionState;
        }

        public void RemoveTransition(T input)
        {
            if (_stateTransitions.ContainsKey(input)) _stateTransitions.Remove(input);
        }

        public void RemoveTransition (IState<T> state)
        {
            if (!_stateTransitions.ContainsValue(state)) return;
            
            foreach (var item in _stateTransitions.Where(item => item.Value == state))
                _stateTransitions.Remove(item.Key);
        }

        public IState <T> GetTransition (T input)
        {
            return _stateTransitions.ContainsKey(input) ? _stateTransitions[input] : null;
        }
    }
}