
public class StateMachine {

	protected IState currentState;

	public IState State {
		get { return currentState; }
	}

	public void ChangeState(IState nextState) {
		if (currentState != null)
			currentState.Exit();

		currentState = nextState;

		if (nextState != null) {
			currentState.Enter();
		}
	}

	public virtual void UpdateState(float elapsedTime) {
		if (currentState != null) 
			currentState.UpdateState(elapsedTime);
	}
}
