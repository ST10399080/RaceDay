using Microsoft.AspNetCore.Http;

namespace RaceDay.API.Services;

public class FileStorageService
{
    private readonly IWebHostEnvironment _environment;

    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveEventImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file was provided.");
        }

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException(
                "The image cannot exceed 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            throw new ArgumentException(
                "Only JPG, JPEG, PNG and WEBP images are allowed.");
        }

        if (!await HasValidImageSignatureAsync(file, extension))
        {
            throw new ArgumentException(
                "The file content does not match a supported image format.");
        }

        var uploadFolder = Path.Combine(
            _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),
            "uploads",
            "events");

        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(uploadFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/events/{fileName}";
    }
    private static async Task<bool> HasValidImageSignatureAsync(
    IFormFile file,
    string extension)
    {
        var header = new byte[12];

        await using var stream = file.OpenReadStream();

        try
        {
            await stream.ReadExactlyAsync(header);
        }
        catch (EndOfStreamException)
        {
            return false;
        }

        return extension switch
        {
            ".jpg" or ".jpeg" =>
                header[0] == 0xFF &&
                header[1] == 0xD8 &&
                header[2] == 0xFF,

            ".png" =>
                header[0] == 0x89 &&
                header[1] == 0x50 &&
                header[2] == 0x4E &&
                header[3] == 0x47 &&
                header[4] == 0x0D &&
                header[5] == 0x0A &&
                header[6] == 0x1A &&
                header[7] == 0x0A,

            ".webp" =>
                header[0] == (byte)'R' &&
                header[1] == (byte)'I' &&
                header[2] == (byte)'F' &&
                header[3] == (byte)'F' &&
                header[8] == (byte)'W' &&
                header[9] == (byte)'E' &&
                header[10] == (byte)'B' &&
                header[11] == (byte)'P',

            _ => false
        };
    }
}
