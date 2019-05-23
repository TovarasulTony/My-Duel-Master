using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_CIVILIZATION
{
    INVALID,
    NATURE,
    DARKNESS,
    FIRE,
    LIGHT,
    WATER
};

public delegate void SendCardTo();

public class Card : MonoBehaviour
{
    private List<TRAITS> mTraits;
    private CARD_CIVILIZATION mCardCivilization;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    public Vector3 mOrigPosition;
    public Quaternion mOrigRotation;
    private float mUntappedEulerAngleY;
    private float mTappedEulerAngleY;

    private BattlezoneManager mBattlezoneManager;
    private CardState mCardState;

    private int mPower;
    private int mManaRequired;
    private int mID;
    private bool mHasEnteredBattlezone = false;
    private bool mHasEnteredManazone = false;
    private bool mIsInAir = false;

    private AbilitiesData mAbilityData = null;
   

    private LineRenderer mLineRenderer;

    public void TestSetOwner()
    {
        mPlayerOwner = PLAYER_ID.TWO;
    }
    
    public PLAYER_ID GetPlayerOwner()
    {
        return mPlayerOwner;
    }

    void Awake ()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mLineRenderer.enabled = false;

        mID = GameManager.instance.GetID();
    }	

    public void NewTurn()
    {
        mCardState.NewTurn();
    }

    public bool IsTapped()
    {
        return mCardState.IsTapped();
    }

    public void LockTap()
    {
        mCardState.LockTap();
    }
	
	void Update ()
    {
        HandleAir();
        TargetingLine();

        if (GameManager.instance.GetIsTargeting() == true || mIsInAir == true)
        {
            GameManager.instance.SetCanHover(false);
        }
        else
        {
            GameManager.instance.SetCanHover(true);
        }

    }

    void HandleAir()
    {
        if(mIsInAir == true)
        {
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);
            transform.position = new Vector3(newPosition.x, newPosition.y > .1f ? newPosition.y : .1f, newPosition.z);
        }
    }

    void OnMouseDown()
    {
        mCardState.OnClick();
    }

    void OnMouseUp()
    {
        if (mIsInAir == false)
        {
            return;
        }

        GameManager.instance.CardOnAir(false);

        if (mHasEnteredBattlezone == true && GameManager.instance.CanSummon(GetComponent<Card>()) == true)
        {
            mBattlezoneManager.AddCardToManager(this);
            return;
        }

        if (mHasEnteredManazone == true && GameManager.instance.CanPlayMana(GetComponent<Card>()) == true)
        {
            SetCardState(new ManaState(GetComponent<Card>()));
            GameManager.instance.GetActiveManazone().AddCardToManager(this);
            return;
        }

        mCardState.OnMouseUp();
    }

    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.GetComponent<BattlezoneManager>() != null)
        {
           
                mHasEnteredBattlezone = true;
                mBattlezoneManager = _collider.gameObject.GetComponent<BattlezoneManager>();
            
        }
        if (_collider.gameObject.GetComponent<ManazoneManager>() != null)
        {
            mHasEnteredManazone = true;
        }
    }

    public void Defeated()
    {
        if (mAbilityData.mAbilityMoment == ABILITY_MOMENT.DEATHRATTLE)
        {
            mAbilityData.DoAbility(GetComponent<Card>());
        }
        else
        {
            mCardState.ToGraveyard();
        }
    }
	
	public void DestroyCard()
	{
		Destroy(gameObject);
	}

    public void ToHand()
    {
        mCardState.ToHand();
    }

    void OnTriggerExit(Collider _collider)
    {
        if (_collider.gameObject.GetComponent<BattlezoneManager>() != null)
        {
            mHasEnteredBattlezone = false;
        }
        if (_collider.gameObject.GetComponent<ManazoneManager>() != null)
        {
            mHasEnteredManazone = false;
        }
    }

    private void TargetingLine()
    {
        if(mLineRenderer.enabled == true)
        {
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);

            mLineRenderer.SetPosition(0, transform.position);
            mLineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y > .1f ? newPosition.y : .1f, newPosition.z));
        }
    }

    private void ChangeCardState(CARD_STATE _cardState)
    {

    }

    void OnMouseEnter()
    {
        if(mCardState.GetState()== CARD_STATE.MANAZONE)
        {
            return;
        }

        GameManager.instance.HoverEnter(transform);
    }

    void OnMouseExit()
    {
        GameManager.instance.HoverExit();
    }

    public int GetID()
    {
        return mID;
    }

    public CARD_CIVILIZATION GetCardCivilization()
    {
        return mCardCivilization;
    }

    public void SetCardCivilization(CARD_CIVILIZATION _civilization)
    {
        mCardCivilization = _civilization;
    }

    public int GetManaRequired()
    {
        return mManaRequired;
    }

    public void SetManaRequired(int _manaRequired)
    {
        mManaRequired = _manaRequired;
    }

    public void SetOrgigPosition(Vector3 _origPosition)
    {
        mOrigPosition = _origPosition;
    }

    public void SetOrigRotation(Quaternion _origRotation)
    {
        mOrigRotation = _origRotation;
    }

    public void SetUntappedEulerAngleY(float _angle)
    {
        mUntappedEulerAngleY = _angle;
        mTappedEulerAngleY = mUntappedEulerAngleY + 90;
    }

    public float GetUntappedEulerAngleY()
    {
        return mUntappedEulerAngleY;
    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }

    void AtTheEndOfTheTurn()
    {

    }

    void AtTheBeginningOfTheTurn()
    {

    }

    public void TurnLineRenderer(bool _isOn)
    {
        mLineRenderer.enabled = _isOn;

    }

    public void SetCardState(CardState _state)
    {
        if (mCardState != null)
        {
            mCardState.LeaveState();
        }
        mCardState = _state;
    }

    public void SetIsInAir(bool _inAir)
    {
        mIsInAir = _inAir;
    }

    public void SetPower(int _power)
    {
        mPower = _power;
    }

    public int GetPower()
    {
        return mPower;
    }

    public bool HasTraits(TRAITS _trait)
    {
        for(int i = 0; i < mTraits.Count; ++i)
        {
            if(mTraits[i] == _trait)
            {
                return true;
            }
        }

        return false;
    }

    public void SetTraits(List<TRAITS> _list)
    {
        mTraits = _list;
    }

    public void SetAbilityData(AbilitiesData _data)
    {
        mAbilityData = _data;
    }

    public AbilitiesData GetAbilityData()
    {
        return mAbilityData;
    }
}
