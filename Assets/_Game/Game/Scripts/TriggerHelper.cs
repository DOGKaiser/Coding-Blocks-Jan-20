using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHelper : MonoBehaviour {

	public delegate void OnTriggerEnter(Collider2D collision);
	public event OnTriggerEnter TriggerEnter;
    
	public delegate void OnTriggerExit(Collider2D collision);
	public event OnTriggerExit TriggerExit;
	
	private void OnTriggerEnter2D(Collider2D collision) {
		TriggerEnter?.Invoke(collision);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		TriggerExit?.Invoke(collision);
	}
}
