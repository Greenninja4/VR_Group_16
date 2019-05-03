using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour
{
    // Initialize external public elements
    public GameObject player;
    public GameObject centerEye;
    public GameObject rockwallprefab;
    public OVRInput.Controller Lcontroller;
    public OVRInput.Controller Rcontroller;
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject statusBars;

    // Initialize private elements
    public int active_walls;
    private GameObject cur_wall;
    private float hand_init_height;
    private Vector3 wall_pos_min;
    private Quaternion wall_rot;
    private Vector3 avg_hands;
    private Vector3 avg_hands_norm;
    private float disp;
    private int rotating_timer_cnt;//Rotates 0->1->2->..->max_active_walls->0 so that timers are not overwritten
    private int elementIndexL;
    private int elementIndexR;

    // Initilialize public constants
    public int max_active_walls = 3;
    public float float_dist = 4.0f; //End distance (in m) away from player
    public float disp_max = 0.8f; // Max displacement for hands (in m) relative to initial hand height
    public float wallLifetime = 10.0f; //Time (in sec) that each wall stays up
    public float pos_y_const = 0.01f; // Prevents z-fighting in wall render
    public float staminaRequired = 20;

    private float[] timers; // Set timers to know how many walls are still active


    // Use this for initialization
    void Start(){

        // Set initial variable defaults
        active_walls = 0;
        cur_wall = null;
        rotating_timer_cnt = 0;
        timers = new float[max_active_walls];
        for (int i = 0;i<max_active_walls;i++){
            timers[i] = 0.0f;
        }
    }

    //Update is called once per frame
    void Update(){

        // Record current element index of each hand
        elementIndexL = leftHandAnchor.GetComponent<BallShooting>().elementIndex;
        elementIndexR = rightHandAnchor.GetComponent<BallShooting>().elementIndex;

        // Update number of active walls using timer array
        active_walls = 0;
        for (int i = 0;i<max_active_walls;i++){
            timers[i] -= Time.deltaTime;
            if (timers[i] > 0.0f){
                active_walls += 1;
            }
        }

        // If Earth is current element for both hands
        if ((elementIndexL == 0)&&(elementIndexR == 0)){

            // If a wall is currently being held
            if(cur_wall != null){

                // Wall position goes to below ground, in front of player
                avg_hands = (OVRInput.GetLocalControllerPosition(Lcontroller)
                        + OVRInput.GetLocalControllerPosition(Rcontroller)) / 2;
                
                //Don't change wall x,z coordinates, nor rotation, but adjust height
                disp = avg_hands.y - hand_init_height;
                
                // If hands above max height, set wall to max height
                if(disp > disp_max){
                    cur_wall.transform.position = new Vector3(cur_wall.transform.position.x, wall_pos_min.y+rockwallprefab.transform.localScale.y, cur_wall.transform.position.z);
                }

                // Else if hands above min height, set wall to proportionate height
                else if(disp > 0.0f){
                    cur_wall.transform.position = new Vector3(cur_wall.transform.position.x, ((disp / disp_max)-0.5f) * rockwallprefab.transform.localScale.y + pos_y_const, cur_wall.transform.position.z);
                }

                else{
                    cur_wall.transform.position = new Vector3(cur_wall.transform.position.x, wall_pos_min.y, cur_wall.transform.position.z);
                }

                // Maintain same rotation
                cur_wall.transform.rotation = wall_rot;

                // If triggers not held anymore, set wall to current position/rotation
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Lcontroller) <= 0.5 
                && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Rcontroller) <= 0.5){
                    
                    Destroy(cur_wall, wallLifetime);
                    cur_wall = null;
                    timers[rotating_timer_cnt] = wallLifetime;
                    rotating_timer_cnt = (rotating_timer_cnt+1) % max_active_walls;
                }
            }

            // Else if no wall being held, check if new wall can be built
            else if (active_walls < max_active_walls){

                // If triggers held, build new wall
                print(statusBars.GetComponent<PlayerBars>().EnoughStamina());
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Lcontroller) > 0.5 
                && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Rcontroller) > 0.5
                && statusBars.GetComponent<PlayerBars>().EnoughStamina()){


                    // Find average hand position when activated
                    avg_hands = (OVRInput.GetLocalControllerPosition(Lcontroller)
                            + OVRInput.GetLocalControllerPosition(Rcontroller)) / 2;

                    // Set wall position float_dist in front of player's hands (relative to headset)
                    hand_init_height = avg_hands.y;
                    avg_hands_norm = avg_hands + player.transform.position - centerEye.transform.position;
                    avg_hands_norm.y = 0.0f;
                    avg_hands_norm = avg_hands_norm.normalized;
                    wall_pos_min =  centerEye.transform.position + float_dist * (avg_hands_norm);
                    wall_pos_min.y = pos_y_const - rockwallprefab.transform.localScale.y/2;

                    // Set wall rotation such that it starts normal to player
                    wall_rot = Quaternion.Euler(0, Mathf.Atan2(avg_hands_norm.x,avg_hands_norm.z) * Mathf.Rad2Deg, 0);

                    print(wall_pos_min);
                    print(wall_rot);

                    // Produce new wall object
                    cur_wall = Instantiate(rockwallprefab, wall_pos_min, wall_rot);

                    // Update stamina bars
                    statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);

                    active_walls += 1;
                }
            }
        }
    }
}

