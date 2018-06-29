using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Pickle : MonoBehaviour
{
    void Update ()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var unit = collision.collider.GetComponent<Unit>();
        if (unit == null)
        {
            return;
        }
        unit.Damage(1);
    }
}
