using UnityEngine;

public class VisualCharacter : MonoBehaviour
{
    [SerializeField]
    private AnimatorController _animatorController;

    public AvatarHandler AvatarHandler { get; private set; }
    public AnimatorController AnimatorController => _animatorController;
    public void SpawnModel(ModelSO modelSO, bool isTinted)
    {
        AvatarHandler = Instantiate(modelSO.Model, AnimatorController.transform);
        if (isTinted)
            AvatarHandler.Mesh.material = modelSO.GetRandomTintedMaterials();
    }
}