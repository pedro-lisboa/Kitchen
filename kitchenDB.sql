Create database kitchen;

CREATE LOGIN usr_kitchen   
    WITH PASSWORD = 'mudar@123'; 
	
CREATE USER usr_kitchen   
    FOR LOGIN usr_kitchen; 
	
USE HR;
GRANT SELECT, INSERT, UPDATE, DELETE ON EmployeeAddress TO Steve;
	
Create table orders (
		  Id int NOT NULL IDENTITY(1,1),  
		  TableNum int DEFAULT NULL,
		  Active int DEFAULT 1,
		  CreatedAt DateTime NULL DEFAULT CURRENT_TIMESTAMP,
		  PRIMARY KEY (Id)
		);

Create table products (
		  Id int NOT NULL IDENTITY(1,1),  
		  Name varchar(100) DEFAULT NULL,
		  Area varchar(10) DEFAULT NULL,
		  Price decimal DEFAULT null,
		  PRIMARY KEY (Id)
		);

Create table orderproducts (
		  Id int NOT NULL IDENTITY(1,1),  
		  OrderId int NOT NULL,
		  ProductId int NOT NULL,
		  Amount int DEFAULT 1,
		  Note varchar(200) DEFAULT NULL,
		  PRIMARY KEY (Id)
		);
		
		

GRANT SELECT, INSERT, UPDATE, DELETE ON orders TO usr_kitchen;
GRANT SELECT, INSERT, UPDATE, DELETE ON products TO usr_kitchen;
GRANT SELECT, INSERT, UPDATE, DELETE ON orderproducts TO usr_kitchen;

