using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos
{
    public class UpdateCategoryInputDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        //[JsonProperty("status")]
        //public string Status { get; set; }

        //[JsonProperty("metadata_title")]
        //public string MetaDataTitle { get; set; }


        //[JsonProperty("metadata_description")]
        //public string MetaDataDescription { get; set; }

        //[JsonProperty("metadata_url")]
        //public string MetaDataUrl { get; set; }
    }
}

