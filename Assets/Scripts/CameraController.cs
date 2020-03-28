using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public new Camera camera;
    public Transform player;
    public float distanceFromPlayer = 8;
    public float sidewayTurnSpeed = 250;
    public float upDownTurnSpeed = 100;

    private float pitch = 0;
    private float yaw = 0;

    private void LateUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime;
        yaw += Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime;
        camera.transform.eulerAngles = new Vector3(pitch, yaw);

        camera.transform.position = player.position - camera.transform.forward * distanceFromPlayer;

        //turnAngle *= Quaternion.Euler(foo, bar, 0);
        //camera.transform.position += turnAngle * distanceFromPlayer;

        //  camera.transform.LookAt(player);
    }
}