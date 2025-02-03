using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoState<InventoryController> 
{
    Dictionary<CollectableTypes, float> _inventory;

    HealthController _healthController;

    protected override void Awake()
    {
        base.Awake();

        _healthController = GetComponent<HealthController>();
        _inventory = new Dictionary<CollectableTypes, float>();
    }

    public void Add(CollectableTypes collectableType, float value, bool isHealth)
    {
        if (isHealth)
        {
            _healthController.Heal(value);
        }
        else
        {
            if(_inventory.ContainsKey(collectableType))
            {
                _inventory[collectableType] = value;
            }
            else
            {
                _inventory.Add(collectableType, value);
            }
        }
    }
}
