using UnityEngine;

[CreateAssetMenu(fileName = "NewCropData", menuName = "Game/Crop Data")]
public class CropsData : ScriptableObject
{
    [Header("Infos")]
    public string cropName;
    [TextArea] public string description;

    [Header("Économie")]
    public int buyPrice;
    public int sellPrice;

    [Header("Croissance")]
    public float growTime;
    [Range(0f, 1f)]
    public float mutationChance;
}