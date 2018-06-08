using UnityEngine;
using System.Collections;

public class RespawnObjects : MonoBehaviour
{

		void Start ()
		{
	
		}

		void Update ()
		{
				if (gameObject.transform.position.y <= -50) {
						gameObject.transform.position = new Vector3 (GameObject.FindWithTag ("FinalCell").transform.position.x, - 30, GameObject.FindWithTag ("FinalCell").transform.position.z - 15f);
				}
		}
}
