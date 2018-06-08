using UnityEngine;
using System.Collections;

public class HeartIndex : MonoBehaviour
{
	public int StateIndex;

	void Start ()
	{
		if (transform.name == "Health(Clone)" || transform.name == "Energy(Clone)") {
			StateIndex = 0;
		} else if (transform.name == "HalfHealth(Clone)" || transform.name == "HalfEnergy(Clone)") {
			StateIndex = 1;
		} else if (transform.name == "Empty(Clone)") {
			StateIndex = 2;
		}
	}

	void Update ()
	{
	
	}
}