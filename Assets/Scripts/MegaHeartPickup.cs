using UnityEngine;

public class MegaHeartPickup: MonoBehaviour
{
    SpriteRenderer sr;
    AudioSource asource;
    bool active;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        asource = GetComponent<AudioSource>();
        active = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject.tag == "Player")
        {
            GameController.Instance.PickupHealthUpgrade();
            asource.Play();
            sr.enabled = false;
            Invoke("Die", 2);
            active = false;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
