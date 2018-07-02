using UnityEngine;
    
public class HealthPickup : MonoBehaviour
{
    SpriteRenderer sr;
    AudioSource asource;
    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        asource = GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameController.Instance.PickupHealth())
            {
                asource.Play();
                sr.enabled = false;
                Invoke("Die", 2);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
