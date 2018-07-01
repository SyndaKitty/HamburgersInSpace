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

    void Start()
    {
        StartCoroutine(DeactivateFadeImage());
    }

    IEnumerator DeactivateFadeImage()
    {
        yield return new WaitUntil(() => FadeImage.color.a == 0);
        FadeImage.gameObject.SetActive(false);
    }

    void StartGame()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        FadeImage.gameObject.SetActive(true);
        FadeImageAnimator.SetBool("Fade", true);
        yield return new WaitUntil(() => FadeImage.color.a == 1);
        SceneManager.LoadSceneAsync(1);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
