using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class OrderHub : Hub
{
    public static void NotifyNewOrder()
    {
        var context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();
        context.Clients.All.receiveOrderNotification();
    }
}
