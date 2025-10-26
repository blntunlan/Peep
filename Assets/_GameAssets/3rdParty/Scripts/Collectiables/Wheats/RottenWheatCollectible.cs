using UnityEngine;
using UnityEngine.Serialization;

public class RottenWheatCollectible : MonoBehaviour, ICollectiable
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] private float _movementDecreaseSpeed;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_movementDecreaseSpeed, _resetBoostDuration);
        Destroy(this.gameObject);
    }
}
