using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//teleporter interactable instance
public class Resource : Interactable {
    public override string InteractionPrompt { get{ return "Open teleport menu?"; } }
    [SerializeField] private string interactKey = "e";
    public override bool interactEvent { get{ return Input.GetKeyDown(interactKey); } }// bool which says if it is being interacted with

    public override bool Interact(Interactor interactor) {
        return true;
    }
}
