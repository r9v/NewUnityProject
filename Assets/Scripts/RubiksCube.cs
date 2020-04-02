using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Axis
{
    X, Y, Z
}

public struct FaceData
{
    public readonly string faceName;
    public readonly int faceX;
    public readonly int faceY;
    public readonly Color pixelColor;

    public FaceData(string faceName, int faceX, int faceY, Color pixelColor)
    {
        this.faceName = faceName;
        this.faceX = faceX;
        this.faceY = faceY;
        this.pixelColor = pixelColor;
    }
}

public struct Rotation
{
    public readonly Axis axis;
    public readonly int dir;

    public Rotation(Axis axis, int dir)
    {
        this.axis = axis;
        this.dir = dir;
    }
}

public class Cubie
{
    public readonly GameObject go;
    public readonly List<Rotation> rotations = new List<Rotation>();

    public Cubie(GameObject go)
    {
        this.go = go;
    }

    public void RotateAround(Axis axis, Vector3 axis2, Vector3 pivot, int dir)
    {
        go.transform.RotateAround(pivot, axis2, dir * 90);
        rotations.Add(new Rotation(axis, dir));
    }
}

public class RubiksCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject manipulationAreaPrefab;
    public GameObject cubeMap;

    private readonly uint CUBE_SIZE = 3;
    private Cubie[][][] _qubies;
    private CubeRotator _cubeRotator;

    private void Awake()
    {
        Instantiate(manipulationAreaPrefab, transform);
        CreateQubies();
        _cubeRotator = new CubeRotator(_qubies, transform.position, transform.right, transform.up, transform.forward);
        ColorCubeMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _cubeRotator.Rotate(0, Axis.Z);
        }
    }

    private void ColorCubeMap()
    {
        var a = 0;
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    if (CubeUtils.IsInternalQb(x, y, z, CUBE_SIZE)) continue;

                    var faceData = GetFaceData(_qubies[x][y][z], x, y, z);
                    a += faceData.Count;
                    faceData.ForEach((f) =>
                    {
                        ColorCubeMapFace(f.faceName, f.faceX, f.faceY, f.pixelColor);
                    });
                }
            }
        }
        print(a);
    }

    private List<FaceData> GetFaceData(Cubie qb, int x, int y, int z)
    {
        var ret = new List<FaceData>();
        if (x == 0)
            ret.Add(new FaceData("Left", y, z, GetSideColor("Left", qb)));
        if (x == CUBE_SIZE - 1)
            ret.Add(new FaceData("Right", y, z, Color.white));
        if (y == 0)
            ret.Add(new FaceData("Down", x, z, Color.black));
        if (y == CUBE_SIZE - 1)
            ret.Add(new FaceData("Up", x, z, Color.blue));
        if (z == 0)
            ret.Add(new FaceData("Back", x, y, Color.red));
        if (z == CUBE_SIZE - 1)
            ret.Add(new FaceData("Front", x, y, Color.yellow));
        return ret;
    }

    private Color GetSideColor(string side, Cubie qb)
    {
        return Color.green;
    }

    private void ColorCubeMapFace(string faceName, int x, int y, Color color)
    {
        var face = cubeMap.transform.Find(faceName);
        var pixel = face.GetChild((int)(x * CUBE_SIZE) + y);
        pixel.GetComponent<Image>().color = color;
    }

    private void CreateQubies()
    {
        _qubies = new Cubie[CUBE_SIZE][][];
        var cubePrefabSize = GetObjectBound(cubePrefab).size;
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            _qubies[x] = new Cubie[CUBE_SIZE][];
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                _qubies[x][y] = new Cubie[CUBE_SIZE];
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    var offset = (CUBE_SIZE - 1f) / 2f;
                    var pos = new Vector3((x - offset) * cubePrefabSize.x,
                        (y - offset) * cubePrefabSize.y,
                        (z - offset) * cubePrefabSize.z) + transform.position;
                    var rot = Quaternion.identity;
                    var go = Instantiate(cubePrefab, pos, rot, transform);
                    go.name = x + "," + y + "," + z;
                    _qubies[x][y][z] = new Cubie(go);
                }
            }
        }
    }

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

public static class CubeUtils
{
    public static bool IsInternalQb(int x, int y, int z, uint cubeSize)
    {
        if (x == 0 || x == cubeSize - 1) return false;
        if (y == 0 || y == cubeSize - 1) return false;
        if (z == 0 || z == cubeSize - 1) return false;
        return true;
    }
}

public class CubeRotator
{
    private bool _rotating;

    private Cubie[][][] _qubies;

    private Vector3 _pivot;
    private Vector3 _right;
    private Vector3 _up;
    private Vector3 _forward;

    public CubeRotator(Cubie[][][] qubies, Vector3 pivot, Vector3 right, Vector3 up, Vector3 forward)
    {
        _qubies = qubies;
        _pivot = pivot;
        _right = right;
        _up = up;
        _forward = forward;
    }

    public void Rotate(int slice, Axis axis, int dir = 1)
    {
        if (_rotating) return;
        _rotating = true;
        switch (axis)
        {
            case Axis.X:
                RotateX(slice, dir);
                break;

            case Axis.Y:
                RotateY(slice, dir);
                break;

            case Axis.Z:
                RotateZ(slice, dir);
                break;
        }
        _rotating = false;
    }

    private void RotateX(int slice, int dir)
    {
        for (int y = 0; y < _qubies.Length; y++)
            for (int z = 0; z < _qubies.Length; z++)
                _qubies[slice][y][z].RotateAround(Axis.X, _right, _pivot, dir);

        var toRotate = new Cubie[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new Cubie[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[slice][i][j];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void RotateY(int slice, int dir)
    {
        for (int x = 0; x < _qubies.Length; x++)
            for (int z = 0; z < _qubies.Length; z++)
                _qubies[x][slice][z].RotateAround(Axis.Y, _up, _pivot, dir);

        var toRotate = new Cubie[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new Cubie[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[i][slice][j];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void RotateZ(int slice, int dir)
    {
        for (int y = 0; y < _qubies.Length; y++)
            for (int x = 0; x < _qubies.Length; x++)
                _qubies[x][y][slice].RotateAround(Axis.Z, _forward, _pivot, dir);

        var toRotate = new Cubie[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new Cubie[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[i][j][slice];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void InplaceRotate(Cubie[][] mat, int slice)
    {
        var n = mat.Length;
        for (var x = 0; x < n / 2; x++)
        {
            for (var y = x; y < n - x - 1; y++)
            {
                if (CubeUtils.IsInternalQb(x, y, slice, (uint)n)) continue;
                var temp = mat[x][y];
                mat[x][y] = mat[y][n - 1 - x];
                mat[y][n - 1 - x] = mat[n - 1 - x][n - 1 - y];
                mat[n - 1 - x][n - 1 - y] = mat[n - 1 - y][x];
                mat[n - 1 - y][x] = temp;
            }
        }
    }
}