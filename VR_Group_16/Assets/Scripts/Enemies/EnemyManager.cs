using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject[] enemies;
    private float spawnTime = 4f;
    public Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
        //first spawnTime is wait in the beginning
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemies.Length);

        Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
