using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Domain.Models
{
    public record UnValidatedItem(string id, int quantity) { };
}
