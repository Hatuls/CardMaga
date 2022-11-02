using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Group", menuName = "ScriptableObjects/Input/Input Identification/New Input Group")]
public class InputGroup : ScriptableObject
{
   [SerializeField] private InputIdentificationSO[] _inputIDs;

   public InputIdentificationSO[] InputIDs
   {
      get => _inputIDs;
   }
}
