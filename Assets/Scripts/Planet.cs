using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Planet : MonoBehaviour {
    //variables for calculating gravity, by default has earth mass and radius
    public ScientificNumber mass = Constants.earthMass.Copy();
    public ScientificNumber radius = Constants.earthRadius.Copy();
    public float Scale;
    void OnValidate() {
        //handles scaling the object live in the inspector
        if (transform.parent != null) {
            Vector3 parentScale = transform.parent.lossyScale; transform.localScale = new Vector3(Scale/parentScale.x,Scale/parentScale.y,Scale/parentScale.z);
        } else {
            transform.localScale = new Vector3(Scale,Scale,Scale);
        }
    }
}
