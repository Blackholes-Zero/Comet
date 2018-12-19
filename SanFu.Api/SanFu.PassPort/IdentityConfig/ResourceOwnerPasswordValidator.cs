using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using SanFu.Entities;
using SanFu.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SanFu.PassPort.IdentityConfig
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IAccountService _adminUsers;

        public ResourceOwnerPasswordValidator(IAccountService adminUsers)
        {
            this._adminUsers = adminUsers;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            if (!string.IsNullOrWhiteSpace(context.UserName) && !string.IsNullOrWhiteSpace(context.Password))
            {
                AdminInfo user = await _adminUsers.LoginAsync(context.UserName, context.Password);
                if (user != null)
                {
                    context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] { new Claim(JwtClaimTypes.Role, "admin") },
                    authTime: DateTime.Now.AddDays(1)
                    );
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid custom credential");
                }
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims()
        {
            return new Claim[]
            {
            new Claim("UserId", "appid"),
            new Claim(JwtClaimTypes.Name,"appName"),
            new Claim(JwtClaimTypes.GivenName, "appGname"),
            new Claim(JwtClaimTypes.FamilyName, "appFname"),
            new Claim(JwtClaimTypes.Email, "dreamtoem@qq.com"),
            new Claim(JwtClaimTypes.Role,"admin")
            };
        }
    }
}