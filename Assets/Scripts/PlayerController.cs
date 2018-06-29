using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;

    public float acceleration;
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update ()
    {
        var targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 force = speed * targetVelocity - rb.velocity;
        rb.AddForce(force * acceleration);
    }
}