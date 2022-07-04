namespace RecuIA1P.Core.AI.Finite_State_Machine
{
    public class StateMachine <T>
    {
        private IState<T> Current { get; set; }

        public StateMachine (IState <T> initialState) => SetInitialState(initialState);

        private void SetInitialState(IState <T> initialState)
        {
            Current = initialState;
            Current.Awake();
            Current.ParentStateMachine = this;
        }
        
        public void UpdateState()
        {
            Current?.Execute();
        }

        public void Transition (T input)
        {
            var newState = Current.GetTransition(input);

            if (newState == null) return;
            
            Current.Sleep();
            Current = newState;
            Current.ParentStateMachine = this;
            Current.Awake();
        }
    }
}