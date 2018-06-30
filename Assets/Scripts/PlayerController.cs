using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration;
    public float Speed;
    public float PlayerHealth = 10;
    public GameObject PicklePrefab;
    public float RateOfFire = .3f;


    Rigidbody2D rb;
    Unit unit;
    CircleCollider2D collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        unit = GetComponent<Unit>();
        unit.Initialize(PlayerHealth, PicklePrefab, RateOfFire, OnDeath);
    }

    void OnDeath()
    {
        // TODO: Lose a life? What?
        Destroy(gameObject);
    }

    void Update()
    {
        unit.CustomUpdate();

        var targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 rightAnalog = new Vector2(Input.GetAxisRaw("RAnalogX"), Input.GetAxisRaw("RAnalogY"));
        var mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 force = Speed * targetVelocity - rb.velocity;
        rb.AddForce(force * Acceleration);

        if (Input.GetMouseButton(1))
        {
            unit.Shield(mouseTarget);
        }
        else if ((Input.GetAxisRaw("TriggerL") > 0 || Input.GetAxisRaw("TriggerR") > 0) && rightAnalog != Vector2.zero)
        {
            unit.Shield(rightAnalog.normalized + (Vector2)transform.position);
        }
        else if (Input.GetMouseButton(0))
        {
            unit.Shoot(collider, mouseTarget);
        }
        else if (rightAnalog != Vector2.zero)
        {
            unit.Shoot(collider, rightAnalog.normalized + (Vector2)transform.position);
        }
    }
}