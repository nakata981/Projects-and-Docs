using UnityEngine;
using System.Collections;

public class Raycasting : MonoBehaviour
{
	Transform target;
	// Use this for initialization
	void Awake ()
	{
		target = GameObject.FindWithTag ("Floor").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector2 targetPos = target.position;
		Vector2 myPos = transform.position;
		RaycastHit2D hit = Physics2D.Raycast (myPos, -(myPos - targetPos).normalized, 30f, 1 << LayerMask.NameToLayer ("Walls"));
		Debug.DrawRay (myPos, -(myPos - targetPos).normalized);
		if (hit.collider != null) {
			Debug.Log ("Raycasted");
			Debug.Log (hit.transform);
			hit.transform.GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}
}
