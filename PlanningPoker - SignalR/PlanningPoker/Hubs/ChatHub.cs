namespace PlanningPoker.Hubs
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using PlanningPoker.Models;

    public class ChatHub : Hub
    {
        public IPlanningPokerDb db;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override Task OnDisconnected()
        {
            log.Info("Instance of hub - disconnected " + GC.GetTotalMemory(true));
            return base.OnDisconnected();
        }

        public override Task OnConnected()
        {
            log.Info("Instance of hub - connected " + GC.GetTotalMemory(true));
            return base.OnConnected();
        }
    }
}