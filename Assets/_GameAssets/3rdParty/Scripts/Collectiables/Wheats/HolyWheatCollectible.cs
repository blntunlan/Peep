using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] private float _movementIncreaseSpeed;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetJumpForce(_movementIncreaseSpeed,_resetBoostDuration);
        Destroy(this.gameObject);
    }
}
