using System;
using UnityEngine;

public class WaitState : State {
	protected float mWaitTime;
	protected Action mActionAfterWait;

	public WaitState(float waitTime, Action actionAfterWait) {
		mWaitTime = waitTime;
		mActionAfterWait = actionAfterWait;

	}

	public override void UpdateState(float elapsedTime) {
		mWaitTime -= elapsedTime;

		if (mWaitTime <= 0) {
			mActionAfterWait();
		}
	}
}
