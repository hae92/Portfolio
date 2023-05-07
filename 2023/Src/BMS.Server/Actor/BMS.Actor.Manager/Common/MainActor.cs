using System.Reflection;
using Akka.Actor;
using BMS.Core.Common;
using BMS.Core.Logger;


namespace BMS.Actor.Manager.Common
{
    public class MainActor : UntypedActor, IActorBehavior
    {

        private Logger _logger;
        private ICancelable _schedule;

        public MainActor()
        {
            _logger = Singleton<Logger>.Instance;
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Create(object props)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMessage(object message)
        {
            throw new NotImplementedException();
        }

        public void Scheduler(object arg)
        {
            throw new NotImplementedException();
        }

        public void Start(object arg)
        {
            throw new NotImplementedException();
        }

        protected override void OnReceive(object message)
        {
            _logger.LogWrite(MethodBase.GetCurrentMethod(), String.Format("Recieved Message : {0}", message.ToString()));

            if (message.ToString() == "Run Schedule")
            {
                _schedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), Self, "Schedule Message", Self);
            }
            else if(message.ToString() == "Stop Schedule")
            {
                if (_schedule != null)
                {
                    _schedule.Cancel();
                }
            }
        }
    }
}
