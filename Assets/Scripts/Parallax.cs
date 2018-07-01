using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform target;
    public float parralaxFactor;

    void LateUpdate()
    {
        transform.position = target.transform.position* parralaxFactor;
    }
}
