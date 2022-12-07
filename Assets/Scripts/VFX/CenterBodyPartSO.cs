﻿using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Center Body Part Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Center Body Part")]
    public class CenterBodyPartSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart( BodyPartEnum.Chest);
            SetPoisition(vfxObject, activeBodyPart);
        }
    }
}