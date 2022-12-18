using ClassLib.Visitor;
using Microsoft.AspNetCore.SignalR.Client;
using static ClassLib.Command;

namespace ClassLib
{
    public class Player : Element
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}