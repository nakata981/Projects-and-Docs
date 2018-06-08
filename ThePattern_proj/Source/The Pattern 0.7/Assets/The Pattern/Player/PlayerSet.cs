using UnityEngine;
using System.Collections;

public class PlayerSet : MonoBehaviour
{
		private bool puzzle = false;
		public static int Pickups = (int)GridScript.GridSize.x / 5;
		void Start ()
		{
				float randomValue = Random.value;
				float chance = (int)GridScript.GridSize.x / 25f;
				Screen.showCursor = false;
				if (randomValue < chance) {
						puzzle = true;
				}
				Debug.Log (puzzle);
				//Set the player's location at the first(green) cell
				gameObject.transform.position = new Vector3 (GameObject.FindWithTag ("FirstCell").transform.position.x, 1, GameObject.FindWithTag ("FirstCell").transform.position.z);
		}

		void OnGUI ()
		{
				string pickupText = "Pickups left: " + Pickups;
				GUI.Box (new Rect (Screen.width - 150, 0, 130, 20), pickupText);
		}

		void OnTriggerEnter (Collider col)
		{
				if (col.gameObject.tag == "NextLevelObject" && Pickups == 0 && puzzle == false) {
						Destroy (col.gameObject);
						Application.LoadLevel (1);
						//Making the level with bigger after the restart
						GridScript.GridSize.x += 5;
						GridScript.GridSize.z += 5;
						Pickups = (int)GridScript.GridSize.x / 5;
				} else if (col.gameObject.tag == "NextLevelObject" && Pickups == 0 && puzzle) {
						Destroy (col.gameObject);
						Destroy (GameObject.FindWithTag ("Beacon"));
						Destroy (GameObject.FindWithTag ("FinalCell"));
				}
				if (col.gameObject.tag == "MazePickup") {
						Pickups--;
						Destroy (col.gameObject);
				}
				if (col.gameObject == GameObject.Find ("Puzzle1Object")) {
						Destroy (col.gameObject);
						Application.LoadLevel (1);
						//Making the level with bigger after the restart
						GridScript.GridSize.x = 5;
						GridScript.GridSize.z = 5;
						Pickups = (int)GridScript.GridSize.x / 5;
				}
		}
		void Update ()
		{
		}
}