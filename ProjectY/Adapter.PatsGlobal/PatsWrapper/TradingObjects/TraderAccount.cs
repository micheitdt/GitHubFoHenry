namespace Adapter.PatsGlobal.PatsWrapper
{
    public class TraderAccount : BaseTradingObject
    {
        private readonly string name;
        
        public TraderAccount(TraderAccountStruct tas)
        {
            name = tas.TraderAccount;
        }

        public override string GetKey()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
