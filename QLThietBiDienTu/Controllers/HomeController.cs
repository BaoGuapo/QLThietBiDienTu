using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Driver;
using StackExchange.Redis;
using QLThietBiDienTu.Models;

namespace QLThietBiDienTu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // 1. Kiểm tra cấu hình MongoDB
            var mongoSetting = ConfigurationManager.ConnectionStrings["MongoDb"];
            if (mongoSetting == null)
            {
                return Content("LỖI: Không tìm thấy connectionString có tên 'MongoDb' trong file Web.config ở thư mục gốc!");
            }

            var mongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
            if (string.IsNullOrEmpty(mongoDbName))
            {
                return Content("LỖI: Không tìm thấy cấu hình 'MongoDbName' trong thẻ appSettings của Web.config!");
            }

            // 2. Kết nối MongoDB
            var mongoClient = new MongoClient(mongoSetting.ConnectionString);
            var db = mongoClient.GetDatabase(mongoDbName);
            var collection = db.GetCollection<Product>("products");
            var productList = collection.Find(_ => true).ToList();

            // 3. Kết nối Redis (Có thể thử / catch nếu Redis chưa chạy)
            try
            {
                var redisSetting = ConfigurationManager.ConnectionStrings["Redis"];
                if (redisSetting != null)
                {
                    var redis = ConnectionMultiplexer.Connect(redisSetting.ConnectionString);
                    var redisDb = redis.GetDatabase();
                    var cartData = redisDb.HashGetAll("cart:1");
                    ViewBag.CartCount = cartData.Length;
                }
                else
                {
                    ViewBag.CartCount = 0;
                }
            }
            catch (Exception)
            {
                ViewBag.CartCount = -1; // Đánh dấu lỗi không kết nối được Redis
            }

            return View(productList);
        }
    }
}