using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody2D controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField] private Transform playerVisuals;
    [SerializeField] private Animator animator;

    Fisher _fisher;
    Vector3 _previousVectorFromFisher;
    bool _pullingAway;
    
    private Vector2 _velocity = Vector2.zero;
    private Vector3 move;
    private Vector3 mousePositionWorld;
    public bool flipped;
    private bool mKeepFiring;
    public Weapon weapon;
    public GameObject FishMouth;

    public float loseDistance;
    bool _playerLost = false;

    private void Awake() {
        controller = gameObject.GetComponent<Rigidbody2D>();
        flipped = false;

    }

    void Start() {
        _fisher = ((MatchGameCustom)MatchMenu.CurrentMatch).fisher;
        animator = playerVisuals.GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (_playerLost) {
            return;
        }
        
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }
        
        _previousVectorFromFisher = transform.position - _fisher.transform.position;
        
        // Move the character by finding the target velocity
        Vector2 targetVelocity = Vector2.zero;
        SetPullingAway();

        if (_fisher.GetHooked()) {
            if (Vector3.Distance(_fisher.transform.position,transform.position) <= loseDistance) {
                _playerLost = true;
                _fisher.GotFish();
                animator.SetTrigger("Player_Caught");
                return;
            }
            
            targetVelocity = _fisher.GetPullStrength();
        }
        if (move != Vector3.zero) {
            targetVelocity += new Vector2(move.x, move.y) * playerSpeed;
            float anispeed = (targetVelocity.magnitude / 4);
            if (_pullingAway) anispeed +=(_fisher.GetPullStrength().magnitude/4);
            animator.SetFloat("AnimationSpeed", anispeed);
        } 
        else {
            
            animator.SetFloat("AnimationSpeed", 0);
        }

        if (targetVelocity != Vector2.zero) {
            // And then smoothing it out and applying it to the character
            controller.velocity = Vector2.SmoothDamp(controller.velocity, targetVelocity, ref _velocity, _movementSmoothing);

            if (move.x > 0) {
                playerVisuals.localScale = new Vector3(-1, 1, 1);
                flipped = true;
            } 
            else if (move.x < 0) {
                playerVisuals.localScale = new Vector3(1, 1, 1);
                flipped = false;
            }

            if (move.y > 0) {
                transform.DORotate(new Vector3(0, 0, -10 * (flipped ? -1 : 1)), 0.5f);
            } 
            else if (move.y < 0) {
                transform.DORotate(new Vector3(0, 0, 10 * (flipped ? -1 : 1)), 0.5f);
            } 
            else {
                transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            }
        }

        /*
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        */

        if (mKeepFiring) {
            weapon.Fire();
        }
        
        // playerVelocity.y += gravityValue * Time.deltaTime;
    }
    
    public void OnMove(InputAction.CallbackContext context) {
        Vector2 movement = context.ReadValue<Vector2>();
        move = new Vector3(movement.x, movement.y, 0);
    }

    public void OnFire(InputAction.CallbackContext context) {
        float fire = context.ReadValue<float>();
        
        if (context.phase == InputActionPhase.Started) {
            weapon.Fire();
            mKeepFiring = true;
        }

        if (context.phase == InputActionPhase.Canceled) {
            mKeepFiring = false;
        }

        Debug.LogFormat("Player Fired {0} with: {1}", context.phase, fire);
    }

    public void OnFireDirectionMouse(InputAction.CallbackContext context) {
        Vector2 mPos = context.ReadValue<Vector2>();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mPos.x, mPos.y, 0));

        LayerMask mask = LayerMask.GetMask("MousePosition");
        if (Physics.Raycast(ray, out hit, 1000, mask)) {
            mousePositionWorld = hit.point;
        }
    }

    public void OnFireDirectionGamePad(InputAction.CallbackContext context) {
        Vector2 mPosition = context.ReadValue<Vector2>();
        if (mPosition != Vector2.zero) {
            mousePositionWorld = new Vector3(mPosition.x, 0, mPosition.y).normalized * 1000;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Fish Hit " + collision.gameObject.name);

        if (collision.CompareTag("Blocker")) {
            _velocity = Vector3.zero;
        }
        if (collision.CompareTag("Lure")) {
            _velocity = Vector3.zero;
        }
    }

    public Vector2 rigidBodyVelocity;

    private void SetPullingAway() {
        _pullingAway = move.x < 0 || move.y < 0;
    }
    
    public bool PullingAway() {
        return _pullingAway;
    }
}
