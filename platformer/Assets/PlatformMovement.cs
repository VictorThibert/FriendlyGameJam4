using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	// Use this for initialization


	private Vector3 A;
	private Vector3 B;
	private Vector3 nextPosition;
	private float speed = 3f;
	[SerializeField]
	private Transform child;
	[SerializeField]
	private Transform tranB;


	void Start () {
		A = child.localPosition;
		B = tranB.localPosition;
		nextPosition = B;

	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move() {
		child.localPosition = Vector3.MoveTowards (child.localPosition, nextPosition, speed * Time.deltaTime);
		if (Vector3.Distance (child.localPosition, nextPosition) <= 0.1) {
			Change ();
		}
	}

	private void Change() {
		nextPosition = nextPosition != A ? A : B;
	}
}
