using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//teleporter interactable instance
public class Resource : Interactable {
    public override string InteractionPrompt { get{ return ""; } }
    public override bool interactEvent { get{ return Input.GetKeyDown("e"); } }// bool which says if it is being interacted with
    public Item item;
    public override bool Interact(Interactor interactor) {
        //print("Mined: " + item.name);
        var inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return false;
        if (inventory.Pickup(item)) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    //set item to a copy so you can modify it without messing things up
    public void Awake() { item = item.Copy(); }
}
