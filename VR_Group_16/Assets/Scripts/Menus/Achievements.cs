using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    //Bound to a main achievements menu
    //Amongst the children are the other acievements menus
    public Transform menu;

    private Dictionary<string, GameObject> submenus = new Dictionary<string, GameObject>();

    private Dictionary<string, string> menuTransitions = new Dictionary<string, string>(){
        {"Fire", "FireMenu"},
        {"Air", "AirMenu"},
        {"Water","WaterMenu"},
        {"Earth", "EarthMenu"},
        {"Back", "TopMenu"}
    };

    //Current things that require submenus
    private string[] approved_submenus = { "AirMenu", "FireMenu", "WaterMenu", "EarthMenu", "TopMenu" };

    //Text data for each tile on the menus
    private static Dictionary<string, string> fireMenuText = new Dictionary<string, string>()
    {
        {"Time", "Time spent: " },
        {"Shots", "Fire balls: " },
        {"Hits", "Hits: " },
        {"Accuracy", "Accuracy: " },
        {"Back", "Back" },
        {"Title", "Fire Achievements" }
    };
    private static Dictionary<string, string> waterMenuText = new Dictionary<string, string>()
    {
        {"Time", "Time spent: " },
        {"Shots", "Water balls: " },
        {"Hits", "Hits: " },
        {"Accuracy", "Accuracy: " },
        {"Back", "Back" },
        {"Title", "Water Achievements" }

    };
    private static Dictionary<string, string> earthMenuText = new Dictionary<string, string>()
    {
        {"Time", "Time spent: " },
        {"Shots", "Rock balls: " },
        {"Hits", "Hits: " },
        {"Accuracy", "Accuracy: " },
        {"Back", "Back" },
        {"Title", "Earth Achievements" }

    };
    private static Dictionary<string, string> airMenuText = new Dictionary<string, string>()
    {
       { "Time", "Time spent: " },
       { "Shots", "Air balls: " },
       { "Hits", "Hits: " },
       { "Accuracy", "Accuracy: " },
       { "Back", "Back" },
        {"Title", "Air Achievements" }

    };

    private static Dictionary<string, string> topMenuText = new Dictionary<string, string>()
    {
       { "Fire", "Fire" },
       { "Water", "Water" },
       { "Earth", "Earth" },
       { "Air", "Air " },
       { "Quit", "Quit" },
       { "Title", "Achievements" }

    };

    private Dictionary<string, Dictionary<string, string>> menuText = new Dictionary<string, Dictionary<string, string>>()
    {
        { "FireMenu", fireMenuText },
        { "WaterMenu", waterMenuText },
        { "EarthMenu", earthMenuText },
        { "AirMenu", airMenuText },
        { "TopMenu", topMenuText } 
    };



    private Dictionary<string, string> currentMenuText;
    private string[] currentMenuOptions;
    private GameObject field; //entry on a paritcular submenu
    private string curr_submenu; //name of current window
    private GameObject visibleMenu; //gameObject of current submenu


    // Use this for initialization
    void Start()
    {
        this.menu = this.transform;
        this.curr_submenu = "TopMenu";
        //Get each achievement submenu from main achievement menu
        //child will be any gameObject attached to the achievement 
        ///menu window
        foreach (Transform child in menu)
        {
            //Remove self and null from list (to avoid null ptr error
            if (child == null) continue;
            else if (child == this.gameObject) continue; //transfer to gameObject required

            //If name matches submenu name, add to list
            else if (approved_submenus.Contains(child.name))
            {
                Debug.Log("Adding " + child.name + " to submenus");
                this.submenus.Add(child.name, child.gameObject);
            }
        }


        //For each submenu we have a name for, populate it's text field
        //with stored data
        foreach (string key in submenus.Keys)
        {
            submenus[key].SetActive(false);

            //currentMenuText is dictionary where each key is a submenu 
            //and each value is the text that should populate that field
            this.currentMenuText = menuText[key];
            //currentMenuText is 
            foreach (string menuOption in currentMenuText.Keys)
            {
                //submenu->menu option
                this.field = this.submenus[key].transform.Find(menuOption).gameObject;
                //menu option -> canvas -> text
                this.field.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = currentMenuText[menuOption];
            }
        }
        this.submenus[this.curr_submenu].gameObject.SetActive(true);

        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(curr_submenu);
        this.visibleMenu = submenus[curr_submenu]; // gameObject for editting
        this.currentMenuText = menuText[curr_submenu]; //dictionary<string, string> with each field label and value
        foreach (string menuOption in currentMenuText.Keys)
        {
            //submenu->menu option
            this.field = this.visibleMenu.transform.Find(menuOption).gameObject;


            //generate text
            string fieldText;
            string playerField; ;
            switch (menuOption)
            {
                case "Time":
                    playerField = curr_submenu + "Time";
                    float timePlayed = PlayerPrefs.GetFloat(playerField, 0)/60;
                    
                    fieldText = currentMenuText[menuOption] + timePlayed.ToString("0.0");
                    this.field.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = fieldText;
                    break;
                case "Shots":
                    playerField = curr_submenu + "Shots";
                    fieldText = currentMenuText[menuOption] + PlayerPrefs.GetInt(playerField, 0);
                    this.field.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = fieldText;

                    break;
                case "Hits":
                    playerField = curr_submenu + "Hits";
                    fieldText = currentMenuText[menuOption] + PlayerPrefs.GetInt(playerField, 0);
                    this.field.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = fieldText;

                    break;
                case "Accuracy":
                    playerField = curr_submenu + "Shots";
                    int shots = PlayerPrefs.GetInt(playerField, 1);
                    playerField = curr_submenu + "Hits";
                    int hits = PlayerPrefs.GetInt(playerField, 0);
                    float accuracy = hits/shots;
                    accuracy /= 60;
                    fieldText = currentMenuText[menuOption] + accuracy.ToString("0.0") + "%";
                    this.field.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = fieldText;

                    break;

            }
        }
    }

    public void clickHandle(GameObject selectedItem)
    {
        // //Handle back press
        // if (selectedItem.name == "Back")
        // {
        //     this.submenus[curr_submenu].SetActive(false);
        //     this.menu.gameObject.SetActive(true);
        // }

        //Handle menu transitions
        if(selectedItem.name == "Quit"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"Startmenu");

        }

        GameObject hitMenu;
        string targetMenu;
        Debug.Log(selectedItem.name);
        Debug.Log("In clickHandle");
        this.menuTransitions.TryGetValue(selectedItem.name, out targetMenu);
        if(targetMenu == null){
            return;
        }

        this.submenus.TryGetValue(targetMenu, out hitMenu);
        if (hitMenu != null)
        {
            Debug.Log(hitMenu.name);
            this.submenus[this.curr_submenu].gameObject.SetActive(false);
            hitMenu.SetActive(true);
            this.curr_submenu = targetMenu;
        }
    }

    void resetAchievements(){
        PlayerPrefs.DeleteAll();
    }

}
