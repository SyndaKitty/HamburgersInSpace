using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration;
    public float Speed;

    Rigidbody2D rb;
    Unit unit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (GameController.Instance.gamePaused) return;
        unit.CustomUpdate();

        var targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 rightAnalog = new Vector2(Input.GetAxisRaw("RAnalogX"), Input.GetAxisRaw("RAnalogY"));
        Debug.Log(rightAnalog);
        var mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 force = Speed * targetVelocity - rb.velocity;
        rb.AddForce(force * Acceleration);

        if (Input.GetMouseButton(1))
        {
            unit.Shield(mouseTarget);
            GameController.Instance.PlayerBlocked();
        }
        else if ((Input.GetAxisRaw("TriggerL") > 0 || Input.GetAxisRaw("TriggerR") > 0) && rightAnalog != Vector2.zero)
        {
            unit.Shield(rightAnalog.normalized + (Vector2)transform.position);
            GameController.Instance.PlayerBlocked();
        }
        else if (Input.GetMouseButton(0))
        {
            unit.Shoot(mouseTarget);
            GameController.Instance.PlayerShot();
        }
        else if (rightAnalog != Vector2.zero)
        {
            unit.Shoot(rightAnalog.normalized + (Vector2)transform.position);
            GameController.Instance.PlayerShot();
        }
    }
}