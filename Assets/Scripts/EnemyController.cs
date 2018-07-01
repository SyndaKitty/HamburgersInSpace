using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool Attack;
    public Vector2 BurstRange;
    public Vector2 BurstTimeoutRange;

    EnemyNavigator navigator;
    GameObject player;
    Rigidbody2D rb;
    Unit unit;
    float burstTimeout;
    float burstAmount;
    bool bursting;
    public int spawningIndex;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navigator = GetComponent<EnemyNavigator>();
        rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        unit.Initialize(true, OnDeath, OnHit);
        burstAmount = Random.Range(BurstRange.x, BurstRange.y);
        burstTimeout = Random.Range(BurstTimeoutRange.x, BurstTimeoutRange.x) * .4f;
        bursting = true;
    }

    void OnHit()
    {
        if (unit.Health <= 2)
        {
            navigator.State = NavigationState.FleeingPlayer;
        }
    }

    void Update()
    {
        unit.CustomUpdate();

        if (!Attack) return;

        if (bursting && burstAmount <= 0)
        {
            burstAmount = Random.Range(BurstRange.x, BurstRange.y);
            burstTimeout = Random.Range(BurstTimeoutRange.x, BurstTimeoutRange.x);
            bursting = false;
        }

        if (bursting && player != null && player.activeSelf)
        {
            Vector2 target = player.transform.position;
            Vector2 direction = target - (Vector2)transform.position;
            Debug.DrawRay(transform.position, direction * direction.magnitude, Color.white);
            if (Physics2D.Raycast(transform.position, direction, direction.magnitude, 1))
            {
                Debug.Log("Can't shoot");
            }
            else if (unit.Shoot(target))
            {
                burstAmount--;
            }
        }
        else if (burstTimeout <= 0)
        {
            burstTimeout = 0;
            bursting = true;
        }
        else
        {
            burstTimeout -= Time.deltaTime;
        }
    }

    void OnDeath()
    {
        WaveController.Instance.Death(gameObject);
        Destroy(gameObject);
    }
}
