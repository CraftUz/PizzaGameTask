using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/New Level Data", fileName = "New Level Data")]
public class LevelMovementData : ScriptableObject
{
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
}
