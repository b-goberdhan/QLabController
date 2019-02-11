using System;
using System.Collections.Generic;
using System.Text;

namespace QLabOSCInterface.Constants
{
    public class CueConstants
    {
        public const string CUE_NUMBER = "cue/{0}/";
        public const string CUE_ID = "cue_id/{0}/";
        public const string START = "start";
        public const string STOP = "stop";
        public const string HARD_STOP = "hardStop";
        public const string DURATION = "duration";
        #region Text
        public const string TEXT = "text";
        public const string LIVE_TEXT = "liveText";
        #endregion
        #region Light
        public const string LIGHT_COMMAND = "lightCommandText";
        public const string LIGHT_REMOVE_COMMAND = "removeLightCommandsMatching";
        public const string LIGHT_REPLACE_COMMAND = "replaceLightCommand";
        #endregion
    }
}
