using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class pauseMenu : NetworkBehaviour
{
	public Text winnerText;

    public static bool isPaused = false;
    public bool showScore = false;
    public Texture2D menu;
    public Drinker drinker;

    public GameObject pauseMenuUI;
    public GameObject scoreMenuUI;
    public GameObject waterLevelUI;

    public Image p1Score;
    public Image p2Score;
    public Image p3Score;

    public Image waterLevel;

    void Start()
    {
        pauseMenuUI = transform.GetChild(0).gameObject;
        scoreMenuUI = transform.GetChild(1).gameObject;
        waterLevelUI = transform.GetChild(2).gameObject;

        p1Score = scoreMenuUI.transform.GetChild(0).GetComponent<Image>();
        p2Score = scoreMenuUI.transform.GetChild(1).GetComponent<Image>();
        p3Score = scoreMenuUI.transform.GetChild(2).GetComponent<Image>();

		winnerText = scoreMenuUI.transform.GetChild (3).GetComponent<Text> ();
		winnerText.text = "";

        waterLevel = waterLevelUI.transform.GetChild(0).GetComponent<Image>();
        waterLevel.fillAmount = 0;

        p1Score.fillAmount = 0;
        p2Score.fillAmount = 0;
        p3Score.fillAmount = 0;

		StartCoroutine ( updateTime() );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Pause();
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Tab))
        {
            ShowTabScore();
        }
    }

    void Pause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
    }

    void ShowTabScore()
    {
        showScore = !showScore;
        updateScore();
        scoreMenuUI.SetActive(showScore);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    void updateScore()
    {
        Vector4 scores = SplatManagerSystem.instance.scores + new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        float totalScores = scores.x + scores.y + scores.z + scores.w;
        float yellowScore = (scores.x / totalScores);
        float redScore = (scores.y / totalScores);
        float greenScore = (scores.z / totalScores);
        float blueScore = (scores.w / totalScores);

		Debug.Log ("Total: " + totalScores);
        //Debug.Log("Press");
        //Debug.Log("Y" + yellowScore);
        //Debug.Log("R" + redScore);
        //Debug.Log("G" + greenScore);
        //Debug.Log("B" + blueScore);

        p1Score.fillAmount = yellowScore;
        p2Score.fillAmount = greenScore;
        p3Score.fillAmount = redScore;
    }

    public void updateWaterLevel(float change){
        waterLevel.fillAmount = change;
    }

	public void endGame()
	{
		updateScore ();
		Time.timeScale = 0;

		Vector4 scores = SplatManagerSystem.instance.scores + new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
		float totalScores = scores.x + scores.y + scores.z + scores.w;

		GameObject[] people = GameObject.FindGameObjectsWithTag ("Player");
		int numPlayers = people.Length;

		if (totalScores < 0.8 * numPlayers)
			winnerText.text = "THE MAN IS THE WINNER!";
		else {

			if (p1Score.fillAmount > p2Score.fillAmount && p1Score.fillAmount > p3Score.fillAmount)
				winnerText.text = "PLAYER 1 IS THE WINNER";
			if (p2Score.fillAmount > p1Score.fillAmount && p2Score.fillAmount > p3Score.fillAmount)
				winnerText.text = "PLAYER 2 IS THE WINNER";
			if (p3Score.fillAmount > p1Score.fillAmount && p3Score.fillAmount > p2Score.fillAmount)
				winnerText.text = "PLAYER 3 IS THE WINNER";
		}

		//scoreMenuUI.transform.GetChild (4).gameObject.SetActive (true);
		scoreMenuUI.SetActive (true);

	}

	IEnumerator updateTime()
	{
		//yield return new WaitForEndOfFrame ();
		while (true) {
			int time = (int)SplatManagerSystem.instance.gameTimer;
			int minutes = time / 60;
			int seconds = time % 60;
			winnerText.text = minutes.ToString () + ":" + seconds.ToString ();

			yield return new WaitForSeconds (1.0f);
		}
	}

}
