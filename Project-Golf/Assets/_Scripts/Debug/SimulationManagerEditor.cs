using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SimulationManager))]
public class SimulationManagerEditor : Editor
{
    private SimulationManager _simulationManager;

    private void OnEnable()
    {
        _simulationManager = (SimulationManager) target;
    }

    private void Button(string text, Action function, string tooltip = "", bool center = false)
    {
        GUILayout.Space(10);
        if (center) GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(text, tooltip))) function();
        GUILayout.FlexibleSpace();
        if (center) GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(15);
        EditorGUILayout.HelpBox("Following buttons only work in play mode.", MessageType.Warning);
        GUILayout.Space(15);
        Button("Start Simulation", _simulationManager.StartSimulation, "Starts the simulation");
        Button("Stop Simulation", _simulationManager.StopSimulation, "Stops the simulation");
        Button("Shoot Asteroid", _simulationManager.ShootAsteroid, "Shoots an asteroid");
    }
}
#endif