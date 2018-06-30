using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    static Stack<GameObject> pooledPickles = new Stack<GameObject>();

    public bool gamePaused;
    public GameObject MainMenuPointer;
    public GameObject QuitGamePointer;
    public GameObject PauseCanvas;
    public Button QuitButton;
    public Button MenuButton;
    public GameObject HealthBarPrefab;
    public GameObject HealthPointPrefab;
    public GameObject UICanvas;
    public Unit PlayerUnit;

    HealthBar healthBar;

    void Awake()
    {
        Instance = this;
        gamePaused = false;
        MainMenuPointer.SetActive(false);
        QuitGamePointer.SetActive(false);
        PauseCanvas.SetActive(false);
        QuitButton.onClick.AddListener(QuitGame);
        MenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Start()
    {
        var healthBarObject = Instantiate(HealthBarPrefab, UICanvas.transform);
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.Initialize(PlayerUnit, HealthPointPrefab);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
        {
            gamePaused = !gamePaused;
            PauseCanvas.SetActive(gamePaused);
            MainMenuPointer.SetActive(gamePaused);
            QuitGamePointer.SetActive(gamePaused);
            Time.timeScale = gamePaused ? 0f : 1;
        }
    }

    private void LateUpdate()
    {
        healthBar.HealthUpdate();
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public static void Deactivate(Pickle pickle)
    {
        pooledPickles.Push(pickle.gameObject);
        pickle.gameObject.SetActive(false);
    }

    public static GameObject GetPickle(GameObject picklePrefab, Vector3 position, Quaternion rotation, bool enemy)
    {
        if (pooledPickles.Count > 0)
        {
            var pickleObject = pooledPickles.Pop();
            pickleObject.transform.position = position;
            pickleObject.transform.rotation = rotation;
            pickleObject.SetActive(true);
            pickleObject.layer = enemy ? 12 : 8;
            var pickle = pickleObject.GetComponent<Pickle>();
            pickle.Initialize();
            return pickleObject;
        }
        else
        {
            var pickleObject = Instantiate(picklePrefab, position, rotation);
            pickleObject.layer = enemy ? 12 : 8;
            return pickleObject;
        }
    }
}