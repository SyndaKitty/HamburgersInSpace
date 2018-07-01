using UnityEngine;

public class BurgerDebris : MonoBehaviour
{
    public Sprite[] DebrisSprites;
    public Vector2 LifeTime;
    public float velocity;

    float lifeLeft;
    SpriteRenderer sr;
    AudioSource audioSource;
    Rigidbody2D rb;
    int index;
    public void Initialize(int index)
    {
        this.index = index;
        if (index == -1)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
            var vel = Random.onUnitSphere;
            vel.z = 0;
            rb.velocity = vel * velocity;
            var torque = Random.Range(.3f, 4f);
            if (Random.Range(0, 2) == 0) torque = -torque;

            rb.AddTorque(torque);

            sr = GetComponent<SpriteRenderer>();
            sr.sprite = DebrisSprites[index];
        }
        lifeLeft = Random.Range(LifeTime.x, LifeTime.y);
    }

    void Update()
    {
        if (index >= 0)
        {
            var c = Color.white;
            c.a = lifeLeft + 1;
            sr.color = c;
        }

        lifeLeft -= Time.deltaTime;
        if (lifeLeft < -1)
        {
            Destroy(gameObject);
        }
    }
}
