using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject backButton;


    public void OffLobbyUI()
    {
        UIManager.Instance.ClosePopUpUI();
        SetLobbyUI(false);
        LobbyCameraManager.Instance.MoveCameraPosition(+1);
        backButton.SetActive(true);
    }

    public void OnLobbyUI()
    {

        UIManager.Instance.ClosePopUpUI();
        SetLobbyUI(true);

        GameManager.Instance.selectedCharacter = null;
        GameManager.Instance.selectedWeapon = null;

        LobbyCameraManager.Instance.ResetCamPosition();
        backButton.SetActive(false);
    }
    private void SetLobbyUI(bool isOn)
    {
        canvas.SetActive(isOn);
    }
}
