using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZoneManager : MonoBehaviour {
    public List<Card> mCardList;
    protected float mNextCardPoz = -3.5f;

   public void AddCardToManager(Card _card)
    {
        mCardList.Add(_card);
        mNextCardPoz += 1.5f;
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.21f, transform.position.z + 0.1f);
         
       _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);
        _card.transform.eulerAngles = new Vector3(0, _card.transform.eulerAngles.y == 0 ? 0 : 180, 0);
        NotifyCardWasAdded(_card);
    }

    protected virtual void NotifyCardWasAdded(Card _card)
    {

    }

    public virtual void RemoveCardFromManager(Card _card)
    {
        
    }

    public List<Card> GetConditionalList(ConditionData _data)
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < mCardList.Count; ++i)
        {
            _data.GetConditionCallback().Invoke(mCardList[i], _data);
            if (_data.Response == true)
            {
                list.Add(mCardList[i]);
            }
        }

        return list;
    }
}
