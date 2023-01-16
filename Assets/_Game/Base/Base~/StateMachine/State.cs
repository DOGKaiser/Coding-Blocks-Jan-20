using UnityEngine;
using System.Reflection;

public class State : IState {

//	float timeInState;

	public virtual void Enter() {
//		Debug.LogFormat("{0}::{1}", this.GetType(), MethodBase.GetCurrentMethod().Name);
//		timeInState = 0;
	}

	public virtual void UpdateState(float elapsedTime) {
//		timeInState += elapsedTime;
	}

	public virtual void OneTimeUpdate() {
		//		timeInState += elapsedTime;
	}

	public virtual void Exit() {
//		Debug.LogFormat("{0}::{1}::{2}", this.GetType(), MethodBase.GetCurrentMethod().Name, timeInState);
	}

}