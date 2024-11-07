using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Card", menuName = "Cards/SpecialCards/SelfVotting")]
public class SelfVottingSpecialCard : SpecialCard
{
    enum TargetType
    {
        OLDEST,
        YOUNGEST,
        LEFT,
        RIGHT
    }

    bool refreshNeeded = true;
    [SerializeField] TargetType targetPlayerType;
    PlayerData targetPlayer;

    protected override void OnEnable()
    {
        base.OnEnable();
        refreshNeeded = true;
        cardName = "Защити игрока";

    }


    public void CalculateTargetPlayer()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        int ownerIndex = -1;
        int targetIndex = -1;
       
        if (refreshNeeded)
        {
            if (targetPlayer != null) targetPlayer.CleanOnKickAction();

            switch (targetPlayerType)
            {
                case TargetType.OLDEST:
                    int oldestAge = 0;
                    foreach (PlayerData player in gameManager.CurrentPlayers)
                    {
                        if (player.ID == owner.ID) continue;
                        if (player.OpenCard[CardType.BIOLOGY])
                        {
                            BiologyCard cardTocheck = (BiologyCard)player.HandCard[CardType.BIOLOGY];
                            if (cardTocheck.Age > oldestAge)
                            {
                                targetPlayer = player;
                            }
                        }
                    }
                    break;

                case TargetType.YOUNGEST:
                    int youngestAge = 120;
                    foreach (PlayerData player in gameManager.CurrentPlayers)
                    {
                        if (player.ID == owner.ID) continue;
                        if (player.OpenCard[CardType.BIOLOGY])
                        {
                            BiologyCard cardTocheck = (BiologyCard)player.HandCard[CardType.BIOLOGY];
                            if (cardTocheck.Age < youngestAge)
                            {
                                targetPlayer = player;
                            }
                        }
                    }
                    break;

                case TargetType.LEFT:
                    ownerIndex = gameManager.CurrentPlayers.FindIndex(x => x.ID == owner.ID);
                    targetIndex = ownerIndex - 1;
                    if (targetIndex < 0) targetIndex = gameManager.CurrentPlayers.Count - 1;
                    targetPlayer = gameManager.CurrentPlayers[targetIndex];
                    refreshNeeded = false;
                    break;

                case TargetType.RIGHT:
                    ownerIndex = gameManager.CurrentPlayers.FindIndex(x => x.ID == owner.ID);
                    targetIndex = (ownerIndex + 1) % gameManager.CurrentPlayers.Count;
                    targetPlayer = gameManager.CurrentPlayers[targetIndex];
                    refreshNeeded = false;
                    break;
            }

            if (targetPlayer != null) targetPlayer.AddOnKickAction(Use);
        }
            
    }


    public override void Use()
    {
        owner.SetOnePlayerForPossibleTargets(owner);
        base.Use();
    }


    public override bool CheckPossibilityToUse()
    {
        return false;
    }

}
