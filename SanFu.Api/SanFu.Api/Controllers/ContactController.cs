using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using SanFu.Entities;
using SanFu.IService;
using SanFu.ViewModels;
using SanFu.ViewModels.Contact;

namespace SanFu.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ContactController :ApiBaseController
    {
        public readonly IContactInfoService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactInfoService contactService, IMapper mapper) : base()
        { 
            this._contactService = contactService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 添加联系方式
        /// </summary>
        /// <param name="model">联系方式</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromForm]ContactInfoInput model)
        {
            var adminmodel = _mapper.Map<ContactInfo>(model);
            IFormFile formFile = model?.WeChat;
            if (formFile != null)
            {
                var fs = formFile.OpenReadStream();
                using (BinaryReader br = new BinaryReader(fs))
                {
                    var imgBytesIn = br.ReadBytes((int)fs.Length);
                    adminmodel.WeChat = imgBytesIn;
                }
            }
            var optresult = await _contactService.AddAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }

        /// <summary>
        /// 添加联系方式
        /// </summary>
        /// <param name="model">联系方式</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(ContactInfoByIdInput model)
        {
            var optresult = await _contactService.GetById(model.Id);
            var outresult = _mapper.Map<ContactInfoOutput>(optresult);
            //outresult.WeChat = Convert.ToBase64String(optresult.WeChat);
            var result = ApiResultBase.GetInstance(ResultCode.Access, result: outresult);
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
        public async Task<IActionResult> Edit(long id,[FromForm]ContactInfoInput model)
        {
            var adminmodel = _mapper.Map<ContactInfo>(model);
            IFormFile formFile = model?.WeChat;
            if (formFile != null)
            {
                var fs = formFile.OpenReadStream();
                using (BinaryReader br = new BinaryReader(fs))
                {
                    var imgBytesIn = br.ReadBytes((int)fs.Length);
                    adminmodel.WeChat = imgBytesIn;
                }
            }
            adminmodel.Id = id;
            var optresult = await _contactService.EditAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }

        
    }
}