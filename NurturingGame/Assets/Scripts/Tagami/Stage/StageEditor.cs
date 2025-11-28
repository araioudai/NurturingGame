using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Linq;
using static StageInfo;

public class StageEditor : EditorWindow
{
    private StageInfo stageInfo;
    private SerializedObject serializedObject;
    private TextAsset csvFile;

    private Vector2 scroll;     // ?X?N???[???p

    private int cellSize = 32;  // ?Z???????
    private bool isMouseDown = false;
    private int currentPaintValue = 0; // ?I???^?C???l

    private Color[] colors;
    private void OnEnable()
    {
        colors = new Color[]
       {
            new Color(0.59f, 0.29f, 0.0f),  // GROUND
            new Color(1f, 1f, 0.3f),        // ROAD
            new Color(0.3f, 1f, 0.3f),      // START
            new Color(1f, 0.3f, 0.3f),      // GOAL
       };

        wantsLessLayoutEvents = true;  // ???C?A?E?g?C?x???g??
        wantsMouseMove = false;        // ?}?E?X???[?u??
    }

    [MenuItem("Window/Stage Editor")]

    public static void Open()
    {
        GetWindow<StageEditor>("Stage Editor");
    }

    private void OnGUI()
    {
        var e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
            isMouseDown = true;
        if (e.type == EventType.MouseUp)
            isMouseDown = false;

        // csvFile?I??.
        StageInfo newData = (StageInfo)EditorGUILayout.ObjectField("csvFile", stageInfo, typeof(StageInfo), false);
        
        // ?I?????????f?[?^????????.
        if(newData != stageInfo)
        {
            stageInfo = newData;
            serializedObject = (stageInfo != null) ? new SerializedObject(stageInfo) : null;
        }

        // csvFile??serializedObject??null???l????.
        if (stageInfo == null || serializedObject == null) return;

        GUILayout.Space(10);

        // csvFile???W?t?B?[???h.
        serializedObject.Update();

        int oldLines = stageInfo.lines;
        int oldColumns = stageInfo.columns;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("lines"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("columns"));

        // ???.
        serializedObject.ApplyModifiedProperties();

        if ((stageInfo.lines != oldLines) || (stageInfo.columns != oldColumns))
        {
            ResizeMap(stageInfo);
            EditorUtility.SetDirty(stageInfo);
        }

        GUILayout.Space(10);

        if (stageInfo.csv == null) return;
        csvFile = stageInfo.csv;

        // ??????{?^????????????.
        if (GUILayout.Button("CSVì«çû"))
        {
            // ?????????
            ImportCSV(csvFile, stageInfo);
            Repaint();
        }

        // ????{?^????????????.
        if (GUILayout.Button("CSVèëèo"))
        {
            // ???????.
            Export(stageInfo);
        }

        if (stageInfo.map == null) return;

        GUILayout.Space(30);

        // ?^?C???I??
        EditorGUILayout.BeginHorizontal();

        GUIStyle style = new GUIStyle(GUI.skin.button);
        for (int i = 0; i < colors.Length; i++)
        {
            
            style.normal.textColor = Color.black;

            // ?I??^?C???????????
            Color c = colors[i];
            if (i == currentPaintValue)
            {
                c *= 2f;
                c.a = 1f;
            }

            Color prev = GUI.backgroundColor;
            GUI.backgroundColor = c;

            if (GUILayout.Button(((StageInfo.Info)i).ToString(), style, GUILayout.Height(30)))
            {
                currentPaintValue = i;
            }

            GUI.backgroundColor = prev;
        }

        EditorGUILayout.EndHorizontal();

        DrawGrid();


    }

    private void DrawGrid()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int y = 0; y < stageInfo.lines; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < stageInfo.columns; x++)
            {
                DrawCell(y, x);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawCell(int y, int x)
    {
        if (stageInfo.map == null) return;
        if (y >= stageInfo.map.Length) return;
        if (stageInfo.map[y] == null) return;
        if (x >= stageInfo.map[y].Length) return;

        int value = stageInfo.map[y][x];
        Color cellColor = colors[value % colors.Length];

        Rect rect = GUILayoutUtility.GetRect(cellSize, cellSize, GUILayout.ExpandWidth(false));
        EditorGUI.DrawRect(rect, cellColor);

        Color border = Color.black;
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), border); // ???
        EditorGUI.DrawRect(new Rect(rect.x, rect.yMax, rect.width, 1), border); // ????
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), border); // ????
        EditorGUI.DrawRect(new Rect(rect.xMax - 1, rect.y, 1, rect.height), border); // ?E??


        // ?h???b?O?h??
        if (rect.Contains(Event.current.mousePosition))
        {
            // ???N???b?N or ?h???b?O??
            if (isMouseDown)
            {
                stageInfo.map[y][x] = currentPaintValue;

                EditorUtility.SetDirty(stageInfo);
                GUI.changed = true;
            }
        }
    }
    private static void ImportCSV(TextAsset csv, StageInfo info)
    {
        string[] lines = csv.text.Replace("\r", "").Split('\n').
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

        EditorUtility.SetDirty(info);
        AssetDatabase.SaveAssets();

        Debug.Log("CSV ??????????I");
    }

    // StageInfo ?? CSV ???
    public static void Export(StageInfo stageInfo)
    {
        string path = EditorUtility.SaveFilePanel(
            "Save CSV",
            "",
            stageInfo.name + ".csv",
            "csv"
        );

        if (string.IsNullOrEmpty(path))
            return;

        SaveCSV(stageInfo, path);
    }

    private static void SaveCSV(StageInfo info, string path)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < info.lines; y++)
        {
            for (int x = 0; x < info.columns; x++)
            {
                sb.Append(info.map[y][x]);
                if (x < info.columns - 1)
                    sb.Append(",");
            }
            sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        Debug.Log($"CSV ???????: {path}");

        AssetDatabase.Refresh();
    }

    private void ResizeMap(StageInfo info)
    {
        int[][] newMap = new int[info.lines][];

        for (int y = 0; y < info.lines; y++)
        {
            newMap[y] = new int[info.columns];

            for (int x = 0; x < info.columns; x++)
            {
                if (info.map != null && y < info.map.Length && info.map[y] != null && x < info.map[y].Length)
                {
                    // ??????l???R?s?[
                    newMap[y][x] = info.map[y][x];
                }
                else
                {
                    // ?V?????????? GROUND ??????
                    newMap[y][x] = (int)Info.GROUND;
                }
            }
        }

        info.map = newMap;
    }
}