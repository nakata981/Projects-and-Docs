using UnityEngine;
using System.Collections;

public class ApplyDamage : MonoBehaviour
{
	PlayerScript playerScript;

	void Awake ()
	{
		playerScript = GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ();
	}

	void OnCollisionExit2D (Collision2D col)
	{
		StopCoroutine ("DrainEnergy");
		StopCoroutine ("DrainHealth");
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		{
			if (col.transform.tag == playerScript.transform.tag) {
				Debug.Log (playerScript.TearsList [0].name);
				if (playerScript.TearsList [0].transform.name == "Empty(Clone)") {
					StopCoroutine ("DrainEnergy");
					StartCoroutine ("DrainHealth");
				} else {
					StartCoroutine ("DrainEnergy");
				}
			}
		}
	}

	IEnumerator DrainEnergy ()
	{
		while (true) {
			yield return new WaitForSeconds (.5f);
			playerScript.DamageEnergy ();
			if (playerScript.TearsList [0].transform.name == "Empty(Clone)") {
				StartCoroutine ("DrainHealth");
				StopCoroutine ("DrainEnergy");
			}
		}
	}

	IEnumerator DrainHealth ()
	{
		while (true) {
			yield return new WaitForSeconds (.5f);
			playerScript.TakeHealthDamage ();
			if (playerScript.TearsList [0].transform.name != "Empty(Clone)") {
				StartCoroutine ("DrainEnergy");
				StopCoroutine ("DrainHealth");
			}
		}
	}
}