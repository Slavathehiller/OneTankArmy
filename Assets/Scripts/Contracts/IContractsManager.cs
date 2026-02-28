using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IContractsManager
{
    public ContractData CurrentContract { get; set; }
    public ContractStatus CurrentContractStatus { get; set; }
    public void SaveData();
}

