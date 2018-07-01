using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance { get; private set; }

    public GameObject StationaryEnemyPrefab;
    public GameObject MovingEnemyPrefab;

    HashSet<GameObject> livingEnemies = new HashSet<GameObject>();

    void Awake()
    {
        Instance = this;    
    }

    public void SpawnWave(Wave wave)
    {
        livingEnemies = new HashSet<GameObject>();
        int stationary = RandomFromRangeInt(wave.stationary);
        for (int i = 0; i < stationary; i++)
        {
            var enemyObject = Instantiate(StationaryEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            SetProperties(enemyObject, controller, unit, wave);
        }

        int moving = RandomFromRangeInt(wave.moving);
        for (int i = 0; i < moving; i++)
        {
            var enemyObject = Instantiate(MovingEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            SetProperties(enemyObject, controller, unit, wave);
        }
    }

    void SetProperties(GameObject enemyObject, EnemyController controller, Unit unit, Wave wave)
    {
        unit.StartingRateOfFire = RandomFromRange(wave.RateOfFire);
        unit.StartingHealth = RandomFromRangeInt(wave.Health);
        unit.StartingPickleVelocity = RandomFromRange(wave.PickleVelocity);
        controller.BurstRange = new Vector2();
        controller.BurstRange.x = RandomFromRange(wave.MinBurstSize);
        controller.BurstRange.y = controller.BurstRange.x + RandomFromRange(wave.BurstRange);
        controller.Initialize();

        livingEnemies.Add(enemyObject);
    }

    public void Death(GameObject enemy)
    {
        livingEnemies.Remove(enemy);
        if (livingEnemies.Count == 0)
        {
            // TODO: The wave is over
        }
    }

    float RandomFromRange(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    int RandomFromRangeInt(Vector2 range)
    {
        return Random.Range(Mathf.FloorToInt(range.x), Mathf.CeilToInt(range.y + 1));
    }
}
