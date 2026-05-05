using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Category
{
    //public class CategoryResultDto
    //{
    //    [JsonProperty("id")]
    //    public long Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("image")]
    //    public string Image { get; set; }

    //    [JsonProperty("status")]
    //    public string Status { get; set; }

    //    [JsonProperty("parent_id")]
    //    public long ParentId { get; set; }

    //    [JsonProperty("sort_order")]
    //    public int SortOrder { get; set; }

    //    [JsonProperty("update_at")]
    //    public string UpdateAt { get; set; }

    //    // لو محتاج sub_categories
    //    [JsonProperty("sub_categories")]
    //    public List<CategoryResultDto> SubCategories { get; set; }

    //    // للـ error handling
    //    public string Message { get; set; }
    //}
    public class CategoryResultDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        [JsonProperty("update_at")]
        public string UpdateAt { get; set; }

        [JsonProperty("sub_categories")]
        public List<CategoryResultDto> SubCategories { get; set; }

        [JsonProperty("metadata")]
        public CategoryMetadataDto Metadata { get; set; }

        [JsonProperty("show_in")]
        public CategoryShowInDto ShowIn { get; set; }

        public string Message { get; set; }
    }

    public class CategoryMetadataDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class CategoryShowInDto
    {
        [JsonProperty("app")]
        public bool App { get; set; }

        //[JsonProperty("salla_points")]
        //public bool SallaPoints { get; set; }
    }

    public class CreateCategoryRequestDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }  // Required

        [JsonProperty("show_in")]
        public CategoryShowInDto showIn { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentId { get; set; }

        //[JsonProperty("status")]
        //public string Status { get; set; }  // "active" or "hidden"

        //[JsonProperty("image")]
        //public string Image { get; set; }

        //[JsonProperty("metadata_title")]
        //public string MetadataTitle { get; set; }

        //[JsonProperty("metadata_description")]
        //public string MetadataDescription { get; set; }

        //[JsonProperty("metadata_url")]
        //public string MetadataUrl { get; set; }

        //[JsonProperty("sort_order")]
        //public int? SortOrder { get; set; }
    }

    public class CategoryProductDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        public string Message { get; set; }
    }
}
