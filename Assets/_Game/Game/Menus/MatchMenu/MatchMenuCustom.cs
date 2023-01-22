using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MatchMenuCustom : MatchMenu {

    [Header("Match Menu")]
    [SerializeField] private GameObject PlayerPrefab;
    [HideInInspector] public GameObject Player;
    
    [SerializeField] private Image GripMeter;

    public AudioClip MayhemMusic;
    
    public override void StartMatchFromServer(int seed) {

        base.StartMatchFromServer(seed);
    }
    
    protected override void CreateMatchArea() {
        base.CreateMatchArea();

        ((MatchGameCustom)CurrentMatch).fisher = MatchArea.transform.Find("Fisher").GetComponent<Fisher>();
        Player = ObjectPoolMgr.Instance.GetObject(PlayerPrefab, MatchArea.transform);
        Player.transform.parent.SetParent(MatchArea.transform);
        SetGrip(100);
    }

    public void SetGrip(float grip) {
        GripMeter.rectTransform.localScale = new Vector3(1,(grip/100),1);
    }

    public void OpenSettings() {
        SettingMenu.Show();
    }
}
