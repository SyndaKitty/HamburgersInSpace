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

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navigator = GetComponent<EnemyNavigator>();
        rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        unit.Initialize(true, OnDeath);
        burstTimeout = Random.Range(BurstTimeoutRange.x, BurstTimeoutRange.x);
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

        if (navigator != null)
        {
            ControlNavigation();
        }
    }

    void OnDeath()
    {
        WaveController.Instance.Death(gameObject);
        Destroy(gameObject);
    }

    void ControlNavigation()
    {
        if (navigator.State == NavigationState.Stationary) return;
        if (player == null)
        {
            navigator.State = NavigationState.Neutral;
        }
        else
        {
            float playerDistance = ( player.transform.position - transform.position).magnitude;
            if (playerDistance < navigator.ProximityRadius.x)
            {
                navigator.State = NavigationState.FleeingPlayer;
            }
            else if (playerDistance > navigator.ProximityRadius.y)
            {
                navigator.State = NavigationState.SeekingPlayer;
            }
            else if (navigator.State == NavigationState.ShimmyLeft)
            {
                navigator.ShimmyTime -= Time.deltaTime;
                if (navigator.ShimmyTime <= 0)
                {
                    navigator.State = NavigationState.ShimmyRight;
                    navigator.ShimmyTime = Random.Range(navigator.ShimmyRange.x, navigator.ShimmyRange.y);
                }
            }
            else if (navigator.State == NavigationState.ShimmyRight)
            {
                navigator.ShimmyTime -= Time.deltaTime;
                if (navigator.ShimmyTime <= 0)
                {
                    navigator.State = NavigationState.ShimmyLeft;
                    navigator.ShimmyTime = Random.Range(navigator.ShimmyRange.x, navigator.ShimmyRange.y);
                }
            }
            else
            {
                int leftRight = Random.Range(0, 2);
                if (leftRight == 0)
                {
                    navigator.State = NavigationState.ShimmyLeft;
                }
                else
                {
                    navigator.State = NavigationState.ShimmyRight;
                }
                navigator.ShimmyTime = Random.Range(navigator.ShimmyRange.x, navigator.ShimmyRange.y);
            }
        }
    }
}
