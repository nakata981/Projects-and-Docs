using UnityEngine;
using System.Collections;

public class PosInGrid : MonoBehaviour
{

	public int posX;
	public int posY;

	void Start ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Floor") {
			posX = col.GetComponent<TileScript> ().posX;
			posY = col.GetComponent<TileScript> ().posY;
		}
	}
}
