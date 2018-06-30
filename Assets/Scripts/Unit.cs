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
    public float BunShiledDistance;
    public Sprite FullBurgerSprite;
    public Sprite SplitBurgerSprite;
    public GameObject BunShiledPrefab;
    public Vector2 BunShiledOffset;

    Action OnDeathCallback;
    GameObject picklePrefab;
    SpriteRenderer sr;
    bool shielding;
    GameObject bunShiled;

    public void Initialize(float health, GameObject picklePrefab, float rateOfFire, Action OnDeathCallback = null)
    {
        sr = GetComponent<SpriteRenderer>();
        this.Health = health;
        this.picklePrefab = picklePrefab;
        this.OnDeathCallback = OnDeathCallback;
        this.RateOfFire = rateOfFire;
        shielding = false;
        sr.sprite = FullBurgerSprite;

        bunShiled = Instantiate(BunShiledPrefab);
        bunShiled.SetActive(false);
    }

    public void CustomUpdate()
    {
        SetShielding(false);
        countDown -= Time.deltaTime;
        if (countDown < 0) countDown = 0;
    }

    public void Shield(Vector2 target)
    {
        SetShielding(true);
        var targetDifference = target - (Vector2)transform.position;
        bunShiled.transform.position = targetDifference.normalized * BunShiledDistance + (Vector2)transform.position + BunShiledOffset;
        bunShiled.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDifference.y, targetDifference.x) * Mathf.Rad2Deg - 90, Vector3.forward);
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

    void SetShielding(bool shielding)
    {
        this.shielding = shielding;
        if (shielding)
        {
            sr.sprite = SplitBurgerSprite;
        }
        else
        {
            sr.sprite = FullBurgerSprite;
        }
        bunShiled.SetActive(shielding);
    }
}
