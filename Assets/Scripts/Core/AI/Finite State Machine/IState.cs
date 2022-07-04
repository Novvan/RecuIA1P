namespace RecuIA1P.Core.AI.Finite_State_Machine
{
	public interface IState<T>
	{
		void Awake();

		void Execute();

		void Sleep();

		void AddTransition(T input, IState<T> transitionState);

		void RemoveTransition(T input);

		void RemoveTransition(IState<T> state);

		IState<T> GetTransition(T input);

		StateMachine<T> ParentStateMachine { get; set; }
	}
}
