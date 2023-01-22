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

	public void OpenHowTo()
	{
		HowToMenu.Show();
	}

	public Button Start, HowTo, Settings;

	public void DisableButtons()
	{
		//stop the player from clicking while loading
		Start.enabled = false;
		HowTo.enabled = false;
		Settings.enabled = false;
	}

	public void EnterMatchMakingCustom(MatchSetting matchSetting)
	{
		EnterMatchMaking(matchSetting);
		DisableButtons();
	}

	}
