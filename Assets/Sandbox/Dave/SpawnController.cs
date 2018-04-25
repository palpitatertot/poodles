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

	private Camera _cam;
	private InputHandler _inputH;
	private Color _fadeColor;
	private DogController _dControl;
	private bool _selecting;
	private GameObject[] spawnLocations;
	GameObject selectedLocation;
	public int i, j;


	// Use this for initialization
	void Start () {

		UICanvas = GameObject.Find ("PauseCanvas").GetComponent<Canvas>();

		waterLevelUI = UICanvas.transform.GetChild (2).gameObject;
		fadeUI = UICanvas.transform.GetChild (3).gameObject;
		fadeUI.SetActive (false);

		fadeImage = fadeUI.GetComponent<Image> ();
		_fadeColor = fadeImage.color;

		_inputH = GetComponent<InputHandler> ();
		_cam = GetComponentInChildren<Camera> ();
		_dControl = GetComponent<DogController>();

		spawnLocations = GameObject.FindGameObjectsWithTag ("respawnLocations");
		Debug.Log ("SPawn Location length: " + spawnLocations.Length);
		i = 0;

		hideSpawnLocations ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			BeginRespawn ();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			showSpawnLocations ();
		}
		if (Input.GetKeyUp (KeyCode.O)) {
			hideSpawnLocations ();
		}

		if (_selecting) {
			//Debug.Log ("In while loop");
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				j = i;
				i = i + 1;
				if (i > spawnLocations.Length - 1)
					i = 0;
				//Debug.Log ("I from D: " + i);
				selectedLocation = spawnLocations[i];
				selectedLocation.GetComponent<Renderer>().material.color = Color.red;
				spawnLocations[j].GetComponent<Renderer>().material.color = Color.white;
			}

            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				j = i;
				i = i - 1;
				if (i < 0)
					i = spawnLocations.Length - 1;
				//Debug.Log ("I from A: " + i);
				selectedLocation = spawnLocations[i];
				selectedLocation.GetComponent<Renderer>().material.color = Color.red;
				spawnLocations [j].GetComponent<Renderer>().material.color = Color.white;
			}

			else if (Input.GetKey (KeyCode.Space)) {
				_selecting = false;
			}
			//Debug.Log ("About to yield");
		}
	}

	public void BeginRespawn()
	{

		StartCoroutine (FadeOutIn ());
	
	}

	IEnumerator FadeOutIn()
	{
		_inputH.setSpawning (true);
		fadeUI.SetActive (true);
		waterLevelUI.SetActive (false);

		//Debug.Log ("Color Alpha Before = " + fadeUI.GetComponent<Image>().color.a + "\n");
		while (_fadeColor.a < 1) {
			_fadeColor.a += Time.deltaTime;
			fadeImage.color = _fadeColor;
			yield return null;
		}
		//Debug.Log ("Color Alpha After = " + fadeUI.GetComponent<Image>().color.a + "\n");

		showSpawnLocations ();
		_dControl.setSpawnCameraHeight (true);
		yield return new WaitForSeconds (1.5f);

		while (_fadeColor.a > 0) {
			_fadeColor.a -= Time.deltaTime;
			fadeImage.color = _fadeColor;
			yield return null;
		}
		_selecting = true;

		while (_selecting) {

			yield return null;
		
		}
			
		Debug.Log ("Out of Whiel LOOP");

		waterLevelUI.SetActive (true);
		fadeUI.SetActive (false);

		yield return new WaitForSeconds (1.0f);
		_dControl.setSpawnCameraHeight (false);
		hideSpawnLocations ();
		_inputH.setSpawning (false);

		yield return null;
	}


	void showSpawnLocations()
	{
		Debug.Log(_cam.cullingMask);
		_cam.cullingMask = _cam.cullingMask | (1 << 8);
	}

	void hideSpawnLocations()
	{
		_cam.cullingMask = _cam.cullingMask & ~(1 << 8);
	}

}
