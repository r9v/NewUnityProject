using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Axis
{
    X, Y, Z
}

public class RubiksCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject manipulationAreaPrefab;
    public GameObject cubeMap;

    private readonly uint CUBE_SIZE = 3;
    private GameObject[][][] _qubies;
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

    private class Foo
    {
        public string faceName;
        public int faceX;
        public int faceY;
        public Color pixelColor;

        public Foo(string faceName, int faceX, int faceY, Color pixelColor)
        {
            this.faceName = faceName;
            this.faceX = faceX;
            this.faceY = faceY;
            this.pixelColor = pixelColor;
        }
    }

    private void ColorCubeMap()
    {
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    if (CubeUtils.IsInternalQb(x, y, z, CUBE_SIZE)) continue;

                    var foo = GetQbFacePosition(_qubies[x][y][z]);
                    foo.ForEach((f) =>
                    {
                        ColorCubeMapFace(f.faceName, f.faceX, f.faceY, f.pixelColor);
                    });
                }
            }
        }
    }

    private System.Random random = new System.Random();

    private List<Foo> GetQbFacePosition(GameObject gameObject)
    {
        var faceName = "Front";
        var faceX = random.Next(0, 2);
        var faceY = random.Next(0, 2);
        var pixelColor = Color.green;
        var ret = new List<Foo>();
        ret.Add(new Foo(faceName, faceX, faceY, pixelColor));
        return ret;
    }

    private void ColorCubeMapFace(string faceName, int x, int y, Color color)
    {
        var face = cubeMap.transform.Find(faceName);
        var pixel = face.GetChild((int)(x * CUBE_SIZE) + y);
        pixel.GetComponent<Image>().color = color;
    }

    private void CreateQubies()
    {
        _qubies = new GameObject[CUBE_SIZE][][];
        var cubePrefabSize = GetObjectBound(cubePrefab).size;
        for (var x = 0; x < CUBE_SIZE; x++)
        {
            _qubies[x] = new GameObject[CUBE_SIZE][];
            for (var y = 0; y < CUBE_SIZE; y++)
            {
                _qubies[x][y] = new GameObject[CUBE_SIZE];
                for (var z = 0; z < CUBE_SIZE; z++)
                {
                    var offset = (CUBE_SIZE - 1f) / 2f;
                    var pos = new Vector3((x - offset) * cubePrefabSize.x,
                        (y - offset) * cubePrefabSize.y,
                        (z - offset) * cubePrefabSize.z) + transform.position;
                    var rot = Quaternion.identity;
                    var qb = Instantiate(cubePrefab, pos, rot, transform);
                    qb.name = x + "," + y + "," + z;
                    _qubies[x][y][z] = qb;
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

    private GameObject[][][] _qubies;

    private Vector3 _pivot;
    private Vector3 _right;
    private Vector3 _up;
    private Vector3 _forward;

    public CubeRotator(GameObject[][][] qubies, Vector3 pivot, Vector3 right, Vector3 up, Vector3 forward)
    {
        _qubies = qubies;
        _pivot = pivot;
        _right = right;
        _up = up;
        _forward = forward;
    }

    public void Rotate(int slice, Axis axis, bool clockwise = true)
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
        for (int y = 0; y < _qubies.Length; y++)
            for (int z = 0; z < _qubies.Length; z++)
                _qubies[slice][y][z].transform.RotateAround(_pivot, _right, dir * 90);

        var toRotate = new GameObject[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new GameObject[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[slice][i][j];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void RotateY(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int x = 0; x < _qubies.Length; x++)
            for (int z = 0; z < _qubies.Length; z++)
                _qubies[x][slice][z].transform.RotateAround(_pivot, _up, dir * 90);

        var toRotate = new GameObject[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new GameObject[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[i][slice][j];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void RotateZ(int slice, bool clockwise = true)
    {
        var dir = clockwise ? 1 : -1;
        for (int y = 0; y < _qubies.Length; y++)
            for (int x = 0; x < _qubies.Length; x++)
                _qubies[x][y][slice].transform.RotateAround(_pivot, _forward, dir * 90);

        var toRotate = new GameObject[_qubies.Length][];
        for (int i = 0; i < _qubies.Length; i++)
        {
            toRotate[i] = new GameObject[_qubies.Length];
            for (int j = 0; j < _qubies.Length; j++)
            {
                toRotate[i][j] = _qubies[i][j][slice];
            }
        }
        InplaceRotate(toRotate, slice);
    }

    private void InplaceRotate(GameObject[][] mat, int slice)
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