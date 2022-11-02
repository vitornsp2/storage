using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace findox.Domain.Interfaces.Service
{
    public interface IBaseService
    {
        IDictionary<string, string[]> ToDictionary(Exception ex);
    }
}