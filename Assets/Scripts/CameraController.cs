﻿using UnityEngine;

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
    private readonly Vector2 pitchMinMax = new Vector2(-40, 85);

    private void LateUpdate()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * upDownTurnSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        yaw += Input.GetAxisRaw("Mouse X") * sidewayTurnSpeed * Time.deltaTime;
        distanceFromPlayer -= Input.GetAxisRaw("Mouse ScrollWheel") * 20;

        camera.transform.eulerAngles = new Vector3(pitch, yaw);
        camera.transform.position = player.position - camera.transform.forward * distanceFromPlayer + camera.transform.up * 2;
        EventSystem.Instance.PlayerCameraRotated(camera.transform.eulerAngles.y);
    }
}