using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance { get; private set; }

    public GameObject StationaryEnemyPrefab;
    public GameObject MovingEnemyPrefab;
    public Wave[] waves;
    public AudioClip win;
    public GameObject SpawnerPrefab;

    HashSet<GameObject> livingEnemies = new HashSet<GameObject>();
    List<Transform> spawningLocations;
    AudioClip[] waveStartAudio;
    AudioSource audioSource;
    int currentWave;
    float waitTime;

    void Awake()
    {
        Instance = this;
        spawningLocations = gameObject.GetComponentsInChildren<Transform>().ToList();
        spawningLocations.Remove(gameObject.transform);
        audioSource = GetComponent<AudioSource>();
    }

    public void SpawnWave()
    {
        waitTime = 0;
        var wave = waves[currentWave++];
        audioSource.clip = wave.waveStartAudio; 
        audioSource.Play();

        HashSet<int> remainingLocations = new HashSet<int>();
        for (int i = 0; i < spawningLocations.Count; i++)
        {
            remainingLocations.Add(i);
        }

        livingEnemies = new HashSet<GameObject>();
        int stationary = RandomFromRangeInt(wave.stationary);
        int moving = RandomFromRangeInt(wave.moving);

        while (stationary > 0 && moving > 0)
        {
            int type = Random.Range(0, 2);
            if (type == 0)
            {
                var enemyObject = Instantiate(StationaryEnemyPrefab);
                var controller = enemyObject.GetComponent<EnemyController>();
                var unit = enemyObject.GetComponent<Unit>();
                var navigator = enemyObject.GetComponent<EnemyNavigator>();
                SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, true);
                stationary--;
            }
            else
            {
                var enemyObject = Instantiate(MovingEnemyPrefab);
                var controller = enemyObject.GetComponent<EnemyController>();
                var unit = enemyObject.GetComponent<Unit>();
                var navigator = enemyObject.GetComponent<EnemyNavigator>();
                SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, false);
                moving--;
            }
        }

        while (stationary > 0)
        {
            var enemyObject = Instantiate(StationaryEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            var navigator = enemyObject.GetComponent<EnemyNavigator>();
            SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, true);
            stationary--;
        }

        while (moving > 0)
        {
            var enemyObject = Instantiate(MovingEnemyPrefab);
            var controller = enemyObject.GetComponent<EnemyController>();
            var unit = enemyObject.GetComponent<Unit>();
            var navigator = enemyObject.GetComponent<EnemyNavigator>();
            SetProperties(remainingLocations, wave, enemyObject, controller, unit, navigator, false);
            moving--;
        }
    }

    void SetProperties(HashSet<int> remainingLocations, Wave wave, GameObject enemyObject, EnemyController controller, Unit unit, EnemyNavigator navigator, bool stationary)
    {
        waitTime += RandomFromRange(wave.EnemySpawnWait);
        int remainingIndex = Random.Range(0, remainingLocations.Count);
        int spawningIndex = remainingLocations.ToArray()[remainingIndex];
        var spawningLocation = spawningLocations[spawningIndex];
        remainingLocations.Remove(spawningIndex);
        controller.spawningIndex = spawningIndex;

        enemyObject.transform.position = spawningLocation.position;
        navigator.StationaryPosition = spawningLocation.position;
        navigator.Aggressiveness *= Random.Range(.5f, 2f);
        navigator.Shyness *= Random.Range(.5f, 2f);
        navigator.Sideness *= Random.Range(.5f, 2f);
        navigator.Acceleration *= Random.Range(.7f, 1.4f);
        navigator.Speed *= Random.Range(.7f, 1.4f);

        unit.StartingRateOfFire = RandomFromRange(wave.RateOfFire);
        unit.StartingHealth = RandomFromRangeInt(wave.Health);
        unit.StartingPickleVelocity = RandomFromRange(wave.PickleVelocity);
        controller.BurstRange = new Vector2();
        controller.BurstRange.x = RandomFromRange(wave.MinBurstSize);
        controller.BurstRange.y = controller.BurstRange.x + RandomFromRange(wave.BurstRange);
        controller.Initialize();

        enemyObject.SetActive(false);
        var spawnerObject = Instantiate(SpawnerPrefab);
        spawnerObject.transform.position = spawningLocation.position;
        var spawner = spawnerObject.GetComponent<Spawner>();
        spawner.Initialize(enemyObject, waitTime);

        livingEnemies.Add(enemyObject);
    }

    public void Death(GameObject enemy)
    {
        livingEnemies.Remove(enemy);
        if (livingEnemies.Count == 0)
        {
            if (currentWave < waves.Length)
            {
                Invoke("SpawnWave", 2f);
            }
            else
            {
                audioSource.clip = win;
                audioSource.Play();
                GameController.Instance.Win();
            }
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
