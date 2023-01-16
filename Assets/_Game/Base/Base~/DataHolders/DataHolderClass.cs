using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolderClass {

	// This value is null for AI
	protected PlayerData mPlayerData;

	protected Dictionary<string, PlayerDataHolder> DataHolders = new Dictionary<string, PlayerDataHolder>();

	public PlayerData GetPlayerData() { return mPlayerData; }

	public void InitAll(PlayerData pData) {
		// Load all DataHolders and Init
		mPlayerData = pData;
		DataHolders = new Dictionary<string, PlayerDataHolder>();
		List<PlayerDataHolder> pDatas = DataHolderGameData.Instance.getPlayerDataHolders();

		for (int i = 0; i < pDatas.Count; i++) {
			PlayerDataHolder newHolder = (PlayerDataHolder)Tools.GetInstance(pDatas[i].ToString());
			newHolder.InitData(this);
		}
	}

	// Load a specific data holder and Init
	public void InitHolderMap(PlayerData pData, string map) {
		mPlayerData = pData;
		int holderMapIndex;
		DataHolderGameData.Instance.getPlayerDataHolderMapLoc(map, out holderMapIndex);
		PlayerDataHolderMap holderMap = DataHolderGameData.Instance.getPlayerDataHolderMaps()[holderMapIndex];

		List<PlayerDataHolder> pDatas = DataHolderGameData.Instance.getPlayerDataHolders();

		for (int i = 0; i < pDatas.Count; i++) {
			if (holderMap.Holders.Contains(pDatas[i].GetHolderString())) {
				PlayerDataHolder newHolder = (PlayerDataHolder)Tools.GetInstance(pDatas[i].ToString());
				newHolder.InitData(this);
			}
		}
	}

	public void InitAIHolderMap(List<DataHolderClass> currentPlayers, PlayerData pData, string map, int AIseed) {
		mPlayerData = pData;
		int holderMapIndex;
		DataHolderGameData.Instance.getPlayerDataHolderMapLoc(map, out holderMapIndex);
		PlayerDataHolderMap holderMap = DataHolderGameData.Instance.getPlayerDataHolderMaps()[holderMapIndex];

		List<PlayerDataHolder> pDatas = DataHolderGameData.Instance.getPlayerDataHolders();
		UnityEngine.Random.InitState(AIseed);

		for (int i = 0; i < pDatas.Count; i++) {
			if (holderMap.Holders.Contains(pDatas[i].GetHolderString())) {
				PlayerDataHolder newHolder = (PlayerDataHolder)Tools.GetInstance(pDatas[i].ToString());
				newHolder.InitAIData(currentPlayers, this);
			}
		}
	}

	public Dictionary<string, PlayerDataHolder> GetDataHolders() {
		return DataHolders;
	}

	public PlayerDataHolder GetDataHolder(string holder) {
		return DataHolders.ContainsKey(holder) ? DataHolders[holder] : null;
	}

	public void AddDataHolder(string holder, PlayerDataHolder data) {
		DataHolders.Add(holder, data);
	}

	// For saving and loading players
	/*
	public void LoadFromBuffer(BufferReadWrite buffer) {
		int holderCount = buffer.ReadInteger();

		for (int i = 0; i < holderCount; i++) {
			string name = buffer.ReadString();
			Debug.LogWarning("Attemp to load holders, name: " + name);
			DataHolders[name].LoadFromBuffer(this, buffer, "PlayerBasic");
		}
	}
	*/

	public void LoadHoldersFromBuffer(BufferReadWrite buffer, string map) {
		int holderMapIndex;
		DataHolderGameData.Instance.getPlayerDataHolderMapLoc(map, out holderMapIndex);
		PlayerDataHolderMap holderMap = DataHolderGameData.Instance.getPlayerDataHolderMaps()[holderMapIndex];

		// Debug.LogWarning("Attempt to load holders, count: " + holderMap.Holders.Count + ", Type: " + map);
		for (int i = 0; i < holderMap.Holders.Count; i++) {
			// Debug.LogWarning("Attempt to load holders, name: " + holderMap.Holders[i].ToString());
			DataHolders[holderMap.Holders[i]].LoadFromBuffer(this, buffer, map);
			if (buffer.BufferLeft() == 0) {
				return;
			}
		}
	}


	public virtual void SaveToBuffer(BufferReadWrite buffer) {

		/*
		buffer.WriteInteger(DataHolders.Values.Count);

		foreach (PlayerDataHolder pData in DataHolders.Values) {
			pData.SaveToBuffer(this, buffer, "PlayerBasic");
		}
		*/
	}

	public virtual void SaveHoldersToBuffer(BufferReadWrite buffer, string holderKey) {
		int holderMapIndex;
		DataHolderGameData.Instance.getPlayerDataHolderMapLoc(holderKey, out holderMapIndex);
		PlayerDataHolderMap holderMap = DataHolderGameData.Instance.getPlayerDataHolderMaps()[holderMapIndex];

		Debug.LogWarning("Attempt to save holders, count: " + holderMap.Holders.Count + ", Type: " + holderMap.name);
		for (int i = 0; i < holderMap.Holders.Count; i++) {
			Debug.LogWarningFormat("Save holder: {0}", holderMap.Holders[i]);
			GetDataHolder(holderMap.Holders[i]).SaveToBuffer(this, buffer, holderKey);
		}
	}
}
