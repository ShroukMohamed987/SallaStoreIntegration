namespace SallaStoreIntegration.Dtos.Brands
{
    public class UpdateBrandDto
    {
       
            public string Name { get; set; }

            public IFormFile Logo { get; set; }      // required
            public IFormFile Banner { get; set; }   

            public string Description { get; set; }
            public string MetadataTitle { get; set; }
            public string MetadataDescription { get; set; }
            public string MetadataUrl { get; set; }

            public string TranslationsJson { get; set; }
        
    }
}
