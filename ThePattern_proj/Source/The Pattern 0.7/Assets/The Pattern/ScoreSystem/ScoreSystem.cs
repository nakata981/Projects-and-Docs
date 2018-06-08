using UnityEngine;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
		public static float LevelScore;
		public static float GlobalScore;
		void Start ()
		{
				LevelScore = GridScript.GridSize.x * 6;
		}
    
		void Update ()
		{
		}
		void OnTriggerEnter (Collider col)
		{
				if (col.gameObject.tag == "MazePickup") {
						GlobalScore += 50;
				}
				if (col.gameObject.tag == "NextLevelObject") {
						GlobalScore += LevelScore;
				}
		}
		void OnGUI ()
		{
				GUI.Box (new Rect (0, 0, 100, 20), "Score: " + GlobalScore);
		}
}
