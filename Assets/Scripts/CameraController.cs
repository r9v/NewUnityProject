﻿using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Transform))]
public class CameraController : MonoBehaviour
{
    public new Camera camera;
    public Transform player;
    public Vector3 distanceFromPlayer = new Vector3(0, 3, -8);
    public float tilt = 10;

    private void LateUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        camera.transform.position = player.position;

        camera.transform.position += player.rotation * distanceFromPlayer;
        //camera.transform.position += player.forward * distanceFromPlayer.z +
        //   player.up * distanceFromPlayer.y +
        //   player.right * distanceFromPlayer.x;

        camera.transform.LookAt(player);
        //  camera.transform.rotation = player.rotation;
    }
}