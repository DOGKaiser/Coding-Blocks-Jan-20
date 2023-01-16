using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolderGameData {
	public static DataHolderGameData Instance = new DataHolderGameData();

	// Client
	List<PlayerDataHolder> mPlayerDataHolders;
	List<PlayerDataHolderMap> mPlayerDataHolderMaps;

	public void LoadDataHolderGameData() {
		// Player Data Holder
		mPlayerDataHolders = (List<PlayerDataHolder>)Tools.GetEnumerableOfType<PlayerDataHolder>();
		UnityTools.DataLogs<PlayerDataHolder>(mPlayerDataHolders.ToArray(), "PlayerHolders");

		// Player Data Holder Maps
		PlayerDataHolderMap[] playerDataHolderMaps = Resources.LoadAll<PlayerDataHolderMap>("");
		UnityTools.DataLogs(playerDataHolderMaps, "PlayerDataHolderMaps");
		mPlayerDataHolderMaps = new List<PlayerDataHolderMap>(playerDataHolderMaps);
	}

	// --------------------------------------------------------


	public List<PlayerDataHolder> getPlayerDataHolders() {
		return mPlayerDataHolders;
	}

	public List<PlayerDataHolderMap> getPlayerDataHolderMaps() {
		return mPlayerDataHolderMaps;
	}

	public void getPlayerDataHolderMapLoc(string holderMapName, out int mapLoc) {
		mapLoc = -1;

		for (int i = 0; i < mPlayerDataHolderMaps.Count; i++) {
			if (mPlayerDataHolderMaps[i].HolderKey.Equals(holderMapName)) {
				mapLoc = i;
				return;
			}
		}
	}

	public void getPlayerDataHolderLocation(string holderName, out int mapLoc, out int holderLoc) {
		mapLoc = -1;
		holderLoc = -1;

		for (int i = 0; i < mPlayerDataHolderMaps.Count; i++) {
			for (int j = 0; j < mPlayerDataHolderMaps[i].Holders.Count; j++) {
				if (mPlayerDataHolderMaps[i].Holders[j].Equals(holderName)) {
					mapLoc = i;
					holderLoc = j;
					return;
				}
			}
		}
	}
}