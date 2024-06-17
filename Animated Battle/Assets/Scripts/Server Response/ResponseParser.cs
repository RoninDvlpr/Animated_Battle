using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseParser : MonoBehaviour
{
    [SerializeField] TextAsset exampleServerResponse;
    FightData exampleFightData;
    public FightData ExampleFightData
    {
        get
        {
            if (exampleFightData == null)
                exampleFightData = ParseExampleFightData();
            return exampleFightData;
        }
    }


    void PrintExampleTurns()
    {
        List<FightTurn> turns = ExampleFightData.GetFightTurns();
        for (int i = 0; i < turns.Count; i++)
            turns[i].PrintToConsole();
    }

    void PrintExampleFightData()
    {
        ExampleFightData.PrintToConsole();
    }

    FightData ParseExampleFightData()
    {
        return FightData.CreateFromJSON(exampleServerResponse.text);
    }
}
