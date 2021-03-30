using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//namespace EmployeeManagement.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly IEmployeeRepository _employeeRepository;

//        
//        public HomeController(IEmployeeRepository employeeRepository)
//        {
//            _employeeRepository = employeeRepository;
//        }



//        public ViewResult Details()
//        {
//            Employee model = _employeeRepository.GetEmployee(1);
//            return View(model);
//        }
//    }
//}



public class HomeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IWebHostEnvironment webHostingEnvironment;

    //using this constructor we are going to inject interface IEmployeeRepository
    //        //Constructor injection
    public HomeController(IEmployeeRepository employeeRepository,
                            IWebHostEnvironment webHostingEnvironment)
    {
        _employeeRepository = employeeRepository;
        this.webHostingEnvironment = webHostingEnvironment;
    }


    public ViewResult Index()
    {
        var model = _employeeRepository.GetAllEmployee();
        return View(model);
    }
    public ViewResult Details(int id)
    {
        HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
        {
            Employee = _employeeRepository.GetEmployee(id),
            PageTitle = "Employee Details"
        };
        return View(homeDetailsViewModel);
       
    }
    [HttpPost]
    public IActionResult Create(EmployeeCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName = null;
            if(model.Photo != null)
            {
               string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            Employee newEmployee = new Employee
            {
                Name = model.Name,
                Email = model.Email,
                Department = model.Department,
                PhotoPath = uniqueFileName
            };
            _employeeRepository.Add(newEmployee);
            return RedirectToAction("Details", new { id = newEmployee.Id });
        }

        else
            return View();
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public ViewResult Edit(int id)
    {
        Employee employee = _employeeRepository.GetEmployee(id);
        EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Department = employee.Department,
            ExistingPhotoPath = employee.PhotoPath
        };
        return View(employeeEditViewModel);
    }
}






//return View("test"); //using overloaded method of Views to use Test.cshtml in place of Details.cshtml (conventionally it will look for Detail.cshtml 
//or 
//return View("~/MyViews/Test.cshtml");  //Absolute file path.
//return Views(../Test/Update) //Relative file path
//return View("../../MyViews/ Test");  //Relative file path going 2 level up 