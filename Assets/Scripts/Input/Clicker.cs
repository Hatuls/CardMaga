using System;
using System.Collections;
using System.Collections.Generic;
using CardMaga.Input;
using UnityEngine;

public class Clicker : TouchableItem<Clicker>
{
    private void Start()
    {
        ChangeState(true);
    }
}
