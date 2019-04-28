using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatetrigger : MonoBehaviour {

    // Use this for initialization
    public GameObject triggerObject;
    public Transform target;
    public GameObject messageCanvas;
    public GameObject messageCanvas1;
    public GameObject messageCanvas2;

    public GameObject picture1;
    public GameObject picture2;
    public GameObject picture3;

    bool triggerornot = true;

    void OnTriggerEnter(Collider triggerObject)
    {
        if ( triggerornot)
        {
            Debug.Log("collide happen with player");
            triggerornot = false;
            transform.LookAt(target);
            messageCanvas.SetActive(true);
            messageCanvas1.SetActive(true);
            messageCanvas2.SetActive(true);

            picture1.SetActive(true);
            picture2.SetActive(true);
            picture3.SetActive(true);

        }
    }
   
}
