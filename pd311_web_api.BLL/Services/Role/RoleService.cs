﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Role;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(RoleDto dto)
        {
            var entity = _mapper.Map<AppRole>(dto);

            var result = await _roleManager.CreateAsync(entity);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity != null)
            {
                var result = await _roleManager.DeleteAsync(entity);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entities = await _roleManager.Roles.ToListAsync();

            var dtos = _mapper.Map<List<RoleDto>>(entities);

            return dtos.ToList();
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<RoleDto>(entity);

            return dto;
        }

        public async Task<bool> UpdateAsync(RoleDto dto)
        {
            var entity = _mapper.Map<AppRole>(dto);

            var result = await _roleManager.UpdateAsync(entity);

            return result.Succeeded;
        }
    }
}
