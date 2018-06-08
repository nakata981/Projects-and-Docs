using UnityEngine;
using System.Collections;

public class FlameShot : MonoBehaviour
{
	float speed;
	float damage;
	float range;

	bool left;
	bool right;
	bool up;
	bool down;

	float spawnLocX;
	float spawnLocY;
	int i = 0;

	Rigidbody2D myRigidbody;
	GameObject myPlayer;
	Animator myAnimator;

	void Awake ()
	{
		myPlayer = GameObject.FindWithTag ("Player");
		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
		myPlayer.GetComponent<PlayerScript> ().DamageEnergy ();

		left = LanternScript.shootLeft;
		right = LanternScript.shootRight;
		up = LanternScript.shootUp;
		down = LanternScript.shootDown;

		damage = LanternScript.damage;
		range = LanternScript.range;
		speed = LanternScript.shotSpeed;

		myAnimator.speed = (speed / range) * 0.4f;
	}

	void Start ()
	{
		Debug.Log (range);
		transform.parent = myPlayer.transform;
		spawnLocX = gameObject.transform.position.x;
		spawnLocY = gameObject.transform.position.y;

		if (left && up) {
			myRigidbody.velocity = new Vector2 (-1, 1) * speed;
		} else if (left && down) {
			myRigidbody.velocity = new Vector2 (-1, -1) * speed;
		} else if (right && up) {
			myRigidbody.velocity = new Vector2 (1, 1) * speed;
		} else if (right && down) {
			myRigidbody.velocity = new Vector2 (1, -1) * speed;
		} else if (left) {
			myRigidbody.velocity = -transform.right * speed;
		} else if (right) {
			myRigidbody.velocity = transform.right * speed;
		} else if (up) {
			myRigidbody.velocity = transform.up * speed;
		} else if (down) {
			myRigidbody.velocity = -transform.up * speed;
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Wall") {
			Destroy (gameObject);
		}
		if (col.tag == "Enemy") {
			col.GetComponent<EnemyHealth> ().health -= damage;
			if (col.GetComponent<EnemyHealth> ().health <= 0) {
				Destroy (col.gameObject);
			}
		}
	}

	void OnTriggerStay2D (Collider2D col)
	{
		i++;
		if (col.tag == "Enemy") {
			if (i == 24) {
				col.GetComponent<EnemyHealth> ().health -= damage / 2;
				if (col.GetComponent<EnemyHealth> ().health <= 0) {
					Destroy (col.gameObject);
				}
				i = 0;
			}
		}
	}

	void Update ()
	{
		if (gameObject.transform.position.x >= spawnLocX + range || gameObject.transform.position.x <= spawnLocX - range) {
			Destroy (gameObject);
		}
		if (gameObject.transform.position.y >= spawnLocY + range || gameObject.transform.position.y <= spawnLocY - range) {
			Destroy (gameObject);
		}
	}
}
