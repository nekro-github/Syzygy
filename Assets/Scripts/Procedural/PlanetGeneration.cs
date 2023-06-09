using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetGeneration : MonoBehaviour
{
    public GameObject prefab;
    public int NumPlanets = 40;
    public int range = 2000;

    // All happens on pressing play
    void Start() {
        //Creates a random number of random planets
        for (int i=0; i < NumPlanets; i++) {
            // Create the planets in a random range
            GameObject planet = Instantiate(prefab, new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range)), Quaternion.identity,transform.parent);
            //randomize size of planets
            Planet planetPlanet = (planet.GetComponent(typeof(Planet)) as Planet);
            planetPlanet.Scale = Random.Range(70, 90);
            planetPlanet.Generate();
            // Get the material for the planet generated and assign a random color to it
            Material planetMaterial = planet.transform.GetChild(0).GetComponent<MeshRenderer>().material;
            planetMaterial.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            planet.name = System.Guid.NewGuid().ToString().Substring(4, 14);;
        }
        
    }
    
    

}
