﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.CategoryDTOs
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
