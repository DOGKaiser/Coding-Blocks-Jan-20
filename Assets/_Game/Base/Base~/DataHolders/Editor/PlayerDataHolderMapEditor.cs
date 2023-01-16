using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerDataHolderMap))]
public class PlayerDataHolderMapEditor : Editor {

	PlayerDataHolderMap holderDataIn;

	private void OnEnable() {
		holderDataIn = (PlayerDataHolderMap)target;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		List<PlayerDataHolder> playerDataHolders = (List<PlayerDataHolder>)Tools.GetEnumerableOfType<PlayerDataHolder>();

		List<string> templateNames = new List<string>();
		templateNames.Add("");
		int curSelected = 0;

		for (int i = 0; i < playerDataHolders.Count; i++) {
			if (!holderDataIn.Holders.Contains(playerDataHolders[i].GetHolderString())) {
				templateNames.Add(playerDataHolders[i].GetHolderString());
			}
		}

		curSelected = EditorGUILayout.Popup("Add Holder", curSelected, templateNames.ToArray());

		if (curSelected != 0) {
			holderDataIn.Holders.Add(templateNames[curSelected]);
		}

		EditorUtility.SetDirty(holderDataIn);
	}


}
