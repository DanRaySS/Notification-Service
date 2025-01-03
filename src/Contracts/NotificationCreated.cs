using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public class NotificationCreated
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string Address { get; set; }
    }
}