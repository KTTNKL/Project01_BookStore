USE [BookStore]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 4/3/2022 11:14:44 AM ******/
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
/****** Object:  Table [dbo].[Category]    Script Date: 4/3/2022 11:14:44 AM ******/
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
SET IDENTITY_INSERT [dbo].[Book] ON 

INSERT [dbo].[Book] ([book_id], [book_name], [book_author], [book_year], [book_cover], [book_buying_price], [book_selling_price], [book_stock], [book_sold], [book_category]) VALUES (1, N'One piece', N'Oda', 1999, N'Images/ava01.jpg', 10000, 20000, 100, 100, 1)
INSERT [dbo].[Book] ([book_id], [book_name], [book_author], [book_year], [book_cover], [book_buying_price], [book_selling_price], [book_stock], [book_sold], [book_category]) VALUES (2, N'Nha ma', N'ghost', 2002, N'Images/ava02.jpg', 20000, 40000, 100, 100, 2)
SET IDENTITY_INSERT [dbo].[Book] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (1, N'Truyện Tranh')
INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (2, N'Kinh dị')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_Book_Category] FOREIGN KEY([book_category])
REFERENCES [dbo].[Category] ([category_id])
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_Book_Category]
GO
