using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Brands
{
  
    public class CreateBrandDto
    {
        public string Name { get; set; }

        public IFormFile Logo { get; set; }      // required
        public IFormFile? Banner { get; set; }   // optional

        public string? Description { get; set; }
        public string? MetadataTitle { get; set; }
        public string? MetadataDescription { get; set; }
        public string? MetadataUrl { get; set; }

        public string? TranslationsJson { get; set; }
    }
    public class TranslationDto
    {
        public string Locale { get; set; }  // "en"

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? MetadataTitle { get; set; }
        public string? MetadataDescription { get; set; }
        public string? MetadataUrl { get; set; }
    }
}
