using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public new Camera camera;
    public Transform player;
    public Vector3 distanceFromPlayer = new Vector3(0, 3, -8);
    public float turnSpeed = 250;

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

        turnAngle *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * turnSpeed * Time.deltaTime, player.up);
        camera.transform.position += player.rotation * turnAngle * distanceFromPlayer;

        camera.transform.LookAt(player);
    }
}