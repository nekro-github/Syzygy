using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientParent : MonoBehaviour
{
    public orient[] children;
    public GameObject planet;
    public float offsetDown = 0.15f;
    
    public void Orient()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].planet = planet;
            children[i].offsetDown = offsetDown;
            children[i].Orient();
        }
    }
}
