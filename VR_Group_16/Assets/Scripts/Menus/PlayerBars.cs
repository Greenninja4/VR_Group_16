using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBars : MonoBehaviour {
    // Health and stamina values
    public float maxHealth = 1000;
    public float maxStamina = 100;
    public float currentHealth = 1000;
    public float currentStamina = 100;
    private float staminaCooldown = 10;
    private float staminaCooldownDepleted = 1;
    public float tiredTimeStamp;
    public OVRInput.Controller controllerL;
    public OVRInput.Controller controllerR;

    // State variables
    bool damaged;
    bool tired;
    bool dead;
    
    // Bars
    public GameObject staminaInnerBar;
    public GameObject staminaOuterBar;
    public GameObject healthInnerBar;
    public GameObject healthOuterBar;

    public float vibe_time = 0.8f; //Seconds on the vibration
    private float vibe_time_remaining;

    // Damage audio source
    public AudioSource damageAudio;

    // Call on battle start - initializes health and stamina and states
    void Awake(){
        // Set initial values & update bars
        UpdateStaminaBar();
        StartHealthBar();
        // Set initial states
        damaged = false;
        tired = false;
        dead = false;

        vibe_time_remaining = 0f;
    }

    // Call on 
    void Update (){
        // Reset the damaged flag
        damaged = false;
        // Reset tired flag after given time
        if (tiredTimeStamp + staminaCooldownDepleted <= Time.time){
            tired = false;
        }
        // If not tired, replenish stamina at given rate
        if (!tired) {
            currentStamina += (Time.deltaTime / staminaCooldown) * maxStamina;
            if (currentStamina >= maxStamina){
                currentStamina = maxStamina;
            }
            UpdateStaminaBar();
        }

        if (vibe_time_remaining > 0)
        {
            vibe_time_remaining -= Time.deltaTime;
            OVRInput.SetControllerVibration(0.8f, 0.6f, controllerR);
            OVRInput.SetControllerVibration(0.8f, 0.6f, controllerL);
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, controllerR);
            OVRInput.SetControllerVibration(0, 0, controllerL);
            vibe_time_remaining = 0f;
        }
    }

    // Update the stamina bar gui
    void UpdateStaminaBar(){
        float staminaBarRatio = currentStamina / maxStamina;
        Vector3 outer = staminaOuterBar.transform.localScale;
        Vector3 outer_pos = staminaOuterBar.transform.position;
        Vector3 inner_pos = staminaInnerBar.transform.position;
        staminaInnerBar.transform.localScale = new Vector3(0.5f * outer.x, 0.9f * outer.y * staminaBarRatio, 0.5f * outer.z);
        staminaInnerBar.transform.position = new Vector3(outer_pos.x - 0.9f *(1 - staminaBarRatio), inner_pos.y, inner_pos.z);
    }

    void StartHealthBar()
    {
        float healthBarRatio = currentHealth / maxHealth;
        Vector3 outer = healthOuterBar.transform.localScale;
        Vector3 outer_pos = healthOuterBar.transform.position;
        Vector3 inner_pos = healthInnerBar.transform.position;
        healthInnerBar.transform.localScale = new Vector3(0.5f * outer.x, 0.9f * outer.y * healthBarRatio, 0.5f * outer.z);
        healthInnerBar.transform.position = new Vector3(outer_pos.x - 0.9f * (1 - healthBarRatio), inner_pos.y, inner_pos.z);
    }

    // Update the health bar gui
    void UpdateHealthBar(){
        float healthBarRatio = currentHealth / maxHealth;
        Vector3 outer = healthOuterBar.transform.localScale;
        Vector3 outer_pos = healthOuterBar.transform.position;
        Vector3 inner_pos = healthInnerBar.transform.position;
        healthInnerBar.transform.localScale = new Vector3(0.5f * outer.x, 0.9f * outer.y * healthBarRatio, 0.5f * outer.z);
        healthInnerBar.transform.position = new Vector3(outer_pos.x - 0.9f *(1 - healthBarRatio), inner_pos.y, inner_pos.z);
        vibe_time_remaining = vibe_time;
    }

    // Omae wa mou shindeiru
    void Death(){
        dead = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName:"GameOver");
    }

    // Take a given amount of damage
    public void TakeDamage (float amount){
        // Set damaged flag and play audio
        damaged = true;
        damageAudio.Play();
        // Update health (possible die)
        currentHealth -= amount;
        UpdateHealthBar();
        if (currentHealth <= 0 && !dead){
            currentHealth = 0;
            UpdateHealthBar();
            Death();
        }
    }

    // Use a given amount of stamina
    public void UseStamina(float amount){
        // Update stamina
        currentStamina -= amount;
        UpdateStaminaBar();
        // Run out stamina (bottoms out at 0) -> depleted cooldown time
        if (currentStamina < 0){
            currentStamina = 0;
            UpdateStaminaBar();
            tired = true;
            tiredTimeStamp = Time.time;
        }
    }

    public bool EnoughStamina(float amount){
        return (currentStamina > amount);
    }
}