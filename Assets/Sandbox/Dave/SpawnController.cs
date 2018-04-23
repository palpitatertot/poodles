using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpawnController : NetworkBehaviour {

	public GameObject fadeUI;
	public GameObject waterLevelUI;

	public Canvas UICanvas;

	public Image fadeImage;

	private InputHandler _inputH;
	private Color fadeImageColor;


	// Use this for initialization
	void Start () {

		UICanvas = GameObject.Find ("PauseCanvas").GetComponent<Canvas>();

		waterLevelUI = UICanvas.transform.GetChild (2).gameObject;
		fadeUI = UICanvas.transform.GetChild (3).gameObject;

		fadeUI.SetActive (false);

		fadeImage = fadeUI.GetComponent<Image> ();

		fadeImageColor = fadeImage.color;

		_inputH = GetComponent<InputHandler> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			DoFade ();
		}
	}

	public void DoFade()
	{
		Debug.Log ("Entering Do Fade Out");
		StartCoroutine (FadeOutIn ());
		Debug.Log ("Leaving Do Fade In");
	}

	IEnumerator FadeOutIn()
	{
		_inputH.setSpawning (true);
		fadeUI.SetActive (true);
		waterLevelUI.SetActive (false);
		Debug.Log ("Color Alpha Before = " + fadeUI.GetComponent<Image>().color.a + "\n");
		while (fadeImageColor.a < 1) {
			fadeImageColor.a += Time.deltaTime;
			fadeImage.color = fadeImageColor;
			yield return null;
		}
		Debug.Log ("Color Alpha After = " + fadeUI.GetComponent<Image>().color.a + "\n");
		yield return new WaitForSeconds (1.5f);

		while (fadeImageColor.a > 0) {
			fadeImageColor.a -= Time.deltaTime;
			fadeImage.color = fadeImageColor;
			yield return null;
		}

		waterLevelUI.SetActive (true);
		fadeUI.SetActive (false);
		_inputH.setSpawning (false);

		yield return null;
	}
}
