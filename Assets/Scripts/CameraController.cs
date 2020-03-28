using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public new Camera camera;
    public Transform player;
    public Vector3 distanceFromPlayer = new Vector3(0, 3, -8);
    public float sidewayTurnSpeed = 250;
    public float upDownTurnSpeed = 100;

    private Quaternion turnAngle;

    private void Start()
    {
        turnAngle = player.rotation;
    }

    private void LateUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        camera.transform.position = player.position;

        turnAngle *= Quaternion.Euler(Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime,
            Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime, 0);
        camera.transform.position += turnAngle * distanceFromPlayer;

        camera.transform.LookAt(player);
    }
}