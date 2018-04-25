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
	private CameraController _cControl;
	private bool _selecting;
	private GameObject[] spawnLocations;
	private GameObject selectedLocation;
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
		_cControl = GetComponent<CameraController> ();

		spawnLocations = GameObject.FindGameObjectsWithTag ("respawnLocations");
		i = 0;
		selectedLocation = spawnLocations[i];
		selectedLocation.GetComponent<Renderer>().material.color = Color.red;

		//_cControl.hideSpawnLocations ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			DoFade ();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			_cControl.showSpawnLocations ();
		}
		if (Input.GetKeyUp (KeyCode.O)) {
			_cControl.hideSpawnLocations ();
		}

		if (_selecting) {
			if(Input.GetKeyDown(KeyCode.D))
			{
				j = i;
				i = i + 1;
				if (i > spawnLocations.Length - 1)
					i = 0;
				selectedLocation = spawnLocations[i];
				selectedLocation.GetComponent<Renderer>().material.color = Color.red;
				spawnLocations[j].GetComponent<Renderer>().material.color = Color.white;
			}

			else if(Input.GetKeyDown(KeyCode.A))
			{
				j = i;
				i = i - 1;
				if (i < 0)
					i = spawnLocations.Length - 1;
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

	public void DoFade()
	{

		StartCoroutine (FadeOutIn ());
	
	}

	IEnumerator FadeOutIn()
	{
		_inputH.setSpawning (true);
		fadeUI.SetActive (true);
		waterLevelUI.SetActive (false);

		//Fade To Black
		while (_fadeColor.a < 1) {
			_fadeColor.a += Time.deltaTime;
			fadeImage.color = _fadeColor;
			yield return null;
		}
			
		_cControl.showSpawnLocations ();
		_cControl.setSpawnCameraHeight (true);
		yield return new WaitForSeconds (1.5f);

		//Fade From Black
		while (_fadeColor.a > 0) {
			_fadeColor.a -= Time.deltaTime;
			fadeImage.color = _fadeColor;
			yield return null;
		}
		_selecting = true;

		while (_selecting) {

			yield return null;
		
		}

		_cControl.beginLerp (selectedLocation.transform.position + new Vector3 (0, 35, 0));
		_dControl.moveDog (selectedLocation.transform.position - new Vector3(0,5,0));
		_cControl.setSpawnCameraHeight (false);

		waterLevelUI.SetActive (true);
		fadeUI.SetActive (false);

		_cControl.hideSpawnLocations ();
		_inputH.setSpawning (false);

		yield return null;
	}




}
