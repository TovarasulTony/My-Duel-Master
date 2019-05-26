using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckState : CardState
{
    int dodo = 10;
    public DeckState(Card _card, GameZoneManager _gameZoneManager) : base(_card, _gameZoneManager)
    {
        mCardState = CARD_STATE.DECK;
    }

}
