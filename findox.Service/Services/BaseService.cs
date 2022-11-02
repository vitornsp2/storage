using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Interfaces.Service;

namespace findox.Service.Services
{
    public abstract class BaseService : IBaseService
    {
        public virtual IDictionary<string, string[]> ToDictionary(Exception ex)
        {
            var returnValue = new Dictionary<string, string[]>();

            returnValue.Add("Message", new List<string> { ex.Message }.ToArray());

            return returnValue;
        }

        public virtual void addMessage(IDictionary<string, string[]> messages, string key, string value)
        {
            messages.Add(key, new List<string> { value}.ToArray());
        }
    }
}