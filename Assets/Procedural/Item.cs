using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="Custom/Item")]
public class Item : ScriptableObject
{
    public GameObject prefab;
    public Sprite hotbarItem;
}