using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNoiThatReal.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageID { get; set; } // Khóa chính

        [Required]
        public int ProductID { get; set; } // Khóa ngoại đến Product

        [Required]
        [StringLength(255)]
        public string ImagePath { get; set; } // Đường dẫn tới hình ảnh

        // Thiết lập mối quan hệ với lớp Product
        public virtual Product Product { get; set; }
    }
}
