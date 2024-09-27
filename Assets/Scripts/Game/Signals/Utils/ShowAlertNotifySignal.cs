namespace Game.Signals.Utils
{
    public class ShowAlertNotifySignal
    {
        private static readonly ShowAlertNotifySignal _instance = new();
        
        public string AlertMessage { get; private set; }

        public static ShowAlertNotifySignal Create( string message)
        {
            _instance.AlertMessage = message;
            return _instance;
        }
    }
}