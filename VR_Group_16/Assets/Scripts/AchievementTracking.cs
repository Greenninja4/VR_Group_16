using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTracking : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Hit(string tag)
    {
        int previousHit;
        switch (tag)
        {
            case "Airball":
                Debug.Log("Hit a :" + tag);
                previousHit = PlayerPrefs.GetInt("AirMenuHits", 0);
                PlayerPrefs.SetInt("AirMenuHits", previousHit + 1);
                break;

            case "Rock":
                Debug.Log("Hit a :" + tag);

                previousHit = PlayerPrefs.GetInt("EarthMenuHits", 0);
                PlayerPrefs.SetInt("EarthMenuHits", previousHit + 1);
                break;

            case "Fireball":
                Debug.Log("Hit a :" + tag);

                previousHit = PlayerPrefs.GetInt("FireMenuHits", 0);
                PlayerPrefs.SetInt("FireMenuHits", previousHit + 1);
                break;

            case "Waterball":
                Debug.Log("Hit a :" + tag);

                previousHit = PlayerPrefs.GetInt("WaterMenuHits", 0);
                PlayerPrefs.SetInt("WaterMenuHits", previousHit + 1);
                break;
        }
        PlayerPrefs.Save();
    }

    public void playTime(string element, float time)
    {
        float previousPlayTime;
        switch (element)
        {
            case "Air":
                previousPlayTime = PlayerPrefs.GetFloat("AirMenuTime", 0);
                PlayerPrefs.SetFloat("AirMenuTime", previousPlayTime + time);
                break;

            case "Water":
                previousPlayTime = PlayerPrefs.GetFloat("WaterMenuTime", 0);
                PlayerPrefs.SetFloat("WaterMenuTime", previousPlayTime + time);
                break;

            case "Fire":
                previousPlayTime = PlayerPrefs.GetFloat("FireMenuTime", 0);
                PlayerPrefs.SetFloat("FireMenuTime", previousPlayTime + time);
                break;

            case "Earth":
                previousPlayTime = PlayerPrefs.GetFloat("EarthMenuTime", 0);
                PlayerPrefs.SetFloat("EarthMenuTime", previousPlayTime + time);
                break;
        }
        PlayerPrefs.Save();
    }
    public void Shot(string tag)
    {
        int previousShot;
        switch (tag)
        {
            case "Airball":
                previousShot = PlayerPrefs.GetInt("AirMenuShots", 0);
                PlayerPrefs.SetInt("AirMenuShots", previousShot + 1);
                break;

            case "Rock":
                previousShot = PlayerPrefs.GetInt("EarthMenuShots", 0);
                PlayerPrefs.SetInt("EarthMenuShots", previousShot + 1);
                break;

            case "Fireball":
                previousShot = PlayerPrefs.GetInt("FireMenuShots", 0);
                PlayerPrefs.SetInt("FireMenuShots", previousShot + 1);
                break;

            case "Waterball":
                previousShot = PlayerPrefs.GetInt("WaterMenuShots", 0);
                PlayerPrefs.SetInt("WaterMenuShots", previousShot + 1);
                break;
        }
        PlayerPrefs.Save();
    }

}
