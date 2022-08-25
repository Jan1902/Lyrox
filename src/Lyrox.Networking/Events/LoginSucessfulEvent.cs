using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Core.Events;

namespace Lyrox.Networking.Events
{
    internal class LoginSucessfulEvent : Event
    {
        public Guid UUID { get; }
        public string Username { get; }

        public LoginSucessfulEvent(Guid uUID, string username)
        {
            UUID = uUID;
            Username = username;

            Console.WriteLine("Username: " + Username);
        }
    }
}
