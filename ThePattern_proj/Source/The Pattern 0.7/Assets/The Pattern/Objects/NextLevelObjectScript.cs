using UnityEngine;
using System.Collections;

public class NextLevelObjectScript : MonoBehaviour
{
	
		void Start ()
		{
		}

		void Update ()
		{
				gameObject.transform.position = new Vector3 (GameObject.FindWithTag ("FinalCell").transform.position.x, 1, GameObject.FindWithTag ("FinalCell").transform.position.z);
				gameObject.renderer.enabled = PlayerSet.Pickups == 0;
		}
}
