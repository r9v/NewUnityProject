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

    private float currentYAngle = 0;
    private float currentXAngle = 0;

    private void LateUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        camera.transform.position = player.position + player.forward * distanceFromPlayer;
        currentYAngle += Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime;
        currentXAngle += Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime;

        camera.transform.RotateAround(player.position, Vector3.up, currentYAngle);
        camera.transform.RotateAround(player.position, Vector3.right, currentXAngle);

        //  turnAngle *= Quaternion.Euler(Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime,
        //     Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime, 0);
        // camera.transform.position = player.position + turnAngle * new Vector3(0, 0, -distanceFromPlayer);

        camera.transform.LookAt(player);
    }
}