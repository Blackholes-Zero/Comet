using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanFu.Entities;
using SanFu.IService;
using SanFu.ViewModels;
using SanFu.ViewModels.Product;

namespace SanFu.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ProductClassController : ApiBaseController
    {
        public readonly IProductClassService _service;
        private readonly IMapper _mapper;

        public ProductClassController(IProductClassService service, IMapper mapper) : base()
        {
            this._service = service;
            this._mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(ProductClassInput model)
        {
            var adminmodel = _mapper.Map<ProductClass>(model);
            var optresult = await _service.AddAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(long id, ProductClassInput model)
        {
            var adminmodel = _mapper.Map<ProductClass>(model);
            adminmodel.Id = id;
            var optresult = await _service.EditAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="model">baseinput</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(BaseInput model)
        {
            var optresult = await _service.GetAllAsync();
            var result = ApiResultBase.GetInstance(ResultCode.Access, result: optresult);
            return Ok(result);
        }
    }
}