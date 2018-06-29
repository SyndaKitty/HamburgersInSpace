using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration;
    public float Speed;
    public float PlayerHealth = 10;
    public GameObject PicklePrefab;

    Rigidbody2D rb;
    Unit unit;
    CircleCollider2D collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        unit = GetComponent<Unit>();
        unit.Initialize(PlayerHealth, PicklePrefab, OnDeath);
    }

    void OnDeath()
    {
        // TODO: Lose a life? What?
        Destroy(gameObject);
    }

    void Update()
    {
        var targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 force = Speed * targetVelocity - rb.velocity;
        rb.AddForce(force * Acceleration);

        if (Input.GetMouseButtonDown(0))
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // TODO
            unit.Shoot(collider, target);
        }
    }
}