using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button StartButton;
    public Button QuitButton;
    public Animator FadeImageAnimator;
    public Image FadeImage;

    private void Awake()
    {
        StartButton.onClick.AddListener(StartGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
