using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMotion : MonoBehaviour {

    Transform player;

    
    private float speed = 3f, playerRadius = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer").transform;

    }

    // Use this for initialization
    void Start () {
        transform.LookAt(player.transform.position);
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - this.transform.position;
        
        if (Vector3.Distance(player.transform.position, transform.position) >
            playerRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        else
        {
            // make object disappear
            Destroy(gameObject);
        }
	}
}
