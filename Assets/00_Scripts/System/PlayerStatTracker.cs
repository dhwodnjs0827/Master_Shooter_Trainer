using UnityEngine;

/// <summary>
/// 플레이어의 통계를 실시간으로 추적하는 클래스
/// 도전과제 조건에 대응되는 값을 기록한다
/// </summary>
public class PlayerStatTracker : MonoBehaviour
{
    #region Stat Fields

    public int killCount;                 // 누적 킬 수
    public int headshotCount;            // 누적 헤드샷 수
    public int totalShotCount;           // 전체 발사 수

    public int longRangeHitCount;        // 특정 거리 이상 명중 수
    public float longestShotDistance;    // 가장 긴 거리 명중 기록

    public int consecutiveHitCount;      // 연속 명중 수
    public int noMissCount;              // 미스 없이 연속 히트 수
    public float clearTimeSeconds;       // 현재 스테이지 클리어 시간

    public int comboCount;               // 연속 콤보 수
    public int stageClearCount;          // 스테이지 클리어 횟수

    public int weaponSpecificKillCount;  // 특정 무기로 킬한 횟수
    public string lastUsedWeaponName;    // 마지막 사용 무기 이름

    #endregion

    #region Derived Stats

    /// <summary>
    /// 헤드샷 명중률 (%) 계산
    /// </summary>
    public float HeadshotRatio
    {
        get
        {
            if (killCount == 0) return 0f;
            return (float)headshotCount / killCount * 100f;
        }
    }

    #endregion

    #region Tracking Methods

    public void RegisterKill(bool isHeadshot, float distance, string weaponName)
    {
        killCount++;

        if (isHeadshot)
        {
            headshotCount++;
        }

        if (distance >= 500f)
        {
            longRangeHitCount++;
        }

        if (distance > longestShotDistance)
        {
            longestShotDistance = distance;
        }

        if (!string.IsNullOrEmpty(weaponName))
        {
            lastUsedWeaponName = weaponName;
            weaponSpecificKillCount++;
        }

        AchievementManager.Instance.CheckAllAchievements();
    }

    public void RegisterCombo(int combo)
    {
        comboCount = Mathf.Max(comboCount, combo);

        AchievementManager.Instance.CheckAllAchievements();
    }

    public void RegisterClear(float time)
    {
        stageClearCount++;
        clearTimeSeconds = time;

        AchievementManager.Instance.CheckAllAchievements();
    }

    public void RegisterMiss()
    {
        noMissCount = 0;
        consecutiveHitCount = 0;

        AchievementManager.Instance.CheckAllAchievements();
    }

    public void RegisterHit()
    {
        consecutiveHitCount++;
        noMissCount++;

        AchievementManager.Instance.CheckAllAchievements();
    }

    #endregion
}
