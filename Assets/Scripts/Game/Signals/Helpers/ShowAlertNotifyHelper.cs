using Game.Signals.Core;
using Game.Signals.Utils;

namespace Game.Signals.Helpers
{
    public class ShowAlertNotifyHelper
    {
        public static void ShowAlert(string message)
        {
            SignalSystem.SignalBus.Fire(ShowAlertNotifySignal.Create(message));
        }
    }
}