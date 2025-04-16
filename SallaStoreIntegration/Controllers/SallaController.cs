using ExternalStores.DTO.Salla;
using Microsoft.AspNetCore.Mvc;
using SallaStoreIntegration.BLL;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Customers;

namespace DexefExternalStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SallaController : ControllerBase
    {
        ISallaBLL _sallaBLL;

        public SallaController(ISallaBLL sallaBLL)
        {
            _sallaBLL = sallaBLL;
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var result = await _sallaBLL.AuthorizeAsync();

            return Redirect(result);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            var result = await _sallaBLL.CallbackAsync(code, state);

            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _sallaBLL.RefreshTokenAsync(refreshToken);

            return Ok(result);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories(string token)
        {
            var result = await _sallaBLL.GetCategoriesAsync(token);

            return Ok(result);
        }

        [HttpPost("Create/Category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto inputDto, string token)
        {
            var result = await _sallaBLL.CreateCategoryAsync(inputDto, token);

            return Ok(result);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> UpdateCategory(string token, UpdateCategoryInputDto inputDto ,int id)
        {
            return Ok(await _sallaBLL.UpdateCategoriesAsync(token, id, inputDto));
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts([FromQuery] FilterProductDto filterDto)
        {
            var result = await _sallaBLL.GetProductsAsync(filterDto);

            return Ok(result);
        }

        [HttpPost("Create/Product")]
        public async Task<IActionResult> CreateProduct(CreateProductDto inputDto, string token)
        {
            var result = await _sallaBLL.CreateProductAsync(inputDto, token);

            return Ok(result);
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(string token)
        {
            var result = await _sallaBLL.GetOrdersAsync(token);

            return Ok(result);
        }
        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrderById(string token, int id)
        {
            var result = await _sallaBLL.GetOrderByIdAsync(token, id);

            return Ok(result);
        }

        [HttpPost("UpdateOrder/{id}/Status")]
        public async Task<IActionResult> UpdateOrder(string token, int id, UpdateOrderStatusInputDto inputDto)
        {
            var result = await _sallaBLL.UpdateOrderStatusAsync(token, id, inputDto);

            return Ok(result);
        }

        [HttpGet("orders/status")]
        public async Task<IActionResult> GetOrderStatus(string token)
        {
            var result = await _sallaBLL.GetOrderStatusAsync(token);

            return Ok(result);
        }

        #region Brands
        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand(string token, CreateBrandDto inputDto)
        {
            return Ok(await _sallaBLL.CreateBrandAsync(inputDto, token));
        }
        #endregion

        #region customers
        [HttpPost("customers")]
        public async Task<IActionResult> CreateCustomer(string token, CreateCustomerDTO inputDto)
        {
            return Ok(await _sallaBLL.CreateCustomerAsync(inputDto, token));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomersAsync(string token)
        {
            return Ok(await _sallaBLL.GetCustomerListAsync(token));
        }

        [HttpGet("customersById")]
        public async Task<IActionResult> GetCustomerByID(string token , int id)
        {
            return Ok(await _sallaBLL.GetCustomerByIdAsync(token, id));
        }

        [HttpPut("Update/customers")]
        public async Task<IActionResult> UpdateCustomer(string token , int id , CreateCustomerDTO inputDto)
        {
            return Ok(await _sallaBLL.UpdateCustomerAsync(token, id, inputDto));
        }
        #endregion
    }
}
