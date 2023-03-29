using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//teleporter interactable instance
public class Teleporter : Interactable {
    public override string InteractionPrompt { get{ return "Open teleport menu?"; } }
    [SerializeField] private string interactKey = "e";
    public override bool interactEvent { get{ return Input.GetKeyDown(interactKey); } }// bool which says if it is being interacted with
    public bool isUnlocked = false;
    public bool disableOrientation = false;

    public string Name = "";
    void Start() {
        if (Name == "" && transform.parent != null) Name = transform.parent.name;
    }



    public override bool Interact(Interactor interactor) {
        if (!isUnlocked) isUnlocked = true;
        //gets the inventory script on the player and makes sure its not null
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;
        //gets the UIController script on the player and makes sure its not null
        var uicontr = interactor.GetComponent<UIController>();
        if (uicontr == null) return false;
        //checks that the player has permission to teleport
        if (inventory.hasUnlockedTP) {
            //can teleport
            uicontr.openTeleportMenu(this);
            return true;
        } else {
            Debug.Log("Has not unlocked ability to teleport"); return false;// didnt have permission to teleport
        }
    }
    public void teleportTo(Transform obj) {
        // successfully teleported
        obj.position = transform.position;
        
        //if obj has player script
        var player = obj.GetComponent<PlayerController>();
        if (player != null) {
            if (disableOrientation) obj.rotation = Quaternion.Euler(0,0,0);// if disabling orientation reset rotation
            player.Orient = !disableOrientation;// turns off or on planet gravity calculation and stuff(used for space station)
            player.snapOrientation = true;// instead of slowling turning to the direction of the planets gravitational field it will snap to it
        }
        
        //if obj has rigidbody
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;// sets velocity to 0
        }
    }
}
