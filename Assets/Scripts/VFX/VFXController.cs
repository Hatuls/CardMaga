﻿

using CardMaga.Battle.Visual;
using CardMaga.Commands;
using CardMaga.Keywords;
using System;
using UnityEngine;
namespace CardMaga.VFX
{
    public class VFXController : MonoBehaviour
    {
        public event Action<IVisualPlayer> OnHittingVFX;
        public event Action<IVisualPlayer> OnDefenseVFX;
        public event Action<IVisualPlayer> OnGetHitVFX;
        public event Action<KeywordType, IVisualPlayer> OnVisualStatChanged;

        private IVisualPlayer _visualCharacter;
        public void Init(IVisualPlayer visualCharacter)
        {
            _visualCharacter = visualCharacter;
        }




        public void PlayHittingVFX()
        => OnHittingVFX?.Invoke(_visualCharacter);
        public void PlayGettingHitVFX()
        => OnGetHitVFX?.Invoke(_visualCharacter);
        public void PlayDefenseVFX()
        => OnDefenseVFX.Invoke(_visualCharacter);




    }

}



public enum BodyPartEnum
{
    None = 0,
    RightArm = 1,
    LeftArm = 2,
    Head = 3,
    LeftLeg = 4,
    RightLeg = 5,
    Pivot = 6,
    Chest = 7,
    LeftKnee = 8,
    RightKnee = 9,
    LeftElbow = 10,
    RightElbow = 11,
    Hips =12,
};

