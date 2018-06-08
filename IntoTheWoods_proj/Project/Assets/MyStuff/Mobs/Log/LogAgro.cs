using UnityEngine;
using System.Collections;

public class LogAgro : MonoBehaviour
{
	Animator myAnimator;
	PlayerScript playerScript;

	void Awake ()
	{
		myAnimator = GetComponent<Animator> ();
		playerScript = GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player") {
			myAnimator.SetTrigger ("Trigger");
			StartCoroutine ("DrainEnergy");
		}
	}

	IEnumerator DrainEnergy ()
	{
		while (true) {
			playerScript.DamageEnergy ();
			yield return new WaitForSeconds (1f);
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "Player") {
			myAnimator.SetTrigger ("Trigger");
			StopCoroutine ("DrainEnergy");
		}
	}
}
