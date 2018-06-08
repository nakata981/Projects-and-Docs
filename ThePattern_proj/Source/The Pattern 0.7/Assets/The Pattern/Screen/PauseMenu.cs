using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	
		private bool showGUI = false;
		private Rect MainMenu = new Rect (600, 0, Screen.width / 4, Screen.height / 4);
		void Start ()
		{
		}
		void Update ()
		{
				if (Input.GetKey (KeyCode.Escape)) {
						showGUI = true;
						Time.timeScale = 0;
				}
		}
		void OnGUI ()
		{
				if (showGUI == true) {
						Screen.showCursor = true;
						GUI.Window (0, MainMenu, TheMainMenu, "Pause Menu");
				}
		}
		void TheMainMenu (int mainMenuID)
		{
				if (GUILayout.Button ("Resume")) {
						Time.timeScale = 1;
						showGUI = false;
						Screen.showCursor = false;
				}
				if (GUILayout.Button ("Main Menu")) {
						Application.LoadLevel (0);
						Time.timeScale = 1;
				}
				if (GUILayout.Button ("Restart")) {
						Application.LoadLevel (1);
						Time.timeScale = 1;
						GridScript.GridSize.x = 5;
						GridScript.GridSize.z = 5;
						PlayerSet.Pickups = 1;
						ScoreSystem.GlobalScore = 0;
				}
				if (GUILayout.Button ("Quit")) {
						Application.Quit ();
				}
		
		}
}
