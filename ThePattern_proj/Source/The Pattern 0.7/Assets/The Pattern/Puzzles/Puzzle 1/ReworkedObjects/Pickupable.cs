using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour
{
	
		void Start ()
		{
				rigidbody.freezeRotation = true;
		}

		void OnTriggerEnter (Collider triggerEnter)
		{
				if (triggerEnter.gameObject.tag == "Trigger") {
						GameObject.Find ("Pillar").transform.Translate (new Vector3 (0, - 2, 0), Space.World);
						;
				}
		}
		void OnTriggerExit (Collider triggerExit)
		{
				if (triggerExit.gameObject.tag == "Trigger") {
						GameObject.Find ("Pillar").transform.Translate (new Vector3 (0, 2, 0), Space.World);
				}
		}

		void Update ()
		{
	
		}
}
