using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMotion : MonoBehaviour {

    public float damagePerObject = 20;

    Transform player;
    GameObject statusBars;

    
    private float speed = 3f, playerRadius = 2f;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("MainPlayer").transform;
        statusBars = GameObject.FindGameObjectWithTag("StatusBars");
    }

    // Use this for initialization
    void Start () {
        transform.LookAt(player.transform.position);
    }

    // Update is called once per frame
    void Update () {
        //RaycastHit hit;
        //Vector3 rayDirection = player.transform.position - this.transform.position;
        speed += speed * Time.deltaTime * 0.00001f;
        
        if (Vector3.Distance(player.transform.position, transform.position) >
            playerRadius){
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        else{
            // make object disappear
            Destroy(gameObject);
            // Damage player and update status bars
            statusBars.GetComponent<PlayerBars>().TakeDamage(damagePerObject);
        }
	}
}
