using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMenuCustom : LogoMenu {
	void Start() {
		Next();
		Close();
	}

	protected virtual void Next() {
		if(skipLogin) {
			MainMenu.TransitionIn();
		}
		else {
			Tools.CallStaticFunctionOfClass(LoginAssembly, LoginClass, LoginMethod);
		}
	}
}
