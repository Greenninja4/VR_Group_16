using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisions : MonoBehaviour {
    private GameObject battle;

	// Use this for initialization
	void Start () {
        battle = GameObject.Find("Battle");
        Debug.Log(battle.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // On collision with certain element, destroy element
    void OnCollisionEnter(Collision collision)
    {
        if (this.tag == "Rock")
        {
            if (collision.gameObject.tag == "Airball")
            {
                battle.GetComponent<AchievementTracking>().Hit(collision.gameObject.tag);
                Destroy(collision.gameObject);
            }
        }

        else if (this.tag == "Airball")
        {
            if (collision.gameObject.tag == "Rock")
            {
                battle.GetComponent<AchievementTracking>().Hit(collision.gameObject.tag);
                Destroy(collision.gameObject);
            }
        }

        else if (this.tag == "Waterball")
        {
            if (collision.gameObject.tag == "Fireball")
            {
                battle.GetComponent<AchievementTracking>().Hit(collision.gameObject.tag);
                Destroy(collision.gameObject);
            }
        }

        else if (this.tag == "Fireball")
        {
            if (collision.gameObject.tag == "Waterball")
            {
                battle.GetComponent<AchievementTracking>().Hit(collision.gameObject.tag);
                Destroy(collision.gameObject);
            }
        }
    }
}
