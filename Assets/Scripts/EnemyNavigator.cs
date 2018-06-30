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
    public float MaxForce;
    public float Acceleration;
    public Vector2 StartingProximityRadius;
    public Vector2 ProximityRadius;
    public Vector2 ShimmyRange;
    public float ShimmyTime;
    public float ShimmyAcceleration;

    Rigidbody2D rb;
    GameObject player;
    Vector2 force;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        StationaryPosition = transform.position;
        ProximityRadius = StartingProximityRadius;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        Vector2 force = Vector2.zero;
        if (player == null) State = NavigationState.Neutral;
        switch (State)
        {
            case NavigationState.SeekingPlayer:
            {
                var targetVelocity = player.transform.position - transform.position;
                force = (Vector2)targetVelocity - rb.velocity;
                break;
            }
            case NavigationState.FleeingPlayer:
            {
                var targetVelocity = transform.position - player.transform.position;
                force = (Vector2)targetVelocity - rb.velocity;
                break;
            }
            case NavigationState.Neutral:
            {
                var targetVelocity = Vector2.zero;
                force = (Vector2)targetVelocity - rb.velocity;
                break;
            }
            case NavigationState.ShimmyLeft:
            {
                var toPlayer = player.transform.position - transform.position;
                toPlayer.Normalize();
                var shimmyVector = Quaternion.Euler(0, 0, -90) * toPlayer * ShimmyAcceleration;
                force = (Vector2)shimmyVector - rb.velocity;
                break;
            }
            case NavigationState.ShimmyRight:
            {
                var toPlayer = player.transform.position - transform.position;
                toPlayer.Normalize();
                var shimmyVector = Quaternion.Euler(0, 0, 90) * toPlayer * ShimmyAcceleration;
                force = (Vector2)shimmyVector - rb.velocity;
                break;
            }
            case NavigationState.Stationary:
            {
                var targetVelocity = StationaryPosition - (Vector2)transform.position;
                force = (Vector2)targetVelocity - rb.velocity;
                break;
            }
        }

        force *= Acceleration;
        if (force.magnitude > MaxForce)
        {
            force = force.normalized * MaxForce;
        }
        rb.AddForce(force);
    }
}
