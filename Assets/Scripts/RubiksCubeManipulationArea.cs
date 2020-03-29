using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCubeManipulationArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;
        EventSystem.Instance.PlayerEnteredRubiksCubeManipulationArea();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag != "Player") return;
        EventSystem.Instance.PlayerLeftRubiksCubeManipulationArea();
    }
}