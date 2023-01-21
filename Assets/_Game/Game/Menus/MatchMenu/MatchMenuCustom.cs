using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchMenuCustom : MatchMenu {

    [Header("Match Menu")]
    [SerializeField] private GameObject PlayerPrefab;
    [HideInInspector] public GameObject Player;
    
    [SerializeField] private TextMeshProUGUI GripMeter;

    public override void StartMatchFromServer(int seed) {

        base.StartMatchFromServer(seed);
    }
    
    protected override void CreateMatchArea() {
        base.CreateMatchArea();

        ((MatchGameCustom)CurrentMatch).fisher = MatchArea.transform.Find("Fisher").GetComponent<Fisher>();
        Player = ObjectPoolMgr.Instance.GetObject(PlayerPrefab, MatchArea.transform);
        SetGrip(0);
    }

    public void SetGrip(float grip) {
        GripMeter.text = ((int)grip).ToString();
    }
}
