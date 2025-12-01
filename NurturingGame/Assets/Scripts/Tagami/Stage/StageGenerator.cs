using System.Linq;
using UnityEditor;
using UnityEngine;
using static StageInfo;

public class StageGenerator : MonoBehaviour
{
    public StageInfo[] stageInfo;
    public Transform stage;
    public int stageIndex = 0;

    [SerializeField] float objInterval = 2;
    [SerializeField] Vector3 genPos = Vector3.zero;

    const float rote = 90;

    void Awake()
    {
        if (stageInfo != null)
        {
            ImportCSV(stageInfo[stageIndex]);
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
        StageInfo currentStage = stageInfo[stageIndex];

        for (int i = 0; i < currentStage.lines; i++)
        {
            for (int j = 0; j < currentStage.columns; j++)
            {
                float x = genPos.x + j * objInterval;
                float z = genPos.z - i * objInterval;

                int index = currentStage.map[i][j];
                if (index >= 0 && index < currentStage.stageObj.Length)
                {
                    if (currentStage.stageObj[index] != null)
                    {

                        if (index == (int)Info.START)
                        {
                            Instantiate(currentStage.stageObj[index], new Vector3(x, genPos.y, z), Quaternion.Euler(-rote, 0, -rote), stage);
                        }
                        else if (index == (int)Info.GOAL)
                        {
                            Instantiate(currentStage.stageObj[index], new Vector3(x, genPos.y, z), Quaternion.Euler(-rote, 0, rote), stage);
                        }
                        else
                        {
                            Instantiate(currentStage.stageObj[index], new Vector3(x, genPos.y, z), Quaternion.identity, stage);
                        }
                    }
                }

            }
        }
    }
}
