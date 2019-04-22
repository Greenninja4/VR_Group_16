using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooting : MonoBehaviour {

    // Use this for initialization
    public OVRInput.Controller controller;
    public Transform laserEnd, laserBegin;
    private LineRenderer laserLine;
    public string selectedItem;
    private float range = 100f, speed = 20f;

    private float nextFire, trigger, fireRate = .1f;
    public GameObject[] projectiles;
    public int elementIndex = 0;
    private int projectileLifetime = 20;


    void Start () {
        laserLine = GetComponent<LineRenderer>();
        laserLine.material.color = Color.yellow;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray laserRay = new Ray(laserBegin.position, laserBegin.transform.forward);
        laserLine.SetPosition(0, laserBegin.position);

        Vector3 rayDirection = laserEnd.position - laserBegin.position;

        if (Physics.Raycast(laserRay, out hit, 100))
        {
            laserLine.SetPosition(1, hit.point);
            selectedItem = hit.collider.name;
        }
        else
        {
            laserLine.SetPosition(1, laserBegin.position + (rayDirection * range));
        }

        trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller); ;


        if (trigger > 0.5 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject newProjectile = Instantiate(projectiles[elementIndex], laserEnd.position, laserEnd.rotation);
        
            newProjectile.GetComponent<Rigidbody>().velocity = rayDirection * speed;
            Destroy(newProjectile, projectileLifetime);

        }

        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            switch (elementIndex)
            {
                case 0:
                    elementIndex = 1;
                    laserLine.material.color = Color.blue;
                    break;
                case 1:
                    elementIndex = 2;
                    laserLine.material.color = Color.green;
                    break;
                case 2:
                    elementIndex = 3;
                    laserLine.material.color = Color.red;
                    break;
                case 3:
                    elementIndex = 0;
                    laserLine.material.color = Color.yellow;
                    break;
                default:
                    elementIndex = 0;
                    laserLine.material.color = Color.yellow;
                    break;
            }
        }


        if (OVRInput.GetDown(OVRInput.Button.Two, controller))
        {
            switch (elementIndex)
            {
                case 0:
                    elementIndex = 3;
                    laserLine.material.color = Color.red;
                    break;
                case 1:
                    elementIndex = 0;
                    laserLine.material.color = Color.yellow;
                    break;
                case 2:
                    elementIndex = 1;
                    laserLine.material.color = Color.blue;
                    break;
                case 3:
                    elementIndex = 2;
                    laserLine.material.color = Color.green;
                    break;
                default:
                    elementIndex = 0;
                    laserLine.material.color = Color.yellow;
                    break;
            }
        }


    }

}
