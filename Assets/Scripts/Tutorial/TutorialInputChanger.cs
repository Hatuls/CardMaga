using CardMaga.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInputChanger : MonoBehaviour
{
    public void ChangeInputGroup(InputGroup inputGroup)
    {
        LockAndUnlockSystem.Instance.SetNewInputGroup(inputGroup);
    }
}
