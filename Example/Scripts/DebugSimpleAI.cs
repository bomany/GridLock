using UnityEngine;
using UnityEditor;

namespace Grid.Example
{
    [CustomEditor(typeof(SimpleAI))]
    public class DebugSimpleAI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SimpleAI script = (SimpleAI)target;
            if (GUILayout.Button("Find Pickup"))
            {
                script.FindNearestPickUp();
            }
            if (GUILayout.Button("Calculate Path"))
            {
                script.CalculatePath();
            }
            if (GUILayout.Button("Move"))
            {
                script.MoveTo();
            }
        }
    }
}