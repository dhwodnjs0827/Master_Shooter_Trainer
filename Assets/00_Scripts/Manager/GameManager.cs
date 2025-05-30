#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;
    public bool isOption = false;

    /// <summary>
    /// PopupOption 여닫기 
    /// TimeScale기본값 false
    /// </summary>
    /// <param name="puaseTime"></param>
    public void TogglePopup(bool puaseTime = false)
    {
        var controller = StageManager.Instance.Player?.Controller;

        if (isOption)
        {
            isOption = false;
            UIManager.Instance.ClosePopUpUI();

            StageManager.Instance.IsGamePause = false;

            if (controller != null)
            {
                controller.EnablePlayerInput(true); // 플레이어 마우스 회전 허용
                UIManager.ToggleMouseCursor(false);
            }
        }
        else
        {
            isOption = true;
            UIManager.Instance.OpenPopupUI<PopupOption>();

            StageManager.Instance.IsGamePause = puaseTime; // true면 정지, false면 정지 X

            if (controller != null)
            {
                controller.EnablePlayerInput(false); // 플레이어 마우스 회전 금지
                UIManager.ToggleMouseCursor(true);
            }
        }
    }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
