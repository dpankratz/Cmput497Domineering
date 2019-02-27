using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public static class Settings
{
    public static AgentType AgentOne = AgentType.Random;
    public static AgentType AgentTwo = AgentType.Random;
}


public enum AgentType
{
    Unimplemented = -2,
    Human = -1,
    //Computer types after this
    Random = 0,
    Greedy = 1,
    Mcts = 2
}