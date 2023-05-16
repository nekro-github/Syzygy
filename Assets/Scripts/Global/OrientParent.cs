using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientParent : MonoBehaviour
{
    public orient[] children;
    public Transform planet;
    public float offsetDown = 0.15f;
    
    public void Orient() {
        if (Application.isPlaying) {
            for (int i = 0; i < children.Length; i++) {
                if (children[i] == null) continue;
                children[i].planet = planet;
                children[i].offsetDown = offsetDown;
                children[i].Orient();
            }
        }
    }
}
