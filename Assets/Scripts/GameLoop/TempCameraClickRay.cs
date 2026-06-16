using UnityEngine;
using UnityEngine.InputSystem;

public class TempCameraClickRay : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask targetLayerMask = ~0;
    [SerializeField] private float rayDistance = 100f;

    private InputAction attackAction;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        attackAction = InputSystem.actions.FindAction("Attack");

        if (attackAction == null)
            Debug.LogError("InputSystem.actions에서 Attack 액션을 찾을 수 없습니다.");
    }

    private void OnEnable()
    {
        if (attackAction == null)
            return;

        attackAction.Enable();
        attackAction.performed += HandleAttack;
    }

    private void OnDisable()
    {
        if (attackAction == null)
            return;

        attackAction.performed -= HandleAttack;
        attackAction.Disable();
    }

    private void HandleAttack(InputAction.CallbackContext context)
    {
        if (targetCamera == null)
            return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = targetCamera.ScreenPointToRay(screenPos);

        RaycastHit2D hit = Physics2D.GetRayIntersection(
            ray,
            rayDistance,
            targetLayerMask
        );

        if (hit.collider == null)
            return;

        StoneActor stone = hit.collider.GetComponentInParent<StoneActor>();

        if (stone == null)
            return;

        // StoneActor 쪽 실제 메소드명에 맞게 바꾸면 됨.
        stone.Mine(2);
    }
}