using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineQueue : StateMachine {
	List<IState> nextStates = new List<IState>();

	public void AddState(IState nextState) {
		nextStates.Add(nextState);
	}

	public void InsertState(int location, IState nextState) {
		nextStates.Insert(location, nextState);
	}

	public void NextState() {
		if (currentState != null) {
			nextStates.RemoveAt(0);
		}

		if (nextStates.Count > 0) {
			ChangeState(nextStates[0]);
		}
		else {
			ChangeState(null);
		}
	}

	public override void UpdateState(float elapsedTime) {
		base.UpdateState(elapsedTime);

		if (currentState == null && nextStates.Count > 0)
			NextState();
	}
}
