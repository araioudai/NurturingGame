using System.Linq;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public StageInfo[] stageInfo;
    public GameObject[] stageObj;
    public Transform stage;

    [SerializeField] float objInterval = 2;
    [SerializeField] Vector3 genPos = Vector3.zero;

    void Awake()
    {
        if (stageInfo != null)
        {
            ImportCSV(stageInfo[0]);
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
        for(int i = 0; i < stageInfo[0].lines;  i++)
        {
            for(int j = 0; j < stageInfo[0].columns; j++)
            {
                float x = genPos.x + j * objInterval;
                float z = genPos.z - i * objInterval;

                int index = stageInfo[0].map[i][j];
                if (index >= 0 && index < stageObj.Length)
                {
                    if (stageObj[index] != null)
                    {
                        Instantiate(stageObj[index], new Vector3(x, genPos.y, z), Quaternion.identity, stage);
                    }
                }
               
            }
        }
    }
}
