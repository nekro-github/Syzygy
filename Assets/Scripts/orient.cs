using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class orient : MonoBehaviour {
    public GameObject planet;
    void Start()
    {
        Vector3 pos = transform.position;
        Vector3 ray = (transform.position - planet.transform.position).normalized;
        Debug.DrawRay(Vector3.zero,Vector3.up*5, Color.red);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        int layerMask = (1<<5)|(1<<6)|(1<<7);
        
        if (Physics.Raycast(pos, ray, out hit, Mathf.Infinity, layerMask)) {
            Debug.DrawRay(pos, ray*hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        } else {
            Debug.DrawRay(pos, ray*1000, Color.red);
            Debug.Log("Did not Hit");
        }
        
        #pragma warning disable 0162// unreachable code detected (which isn't actually true)
        if (planet == null) { print("cannot orient: " + name); return; }
        //gets planet direction
        Vector3 vec = (planet.transform.position-transform.position).normalized;
        //aligns rotation to surface
        transform.rotation = Quaternion.FromToRotation(transform.up,-vec)*transform.rotation;
        #pragma warning disable 0162
    }
}
