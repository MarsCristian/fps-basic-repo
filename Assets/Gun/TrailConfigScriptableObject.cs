using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName ="Trail Config",menuName ="Guns/Gun Trail Configuration", order = 4)]
public class TrailConfigScriptableObject : ScriptableObject
{
    public Material material;
    public AnimationCurve widithCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient color;

    public float missDistance = 100f;
    public float simulationSpeed = 100f;


}
