using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Planet : MonoBehaviour {
    //variables for calculating gravity, by default has earth mass and radius
    public ScientificNumber mass = Constants.earthMass.Copy();
    public ScientificNumber radius = Constants.earthRadius.Copy();
    public float Scale;
    private float heightMult = 5f;
    private float noiseScale = 7f;
    public GameObject prefab;
    
    
    void OnValidate() {
        //handles scaling the object live in the inspector
        if (transform.parent != null) {
            Vector3 parentScale = transform.parent.lossyScale; transform.localScale = new Vector3(Scale/parentScale.x,Scale/parentScale.y,Scale/parentScale.z);
        } else {
            transform.localScale = new Vector3(Scale,Scale,Scale);
        }
    }

    void Awake()
    {
        //raise terrain
        GameObject child = transform.GetChild(0).gameObject;
        MeshFilter meshFilter = child.GetComponent(typeof(MeshFilter)) as MeshFilter;
        MeshCollider meshCollider = child.GetComponent(typeof(MeshCollider)) as MeshCollider;
        Mesh mesh = meshFilter!.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3 pos = transform.position;
        float w = 1000 * pos.x + 100 * pos.y + 10 * pos.z;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vert = vertices[i];
            float value = PerlinNoise4D(vert.x*noiseScale, vert.y*noiseScale, vert.z*noiseScale,w);
            vertices[i] = vert.normalized * (vert.magnitude+((value*2-2)*heightMult)/Scale);
        }
        mesh.vertices = vertices;
        meshCollider!.sharedMesh = mesh;
        
        //Create crystals
        int num = 20;
        for (int j = 0; j < num; j++)
        {
            Vector3 vert = vertices[Mathf.RoundToInt(j*vertices.Length/num)];
            GameObject interactable = Instantiate(prefab, transform.position + vert*Scale*0.5f + vert.normalized, Quaternion.identity, transform);
            interactable.transform.localScale /= Scale;
            OrientParent obj = interactable.GetComponent(typeof(OrientParent)) as OrientParent;
            obj.planet = transform;
            obj.Orient();
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
    float PerlinNoise4D(float x, float y, float z, float w)
    {
        //X coordinate
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float xw = _perlin3DFixed(x, w);
        
        //Ycoordinate
        float yx = _perlin3DFixed(y, x);
        float yz = _perlin3DFixed(y, z);
        float yw = _perlin3DFixed(y, w);
        
        //Z coordinate
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        float zw = _perlin3DFixed(z, w);
        
        //W coordinate
        float wx = _perlin3DFixed(w, x);
        float wy = _perlin3DFixed(w, y);
        float wz = _perlin3DFixed(w, z);
 
        return (xy + xz + xw + yx + yz + yw + zx + zy + zw + wx + wy + wz)/12.0f;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    } 
}
