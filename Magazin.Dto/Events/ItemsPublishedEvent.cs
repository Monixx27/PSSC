using Magazin.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Dto.Events
{
    public class ItemsPublishedEvent
    {
        public List<ItemDto> Items { get; init; }
    }
}
