using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContractData
{
    public int ID;
    public ContractType Type;
    public string Description;
    public string Details;
    public int RatingNeeded;
}

[CreateAssetMenu(fileName = "Contracts", menuName = "Scriptable Objects/Contracts")]
public class Contracts : ScriptableObject
{
    public List<ContractData> Data;
}