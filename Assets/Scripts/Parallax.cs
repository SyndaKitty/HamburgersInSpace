using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform target;
    public float parralaxFactor;

    Vector3 offset;

    void Awake()
    {
        offset = transform.position;    
    }

    void Update()
    {
        if (target == null) return;
        Vector3 newPosition = (Vector2)target.transform.position * parralaxFactor;
        transform.position = newPosition + offset;
    }
}
