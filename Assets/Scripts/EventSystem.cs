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

    public event Action<float> onPlayerCameraRotated;

    public void PlayerCameraRotated(float cameraYAngle)
    {
        if (onPlayerCameraRotated != null) // no c# 6 ;(
            onPlayerCameraRotated.Invoke(cameraYAngle);
    }

    public event Action onPlayerEnteredRubiksCubeManipulationArea;

    public void PlayerEnteredRubiksCubeManipulationArea()
    {
        if (onPlayerEnteredRubiksCubeManipulationArea != null) // no c# 6 ;(
            onPlayerEnteredRubiksCubeManipulationArea.Invoke();
    }

    public event Action onPlayerLeftRubiksCubeManipulationArea;

    public void PlayerLeftRubiksCubeManipulationArea()
    {
        if (onPlayerLeftRubiksCubeManipulationArea != null) // no c# 6 ;(
            onPlayerLeftRubiksCubeManipulationArea.Invoke();
    }
}