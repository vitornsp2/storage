using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IGroupService
    {
        Task<ApiReponse> Create(GroupDto groupDto);
        Task<ApiReponse> ReadAll();
        Task<ApiReponse> ReadById(long? id);
        Task<ApiReponse> Update(GroupDto groupDto);
        Task<ApiReponse> DeleteById(long? id);
    }
}