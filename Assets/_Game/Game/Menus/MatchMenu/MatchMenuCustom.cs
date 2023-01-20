using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMenuCustom : MatchMenu {

    [Header("Match Menu")]
    [SerializeField] private GameObject PlayerPrefab;
    [HideInInspector] public GameObject Player;

    public override void StartMatchFromServer(int seed) {

        base.StartMatchFromServer(seed);
    }


    protected override void CreateMatchArea() {
        base.CreateMatchArea();

        Player = ObjectPoolMgr.Instance.GetObject(PlayerPrefab, MatchArea.transform);
    }
}
