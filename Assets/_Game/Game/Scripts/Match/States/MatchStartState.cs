using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStartState : MatchState {

	public MatchStartState(MatchGame match) : base(match) {
	}

	public override void Enter() {
		base.Enter();

	}

	public override void UpdateState(float elapsedTime) {
		base.UpdateState(elapsedTime);

	}

	public override void OneTimeUpdate() {
		base.OneTimeUpdate();
	}

	public override void Exit() {
		base.Exit();
	}

	public void GoToIdle() {
		mMatch.PopState();
		mMatch.AddState(new MatchIdleState(mMatch));
	}
}
