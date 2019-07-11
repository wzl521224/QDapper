USE [ELF_Community]
GO

/****** Object:  Table [dbo].[goods]    Script Date: 2019/7/1 19:34:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[goods](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[goodsname] [nvarchar](200) NULL,
	[pid] [bigint] NULL,
	[remark] [text] NULL,
	[price] [numeric](18, 2) NULL,
	[image] [bigint] NULL,
 CONSTRAINT [PK_GOODS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[goods]  WITH CHECK ADD  CONSTRAINT [FK_GOODS_REFERENCE_GOODS_TY] FOREIGN KEY([pid])
REFERENCES [dbo].[goods_type] ([id])
GO

ALTER TABLE [dbo].[goods] CHECK CONSTRAINT [FK_GOODS_REFERENCE_GOODS_TY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goodsname'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'pid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'price'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'image'
GO

