using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player _player;

    private int _forwardInput = 0;
    private int _sidewayInput = 0;
    private int _turnInput = 0;

    private Rigidbody _playerRb { get { return _player.GetComponent<Rigidbody>(); } }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

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
            turnVel = _turnInput * _player.turnSpeed * Time.deltaTime;
        }
        _playerRb.rotation *= Quaternion.AngleAxis(turnVel, _playerRb.transform.up);
    }

    private void Run()
    {
        var forwardVel = Vector3.zero;
        var sidewayVel = Vector3.zero;
        if (_forwardInput != 0)
        {
            forwardVel = _playerRb.transform.forward * _forwardInput * _player.moveSpeed;
        }
        if (_sidewayInput != 0)
        {
            sidewayVel = _playerRb.transform.right * _sidewayInput * _player.moveSpeed;
        }
        _playerRb.velocity = sidewayVel + forwardVel;
    }
}