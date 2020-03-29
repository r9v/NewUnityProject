using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public Transform player;
    public float distanceFromPlayer = 8;
    public float sidewayTurnSpeed = 250;
    public float upDownTurnSpeed = 100;

    private float pitch = 0;
    private float yaw = 0;
    private readonly Vector2 pitchMinMax = new Vector2(-40, 85);
    private readonly Vector2 zoomMinMax = new Vector2(0, 500);

    private void LateUpdate()
    {
        var mouseInput = MouseInput();
        pitch = Mathf.Clamp(pitch - mouseInput.x, pitchMinMax.x, pitchMinMax.y);
        yaw += mouseInput.y;
        distanceFromPlayer = Mathf.Clamp(distanceFromPlayer - mouseInput.z,
            zoomMinMax.x, zoomMinMax.y);

        transform.eulerAngles = new Vector3(pitch, yaw);
        transform.position = player.position - transform.forward *
            distanceFromPlayer + transform.up * 2;
        EventSystem.Instance.PlayerCameraRotated(transform.eulerAngles.y);
    }

    private Vector3 MouseInput()
    {
        return new Vector3(Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime,
            Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime,
            Input.GetAxisRaw("Mouse ScrollWheel") * 20);
    }
}