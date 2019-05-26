using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardState
{
    protected Card mCardReference;
    protected GameZoneManager mGameZoneManager;
    protected CARD_STATE mCardState = CARD_STATE.INVALID;
    private BattlezoneManager mBattlezoneManager = null;

    private CardState() { }

    public CardState(Card _card, GameZoneManager _gameZoneManager)
    {
        mCardReference = _card; //it works?
        mCardState = CARD_STATE.INVALID;
        mCardReference.SetIsInAir(false);
        mGameZoneManager = _gameZoneManager;

        if (mGameZoneManager != null)
        {
            mGameZoneManager.AddCardToManager(mCardReference);
        }
    }

    protected virtual void InitCardState()
    {

    }

    public virtual void OnClick()
    {
        Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual void OnMouseUp()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual void NewTurn()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual bool IsTapped()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
        return false;
    }

    public virtual void LockTap()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public CARD_STATE GetState()
    {
        return mCardState;
    }
   

    public virtual void LeaveState()
    {
        //Debug.LogWarning("Not implemented LeaveState()!!!");
        mGameZoneManager.RemoveCardFromManager(mCardReference);
    }
}