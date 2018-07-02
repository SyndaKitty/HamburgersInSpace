using UnityEngine;

public enum NavigationState
{
    SeekingPlayer,
    FleeingPlayer,
    Neutral,
    ShimmyLeft,
    ShimmyRight,
    Stationary,
}

public class EnemyNavigator : MonoBehaviour
{
    public NavigationState State;
    public Vector2 StationaryPosition;

    public float Acceleration;
    public float Speed;
    
    public float Aggressiveness = 1;
    public float Shyness = 1;
    public float Sideness = 1;
    public float Reset = 1;

    public Vector2 PlayerDistanceMinMax;
    public Vector2 PlayerDistanceTowardBehavior;

    float directionTime;
    Rigidbody2D rb;
    GameObject player;
    Vector2 force;
    bool left;

    private void Awake()
    {
        Initialize();
        directionTime = Random.Range(3, 10);
    }

    void Initialize()
    {
        StationaryPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        directionTime -= Time.deltaTime;
        if (directionTime <= 0)
        {
            directionTime = Random.Range(3, 10);
            left = !left;
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;
        Vector2 target = Vector2.zero;
        if (player == null) State = NavigationState.Neutral;
        switch (State)
        {
            case NavigationState.SeekingPlayer:
            {
                target += ToTarget(player) * Aggressiveness * DistanceFactor(player, PlayerDistanceMinMax, PlayerDistanceTowardBehavior);
                target -= ToTarget(player) * Shyness;
                if (left)
                {
                    var shimmyVector = Quaternion.Euler(0, 0, -90) * ToTarget(player) * Sideness;
                    target += (Vector2)shimmyVector;
                }
                else
                {
                    var shimmyVector = Quaternion.Euler(0, 0, 90) * ToTarget(player) * Sideness;
                    target += (Vector2)shimmyVector;
                }
                break;
            }
            case NavigationState.FleeingPlayer:
            {
                target += ToTarget(player) * Aggressiveness * DistanceFactor(player, PlayerDistanceMinMax, PlayerDistanceTowardBehavior);
                target -= ToTarget(player) * Shyness * 4;
                if (left)
                {
                    var shimmyVector = Quaternion.Euler(0, 0, -90) * ToTarget(player) * Sideness;
                    target += (Vector2)shimmyVector;
                }
                else
                {
                    var shimmyVector = Quaternion.Euler(0, 0, 90) * ToTarget(player) * Sideness;
                    target += (Vector2)shimmyVector;
                }
                break;
            }
            case NavigationState.Neutral:
            {
                target -= ToTarget(Vector2.zero) * Reset;
                break;
            }
            case NavigationState.Stationary:
            {
                target += ToTarget(StationaryPosition);
                target -= ToTarget(player) * Shyness * DistanceFactor(player, PlayerDistanceMinMax, PlayerDistanceTowardBehavior);
                break;
            }
        }

        Vector2 targetVelocity = target;
        if (targetVelocity.SqrMagnitude() > 1)
        {
            targetVelocity.Normalize();
        }
        Vector2 force = Speed * targetVelocity - rb.velocity;
        rb.AddForce(force * Acceleration);
    }

    float DistanceFactor(GameObject go, Vector2 distances, Vector2 multipliers)
    {
        return DistanceFactor(go.transform.position, distances, multipliers);
    }

    float DistanceFactor(Vector2 target, Vector2 distances, Vector2 multipliers)
    {
        float distance = (target - (Vector2)transform.position).magnitude;
        float slope = 1f / (distances.y - distances.x);
        float fromFactor = (distance - distances.x) * slope;
        return Mathf.Lerp(multipliers.x, multipliers.y, fromFactor);
    }

    Vector2 ToTarget(Vector2 target)
    {
        return target - (Vector2)transform.position;
    }

    Vector2 ToTarget(GameObject go)
    {
        return go.transform.position - transform.position;
    }
}
