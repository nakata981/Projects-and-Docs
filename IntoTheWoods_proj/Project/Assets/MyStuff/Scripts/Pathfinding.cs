using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	public GameObject[,] myGrid;

	void Start ()
	{

	}

	/*void OnDrawGizmos ()
	{
		if (myGrid != null) {
			foreach (GameObject tile in myGrid) {
				Gizmos.color = (tile.GetComponent<TileScript> ().isWalkable) ? Color.white : Color.red;
				if (path != null) {
					if (path.Contains (tile.GetComponent<TileScript> ())) {
						Gizmos.color = Color.green;
					}
				}
				Gizmos.DrawWireCube (tile.transform.position, Vector2.one);
			}
		}
	}*/

	//public List<TileScript> path;

	public List<TileScript> FindPath (GameObject startingPos, GameObject targetPos)
	{
		TileScript startingTile = startingPos.GetComponent<TileScript> ();
		TileScript targetTile = targetPos.GetComponent<TileScript> ();

		List<TileScript> openSet = new List<TileScript> ();
		HashSet<TileScript> closedSet = new HashSet<TileScript> ();
		openSet.Add (startingTile);

		List<TileScript> myPath = new List<TileScript> ();

		while (openSet.Count > 0) {
			TileScript currentTile = openSet [0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet [i].fCost < currentTile.fCost || openSet [i].fCost == currentTile.fCost && openSet [i].hCost < currentTile.hCost) {
					currentTile = openSet [i];
				}
			}
			openSet.Remove (currentTile);
			closedSet.Add (currentTile);
			if (currentTile == targetTile) {
				myPath = Backtrack (startingTile, targetTile);
				break;
			}
			foreach (TileScript neighbour in GetNeighbours(currentTile)) {
				if (!neighbour.isWalkable || closedSet.Contains (neighbour)) {
					continue;
				}
				int newMovementCost = currentTile.gCost + GetDistance (currentTile, neighbour);
				if (newMovementCost < neighbour.gCost || !openSet.Contains (neighbour)) {
					neighbour.gCost = newMovementCost;
					neighbour.hCost = GetDistance (neighbour, targetTile);
					neighbour.parent = currentTile;

					if (!openSet.Contains (neighbour)) {
						openSet.Add (neighbour);
					}
				}
			}
		}
		//path = myPath;
		return myPath;
	}

	List<TileScript> Backtrack (TileScript startTile, TileScript endTile)
	{
		List<TileScript> path = new List<TileScript> ();
		TileScript currentTile = endTile;

		while (currentTile != startTile) {
			path.Add (currentTile);
			currentTile = currentTile.parent;
		}
		path.Reverse ();
		return path;
	}

	public List<TileScript> GetNeighbours (TileScript tile)
	{
		List<TileScript> neighbours = new List<TileScript> ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
				} else {
					int checkX = tile.posX + x;
					int checkY = tile.posY + y;
					if (checkX >= 0 && checkX < GetComponent<CaveGenerator> ().width && checkY >= 0 && checkY < GetComponent<CaveGenerator> ().height) {
						neighbours.Add (myGrid [checkX, checkY].GetComponent<TileScript> ());
					}
				}
			}
		}
		return neighbours;
	}

	int GetDistance (TileScript tileA, TileScript tileB)
	{
		int disX = Mathf.Abs (tileA.posX - tileB.posX);
		int disY = Mathf.Abs (tileA.posY - tileB.posY);

		if (disX > disY) {
			return 14 * disY + 10 * (disX - disY);
		} else {
			return 14 * disX + 10 * (disY - disX);
		}
	}

	struct Coord
	{
		public int X;
		public int Y;

		public Coord (int x, int y)
		{
			X = x;
			Y = y;
		}
	}

}
