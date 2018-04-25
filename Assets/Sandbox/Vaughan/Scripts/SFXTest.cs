using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTest : MonoBehaviour {

    private AudioSource[] sfx;

	// Use this for initialization
	void Start () {
        sfx = GetComponents<AudioSource>();        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Alpha1)) //bark 1
        {
            PlaySound(sfx[0]);
        }

        if (Input.GetKey(KeyCode.Alpha2)) //bark 2
        {
            PlaySound(sfx[1]);
        }

        if (Input.GetKey(KeyCode.Alpha3)) //wet splat
        {
            PlaySound(sfx[2]);
        }

        if (Input.GetKey(KeyCode.Alpha4)) //water dropping
        {
            PlaySound(sfx[3]);
        }

        if (Input.GetKey(KeyCode.Alpha5)) //gross splat
        {
            PlaySound(sfx[4]);
        }

        if (Input.GetKey(KeyCode.Alpha6)) //dog walking
        {
            PlaySound(sfx[5]);
        }

        if (Input.GetKey(KeyCode.Alpha7)) //bark 3
        {
            PlaySound(sfx[6]);
        }

        if (Input.GetKey(KeyCode.Alpha8)) //whimper
        {
            PlaySound(sfx[7]);
        }

        if (Input.GetKey(KeyCode.Alpha9)) //manwalk 1
        {
            PlaySound(sfx[8]);
        }

        if (Input.GetKey(KeyCode.Alpha0)) //manwalk 2
        {
            PlaySound(sfx[9]);
        }

        if (Input.GetKey(KeyCode.Q)) //hit plexiglass
        {
            PlaySound(sfx[10]);
        }

        if (Input.GetKey(KeyCode.W)) //sink
        {
            PlaySound(sfx[11]);
        }

        if (Input.GetKey(KeyCode.E)) //spray 1
        {
            PlaySound(sfx[12]);
        }

        if (Input.GetKey(KeyCode.R)) //spray 2
        {
            PlaySound(sfx[13]);
        }

        if (Input.GetKey(KeyCode.T)) //whining
        {
            PlaySound(sfx[14]);
        }

        if (Input.GetKey(KeyCode.Y)) //rub thumb
        {
            PlaySound(sfx[15]);
        }

        if (Input.GetKey(KeyCode.U)) //toilet 1
        {
            PlaySound(sfx[16]);
        }

        if (Input.GetKey(KeyCode.I)) //toilet 2
        {
            PlaySound(sfx[17]);
        }
    }

    private void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(1.0f, 1.2f);
        source.Play();
    }

}