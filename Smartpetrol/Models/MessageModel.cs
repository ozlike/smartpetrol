using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Models
{
    public class MessageModel
    {
        public int SecondsToRedirect { get; set; } = 2;
        public string Message { get; set; } = "Перенаправление...";
        public string Title { get; set; } = "Loading";
        public string Url { get; set; } = "/";
        public bool Error { get; set; } = false;

        public MessageModel() { }

        public MessageModel(string url, string message, bool error = false, int secondsToRedirect = 2)
        {
            Url = url;
            Message = message;
            Error = error;
            SecondsToRedirect = secondsToRedirect;
        }
    }
}
