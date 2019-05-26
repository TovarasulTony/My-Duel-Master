using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class AbilitiesData
{
    [SerializeField]
    public AbilitiesCallback mAbilityCallback = null;
    [SerializeField]
    private ConditionData Condition_Data = null;
    [SerializeField]
    public ABILITY_MOMENT mAbilityMoment = ABILITY_MOMENT.INVALID;
    [SerializeField]
    public int mMaxNumber = -1;
    public Card mCaster;


    public void DoAbility(Card _card)
    {
        mCaster = _card;
        //Condition_Data.GetConditionCallback().Invoke(_card, Condition_Data);
        mAbilityCallback.Invoke(this);
    }

    public ConditionData GetConditionData()
    {
        return Condition_Data;
    }

    public ConditionCallback GetConditionCallback()
    {
        return Condition_Data.GetConditionCallback();
    }
}
