using Akka.Actor;
using BMS.Actor.Manager.Common;
using BMS.Core.Common;
using BMS.Core.Logger;

namespace BMS.Actor.Manager
{
    public class Manager
    {
        private ActorSystem _actorSystem;
        private ClassCollector _collector = default(ClassCollector);
        private Logger _logger;
        private Dictionary<string, System.Type> _classes = new Dictionary<string, System.Type>();
        private IActorRef mainActor;

        public Manager(string systemName)
        {
            _actorSystem = ActorSystem.Create(systemName);
            _collector = Singleton<ClassCollector>.Instance;
            _logger = Singleton<Logger>.Instance;

            GetClassInformation("Collector");
            GetClassInformation("Common");
            GetClassInformation("Schedule");
            GetClassInformation("Simulator");

            var props = Props.Create<MainActor>();
            mainActor = _actorSystem.ActorOf(props, "MainActor");
        }

        public void GetClassInformation(string serviceName)
        {
            var func = _collector.GetExecutingAssembly();
            var currentNamespace = func.Invoke().GetName().Name;
            var targetNamespace = string.Format("{0}.{1}", currentNamespace, serviceName);
            var col = _collector.GetClasses(targetNamespace, func.Invoke().GetTypes());
            foreach (var key in col.Keys)
            {
                if (!_classes.ContainsKey(key))
                {
                    _classes.Add(key, col[key]);
                }
            }
        }

        public void SendMessage(string meesage)
        {
            mainActor.Tell(meesage);
        }
    }
}