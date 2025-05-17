using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInput : MonoBehaviour
{
    private PlayerInputs playerInput;

    private void Awake()
    {
      playerInput = new PlayerInputs();  
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.UI.Puase.performed += OnPause;
    }

    private void OnDisable()
    {
        playerInput.UI.Puase.performed -= OnPause;
        playerInput.Disable();
    }
     private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //GameStateManager.ToggleGameState();
    }
}
