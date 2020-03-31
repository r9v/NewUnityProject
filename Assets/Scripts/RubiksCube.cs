using System;
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
    private List<Face> _faces = new List<Face>(6);
    private GameObject[,,] _qubies = new GameObject[3, 3, 3];

    private void Start()
    {
        InitState();
        Instantiate(manipulationAreaPrefab, transform);

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
                    _qubies[x, y, z] = qb;
                }
            }
        }
    }

    private void InitState()
    {
        _faces[0] = new Face(CUBE_SIZE, Pixel.T());
        _faces[1] = new Face(CUBE_SIZE, Pixel.L());
        _faces[2] = new Face(CUBE_SIZE, Pixel.F());
        _faces[3] = new Face(CUBE_SIZE, Pixel.R());
        _faces[4] = new Face(CUBE_SIZE, Pixel.B());
        _faces[5] = new Face(CUBE_SIZE, Pixel.D());

        ColorCubeMap();
    }

    private void ColorCubeMap()
    {
        _faces.ForEach((face) =>
        {
            var panel = cubeMap.transform.GetChild(s);
            var panelChildIdx = 0;
            for (var i = 0; i < CUBE_SIZE; i++)
                for (var j = 0; j < CUBE_SIZE; j++)
                {
                    panel.GetChild(panelChildIdx++).GetComponent<Image>().color = state[s, i, j].color;
                }
        });
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

public class Face
{
    public List<Pixel> pixels;
    private uint _size;

    public Face(uint size, Pixel startingPixel)
    {
        pixels = new List<Pixel>((int)size);

        for (var i = 0; i < size; i++)
        {
            pixels.Add(startingPixel.Clone());
        }

        _size = size;
    }
}

public class Pixel
{
    private string _code;

    public readonly Color color;

    private Pixel(string code, Color color)
    {
        _code = code;
        this.color = color;
    }

    public static Pixel T()
    {
        return new Pixel("T", Color.red);
    }

    public static Pixel R()
    {
        return new Pixel("R", new Color32(255, 164, 15, 255));
    }

    public static Pixel F()
    {
        return new Pixel("F", Color.blue);
    }

    public static Pixel L()
    {
        return new Pixel("L", Color.white);
    }

    public static Pixel B()
    {
        return new Pixel("B", Color.green);
    }

    public static Pixel D()
    {
        return new Pixel("D", Color.yellow);
    }

    public Pixel Clone()
    {
        return new Pixel(_code, new Color(color.r, color.g, color.b, color.a));
    }
}