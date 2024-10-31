using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_APP_BTL.Models;
using System.Linq;

namespace Web_APP_BTL.Controllers
{
    public class CustomerController : Controller
    {
        private readonly QlBanHangKhoHangContext db;

        public CustomerController(QlBanHangKhoHangContext context)
        {
            db = context;
        }

        // Hiển thị danh sách khách hàng với tìm kiếm và phân trang
        public IActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            // Lấy toàn bộ danh sách khách hàng từ cơ sở dữ liệu
            var customers = from c in db.Customers select c;

            // Tìm kiếm theo tên hoặc số điện thoại
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.CustomerName.Contains(searchString) || c.ContactInfo.Contains(searchString));
            }

            // Phân trang
            var totalCustomers = customers.Count();
            var totalPages = (int)Math.Ceiling((double)totalCustomers / pageSize);
            var paginatedCustomers = customers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;

            return View(paginatedCustomers);
        }
        public IActionResult GetCustomer(int id)
        {
            var customer =db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Json(new { customerId = customer.CustomerId, customerName = customer.CustomerName, contactInfo = customer.ContactInfo });
        }


        // Tạo khách hàng mới (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Tạo khách hàng mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Chỉnh sửa khách hàng (GET)
        public IActionResult Edit(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // Chỉnh sửa khách hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(customer);
                    db.SaveChanges();
                    return Ok(); // Trả về 200 khi lưu thành công
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return BadRequest(); // Trả về lỗi nếu dữ liệu không hợp lệ
        }


        // Xóa khách hàng (GET)
        public IActionResult Delete(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer); // Nếu tìm thấy khách hàng, trả về view
        }

        // Xác nhận xóa khách hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(); // Trả về Ok nếu xóa thành công
        }

        // Kiểm tra sự tồn tại của khách hàng
        private bool CustomerExists(int id)
        {
            return db.Customers.Any(e => e.CustomerId == id);
        }
    }
}
