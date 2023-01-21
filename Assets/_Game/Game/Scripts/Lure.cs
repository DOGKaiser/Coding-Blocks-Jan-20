using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Lure : MonoBehaviour {

    public void Start() {
        MoveLeft();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Lure Hit " + collision.gameObject.name);
    }
    
    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log("Lure Happening " + collision.gameObject.name);
    }
    
    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("Lure Exit " + collision.gameObject.name);
    }

    void MoveLeft() {
        transform.DOMoveX(-15, 2).OnComplete(MoveRight);
    }
    
    void MoveRight() {
        transform.DOMoveX(15, 2).OnComplete(MoveLeft);
    }
}
