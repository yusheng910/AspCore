using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace prjToDoList.Controllers
{
    public class DemoController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        public DemoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult SessionDemo()
        {
            int count = 0;
            bool countIsInt = false;

            if (HttpContext.Session.Keys.Contains("Count"))
            {
                string? countValueStr = HttpContext.Session.GetString("Count");
                countIsInt = int.TryParse(countValueStr, out count);
            }
            else {
                count++;
                HttpContext.Session.SetString("Count", count.ToString());
            }

            if (countIsInt)
            {
                count++;
                HttpContext.Session.SetString("Count", count.ToString());
            }
            ViewBag.Count = count;
            return View();
        }

        public IActionResult CookieDemo()
        {
            return View();
        }

        public IActionResult UploadDemo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadDemo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            // 取得檔案的名稱並使用 WebRootPath 取得 wwwroot 下的上傳檔案的路徑
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath + @"\assets", fileName);

            // 檢查資料夾是否存在，如果不存在，則建立資料夾
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 使用完 FileStream 物件後須 Dispose 或 Close 以釋放資源
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Dispose();
            return View();
        }
    }
}
