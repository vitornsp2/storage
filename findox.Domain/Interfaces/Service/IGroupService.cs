using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using findox.Domain.Interfaces.DAL;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IGroupService
    {
        Task<IGroupServiceResponse> Create(IGroupServiceRequest request);
        Task<IGroupServiceResponse> ReadAll(IGroupServiceRequest request);
        Task<IGroupServiceResponse> ReadById(IGroupServiceRequest request);
        Task<IGroupServiceResponse> Update(IGroupServiceRequest request);
        Task<IGroupServiceResponse> DeleteById(IGroupServiceRequest request);
    }
}