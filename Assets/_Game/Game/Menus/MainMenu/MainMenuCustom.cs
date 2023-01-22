using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MainMenuCustom : MainMenu {

    
	public static void TransitionInFromMatch() {
		Destroy(MatchMenuCustom.Instance.MatchArea);
		MatchMenuCustom.CurrentMatch = null;
		
		MenuManager.Instance.FadeIn();
		Open();
		LoseMenu.Close();
		MatchMenuCustom.Close();
	}
    
}
