using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract class used to define an object you can interact with in some way
public abstract class Interactable : MonoBehaviour {
    public abstract string InteractionPrompt { get; }
    public abstract bool interactEvent { get; }
    public virtual bool Interact(Interactor interactor) {
        //code that will be run when interacted with by default, should be overriden
        print(interactor.name + " Interacted with " + this.name);
        return true;
    }
}