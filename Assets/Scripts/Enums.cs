public enum TARGETS
{
    INVALID = 0,
    ALL,
    ENEMY,
    PLAYER,
    CHOICE,
    SELF
}

public enum CARD_CIVILIZATION
{
    INVALID,
    NATURE,
    DARKNESS,
    FIRE,
    LIGHT,
    WATER
};

public enum CARD_STATE
{
    INVALID,
    DECK,
    HAND,
    BATTLEZONE,
    AIR,
    MANAZONE,
    GRAVEYARD,
    TARGETING,
    NO_TARGETING
};

public enum TRAITS
{
    INVALID = 0,
    BLOCKER = 1
}

public enum GAME_PHASE
{
    INVALID = 0,
    MANA_PHASE = 1,
    SUMMONING_PHASE = 2,
    ATTACKING_PHASE = 3

};

public enum ABILITY_MOMENT
{
    INVALID = 0,
    BATTLECRY,
    END_OF_TURN,
    BEGINNING_OF_TURN,
    DEATHRATTLE
};

public enum PLAYER_ID
{
    INVALID = 0,
    ONE = 1,
    TWO = 2
};

public enum PLAYER_ASSOCIATION
{
    INVALID = 0,
    ALLY = 1,
    ENEMY = 2
};