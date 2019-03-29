using System;
using System.Collections.Generic;
using System.Text;

namespace QLabInterface.QLabClasses
{
    public class WorkSpace
    {
        public string uniqueID { get; set; }
        public string displayName { get; set; }
        public string hasPasscode { get; set; }
        public string version { get; set; }
    }
}
