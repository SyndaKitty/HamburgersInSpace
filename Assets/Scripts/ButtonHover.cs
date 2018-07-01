using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Pointer;
    SpriteRenderer srenderer;

    void Awake()
    {
        srenderer = Pointer.GetComponent<SpriteRenderer>();
        srenderer.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (srenderer != null)
            srenderer.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (srenderer != null)
            srenderer.enabled = false;
    }
}
