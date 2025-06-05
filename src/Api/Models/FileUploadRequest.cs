using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class FileUploadRequest
{
    [Required]
    public IFormFile? File { get; set; }
}
