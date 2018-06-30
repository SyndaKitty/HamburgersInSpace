using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    static Stack<GameObject> pooledPickles = new Stack<GameObject>();

    public Button StartButton;
    public Button QuitButton;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        StartButton.onClick.AddListener(StartGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    void Start ()
    {
        
    }
    
    void Update ()
    {
        
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    public static void Deactivate(Pickle pickle)
    {
        pooledPickles.Push(pickle.gameObject);
        pickle.gameObject.SetActive(false);
    }

    public static GameObject GetPickle(GameObject picklePrefab, Vector3 position, Quaternion rotation)
    {
        if (pooledPickles.Count > 0)
        {
            var pickleObject = pooledPickles.Pop();
            pickleObject.transform.position = position;
            pickleObject.transform.rotation = rotation;
            pickleObject.SetActive(true);
            var pickle = pickleObject.GetComponent<Pickle>();
            pickle.Initialize();
            return pickleObject;
        }
        else
        {
            return Instantiate(picklePrefab, position, rotation);
        }
    }
}