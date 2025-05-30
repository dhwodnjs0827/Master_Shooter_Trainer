using System.Collections;
using UnityEngine;

public class StageScene : BaseScene
{
    public override void EnterScene()
    {
        CoroutineRunner.Run(EnterSceneRoutine());
    }

    private IEnumerator EnterSceneRoutine()
    {
        UIManager.ToggleMouseCursor(false);
        yield return null;

        RoomManager.Instance.InitRoom();
        yield return null;

        StageManager.Instance.SpawnPlayer();
        yield return null;

        UIManager.Instance.InitMainUI<HUDUI>();
        yield return null;

        var bgm = ResourceManager.Instance.Load<AudioClip>("Audio/BGM/Can't Stop Me_demo");
        yield return null;

        SoundManager.Instance.PlayBackgroundMusic(bgm);
        yield return null;
    }

    public override void ExitScene()
    {
        base.ExitScene();
        UIManager.ToggleMouseCursor(true);
    }
}