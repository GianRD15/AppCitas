using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace AppCitas.Service.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account
        (
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);

    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile photofile)
    {
        var uploadResult = new ImageUploadResult();
        if (photofile.Length > 0)
        {
            using var stream = photofile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(photofile.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deteleParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deteleParams);

        return result;
    }
}
