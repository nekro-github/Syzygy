using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetGeneration : MonoBehaviour
{
    public GameObject prefab;
    public int NumPlanets = 40;
    public int range = 2000;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i=0; i < NumPlanets; i++)
        {
            GameObject planet = Instantiate(prefab, new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range)), Quaternion.identity,transform.parent);
            Material planetMaterial = planet.transform.GetChild(0).GetComponent<MeshRenderer>().material;
            planetMaterial.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        
    }

}
