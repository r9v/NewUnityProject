using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject manipulationAreaPrefab;
    private GameObject[,,] qubies = new GameObject[3, 3, 3];

    private void Start()
    {
        var ma = Instantiate(manipulationAreaPrefab, transform);

        var cubePrefabSize = GetObjectBound(cubePrefab).size;

        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var z = 0; z < 3; z++)
                {
                    if (x == 1 && y == 1 && z == 1) continue;
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
        for (int x = 0; x < qubies.GetLength(0); x++)
        {
            for (int y = 0; y < qubies.GetLength(1); y++)
            {
                if (!qubies[x, y, 0]) continue;
                qubies[x, y, 0].transform.Rotate(new Vector3(0, 0, Time.deltaTime * 10));
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