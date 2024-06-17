using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;


public class TestFightController : FightController
{
    void Start()
    {
        //Time.timeScale = 0.25f;

        //playerFighter.MoveForward(100000f, null); //walk test
        //AttacksDemo();
        StartFight();
    }

    #region Demonstration

    void AttacksDemo()
    {
        CloseIn();
        //response to an attack should be set before calling the attack method -
        //otherwise defender will react to the attack start as if he has no defense task
        //opponentFighter.BlockNextAttack();
        //opponentFighter.DodgeNextAttack();

        AttackContext randomAttackContext = GenerateRandomAttackContext(opponentFighter, AttacksDemo);
        playerFighter.AttackOpponent(randomAttackContext);
    }

    #endregion


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
