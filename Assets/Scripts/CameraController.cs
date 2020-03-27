using UnityEngine;

public class CameraController : MonoBehaviour
{
    // private Transform camTransform;
    private Camera _camera;

    private float currentY = 0;
    private float currentX = 0;

    public Transform player;
    public Vector3 distanceFromPlayer = new Vector3(0, 3, 8);
    public float tilt = 10;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    { }

    private void LateUpdate()
    {
        var camPosition = player.position;
        camPosition += Vector3.back * distanceFromPlayer.z + Vector3.up * distanceFromPlayer.y;
        _camera.transform.position = camPosition;
        _camera.transform.rotation = Quaternion.AngleAxis(tilt, Vector3.right);
        //  _camera.transform.rotation = camPosition;
        // var dir = Vector3.forward * distanceFromLookAt;
        // var camRotation = Quaternion.Euler(currentY,currentX,0)
    }
}