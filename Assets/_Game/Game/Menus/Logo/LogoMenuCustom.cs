using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMenuCustom : LogoMenu {
	void Start() {
		Next();
		Close();
	}

	protected virtual void Next() {
		float volume = PlayerPrefs.GetFloat("MusicVolume", .4f);
		AudioManager.Instance.SetMusicVolume(volume);
		if (skipLogin) {
			MainMenu.TransitionIn();
		}
		else {
			Tools.CallStaticFunctionOfClass(LoginAssembly, LoginClass, LoginMethod);
		}
	}
}
