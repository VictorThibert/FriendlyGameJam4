using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPlatformController : PhysicsObject {

	public float jumpTakeOffSpeed = 10f;
	public float maxSpeed = 7f;
	public bool firstJump = false;
	private bool canJump = true;


	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private AudioSource[] audioSource;

	private int score;
	public Text text;


	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponents<AudioSource>();

		score = 0;

	}




//	private Vector3 originalPos;

	void Start(){
//		originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		//alternatively, just: originalPos = gameObject.transform.position;

	}

	protected override void ComputeVelocity () {

		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis ("Horizontal");

	
		canJump = grounded || firstJump || canJump;

		if (Input.GetButtonDown ("Jump") && canJump) {

			animator.SetBool ("jump", true);
			
			firstJump = grounded ? true : false;
			canJump = firstJump;
			velocity.y = jumpTakeOffSpeed;
			audioSource[0].Play ();


		} else if (Input.GetButtonUp ("Jump")) { // jump cancellation
			if (velocity.y > 0) {
				velocity.y = velocity.y * 0.5f;

				if (Random.value < 0.01) {
					audioSource [1].Play ();
				}

			}
			animator.SetBool ("jump", false);

		}

		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
		if (flipSprite) {
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}



		animator.SetFloat ("playerSpeed", Mathf.Abs (velocity.x) / maxSpeed); // X direction only
		targetVelocity = move * maxSpeed;


	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Coin"){
			Debug.Log (score);
			score += 1;
			UpdateText (0);

			audioSource [2].Play ();
			collider.gameObject.SetActive (false);
		}
		else if (collider.tag == "Spike"){
			audioSource [3].Play ();
//			gameObject.transform.position = originalPos;
			animator.SetBool ("dead", true);
			Die ();
		}
		else if (collider.tag == "Finish"){
			audioSource [4].Play ();
		}
		else if (collider.tag == "Fish"){
			audioSource [5].Play ();
			maxSpeed += 3f;
			collider.gameObject.SetActive (false);
		}
		else if (collider.tag == "Potion"){
			audioSource [6].Play ();
			jumpTakeOffSpeed += 3f;
			collider.gameObject.SetActive (false);
		}
	}

	void UpdateText(int v) {
		text.text = "" + score;
	}


	public void Die(){
		StartCoroutine(DelayedDeath());
	}

	private IEnumerator DelayedDeath(){
		yield return new WaitForSeconds (0.7f);
		Application.LoadLevel(Application.loadedLevel);
	}

		
}
