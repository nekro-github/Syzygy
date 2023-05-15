using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {
    public RectTransform[] hotBarItems;
    public Sprite transparentImage;

    [HideInInspector]
    public bool hasUnlockedTP = true;
    private Item[] hotBar;
    private void Start() {
        hotBar = new Item[hotBarItems.Length];
    }

    private void Update() {
        //when you press q unlock teleportation between planets
        if (Input.GetKeyUp("q") && !hasUnlockedTP) {
            print("Unlocked teleportation!");
            hasUnlockedTP = true;
        }
    }
    const int stackSize = 65;
    public bool Pickup(Item item) {
        //print("Picked up: " + item.name);
        for (int i = 0; i < hotBar.Length; i++) {
            if (hotBar[i]==null) continue;
            if (hotBar[i].name == item.name) {
                if (hotBar[i].Count >= stackSize) continue;
                int amount = Mathf.Min(item.Count, (stackSize-hotBar[i].Count));
                item.Count -= amount;
                hotBar[i].Count += amount;
                updateHotbar();
                if (item.Count==0) return true;
            }
        }
        //matching item not found
        for (int i = 0; i < hotBar.Length; i++) {
            if (hotBar[i] == null) {
                int amount = Mathf.Min(item.Count, stackSize);
                item.Count -= amount;
                hotBar[i] = item.Copy();
                hotBar[i].Count = amount;
                updateHotbar();
                if (item.Count==0) return true;
            }
        }
        return false;
    }
    public void updateHotbar() {
        for (int i = 0; i < hotBar.Length; i++) {
            if (hotBar[i] != null && hotBar[i].Count == 0) hotBar[i] = null;
            hotBarItems[i].transform.GetChild(0).GetComponent<Image>().sprite = (hotBar[i]==null)?transparentImage:hotBar[i].hotbarItem;
            TextMeshProUGUI tmp = hotBarItems[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (tmp) tmp.text = (hotBar[i]==null)?"":(hotBar[i].Count.ToString());
        }
    }
}
