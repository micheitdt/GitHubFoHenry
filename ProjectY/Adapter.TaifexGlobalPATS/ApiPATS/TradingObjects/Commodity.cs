using System.Collections;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public class Commodity : BaseTradingObject
    {
        private string exchangeName;
        private readonly string name;
        private string currency;
        private string group;
        private string onePoint;
        private int ticksPerPoint;
        private string tickSize;

        private readonly Hashtable contracts;
        
        public Commodity(CommodityStruct cs)
        {
            exchangeName = cs.ExchangeName;
            name = cs.ContractName;
            currency = cs.Currency;
            group = cs.Group;
            onePoint = cs.OnePoint;
            ticksPerPoint = cs.TicksPerPoint;
            tickSize = cs.TickSize;
            contracts = new Hashtable();
        }

        /** returns the unique key for the commodity within this exchange **/
        public override string GetKey()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        /** adds a contract to the hash table **/
        public void PutContract(Contract contract)
        {
            contracts.Add(contract.GetKey(), contract);
        }

        /** returns the contract item specified by the key string **/
        public Contract GetContract(string contractKey)
        {
            return (Contract)contracts[contractKey];
        }

        /** returns the enumerator for the contracts within this commodity **/
        public IDictionaryEnumerator GetContractsEnumerator()
        {
            return contracts.GetEnumerator();
        }
    }
}
