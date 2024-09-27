using Game.Signals.Core;
using Game.Signals.Utils;
using UnityEngine;

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