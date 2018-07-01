using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ZoomAcceleration;
    public float ZoomSpeed;
    public Vector2 ZoomMinMax;
    public GameObject target;

    float targetZoom;
    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        var deltaZoom = Input.GetAxisRaw("Mouse ScrollWheel");
        targetZoom -= deltaZoom * ZoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, ZoomMinMax.x, ZoomMinMax.y);
        float force = targetZoom - cam.orthographicSize;
        force *= ZoomAcceleration;
        float currentZoom = cam.orthographicSize;
        currentZoom += force * Time.deltaTime;
        cam.orthographicSize = currentZoom;

        if (target == null) return; 
        // TODO Smooth camera
        var focus = target.transform.position;
        focus.z = -10;
        transform.position = focus;
    }
}
