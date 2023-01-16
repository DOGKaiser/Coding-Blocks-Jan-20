using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDataHolderMap")]
public class PlayerDataHolderMap : ScriptableObject {
    public int SaveNumber;
    public string HolderKey;
    public List<string> Holders;
}
