﻿using System.ComponentModel.DataAnnotations;

namespace WishlistAPI.Models.DataModels
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}