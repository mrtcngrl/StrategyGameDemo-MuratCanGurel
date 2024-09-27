using Game.Signals.Utils;
using Zenject;

namespace Game.Signals.Core
{
    public static class SignalSystem
    {
        public static SignalBus SignalBus;
        public static void Initialize(DiContainer container)
        {
            SignalBusInstaller.Install(container);
            SignalBus = container.Resolve<SignalBus>();
            DeclareSignals();
        }

        private static void DeclareSignals()
        {
            SignalBus.DeclareSignal<ShowAlertNotifySignal>();
        }
    }
}