using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody player;
    public float moveSpeed = 3;

    private void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        Move(input.normalized);
    }

    private void Move(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            player.velocity = Vector3.zero;
            return;
        };
        player.transform.rotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var forwardVel = player.transform.forward * dir.x * moveSpeed;
        var sidewayVel = player.transform.right * dir.y * moveSpeed;
        player.velocity = sidewayVel + forwardVel;
    }
}