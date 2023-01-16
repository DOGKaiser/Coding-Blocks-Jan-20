using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOverlayManager : MonoBehaviour {

    public static ScreenOverlayManager Instance;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
    }

    public void SetObjectLayer(GameObject g) {
        g.layer = gameObject.layer;
    }

}
