using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);

    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
