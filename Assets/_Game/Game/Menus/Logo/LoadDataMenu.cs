using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LoadDataMenu :  Menu<LoadDataMenu> {

    void Start() {
        GameDataMgr.Instance.Init();
        PlayerData.CreateLocalPlayerData();

        GoToMainMenu();
    }
    
    public static void TransitionIn() {
        Open();
    }

    void GoToMainMenu() {
        MainMenu.TransitionIn();
    }
}
