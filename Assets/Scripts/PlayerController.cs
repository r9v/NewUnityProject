using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody player;
    public float moveSpeed = 3;

    private void Start()
    {
        EventSystem.Instance.onPlayerCameraRotated += onPlayerCameraRotated;
    }

    private void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        Move(input.normalized);
    }

    private void Move(Vector2 dir)
    {
        var forwardVel = player.transform.forward * dir.x * moveSpeed;
        var sidewayVel = player.transform.right * dir.y * moveSpeed;
        player.velocity = sidewayVel + forwardVel;
    }

    private void onPlayerCameraRotated(float cameraYAngle)
    {
        if (player.velocity == Vector3.zero)
        {
            return;
        };
        player.transform.rotation = Quaternion.AngleAxis(cameraYAngle, Vector3.up);
    }

    private void OnDestroy()
    {
        EventSystem.Instance.onPlayerCameraRotated -= onPlayerCameraRotated;
    }
}