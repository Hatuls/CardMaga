using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.Card;
using System;

public class RewardPack : MonoBehaviour
{
    private int _diamonds;
    private int _experience;
    private CardData[] _cards;

    public void Init(int diamonds, int experience, CardData[] cards)
    {
        throw new NotImplementedException();
    }

    public int Diamonds
    {
        get { return _diamonds; }
    }

    public int Experience
    {
        get { return _experience; }
    }

    public CardData[] Cards
    {
        get { return _cards; }
    }
}
