using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 3;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;

        EventSystem.Instance.onPlayerCameraRotated += onPlayerCameraRotated;
    }

    private void FixedUpdate()
    {
        var input = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        var dir = input.normalized;
        var forwardVel = transform.forward * dir.x * moveSpeed;
        var sidewayVel = transform.right * dir.y * moveSpeed;
        rb.velocity = sidewayVel + forwardVel;
    }

    private void onPlayerCameraRotated(float cameraYAngle)
    {
        if (rb.velocity == Vector3.zero) return;
        transform.rotation = Quaternion.AngleAxis(cameraYAngle, Vector3.up);
    }

    private void OnDestroy()
    {
        EventSystem.Instance.onPlayerCameraRotated -= onPlayerCameraRotated;
    }
}