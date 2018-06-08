using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour
{

	public bool isWalkable = true;
	public int posX;
	public int posY;
	public TileScript parent;

	public int gCost;
	public int hCost;

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	void Start ()
	{
		if (transform.tag == "Wall") {
			isWalkable = false;
		}
	}

}
