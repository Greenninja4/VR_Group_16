using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public Text timer;
    private float loadTime;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        timer.text = string.Format("{0:N2}", Time.timeSinceLevelLoad);
	}
}
