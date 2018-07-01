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
    public Text TutorialTextUI;
    public Canvas tutorialCanvas;

    HealthBar healthBar;
    int tutorialState;

    // Tutorial variables
    Vector3 playerStart;
    int stringShow;
    string tutorialText;
    int collected;
    bool textPause;

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

        playerStart = PlayerUnit.transform.position;
        tutorialState = 1;
        SetTutorialText("Move around with W,A,S,D");
        healthBar.gameObject.SetActive(false);
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

        if (gamePaused) return;

        if (tutorialState < 6)
        {
            textPause = !textPause;
            if (stringShow < tutorialText.Length && textPause) stringShow++;
            if (tutorialState == 1)
            {
                if ((PlayerUnit.transform.position - playerStart).magnitude > 1)
                {
                    SetTutorialText("Left click to shoot");
                    tutorialState = 2;
                }
            }
            TutorialTextUI.text = tutorialText.Substring(0, stringShow);
        }
        else
        {

        }
    }

    void SetTutorialText(string text)
    {
        tutorialText = text;
        stringShow = 0;
    }

    public void PlayerShot()
    {
        if (tutorialState == 2)
        {
            tutorialState = 3;
            SetTutorialText("Block enemy fire with RMB");
        }
    }

    public void PlayerBlocked()
    {
        if (tutorialState == 3)
        {
            tutorialState = 4;
            SetTutorialText("Zoom in or out with the scroll wheel");
        }
    }

    public void PlayerScrolled()
    {
        if (tutorialState == 4)
        {
            tutorialState = 5;
            SetTutorialText("Pick up collectibles to heal or powerup");
            // TODO: Remove stub
            PlayerPickedUpCollectible();
            PlayerPickedUpCollectible();
            PlayerPickedUpCollectible();
            PlayerPickedUpCollectible();
        }
    }

    public void PlayerPickedUpCollectible()
    {
        if (tutorialState == 5)
        {
            collected++;
            if (collected == 4)
            {
                tutorialState = 6;
                tutorialCanvas.gameObject.SetActive(false);
                WaveController.Instance.SpawnWave();
                UICanvas.gameObject.SetActive(true);
            }
        }
    }

    void LateUpdate()
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