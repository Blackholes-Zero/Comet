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
using SanFu.ViewModels.Account;

namespace SanFu.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : ApiBaseController
    {
        public readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper) : base()
        {
            this._accountService = accountService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">注册模板</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterInput model)
        {
            var adminmodel = _mapper.Map<AdminInfo>(model);
            var optresult = await _accountService.AddAsync(adminmodel);
            var result = ApiResultBase.GetInstance(optresult ? ResultCode.Access : ResultCode.Fail, result: optresult);
            return Ok(result);
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">注册模板</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInput model)
        {
            object obj = null;

            if (string.IsNullOrWhiteSpace(obj?.ToString()))
            {

            }

            var optresult = await _accountService.LoginAsync(model.LoginName,model.PassWord);
            var result = ApiResultBase.GetInstance(ResultCode.Access, result: optresult);
            return Ok(result);
        }
    }
}