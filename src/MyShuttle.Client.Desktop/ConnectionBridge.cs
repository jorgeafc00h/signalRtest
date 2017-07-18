using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Desktop
{
    public class ConnectionBridge
    {
        private Action<string, string> add = null;
        internal void ConnectTohub(Action<string, string> addMOdelToCollection)
        {
            ConnectTohub();
            add = addMOdelToCollection;
        }

        public void ConnectTohub(bool wait = false)
        {

            //if(wait) Thread.Sleep(new TimeSpan(0,0,10)); // wait 10secs
            var queryString = new Dictionary<string, string>();

            queryString.Add("whscode", "SP001");

            //HubConnection = null;
            //notificationsProxy = null;

            HubConnection = new HubConnection(Settings.WebApiUri + "/NotifyHub", queryString, useDefaultUrl: false);

            notificationsProxy = HubConnection.CreateHubProxy("NotifyHub");


            ConnectEvents();
            notificationsProxy.On("FireSync",FireSynchro) ;

            HubConnection.Start();
        }

        

        private void FireSynchro(dynamic model)
        {

            var title = (string)model.Title ;
            var make = (string) model.Content;
            
            if (add != null) add(title,make);
        }

        private void ConnectEvents()
        {
            HubConnection.Closed += () => { if (OnConnectionClosed != null) OnConnectionClosed(); };
            HubConnection.ConnectionSlow += () => { if (OnConnectionSlow != null) OnConnectionSlow(); };
            HubConnection.Reconnecting += () => { if (OnReconnecting != null) OnReconnecting(); };
            HubConnection.Reconnected += () => { if (OnReconnected != null) OnReconnected(); };
            HubConnection.StateChanged += (stateChange) =>
            {

                StateConnection = stateChange.NewState;
                if (StateConnection == ConnectionState.Disconnected) TryReconnect();

                if (OnStateChanged != null) OnStateChanged(stateChange.NewState.ToString());

                if (ConnectionState.Connected == StateConnection
                    && onFirstConnect != null && !IsLockFirstSync)
                {
                    onFirstConnect();
                    IsLockFirstSync = true;
                }

            };

            HubConnection.Error += (error) =>
            {

                //if(!isTryngReconect)  TryReconnect();
            };

        }

       

        public void TryReconnect()
        {
            isTryngReconect = true;
            HubConnection = null;
            notificationsProxy = null;
            

            Task.Run(() => ConnectTohub());
        }


        public event Action OnFireSync;
        public static event Action OnConnectionSlow;
        public static event Action OnConnectionClosed;
        public static event Action OnReconnecting;
        public static event Action OnReconnected;
        public static event Action<string> OnStateChanged;
        public static event Action OnRecived;
        public static event Action OnError;
        private bool isTryngReconect = false;
        private bool IsLockFirstSync = false;

        public event Action onFirstConnect;

        public static ConnectionState StateConnection;

        public static string StateConnectionString()
        {
            return StateConnection.ToString();
        }

        private HubConnection HubConnection = null;
        private IHubProxy notificationsProxy;
        private bool IsConnectionPending = false;

        private object connectionLocker = new object();
    }
}
