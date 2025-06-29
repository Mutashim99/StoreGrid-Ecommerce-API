﻿using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Services.AdminManagement.AdminReview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminReviewController : ControllerBase
    {
        private readonly IAdminReview _adminReviewService;

        public AdminReviewController(IAdminReview adminReviewService)
        {
            _adminReviewService = adminReviewService;
        }

      
        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<List<AdminReviewResponseDTO>>> GetAllReviews()
        {
            var result = await _adminReviewService.GetAllReviewsAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        
        [HttpGet("FilterByProduct/{productId}")]
        public async Task<ActionResult<List<AdminReviewResponseDTO>>> FilterByProduct(int productId)
        {
            var result = await _adminReviewService.FilterReviewsbyProductAsync(productId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

      
        [HttpDelete("DeleteReview/{id}")]
        public async Task<ActionResult<string>> DeleteReview(int id)
        {
            var result = await _adminReviewService.DeleteReviewAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
