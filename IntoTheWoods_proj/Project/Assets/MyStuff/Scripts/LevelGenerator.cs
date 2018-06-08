using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
	public Transform[,] Grid;
	public static Vector3 GridSize = new Vector3 (15, 1, 15);

	public Transform CellPrefab;
	public Transform CavePrefab;
	public Transform FirstRoom;
	public static int LevelBaseSize = 10;

	public List <Transform> NormalRooms;
	public List <Transform> SpecialRooms;

	int spawnPosCount;

	void Start ()
	{
		LevelBaseSize--;
		CreateGrid ();
		SetAdjacents ();
		SetStart ();
		SetGen ();
		SpawnCaves ();
	}

	void CreateGrid ()
	{
		Grid = new Transform[(int)GridSize.x, (int)GridSize.z];
		for (int x = 0; x < GridSize.x; x++) {
			for (int y = 0; y < GridSize.z; y++) {
				Transform newCell = (Transform)Instantiate (CellPrefab, new Vector3 (x * 60, y * 60, 0), Quaternion.identity);
				newCell.transform.parent = transform;
				Grid [x, y] = newCell;
			}
		}
	}

	void SetAdjacents ()
	{
		for (int x = 0; x < GridSize.x; x++) {
			for (int y = 0; y < GridSize.z; y++) {
				CellScript cScript = Grid [x, y].GetComponent<CellScript> ();
				if (x - 1 >= 0) {
					cScript.Adjacents.Add (Grid [x - 1, y].transform);
				}
				if (x + 1 < GridSize.x) {
					cScript.Adjacents.Add (Grid [x + 1, y].transform);
				}
				if (y - 1 >= 0) {
					cScript.Adjacents.Add (Grid [x, y - 1].transform);
				}
				if (y + 1 < GridSize.z) {
					cScript.Adjacents.Add (Grid [x, y + 1].transform);
				}
			}
		}
	}

	void SetStart ()
	{
		AddToSet (Grid [7, 7].transform);
	}

	void AddToSet (Transform current)
	{
		NormalRooms.Add (current);
		foreach (Transform adj in current.GetComponent<CellScript>().Adjacents) {
			adj.GetComponent<CellScript> ().Adjacents.Remove (current);
		}
	}

	void SetGen ()
	{
		Transform newDoor;
		Transform previousCell;
		Transform currentCell;
		List <Transform> adjacents;
		List <Transform> normalRooms = NormalRooms;
		for (int i = 0; i < LevelBaseSize; i++) {
			currentCell = normalRooms [Random.Range (0, normalRooms.Count)];
			adjacents = currentCell.GetComponent<CellScript> ().Adjacents;
			previousCell = currentCell;
			if (adjacents.Count == 0) {
				i--;
			} else {
				currentCell = adjacents [Random.Range (0, adjacents.Count)];
				if (!normalRooms.Contains (currentCell)) {
					if (currentCell.GetComponent<CellScript> ().Adjacents.Count > 2) {
						AddToSet (currentCell);
						/*if (currentCell.transform.position.x > previousCell.transform.position.x) {
						} else if (currentCell.transform.position.x < previousCell.transform.position.x) {
						} else if (currentCell.transform.position.y > previousCell.transform.position.y) {
						} else if (currentCell.transform.position.y < previousCell.transform.position.y) {
						}*/
						if (previousCell.GetComponent<CellScript> ().Adjacents.Contains (currentCell)) {
							previousCell.GetComponent<CellScript> ().Adjacents.Remove (currentCell);
						}
					} else {
						i--;
					}
				} else {
					i--;
				}
			}
		}
	}

	void SpawnCaves ()
	{
		float a = Time.time;
		foreach (Transform room in NormalRooms) {
			Transform newCave = (Transform)Instantiate (CavePrefab, room.position, Quaternion.identity);
			newCave.parent = room;
			newCave.name = "CaveCell";
		}
	}
}