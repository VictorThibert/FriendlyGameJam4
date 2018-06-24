using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	private int hits = 0;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col){
		animator.SetBool ("break", true);
		Debug.Log("OnCollisionEnter2D");
		hits++;
		if (hits >= 2) {
			gameObject.SetActive (false);
		}
	}
}
