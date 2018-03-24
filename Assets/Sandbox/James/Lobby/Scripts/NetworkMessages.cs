using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestDogColorsMsg : MessageBase
{
    public string hi;
}

public class DogColorsMsg : MessageBase
{
    public Vector4 Dog0Color;
    public Vector4 Dog1Color;
    public Vector4 Dog2Color;
}
