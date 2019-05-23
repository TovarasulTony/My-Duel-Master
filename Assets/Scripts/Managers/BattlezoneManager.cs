using System;
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

    public override void RemoveCardFromManager(Card _card)
    {
        mNextCardPoz -= 1.5f;
        RepositionCards(_card);
        mCardList.Remove(_card);
        
      
    }
    private void RepositionCards(Card _card)
    {
        //Add Security Check
        int startIndex  = mCardList.IndexOf(_card);
        for (int i = startIndex + 1; i< mCardList.Count; i++)
        {
            mCardList[i].transform.position = new Vector3(mCardList[i].transform.position.x - 1.5f, mCardList[i].transform.position.y, mCardList[i].transform.position.z);
        }
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
