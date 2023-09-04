using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts
{
    public interface UserValidated
    {
        Guid UserId { get; }
        string Email { get; }
    }
}