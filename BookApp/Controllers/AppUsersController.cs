﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;

        public AppUsersController(IUnitOfWork unitOfWork, IMapper mapper, IAppUserService appUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appUserService = appUserService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.ApplicationUsers.GetAll();
            var userDtos = _mapper.Map<IEnumerable<AppUserDTO>>(users);
            return View(userDtos);
        }

        public async Task<IActionResult> ToggleAdminAccepted(string id)
        {
            var user = await _appUserService.ToggleAdminAccepted(id);
            if (user == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ToggleDelete(string id)
        {
            var user = await _appUserService.ToggleDelete(id);
            if (user == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
