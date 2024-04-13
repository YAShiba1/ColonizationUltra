using UnityEngine;
using UnityEngine.EventSystems;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private SelectionController _selectionController;

    private Flag _flag;
    private bool _isFlagSpawned;

    private void Update()
    {
        TrySpawnFlag();
        _selectionController.TrySelectBase();
    }

    private void TrySpawnFlag()
    {
        if (_selectionController.IsBaseSelected == true && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_selectionController.Ray, out RaycastHit hit, Mathf.Infinity, _selectionController.LayerMask))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject != null && hitObject.TryGetComponent(out Ground ground) && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 flagSpawnPosition = new Vector3(hit.point.x, _flagPrefab.transform.position.y, hit.point.z);

                    if (_isFlagSpawned == true && _flag != null)
                    {
                        Destroy(_flag.gameObject);

                        _isFlagSpawned = false;
                    }

                    _flag = CreateFlag(flagSpawnPosition);

                    _selectionController.ClickedBase.SetFlag(_flag);

                    _isFlagSpawned = true;
                }
            }
        }
    }

    private Flag CreateFlag(Vector3 spawnPosition)
    {
        return Instantiate(_flagPrefab, spawnPosition, Quaternion.identity);
    }
}
