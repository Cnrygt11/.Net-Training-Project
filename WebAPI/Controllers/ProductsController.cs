using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntitFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //Attribute
    public class ProductsController : ControllerBase
    {
        //Loosely coupled
        //naming convention
        //Ioc Container - Inversion of Control
        IProductService _productService;
       
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public List<Product> Get()
        {           
            //Dependency chain
            var result = productService.GetAll();
            return result.Data;
        }
    }
}

