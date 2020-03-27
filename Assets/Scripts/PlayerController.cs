using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Quaternion _targetRotation;

    private int _forwardInput = 0;
    private int _sidewayInput = 0;
    private int _turnInput = 0;

    private Rigidbody _playerRb { get { return _player.GetComponent<Rigidbody>(); } }

    private void Start()
    {
        _player = GetComponent<Player>();
        _targetRotation = _playerRb.rotation;
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
            turnVel = _turnInput * _player.moveSpeed;
        }
        _playerRb.rotation *= Quaternion.Euler(turnVel, 0, 0);
    }

    private void Run()
    {
        var forwardVel = 0f;
        var sidewayVel = 0f;
        if (_forwardInput != 0)
        {
            forwardVel = _forwardInput * _player.moveSpeed;
        }
        if (_sidewayInput != 0)
        {
            sidewayVel = _sidewayInput * _player.moveSpeed;
        }
        _playerRb.velocity = new Vector3(sidewayVel, 0, forwardVel);
    }
}