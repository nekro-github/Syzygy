using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour {
    public GameObject canvas;
    public RectTransform pauseOverlay;
    public RectTransform pauseText;
    public RectTransform crosshair;
    [Space]
    public RectTransform teleporterMenu;
    public RectTransform teleporterScrollViewportContent;
    public GameObject teleporterMenuItemPrefab;

    private RectTransform[] teleporterMenuItems;
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public bool teleportMenuOpen;

    void Start() {
        //init
        teleporterMenuItems = new RectTransform[0];

        //set inital states of UI
        isPaused = false;
        teleportMenuOpen = false;
        canvas.SetActive(true);
        pauseOverlay.gameObject.SetActive(true);
        pauseText.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
        teleporterMenu.gameObject.SetActive(true);
        SetRectActive(pauseOverlay, false);
        SetRectActive(pauseText, false);
        SetRectActive(crosshair, true);
        SetRectActive(teleporterMenu, false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        //when player presses escape it will unlock the cursor, show the pause screen and disable the crosshair
        if(Input.GetKeyDown("escape")) {
            if (!teleportMenuOpen) {
                isPaused = !isPaused;
                //lock or unlock cursor
                if (isPaused) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
                else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
                //set ui elements active or in-active
                SetRectActive(pauseOverlay, isPaused);
                SetRectActive(pauseText, isPaused);
                SetRectActive(crosshair, !isPaused);
            } else {
                teleportMenuOpen = false;
                SetRectActive(pauseOverlay, teleportMenuOpen);// set ui elements active or in-active
                SetRectActive(teleporterMenu, teleportMenuOpen);
                SetRectActive(crosshair, !teleportMenuOpen);
                Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;// locks cursor
            }
        }
    }
    public void openTeleportMenu(Teleporter from) {
        if (isPaused) Debug.LogError("state not supposed to occur. \"UIControler.cs ln#48\"");
        teleportMenuOpen = true;
        SetRectActive(pauseOverlay, teleportMenuOpen);// set ui elements active or in-active
        SetRectActive(teleporterMenu, teleportMenuOpen);
        SetRectActive(crosshair, !teleportMenuOpen);
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;// unlocks cursor

        //find all unlocked teleporters and make a button on the screen for it
        Teleporter[] teleporters = FindObjectsOfType(typeof(Teleporter)) as Teleporter[];// get a list of every Teleporter
        int count = 0;
        for(int i = 0; i < teleporters.Length; i++) {
            //if it is not unlocked set it to null in the array
            if (teleporters[i] == null) continue;//error checking
            if (!teleporters[i].isUnlocked || teleporters[i] == from) { teleporters[i] = null; }
            else count++;
        }
        RectTransform[] temp = new RectTransform[count];// create temp array
        for(int i = 0; i < count; i++) {
            //either take object from existing array of objects if it exists or instatiate a new one
            if (teleporterMenuItems.Length > i) {
                temp[i] = teleporterMenuItems[i];
            } else {
                temp[i] = Instantiate(teleporterMenuItemPrefab,teleporterScrollViewportContent).GetComponent<RectTransform>();
            }
        }
        if (teleporterMenuItems.Length > count) {for(int i = count; i < teleporterMenuItems.Length; i++) { Destroy(teleporterMenuItems[i].gameObject); }}//delete other menu items that are no longer used
        teleporterMenuItems = temp;

        count = 0;//re-using this variable to keep track of index
        for(int i = 0; i < teleporters.Length; i++) {
            Teleporter teleporter = teleporters[i];
            if (teleporter == null) continue;
            RectTransform elementTransform = teleporterMenuItems[count];
            GameObject element = elementTransform.gameObject;
            TextMeshProUGUI TMProText = element.transform.GetChild(0).GetComponent<TextMeshProUGUI>();//  gets text box of button
            element.gameObject.name = teleporter.Name;//                                                  sets the name
            TMProText.text = teleporter.Name;//                                                           ^
            Vector3 curPos = elementTransform.anchoredPosition;//                                         sets the position
            elementTransform.anchoredPosition = new Vector3(curPos.x, -(17 + count*30), curPos.z);//      +1  to account for space station button


            Button but = element.GetComponent<Button>();//                                                sets callback for when each button is pressed
            but.onClick.RemoveAllListeners();
            but.onClick.AddListener(delegate{
                teleporter.teleportTo(transform);//                                                       teleport player
                teleportMenuOpen = false;//                                                               close teleport menu
                SetRectActive(pauseOverlay, teleportMenuOpen);//                                          set ui elements active or in-active
                SetRectActive(teleporterMenu, teleportMenuOpen);//                                        ^
                SetRectActive(crosshair, !teleportMenuOpen);//                                            ^
                Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;//                       locks cursor
            });

            count++;//                                                                                    increments "count"
        }
    }

    void SetRectActive(RectTransform rect, bool active) {//more efficient way of hiding an object than transform.SetActive()
        if (active) {  rect.anchoredPosition = new Vector3(0,0,0); }//either moves the item off screen or back on screen
        else {  rect.anchoredPosition = new Vector3(0,100000,0); }
    }
}
