using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalAgro : MonoBehaviour
{

	public int agroRange = 4;
	int chaseRange = 30;
	public float speed = 5f;
	public bool wasHit = false;
	int layerMask;

	GameObject player;
	Rigidbody2D myRigidbody;
	Pathfinding myPathfinding;
	GameObject myTile;
	GameObject targetTile;
	Transform myCave;
	List<TileScript> myPath;


	int myX, myY;
	int tarX, tarY;

	void Awake ()
	{
		player = GameObject.FindWithTag ("Player");
		myRigidbody = transform.GetComponent<Rigidbody2D> ();
		layerMask = LayerMask.NameToLayer ("Walls");
		Debug.Log (layerMask);
	}

	void Start ()
	{
		StartCoroutine ("CheckRange");
	}

	void GetPositions ()
	{
		myX = GetComponent<PosInGrid> ().posX;
		myY = GetComponent<PosInGrid> ().posY;
		tarX = player.GetComponent<PosInGrid> ().posX;
		tarY = player.GetComponent<PosInGrid> ().posY;
	}

	IEnumerator CheckRange ()
	{
		while (true) {
			if (player == null) {
				StopCoroutine ("CheckRange");
			}
			yield return new WaitForSeconds (1f);
			GetPositions ();

			if (Vector2.Distance (transform.position, player.transform.position) <= agroRange || wasHit) {
				StartCoroutine ("FindPath");
				GetCave ();
				StopCoroutine ("CheckRange");
			}

		}
	}

	IEnumerator FindPath ()
	{
		while (true) {
			if (player == null) {
				StopCoroutine ("FindPath");
			}
			if (!isInSight ()) {
				GetPositions ();
				targetTile = myPathfinding.myGrid [tarX, tarY];
				myTile = myPathfinding.myGrid [myX, myY];
				myPath = myPathfinding.FindPath (myTile, targetTile);
			}
			if (Vector2.Distance (transform.position, player.transform.position) >= chaseRange && !wasHit) {
				StartCoroutine ("CheckRange");
				myPath.Clear ();
				StopCoroutine ("FindPath");
			}
			yield return new WaitForSeconds (0.2f);
		}
	}

	bool  isInSight ()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -(transform.position - player.transform.position).normalized, 30f, 1 << LayerMask.NameToLayer ("Walls"));
		if (hit.collider != null) {
			if (hit.transform.tag == "Wall") {
				Debug.Log ("Hit Wall");
				return false;
			} else if (hit.transform.tag == "Player") {
				return true;
			} else {
				Debug.Log ("Hit something else");
				return false;
			}
		} else {
			Debug.Log ("Null hit");
			return false;
		}
	}

	void FixedUpdate ()
	{
		Debug.DrawRay (transform.position, -(transform.position - player.transform.position).normalized);
		if (isInSight () && Vector2.Distance (transform.position, player.transform.position) <= agroRange) {
			Vector2 dir = -(transform.position - player.transform.position).normalized * speed;
			myRigidbody.velocity = dir;
		} else if (myPath != null && myPath.Count > 1 && !isInSight ()) {
			Vector2 dir = (myPath [0].transform.position - transform.position).normalized * speed;
			myRigidbody.velocity = dir;
		}
	}

	void GetCave ()
	{
		if (transform.parent != null) {
			myCave = transform;
			while (myCave.parent != null) {
				if (myCave.parent.tag == "Cave") {
					myCave = myCave.parent;
					break;
				} else {
					myCave = myCave.parent;
				}
			}
			myPathfinding = myCave.GetComponent<Pathfinding> ();
		} else {
			Debug.Log ("Enemy must be a Cave's child!");
		}
	}

	void OnDrawGizmos ()
	{
		if (myPath != null) { 
			foreach (TileScript tile in myPath) {
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube (tile.transform.position, Vector2.one);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Shot") {
			wasHit = true;
		}
	}
}
