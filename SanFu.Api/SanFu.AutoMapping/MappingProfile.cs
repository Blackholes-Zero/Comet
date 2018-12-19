using AutoMapper;
using NetCore.Framework;
using NetCore.Framework.Snowflake;
using SanFu.Entities;
using SanFu.ViewModels.Account;
using SanFu.ViewModels.Contact;
using SanFu.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.AutoMapping
{
    public class MappingProfile : Profile, IProfile
    {
        public MappingProfile()
        {
            //此文件中添加所有的实体到实体间的映射
            ///CreateMap<AdminUsers, AdminUsersDto>();
            CreateMap<RegisterInput, AdminInfo>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.LoginName, opt => opt.MapFrom(src => src.LoginName))
                .ForMember(p => p.PassWord, opt => opt.MapFrom(src => src.PassWord))
                .ForMember(p => p.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(p => p.SaltKey, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(p => p.IsDel, opt => opt.MapFrom(src => false))
                .ForMember(p => p.State, opt => opt.MapFrom(src => 1))
                .ForMember(p => p.CreateTime, opt => opt.MapFrom(src => DateTime.Now)).ReverseMap();
            CreateMap<ContactInfoInput, ContactInfo>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(p => p.HotwireTelephone, opt => opt.MapFrom(src => src.HotwireTelephone))
                .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(p => p.WeChat, opt => opt.Ignore())
                .ForMember(p => p.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(p => p.QQ, opt => opt.MapFrom(src => src.QQ))
                .ForMember(p => p.IsEnabled, opt => opt.MapFrom(src => true)).ReverseMap();
            CreateMap<ContactInfo, ContactInfoOutput>()
                .ForMember(p => p.WeChat, opt => opt.MapFrom(src => Convert.ToBase64String(src.WeChat)));
            CreateMap<ProductClassInput, ProductClass>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.ClassID, opt => opt.MapFrom(src => SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.IsEnabled, opt => opt.MapFrom(src => true))
                .ForMember(p => p.ClassName, opt => opt.MapFrom(src => src.ClassName));
            CreateMap<ProductsAddInput, Products>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.ImageUrl, opt => opt.Ignore())
                .ForMember(p => p.Price, opt => opt.MapFrom(src => src.Price ?? 0));
            //Mapper.AssertConfigurationIsValid();
        }
    }
}
