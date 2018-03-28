using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour {

	public static bool isPaused = false;
	public bool showScore = false;
	public Texture2D menu;

	public GameObject pauseMenuUI;
	public GameObject scoreMenuUI;

	public Image p1Score;
	public Image p2Score;
	public Image p3Score;

	void Start()
	{
        pauseMenuUI = transform.GetChild(0).gameObject;
        scoreMenuUI = transform.GetChild(1).gameObject;

		p1Score = scoreMenuUI.transform.GetChild(0).GetComponent<Image>();
		p2Score = scoreMenuUI.transform.GetChild(1).GetComponent<Image>();
		p3Score = scoreMenuUI.transform.GetChild(2).GetComponent<Image>();

		p1Score.fillAmount = 0;
		p2Score.fillAmount = 0;
		p3Score.fillAmount = 0;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}

		if (Input.GetKeyDown (KeyCode.Tab) || Input.GetKeyUp(KeyCode.Tab)) {
			ShowTabScore ();
		}
	}

	void Resume()
	{
		pauseMenuUI.SetActive (false);
		isPaused = false;
	}

	void Pause()
	{
		isPaused = !isPaused;
		pauseMenuUI.SetActive (isPaused);
	}

	void ShowTabScore()
	{
		showScore = !showScore;
		updateScore ();
		scoreMenuUI.SetActive (showScore);
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

	void updateScore()
	{
		Vector4 scores = SplatManagerSystem.instance.scores + new Vector4(0.1f,0.1f,0.1f,0.1f);
		float totalScores = scores.x + scores.y + scores.z + scores.w;
		float yellowScore =( scores.x / totalScores );
		float redScore = ( scores.y / totalScores );
		float greenScore = ( scores.z / totalScores );
		float blueScore = ( scores.w / totalScores );

		Debug.Log ("Press");
		Debug.Log("Y"+yellowScore);
		Debug.Log ("R" + redScore);
		Debug.Log ("G" + greenScore);
		Debug.Log ("B" + blueScore);

		p1Score.fillAmount = yellowScore;
		p2Score.fillAmount = greenScore;
		p3Score.fillAmount = redScore;
	}

}
