using UnityEngine;
using System.Collections;

public class Shoot_Backup : MonoBehaviour
{

	/*float speed = 0f;
	float damage = 2f;
	float range = 0f;

	float spawnLocX;
	float spawnLocY;

	Rigidbody2D myRigidbody;

	void Start ()
	{
		transform.parent = GameObject.FindWithTag ("Player").transform;
		myRigidbody = transform.GetComponent<Rigidbody2D> ();
		spawnLocX = gameObject.transform.position.x;
		spawnLocY = gameObject.transform.position.y;

		range += PlayerScript.shotRange;
		speed += PlayerScript.shotSpeed;
		damage += PlayerScript.shotDamage;
		/*if (PlayerScript.shootLeft && PlayerScript.shootUp) {

		} else if (PlayerScript.shootLeft && PlayerScript.shootDown) {

		} else if (PlayerScript.shootRight && PlayerScript.shootUp) {

		} else if (PlayerScript.shootRight && PlayerScript.shootDown) {

		} else* /
		if (PlayerScript.shootLeft) {
			myRigidbody.velocity = -transform.right * speed;
		} else if (PlayerScript.shootRight) {
			myRigidbody.velocity = transform.right * speed;
		} else if (PlayerScript.shootUp) {
			myRigidbody.velocity = transform.up * speed;
		} else if (PlayerScript.shootDown) {
			myRigidbody.velocity = -transform.up * speed;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		
	}

	void Update ()
	{
		if (gameObject.transform.position.x >= spawnLocX + range || gameObject.transform.position.x <= spawnLocX - range) {
			Destroy (gameObject);
		}
		if (gameObject.transform.position.y >= spawnLocY + range || gameObject.transform.position.y <= spawnLocY - range) {
			Destroy (gameObject);
		}
	}*/
}
