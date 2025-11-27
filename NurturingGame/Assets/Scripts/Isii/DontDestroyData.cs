using UnityEngine;

public class DontDestroyData : MonoBehaviour
{
    [System.Obsolete]
    private void Awake()
    {
        // オブジェクトが既に存在する場合は破棄する
        if (FindObjectsOfType<DontDestroyData>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
