using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class WinMenu : Menu<WinMenu> {


	public void GoBackToMainMenu() {
		MenuManager.Instance.FadeAnimator.AddOnFinishFunction(GoToMainMenu);
		MenuManager.Instance.FadeAnimator.AddOnFinishFunction(Close);
		MenuManager.Instance.FadeOut();
	}

	private void GoToMainMenu() {
		MainMenuCustom.TransitionInFromMatch();
	}
	
	public static void TransitionIn() {
		Open();
	}
}
