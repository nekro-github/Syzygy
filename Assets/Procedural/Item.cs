using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="Custom/Item")]
public class Item : ScriptableObject
{
    public GameObject prefab;
    public Sprite hotbarItem;
    public int Count = 1;
    public Item Copy() {
        Item itm = ScriptableObject.CreateInstance(typeof(Item)) as Item;
        itm.prefab = prefab;
        itm.hotbarItem = hotbarItem;
        itm.Count = Count;
        itm.name = name;
        return itm;
    }
}