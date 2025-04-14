using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs;
    private GameObject currentWeaponObject;
    private int currentWeaponIndex = -1;

    [Header("공통 참조")]
    public Transform handransform;
    public Transform camRoot;
    public Camera playerCam;
    public FpsCamera fpsCamera;
    public GameObject playerObject;

    void Start()
    {
        SwitchWeapon(0); // 시작 시 무기 장착
    }

    void Update()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
            }
        }
    }

    void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length || index == currentWeaponIndex)
            return;

        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        currentWeaponObject = Instantiate(weaponPrefabs[index], handransform, false);
        currentWeaponIndex = index;

        var handler = currentWeaponObject.GetComponent<WeaponStatHandler>();
        if (handler != null)
        {
            handler.SetSharedReferences(handransform, camRoot, playerCam, fpsCamera, playerObject);

            // 여기에서 InitReferences 호출!
            var fireController = currentWeaponObject.GetComponent<WeaponFireController>();
            if (fireController != null)
            {
                fireController.InitReferences();
            }
        }
    }

}
