using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectiable
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] WheatDesignSO _wheatDesign;

    public void Collect()
    {
        _playerController.SetJumpForce(_wheatDesign.IncreaseDescraseMultiplier, _wheatDesign.ResetBoostDuration);
        Destroy(this.gameObject);
    }
}
