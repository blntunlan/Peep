using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesign", menuName = "Scriptable Objects/Wheat Design")]
public class WheatDesignSO : ScriptableObject
{
    [SerializeField] private float _increacesDecreaseMultiplier;
    [SerializeField] private float _resetBoostDuration;

    public float IncreaseDescraseMultiplier => _increacesDecreaseMultiplier;
    public float ResetBoostDuration => _resetBoostDuration;
}
