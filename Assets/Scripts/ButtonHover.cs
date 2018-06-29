using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Pointer;
    SpriteRenderer renderer;

    void Awake()
    {
        renderer = Pointer.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        renderer.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        renderer.enabled = false;
    }
}
