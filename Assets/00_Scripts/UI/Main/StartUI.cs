using UnityEngine;
using UnityEngine.UI;
using DataDeclaration;
using Unity.Services.Authentication;

public class StartUI : MainUI
{
    public override MainUIType UIType { get; protected set; }

    [SerializeField] private Button startBtn;

    private void Awake()
    {
        UIType = MainUIType.Start;
        startBtn.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        //SceneLoadManager.Instance.LoadScene(Scene.Lobby);
        UIManager.Instance.OpenPopupUI<PopupRankingBoard>();
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(activeUIType == UIType);
    }
}