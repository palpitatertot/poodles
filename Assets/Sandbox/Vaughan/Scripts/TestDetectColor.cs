using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDetectColor : MonoBehaviour {

    public Texture2D colorTex;
    public RenderTexture splatTex;
    public RenderTexture RT256;
    public RenderTexture RT4;
    public Texture2D Tex4;

    public int sizeX;
    public int sizeY;

    private Material splatBlitMaterial;
    private int counter = 0;

    // Use this for initialization
    void Start () {


        splatTex = new RenderTexture(sizeX, sizeY, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        splatTex.Create();

        splatBlitMaterial = new Material(Shader.Find("Splatoonity/SplatBlit"));

        RT256 = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        RT256.autoGenerateMips = true;
        RT256.useMipMap = true;
        RT256.Create();
        RT4 = new RenderTexture(4, 4, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        RT4.Create();
        Tex4 = new Texture2D(4, 4, TextureFormat.ARGB32, false);
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.F))
        {
            Graphics.Blit(splatTex, RT256, splatBlitMaterial, 3);
            Graphics.Blit(RT256, RT4);

            RenderTexture.active = RT4;
            Tex4.ReadPixels(new Rect(0, 0, 4, 4), 0, 0);
            Tex4.Apply();            

            RaycastHit detectColorHit;

            if (Physics.Raycast(transform.position, Vector3.down, out detectColorHit, 100))
            {
                Renderer rend = detectColorHit.transform.GetComponent<Renderer>();
                MeshCollider meshCollider = detectColorHit.collider as MeshCollider;

                if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null)
                    return;                

                Texture2D tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = detectColorHit.textureCoord;

                Texture2D myTexture2D = new Texture2D(splatTex.width, splatTex.height);

                RenderTexture.active = splatTex;

                myTexture2D.ReadPixels(new Rect(0,0,splatTex.width, splatTex.height), 0, 0);
                
                myTexture2D.Apply();

                Color pixelColor = myTexture2D.GetPixel((int)pixelUV.x, (int)pixelUV.y);

                counter++;

                Debug.Log("Take " + counter + ": Pixel Color = " + pixelColor);
            }
        }

        /*
         *                            
         * INITIALIZE IN START FUNCTION                               
        RT256 = new RenderTexture (256, 256, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
		RT256.autoGenerateMips = true;
		RT256.useMipMap = true;
		RT256.Create ();
		RT4 = new RenderTexture (4, 4, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		RT4.Create ();
		Tex4 = new Texture2D (4, 4, TextureFormat.ARGB32, false);

        //IN UPDATE SCORES
        
		Graphics.Blit (splatTex, RT256, splatBlitMaterial, 3);
		Graphics.Blit (RT256, RT4);

		RenderTexture.active = RT4;
		Tex4.ReadPixels (new Rect (0, 0, 4, 4), 0, 0);
		Tex4.Apply ();
        */

    }
}
