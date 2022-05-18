﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
   StateIdentificationSO StateID { get; }
   
   BaseCondition[] Conditions { get; }

   void OnEnterState();
   
   void OnExitState();

   StateIdentificationSO OnHoldState();

   StateIdentificationSO CheckStateCondition();
   
   
}