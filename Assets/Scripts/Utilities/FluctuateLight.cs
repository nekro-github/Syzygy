using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluctuateLight : MonoBehaviour {
    Light lgt;
    float startIntensity;
    float speed = 2;
    float change = 7;
    private float lightChange;
    void Start() {
        //initialize
        lgt = GetComponent<Light>();
        lightChange = -speed;
        startIntensity = lgt.intensity;
        StartCoroutine(flip());
    }
    IEnumerator flip() {
        //wait 3.5 seconds
        yield return new WaitForSeconds(change/speed);
        //after 3.5 seconds if it is at -2 per second change it to +2 and visa versa
        //and insure the intesity stays close to what it started at(prevents compounding error)
        if (lightChange > 0) { lgt.intensity = startIntensity; lightChange = -speed; }
        else { lgt.intensity = startIntensity-change; lightChange = speed;  }
        StartCoroutine(flip());
    }
    void Update() {
        //increase or decrease intesity
        lgt.intensity += lightChange*Time.deltaTime;
    }
}
