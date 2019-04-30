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
                previousHit = PlayerPrefs.GetInt("AirMenuHits");
                PlayerPrefs.SetInt("AirMenuHits", previousHit + 1);
                break;

            case "Rock":
                previousHit = PlayerPrefs.GetInt("EarthMenuHits");
                PlayerPrefs.SetInt("EarthMenuHits", previousHit + 1);
                break;

            case "Fireball":
                previousHit = PlayerPrefs.GetInt("FireMenuHits");
                PlayerPrefs.SetInt("FireMenuHits", previousHit + 1);
                break;

            case "Waterball":
                previousHit = PlayerPrefs.GetInt("WaterMenuHits");
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
                previousPlayTime = PlayerPrefs.GetFloat("AirMenuTime");
                PlayerPrefs.SetFloat("AirMenuTime", previousPlayTime + time);
                break;

            case "Water":
                previousPlayTime = PlayerPrefs.GetFloat("WaterMenuTime");
                PlayerPrefs.SetFloat("WaterMenuTime", previousPlayTime + time);
                break;

            case "Fire":
                previousPlayTime = PlayerPrefs.GetFloat("FireMenuTime");
                PlayerPrefs.SetFloat("FireMenuTime", previousPlayTime + time);
                break;

            case "Earth":
                previousPlayTime = PlayerPrefs.GetFloat("EarthMenuTime");
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
                previousShot = PlayerPrefs.GetInt("AirMenuShots");
                PlayerPrefs.SetInt("AirMenuShots", previousShot + 1);
                break;

            case "Rock":
                previousShot = PlayerPrefs.GetInt("EarthMenuShots");
                PlayerPrefs.SetInt("EarthMenuShots", previousShot + 1);
                break;

            case "Fireball":
                previousShot = PlayerPrefs.GetInt("FireMenuShots");
                PlayerPrefs.SetInt("FireMenuShots", previousShot + 1);
                break;

            case "Waterball":
                previousShot = PlayerPrefs.GetInt("WaterMenuShots");
                PlayerPrefs.SetInt("WaterMenuShots", previousShot + 1);
                break;
        }
        PlayerPrefs.Save();
    }

}
