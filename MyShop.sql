Create DATABASE BookStore
GO
USE [BookStore]
GO
/****** Object:  User [admin]    Script Date: 4/12/2022 2:57:22 PM ******/
CREATE LOGIN [admin] WITH PASSWORD='1234', DEFAULT_DATABASE=[BookStore]

CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]

GO
/****** Object:  User [sale01]    Script Date: 4/12/2022 2:57:22 PM ******/

ALTER ROLE [db_owner] ADD MEMBER [admin]
GO

/****** Object:  Table [dbo].[Book]    Script Date: 4/12/2022 2:57:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[book_id] [int] IDENTITY(1,1) NOT NULL,
	[book_name] [ntext] NULL,
	[book_author] [ntext] NULL,
	[book_year] [int] NULL,
	[book_cover] [ntext] NULL,
	[book_buying_price] [int] NULL,
	[book_selling_price] [int] NULL,
	[book_stock] [int] NULL,
	[book_sold] [int] NULL,
	[book_category] [int] NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[book_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 4/12/2022 2:57:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [ntext] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchase]    Script Date: 4/12/2022 2:57:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchase](
	[purchase_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_name] [ntext] NULL,
	[customer_tel] [ntext] NULL,
	[customer_address] [ntext] NULL,
	[purchase_final_total] [int] NULL,
	[purchase_created_at] [ntext] NULL,
	[purchase_status] [ntext] NULL,
	[purchase_final_profit] [int] NULL,
	[purchase_final_quantity] [int] NULL,
 CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED 
(
	[purchase_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseDetail]    Script Date: 4/12/2022 2:57:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseDetail](
	[purchasedetail_id] [int] IDENTITY(1,1) NOT NULL,
	[purchase_id] [int] NULL,
	[book_id] [int] NULL,
	[purchasedetail_quantity] [int] NULL,
	[purchasedetail_price] [int] NULL,
	[purchasedetail_total] [int] NULL,
 CONSTRAINT [PK_PurchaseDetail] PRIMARY KEY CLUSTERED 
(
	[purchasedetail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_Book_Category] FOREIGN KEY([book_category])
REFERENCES [dbo].[Category] ([category_id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_Book_Category]
GO
ALTER TABLE [dbo].[PurchaseDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetail_Book] FOREIGN KEY([book_id])
REFERENCES [dbo].[Book] ([book_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[PurchaseDetail] CHECK CONSTRAINT [FK_PurchaseDetail_Book]
GO
ALTER TABLE [dbo].[PurchaseDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetail_Purchase] FOREIGN KEY([purchase_id])
REFERENCES [dbo].[Purchase] ([purchase_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseDetail] CHECK CONSTRAINT [FK_PurchaseDetail_Purchase]
GO
