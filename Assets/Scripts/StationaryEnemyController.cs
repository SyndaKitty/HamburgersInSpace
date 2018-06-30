using UnityEngine;

public class StationaryEnemyController : MonoBehaviour
{
    public bool Attack;
    public int StartingBurstAmount;
    public float StartingBurstTimeout;

    GameObject player;
    Rigidbody2D rb;
    Unit unit;
    public float burstTimeout;
    public float burstAmount;
    public bool bursting;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        unit.Initialize(OnDeath);
        burstTimeout = StartingBurstTimeout;
        burstAmount = StartingBurstAmount;
    }

    void Update()
    {
        unit.CustomUpdate();

        if (!Attack) return;

        if (bursting && burstAmount <= 0)
        {
            burstAmount = StartingBurstAmount;
            burstTimeout = StartingBurstTimeout;
            bursting = false;
        }

        if (bursting)
        {
            Vector2 target = player.transform.position;
            if (unit.Shoot(target))
            {
                burstAmount--;
            }
        }
        else if (burstTimeout < 0)
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
        Destroy(gameObject);
    }
}
