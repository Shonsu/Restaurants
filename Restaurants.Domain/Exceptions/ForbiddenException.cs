using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Domain.Exceptions
{
    public class ForbiddenException(string message):Exception(message)
    {
        
    }
}