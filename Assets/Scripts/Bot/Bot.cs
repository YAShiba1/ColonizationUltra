using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField][Range(1, 30)] private int _speed;

    private Base _parentBase;
    private Transform _currentTarget;

    private bool _isTargetGold;

    public event UnityAction FlagReached;

    public Transform CurrentTarget => _currentTarget;

    private void Update()
    {
        if (_currentTarget != null)
        {
            MoveTo();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _parentBase.gameObject && _currentTarget == _parentBase.transform)
        {
            _parentBase.TakeGold();
            DestroyFirstChild();

            _currentTarget = null;
        }

        if (other.gameObject.TryGetComponent(out Flag flag))
        {
            if (_currentTarget == flag.transform)
            {
                FlagReached?.Invoke();
                Destroy(flag.gameObject);
            }
        }
    }

    public void SetParentBase(Base parentBase)
    {
        _parentBase = parentBase;
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;

        if(_currentTarget.TryGetComponent(out Gold gold))
        {
            _isTargetGold = true;
        }
        else
        {
            _isTargetGold = false;
        }
    }

    private void MoveTo()
    {
        Vector3 targetPosition = _currentTarget.position;
        targetPosition.y = transform.position.y;

        transform.LookAt(targetPosition);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        if(_isTargetGold == true)
        {
            CheckForNearbyGoldResources();
        }
    }

    private void CheckForNearbyGoldResources()
    {
        float distance = Vector3.Distance(transform.position, _currentTarget.position);
        float pickupRadius = 2f;

        if (distance < pickupRadius)
        {
            PickUpGoldResource();
            SetTarget(_parentBase.transform);
        }
    }

    private void PickUpGoldResource()
    {
        _currentTarget.SetParent(transform);
    }

    private void DestroyFirstChild()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        else
        {
            throw new System.Exception("No child objects to destroy.");
        }
    }
}
