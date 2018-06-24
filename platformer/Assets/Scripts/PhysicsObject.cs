using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

	public float gravityModifier = 1f;
	public float minGroundNormalY = 0.65f;


	protected Vector2 groundNormal; 

	protected Rigidbody2D rb2d;
	protected Vector2 velocity;

	protected Vector2 targetVelocity;

	protected bool grounded;

	protected ContactFilter2D contactFilter;

	protected const float minMoveDistance = 0.001f;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected const float shellRadius = 0.01f; // Small padding
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


	// Use this for initialization

	void OnEnable(){
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	// Update is called once per frame
	void Update () {

		targetVelocity = Vector2.zero; // zero it out each frame
		ComputeVelocity ();

	}

	protected virtual void ComputeVelocity() {

	}

	void FixedUpdate() {
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		Vector2 deltaPosition = velocity * Time.deltaTime;

		grounded = false;

		Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);
		Vector2 move = moveAlongGround * deltaPosition.x;

		Movement (move, false); // moving along x axis

		move = Vector2.up * deltaPosition.y;
		Movement (move, true);

	}

	void Movement(Vector2 move, bool yMovement){
		float distance = move.magnitude; // Check that were actually moving significantly.
		if (distance > minMoveDistance) {
			int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius); // check collision before calculating next move position
			hitBufferList.Clear(); // do not reuse old data
			for (int i = 0; i < count; i++) {
				hitBufferList.Add (hitBuffer [i]); // copy from array into list
			}

			// check the normal of each of the raycast2dhit objects
			for (int i = 0; i < hitBufferList.Count; i++){
				Vector2 currentNormal = hitBufferList [i].normal;
				if (currentNormal.y > minGroundNormalY) { // want to see if on ground (not vertical walls)
					grounded = true;

					if (yMovement) {
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot (velocity, currentNormal);
				if (projection < 0) {
					velocity = velocity - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList [i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;

			}
		}



		rb2d.position = rb2d.position + move.normalized * distance;
	}


}
