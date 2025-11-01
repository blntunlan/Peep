using UnityEngine;

public class EggCollectible : MonoBehaviour, ICollectiable
{
    public void Collect()
    {
        GameManager.Instance.OnEggCollected();
        Destroy(this.gameObject);
    }
}
