using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

	private Camera _cam;
	private Transform _camTransform;
	public Vector3 CameraHeight;
	private Vector3 _originalCamHeight = new Vector3(0,35,0);
	private bool _isLerping;
	private Vector3 _cameraStartPosition;
	private Vector3 _cameraEndPosition;
	private float _timeStartedLerp;
	private float _timeTakenToLerp;

	// Use this for initialization
	void Start () {
		CameraHeight = _originalCamHeight;
		_isLerping = false;
		_timeTakenToLerp = 1.0f;

		if (isLocalPlayer) {
			_cam = Camera.allCameras [0];
			_camTransform = _cam.transform;
			_camTransform.SetParent(transform);
			_camTransform.position = transform.position + CameraHeight;
			_camTransform.LookAt(transform);
			hideSpawnLocations ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate()
	{
		if (_isLerping) {
			float timeSinceStarted = Time.time - _timeStartedLerp;
			float percentageComplete = timeSinceStarted / _timeTakenToLerp;

			_camTransform.position = Vector3.Lerp (_cameraStartPosition, _cameraEndPosition, percentageComplete);

			if (percentageComplete >= 1.0f) {
				_isLerping = false;
			}
		} else {
			_camTransform.position = transform.position + CameraHeight;
		}
	}

	public
	void setSpawnCameraHeight(bool spawn)
	{
		if (spawn)
			CameraHeight = new Vector3 (0, 70, 0);
		else
			CameraHeight = _originalCamHeight;
	}

	public
	void beginLerp(Vector3 _endPos)
	{
		_isLerping = true;
		_timeStartedLerp = Time.time;

		_cameraStartPosition = _camTransform.position;
		_cameraEndPosition = _endPos;
	}

	public
	void showSpawnLocations()
	{
		Debug.Log(_cam.cullingMask);
		_cam.cullingMask = _cam.cullingMask | (1 << 8);
	}

	public
	void hideSpawnLocations()
	{
		_cam.cullingMask = _cam.cullingMask & ~(1 << 8);
	}
}
