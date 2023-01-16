using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData {

	public string Username;
	public string Password;
	public string DisplayName;
	DataHolderClass PlayerDataHolderClass;
	// protected Dictionary<string, PlayerDataHolder> DataHolders;

	protected Dictionary<string, int> mStatisticValues;
	protected Dictionary<string, int> mVirtualCurrencies = new Dictionary<string, int>();

	protected Dictionary<string, List<PlayerDataItem>> mInventory = new Dictionary<string, List<PlayerDataItem>>();

	public virtual void Init() {
		Username = "";
		Password = "";
		DisplayName = "";
		PlayerDataHolderClass = new DataHolderClass();
		mStatisticValues = new Dictionary<string, int>();

		// Load all DataHolders and Init
		PlayerDataHolderClass.InitAll(this);
	}

	// ------------------------------------------------------------

	public void AddItem(PlayerDataItem item) {
		List <PlayerDataItem> foundItem = GetItemID(item.ItemID);
		if (foundItem == null) {
			List<PlayerDataItem> playerDataItems = new List<PlayerDataItem>();
			playerDataItems.Add(item);
			mInventory.Add(item.ItemID, playerDataItems);
		}
		else {
			foundItem.Add(item);
		}
	}

	public List<PlayerDataItem> GetItemID(string id) {
		if (mInventory.ContainsKey(id)) {
			return mInventory[id];
		}
		
		return null;
	}

	public int GetInventoryCount(string id) {
		int count = 0;

		List<PlayerDataItem> foundItem = GetItemID(id);
		if (foundItem != null) {
			foreach (PlayerDataItem playerDataItem in foundItem) {
				count += playerDataItem.Uses != null ? playerDataItem.Uses.Value : 1;
			}
		}

		return count;
	}

	public void UseInventoryItem(string id, string itemInstanceId) {
		List<PlayerDataItem> foundItem = GetItemID(id);
		if (foundItem != null) {
			for (int i = 0; i < foundItem.Count; i++) {
				PlayerDataItem playerDataItem = foundItem[i];
				if (playerDataItem.ItemInstanceID == itemInstanceId) {
					if (playerDataItem.Uses == null || playerDataItem.Uses.Value == 1) {
						foundItem.RemoveAt(i);
					}
					else {
						playerDataItem.Uses--;
					}
					return;
				}
			}
		}
	}

	// ------------------------------------------------------------

	public int GetVirtualCurrency(string vc) {
		if (mVirtualCurrencies.ContainsKey(vc)) {
			return mVirtualCurrencies[vc];
		}
		return int.MinValue;
	}

	public void SetVirtualCurrency(string vc, int value) {
		if (mVirtualCurrencies.ContainsKey(vc)) {
			mVirtualCurrencies[vc] = value;
		}
	}

	public Dictionary<string, int> GetVirtualCurrencies() {
		return mVirtualCurrencies;
	}

	public void SetVirtualCurrencies(Dictionary<string, int> vcs) {
		mVirtualCurrencies = vcs;
	}

	public bool HasEnoughVirtualCurrency(string vc, int amount) {
		int value = GetVirtualCurrency(vc);
		return amount <= value;
	}

	public void AdjustVirtualCurrencyAmount(string vc, int amount) {
		int value = GetVirtualCurrency(vc);
		if (value == int.MinValue) {
			return;
		}
	   mVirtualCurrencies[vc] += amount;
	}

	// ------------------------------------------------------------

	public int GetStatisticValue(string statistic) {
		if (mStatisticValues.ContainsKey(statistic)) {
			return mStatisticValues[statistic];
		}
		return -1;
	}

	public void SetStatisticValue(string statistic, int value) {
		if (mStatisticValues.ContainsKey(statistic)) {
			mStatisticValues[statistic] = value;
		}
	}

	public Dictionary<string, int> GetStatisticValues() {
		return mStatisticValues;
	}

	public void SetStatisticValues(Dictionary<string, int> statisticValues) {
		mStatisticValues = statisticValues;
	}

	// ------------------------------------------------------------

	public DataHolderClass GetDataHolderClass() {
		return PlayerDataHolderClass;
	}

	public Dictionary<string, PlayerDataHolder> GetDataHolders() {
		return PlayerDataHolderClass.GetDataHolders();
	}

	public PlayerDataHolder GetDataHolder(string holder) {
		return GetDataHolders().ContainsKey(holder) ? GetDataHolders()[holder] : null;
	}

	public void AddDataHolder(string holder, PlayerDataHolder data) {
		GetDataHolders().Add(holder, data);
	}

	// For Login
	public virtual void LoadLoginFromBuffer(BufferReadWrite buffer) {
		Username = buffer.ReadString();
		Password = buffer.ReadString();
	}

	public virtual void SaveLoginToBuffer(BufferReadWrite buffer) {
		 buffer.WriteString(Username);
		 buffer.WriteString(Password);
	}

	// For saving and loading players
	public virtual void LoadFromBuffer(BufferReadWrite buffer) {
		DisplayName = buffer.ReadString();
	}

	public virtual void LoadHoldersFromBuffer(BufferReadWrite buffer, string type) {
		PlayerDataHolderClass.LoadHoldersFromBuffer(buffer, type);
	}


	public virtual void SaveToBuffer(BufferReadWrite buffer) {
		buffer.WriteString(DisplayName);
	}

	public virtual void SaveHoldersToBuffer(BufferReadWrite buffer, string holderKey) {
		PlayerDataHolderClass.SaveHoldersToBuffer(buffer, holderKey);
	}
}

public class PlayerDataItem {
	public string ItemID;
	public string ItemInstanceID;
	public int? Uses;
	public Dictionary<string, string> CustomData;
}