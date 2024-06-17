using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class FightController : MonoBehaviour
{
    [InspectorLabel("Dependencies")]
    [SerializeField] protected Fighter playerFighter, opponentFighter;
    [SerializeField] ResponseParser responseParser;

    [InspectorLabel("Fight settings")]
    [SerializeField] protected bool playRandomFight = true,
        playFightFromExampleJSON = false,
        takeTurnsDuringRandomFight = false;

    // Fight State
    List<FightTurn> turnsList = new List<FightTurn>();
    int currentTurnIndex;
    bool FightersAreFree
    {
        get
        {
            return !playerFighter.IsBusy && !opponentFighter.IsBusy;
        }
    }



    void Start()
    {
        StartFight();
    }

    protected void StartFight()
    {
        if (playFightFromExampleJSON)
            turnsList = responseParser.ExampleFightData.GetFightTurns();
        currentTurnIndex = 0;

        CloseIn();
        StartNextTurn();
    }

    void StartNextTurn()
    {
        if (currentTurnIndex >= turnsList.Count)
        {
            if (playRandomFight)
                AddRandomTurn();
            else
                return;
        }


        if (!FightersAreFree)
        {
            StartCoroutine(AwaitForFightersToFreeUp(StartNextTurn));
            return;
        }

        PlayTurn(turnsList[currentTurnIndex], StartNextTurn);
        currentTurnIndex++;
    }

    IEnumerator AwaitForFightersToFreeUp(Action onFightersFreedUp)
    {
        while (!FightersAreFree)
            yield return null;
        onFightersFreedUp?.Invoke();
    }

    void PlayTurn(FightTurn turn, Action onTurnFinished)
    {
        Fighter attacker, defender;
        if (turn.attacker == Fighters.Player)
        {
            attacker = playerFighter;
            defender = opponentFighter;
        }
        else
        {
            attacker = opponentFighter;
            defender = playerFighter;
        }

        defender.SetNextDefense(turn.defenseType);
        attacker.AttackOpponent(new AttackContext(defender, turn.attackType, onTurnFinished));
    }

    void AddRandomTurn()
    {
        Fighters attacker = GetAttackerForRandomTurn();
        turnsList.Add(FightTurn.GenerateRandomFightTurn(attacker));
    }

    Fighters GetAttackerForRandomTurn()
    {
        if (!takeTurnsDuringRandomFight || turnsList.Count == 0)
            return (Fighters) Random.Range(0, 2);

        Fighters lastAttacker = turnsList[turnsList.Count - 1].attacker; // the fighter who performed attack on previous turn
        Fighters nextAttacker = (Fighters) (((int)lastAttacker + 1) % 2); // the next attacker is another fighter from the player-opponent pair
        return nextAttacker;
    }

    protected void CloseIn()
    {
        playerFighter.CloseInOnOpponent(opponentFighter);
        opponentFighter.CloseInOnOpponent(playerFighter);
    }
}
