using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
	public Transform[] HealthPrefabs;
	public Transform[] TearPrefabs;
	public List <Transform> HealthList;
	public List <Transform> TearsList;

	public static int health = 3;
	public static int energy = 3;
	public static float speed = 3f;

	int energyRecharge = 96;
	//Flipped

	public static float shotSpeed = 0f;
	public static float shotRange = 0f;
	public static float shotDamage = 0f;

	public static int attackRate = 36;
	//Flipped

	Animator myAnimator;
	Animator weaponAnimator;

	static float mHP;
	static float mT;

	int a = 0;

	void Start ()
	{
		myAnimator = GetComponent<Animator> ();
		weaponAnimator = transform.GetChild (transform.childCount - 1).GetComponent<Animator> ();
		CreateEnergy (energy);
		CreateHearts (health);
	}

	void CreateHearts (int n)
	{
		for (int i = 0; i < n; i++) {
			Transform newHeart = (Transform)Instantiate (HealthPrefabs [0], Vector3.zero, Quaternion.identity);
			newHeart.transform.position = new Vector3 (Camera.main.transform.position.x - 8f + mHP, Camera.main.transform.position.y + 4.2f, -2);
			newHeart.transform.parent = Camera.main.transform;
			HealthList.Add (newHeart);
			mHP += 1f;
		}
	}

	public void TakeHealthDamage ()
	{
		int heartIndex = HealthList.Count - 1;
		for (int i = HealthList.Count - 1; i >= 0; i--) {
			if (HealthList [i].transform.name != "Empty(Clone)") {
				heartIndex = i;
				break;
			}
		}
		if (HealthList [heartIndex].transform.name != "Empty(Clone)") {
			Transform newSprite = (Transform)Instantiate (HealthPrefabs [HealthList [heartIndex].GetComponent<HeartIndex> ().StateIndex + 1], HealthList [heartIndex].transform.position, Quaternion.identity);
			newSprite.parent = Camera.main.transform;
			Destroy (HealthList [heartIndex].gameObject);
			HealthList.RemoveAt (heartIndex);
			HealthList.Insert (heartIndex, newSprite);
		}
		if (HealthList [0].transform.name == "Empty(Clone)") {
			Destroy (gameObject);
			Debug.Log ("You are Dead!");
		}
	}

	public void HealHP ()
	{
		int heartIndex = 0;
		for (int i = 0; i < HealthList.Count; i++) {
			if (HealthList [i].transform.name != "Health(Clone)") {
				heartIndex = i;
				break;
			}
		}
		if (HealthList [heartIndex].transform.name != "Health(Clone)") {
			Transform newSprite = (Transform)Instantiate (HealthPrefabs [HealthList [heartIndex].GetComponent<HeartIndex> ().StateIndex - 1], HealthList [heartIndex].transform.position, Quaternion.identity);
			newSprite.parent = Camera.main.transform;
			Destroy (HealthList [heartIndex].gameObject);
			HealthList.RemoveAt (heartIndex);
			HealthList.Insert (heartIndex, newSprite);
		}
	}

	void CreateEnergy (int n)
	{
		for (int i = 0; i < n; i++) {
			Transform newTearHeart = (Transform)Instantiate (TearPrefabs [0], Vector3.zero, Quaternion.identity);
			newTearHeart.transform.position = new Vector3 (Camera.main.transform.position.x - 8f + mT, Camera.main.transform.position.y + 3.5f, -2);
			newTearHeart.transform.parent = Camera.main.transform;
			TearsList.Add (newTearHeart);
			mT += 1f;
		}
	}

	public void DamageEnergy ()
	{
		int heartIndex = TearsList.Count - 1;
		for (int i = TearsList.Count - 1; i >= 0; i--) {
			if (TearsList [i].transform.name != "Empty(Clone)") {
				heartIndex = i;
				break;
			}
		}
		if (TearsList [heartIndex].transform.name != "Empty(Clone)") {
			Transform newSprite = (Transform)Instantiate (TearPrefabs [TearsList [heartIndex].GetComponent<HeartIndex> ().StateIndex + 1], TearsList [heartIndex].transform.position, Quaternion.identity);
			newSprite.parent = Camera.main.transform;
			Destroy (TearsList [heartIndex].gameObject);
			TearsList.RemoveAt (heartIndex);
			TearsList.Insert (heartIndex, newSprite);
		}
	}

	public void RechargeEnergy ()
	{
		int heartIndex = 0;
		for (int i = 0; i < TearsList.Count; i++) {
			if (TearsList [i].transform.name != "Energy(Clone)") {
				heartIndex = i;
				break;
			}
		}
		if (TearsList [heartIndex].transform.name != "Energy(Clone)") {
			Transform newSprite = (Transform)Instantiate (TearPrefabs [TearsList [heartIndex].GetComponent<HeartIndex> ().StateIndex - 1], TearsList [heartIndex].transform.position, Quaternion.identity);
			newSprite.parent = Camera.main.transform;
			Destroy (TearsList [heartIndex].gameObject);
			TearsList.RemoveAt (heartIndex);
			TearsList.Insert (heartIndex, newSprite);
		}
	}

	void FixedUpdate ()
	{
		a++;
		if (a >= energyRecharge) {
			RechargeEnergy ();
			a = 0;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		GetComponent<Rigidbody2D> ().velocity = movement * speed;

		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.W) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 7);
			weaponAnimator.SetInteger ("State", 7);
		} else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.S) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 1);
			weaponAnimator.SetInteger ("State", 1);
		} else if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.W) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 9);
			weaponAnimator.SetInteger ("State", 9);
		} else if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.S) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 3);
			weaponAnimator.SetInteger ("State", 3);
		} else if (Input.GetKey (KeyCode.A) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 4);
			weaponAnimator.SetInteger ("State", 4);
		} else if (Input.GetKey (KeyCode.D) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 6);
			weaponAnimator.SetInteger ("State", 6);
		} else if (Input.GetKey (KeyCode.W) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 8);
			weaponAnimator.SetInteger ("State", 8);
		} else if (Input.GetKey (KeyCode.S) && !LanternScript.isShooting) {
			myAnimator.SetInteger ("State", 2);
			weaponAnimator.SetInteger ("State", 2);
		}
	}
}