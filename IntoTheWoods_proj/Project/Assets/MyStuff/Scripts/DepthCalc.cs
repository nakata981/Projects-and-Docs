using UnityEngine;
using System.Collections;

public class DepthCalc : MonoBehaviour
{
	float myZ = 0f;
	//public bool isStatic = false;

	void Start ()
	{
		myZ = transform.position.z;
		//transform.position = new Vector3 (transform.position.x, transform.position.y, myZ + transform.localPosition.y / 100f);
	}

	void Update ()
	{
		//if (!isStatic) {
		transform.position = new Vector3 (transform.position.x, transform.position.y, myZ + transform.localPosition.y / 100f);
		//}
	}
}
