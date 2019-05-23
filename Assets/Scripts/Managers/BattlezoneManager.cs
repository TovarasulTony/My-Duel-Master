using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : GameZoneManager
{
  
    public List<Transform> mInitCardList;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    private List<Card> mBlockerList;

    void Awake()
    {
        mBlockerList = new List<Card>();
    }

    protected override void NotifyCardWasAdded(Card _card)
    {
        BattleState battleState = new BattleState(_card);
        battleState.SetBattlezoneManager(this);

        _card.SetCardState(battleState);

        if (_card.HasTraits(TRAITS.BLOCKER) == true)
        {
            mBlockerList.Add(_card);
        }

        battleState.WhenSummoned();
    }

    public void RemoveCard(Card _card)
    {
        mCardList.Remove(_card);
    }

    public void NewTurn()
    {
        for (int i = 0; i < mCardList.Count; ++i)
        {
            mCardList[i].NewTurn();
        }
    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }
}
