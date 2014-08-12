﻿namespace PMS.Models.DomainModels
{

    /// <summary>
    /// Product Domain Model
    /// </summary>
    public class Product
    {

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public virtual Category Category { get; set; }
    }
}