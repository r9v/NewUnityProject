﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject manipulationAreaPrefab;
    public GameObject cubeMap;

    private bool _rotating;
    private readonly uint CUBE_SIZE = 3;
    private GameObject[,,] _qubies = new GameObject[3, 3, 3];

    private void Start()
    {
        Instantiate(manipulationAreaPrefab, transform);
        MakeCube();
        ColorCubeMap();
    }

    private void ColorCubeMap()
    {
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    if (IsInternalQb(x, y, z)) continue;

                    var qb = _qubies[x, y, z];
                    // if (x != 0) continue;

                    //check what sides the qb belongs to
                    dick.Add(new Foo("Front", x, y),
                        qb.transform.Find("Front").GetComponent<Renderer>().material.color);
                }
            }
        }
    }

    private void ColorCubeMapFace(string faceName, int x, int y, Color color)
    {
        var cubeMapFace = cubeMap.transform.Find(faceName);
        cubeMapFace.GetChild((int)(x * CUBE_SIZE) + y)
            .GetComponent<Image>().color = color;
    }

    private bool IsInternalQb(int x, int y, int z)
    {
        if (x == 0 || x == CUBE_SIZE - 1) return false;
        if (y == 0 || y == CUBE_SIZE - 1) return false;
        if (z == 0 || z == CUBE_SIZE - 1) return false;
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Rotate(0, Axis.Z);
        }
    }

    private void MakeCube()
    {
        var cubePrefabSize = GetObjectBound(cubePrefab).size;
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    //todo: use CUBE_SIZE here
                    var offset = (CUBE_SIZE - 1f) / 2f;
                    var pos = new Vector3((x - offset) * cubePrefabSize.x,
                        (y - offset) * cubePrefabSize.y,
                        (z - offset) * cubePrefabSize.z) + transform.position;
                    var rot = Quaternion.identity;
                    var qb = Instantiate(cubePrefab, pos, rot, transform);
                    qb.name = x + "," + y + "," + z;
                    _qubies[x, y, z] = qb;
                }
            }
        }
    }

    private void Rotate(int slice, Axis axis, bool clockwise = true)
    {
        if (_rotating) return;
        _rotating = true;
        switch (axis)
        {
            case Axis.X:
                RotateX(slice, clockwise);
                break;

            case Axis.Y:
                RotateY(slice, clockwise);
                break;

            case Axis.Z:
                RotateZ(slice, clockwise);
                break;
        }
        _rotating = false;
    }

    private void RotateX(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int y = 0; y < _qubies.GetLength(1); y++)
        {
            for (int z = 0; z < _qubies.GetLength(2); z++)
            {
                _qubies[slice, y, z].transform.RotateAround(transform.position,
                   transform.right, dir * 90);
            }
        }
    }

    private void RotateY(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int x = 0; x < _qubies.GetLength(0); x++)
        {
            for (int z = 0; z < _qubies.GetLength(2); z++)
            {
                _qubies[x, slice, z].transform.RotateAround(transform.position,
                   transform.up, dir * 90);
            }
        }
    }

    private void RotateZ(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int y = 0; y < _qubies.GetLength(1); y++)
        {
            for (int x = 0; x < _qubies.GetLength(0); x++)
            {
                _qubies[x, y, slice].transform.RotateAround(transform.position,
                  -transform.forward, dir * 90);
            }
        }
    }

    //get rid of this
    private Bounds GetObjectBound(GameObject go)
    {
        MeshRenderer[] mfs = go.GetComponentsInChildren<MeshRenderer>();

        if (mfs.Length > 0)
        {
            Bounds b = mfs[0].bounds;
            for (int i = 1; i < mfs.Length; i++)
            {
                b.Encapsulate(mfs[i].bounds);
            }
            return b;
        }
        else
            return new Bounds();
    }
}

public enum Axis
{
    X, Y, Z
}