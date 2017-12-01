namespace Adapter.PatsGlobal.PatsWrapper
{

    /** all trading objects will extend the BaseTradingObject class **/
    public abstract class BaseTradingObject
    {
        public BaseTradingObject()
        {

        }

        /** used for converting Y N chars into boolean equivalents **/
        public bool YNToBool(char chr)
        {
            if (chr == 'Y')
                return true;
            return false;
        }

        /** all trading objects must implement the GetKey method **/
        public abstract string GetKey();     
        

    }
}
