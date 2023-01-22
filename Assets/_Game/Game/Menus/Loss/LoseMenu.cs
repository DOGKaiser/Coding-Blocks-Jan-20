using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LoseMenu : Menu<LoseMenu> {


	public void GoBackToMainMenu() {
		MenuManager.Instance.FadeAnimator.AddOnFinishFunction(GoToMainMenu);
		MenuManager.Instance.FadeOut();
	}

	private void GoToMainMenu() {
		MainMenuCustom.TransitionInFromMatch();
	}
	
	public static void TransitionIn() {
		Open();
	}
}
