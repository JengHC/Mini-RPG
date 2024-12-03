using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public float damage;
    public float cool;
    public string animationName;
    public Sprite icon;
}
