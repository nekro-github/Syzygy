using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class orient : MonoBehaviour {
    public GameObject planet;
    public float offsetDown = 0.15f;
    void Awake() {
        Orient();
    }
    public void Orient() {
        //if (planet == null) print("cannot orient: " + name);
        if (planet == null) return;
        //gets planet direction
        Vector3 vec = (planet.transform.position-transform.position).normalized;
        //aligns rotation to surface
        transform.rotation = Quaternion.FromToRotation(transform.up,-vec)*transform.rotation;

        Vector3 pos = transform.position;
        Vector3 ray = (planet.transform.position - pos).normalized;
        RaycastHit hit;
        if(Physics.Raycast(pos, ray, out hit, 15)) {
            if(hit.transform.parent == planet.transform) {
                transform.position+=ray*(hit.distance-transform.lossyScale.y+offsetDown);
            }
        }
    }
}
