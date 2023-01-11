using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Dto.Models
{
    [Serializable]
    public record PublishedCartItemsDto ()
    {
        public IEnumerable<ItemDto> Items { get; }
        public DateTime PublishedDate { get; }
        public decimal? Total { get; }
    }
}
