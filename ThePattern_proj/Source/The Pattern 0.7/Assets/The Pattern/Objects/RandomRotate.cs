using UnityEngine;
using System.Collections;

public class RandomRotate : MonoBehaviour
{
		public float speed;
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.Rotate (new Vector3 (Random.Range (1, 45), Random.Range (1, 45), Random.Range (1, 45)) * Time.deltaTime * speed);
		}
}
