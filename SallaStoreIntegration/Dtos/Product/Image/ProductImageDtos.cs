using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Product.Image
{
    public class ProductImageDtos
    {
    }

    public class AttachImageFormDto
    {
        public IFormFile Photo { get; set; }
    }

    public class AttachImageByProductIdFormDto
    {
        public IFormFile? Photo { get; set; }
        public string? Original { get; set; }
        [Range(0, 1, ErrorMessage = "Main must be 0 or 1")]
        public int? Main { get; set; }
        public int? Sort { get; set; }
        public string? Alt { get; set; }
    }

    public class UpdateImageFormDto
    {
        public IFormFile Photo { get; set; }
        [Range(0, 1, ErrorMessage = "Default must be 0 or 1")]
        public int? Default { get; set; }
        public int? Sort { get; set; }
        public string? Alt { get; set; }
    }

    public class AttachImageBySkuDto
    {
        [JsonProperty("photo")]
        public string? Photo { get; set; } // optional - file upload handled separately
    }
    public class AttachImageResponseDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("image")]
        public ImageDetailsDto Image { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("alt_seo")]
        public string? AltSeo { get; set; }

        [JsonProperty("video_url")]
        public string? VideoUrl { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }

    public class ImageDetailsDto
    {
        [JsonProperty("original")]
        public ImageResolutionDto? Original { get; set; }

        [JsonProperty("standard_resolution")]
        public ImageResolutionDto? StandardResolution { get; set; }

        [JsonProperty("low_resolution")]
        public ImageResolutionDto? LowResolution { get; set; }

        [JsonProperty("thumbnail")]
        public ImageResolutionDto? Thumbnail { get; set; }
    }

    public class ImageResolutionDto
    {
        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }
    }
    public class  AttachVideoRequestDto
    {
        [JsonProperty("video_url")]
        public string video_url { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
        [JsonProperty("alt")]
        public string alt { get; set; }
    }
}
