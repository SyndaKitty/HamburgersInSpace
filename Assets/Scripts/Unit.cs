using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Unit : MonoBehaviour
{
    public float Health;
    public float PickleVelocity;
    public float RateOfFire;
    public float countDown;
    Action OnDeathCallback;
    GameObject picklePrefab;

    public void Initialize(float health, GameObject picklePrefab, float rateOfFire, Action OnDeathCallback = null)
    {
        this.Health = health;
        this.picklePrefab = picklePrefab;
        this.OnDeathCallback = OnDeathCallback;
        this.RateOfFire = rateOfFire;
    }

    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown < 0) countDown = 0;
    }

    public void Shoot(Collider2D sourceCollider, Vector3 target)
    {
        if (countDown > 0)
        {
            return;
        }

        countDown += RateOfFire;
        
        target.z = transform.position.z;
        if (target == transform.position)
        {
            return;
        }

        var pickleObject = GameController.GetPickle(picklePrefab, transform.position, Quaternion.identity);
        var pickle = pickleObject.GetComponent<Pickle>();
        var collider = pickleObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(collider, sourceCollider);

        // Shoot our pickle
        Vector2 forceDirection = target - pickleObject.transform.position;
        var pickleVelocity = forceDirection.normalized* PickleVelocity;
        var pickleRb = pickleObject.GetComponent<Rigidbody2D>();
        pickleRb.velocity = pickleVelocity;

        // Assign a random rotation
        float magnitude = UnityEngine.Random.Range(0, 2);
        if (magnitude == 0)
        {
            magnitude = -1;
        }
        pickleRb.angularVelocity = magnitude * UnityEngine.Random.Range(80, 300f);
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
