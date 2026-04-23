using Microsoft.AspNetCore.Mvc;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Dtos.Customers;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Dtos.Product;
using SallaStoreIntegration.Dtos.Product.Image;
using SallaStoreIntegration.Enums.Products;
using SallaStoreIntegration.Repositories.Auth;
using SallaStoreIntegration.Repositories.Brand;
using SallaStoreIntegration.Repositories.Category;
using SallaStoreIntegration.Repositories.Customer;
using SallaStoreIntegration.Repositories.Order;
using SallaStoreIntegration.Repositories.Product;

namespace DexefExternalStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SallaController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICustomerRepository _customerRepository;

        public SallaController(
            IAuthRepository authRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IBrandRepository brandRepository,
            ICustomerRepository customerRepository)
        {
            _authRepository = authRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _brandRepository = brandRepository;
            _customerRepository = customerRepository;
        }

        #region Auth
        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var result = await _authRepository.AuthorizeAsync();
            return Redirect(result);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            var result = await _authRepository.CallbackAsync(code, state);
            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _authRepository.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }

        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromQuery] string token)
        {
            var result = await _authRepository.GetUserInfoAsync(token);
            return Ok(result);
        }
        #endregion

        #region Categories
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories([FromQuery] string token)
        {
            var result = await _categoryRepository.GetCategoriesAsync(token);
            return Ok(result);
        }

        [HttpGet("GetCategory/{categoryId}")]
        public async Task<IActionResult> GetCategory(long categoryId, [FromQuery] string token)
        {
            var result = await _categoryRepository.GetCategoryAsync(token, categoryId);
            return Ok(result);
        }

        [HttpPost("Create/category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto inputDto, [FromQuery] string token)
        {
            var result = await _categoryRepository.CreateCategoryAsync(token, inputDto);
            return Ok(result);
        }

        [HttpPut("Update/category/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(long categoryId, [FromBody] CreateCategoryRequestDto inputDto, [FromQuery] string token)
        {
            var result = await _categoryRepository.UpdateCategoryAsync(token, categoryId, inputDto);
            return Ok(result);
        }

        [HttpDelete("Delete/category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(long categoryId, [FromQuery] string token)
        {
            var result = await _categoryRepository.DeleteCategoryAsync(token, categoryId);
            if (!result) return BadRequest("Failed to delete category");
            return Ok(result);
        }

        [HttpGet("GetCategoryChildren/{categoryId}")]
        public async Task<IActionResult> GetCategoryChildren(long categoryId, [FromQuery] string token)
        {
            var result = await _categoryRepository.GetCategoryChildrenAsync(token, categoryId);
            return Ok(result);
        }

        [HttpGet("GetCategoryProducts/{categoryId}")]
        public async Task<IActionResult> GetCategoryProducts(long categoryId, [FromQuery] string token)
        {
            var result = await _categoryRepository.GetCategoryProductsAsync(token, categoryId);
            return Ok(result);
        }

        [HttpGet("SearchCategories")]
        public async Task<IActionResult> SearchCategories(
            [FromQuery] string token,
            [FromQuery] string keyword = null,
            [FromQuery] List<long> ids = null)
        {
            var result = await _categoryRepository.SearchCategoriesAsync(token, keyword, ids);
            return Ok(result);
        }
        #endregion

        #region Products
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts([FromQuery] FilterProductDto filterDto)
        {
            var result = await _productRepository.GetProductsAsync(filterDto);
            return Ok(result);
        }

        [HttpPost("Create/Product")]
        public async Task<IActionResult> CreateProduct(CreateProductDto inputDto, string token)
        {
            var result = await _productRepository.CreateProductAsync(inputDto, token);
            return Ok(result);
        }
        [HttpGet("GetProductDetails")]
        public async Task<IActionResult> GetProductDetails([FromQuery] int productId,string token)
        {
            var result = await _productRepository.GetProductAsync(productId,token);
            return Ok(result);
        }
        [HttpPost("Update/Product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto inputDto, string token)
        {
            var result = await _productRepository.UpdateProductAsync(inputDto, token);
            return Ok(result);
        }
        [HttpGet("GetProductBySku")]
        public async Task<IActionResult> GetProductBySku([FromQuery] string sku, string token)
        {
            var result = await _productRepository.GetProductBySKUAsync(sku, token);
            return Ok(result);
        }
        [HttpPost("ChangeProductStatus")]
        public async Task<IActionResult> ChangeProductStatus(ChangeProductStatusInputDto inputDto, string token)
        {
            var result = await _productRepository.ChangeProductStatusAsync(inputDto, token);
            return Ok(result);
        }
        [HttpPost("UpdateProductBySku")]
        public async Task<IActionResult> UpdateProductBySku(UpdateProductDto inputDto,string sku, string token)
        {
            var result = await _productRepository.UpdateProductBySkuAsync(inputDto,sku, token);
            return Ok(result);
        }
        [HttpPost("UpdateProductPriceBySku")]
        public async Task<IActionResult> UpdateProductPriceBySku(UpdateProductPriceBySkuDto inputDo, string sku, string token)
        {
            var result = await _productRepository.UpdateProductPriceBySkuAsync(inputDo, sku, token);
            return Ok(result);
        }
        [HttpPost("UpdateBulkProductPrice")]
        public async Task<IActionResult> UpdateBulkProductPrice(BulkUpdateProductPriceRequestDto inputDto, string token)
        {
            var result = await _productRepository.UpdateBulkProductPriceAsync(inputDto, token);
            return Ok(result);
        }
        [HttpPost("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId, string token)
        {
            var result = await _productRepository.DeleteProducteAsync(productId, token);
            return Ok(result);
        }
        [HttpPost("DeleteProductBySku/{sku}")]
        public async Task<IActionResult> DeleteProductBySku(string sku, string token)
        {
            var result = await _productRepository.DeleteProducteBySkuAsync(sku, token);
            return Ok(result);
        }
        // Controller
        [HttpPost("ImportProducts")]
        public async Task<IActionResult> ImportProducts(
            IFormFile file,
            [FromQuery] ImportProductEnum type,
            [FromQuery] string token)
        {
            using var stream = file.OpenReadStream();
            var result = await _productRepository.ImportProducteAsync(stream, file.FileName, type, token);
            return Ok(result);
        }

        #region productImages
        [HttpPost("AttachImageBySku")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AttachImageBySku(
            [FromForm] AttachImageFormDto photo,
            [FromQuery] string sku,
            [FromQuery] string token)
        {
            

            var result = await _productRepository.AttachImageBySkuAsync(
                photo, sku, token);

            return Ok(result);
        }

        [HttpPost("AttachImageByProductId")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AttachImageByProductId(
            [FromForm] AttachImageByProductIdFormDto form,
            [FromQuery] long productId,
            [FromQuery] string token)
        {
            var result = await _productRepository.AttachImageByProductIdAsync(form, productId, token);
            return Ok(result);
        }

        [HttpPost("UpdateImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage(
            [FromForm] UpdateImageFormDto form,
            [FromQuery] string imageId,
            [FromQuery] string token)
        {
            var result = await _productRepository.UpdateImageAsync(form, imageId, token);
            return Ok(result);
        }
        [HttpPost("DeleteImage")]
        
        public async Task<IActionResult> DeleteImage(
          
           [FromQuery] string imageId,
           [FromQuery] string token)
        {
            var result = await _productRepository.DeleteImageAsync( imageId, token);
            return Ok(result);
        }
        [HttpPost("AttachYoutubeVideo")]

        public async Task<IActionResult> AttachYoutubeVideo(
            AttachVideoRequestDto inputDto,
           [FromQuery] int productId,
           [FromQuery] string token)
        {
            var result = await _productRepository.AttachYoutubeVideoAsync(inputDto,productId, token);
            return Ok(result);
        }

        #endregion
        #endregion

        #region Orders
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string token,
            [FromQuery] string? keyword = null,
            [FromQuery] string? payment_method = null,
            [FromQuery] string? from_date = null,
            [FromQuery] string? to_date = null,
            [FromQuery] int? country = null,
            [FromQuery] string? city = null,
            [FromQuery] int? page = null,
            [FromQuery] string? sort_by = null)
        {
            var filter = new ListOrdersFilterDto
            {
                Keyword = keyword,
                PaymentMethod = payment_method,
                FromDate = from_date,
                ToDate = to_date,
                Country = country,
                City = city,
                Page = page,
                SortBy = sort_by
            };

            var result = await _orderRepository.GetOrdersAsync(token, filter);
            return Ok(result);
        }

        [HttpGet("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(long orderId, [FromQuery] string token)
        {
            var result = await _orderRepository.GetOrderAsync(orderId, token);
            return Ok(result);
        }

        [HttpPost("OrderActions")]
        public async Task<IActionResult> OrderActions(
            [FromBody] OrderActionsRequestDto inputDto,
            [FromQuery] string token)
        {
            var result = await _orderRepository.ExecuteOrderActionsAsync(inputDto, token);
            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequestDto inputDto,
            [FromQuery] string token)
        {
            var result = await _orderRepository.CreateOrderAsync(inputDto, token);
            return Ok(result);
        }

        [HttpPost("RelocateOrderStock/{orderId}")]
        public async Task<IActionResult> RelocateOrderStock(
            long orderId,
            [FromBody] RelocateOrderStockRequestDto inputDto,
            [FromQuery] string token)
        {
            var result = await _orderRepository.RelocateOrderStockAsync(inputDto, orderId, token);
            return Ok(result);
        }
        #endregion

        #region Brands
        [HttpPost("brand/create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateBrand([FromQuery] string token, [FromForm] CreateBrandDto dto)
        {
            return Ok(await _brandRepository.CreateBrandAsync(dto, token));
        }

        [HttpGet("Getbrands")]
        public async Task<IActionResult> ListBrands([FromQuery] string token, [FromQuery] ListBrandsFilterDto filter)
        {
            return Ok(await _brandRepository.ListBrandsAsync(filter, token));
        }
        [HttpGet("GetbrandDetails")]
        public async Task<IActionResult> GetbrandDetails([FromQuery] string token, [FromQuery] int brandId , string? with)
        {
            return Ok(await _brandRepository.GetBrandDetailsAsync(brandId,with, token));
        }
        [HttpPost("brand/update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBrand([FromQuery] string token,int brandId, [FromForm] UpdateBrandDto dto)
        {
            return Ok(await _brandRepository.UpdateBrandAsync(dto,brandId, token));
        }
        [HttpGet("DeleteBrand")]
        public async Task<IActionResult> DeleteBrand([FromQuery] int brandId, string token )
        {
            return Ok(await _brandRepository.DeleteBrandAsync(brandId, token));
        }
        #endregion

        #region Customers
        [HttpPost("customers")]
        public async Task<IActionResult> CreateCustomer(string token, CreateCustomerDTO inputDto)
        {
            return Ok(await _customerRepository.CreateCustomerAsync(inputDto, token));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomersAsync(string token)
        {
            return Ok(await _customerRepository.GetCustomerListAsync(token));
        }

        [HttpGet("customersById")]
        public async Task<IActionResult> GetCustomerByID(string token, int id)
        {
            return Ok(await _customerRepository.GetCustomerByIdAsync(token, id));
        }

        [HttpPut("Update/customers")]
        public async Task<IActionResult> UpdateCustomer(string token, int id, CreateCustomerDTO inputDto)
        {
            return Ok(await _customerRepository.UpdateCustomerAsync(token, id, inputDto));
        }
        #endregion
    }
}
