using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class pauseMenu : MonoBehaviour
{
	public Text winnerText;
	public Text MessText;

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


	int numPlayers;

    void Start()
    {
		pauseMenuUI = transform.GetChild(0).gameObject;
		pauseMenuUI.SetActive (false);
        scoreMenuUI = transform.GetChild(1).gameObject;
		scoreMenuUI.SetActive (false);
        waterLevelUI = transform.GetChild(2).gameObject;
		waterLevelUI.SetActive (true);

        p1Score = scoreMenuUI.transform.GetChild(0).GetComponent<Image>();
        p2Score = scoreMenuUI.transform.GetChild(1).GetComponent<Image>();
        p3Score = scoreMenuUI.transform.GetChild(2).GetComponent<Image>();

		winnerText = scoreMenuUI.transform.GetChild (3).GetComponent<Text> ();
		winnerText.text = "";

		MessText = scoreMenuUI.transform.GetChild (4).GetComponent<Text> ();
		MessText.text = "Total Mess:";

        waterLevel = waterLevelUI.transform.GetChild(0).GetComponent<Image>();
        waterLevel.fillAmount = 0;

        p1Score.fillAmount = 1;
        p2Score.fillAmount = 1;
        p3Score.fillAmount = 1;

		GameObject[] people = GameObject.FindGameObjectsWithTag ("Player");
		int numPlayers = people.Length;

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
		waterLevelUI.SetActive (!showScore);
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
		Vector4 scores = SplatManagerSystem.instance.scores;// + new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        float totalScores = scores.x + scores.y + scores.z + scores.w;
        float yellowScore = (scores.x / totalScores);
        float redScore = (scores.y / totalScores);
        float greenScore = (scores.z / totalScores);
        float blueScore = (scores.w / totalScores);

		Debug.Log ("Total: " + totalScores);

		if (totalScores > 1.5) 
			scores = scores / totalScores;
		
		if(totalScores > 0.9 * numPlayers){
			MessText.text = "Total Mess: " + totalScores.ToString("n2") + "! The house has gone to the dogs!";
		} else {
			MessText.text = "Total Mess: " + totalScores.ToString("n2") + ". Man rules this roost.";
		}

		p1Score.rectTransform.sizeDelta = new Vector2(scores.x * Screen.width, p1Score.rectTransform.sizeDelta.y);

		p2Score.rectTransform.localPosition = new Vector3 (p1Score.rectTransform.localPosition.x + p1Score.rectTransform.sizeDelta.x, p2Score.rectTransform.localPosition.y, p2Score.rectTransform.localPosition.z);
		p2Score.rectTransform.sizeDelta = new Vector2(scores.y * Screen.width, p2Score.rectTransform.sizeDelta.y);

		p3Score.rectTransform.localPosition = new Vector3 (p2Score.rectTransform.localPosition.x + p2Score.rectTransform.sizeDelta.x, p3Score.rectTransform.localPosition.y, p3Score.rectTransform.localPosition.z);
		p3Score.rectTransform.sizeDelta = new Vector2(scores.z * Screen.width, p3Score.rectTransform.sizeDelta.y);

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

		if (totalScores < 0.9 * numPlayers)
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

	public
	void setColors(List<Vector4> colors)
	{
		p1Score.color = colors [0];
		p2Score.color = colors [1];
		p3Score.color = colors [2];
	}
		
	IEnumerator updateTime()
	{
		//yield return new WaitForEndOfFrame ();
		while (true) {
			int time = (int)SplatManagerSystem.instance.gameTimer;
			int minutes = time / 60;
			int seconds = time % 60;
			winnerText.text = minutes.ToString () + ":" + seconds.ToString ("D2");

			yield return new WaitForSeconds (1.0f);
		}
	}
}
