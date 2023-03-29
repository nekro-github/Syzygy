using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Interactor : MonoBehaviour {
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 1;
    [SerializeField] private LayerMask layerMask = (LayerMask)2*2*2*2*2*2*2*2;// layer mask 2^8, or just layer 8(interactables)

    private readonly Collider[] colliders = new Collider[3];
    //[SerializeField]
    private int numFound;

    
    private UIController controller;
    void Start() {
        controller = GetComponent<UIController>();
    }

    private void Update() {
        if (controller.isPaused) return;
        //gets colliders of interactables and assigns it to "colliders"
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, layerMask);
        if (numFound > 0) {
            //loops through all currently in contact with interactor
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i] != null) {
                    //if interact event is true, call interact()
                    Interactable interactable = colliders[i].GetComponent<Interactable>();
                    if (interactable == null) return;
                    if (interactable.interactEvent) interactable.Interact(this);
                }
            }
        }
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        //show red wireframe sphere to show interaction sphere visually
        try {
            if (Selection.activeTransform == null || !(
                (Selection.activeTransform == transform) ||
                (Selection.activeTransform.parent == transform) ||
                (Selection.activeTransform.parent.parent == transform) 
            ) || interactionPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
        }
        catch{}
    }
    #endif
}
