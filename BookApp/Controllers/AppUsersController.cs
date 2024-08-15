using AutoMapper;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IBaseRepository;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;


namespace YourNamespace.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppUsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.ApplicationUsers.GetAll();
            var userDtos = _mapper.Map<IEnumerable<AppUserDTO>>(users);
            return View(userDtos);
        }
    }
}
