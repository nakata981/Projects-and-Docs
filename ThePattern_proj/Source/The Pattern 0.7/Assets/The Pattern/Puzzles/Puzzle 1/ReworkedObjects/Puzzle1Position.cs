using UnityEngine;
using System.Collections;

public class Puzzle1Position : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
				gameObject.transform.position = new Vector3 (GameObject.FindWithTag ("FinalCell").transform.position.x, -20, GameObject.FindWithTag ("FinalCell").transform.position.z - 7.5f);
		}
}
