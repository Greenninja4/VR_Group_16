using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerpicture : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collider detect with ");
        Debug.Log(other);
    }

}
