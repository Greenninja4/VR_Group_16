using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBars : MonoBehaviour {
    // Health and stamina values
    public float maxHealth = 100;
    public float maxStamina = 100;
    public float currentHealth = 100;
    public float currentStamina = 100;
    public float staminaCooldown = 10;
    public float staminaCooldownDepleted = 3;
    public float tiredTimeStamp;

    // State variables
    bool damaged;
    bool tired;
    bool dead;
    
    // Bars
    public GameObject staminaInnerBar;
    public GameObject staminaOuterBar;
    public GameObject healthInnerBar;
    public GameObject healthOuterBar;

    // Damage audio source
    public AudioSource damageAudio;

    // Call on battle start - initializes health and stamina and states
    void Awake(){
        // Set initial values & update bars
        UpdateStaminaBar();
        UpdateHealthBar();
        // Set initial states
        damaged = false;
        tired = false;
        dead = false;
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
    }

    // Update the stamina bar gui
    void UpdateStaminaBar(){
        float staminaBarRatio = currentStamina / maxStamina;
        Vector3 outer = staminaOuterBar.transform.localScale;
        staminaInnerBar.transform.localScale = new Vector3(0.5f * outer.x, 0.9f * outer.y * staminaBarRatio, 0.5f * outer.z);
    }

    // Update the health bar gui
    void UpdateHealthBar(){
        float healthBarRatio = currentHealth / maxHealth;
        Vector3 outer = healthOuterBar.transform.localScale;
        healthInnerBar.transform.localScale = new Vector3(0.5f * outer.x, 0.9f * outer.y * healthBarRatio, 0.5f * outer.z);
    }

    // Omae wa mou shindeiru
    void Death(){
        dead = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName:"Achievements");
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

    public bool EnoughStamina(){
        return (currentStamina > 0);
    }
}