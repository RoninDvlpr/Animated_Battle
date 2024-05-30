using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;


public class FightController : MonoBehaviour
{
    [SerializeField] Fighter playerFighter, opponentFighter;
    [InspectorLabel("Random fight")] 
    [SerializeField] bool playRandomFight, takeTurnsDuringRandomFight;

    Fighter randomFightAttacker;


    void Start()
    {
        CloseIn();
        //AttacksDemo();
        if (playRandomFight)
            StartRandomFight();
    }

    void CloseIn()
    {
        playerFighter.CloseInOnOpponent(opponentFighter);
        opponentFighter.CloseInOnOpponent(playerFighter);
    }

    #region Demonstration

    void AttacksDemo()
    {
        //response to an attack should be set before calling the attack method -
        //otherwise defender will react to the attack start as if he has no defense task
        //opponentFighter.BlockNextAttack();
        //opponentFighter.DodgeNextAttack();

        AttackContext randomAttackContext = GenerateRandomAttackContext(opponentFighter, AttacksDemo);
        playerFighter.AttackOpponent(randomAttackContext);
    }

    #endregion

    void StartRandomFight()
    {
        if (takeTurnsDuringRandomFight)
            randomFightAttacker = playerFighter;
        ContinueRandomFight();
    }

    void ContinueRandomFight()
    {
        if (takeTurnsDuringRandomFight)
            randomFightAttacker = GetAnotherFighter(randomFightAttacker);
        else
            randomFightAttacker = GetRandomFighter();
        Fighter defender = GetAnotherFighter(randomFightAttacker);

        StartCoroutine(PlayRandomFightRound(randomFightAttacker, defender, ContinueRandomFight));
    }

    IEnumerator PlayRandomFightRound(Fighter attacker, Fighter defender, Action onRoundFinished)
    {
        int randomDefenseIndex = Random.Range(0, 3);
        switch (randomDefenseIndex)
        {
            case 1:
                defender.BlockNextAttack();
                break;
            case 2:
                defender.DodgeNextAttack();
                break;
        }

        while (attacker.IsBusy || defender.IsBusy)
            yield return null;

        AttackContext randomAttackContext = GenerateRandomAttackContext(defender, onRoundFinished);
        attacker.AttackOpponent(randomAttackContext);
    }

    Fighter GetAnotherFighter(Fighter fighter)
    {
        if (fighter == opponentFighter)
            return playerFighter;
        else if (fighter == playerFighter)
            return opponentFighter;
        else
        {
            Debug.LogError("Such fighter doesn't exist " + fighter);
            return playerFighter;
        }
    }

    Fighter GetRandomFighter()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
            return playerFighter;
        else
            return opponentFighter;
    }

    AttackContext GenerateRandomAttackContext(Fighter attackTarget, Action onAttackFinishedCallback)
    {
        AttackTypes randomAttackType = (AttackTypes) Random.Range(0, 2);

        return new AttackContext(
            attackTarget,
            randomAttackType,
            onAttackFinishedCallback);
    }
}
