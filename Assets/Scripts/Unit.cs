using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Unit : MonoBehaviour
{
    public Sprite FullBurgerSprite;
    public Sprite SplitBurgerSprite;
    public GameObject BunShiledPrefab;
    public GameObject PicklePrefab;
    public Vector2 BunShiledOffset;
    public AudioClip[] Plops;
    public Vector2 PitchMinMax;

    public float StartingHealth;
    public float StartingPickleVelocity;
    public float StartingRateOfFire;
    public float StartingBunShiledDistance;
    public float MaxHealth;
    public float Health;

    float PickleVelocity;
    float RateOfFire;
    float BunShiledDistance;

    bool enemy;
    bool shielding;
    float countDown;
    Action OnDeathCallback;
    SpriteRenderer sr;
    GameObject bunShiled;
    Collider2D bunShiledCollider;
    Collider2D burgerCollider;
    AudioSource audioSource;

    public void Initialize(bool enemy, Action OnDeathCallback = null)
    {
        this.enemy = enemy;
        this.OnDeathCallback = OnDeathCallback;
        shielding = false;
        Health = MaxHealth = StartingHealth;
        RateOfFire = StartingRateOfFire;
        PickleVelocity = StartingPickleVelocity;
        BunShiledDistance = StartingBunShiledDistance;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = FullBurgerSprite;

        if (bunShiled == null)
        {
            Destroy(bunShiled);
        }

        bunShiled = Instantiate(BunShiledPrefab);
        bunShiledCollider = bunShiled.GetComponent<BoxCollider2D>();
        burgerCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

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

    public bool Shoot(Vector3 target)
    {
        if (countDown > 0)
        {
            return false;
        }

        countDown += RateOfFire;
        
        target.z = transform.position.z;
        if (target == transform.position)
        {
            return false;
        }

        PlayPlop();

        var pickleObject = GameController.GetPickle(PicklePrefab, transform.position, Quaternion.identity, enemy);
        var collider = pickleObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(collider, bunShiledCollider);
        Physics2D.IgnoreCollision(collider, burgerCollider);

        // Shoot our pickle
        Vector2 forceDirection = target - pickleObject.transform.position;
        var pickleVelocity = forceDirection.normalized* PickleVelocity;
        var pickleRb = pickleObject.GetComponent<Rigidbody2D>();
        pickleRb.velocity = pickleVelocity;
        return true;
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

    void PlayPlop()
    {
        var pitch = UnityEngine.Random.Range(PitchMinMax.x, PitchMinMax.y);
        var plop = Plops[UnityEngine.Random.Range(0, Plops.Length)];
        audioSource.clip = plop;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
