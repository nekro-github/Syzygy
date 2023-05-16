using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class orient : MonoBehaviour {
    public Transform planet;
    public float offsetDown = 0.15f;
    void Awake() {
        Orient();
    }
    public void Orient() {
        //if (planet == null) print("cannot orient: " + name);
        if (planet == null) return;
        Vector3 pos = transform.position;
        Vector3 planetPos = planet.position;
        
        //gets planet direction
        Vector3 vec = (planetPos-pos).normalized;
        //aligns rotation to surface
        transform.rotation = Quaternion.FromToRotation(transform.up,-vec)*transform.rotation;
        
        Vector3 ray = (planetPos - pos).normalized;
        RaycastHit hit;
        if(Physics.Raycast(pos-ray*15, ray, out hit, 35, LayerMask.GetMask(LayerMask.LayerToName(planet.gameObject.layer))))
        {
            if(hit.transform.parent.name == planet.name) {
                transform.position+=ray*(hit.distance-15+transform.lossyScale.y*offsetDown);
            }
        }
    }
}
