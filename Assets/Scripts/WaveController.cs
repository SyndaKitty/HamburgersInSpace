using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance { get; private set; }

    public GameObject StationaryEnemyPrefab;
    public GameObject MovingEnemyPrefab;

    HashSet<GameObject> livingEnemies = new HashSet<GameObject>();
    List<Transform> spawningLocations;

    void Awake()
    {
        Instance = this;
        spawningLocations = gameObject.GetComponentsInChildren<Transform>().ToList();
        spawningLocations.Remove(gameObject.transform);
    }

    public void SpawnWave(Wave wave)
    {
        HashSet<int> remainingLocations = new HashSet<int>();
        for (int i = 0; i < spawningLocations.Count; i++)
        {
            remainingLocations.Add(i);
        }

        livingEnemies = new HashSet<GameObject>();
        int stationary = RandomFromRangeInt(wave.stationary);
        for (int i = 0; i < stationary; i++)
        {
            var enemyObject = Instantiate(StationaryEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            var navigator = enemyObject.GetComponent<EnemyNavigator>();
            SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, true);
        }

        int moving = RandomFromRangeInt(wave.moving);
        for (int i = 0; i < moving; i++)
        {
            var enemyObject = Instantiate(MovingEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            var navigator = enemyObject.GetComponent<EnemyNavigator>();
            SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, false);
        }
    }

    void SetProperties(HashSet<int> remainingLocations, Wave wave, GameObject enemyObject, EnemyController controller, Unit unit, EnemyNavigator navigator, bool stationary)
    {
        int remainingIndex = Random.Range(0, remainingLocations.Count);
        int spawningIndex = remainingLocations.ToArray()[remainingIndex];
        var spawningLocation = spawningLocations[spawningIndex];
        remainingLocations.Remove(spawningIndex);
        controller.spawningIndex = spawningIndex;

        enemyObject.transform.position = spawningLocation.position;
        navigator.StationaryPosition = spawningLocation.position;

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
            Debug.Log("Wave over");
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
