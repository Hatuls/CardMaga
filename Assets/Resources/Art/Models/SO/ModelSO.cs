using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Model SO", menuName = "ScriptableObjects/Art/Models/New Model SO")]
public class ModelSO : ScriptableObject
{
    public GameObject Model;
    public MaterialsForModel[] Materials;
}

[Serializable]
public class MaterialsForModel
{
    public Material Normal;
    public Material Tinted;
} 