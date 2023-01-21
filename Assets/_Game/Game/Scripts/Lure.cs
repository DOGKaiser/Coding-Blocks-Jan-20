using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Lure : MonoBehaviour {

    public Animator animator;
    public TriggerHelper lure;
    public Vector3 hookedOffset = new Vector3(-0.56f, 0.03f, 0);
    [SerializeField] private Transform lureVisuals;
    public bool stopLureDebug;

    bool _hooked;
    PlayerController _playerController;
    Fisher _fisher;
    
    
    public void Start() {
        MoveLeft();
        lure.TriggerEnter += LureTriggerEnter;
        _fisher = ((MatchGameCustom)MatchMenu.CurrentMatch).fisher;
    }
    
    public void FixedUpdate() {
        if (_hooked) {
            if (_playerController.flipped) {
                Flipped(true);
                transform.position = _playerController.FishMouth.transform.position + (hookedOffset * -1);
            } 
            else {
                Flipped(false);
                transform.position = _playerController.FishMouth.transform.position + hookedOffset;
            }
        }
    }

    void MoveLeft() {
        if (!stopLureDebug) {
            transform.DOMoveX(-15, 5).OnComplete(MoveRight);
        }
    }
    
    void MoveRight() {
        if (!stopLureDebug) {
            transform.DOMoveX(15, 5).OnComplete(MoveLeft);
        }
    }
    
    private void LureTriggerEnter(Collider2D collision) {

        if (collision.CompareTag("Player")) {
            Debug.Log("Lure Hit " + collision.gameObject.name);
            animator.SetBool("Hooked", true);
            _playerController = collision.transform.GetComponent<PlayerController>();
            _hooked = true;
            stopLureDebug = false;
            _fisher.SetHooked(_playerController, true);
            transform.DOKill();
        }
    }

    void Flipped(bool flipped) {
        if (flipped) {
            lureVisuals.localScale = new Vector3(-1, 1, 1);
        } 
        else {
            lureVisuals.localScale = new Vector3(1, 1, 1);
        }
    }
}
