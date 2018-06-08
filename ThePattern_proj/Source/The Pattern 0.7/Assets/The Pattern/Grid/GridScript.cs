using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class GridScript : MonoBehaviour
{
		public Transform CellPrefab;
		public static Vector3 GridSize = new Vector3 (5, 1, 5);
		public Transform[,] Grid;
		public Transform PickupPrefab;
		public Transform WallPrefab;
		private bool showGUI = false;
		public Texture2D LoadingScreenTexture;
		public Material PathMaterial;
		public Material GridWallMaterial;

		void Start ()
		{
				CreateGrid ();
				SetRandomNumbers ();
				SetAdjacents ();
				SetStart ();
				//Start at the left corner(0,0)
				//FindNext looks for the lowest weight adjacent
				// Also ads and removes cells from the Set
				//FindNext also will invoke itself as soon as it
				// finishes, allowing it to loop indefinitely until the maze is done
				FindNext ();
		}
	
		void CreateGrid ()
		{
				//Assigning the grid size in the inspector
				Grid = new Transform[(int)GridSize.x, (int)GridSize.z];

				// This will create each cell by looping from x = 0 while x < Size.x,
				//Creating the cells
				for (int x = 0; x < GridSize.x; x++) {
						for (int z = 0; z < GridSize.z; z++) {
								Transform newCell;
								//A new CellPrefab is Instantiated at the correct location
								newCell = (Transform)Instantiate (CellPrefab, new Vector3 (x, 0, z), Quaternion.identity);
								//Renaming the cells "(x,0,z)"
								newCell.name = string.Format ("({0},0,{1})", x, z);
								//Peranting the cells to the grid
								newCell.parent = transform;		//Grid[,](the script) keeps track of all of the cells' locations
								newCell.GetComponent<CellScript> ().Position = new Vector3 (x, 0, z);
								// We add the newCell to the appropriate location in the Grid array.
								Grid [x, z] = newCell;
						}
				}
				Transform newWall;
				WallPrefab.transform.localScale = new Vector3 (1, 1, GridSize.z + 1f);
				newWall = (Transform)Instantiate (WallPrefab, new Vector3 (-1, 1, GridSize.z / 2f), Quaternion.identity);
				newWall = (Transform)Instantiate (WallPrefab, new Vector3 (GridSize.x / 2f, 1, -1), new Quaternion (0, 90, 0, 90));
				newWall = (Transform)Instantiate (WallPrefab, new Vector3 (GridSize.x, 1, GridSize.z / 2f), Quaternion.identity);
				newWall = (Transform)Instantiate (WallPrefab, new Vector3 (GridSize.x / 2f, 1, GridSize.z), new Quaternion (0, -90, 0, 90));
				//Centering the camera
				//Camera.mainCamera.transform.position = Grid[(int)(GridSize.x/2f),(int)(GridSize.z/2f)].position + Vector3.up*20f;
				//Camera FOV
				//Camera.mainCamera.orthographicSize = Mathf.Max(GridSize.x, GridSize.z);
		}

		void SetRandomNumbers ()
		{
				//ForEach cell in the Grid gameObject:
				foreach (Transform child in transform) {
						int weight = Random.Range (0, 5);
						//Assign that number to both the cell's text and Weight variable in the CellScript.
						child.GetComponentInChildren<TextMesh> ().text = weight.ToString ();
						child.GetComponent<CellScript> ().Weight = weight;
				}
		}
	
		void SetAdjacents ()
		{
				for (int x = 0; x < GridSize.x; x++) {
						for (int z = 0; z < GridSize.z; z++) {
								//Create a new variable to be the
								// cell at position x, z.
								Transform cell;
								cell = Grid [x, z];
								//Create a new CellScript variable to
								// hold the cell's script component.
								CellScript cScript = cell.GetComponent<CellScript> ();
								//Detecting the cells' adjacents
								if (x - 1 >= 0) {
										cScript.Adjacents.Add (Grid [x - 1, z]);
								}
								if (x + 1 < GridSize.x) {
										cScript.Adjacents.Add (Grid [x + 1, z]);
								}
								if (z - 1 >= 0) {
										cScript.Adjacents.Add (Grid [x, z - 1]);
								}
								if (z + 1 < GridSize.z) {
										cScript.Adjacents.Add (Grid [x, z + 1]);
								}
								//Sort adjacents in ascending order
								cScript.Adjacents.Sort (SortByLowestWeight);
						}
				}
		}

		int SortByLowestWeight (Transform inputA, Transform inputB)
		{
				int a = inputA.GetComponent<CellScript> ().Weight; //a's weight
				int b = inputB.GetComponent<CellScript> ().Weight; //b's weight
				return a.CompareTo (b);
		}

		//Creating the Set(group) of adjacents
		public List<Transform> Set;

		// Note: Multiple entries of the same cell
		//  will not appear as duplicates.
		//  (Some adjacent cells will be next to
		//  two or three or four other Set cells).
		//  They are only recorded in the AdjSet once.
		public List<List<Transform>> AdjSet; //List of Lists
	
		void SetStart ()
		{
				Set = new List<Transform> ();
				AdjSet = new List<List<Transform>> ();
				for (int i = 0; i < 10; i++) {
						AdjSet.Add (new List<Transform> ());	
				}
				//Starting cell(green)
				Grid [0, 0].renderer.material.color = Color.green;
				Grid [0, 0].tag = "FirstCell";
				AddToSet (Grid [0, 0]);
		}
	
		void AddToSet (Transform toAdd)
		{
				//Adds the toAdd object to the Set.
				// The toAdd transform is sent as a parameter.
				Set.Add (toAdd);
				//For every adjacent next to the toAdd object:
				foreach (Transform adj in toAdd.GetComponent<CellScript>().Adjacents) {
						//Add one to the adjacent's cellScript's AdjacentsOpened
						adj.GetComponent<CellScript> ().AdjacentsOpened++;
						//If
						// a) The Set does not contain the adjacent
						//   (cells in the Set are not valid to be entered as
						//   adjacentCells as well).
						//  and
						// b) The AdjSet does not already contain the adjacent cell.
						// then..
						if (!Set.Contains (adj) && !(AdjSet [adj.GetComponent<CellScript> ().Weight].Contains (adj))) {
								//.. Add this new cell into the proper AdjSet sub-list.
								AdjSet [adj.GetComponent<CellScript> ().Weight].Add (adj);
						}
				}
		}
	
		void FindNext ()
		{
				//Creating a transform variable to stor the next cell
				Transform next;
				// While: The proposed next gameObject's AdjacentsOpened <= 2 
				do {
						//We'll initially assume that each sub-list of AdjSet is empty
						// and try to prove that assumption false in the for loop.
						bool empty = true;
						//Keeping track of the lowest list
						int lowestList = 0;
						for (int i = 0; i < 10; i++) {
								lowestList = i;
								if (AdjSet [i].Count > 0) {
										empty = false;
										break;
								}
						}
						//Checking if the maze is done
						if (empty) { 
								//Keeping track of the time needed
								Debug.Log ("We're Done, " + Time.timeSinceLevelLoad + " seconds taken"); 
								//Stopping the search for new cells
								CancelInvoke ("FindNext");
								//Stop showing the GUI LoadingScreen
								showGUI = false;
								//Marking the last cell added
								Set [Set.Count - 1].renderer.material.color = Color.red;
								Set [Set.Count - 1].tag = "FinalCell";
								Pickups ();
								//Making the cells from the Set move up and turn black(so they can act as walls)
								foreach (Transform cell in Grid) {
										/*if (Set.Contains (cell) && !GameObject.FindWithTag ("FirstCell") && !(GameObject.FindWithTag ("FinalCell"))) {
												cell.renderer.material = PathMaterial;
										}*/
										if (!Set.Contains (cell)) {
												cell.Translate (Vector3.up);
												cell.renderer.material = GridWallMaterial;
										}
								}
								return;
						}
						//If we did not finish, then we continue with the next SubList
						next = AdjSet [lowestList] [0];
						//When a cell is added to the Set we romove it from the AdjSet
						AdjSet [lowestList].Remove (next);
				} while(next.GetComponent<CellScript>().AdjacentsOpened >= 2);
				//The rest of the cells(the floor) are white
				next.renderer.material = PathMaterial;
				//We add this 'next' transform to the Set our function.
				AddToSet (next);
				//Continue finding cell
				Invoke ("FindNext", 0);
				//Show GUI LoadingScreen
				showGUI = true;
		}
		void Pickups ()
		{
				Transform pickup;
				Transform pickupCell;
				for (int i = 0; i < GridSize.x / 5; i++) {
						pickupCell = Set [Random.Range (0, Set.Count)];
						pickup = (Transform)Instantiate (PickupPrefab, new Vector3 (pickupCell.transform.position.x, 1, pickupCell.transform.position.z), Quaternion.identity);
				}
		}
		//Creating the GUI LoadingScreen
		void OnGUI ()
		{
				if (showGUI) {
						GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), LoadingScreenTexture);
				}
		}

		void Update ()
		{

		}
}

