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
    public float gripDifficulty = 3f;
    public float gripLoosen = 6f;

    float _currentGrip;
    bool _hooked;
    PlayerController _playerController;
    
    
    public void Start() {
        MoveLeft();
        lure.TriggerEnter += LureTriggerEnter;
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

            if (_playerController.TriggerGrip()) {
                _currentGrip += Time.deltaTime * gripDifficulty;
                _currentGrip = Mathf.Min(_currentGrip, 100);
            } 
            else {
                _currentGrip -= Time.deltaTime * gripLoosen;
                _currentGrip = Mathf.Max(_currentGrip, 0);
            }

            ((MatchMenuCustom)MatchMenuCustom.Instance).SetGrip(_currentGrip);
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
            _playerController.Hooked(this);
            _hooked = true;
            stopLureDebug = false;
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

    public float GetGrip() {
        return _currentGrip;
    }
}
