using System;
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
    public BurgerDebris BurgerDebrisPrefab;

    public float StartingHealth;
    public float StartingPickleVelocity;
    public float StartingRateOfFire;
    public float StartingBunShiledDistance;
    public float MaxHealth;
    public float Health;
    public float Innaccuracy;

    float PickleVelocity;
    float RateOfFire;
    float BunShiledDistance;

    bool enemy;
    bool shielding;
    float countDown;
    Action OnDeathCallback;
    Action OnHitCallback;
    SpriteRenderer sr;
    GameObject bunShiled;
    Collider2D bunShiledCollider;
    Collider2D burgerCollider;
    AudioSource audioSource;
    bool invincible;
    float IFrameFlash;

    public void Initialize(bool enemy, Action OnDeathCallback, bool first = true, Action OnHitCallback = null)
    {
        this.enemy = enemy;
        this.OnDeathCallback = OnDeathCallback;
        this.OnHitCallback = OnHitCallback;
        shielding = false;
        if (first)
        {
            MaxHealth = StartingHealth;
        }
        Health = MaxHealth;
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

   void Update()
    {
        if (invincible)
        {
            IFrameFlash -= Time.deltaTime;
            if (IFrameFlash < 0)
            {
                IFrameFlash = .1f;
                sr.enabled = !sr.enabled;
            }
        }
        else
        {
            sr.enabled = true;
        }
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
        pickleVelocity += UnityEngine.Random.insideUnitCircle * Innaccuracy;
        var pickleRb = pickleObject.GetComponent<Rigidbody2D>();
        pickleRb.velocity = pickleVelocity;
        return true;
    }

    public void SetInvincible(bool i)
    {
        invincible = i;
        if (i)
        {
            IFrameFlash = .1f;
            Invoke("StopInvincible", 2f);
        }
    }

    void StopInvincible()
    {
        SetInvincible(false);
    }

    public void Damage(float amount)
    {
        if (invincible) return;
        if (OnHitCallback != null)
        {
            OnHitCallback();
        }

        Health--;
        if (Health <= 0)
        {
            Die();
        }
        else if (!enemy)
        {
            SetInvincible(true);
        }
    }

    public void Die()
    {
        for (int i = -1; i < 6; i++)
        {
            var chunk = Instantiate(BurgerDebrisPrefab).GetComponent<BurgerDebris>();
            chunk.transform.position = transform.position;
            chunk.Initialize(i);
        }
        OnDeathCallback();
    }

    void SetShielding(bool shielding)
    {
        if (invincible && shielding) return;
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
