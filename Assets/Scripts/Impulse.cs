using UnityEngine;

public class Impulse : MonoBehaviour
{
    public float torque;
    void Awake()
    {
        GetComponent<Rigidbody2D>().AddTorque(torque);
    }
}
