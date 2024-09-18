﻿using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorController(IUnitOfWork unitOfWork, IAuthorService authorService)
        {
            _unitOfWork = unitOfWork;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _unitOfWork.Authors.GetAll();
            return View(authors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadAuthorDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorService.UploadAuthor(model);
                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            var authorDTO = new UploadAuthorDTO
            {
                Id = author.Id,
                FullName = author.FullName,
                Bio = author.Bio,
            };

            return View(authorDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UploadAuthorDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _authorService.UpdateAuthor(id, model);
                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            var author = await _authorService.ToggleDelete(id);
            if (author == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _unitOfWork.Authors.Find(e => e.Id == id) != null;
        }
    }
}
