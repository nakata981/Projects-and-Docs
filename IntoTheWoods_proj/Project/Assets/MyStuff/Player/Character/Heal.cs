using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour
{
	
	void Start ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerScript> ().HealHP ();
		}
	}

	void Update ()
	{
	
	}
}
