﻿using System;
using System.Collections.Generic;

namespace WebTorrent.Data.Models
{
    public class ProductCategory : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}