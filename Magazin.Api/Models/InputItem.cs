using Magazin.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazin.Api.Models
{
    public class InputItem
    {
        [Required]
        [RegularExpression(Item.Pattern)]
        public String ItemId { get; set; }
        [Required]
        [Range(1,double.MaxValue)]
        public int Quantity { get; set; }
    }
}
