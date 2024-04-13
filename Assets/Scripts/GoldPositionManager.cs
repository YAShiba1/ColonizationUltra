using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldPositionManager : MonoBehaviour
{
    private Dictionary<Transform, bool> _freeGoldsPoints;

    public int GetFiltredGoldsPositionsCount => _freeGoldsPoints.Count;

    private void Awake()
    {
        _freeGoldsPoints = new Dictionary<Transform, bool>();
    }

    public void TakeGoldPosition(Transform goldPosition)
    {
        if(_freeGoldsPoints.ContainsKey(goldPosition) == false)
        {
            bool isPositionIssued = false;

            _freeGoldsPoints.Add(goldPosition, isPositionIssued);
        }
    }

    public Transform GetFiltredGoldPosition()
    {
        foreach (var goldPosition in _freeGoldsPoints)
        {
            if (goldPosition.Value == false)
            {
                bool isPositionIssued = true;

                _freeGoldsPoints[goldPosition.Key] = isPositionIssued;

                return goldPosition.Key;
            }
        }

        return null;
    }
}
