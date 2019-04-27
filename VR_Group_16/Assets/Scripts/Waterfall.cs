using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour {

    public float WF_speed = 0.75f;
    public Renderer WF_renderer;

	// Use this for initialization
	void Start () {
        WF_renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float TextureOffset = Time.time * WF_speed;
        WF_renderer.material.SetTextureOffset("_MainTex", new Vector2(-TextureOffset, 0));
	}
}
