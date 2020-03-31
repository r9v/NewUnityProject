using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event Action<float> OnPlayerCameraRotated;

    public void PlayerCameraRotated(float cameraYAngle)
    {
        if (OnPlayerCameraRotated != null) // no c# 6 ;(
            OnPlayerCameraRotated.Invoke(cameraYAngle);
    }

    public event Action OnPlayerEnteredRubiksCubeManipulationArea;

    public void PlayerEnteredRubiksCubeManipulationArea()
    {
        if (OnPlayerEnteredRubiksCubeManipulationArea != null) // no c# 6 ;(
            OnPlayerEnteredRubiksCubeManipulationArea.Invoke();
    }

    public event Action OnPlayerLeftRubiksCubeManipulationArea;

    public void PlayerLeftRubiksCubeManipulationArea()
    {
        if (OnPlayerLeftRubiksCubeManipulationArea != null) // no c# 6 ;(
            OnPlayerLeftRubiksCubeManipulationArea.Invoke();
    }
}