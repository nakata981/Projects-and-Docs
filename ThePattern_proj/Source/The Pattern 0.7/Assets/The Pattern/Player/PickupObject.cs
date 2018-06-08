using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour
{
		GameObject mainCamera;
		bool carrying;
		GameObject carriedObject;
		public float distance;
		public float smooth;

		void Start ()
		{
				mainCamera = GameObject.FindWithTag ("MainCamera");
		}

		void Update ()
		{
				if (carrying) {
						Carry (carriedObject);
						CheckDrop ();
				} else {
						Pickup ();
				}
		}

		void Carry (GameObject o)
		{
				o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
		}

		void Pickup ()
		{
				if (Input.GetKeyDown (KeyCode.E)) {
						int x = Screen.width / 2;
						int y = Screen.height / 2;
						Ray ray = mainCamera.camera.ScreenPointToRay (new Vector3 (x, y));
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit)) {
								Pickupable p = hit.collider.GetComponent<Pickupable> ();
								if (p != null) {
										carrying = true;
										carriedObject = p.gameObject;
										p.rigidbody.isKinematic = true;
								}
						}
				}
		}
		void CheckDrop ()
		{
				if (Input.GetKeyDown (KeyCode.E)) {
						DropObject ();
				}
		}

		void DropObject ()
		{
				carrying = false;
				carriedObject.gameObject.rigidbody.isKinematic = false;
				carriedObject = null;
		}

}
