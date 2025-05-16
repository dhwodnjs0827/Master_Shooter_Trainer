using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Achievement", menuName = "SO/Achievement")]
public class AchievementSO : ScriptableObject
{
    [Header("기본 정보")]

    /// <summary> 도전과제 ID (고유값) </summary>
    public string id;

    /// <summary> 도전과제 이름 </summary>
    public string title;

    /// <summary> 도전과제 설명 </summary>
    public string description;

    [Header("조건 목록")]

    /// <summary> 클리어 조건 목록 (모두 충족 시 달성) </summary>
    public List<AchievementCondition> conditions = new();
}

[CreateAssetMenu(menuName = "SO/AchievementUIPrefabSet")]
public class AchievementUIPrefabSet : ScriptableObject
{
    public Canvas mainCanvasPrefab;
    public Canvas popupCanvasPrefab;
    public AchievementMainUI achievementMainUIPrefab;
    public UIAchievementPopup popupPrefab;
}

public enum AchievementType
{
    StageClearCount,   // 스테이지 클리어 횟수
    ComboMax,          // 최대 콤보 수
    HeadshotRatio,     // 헤드샷 비율
    NoMissRun,         // 무피격 또는 미스 없는 클리어
    HiddenAction,      // 숨겨진 행동 감지
    FastTargetKill     // 일정 시간 내 표적 처치
}

public class AchievementListSO : ScriptableObject
{
    public List<AchievementSO> achievements;
}
