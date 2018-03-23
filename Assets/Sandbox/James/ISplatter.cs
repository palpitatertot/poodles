using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplatter {
	void Splat();
    void SetChannel(Vector4 c);
    void SetColor(Vector4 c);
    void RegisterSplatter();
    void SetColors(List<Vector4> colors);
} 