Create DataBase ProductInfoDB5
use ProductInfoDB

--Definicion de la tabla ProductCategory
CREATE TABLE ProductCategory(
CategoryProductId NUMERIC(5) IDENTITY PRIMARY KEY not null,
CategoryDescription VARCHAR(200),
IsActive Char(1) not null);

--Definicion de la tabla Product
CREATE TABLE Product(
ProductId NVARCHAR(30)  PRIMARY KEY not null,
CategoryProductId NUMERIC(5) not null,
ProductDescription NVARCHAR(200),
Stock NUMERIC(5) not null,
Price NUMERIC(5,2) not null,
HaveECDiscount CHAR(1) not null,
IsActive Char(1) not null,
CONSTRAINT fk_CategoryProduct FOREIGN KEY (CategoryProductId) REFERENCES ProductCategory(CategoryProductId));

--STORED PROCEDURES
--Funcionalidad: Devolver una lista de todos los productos con todos los datos de cada producto
CREATE PROCEDURE sp_ProductListing
AS
BEGIN
	SELECT P.ProductId,P.CategoryProductId,P.ProductDescription,P.Stock,P.Price,P.HaveECDiscount,P.IsActive,PC.CategoryDescription AS 'DescriptionStr' FROM Product P JOIN ProductCategory PC ON P.CategoryProductId = PC.CategoryProductId
END

--Funcionalidad: Devolver un producto especifico, identificandolo con su ID
CREATE PROCEDURE sp_GetProduct(@IdProduct VARCHAR(30))
AS
BEGIN
	SELECT P.ProductId,P.CategoryProductId,P.ProductDescription,P.Stock,P.Price,P.HaveECDiscount,P.IsActive,PC.CategoryDescription AS 'DescriptionStr' FROM Product P JOIN ProductCategory PC ON P.CategoryProductId = PC.CategoryProductId WHERE ProductId = @IdProduct
END

--Funcionalidad: Almacenar un nuevo Producto en la tabla Product
CREATE PROCEDURE sp_SaveProduct(
@ProductId VARCHAR(30),
@ProductDescription VARCHAR(200),
@ProductCategory NUMERIC(5),
@Stock NUMERIC(5),
@Price NUMERIC(5,2),
@HaveECDiscount CHAR(1),
@IsActive Char(1))
AS
BEGIN
	INSERT INTO Product(ProductId,ProductDescription,CategoryProductId,Stock,Price,HaveECDiscount,IsActive) VALUES (@ProductId,@ProductDescription,@ProductCategory,@Stock,@Price,@HaveECDiscount,@IsActive)
END

--Funcionalidad: Modificar los datos de un producto de la tabla
CREATE PROCEDURE sp_EditProduct(
@ProductId VARCHAR(30),
@ProductDescription VARCHAR(200),
@ProductCategory NUMERIC(5),
@Stock NUMERIC(5),
@Price NUMERIC(5,2),
@HaveECDiscount CHAR(1),
@IsActive Char(1))
AS
BEGIN
	UPDATE Product SET CategoryProductId = @ProductCategory, ProductDescription = @ProductDescription, Stock = @Stock, Price = @Price, HaveECDiscount = @HaveECDiscount, IsActive = @IsActive WHERE ProductId = @ProductId
END

--Funcionalidad: Eliminar un producto de la tabla Product
CREATE PROCEDURE sp_DeleteProduct(@IdProduct VARCHAR(30))
AS
BEGIN
	DELETE FROM Product WHERE ProductId = @IdProduct
END

--Listar todas las categorias que existen activas
CREATE PROCEDURE sp_CategoryListing
AS
BEGIN
	SELECT CategoryDescription FROM ProductCategory WHERE IsActive = 1
END

--Insercion de datos de ejemplo
INSERT INTO ProductCategory VALUES ('Limpieza',1)
INSERT INTO ProductCategory VALUES ('Alimento',1)
INSERT INTO ProductCategory VALUES ('Decoracion',1)

INSERT INTO Product VALUES ('COD1',1,'Producto ejemplo',23,20,0,1)
