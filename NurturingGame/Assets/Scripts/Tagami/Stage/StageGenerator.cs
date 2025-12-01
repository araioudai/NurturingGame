using System.Linq;
using UnityEditor;
using UnityEngine;
using static StageInfo;

public class StageGenerator : MonoBehaviour
{
    public StageInfo[] stageInfo;
    public GameObject[] stageObj;
    public Transform stage;
    public int stageNow = 0;

    [SerializeField] float objInterval = 2;
    [SerializeField] Vector3 genPos = Vector3.zero;

    const float rote = 90;

    void Awake()
    {
        if (stageInfo != null)
        {
            ImportCSV(stageInfo[stageNow]);
            Generator();
        }
    }

    void Update()
    {

    }

    private static void ImportCSV(StageInfo info)
    {
        string[] lines = info.csv.text.Replace("\r", "").Split('\n').
            Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        if (lines.Length == 0) return;

        int row = lines.Length;
        int col = lines[0].Split(',').Length;

        info.lines = row;
        info.columns = col;
        info.map = new int[row][];

        for (int y = 0; y < row; y++)
        {
            string[] cols = lines[y].Split(',');
            info.map[y] = new int[col];
            for (int x = 0; x < col; x++)
            {
                int.TryParse(cols[x], out info.map[y][x]);
            }
        }
    }

    private void Generator()
    {
        for(int i = 0; i < stageInfo[stageNow].lines;  i++)
        {
            for(int j = 0; j < stageInfo[stageNow].columns; j++)
            {
                float x = genPos.x + j * objInterval;
                float z = genPos.z - i * objInterval;

                int index = stageInfo[stageNow].map[i][j];
                if (index >= 0 && index < stageObj.Length)
                {
                    if (stageObj[index] != null)
                    {
                        
                        if(index == (int)Info.START)
                        {
                            Instantiate(stageObj[index], new Vector3(x, genPos.y, z), Quaternion.Euler(-rote,0,-rote), stage);
                        }
                        else if(index == (int)Info.GOAL)
                        {
                            Instantiate(stageObj[index], new Vector3(x, genPos.y, z), Quaternion.Euler(-rote, 0, rote), stage);
                        }
                        else
                        {
                            Instantiate(stageObj[index], new Vector3(x, genPos.y, z), Quaternion.identity, stage);
                        }
                    }
                }
               
            }
        }
    }
}
