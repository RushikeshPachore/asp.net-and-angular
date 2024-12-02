using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly EmplyoeeContext _context;
        private readonly IWebHostEnvironment _environment;
        private object _hostingEnvironment;

        public ImageController(EmplyoeeContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/Image/Upload
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImages([FromForm] int employeeId, [FromForm] List<IFormFile> images)
        {

            try
            {
                if (!_context.TblEmployee.Any(e => e.Id == employeeId))
                {
                    return NotFound(new { message = "Employee not found." });
                }

                if (images == null || images.Count == 0)
                {
                    return BadRequest(new { message = "No images provided." });
                }

                var uploadedImages = new List<TblImage>();

                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Save image data in the database
                        var tblImage = new TblImage
                        {
                            EmployeeId = employeeId,
                            MultiImage = $"/images/{uniqueFileName}"
                        };

                        uploadedImages.Add(tblImage);
                    }
                }

                await _context.TblImage.AddRangeAsync(uploadedImages);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Images uploaded successfully.", images = uploadedImages });
            }

            catch(Exception ex)
            {
                // Log the exception (use ILogger or any logging framework)
                return StatusCode(500, new { message = "An error occurred while uploading images.", error = ex.Message });
            }
        }

        // GET: api/Image/Employee/{employeeId}
        [HttpGet("Employee/{employeeId}")]
        public async Task<IActionResult> GetImagesByEmployee(int employeeId)
        {
            if (!_context.TblEmployee.Any(e => e.Id == employeeId))
            {
                return NotFound(new { message = "Employee not found." });
            }

            var images = await _context.TblImage
                .Where(i => i.EmployeeId == employeeId)
                .Select(i =>  i.MultiImage)
                .ToListAsync();

            return Ok(images);
        }






        [HttpPut("UpdateImages/{employeeId}")]
        public async Task<IActionResult> UpdateImages(int employeeId, [FromForm] List<IFormFile> newImages, [FromForm] List<int> deletedImageIds)
        {
            if (!_context.TblEmployee.Any(e => e.Id == employeeId))
            {
                return NotFound(new { message = "Employee not found." });
            }

            // Remove deleted images
            if (deletedImageIds != null && deletedImageIds.Any())
            {
                var imagesToDelete = await _context.TblImage
                    .Where(i => deletedImageIds.Contains(i.Id) && i.EmployeeId == employeeId)
                    .ToListAsync();

                if (imagesToDelete.Any())
                {
                    foreach (var image in imagesToDelete)
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, image.MultiImage.TrimStart('/'));
                        // Delete the image file from server
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        // Remove the image record from the database
                        _context.TblImage.Remove(image);
                    }
                }
            }

            // Add new images
            var uploadedImages = new List<TblImage>();
            if (newImages != null && newImages.Count > 0)
            {
                foreach (var image in newImages)
                {
                    if (image.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the new image to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Save the new image data in the database
                        var tblImage = new TblImage
                        {
                            EmployeeId = employeeId,
                            MultiImage = $"/images/{uniqueFileName}"
                        };

                        uploadedImages.Add(tblImage);
                    }
                }
            }

            // Add the new images to the database
            if (uploadedImages.Any())
            {
                await _context.TblImage.AddRangeAsync(uploadedImages);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Images updated successfully.", images = uploadedImages });
        }





        [HttpPost("removeImages/{employeeId}")]
        public async Task<IActionResult> RemoveImages(int employeeId, [FromBody] List<int> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0)
            {
                return BadRequest("No images to remove.");
            }

            var imagesToDelete = await _context.TblImage
                .Where(image => imageIds.Contains(image.Id) && image.EmployeeId == employeeId)
                .ToListAsync();

            if (imagesToDelete.Count == 0)
            {
                return NotFound("No images found for removal.");
            }

            // Delete the images from the database
            _context.TblImage.RemoveRange(imagesToDelete);
            await _context.SaveChangesAsync();

            // Optionally, delete the physical files from the server
            foreach (var image in imagesToDelete)
            {
                var filePath = Path.Combine(_environment.WebRootPath, image.MultiImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return Ok(new { message = "Images removed successfully." });
        }



        // DELETE: api/Image/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.TblImage.FindAsync(id);
            if (image == null)
            {
                return NotFound(new { message = "Image not found." });
            }

            var filePath = Path.Combine(_environment.WebRootPath, image.MultiImage.TrimStart('/'));

            // Delete the file from the server
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Delete the image record from the database
            _context.TblImage.Remove(image);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Image deleted successfully." });
        }
    }
}
