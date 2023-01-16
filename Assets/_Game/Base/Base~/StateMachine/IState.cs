
public interface IState  {
	void Enter();
	void UpdateState(float elapsedTime);
	void OneTimeUpdate();
	void Exit();
}
