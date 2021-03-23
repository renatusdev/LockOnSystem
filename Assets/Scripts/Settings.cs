using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Settings : MonoBehaviour
{
    #region Changeable Variables

    public static CinemachineTransposer.BindingMode m_OrbitalBinding = CinemachineTransposer.BindingMode.WorldSpace;
    public static int m_LockOnZoomIn = 15;

    #endregion

    void Awake()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        Cursor.visible = false;


        DontDestroyOnLoad(this.gameObject);    
    }
}