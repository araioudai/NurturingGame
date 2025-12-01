using Unity.VisualScripting;
using UnityEngine;

public enum Info
{
    GROUND,
    ROAD,
    START,
    GOAL,
    TOWER,
    OBSTACLE,
}


[CreateAssetMenu(fileName = "StageInfo", menuName = "Scriptable Objects/StageInfo")]
public class StageInfo : ScriptableObject
{
    public TextAsset csv;
    public int lines;
    public int columns;
    public int[][] map;
    public GameObject[] stageObj;
}
