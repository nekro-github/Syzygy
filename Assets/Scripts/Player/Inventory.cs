using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {
    public RectTransform[] hotBarItems;
    public Sprite transparentImage;
    public int selectedSlot = 0;
    public GameObject debug;

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
        for (int i = 0; i < hotBarItems.Length; i++) {
            if (Input.GetKeyDown((i+1).ToString())) {selectedSlot=i;updateHotbar();}
        }
        if (Input.GetMouseButtonDown(1) && hotBar[selectedSlot] != null && hotBar[selectedSlot].Count > 0) {
            Transform cam = Camera.main.transform;
            Vector3 forward =cam.forward.normalized;
            Vector3 right =(cam.rotation * cam.right).normalized;
            RaycastHit hit;
            if(Physics.Raycast(cam.position, forward, out hit, 10, ((LayerMask)(~0 - LayerMask.GetMask("player"))) )) {
                Transform placed = Instantiate(hotBar[selectedSlot].prefab).transform;
                hotBar[selectedSlot].Count--;
                updateHotbar();

                placed.position = hit.point;
                orient or = placed.GetComponent(typeof(orient)) as orient;
                if (or) placed.position+=hit.normal.normalized*(placed.lossyScale.y*or.offsetDown/80.0f);
                Vector3 lookAt  = Vector3.Cross(-hit.normal, right);
                lookAt = lookAt.y < 0 ? -lookAt : lookAt;// reverse it if it is down.
                placed.rotation *= Quaternion.FromToRotation(placed.up,hit.normal);//set rotation
            }
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

            hotBarItems[i].transform.GetChild(2).gameObject.SetActive(i==selectedSlot);
        }
    }
}
