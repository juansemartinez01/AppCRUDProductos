using Microsoft.AspNetCore.Mvc;
using AplicacionCRUDAddAccion.Data;
using AplicacionCRUDAddAccion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Data;
using ClosedXML.Excel;

namespace AplicacionCRUDAddAccion.Controllers
{
    public class MantainanceController : Controller
    {
        
        ProductData _ProductData = new ProductData();
        public IActionResult Listing()
        {
            //La vista muestra la lista de todos los productos
            var oLista = _ProductData.Listing();
            return View(oLista);
        }

        public IActionResult Save()
        {  
            //Metodo para mostrar el formulario de guardado
            return View();
        }

        [HttpPost]
        public IActionResult Save(ProductModel oProduct)
        {
            //Recibe el producto para guardarlo en la base de datos, si lo agrega correctamente muestra la vista del listado, sino rebota
            //Validacion de campos
            if(!ModelState.IsValid) { return View(); }
            var answer = _ProductData.SaveNewProduct(oProduct);
            if(answer)
                return RedirectToAction("Listing");
            else
                return View();
            
        }

        public IActionResult Edit(string IdProducto)
        {
  
            //Metodo para mostrar el formulario de editado
            var oProduct = _ProductData.ObtainProduct(IdProducto);
            return View(oProduct);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel oProduct)
        {
            //Recibe el producto para editarlo en la base de datos, si lo agrega correctamente muestra la vista del listado, sino rebota

            //Validacion de campos
            if (!ModelState.IsValid) { return View(); }
            var answer = _ProductData.EditProduct(oProduct);
            if (answer)
                return RedirectToAction("Listing");
            else
                return View();

        }
        public IActionResult Delete(string IdProducto)
        {
            //Metodo para mostrar el form de editado
            var oProduct = _ProductData.ObtainProduct(IdProducto);
            return View(oProduct);
        }
        [HttpPost]
        public IActionResult Delete(ProductModel oProduct)
        {
            //Recibe el producto para eliminarlo en la base de datos      
            var answer = _ProductData.DeleteProduct(oProduct.ProductId);
            if (answer)
                return RedirectToAction("Listing");
            else
                return View();
        }

        //Metdo para generar el archivo excel. 
        [HttpGet]
        public async Task<FileResult> ExportarProductosAExcel()
        {
            var oList = _ProductData.Listing();
            var nombreArchivo = $"Productos.xslx";
            return generarExcel(nombreArchivo, oList);
        }
        
        private FileResult generarExcel(string nombreArchivo, IEnumerable<ProductModel> oLista)
        {
            DataTable dataTable = new DataTable("Productos");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("IdProducto"),
                new DataColumn("Descripcion"),
                new DataColumn("Categoria"),
                new DataColumn("Stock"),
                new DataColumn("Precio unitario"),
                new DataColumn("Descuento Web"),
                new DataColumn("Activo")
            }) ;
            foreach(var Product in oLista)
            {
                dataTable.Rows.Add(Product.ProductId,
                    Product.ProductDescription,
                    Product.CategoryString,
                    Product.Stock,
                    Product.Price,
                    Product.HaveECDiscount,
                    Product.IsActive);
            }

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using(MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",nombreArchivo);
                }
            }
        }
        
    }
}
