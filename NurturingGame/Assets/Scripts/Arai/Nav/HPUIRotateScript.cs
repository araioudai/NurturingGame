using UnityEngine;

public class HPUIRotateScript : MonoBehaviour
{
    #region ƒJƒƒ‰‚Æ“¯‚¶Œü‚«‚ÉHPƒo[‚ğİ’è
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    #endregion
}
