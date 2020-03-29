using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public Transform player;
    public float scroll = 8;
    public float yawSpeed = 250;
    public float pitchSpeed = 100;
    public float scrollSpeed = 30;

    private float pitch = 0;
    private float yaw = 0;
    private readonly Vector2 pitchMinMax = new Vector2(-40, 85);
    private readonly Vector2 zoomMinMax = new Vector2(0, 500);

    private void LateUpdate()
    {
        var mouseInput = MouseInput();
        pitch = Mathf.Clamp(pitch - mouseInput.x, pitchMinMax.x, pitchMinMax.y);
        yaw += mouseInput.y;
        scroll = Mathf.Clamp(scroll - mouseInput.z, zoomMinMax.x, zoomMinMax.y);

        transform.eulerAngles = new Vector3(pitch, yaw);
        transform.position = player.position - transform.forward *
            scroll + transform.up * 2;
        EventSystem.Instance.PlayerCameraRotated(transform.eulerAngles.y);
    }

    private Vector3 MouseInput()
    {
        return new Vector3(Input.GetAxis("Mouse Y") * pitchSpeed,
            Input.GetAxis("Mouse X") * yawSpeed,
            Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);
    }
}