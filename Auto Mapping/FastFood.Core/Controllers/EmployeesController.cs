namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Http Get
        public async Task<IActionResult> Register()
        {
            var position = await _context.Positions
                .ProjectTo<RegisterEmployeeViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(position);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Error", "Home");
            }

            var employee = _mapper.Map<Employee>(model);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            //Option 1 - No Automapper
            //var employees = await _context.Employees
            //     .Select(e => new EmployeesAllViewModel
            //     {
            //         name = e.Name,
            //         age = e.Age,
            //         address = e.Address,
            //         position = e.Position.Name
            //     })
            //     .tolistasync();

            var employees = await _context.Employees
                .ProjectTo<EmployeesAllViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(employees);
        }
    }
}
