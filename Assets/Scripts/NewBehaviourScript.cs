using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class NewBehaviourScript : MonoBehaviour
{
    public Transform archon;
    public GameObject qubiePrefab = null;
    public float qubieLength = 1;
    private GameObject[,,] qubies = new GameObject[3, 3, 3];

    private void Start()
    {
        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var z = 0; z < 3; z++)
                {
                    qubies[x, y, z] = GameObject.Instantiate(qubiePrefab);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}