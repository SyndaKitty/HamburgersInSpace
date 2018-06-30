using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Pickle : MonoBehaviour
{
    public float LifeTime = 3f;
    public float FadeOutTime = .5f;

    float currentLifetime;
    float currentFadeTime;
    SpriteRenderer sr;

    public void Initialize()
    {
        currentLifetime = LifeTime;
        currentFadeTime = FadeOutTime;
        sr = GetComponent<SpriteRenderer>();
        var newColor = sr.color;
        newColor.a = 1;
        sr.color = newColor;
    }

    void Start()
    {
        Initialize();
    }

    void Update ()
    {
        if (currentLifetime > 0)
        {
            currentLifetime -= Time.deltaTime;
        }
        else
        {
            currentFadeTime -= Time.deltaTime;
            if (currentFadeTime <= 0)
            {
                GameController.Deactivate(this);
                return;
            }
            var newColor = sr.color;
            newColor.a = currentFadeTime / FadeOutTime;
            sr.color = newColor;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentLifetime = 0;

        var unit = collision.collider.GetComponent<Unit>();
        if (unit == null)
        {
            return;
        }
        unit.Damage(1);
    }
}
