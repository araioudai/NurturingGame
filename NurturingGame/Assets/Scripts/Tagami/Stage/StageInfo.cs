using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Scriptable Objects/StageInfo")]
public class StageInfo : ScriptableObject
{
    public enum Info
    {
        GROUND,
        ROAD,
        START,
        GOAL,
    }

    public TextAsset csv;
    public int lines;
    public int columns;
    public int[][] map;
}
