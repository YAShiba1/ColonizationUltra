using UnityEngine;

public class BaseSelection : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera _mainCamera;

    public LayerMask LayerMask => _layerMask;

    public Base ClickedBase { get; private set; }

    public bool IsBaseSelected { get; private set; } = false;

    public Ray Ray { get; private set; }

    public void TrySelect()
    {
        Ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out Base goldBase))
                {
                    IsBaseSelected = true;

                    ClickedBase = goldBase;
                    return;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && IsBaseSelected == true)
        {
            IsBaseSelected = false;
        }
    }
}
