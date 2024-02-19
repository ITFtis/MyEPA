using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Utility
{
    public class TelegramMessageParameter
    {
        public TelegramChannelEnum Channel { get; set; }
        public SlackMessageLevelEnum MessageLevel { get; set; }
        public string Message { get; set; }
    }
}