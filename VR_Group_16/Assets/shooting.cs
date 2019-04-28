using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shooting : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
           
            nextFire = Time.time + fireRate;
            GameObject projectile =
             Instantiate(shot, shotSpawn.transform.position, transform.rotation);

            Rigidbody shotrb = projectile.GetComponent<Rigidbody>();
            shotrb.velocity = transform.forward * 80;

            float moveVertical = Input.GetAxis("Horizontal") + 10;
            float moveHorizontal = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            
        }

    }
}
