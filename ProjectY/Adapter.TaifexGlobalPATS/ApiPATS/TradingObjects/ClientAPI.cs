using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Serialization;
using Adapter.TaifexGlobalPATS.ApiPATS;

namespace Adapter.TaifexGlobalPATS.TradingObjects
{
    public class ContractData : IFormattable , IDisposable
    {
        public string Data1;
        public string Data2;
        public void contractData()
        {

        }

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return format.ToString();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
             
        }

        #endregion
    }
    public class MyDataList :   Dictionary<int,ContractData>
    {

  
    }

    /** ClientAPI class will hold references to everything to do with the Delphi API.
    * The constructor will start the logon process by initialising the Delphi API, logging on
    * and downloading all of the trading data
    **/
      public struct LoginStruct: IDisposable
      {
          /// <summary>
          /// Enter the user name required to connect to the STAS Server
          /// </summary>
         public string lgUser;
          /// <summary>
          /// Enter the password required to connect to the STAS Server
          /// </summary>
         public string lgPassword;
          /// <summary>
          /// Enter the IPaddress of the Stats Host Server
          /// </summary>
         public string hsIpAddress;
          /// <summary>
          /// Enter the IPaddress of the Price Server PDD  
          /// </summary>
         public string pdIpAddress;
          /// <summary>
          /// Enter the Port number for the Host ipAddress
          /// </summary>
         public string hPort;
          /// <summary>
          /// Enter the host port of the   
          /// </summary>
         public string pport;
        /// <summary>
        /// New Password
        /// </summary>
          public string lgNewPassword;
        #region IDisposable Members

        public void Dispose()
        {
            Dispose();
        }

        #endregion
    }

    public class MyEventArgs : EventArgs
    {
        private string msg;

        public MyEventArgs(string messageData)
        {
            msg = messageData;
        }
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }
    }

   

	//public class ClientAPI : LogonStatusListenerInterface, HostLinkStatusListenerInterface, 
 //       PriceLinkStatusListenerInterface, DownloadCompleteListenerInterface 
	//{   
 //       public event EventHandler<MyEventArgs> ProgressEvent;
    
 //       public string workingDirectory = "";
	//	public ClientAPIMethods clientAPIMethods;
 //       public Listeners listeners;
 //       public Logging log;
 //       public Callbacks callbacks;
 //       public bool callbackThreadTerminate;
 //       public string defaultTraderAccount;
 //       public FormLogon formLogon;
 //       public GUIToolbar guiToolbar;
 //       private Hashtable exchanges;
 //       private Hashtable traderAccounts;

 //       public void DoGenericEvent(string val)
 //       {
 //           //this.ProgressEvent(this, new MyEventArgs(val));
 //           // Copy to a temporary variable to be thread-safe.
 //           EventHandler<MyEventArgs> temp = ProgressEvent;
 //           if (temp != null)
 //               temp(this, new MyEventArgs(val));
 //       }
 //       private static void SampleEventHandler(object src, MyEventArgs mea)
 //       {
 //           Console.WriteLine(mea.Message);
 //       }

	//	public ClientAPI(string workingDir, FormLogon formLogon,  LoginStruct LoginDetail)
	//	{
 //           try
 //           {
 //                this.formLogon = formLogon;
 
 //               workingDirectory = workingDir;
 //               Init();
 
 //               clientAPIMethods.doInitialise();

 //               callbacks = new Callbacks(this);

 //               callbackThreadTerminate = false;

 //               clientAPIMethods.doSetHostHandShake(0, 3000);

 //               clientAPIMethods.doSetPriceHandShake(0, 3000);

 //               clientAPIMethods.doSetHostHandShake(10, 3000);

 //               clientAPIMethods.doSetPriceHandShake(10,3000 );

 //               clientAPIMethods.doDiagnostics(255);
                
 //               clientAPIMethods.doSetHostAddress(LoginDetail.hsIpAddress,LoginDetail.hPort);
          
 //               clientAPIMethods.doSetPriceAddress(LoginDetail.pdIpAddress,LoginDetail.pport);

 //               clientAPIMethods.doEnableEncryption('B');

 //               clientAPIMethods.doSetInternetUser('0');

 //               clientAPIMethods.doNotifyAllMessages('Y');

 //               clientAPIMethods.doLockUPdates();

 //               clientAPIMethods.doSuperTas('Y');

 //               clientAPIMethods.doReady();
                
 //              // this.DoGenericEvent("Test");
 //               //DoGenericEvent("Test");
 //               System.Threading.Thread.Sleep(5000);
 //               if ((LoginDetail.lgUser != null) && (LoginDetail.lgPassword != null))
 //               {
 //                   Debug.WriteLine("About to connect to server");
 //                   clientAPIMethods.doLogon(LoginDetail.lgUser, LoginDetail.lgPassword, "", 'Y', 'Y');
 //               }
 //               else
 //               {
 //                   Debug.WriteLine("About to connect to server");
 //                   DoGenericEvent("About To Connect To The Server");
 //               }
             
        
 //           }
 //           catch (Exception e)
 //           {
 //               log.Write(Constants.logError, "Error creating the ClientAPI: " + e.Message);
 //               Debug.WriteLine(e.Message.ToString());
 //           }
	//	}
       
 //       /** initialises the clientAPI object, this involves creating objects that will be 
 //        * need throughout the execution of this App**/
 //       private void Init()
 //       {
 //           //create a log file to store all method calls and errors
 //           log = new Logging();
 //           clientAPIMethods = new ClientAPIMethods(this);            
 //           listeners = new Listeners(); // create the class which will reference all listeners
 //           exchanges = new Hashtable();
 //           traderAccounts = new Hashtable();
 //       }

 //       /** Stops the callback thread, logs off the user and closes the app **/
 //       public void DestroyClientAPI()
 //       {
 //           try
 //           {
 //               log.Write(Constants.logComment, "Destroying the Client API and exiting program");
 //               log.Write(Constants.logComment, "Calling logoff");
 //               clientAPIMethods.doLogoff();
 //               log.Write(Constants.logComment, "Terminating the callback thread");
 //               callbackThreadTerminate = true;
 //               log.Write(Constants.logComment, "End of session");
 //               log.CloseFile();
 //               Application.Exit();
 //           }
 //           catch (Exception e)
 //           {
 //               MessageBox.Show("Error destroying clientAPI - " + e.Message);
 //           }
 //       }

 //       /** returns the default trader account received from the Delphi API **/
 //       public string GetDefaultTraderAccount()
 //       {
 //           return defaultTraderAccount;
 //       }

 //       /** Returns a trader account specified by the key**/
 //       public TraderAccount GetTraderAccount(string traderAccountKey)
 //       {
 //           return (TraderAccount)traderAccounts[traderAccountKey];
 //       }

 //       public IDictionaryEnumerator GetTraderAccountEnumerator()
 //       {
 //           return traderAccounts.GetEnumerator();
 //       }

 //       /** Adds a trader account object to the hash table**/
 //       public void PutTraderAccount(TraderAccount traderAcccount)
 //       {
 //           this.traderAccounts.Add(traderAcccount.GetKey(), traderAcccount);
 //       }

 //       /** returns the exchange object referenced by the key provided **/
 //       public Exchange GetExchange(string exchangeKey)
 //       {
 //           if (exchangeKey != null)
 //           {

 //           }
 //           return (Exchange)exchanges[exchangeKey];
 //       }

 //       /** adds an exchange object to the hash table **/
 //       public void PutExchange(Exchange exchange)
 //       {
 //           formLogon.SetProgBar();
 //           this.exchanges.Add(exchange.GetKey(), exchange);
 //       }

 //       /** creates the gui toolbar where all trading tools will be created **/
 //       public void CreateGUIToolbar()
 //       {
 //           formLogon.CreateGUIToolbar();
 //       }

 //       /** returns the enumerator for the exchanges **/

 //       public IDictionaryEnumerator GetExchangesEnumerator()
 //       {
 //           return exchanges.GetEnumerator();
 //       }

 //       #region entry points for all the listeners
 //       /** Entry point from the LogonStatusListener **/
 //       public void LogonStatusAction(int statusInt, string statusStr, string reason, string defaultTraderAccount)
 //       {
 //           log.Write(Constants.logComment, "LogonStatus = " + statusStr + ". " + reason + ". DefaultTrader Account = " + defaultTraderAccount);
 //           if (defaultTraderAccount != "")
 //               this.defaultTraderAccount = defaultTraderAccount;
 //           if (statusInt == Constants.LogonStatusForcedOut)
 //           {
 //               clientAPIMethods.doLogoff();
 //               formLogon.SetEnableLogonButton();
 //           }
 //           else if (statusInt != Constants.LogonStatusLogonSucceeded)
 //           {
 //               formLogon.SetMessageStatus("Error in logon (" + statusStr + ") " + reason, statusInt);
 //              // MessageBox.Show("Error in logon (" + statusStr + ") " + reason);
 //               formLogon.SetEnableLogonButton();
 //           }
 //           else
 //               formLogon.SetLabelStatusText("Downloading session data...");
 //           listeners.logonStatusListener.removeListener(this);
 //       }
        
 //       /** Entry point from the HostLinkStatusListener **/
 //       public void HostLinkStatusAction(int oldStatus, int newStatus)
 //       {
 //           log.Write(Constants.logComment, "Host link changed from " + oldStatus + " to " + newStatus);
 //           if (newStatus == Constants.LinkStatusConnected)
 //           {
 //               listeners.logonStatusListener.addListener(this);
 //               listeners.priceLinkStatusListener.addListener(this);
 //               listeners.downloadCompleteListener.addListener(this);
 //               formLogon.SetLabelStatusText("Logging in...please wait");
 //               formLogon.SetProgBar();
 //               //clientAPIMethods.doLogon("67720103", "21GOTRADE", "", 'Y', 'Y');
            
 //           }

 //       }
 //       public void UpdateProgressBar()
 //       {
 //           formLogon.SetProgBar();
 //       }
 //       /** Entry point from the PriceLinkStatusListener **/
 //       public void PriceLinkStatusAction(int oldStatus, int newStatus)
 //       {
 //           log.Write(Constants.logComment, "Price link changed from " + oldStatus + " to " + newStatus);
 //       }        

 //       /** Entry point for the DownloadCompleteListener **/
 //       public void DownloadCompleteAction()
 //       {
 //           listeners.downloadCompleteListener.removeListener(this);
 //           formLogon.SetLabelStatusText("Downloading session data...");
 //           //now we need to obtain all the tradables from the Delphi API
 //           try
 //           {
 //               //get the trader account count and collect all trader account objects
 //               int traderCount = clientAPIMethods.doTraderCount();
 //               for (int x = 0; x < traderCount; x++)
 //               {
 //                   byte[] buffer = new byte[Constants.SIZE_OF_TRADERACCOUNT_STRUCT];
 //                   clientAPIMethods.doGetTrader(x, ref buffer);
 //                   new TraderAccount(buffer, this);                    
 //               }
                
 //               //get the exchange count and cycle through the exchanges storing them in the hashtable
 //               int exchangeCount = clientAPIMethods.doExchangeCount();
 //               for (int x = 0; x < exchangeCount; x++)
 //               {
 //                   byte[] buffer = new byte[Constants.SIZE_OF_EXCHANGE_STRUCT];
 //                   clientAPIMethods.doGetExchange(x, ref buffer);
 //                   new Exchange(buffer, this);                    
 //               }
 //               //collect all the commodities
 //               int commodityCount = clientAPIMethods.doCommodityCount();
 //               for (int x = 0; x < commodityCount; x++)
 //               {
 //                   byte[] buffer = new byte[Constants.SIZE_OF_COMMODITY_STRUCT];
 //                   clientAPIMethods.doGetCommodity(x, ref buffer);
 //                   new Commodity(buffer, this);                    
 //               }
 
 //               //collect all the contracts
 //               int contractCount = clientAPIMethods.doContractCount();
 //               for (int x = 0; x < contractCount; x++)
 //               {
 //                   byte[] buffer = new byte[Constants.SIZE_OF_CONTRACT_STRUCT];
 //                   clientAPIMethods.doGetContract(x, ref buffer);
 //                   new Contract(buffer, this);                    
 //               }
 //               //collect all the order types

 //               CreateGUIToolbar();

 //               int orderTypeCount = clientAPIMethods.doOrderTypeCount();
 //               for (int x = 0; x < orderTypeCount; x++)
 //               {
 //                   byte[] buffer = new byte[Constants.SIZE_OF_ORDERTYPE_STRUCT];
 //                   clientAPIMethods.doGetOrderType(x, ref buffer);
 //                   new OrderType(buffer, this);
 //               }
 //           }                
 //           catch (Exception e)
 //           {
 //               log.Write(Constants.logError, " while collecting trading session data - " + e.Message);
 //           }
 //          // CreateGUIToolbar();
 //       }
 //       #endregion

       
 //   }
}
