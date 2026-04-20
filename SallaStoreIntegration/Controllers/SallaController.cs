using Microsoft.AspNetCore.Mvc;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Dtos.Customers;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Dtos.Product;
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
        #endregion

        #region Brands
        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand(string token, CreateBrandDto inputDto)
        {
            return Ok(await _brandRepository.CreateBrandAsync(inputDto, token));
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
