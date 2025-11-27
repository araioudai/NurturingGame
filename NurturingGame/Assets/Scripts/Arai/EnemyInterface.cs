using System.Security.Cryptography;
using UnityEngine;

public interface IReceiveLevel //インターフェースの定義
{
    void RandomLevel(int lower, int upper);  //レベルを設定する処理
    void ApplyLevelData();                   //レベル反映用
}
