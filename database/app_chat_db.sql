USE [AppChatDB]
GO
/****** Object:  Table [dbo].[ChatGroup]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ChatGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Online] [bit] NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientsChatGroup]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientsChatGroup](
	[IDGroup] [int] NOT NULL,
	[IDClient] [int] NOT NULL,
 CONSTRAINT [PK_ClientsChatGroup] PRIMARY KEY CLUSTERED 
(
	[IDGroup] ASC,
	[IDClient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DetailMessage] [varbinary](max) NOT NULL,
	[ClientSent] [int] NOT NULL,
	[ClientReceiver] [int] NOT NULL,
	[Sent] [bit] NOT NULL,
	[MessageType] [int] NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageType]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageType](
	[ID] [int] NOT NULL,
	[MessageTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChatGroup] ON 

INSERT [dbo].[ChatGroup] ([ID], [GroupName]) VALUES (1, N'ae tao')
INSERT [dbo].[ChatGroup] ([ID], [GroupName]) VALUES (2, N'anh em xa doan')
INSERT [dbo].[ChatGroup] ([ID], [GroupName]) VALUES (3, N'123')
SET IDENTITY_INSERT [dbo].[ChatGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ID], [Name], [Password], [Online]) VALUES (1, N'server', N'1962026656160185351301320480154111117132155', 0)
INSERT [dbo].[Clients] ([ID], [Name], [Password], [Online]) VALUES (2, N'dunlok', N'1962026656160185351301320480154111117132155', 1)
INSERT [dbo].[Clients] ([ID], [Name], [Password], [Online]) VALUES (3, N'hauxun', N'1962026656160185351301320480154111117132155', 1)
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (0, N'PushLog')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (1, N'PushStatus')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (2, N'PushMessage')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (3, N'PushOfflineMessage')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (4, N'PushOfflineGroupMessage')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (100, N'ServerSendAll')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (101, N'ServerToSingleClient')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (102, N'ClientToServer')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (103, N'ClientToClient')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (104, N'OfflineSending')
INSERT [dbo].[MessageType] ([ID], [MessageTypeName]) VALUES (105, N'ChatToGroup')
GO
ALTER TABLE [dbo].[ClientsChatGroup]  WITH CHECK ADD  CONSTRAINT [FK_ClientsChatGroup_ChatGroup] FOREIGN KEY([IDGroup])
REFERENCES [dbo].[ChatGroup] ([ID])
GO
ALTER TABLE [dbo].[ClientsChatGroup] CHECK CONSTRAINT [FK_ClientsChatGroup_ChatGroup]
GO
ALTER TABLE [dbo].[ClientsChatGroup]  WITH CHECK ADD  CONSTRAINT [FK_ClientsChatGroup_Clients] FOREIGN KEY([IDClient])
REFERENCES [dbo].[Clients] ([ID])
GO
ALTER TABLE [dbo].[ClientsChatGroup] CHECK CONSTRAINT [FK_ClientsChatGroup_Clients]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Clients] FOREIGN KEY([ClientSent])
REFERENCES [dbo].[Clients] ([ID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Clients]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Clients1] FOREIGN KEY([ClientReceiver])
REFERENCES [dbo].[Clients] ([ID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Clients1]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_MessageType] FOREIGN KEY([MessageType])
REFERENCES [dbo].[MessageType] ([ID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_MessageType]
GO
/****** Object:  StoredProcedure [dbo].[USP_AddGroupChat]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_AddGroupChat]
@GroupName AS NVARCHAR(100)
AS
BEGIN
	INSERT dbo.ChatGroup (GroupName) VALUES (@GroupName)
	SELECT SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SaveMessage]    Script Date: 11/18/2022 9:44:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_SaveMessage]
@MessageDetail AS VARBINARY(MAX),
@ClientSent AS INT = NULL,
@ClientReceiver AS INT,
@Sent AS BIT,
@MessageType INT
AS
BEGIN
    INSERT dbo.Messages
    (
        DetailMessage,
        ClientSent,
        ClientReceiver,
        Sent,
        MessageType
    )
    VALUES
    (   @MessageDetail,
        @ClientSent,
        @ClientReceiver,
        @Sent,
        @MessageType
        )
END
GO
