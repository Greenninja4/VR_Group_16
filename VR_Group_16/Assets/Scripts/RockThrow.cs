using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour {

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
    private GameObject rock;
    private Vector3 controller_pos;
    private Vector3 vec_to_pos;
    private Vector3 end_pos;
    private Vector3 above_ground;
    private Vector3 forward;
    private Quaternion controller_rot;

    //Initialize public constants
    public float trigger_thresh = 0.5f; //0 to 1, threshold for trigger activation
    public float float_dist = 2.0f; //End distance (in m) away from hand
    public float attraction_const = 0.02f; //0 to 1, Larger -> Faster attraction
    public float thrust_const = 175.0f; //Constant of thrust
    public int projectileLifetime = 20;
    public float staminaRequired = 20;
    public float vibe_time = 1f; //Seconds on the vibration
    private float vibe_time_remaining;
    private bool isPaused = false;

    private GameObject battle;
    public GameObject mainPlayer;


    // Use this for initialization
    void Start () {
        battle = this.transform.root.gameObject;

        // Get left and right hand game objects
        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;

        // Set rock to null
        rock = null;

        // Set height of rock instantiation
        above_ground = new Vector3(0.0f, 0.5f, 0.0f);
        forward = new Vector3(0.0f,0.0f,1.0f);
        
        // Find status bars
        statusBars = GameObject.FindGameObjectWithTag("StatusBars");
        mainPlayer = GameObject.FindGameObjectWithTag("MainPlayer");

        vibe_time_remaining = 0f;

    }

    // Update is called once per frame
    void Update () {

        isPaused = mainPlayer.GetComponent<Pause>().paused;
        if (!isPaused)
        {
            print(vibe_time_remaining);
            if (vibe_time_remaining > 0)
            {
                vibe_time_remaining -= Time.deltaTime;
                OVRInput.SetControllerVibration(0.8f, 0.6f, controller);
            }
            else
            {
                OVRInput.SetControllerVibration(0, 0, controller);
                vibe_time_remaining = 0f;
            }

            //Record current element index of hand
            elementIndex = this.GetComponent<BallShooting>().elementIndex;

            //If Earth is current element in hand
            if (elementIndex == 0)
            {

                //If rock is held, check trigger
                if (rock != null)
                {

                    if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh)
                    {

                        //Calculate expected end position of rock
                        controller_pos = OVRInput.GetLocalControllerPosition(controller);
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);

                        //Record expected end position of rock
                        end_pos = controller_pos + controller_rot * forward * float_dist + trackingSpace.transform.position;

                        //Account for rotation of player
                        //end_pos = player.transform.rotation*end_pos;

                        // Bring rock a fraction closer to expected position each update
                        vec_to_pos = end_pos - rock.transform.position;
                        rock.transform.position += vec_to_pos * attraction_const;
                    }

                    // If trigger not held, throw rock
                    else
                    {
                        // Launch rock
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        rock.GetComponent<Rigidbody>().AddForce(controller_rot * forward * thrust_const);
                        vibe_time_remaining = vibe_time;
                        Destroy(rock, projectileLifetime);
                        rock = null;
                    }
                }

                //If rock is not held, check trigger
                else
                {

                    // Instantiate/control rock if index trigger is held and if enough stamina
                    if ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh) && statusBars.GetComponent<PlayerBars>().EnoughStamina(staminaRequired))
                    {

                        //Record current selected item
                        selectedItem = this.GetComponent<BallShooting>().selectedItem;

                        // If selected item is not null
                        if (selectedItem != null)
                        {

                            // If controller pointed at ground, produce new object
                            if (selectedItem.tag == "Ground")
                            {
                                rock = Instantiate(projectiles[elementIndex], this.GetComponent<BallShooting>().hitpoint + above_ground, Quaternion.identity);
                                battle.GetComponent<AchievementTracking>().Shot("Rock");
                                // Update stamina bars
                                statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);
                                vibe_time_remaining = vibe_time;
                            }

                            else if (selectedItem.tag == "Rock")
                            {
                                rock = selectedItem;
                                rock.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                rock.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                                // Update stamina bars
                                statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);

                                vibe_time_remaining = vibe_time;
                            }
                        }
                    }
                }
            }

        }

    }
} 
        
        

    
