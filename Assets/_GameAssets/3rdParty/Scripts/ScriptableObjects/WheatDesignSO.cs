using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesign", menuName = "Scriptable Objects/Wheat Design")]
public class WheatDesignSO : ScriptableObject
{
    [SerializeField] private float _increacesDecreaseMultiplier;
    [SerializeField] private float _resetBoostDuration;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _passiveSprite;
    [SerializeField] private Sprite _activeWheatSprite;
    [SerializeField] private Sprite _passiveWheatSprite;
    

    public float IncreaseDescraseMultiplier => _increacesDecreaseMultiplier;
    public float ResetBoostDuration => _resetBoostDuration;
    public Sprite ActiveSprite => _activeSprite;
    public Sprite PassiveSprite => _passiveSprite;
    public Sprite ActiveWheatSprite => _activeWheatSprite;
    public Sprite PassiveWheatSprite => _passiveWheatSprite;
}
