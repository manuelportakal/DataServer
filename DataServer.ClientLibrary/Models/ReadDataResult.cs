using System;

namespace DataServer.ClientLibrary.Models
{
    public class ReadDataResult
    {
        public string Value { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSucceded { get; set; }
    }
}
