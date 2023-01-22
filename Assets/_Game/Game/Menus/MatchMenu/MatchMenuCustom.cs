using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MatchMenuCustom : MatchMenu {

    [Header("Match Menu")]
    [SerializeField] private GameObject PlayerPrefab;
    [HideInInspector] public GameObject Player;
    
    [SerializeField] private Image GripMeter;

    public override void StartMatchFromServer(int seed) {

        base.StartMatchFromServer(seed);
    }
    
    protected override void CreateMatchArea() {
        base.CreateMatchArea();

        ((MatchGameCustom)CurrentMatch).fisher = MatchArea.transform.Find("Fisher").GetComponent<Fisher>();
        Player = ObjectPoolMgr.Instance.GetObject(PlayerPrefab, MatchArea.transform);
        SetGrip(100);
    }

    public void SetGrip(float grip) {
        Debug.Log(grip+" "+ ((int)grip));
        GripMeter.rectTransform.localScale = new Vector3(1,(grip/100),1);
    }
}
