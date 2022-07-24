using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Model SO", menuName = "ScriptableObjects/Art/Models/New Model SO")]
public class ModelSO : ScriptableObject
{
    [Sirenix.OdinInspector.PreviewField(75f)]
    public AvatarHandler Model;
    public MaterialsForModel[] Materials;
}

[Serializable]
public class MaterialsForModel
{
    public Material Normal;
    public Material Tinted;
} 