using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject manipulationAreaPrefab;

    private readonly uint CUBE_SIZE = 3;
    private Side[,,] state;

    private void InitState()
    {
        state = new Side[6, CUBE_SIZE, CUBE_SIZE];
        for (var s = 0; s < 6; s++)
            for (var i = 0; i < CUBE_SIZE; i++)
                for (var j = 0; j < CUBE_SIZE; j++)
                    state[s, i, j] = (Side)s;
    }

    private GameObject[,,] qubies = new GameObject[3, 3, 3];

    private bool rotating;

    private void Start()
    {
        InitState();
        var ma = Instantiate(manipulationAreaPrefab, transform);

        var cubePrefabSize = GetObjectBound(cubePrefab).size;

        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var z = 0; z < 3; z++)
                {
                    // if (x == 1 && y == 1 && z == 1) continue;
                    var pos = new Vector3((x - 1) * cubePrefabSize.x,
                        (y - 1) * cubePrefabSize.y,
                        (z - 1) * cubePrefabSize.z) + transform.position;
                    var rot = Quaternion.identity;
                    var qb = Instantiate(cubePrefab, pos, rot, transform);
                    qb.name = "x:" + x + " y:" + y + " z:" + z;
                    qubies[x, y, z] = qb;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Rotate(0, Axis.Z);
        }
    }

    private void Rotate(int slice, Axis axis, bool clockwise = true)
    {
        if (rotating) return;
        rotating = true;
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
        rotating = false;
    }

    private void RotateX(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int y = 0; y < qubies.GetLength(1); y++)
        {
            for (int z = 0; z < qubies.GetLength(2); z++)
            {
                qubies[slice, y, z].transform.RotateAround(transform.position,
                   transform.right, dir * 90);
            }
        }
    }

    private void RotateY(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int x = 0; x < qubies.GetLength(0); x++)
        {
            for (int z = 0; z < qubies.GetLength(2); z++)
            {
                qubies[x, slice, z].transform.RotateAround(transform.position,
                   transform.up, dir * 90);
            }
        }
    }

    private void RotateZ(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int y = 0; y < qubies.GetLength(1); y++)
        {
            for (int x = 0; x < qubies.GetLength(0); x++)
            {
                qubies[x, y, slice].transform.RotateAround(transform.position,
                  -transform.forward, dir * 90);
            }
        }
    }

    public Bounds GetObjectBound(GameObject go)
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

public enum Side
{
    U, L, F, R, B, D
}