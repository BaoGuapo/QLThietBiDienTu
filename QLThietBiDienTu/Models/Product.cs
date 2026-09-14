using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QLThietBiDienTu.Models
{
    // THÊM DÒNG NÀY ĐỂ BỎ QUA CÁC TRƯỜNG DƯ THỪA TRONG MONGODB
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("tenSP")]
        public string TenSP { get; set; }

        [BsonElement("giaBan")]
        public decimal GiaBan { get; set; }

        // Bổ sung luôn cột namSX để hiển thị nếu bạn thích
        [BsonElement("namSX")]
        public int NamSX { get; set; }
    }
}