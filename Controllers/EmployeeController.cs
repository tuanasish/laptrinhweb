using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_APP_BTL.Models;

namespace YourNamespace.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ManageContext db;

        public EmployeeController(ManageContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await db.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();
            return View(employees); // Trả về danh sách nhân viên cho Index
        }
       
        public async Task<IActionResult> Create()
        {
            var departments = await db.Departments.ToListAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            var positions = await db.Positions.ToListAsync();
            ViewBag.Positions = new SelectList(positions, "PositionId", "PositionName");
            return View(new Employee());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Employees.Add(employee);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm nhân viên.");
                }
            }

            // Thiết lập lại ViewBag cho Positions và Departments khi ModelState không hợp lệ
            ViewBag.Positions = new SelectList(await db.Positions.ToListAsync(), "PositionId", "PositionName");
            ViewBag.Departments = new SelectList(await db.Departments.ToListAsync(), "DepartmentId", "DepartmentName");

            return PartialView("Create", employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await db.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "PositionName", employee.PositionId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] EmployeeEditViewModel employeeEdit)
        {
            if (employeeEdit == null || employeeEdit.EmpId <= 0) return BadRequest("Thông tin không hợp lệ.");

            if (ModelState.IsValid)
            {
                var existingEmployee = await db.Employees.FindAsync(employeeEdit.EmpId);
                if (existingEmployee == null) return NotFound();

                // Cập nhật các trường nhân viên
                existingEmployee.EmpName = employeeEdit.EmpName;
                existingEmployee.Email = employeeEdit.Email;
                existingEmployee.Phone = employeeEdit.Phone;

                // Lưu thay đổi vào cơ sở dữ liệu
                await db.SaveChangesAsync();
                return Json(new { message = "Chỉnh sửa nhân viên thành công!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new { message = string.Join(", ", errors) });
        }


        [HttpGet]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = db.Employees
                .FirstOrDefault(e => e.EmpId == id);

            if (employee == null)
            {
                return Json(new { error = "Không tìm thấy nhân viên" });
            }

            return Json(new
            {
                empId = employee.EmpId,
                empName = employee.EmpName,
                empPw = employee.EmpPw,
                email = employee.Email,
                phone = employee.Phone,
                hireDate = employee.HireDate.ToString("yyyy-MM-dd"),
                departmentId = employee.DepartmentId,
                positionId = employee.PositionId
            });
        }

        // GET: Employee/Delete/5
        public IActionResult Delete(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee); // Nếu tìm thấy nhân viên, trả về view
        }

        // Xác nhận xóa nhân viên (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(); // Trả về Ok nếu xóa thành công
        }

        public IActionResult Details(int id)
        {
            var employee = db.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefault(e => e.EmpId == id);

            if (employee == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
                return RedirectToAction(nameof(Index));
            }

            return PartialView("Details", employee);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Any(e => e.EmpId == id);
        }
    }
}