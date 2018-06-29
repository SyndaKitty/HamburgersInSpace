using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Unit : MonoBehaviour
{
    public float Health;
    public float PickleVelocity;

    Action OnDeathCallback;
    GameObject picklePrefab;

    public void Initialize(float health, GameObject picklePrefab, Action OnDeathCallback = null)
    {
        this.Health = health;
        this.picklePrefab = picklePrefab;
        this.OnDeathCallback = OnDeathCallback;
    }

    void Update()
    {

    }

    public void Shoot(Collider2D sourceCollider, Vector3 target)
    {
        target.z = transform.position.z;
        if (target == transform.position)
        {
            return;
        }

        var pickleObject = Instantiate(picklePrefab, transform.position, Quaternion.identity);
        var pickle = pickleObject.GetComponent<Pickle>();
        var collider = pickleObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(collider, sourceCollider);

        // Shoot our pickle
        Vector2 forceDirection = target - pickleObject.transform.position;
        var pickleVelocity = forceDirection.normalized* PickleVelocity;
        var pickleRb = pickleObject.GetComponent<Rigidbody2D>();
        pickleRb.velocity = pickleVelocity;

        // Assign a random rotation
        pickleRb.angularVelocity = UnityEngine.Random.Range(-45, 45);
    }

    public void Damage(float amount)
    {
        Health--;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeathCallback();
    }
}
