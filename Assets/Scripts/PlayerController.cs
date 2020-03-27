using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody player;
    public float moveSpeed = 3;
    public float turnSpeed = 70;

    private int _forwardInput = 0;
    private int _sidewayInput = 0;
    private int _turnInput = 0;

    private void Update()
    {
        GetInput();
        Trun();
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void GetInput()
    {
        _forwardInput = Convert.ToInt32(Input.GetAxisRaw("Vertical"));
        _sidewayInput = Convert.ToInt32(Input.GetAxisRaw("Horizontal"));
        if (Input.GetKey(KeyCode.E))
        {
            _turnInput = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _turnInput = -1;
        }
        else
        {
            _turnInput = 0;
        }
    }

    private void Trun()
    {
        var turnVel = 0f;
        if (_turnInput != 0)
        {
            turnVel = _turnInput * turnSpeed * Time.deltaTime;
        }
        player.rotation *= Quaternion.AngleAxis(turnVel, player.transform.up);
    }

    private void Run()
    {
        var forwardVel = Vector3.zero;
        var sidewayVel = Vector3.zero;
        if (_forwardInput != 0)
        {
            forwardVel = player.transform.forward * _forwardInput * moveSpeed;
        }
        if (_sidewayInput != 0)
        {
            sidewayVel = player.transform.right * _sidewayInput * moveSpeed;
        }
        player.velocity = sidewayVel + forwardVel;
    }
}