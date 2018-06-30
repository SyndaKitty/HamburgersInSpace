using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    public Sprite FullSprite;
    public Sprite EmptySprite;

    Image image;    

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetFilled(bool filled)
    {
        image.sprite = filled ? FullSprite : EmptySprite;
    }
}
