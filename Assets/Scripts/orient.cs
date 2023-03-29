using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class orient : MonoBehaviour {
    public GameObject planet;
    void Start() {
        #pragma warning disable 0162// unreachable code detected (which isn't actually true)
        if (planet == null) { print("cannot orient: " + name); return; }
        //gets planet direction
        Vector3 vec = (planet.transform.position-transform.position).normalized;
        //aligns rotation to surface
        transform.rotation = Quaternion.FromToRotation(transform.up,-vec)*transform.rotation;
        #pragma warning disable 0162
    }
}
