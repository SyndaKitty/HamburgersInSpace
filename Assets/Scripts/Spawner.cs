using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject spawnObject;
    float activateTime;
    SpriteRenderer srenderer;
    AudioSource audioSource;

    public void Initialize(GameObject spawnObject, float activateTime)
    {
        this.spawnObject = spawnObject;
        this.activateTime = activateTime;

        srenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        srenderer.enabled = false;
        Invoke("Activate", activateTime);
    }

    void Activate()
    {
        srenderer.enabled = true;
        audioSource.Play();
        Invoke("Spawn", 3);
    }

    void Spawn()
    {
        spawnObject.SetActive(true);
        Destroy(gameObject);
    }
}
