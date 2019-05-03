using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrow : MonoBehaviour {

    //Intialize external public elements
    public OVRInput.Controller controller;
    public GameObject[] projectiles;
    public GameObject player;
    GameObject statusBars;

    //Initialize private elements
    private int elementIndex;
    private float thrust;
    private GameObject selectedItem;
    private GameObject trackingSpace;
    private GameObject fireball;
    private Vector3 controller_pos;
    private Vector3 end_pos;
    private Vector3 forward;
    private Quaternion controller_rot;

    //Initialize public constants
    public float trigger_thresh = 0.5f; //0 to 1, threshold for trigger activation
    public float float_dist = 2.0f; //End distance (in m) away from hand
    public float thrust_const = 175.0f; //Constant of thrust
    public int projectileLifetime = 20;
    public float staminaRequired = 20;
    public float vibe_time = 0.2f; //Seconds on the vibration
    private float vibe_time_remaining;

    private GameObject battle;


    // Use this for initialization
    void Start () {
        battle = this.transform.root.gameObject;


        // Get left and right hand game objects
        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;

        // Set fireball to null
        fireball = null;

        // Set fireball instantiation direction
        forward = new Vector3(0.0f,0.0f,1.0f);
        
        // Find status bars
        statusBars = GameObject.FindGameObjectWithTag("StatusBars");
	}
	
	// Update is called once per frame
	void Update () {
        if(vibe_time_remaining > 0){
            vibe_time_remaining -= Time.deltaTime;
            OVRInput.SetControllerVibration(0.6f,0.5f, controller);
        }
        else{
            OVRInput.SetControllerVibration(0,0,controller);
        }

        //Record current element index of hand
        elementIndex = this.GetComponent<BallShooting>().elementIndex;

        //If fire is current element in hand
        if(elementIndex == 3){

            //If fireball is held, check trigger
            if(fireball != null){

                if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh){

                    //Calculate expected end position of fireball
                    controller_pos = OVRInput.GetLocalControllerPosition(controller);
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);

                    //Record expected end position of fireball
                    end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;

                    // Bring fireball to expected position each update
                    fireball.transform.position = end_pos;            
                }

                // If trigger not held, throw fireball
                else{

                    // Launch fireball
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);
                    fireball.GetComponent<Rigidbody>().AddForce(controller_rot*forward*thrust_const);
                    Destroy(fireball, projectileLifetime);
                    vibe_time_remaining = vibe_time;
                    fireball = null;
                    
                }

            }

            //If fireball is not held, check trigger
            else{

                 // Instantiate/control fireball if index trigger is held and cooldown period has passed
                if((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh)&&statusBars.GetComponent<PlayerBars>().EnoughStamina(staminaRequired)){
            
                    //Record current selected item
                    selectedItem = this.GetComponent<BallShooting>().selectedItem;
                    
                    // If selected item is not null
                    if((selectedItem != null)&&(selectedItem.tag == "Fireball")){
                        fireball = selectedItem;
                        fireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        fireball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 

                        // Update stamina bars
                        statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);

                        vibe_time_remaining = vibe_time; 
                        }

                    else{
                        //If controller not pointed at previous fireball, instantiate new object
                        controller_pos = OVRInput.GetLocalControllerPosition(controller);
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;
                        fireball = Instantiate(projectiles[elementIndex], end_pos, Quaternion.identity);
                        battle.GetComponent<AchievementTracking>().Shot("Fireball");

                        // Update stamina bars
                        statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);

                        vibe_time_remaining = vibe_time;               
                    }
                }
            }
        }
    }
} 
        
        
        

    
