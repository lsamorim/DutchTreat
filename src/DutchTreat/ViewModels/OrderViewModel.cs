﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime? OrderDate { get; set; }
        [Required]
        [MinLength(4)]
        public string OrderNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderItemViewModel> Items { get; set; }
    }
}
