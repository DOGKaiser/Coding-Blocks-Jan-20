using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NamesToAdd {
	None,
	UserNames,
}

public class NameGenerator {

	public static List<string> userNames;

	public static string[] lines;

	static System.Random ran;

	public static void Init() {
		if (userNames != null)
			return;

		ran = new System.Random();

		userNames = new List<string>();

		TextAsset nameText = Resources.Load<TextAsset>("RandomNames");

		lines = nameText.text.Split('\n');

		NamesToAdd addingUserNames = NamesToAdd.None;

		for (int i = 0; i < lines.Length; i++) {
			string toAdd = lines[i].Trim();

			if (toAdd == "Usernames:") {
				addingUserNames = NamesToAdd.UserNames;
				continue;
			}

			if (toAdd != "") {
				switch (addingUserNames) {
					case NamesToAdd.UserNames:
						userNames.Add(toAdd);
						break;
					default:
						break;
				}
			}
		}

	}

	public static string GenerateName() {
		string returnName = "";

		if (userNames == null) {
			Init();
        }

		returnName = userNames[ran.Next(0, userNames.Count)];

		return returnName;
	}
}
