using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.Models
{
    /// <summary>
    /// Abstaction of an Internal Service Response to send as part of the body of an HTTP response
    /// </summary>
    /// <typeparam name="T">The Data object to be sent with the HTTP response</typeparam>
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool SuccessFlag { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}