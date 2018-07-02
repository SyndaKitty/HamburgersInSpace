using UnityEngine;
    
public class HealthPickup : MonoBehaviour
{
    SpriteRenderer sr;
    AudioSource asource;
    bool active;

    void Awake()
    {
        active = true;
        sr = GetComponent<SpriteRenderer>();
        asource = GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject.tag == "Player")
        {
            if (GameController.Instance.PickupHealth())
            {
                asource.Play();
                sr.enabled = false;
                Invoke("Die", 2);
                active = false;
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
