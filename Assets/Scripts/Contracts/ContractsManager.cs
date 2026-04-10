using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public enum ContractStatus
{
    Undefined = -1,
    Unsigned = 0,
    Signed = 1,
    Completed = 2,
    Failed = 3

}

public class ContractsManager : IContractsManager
{
    private const string CURRENT_CONTRACT_ID = "CurrentContractID";
    private const string CURRENT_CONTRACT_STATUS = "CurrentContractStatus";
    public ContractData CurrentContract {  get; set; }
    public ContractStatus CurrentContractStatus { get; set; }

    [Inject]
    private IPlayerSettings _playerSettings;

    public ContractsManager()
    {
        LoadData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(CURRENT_CONTRACT_ID, CurrentContract == null ? 0 : CurrentContract.ID);
        PlayerPrefs.SetInt(CURRENT_CONTRACT_STATUS, (int)CurrentContractStatus);
    }
    public void LoadData()
    { 
        var currentContractID = PlayerPrefs.GetInt(CURRENT_CONTRACT_ID, 0);
        if (currentContractID == 0)
        { 
            CurrentContract = null;
            CurrentContractStatus = ContractStatus.Unsigned;
            return;
        }
        var allContracts = Resources.Load<Contracts>("Contracts");
        CurrentContract = allContracts.Data.First(x => x.ID == currentContractID);
        CurrentContractStatus = (ContractStatus)PlayerPrefs.GetInt(CURRENT_CONTRACT_STATUS, 0); 
    }
}

