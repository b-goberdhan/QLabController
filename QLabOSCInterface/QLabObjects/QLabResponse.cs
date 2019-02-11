using System;
using System.Collections.Generic;
using System.Text;

namespace QLabOSCInterface.QLabClasses
{
    public class QLabResponse<TData> : QLabResponse
    { 
        public TData data { get; set; }
    }

    public class QLabResponse
    {
        public string workspace_id { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        /// you changed this btw!
    }
}
