using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using PagedList;

namespace WebNoiThatReal
{
    public class ProductController : Controller
    {
        WebNoiThatRealEntities db = new WebNoiThatRealEntities();
        // GET: Product

        public ActionResult Index(string Category, int? page)
        {

            if (page == null) page = 1;

            if (Category == null)
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro).ToPagedList(page.Value, 8);
                return View(productList);
            }
            else
            {

                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.IDCate == Category).ToPagedList(page.Value, 8);
                return View(productList);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.listCategory = new SelectList(list, "IDCate", "NameCate", "");
            Product pro = new Product();
            return View(pro);
        }
        public ActionResult SelectCate()
        {
            Category se_cate = new Category();
            se_cate.ListCate = db.Categories.ToList<Category>();
            return PartialView(se_cate);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product pro, IEnumerable<HttpPostedFileBase> UploadImages)
        {
            List<Category> list = db.Categories.ToList();
            try
            {
                // Xử lý từng hình ảnh được tải lên
                if (UploadImages != null && UploadImages.Any())
                {
                    foreach (var file in UploadImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Lưu ảnh vào thư mục
                            string filename = Path.GetFileNameWithoutExtension(file.FileName);
                            string extent = Path.GetExtension(file.FileName);
                            filename = filename + extent;
                            string path = "~/Content/Images/" + filename;
                            file.SaveAs(Server.MapPath(path));

                            // Tạo đối tượng ProductImage và thêm vào cơ sở dữ liệu
                            ProductImage img = new ProductImage
                            {
                                ProductID = pro.ProductID,
                                ImagePath = path
                            };
                            pro.ProductImages.Add(img);
                        }
                    }
                }

                ViewBag.listCategory = new SelectList(list, "IDCate", "NameCate", 1);
                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Danhsachsp");
            }
            catch
            {
                return View(pro);
            }
        }


        public ActionResult Danhsachsp()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Details(int id)
        {
            return View(db.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pro = db.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(pro);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Product pro)
        {
            var existingProduct = db.Products.FirstOrDefault(p => p.ProductID == id);
            if (existingProduct != null)
            {
                existingProduct.NamePro = pro.NamePro;
                existingProduct.DecriptionPro = pro.DecriptionPro;
                existingProduct.IDCate = pro.IDCate;
                existingProduct.Price = pro.Price;

                if (pro.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pro.UploadImage.FileName);
                    string extension = Path.GetExtension(pro.UploadImage.FileName);
                    filename = filename + extension;
                    existingProduct.ImagePro = "~/Content/Images/" + filename;
                    pro.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), filename));
                }

                db.Entry(existingProduct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Danhsachsp");
        }

        public ActionResult Delete(int id)
        {
            return View(db.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product pro)
        {
            pro = db.Products.Where(s => s.ProductID == id).FirstOrDefault();
            db.Products.Remove(pro);
            db.SaveChanges();
            return RedirectToAction("Danhsachsp");
        }


        public ActionResult DetailsSP(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {

                return HttpNotFound();
            }

            // Định dạng giá sản phẩm
            product.PriceFormatted = string.Format("{0:N0}", product.Price);
            return View(product);
        }



    }
}
