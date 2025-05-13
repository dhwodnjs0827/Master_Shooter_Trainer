using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private Player player;
    [field: SerializeField] public Transform ArmTransform { get; private set; }
    [field: SerializeField] public GameObject HandPos { get; private set; }

    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // 0.4초마다 흔든다 (스텝 간격)
    private bool isHeadbobUp = true;
    private float stepForce = 0f;


    private Quaternion initialLocalRotation;
    private void Awake()
    {
        player = GetComponent<Player>();

    }
    private void Start()
    {
        initialLocalRotation = HandPos.transform.localRotation;
    }


    /// <summary>
    /// 조준 시 캐릭터 HDL 수치에 따른 조준 흔들림 기능
    /// </summary>
    public void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - player.Stat.HDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        HandPos.transform.localRotation = initialLocalRotation * shakeRotation;
    }


    public void HeadbobUpdate()
    {
        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            isHeadbobUp = !isHeadbobUp; // 업다운 반전

            float t = Mathf.InverseLerp(1f, 99f, player.Stat.STP);
            stepForce = Mathf.Lerp(1.0f, 0.1f, t) * 0.02f;

            if (isHeadbobUp)
            {
                player.impulseSource.GenerateImpulse(Vector3.up * stepForce); // 위로
            }
            else
            {
                player.impulseSource.GenerateImpulse(Vector3.down * stepForce); // 아래로 (조금 덜)
            }
        }
    }
    public void HeadbobUp()
    {
        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f; // 타이머 초기화

            float t = Mathf.InverseLerp(1f, 99f, player.Stat.STP);
            float force = Mathf.Lerp(1.0f, 0.1f, t);

            player.impulseSource.GenerateImpulse(force * 0.02f); // 한번 흔들어줌
        }
    }

    public void ApplyRecoil()
    {
        float rcl = player.Stat.RCL; // 1~99 플레이어
        float t = Mathf.InverseLerp(1f, 99f, rcl);
        float controlFactor = Mathf.Lerp(1.0f, 0.2f, t); // RCL 높을수록 감소

        float weaponRecoil = player.Weapon.Stat.Recoil;
        float recoil = weaponRecoil * controlFactor;

        StopCoroutine("SmoothRecoil"); // 이전 코루틴 중복 방지
        StartCoroutine(SmoothRecoil(recoil));
    }

    private IEnumerator SmoothRecoil(float amount)
    {
        float duration = 0.1f; // 반동이 적용되는 시간
        float timer = 0f;

        float startOffset = player.stateMachine.RecoilOffsetX;
        float targetOffset = startOffset - amount;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            player.stateMachine.RecoilOffsetX = Mathf.Lerp(startOffset, targetOffset, t);
            yield return null;
        }

        player.stateMachine.RecoilOffsetX = targetOffset; // 정확한 마무리
    }
    public void HeadbobDown()
    {
        stepTimer = Mathf.MoveTowards(stepTimer, stepInterval, Time.deltaTime * 2f);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
