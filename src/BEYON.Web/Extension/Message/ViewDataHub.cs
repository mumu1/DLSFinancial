using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace BEYON.Web.Extension.Message
{
    [HubName("ViewDataHub")]
    public class ViewDataHub : Hub
    {
        //this fucntion will be called by client and the inside function 
        //Clients.Others.talk(message);
        //will be called by clinet javascript function .
        public void SendMessag(object messageObj)
        {
            string strSerializeJSON = JsonConvert.SerializeObject(messageObj);
            Clients.Others.AuditStatus(strSerializeJSON);
        }

        public void SendMessag(string message)
        {
            Clients.Others.AuditStatus(message);
        }
    }
}