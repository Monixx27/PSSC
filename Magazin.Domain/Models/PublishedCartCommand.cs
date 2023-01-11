using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Domain.Models
{
    public record PublishCartCommand
    {
        public PublishCartCommand(IReadOnlyCollection<UnValidatedItem> inputCartItems)
        {
            InputCartItems = inputCartItems;
        }

        public IReadOnlyCollection<UnValidatedItem> InputCartItems { get; }
    }
}
