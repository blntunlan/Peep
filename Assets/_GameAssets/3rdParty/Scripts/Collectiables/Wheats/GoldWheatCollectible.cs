using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectiable
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] WheatDesignSO _wheatDesign;


    public void Collect()
    {
        _playerController.SetMovementSpeed(_wheatDesign.IncreaseDescraseMultiplier, _wheatDesign.ResetBoostDuration);
        Destroy(this.gameObject);
    }
}
