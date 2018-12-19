using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductsController : ApiBaseController
    {
        public readonly IProductsService _service;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;
        public ProductsController(IProductsService service, IMapper mapper, IHostingEnvironment environment) : base()
        {
            this._service = service;
            this._mapper = mapper;
            this._environment = environment;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromForm]ProductsAddInput model)
        {
            var fileName = "";
            if (model?.Image != null)
            {
                var formFile = model?.Image;
                fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                string filePath = _environment.WebRootPath + $@"\Files\Pictures\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileName.Split('.')[1]);
                string fileFullName = filePath + fileName;
                using (FileStream fs = new FileStream(fileFullName, FileMode.Create))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                }
            }
            string imgpath = $"/src/Pictures/{fileName}";
            var adminmodel = _mapper.Map<Products>(model);
            adminmodel.ImageUrl = imgpath;

            var optresult = await _service.AddAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(PaginatedBaseInput model)
         {
           return await Task.Run<IActionResult>(() =>
            {
                int total;
                var optresult = _service.GetPager(model.PageIndex, model.PageSize, out total);
                var resultOut = new PaginatedBaseOutput<Products>(model.PageIndex, model.PageSize, total, optresult);
                var result = ApiResultBase.GetInstance(ResultCode.Access, result: resultOut);
                return Ok(result);
            });

        }
    }
}