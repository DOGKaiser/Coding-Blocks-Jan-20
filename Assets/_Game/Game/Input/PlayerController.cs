using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Vector3 move;
    private Vector3 mousePositionWorld;
    private bool mKeepFiring;
    public Weapon weapon;

    private void Awake() {
    }

    void Update() {
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        if (move != Vector3.zero) {
            transform.DOMove(move * playerSpeed + transform.position, Time.deltaTime);

            if (move.x != 0) {
                //gameObject.transform.forward = new Vector3(move.x, move.y, 0);
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
}
