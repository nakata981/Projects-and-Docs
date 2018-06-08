using UnityEngine;
using System.Collections;

public class LanternScript : MonoBehaviour
{
	public Transform shotPrefab;
	public static float shotSpeed = 0.5f;
	public static float damage = 4f;
	public static float range = 1.5f;
	PlayerScript myPlayerScript;
	Animator playerAnimator;
	Animator myAnimator;

	int attackRate;
	int i;
	public static bool shootLeft;
	public static bool shootRight;
	public static bool shootUp;
	public static bool shootDown;

	public static bool isShooting;

	void Awake ()
	{
		damage += PlayerScript.shotDamage;
		attackRate = PlayerScript.attackRate;
		myAnimator = GetComponent<Animator> ();
		playerAnimator = transform.parent.GetComponent<Animator> ();
		myPlayerScript = transform.parent.GetComponent<PlayerScript> ();
		i = attackRate;
	}

	void FalseBools ()
	{
		shootLeft = false;
		shootRight = false;
		shootUp = false;
		shootDown = false;
	}

	void FixedUpdate ()
	{
		i++;
		if (i >= attackRate) {
			i = attackRate;
		}
		if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.UpArrow)) {
			myAnimator.SetInteger ("State", 7);
			playerAnimator.SetInteger ("State", 7);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootLeft = true;
				shootUp = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.DownArrow)) {
			myAnimator.SetInteger ("State", 1);
			playerAnimator.SetInteger ("State", 1);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootLeft = true;
				shootDown = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.RightArrow) && Input.GetKey (KeyCode.UpArrow)) {
			myAnimator.SetInteger ("State", 9);
			playerAnimator.SetInteger ("State", 9);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootRight = true;
				shootUp = true;
				isShooting = true;
				Instantiate (shotPrefab, new Vector3 (transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.RightArrow) && Input.GetKey (KeyCode.DownArrow)) {
			myAnimator.SetInteger ("State", 3);
			playerAnimator.SetInteger ("State", 3);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootRight = true;
				shootDown = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			myAnimator.SetInteger ("State", 4);
			playerAnimator.SetInteger ("State", 4);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootLeft = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			myAnimator.SetInteger ("State", 6);
			playerAnimator.SetInteger ("State", 6);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootRight = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;

			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			myAnimator.SetInteger ("State", 8);
			playerAnimator.SetInteger ("State", 8);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootUp = true;
				isShooting = true;
				Instantiate (shotPrefab, transform.parent.position + new Vector3 (0, 0, 0.01f), Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			myAnimator.SetInteger ("State", 2);
			playerAnimator.SetInteger ("State", 2);
			if (i >= attackRate && myPlayerScript.TearsList [0].name != "Empty(Clone)") {
				shootDown = true;
				isShooting = true;
				Instantiate (shotPrefab, gameObject.transform.position, Quaternion.identity);
				i = 0;
			}
			isShooting = false;
			FalseBools ();
		}
	}
}
