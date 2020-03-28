using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public new Camera camera;
    public Transform player;
    public Vector3 distanceFromPlayer = new Vector3(0, 3, -8);
    public float tilt = 10;
    public Quaternion turnAngle;

    private void Start()
    {
        turnAngle = camera.transform.rotation;
    }

    private void LateUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        camera.transform.position = player.position;

        turnAngle *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * 100, player.up);
        Debug.Log(turnAngle);
        camera.transform.position += turnAngle * distanceFromPlayer;
        camera.transform.LookAt(player);
    }
}