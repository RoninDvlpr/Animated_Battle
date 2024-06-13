using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseParser : MonoBehaviour
{
    [SerializeField] TextAsset exampleServerResponse;

    void Start()
    {
        PrintExampleFightData();
    }

    void PrintExampleFightData()
    {
        GetExampleFightData().PrintToConsole();
    }

    public FightData GetExampleFightData()
    {
        return FightData.CreateFromJSON(exampleServerResponse.text);
    }
}
