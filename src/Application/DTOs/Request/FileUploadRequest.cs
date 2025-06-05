using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request;

public class FileUploadRequest
{
    [Required]
    public IFormFile File { get; set; }
}
