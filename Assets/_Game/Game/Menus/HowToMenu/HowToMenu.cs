using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class HowToMenu : Menu<HowToMenu>
{
	public static void Show()
	{
		Open();
	}

	public void Hide()
	{
		Close();
	}
}
