using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetGeneration : MonoBehaviour
{
    public GameObject prefab;
    public int NumPlanets = 40;
    public int range = 2000;

    // All happens on pressing play
    void Start()
    {
        //Creates a random number of random planets
        for (int i=0; i < NumPlanets; i++)
        {
            // Create the planets in a random range
            GameObject planet = Instantiate(prefab, new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range)), Quaternion.identity,transform.parent);
            
            // Get the material for the planet generated and assign a random color to it
            Material planetMaterial = planet.transform.GetChild(0).GetComponent<MeshRenderer>().material;
            planetMaterial.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        
    }
    
    // Makes 3D Perlin Noise to be projected on to the planets by combining axis of 2D Perlin Noises and their opposites
    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    } 

}
