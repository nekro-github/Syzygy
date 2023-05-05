using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour {
    [HideInInspector]
    public bool hasUnlockedTP = true;

    private void Update() {
        //when you press q unlock teleportation between planets
        if (Input.GetKeyUp("q") && !hasUnlockedTP) {
            print("Unlocked teleportation!");
            hasUnlockedTP = true;
        }
    }
    public void Pickup(Item item) {
        print("Picked up: " + item.name);
    }
}
