using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

    public float smoothTime = 0.3F;
    private Vector3 dampVelocity = Vector3.zero;

	private Camera _cam;
	private Transform _camTransform;
	public Vector3 CameraHeight;
    private Vector3 _spawnCameraHeight;
    private Vector3 _spawnCameraPosition;
	private Vector3 _originalCamHeight = new Vector3(0,7,0);
	private bool _isLerping;
    private bool _isSpawning;
	private Vector3 _cameraStartPosition;
	private Vector3 _cameraEndPosition;
	private float _timeStartedLerp;
	private float _timeTakenToLerp;
    private GameObject _floor;

	// Use this for initialization
	void Start () {
        
        CameraHeight = _originalCamHeight;
        //_spawnCameraHeight = CalculateSpawnCameraHeight();
        //_spawnCameraPosition = CalculateSpawnCameraPosition();
        //_spawnCameraPosition = new Vector3(_spawnCameraPosition.x, _spawnCameraHeight.y, _spawnCameraPosition.z);
		_isLerping = false;
		_timeTakenToLerp = 1.0f;

		if (isLocalPlayer) {
			_cam = Camera.allCameras [0];
            _cam.cullingMask = _cam.cullingMask & ~(1 << 10);
			_camTransform = _cam.transform;
			_camTransform.SetParent(transform);
			_camTransform.position = transform.position + CameraHeight;
			_camTransform.LookAt(transform);
			hideSpawnLocations ();
		}
	}

	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer){
            return;
        }
        if(_floor == null){
            _spawnCameraHeight = CalculateSpawnCameraHeight();
            _spawnCameraPosition = CalculateSpawnCameraPosition();
            _spawnCameraPosition = new Vector3(_spawnCameraPosition.x, _spawnCameraHeight.y, _spawnCameraPosition.z);
        }
        //Debug.Log("SpawnCamera " + _spawnCameraPosition);
	}

	void LateUpdate()
	{
        if (!isLocalPlayer)
        {
            return;
        }
        if (_isLerping)
        {
            //Debug.Log("Lerping");
            float timeSinceStarted = Time.time - _timeStartedLerp;
            float percentageComplete = timeSinceStarted / _timeTakenToLerp;

            _camTransform.position = Vector3.Lerp(_cameraStartPosition, _cameraEndPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
        else if (_isSpawning){
            _camTransform.position = _spawnCameraHeight;
		} else {
            float locVel = gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude;
            if (locVel < 10) {
                locVel = 10;
            }

             Vector3 newPos = transform.position + CameraHeight * Mathf.Min(35,locVel * (float).1);
            _camTransform.position = Vector3.SmoothDamp(_camTransform.position, newPos, ref dampVelocity, smoothTime);
            _camTransform.LookAt(transform);
		}
	}

	public
	void setSpawnCamera(bool spawn)
	{
        if (spawn)
        {
            _isSpawning = true;
            _cam.transform.position = _spawnCameraPosition;
        }
        else
            _isSpawning = false;
            _cam.transform.localPosition = Vector3.zero;
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
		//Debug.Log(_cam.cullingMask);
		_cam.cullingMask = _cam.cullingMask | (1 << 8);
	}

	public
	void hideSpawnLocations()
	{
		_cam.cullingMask = _cam.cullingMask & ~(1 << 8);
	}

    Vector3 CalculateSpawnCameraHeight(){
        _floor = GameObject.FindGameObjectWithTag("Floor");
        float x = _floor.transform.lossyScale.x;
        float y = _floor.transform.lossyScale.y;
        if (y > x)
        {
            x = y;
        }
        x *= 50f;
        float tanTheta = Mathf.Abs(_cam.fieldOfView * .5f);
        //Debug.Log("FOV" + tanTheta);
        tanTheta = Mathf.Tan(Mathf.Deg2Rad * tanTheta);
        //Debug.Log("width" + x + " height " + x / tanTheta);
        return new Vector3(0,x/tanTheta,0);
    }

    Vector3 CalculateSpawnCameraPosition()
    {
        _floor = GameObject.FindGameObjectWithTag("Floor");
        return _floor.transform.position;
    }
}
