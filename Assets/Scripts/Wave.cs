using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public Vector2 stationary;
    public Vector2 moving;
    public Vector2 RateOfFire;
    public Vector2 Health;
    public Vector2 PickleVelocity;
    public Vector2 MinBurstSize;
    public Vector2 BurstRange;
    public Vector2 BurstTimeout;
    public Vector2 EnemySpawnWait;
    public AudioClip WaveStartAudio;
    public bool MegaHeart;
}
