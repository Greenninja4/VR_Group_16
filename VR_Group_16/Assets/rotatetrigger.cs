using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatetrigger : MonoBehaviour {

    // Use this for initialization
    public GameObject triggerObject;
    public Transform target;
    public GameObject messageCanvas;
    bool triggerornot = true;

    void OnTriggerEnter(Collider triggerObject)
    {
        if ( triggerornot)
        {
            Debug.Log("collide happen with player");
            triggerornot = false;
            transform.LookAt(target);
            messageCanvas.SetActive(true);

        }
    }
   
}
