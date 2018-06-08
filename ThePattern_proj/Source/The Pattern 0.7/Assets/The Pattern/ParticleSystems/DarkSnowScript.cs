using UnityEngine;
using System.Collections;

public class DarkSnowScript : MonoBehaviour
{
	
		void Start ()
		{

		}

		void Update ()
		{
				//Making the particle system chase the player
				gameObject.transform.position = new Vector3 (GameObject.FindWithTag ("Player").transform.position.x, GameObject.FindWithTag ("Player").transform.position.y + 10, GameObject.FindWithTag ("Player").transform.position.z);
		}
}
