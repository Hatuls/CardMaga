using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Group", menuName = "ScriptableObjects/Input/Input Identification/New Input Group")]
public class InputGroup : ScriptableObject
{
   [SerializeField] private List<InputIdentificationSO> _inputs;
}
