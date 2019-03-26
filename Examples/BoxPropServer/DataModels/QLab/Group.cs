using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.DataModels.QLab
{
    public class Group
    {
        public string Id { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }
}
