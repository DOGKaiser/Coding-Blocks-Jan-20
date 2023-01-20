using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerController playerController;

    Vector3 startPos = Vector3.zero;

    public void Awake() {
        if (playerPrefab != null) {
            playerController = GameObject.Instantiate(playerPrefab, startPos, transform.rotation).GetComponent<PlayerController>();
            transform.parent = playerController.transform;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        playerController.OnMove(context);
    }

    public void OnFire(InputAction.CallbackContext context) {
        // playerController.OnFire(context);
    }

    public void OnFireDirectionMouse(InputAction.CallbackContext context) {
        // playerController.OnFireDirectionMouse(context);
    }

    public void OnFireDirectionGamePad(InputAction.CallbackContext context) {
        // playerController.OnFireDirectionGamePad(context);
    }
}
