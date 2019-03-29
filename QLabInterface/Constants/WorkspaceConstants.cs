using System;
using System.Collections.Generic;
using System.Text;

namespace QLabInterface.Constants
{
    public class WorkspaceConstants
    {
        public const string WORKSPACES = "/workspaces";
        public const string WORKSPACE = "/workspace/{0}/";
        public const string CONNECT = "connect";
        public const string DISCONNECT = "disconnect";
        public const string GO = "go";
        public const string PAUSE = "pause";
        public const string RESUME = "resume";
        public const string STOP = "stop";
        public const string CUE_LISTS = "cueLists";
        public const string DELETE_CUE_ID = "delete_id/{0}";
        public const string CREATE_CUE = "new";
        public const string MOVE = "move/{0}";
    }
}
