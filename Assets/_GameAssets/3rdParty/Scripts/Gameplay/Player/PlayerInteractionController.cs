using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private string _interactableObjectName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectiable>(out var collectible))
        {
            collectible.Collect();
        }
    }
}
