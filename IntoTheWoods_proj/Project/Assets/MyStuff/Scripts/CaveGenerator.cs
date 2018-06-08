using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CaveGenerator : MonoBehaviour
{
	public int width;
	public int height;

	public GameObject[,] Tiles;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	List<Vector2[]> passagePoints = new List<Vector2[]> ();

	public string seed;
	public bool useRandomSeed;

	[Range (0, 100)]
	public int
		randomFillPercent;

	int[,] cave;

	void Start ()
	{
		GenerateMap ();
		CreateTiles ();
		GetComponent<Pathfinding> ().myGrid = Tiles;
	}

	void GenerateMap ()
	{
		cave = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}
		ClearOutMap ();
	}

	void RandomFillMap ()
	{
		if (useRandomSeed) {
			seed = UnityEngine.Random.value.ToString ();
			//Debug.Log ("seed:" + seed);
		}

		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					cave [x, y] = 1;
				} else {
					cave [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap ()
	{
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int adjWallTiles = GetAdjWallCount (x, y);
				if (adjWallTiles > 4) {
					cave [x, y] = 1;
				} else if (adjWallTiles < 4) { 
					cave [x, y] = 0;
				}
			}
		}
	}

	int GetAdjWallCount (int gridX, int gridY)
	{
		int wallCount = 0;

		for (int adjX = gridX - 1; adjX <= gridX + 1; adjX++) {
			for (int adjY = gridY - 1; adjY <= gridY + 1; adjY++) {
				if (adjX >= 0 && adjX < width && adjY >= 0 && adjY < height) {
					if (adjX != gridX || adjY != gridY) {
						wallCount += cave [adjX, adjY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	void CreateTiles ()
	{
		Tiles = new GameObject[width, height];
		if (cave != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Vector3 pos = new Vector3 (-width / 2 + x + .5f + transform.position.x, -height / 2 + y + .5f + transform.position.y, 0);
					if (cave [x, y] == 0) {
						GameObject floorTile = Instantiate (floorTiles [0], pos, Quaternion.identity) as GameObject;
						floorTile.transform.parent = gameObject.transform.GetChild (0);
						floorTile.name = string.Format ("{0},{1}", x, y);
						Tiles [x, y] = floorTile;
						floorTile.GetComponent<TileScript> ().posX = x;
						floorTile.GetComponent<TileScript> ().posY = y;
					} else if (cave [x, y] == 1) {
						GameObject wallTile = Instantiate (wallTiles [0], pos, Quaternion.identity) as GameObject;
						wallTile.transform.parent = gameObject.transform.GetChild (1);
						wallTile.name = string.Format ("{0},{1}", x, y);
						Tiles [x, y] = wallTile;
						wallTile.GetComponent<TileScript> ().posX = x;
						wallTile.GetComponent<TileScript> ().posY = y;
					}
				}
			}
			Debug.Log (passagePoints.Count);
			foreach (Vector2[] pasPoints in passagePoints) {
				CreateTunnel (pasPoints [0], pasPoints [1]);
			}
		}
	}

	void ClearOutMap ()
	{
		List<List<Coord>> spaceRegions = GetRegions (0);
		int spaceTresholdSize = 30;
		List<SpaceRegion> remainedRegions = new List<SpaceRegion> ();
		foreach (List<Coord> region in spaceRegions) {
			if (region.Count < spaceTresholdSize) {
				foreach (Coord tile in region) {
					cave [tile.tileX, tile.tileY] = 1;
				}
			} else {
				remainedRegions.Add (new SpaceRegion (region, cave));
			}
		}
		ConnectClosestRegions (remainedRegions);
	}

	void ConnectClosestRegions (List<SpaceRegion> allRegions)
	{
		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		SpaceRegion bestRegionA = new SpaceRegion ();
		SpaceRegion bestRegionB = new SpaceRegion ();
		bool possibleConnection = false;

		foreach (SpaceRegion regionA in allRegions) {
			possibleConnection = false;

			foreach (SpaceRegion regionB in allRegions) {
				for (int tileIndexA = 0; tileIndexA < regionA.edgeTiles.Count; tileIndexA++) {
					if (regionA == regionB) {
						continue;
					}
					if (regionA.isConnected (regionB)) {
						possibleConnection = false;
						break;
					}
					for (int tileIndexB = 0; tileIndexB < regionB.edgeTiles.Count; tileIndexB++) {
						
						Coord tileA = regionA.edgeTiles [tileIndexA];
						Coord tileB = regionB.edgeTiles [tileIndexB];
						int distance = (int)(Mathf.Pow (tileA.tileX - tileB.tileX, 2) + Mathf.Pow (tileA.tileY - tileB.tileY, 2));
						if (distance < bestDistance || !possibleConnection) {
							bestDistance = distance;
							possibleConnection = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRegionA = regionA;
							bestRegionB = regionB;
						}
					}
				}
			}
			if (possibleConnection) {
				CreatePassage (bestRegionA, bestRegionB, bestTileA, bestTileB);
			}
		}
	}

	void CreatePassage (SpaceRegion regionA, SpaceRegion regionB, Coord tileA, Coord tileB)
	{
		SpaceRegion.ConnectRegions (regionA, regionB);
		Vector2[] points = { CoordToWorldPoint (tileA), CoordToWorldPoint (tileB) };
		//Debug.DrawLine (points [0], points [1], Color.red, 100f);
		passagePoints.Add (points);
	}

	void CreateTunnel (Vector2 startPos, Vector2 tarPos)
	{
		List<Transform> tiles = new List<Transform> ();
		RaycastHit2D hit = Physics2D.Raycast (startPos, -(startPos - tarPos).normalized, Vector2.Distance (startPos, tarPos));
		if (hit.collider != null) {
			hit.collider.enabled = false;
			tiles.Add (hit.transform);
			while (true) {
				hit = Physics2D.Raycast (startPos, -(startPos - tarPos).normalized);
				if (hit.transform.tag == "Wall") {
					hit.collider.enabled = false;
					tiles.Add (hit.transform);
				} else {
					break;
				}
			}
		} 
		foreach (Transform tile in tiles) {
			Instantiate (floorTiles [0], tile.position, Quaternion.identity);
			Destroy (tile.gameObject);
		}

	}

	Vector2 CoordToWorldPoint (Coord tile)
	{
		return new Vector2 (-width / 2 + .5f + tile.tileX + transform.position.x, -height / 2 + .5f + tile.tileY + transform.position.y);
	}

	List<List<Coord>> GetRegions (int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] isChecked = new int[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (isChecked [x, y] == 0 && cave [x, y] == tileType) {
					List<Coord> newRegion = GetRegionsTiles (x, y);
					regions.Add (newRegion);
					foreach (Coord tile in newRegion) {
						isChecked [tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}

	List<Coord> GetRegionsTiles (int startX, int startY)
	{

		List<Coord> tiles = new List<Coord> ();
		int[,] isChecked = new int[width, height];
		int tileType = cave [startX, startY];

		Queue<Coord> myQueue = new Queue<Coord> ();
		myQueue.Enqueue (new Coord (startX, startY));
		isChecked [startX, startY] = 1;

		while (myQueue.Count > 0) {
			Coord tile = myQueue.Dequeue ();
			tiles.Add (tile);
			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if ((x >= 0 && x < width && y >= 0 && y < height) && (x == tile.tileX || y == tile.tileY)) {
						if (isChecked [x, y] == 0 && cave [x, y] == tileType) {
							isChecked [x, y] = 1;
							myQueue.Enqueue (new Coord (x, y));
						}
					}
				}
			}
		}
		return tiles;
	}

	public struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord (int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}

	class SpaceRegion
	{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<SpaceRegion> connectedRegions;
		public int regionSize;

		public SpaceRegion ()
		{
		}

		public SpaceRegion (List<Coord> spaceTiles, int[,] cave)
		{
			tiles = spaceTiles;
			regionSize = tiles.Count;
			connectedRegions = new List<SpaceRegion> ();

			edgeTiles = new List<Coord> ();
			foreach (Coord tile in tiles) {
				for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
					for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
						if (x == tile.tileX || y == tile.tileY) {
							if (cave [x, y] == 1) {
								edgeTiles.Add (tile);
							}
						}
					}
				}
			}
		}

		public static void ConnectRegions (SpaceRegion regionA, SpaceRegion regionB)
		{
			regionA.connectedRegions.Add (regionB);
			regionB.connectedRegions.Add (regionA);
		}

		public bool isConnected (SpaceRegion otherRegion)
		{
			return connectedRegions.Contains (otherRegion);
		}
	}

}