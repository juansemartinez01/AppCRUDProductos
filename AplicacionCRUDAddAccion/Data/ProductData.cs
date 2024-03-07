using AplicacionCRUDAddAccion.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;


namespace AplicacionCRUDAddAccion.Data
{
    public class ProductData
    {
        //Uso de todos los stored procedures que tenemos en la base de datos
        //Metodo para listar todos los productos cargados en la base de datos, devuelve una lista de objetos del tipo ProductModel
        public List<ProductModel> Listing()
        {
            var oLista = new List<ProductModel>();

            var con = new Connection();

            //Abrimos la conexion y llamamos el stored procedure sp_ProductListing
            using(var connection = new SqlConnection(con.getCadenaSQL()))
            {
                connection.Open();


                SqlCommand cmd = new SqlCommand("sp_ProductListing", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                

                using(var reader = cmd.ExecuteReader())
                {
                    bool descuento;
                    bool activo;
                    while(reader.Read())
                    {
                        //Hacemos la conversion de int a boolean para los valores de descuento y activo
                        if (int.Parse(reader["HaveECDiscount"].ToString()) == 0)
                        {
                            descuento = false;
                        }
                        else
                        {
                            descuento = true;
                        }

                        if (int.Parse(reader["IsActive"].ToString()) == 0)
                        {
                            activo = false;
                        }
                        else
                        {
                            activo = true;
                        }
                        oLista.Add(new ProductModel()
                        {
                            ProductId = reader["ProductId"].ToString(),
                            CategoryProductId = Convert.ToInt32(reader["CategoryProductId"]),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Stock = Convert.ToInt32(reader["Stock"]),
                            Price = Convert.ToDouble(reader["Price"]),
                            HaveECDiscount = descuento,
                            IsActive = activo,
                            CategoryString = reader["DescriptionStr"].ToString()


                        }) ;
                    }
                }
            }

            return oLista;
        }

        //Metodo para obtener un producto especifico, devuelve un objeto del tipo ProductModel
        public ProductModel ObtainProduct(string ProductId)
        {
            var oProduct = new ProductModel();

            var con = new Connection();

            //Abrimos la conexion y llamamos el stored procedure sp_GetProduct
            using (var connection = new SqlConnection(con.getCadenaSQL()))
            {
                connection.Open();


                SqlCommand cmd = new SqlCommand("sp_GetProduct", connection);
                cmd.Parameters.AddWithValue("IdProduct", ProductId);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        oProduct.ProductId = reader["ProductId"].ToString();
                        oProduct.CategoryProductId = Convert.ToInt32(reader["CategoryProductId"]);
                         oProduct.ProductDescription = reader["ProductDescription"].ToString();
                        oProduct.Stock = Convert.ToInt32(reader["Stock"]);
                        oProduct.Price = Convert.ToDouble(reader["Price"]);

                        //Hacemos la conversion de int a boolean para los siguientes valores
                        if(int.Parse(reader["HaveECDiscount"].ToString()) == 0)
                        {
                            oProduct.HaveECDiscount = false;
                        }
                        else
                        {
                            oProduct.HaveECDiscount = true;
                        }

                        if (int.Parse(reader["IsActive"].ToString()) == 0)
                        {
                            oProduct.IsActive = false;
                        }
                        else
                        {
                            oProduct.IsActive = true;
                        }                       
                    }
                }
            }

            return oProduct;
        }

        //Metodo para guardar un nuevo producto, devuelve true si se ejecuta el stored procedure y false si no lo hace
        public bool SaveNewProduct(ProductModel oProduct)
        {
            bool ans;
            if(oProduct.Stock is null)
            {
                oProduct.Stock = 0;
            }
            try
            {
                var con = new Connection();

                //Abrimos la conexion y llamamos el stored procedure sp_SaveProduct
                using (var connection = new SqlConnection(con.getCadenaSQL()))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("sp_SaveProduct", connection);
                    cmd.Parameters.AddWithValue("ProductId", oProduct.ProductId);
                    cmd.Parameters.AddWithValue("ProductCategory", oProduct.CategoryProductId);
                    cmd.Parameters.AddWithValue("ProductDescription", oProduct.ProductDescription);
                    cmd.Parameters.AddWithValue("Stock", oProduct.Stock);
                    cmd.Parameters.AddWithValue("Price", oProduct.Price);
                    cmd.Parameters.AddWithValue("HaveECDiscount", oProduct.HaveECDiscount);
                    cmd.Parameters.AddWithValue("IsActive", oProduct.IsActive);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                ans = true;
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                ans = false;
            }

            return ans;
        }

        //Metodo para editar un producto, devuelve true si se ejecuta el stored procedure y false si no lo hace
        public bool EditProduct(ProductModel oProduct)
        {
            bool ans;
            try
            {
                var con = new Connection();
                //Abrimos la conexion y llamamos el stored procedure sp_EditProduct
                using (var connection = new SqlConnection(con.getCadenaSQL()))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("sp_EditProduct", connection);
                    cmd.Parameters.AddWithValue("ProductId", oProduct.ProductId);
                    cmd.Parameters.AddWithValue("ProductCategory", oProduct.CategoryProductId);
                    cmd.Parameters.AddWithValue("ProductDescription", oProduct.ProductDescription);
                    cmd.Parameters.AddWithValue("Stock", oProduct.Stock);
                    cmd.Parameters.AddWithValue("Price", oProduct.Price);
                    cmd.Parameters.AddWithValue("HaveECDiscount", oProduct.HaveECDiscount);
                    cmd.Parameters.AddWithValue("IsActive", oProduct.IsActive);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                ans = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ans = false;
            }

            return ans;
        }

        //Metodo para eliminar un producto, devuelve true si se ejecuta el stored procedure y false si no lo hace
        public bool DeleteProduct(string ProductId)
        {
            bool ans;
            try
            {
                var con = new Connection();

                //Abrimos la conexion y llamamos el stored procedure sp_DeleteProduct
                using (var connection = new SqlConnection(con.getCadenaSQL()))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("sp_DeleteProduct", connection);
                    cmd.Parameters.AddWithValue("IdProduct", ProductId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                ans = true;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ans = false;
            }
            return ans;
        }
    }
}
