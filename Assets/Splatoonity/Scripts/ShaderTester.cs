using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTester : MonoBehaviour {

    Renderer renderer;

    int counter = 0;
    int remainder = 2;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {	

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Before");          

            renderer.material.SetVector("_Dog0Color", new Vector4(1, 1, 0, 1));

            renderer.material.SetVector("_Dog1Color", new Vector4(1, 0, 1, 1));

            renderer.material.SetVector("_Dog2Color", new Vector4(1, 1, 1, 1));

            Debug.Log("After");          
        }

    }  
}
