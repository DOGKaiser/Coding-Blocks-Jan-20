using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Lure : MonoBehaviour {

    public TriggerHelper lure;
    public bool stopLure;
    
    public void Start() {
        MoveLeft();
        lure.TriggerEnter += LureTriggerEnter;
    }

    private void LureTriggerEnter(Collider2D collision) {
        Debug.Log("Lure Hit " + collision.gameObject.name);
    }

    void MoveLeft() {
        if (!stopLure) {
            transform.DOMoveX(-15, 5).OnComplete(MoveRight);
        }
    }
    
    void MoveRight() {
        if (!stopLure) {
            transform.DOMoveX(15, 5).OnComplete(MoveLeft);
        }
    }
}
