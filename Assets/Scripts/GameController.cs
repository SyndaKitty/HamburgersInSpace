using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool completedTutorial;
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
    public GameObject PlayerObject;
    public Text TutorialTextUI;
    public Canvas tutorialCanvas;
    public Image Life1;
    public Image Life2;
    public Image Life3;
    public AudioClip GameOverClip;

    HealthBar healthBar;
    int tutorialState;
    int lives;
    AudioSource audioSource;
    bool gameOver;

    // Tutorial variables
    Vector3 playerStart;
    int stringShow;
    string tutorialText;
    int collected;
    bool textPause;
    bool win;

    void Awake()
    {
        Instance = this;
        gamePaused = false;
        MainMenuPointer.SetActive(false);
        QuitGamePointer.SetActive(false);
        PauseCanvas.SetActive(false);
        QuitButton.onClick.AddListener(QuitGame);
        MenuButton.onClick.AddListener(GoToMainMenu);
        lives = 3;
        audioSource = GetComponent<AudioSource>();
        PlayerUnit.Initialize(false, PlayerDied, null);
    }

    void Start()
    {
        var healthBarObject = Instantiate(HealthBarPrefab, UICanvas.transform);
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.Initialize(PlayerUnit, HealthPointPrefab);

        playerStart = PlayerUnit.transform.position;
        if (!completedTutorial)
        {
            tutorialState = 1;
            SetTutorialText("Move around with W,A,S,D");
            UICanvas.SetActive(false);
        }
        else
        {
            TutorialComplete();
        }
    }

    void Update ()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
        {
            gamePaused = !gamePaused;
            PauseCanvas.SetActive(gamePaused);
            MainMenuPointer.SetActive(gamePaused);
            QuitGamePointer.SetActive(gamePaused);
            Time.timeScale = gamePaused ? 0f : 1;
        }

        if (gamePaused) return;

        if (!completedTutorial)
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
                TutorialComplete();
            }
        }
    }

    void TutorialComplete()
    {
        completedTutorial = true;
        tutorialState = 6;
        tutorialCanvas.gameObject.SetActive(false);
        WaveController.Instance.SpawnWave();
        UICanvas.gameObject.SetActive(true);
    }

    public void PlayerDied()
    {
        PlayerObject.SetActive(false);
        if (lives == 3)
        {
            Life3.gameObject.SetActive(false);
        }
        else if (lives == 2)
        {
            Life2.gameObject.SetActive(false);
        }
        else if (lives == 1)
        {
            Life1.gameObject.SetActive(false);
            if (!win)
            {
                GameOver();
                return;
            }
        }
        lives--;
        Invoke("SpawnPlayer", 3);
    }

    public void Win()
    {
        win = true;
        Invoke("GoToMainMenu", 4);
    }

    void SpawnPlayer()
    {
        PlayerUnit.Initialize(false, PlayerDied, null);
        PlayerUnit.SetInvincible(true);
        PlayerObject.transform.position = Vector3.zero;
        PlayerObject.SetActive(true);
    }

    void GameOver()
    {
        gameOver = true;
        audioSource.clip = GameOverClip;
        audioSource.Play();
        Invoke("GoToMainMenu", 3);
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
        while (pooledPickles.Count > 0)
        {
            var pickle = pooledPickles.Pop();
            Destroy(pickle.gameObject);
        }

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