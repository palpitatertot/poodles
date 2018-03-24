using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour {

	public static bool isPaused = false;
	public Texture2D menu;

	public GameObject pauseMenuUI;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (isPaused)
				Resume ();
			else
				Pause();
		}
	}

	void Resume()
	{
		pauseMenuUI.SetActive (false);
		isPaused = false;
	}

	void Pause()
	{
		pauseMenuUI.SetActive (true);
		isPaused = true;
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene ("Lobby");
	}

	public void QuitGame()
	{
		Debug.Log ("Quitting Game");
		Application.Quit ();
	}


	void OnGui()
	{
		GUI.DrawTexture (new Rect (20, 20, menu.width, menu.height), menu);
	}
}
