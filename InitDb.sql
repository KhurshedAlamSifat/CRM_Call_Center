
-- USER --
CREATE TABLE [DocumentType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[DocumentTypeID] [int] NULL,
	[Identification] [varchar](150) NULL,
	[Nombres] [varchar](150) NULL,
	[Apelllidos] [varchar](150) NULL,
	[Email] [varchar](150) NULL,
	[Password] [varchar](150) NULL,
	[Telefonos] [varchar](150) NULL,
	[Direccion] [varchar](150) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [User]  WITH CHECK ADD  CONSTRAINT [FK_User_DocumentType] FOREIGN KEY([DocumentTypeID])
REFERENCES [DocumentType] ([ID])
GO

ALTER TABLE [User] CHECK CONSTRAINT [FK_User_DocumentType]
GO



CREATE TABLE [GuestUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[LastActivityAt] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_GuestUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO





-- ROLE --
CREATE TABLE [Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



-- USER-ROLE-MAPPING --
CREATE TABLE [User_Role_Mapping](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_User_Role_Mapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [User_Role_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_Mapping_Role] FOREIGN KEY([RoleID])
REFERENCES [Role] ([ID])
GO
ALTER TABLE [User_Role_Mapping] CHECK CONSTRAINT [FK_User_Role_Mapping_Role]
GO
ALTER TABLE [User_Role_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_Mapping_User] FOREIGN KEY([UserID])
REFERENCES [User] ([ID])
GO
ALTER TABLE [User_Role_Mapping] CHECK CONSTRAINT [FK_User_Role_Mapping_User]
GO



CREATE TABLE [Permission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[SystemName] [varchar](350) NOT NULL,
	[CategoryName] [varchar](250) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Permission_Role_Mapping](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Permission_Role_Mapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Permission_Role_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Permission_Role_Mapping_Permission] FOREIGN KEY([PermissionID])
REFERENCES [Permission] ([ID])
GO

ALTER TABLE [Permission_Role_Mapping] CHECK CONSTRAINT [FK_Permission_Role_Mapping_Permission]
GO

ALTER TABLE [Permission_Role_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Permission_Role_Mapping_Role] FOREIGN KEY([RoleID])
REFERENCES [Role] ([ID])
GO

ALTER TABLE [Permission_Role_Mapping] CHECK CONSTRAINT [FK_Permission_Role_Mapping_Role]
GO





-- SYSTEM LOG --
CREATE TABLE [SystemLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogLevelId] [int] NULL,
	[ShortMessage] [text] NULL,
	[FullMessage] [text] NULL,
	[PageUrl] [text] NULL,
	[IpAddress] [varchar](100) NULL,
	[UserID] [int] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [SystemLog]  WITH CHECK ADD  CONSTRAINT [FK_Log_User] FOREIGN KEY([UserID])
REFERENCES [User] ([ID])
GO
ALTER TABLE [SystemLog] CHECK CONSTRAINT [FK_Log_User]
GO



-- SETTINGS --
CREATE TABLE [Setting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Value] [varchar](200) NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- LANGUAGE --
CREATE TABLE [Language](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[LanguageCulture] [varchar](20) NULL,
	[UniqueCode] [varchar](10) NULL,
	[Icon] [varchar](100) NULL,
	[IsDefault] [bit] NOT NULL,
	[DisplayOrder] [int] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- LOCALE STRINGS --
CREATE TABLE [LocaleStringResource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NULL,
	[ResourceName] [varchar](200) NULL,
	[ResourceValue] [text] NULL,
 CONSTRAINT [PK_LocaleStringResource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [LocaleStringResource]  WITH CHECK ADD  CONSTRAINT [FK_LocaleStringResource_Language] FOREIGN KEY([LanguageID])
REFERENCES [Language] ([ID])
GO
ALTER TABLE [LocaleStringResource] CHECK CONSTRAINT [FK_LocaleStringResource_Language]
GO




-- EMAIL AND EMAIL TEMPLATES --
CREATE TABLE [EmailTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Html] [text] NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [EmailAccount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Host] [varchar](350) NULL,
	[Port] [int] NULL,
	[EnableSsl] [bit] NOT NULL,
	[Username] [varchar](350) NULL,
	[Password] [varchar](350) NULL,
	[UseDefaultCredentials] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_EmailAccounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [GenericAttribute](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NULL,
	[KeyGroup] [varchar](400) NULL,
	[Key] [varchar](400) NULL,
	[Value] [text] NULL,
 CONSTRAINT [PK_GenericAttribute] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO









-- INSERTS --


SET IDENTITY_INSERT [DocumentType] ON 

INSERT [DocumentType] ([ID], [Name], [CreatedAt], [UpdatedAt], [Active], [Deleted]) VALUES (1, N'CEDULA', NULL, NULL, 1, 0)
INSERT [DocumentType] ([ID], [Name], [CreatedAt], [UpdatedAt], [Active], [Deleted]) VALUES (2, N'RUC', NULL, NULL, 1, 0)
INSERT [DocumentType] ([ID], [Name], [CreatedAt], [UpdatedAt], [Active], [Deleted]) VALUES (3, N'PASAPORTE', NULL, NULL, 1, 0)
SET IDENTITY_INSERT [DocumentType] OFF
GO



SET IDENTITY_INSERT [Language] ON 

INSERT [Language] ([ID], [Name], [LanguageCulture], [UniqueCode], [Icon], [IsDefault], [DisplayOrder], [Active], [Deleted]) VALUES (1, N'Español', N'es-EC', N'ec', N'ec', 1, 1, 1, 0)
INSERT [Language] ([ID], [Name], [LanguageCulture], [UniqueCode], [Icon], [IsDefault], [DisplayOrder], [Active], [Deleted]) VALUES (2, N'English', N'en-US', N'en', N'us', 0, 2, 1, 0)
SET IDENTITY_INSERT [Language] OFF
GO
SET IDENTITY_INSERT [LocaleStringResource] ON 

INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (1, 1, N'Secure.Login.Title', N'Ingresar')
INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (2, 2, N'Secure.Login.Title', N'Login')
INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (3, 1, N'Account.Fields.Email', N'Correo electrónico')
INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (4, 2, N'Account.Fields.Email', N'Email')
INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (5, 1, N'Account.Fields.Password', N'Contraseña')
INSERT [LocaleStringResource] ([ID], [LanguageID], [ResourceName], [ResourceValue]) VALUES (6, 2, N'Account.FieldsPassword', N'Password')
SET IDENTITY_INSERT [LocaleStringResource] OFF
GO
SET IDENTITY_INSERT [Role] ON 

INSERT [Role] ([ID], [Name], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (2, N'Administrador', NULL, NULL, 0)
INSERT [Role] ([ID], [Name], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (3, N'Usuario', NULL, NULL, 0)
SET IDENTITY_INSERT [Role] OFF
GO
SET IDENTITY_INSERT [User] ON 

INSERT [User] ([ID], [Guid],[DocumentTypeID], [Identification], [Nombres], [Apelllidos], [Email], [Password], [Telefonos], [Direccion], [CreatedAt], [UpdatedAt], [Active], [Deleted]) VALUES (1, N'47330ab1-d217-4343-976c-cef285a1508f',1, 2,  N'Juan Carlos', N'Méndez', N'jmendez@accroachcode.com', N'rH8UZp/588ihjFo30I71X9K/PNHGGoAH', N'090934882984', N'La Joya, Etapa Coral', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-05-15T02:46:19.210' AS DateTime), 1, 0)
SET IDENTITY_INSERT [User] OFF
GO
SET IDENTITY_INSERT [User_Role_Mapping] ON 

INSERT [User_Role_Mapping] ([ID], [UserID], [RoleID]) VALUES (1, 1, 2)
SET IDENTITY_INSERT [User_Role_Mapping] OFF
GO
SET IDENTITY_INSERT [Setting] ON 

INSERT [Setting] ([ID], [Name], [Value]) VALUES (1, N'system.default.language', N'1')
SET IDENTITY_INSERT [Setting] OFF
GO


SET IDENTITY_INSERT [EmailAccount] ON 
INSERT [EmailAccount] ([ID], [Name], [Host], [Port], [EnableSsl], [Username], [Password], [UseDefaultCredentials], [Deleted], [CreatedAt]) VALUES (1, N'Default', N'in-v3.mailjet.com', 587, 0, N'f7ba02c8043dcc6d2ba9acea47d9b092', N'9f18f1e11e906696ea5c19f332459348', 1, 0, NULL)
SET IDENTITY_INSERT [EmailAccount] OFF
GO
SET IDENTITY_INSERT [EmailTemplate] ON 

INSERT [EmailTemplate] ([ID], [Name], [Html]) VALUES (1, N'Contactus.Admin', N'<table><tr><td>NOMBRES</td><td>{NOMBRES}</td></tr><tr><td>APELLIDOS</td><td>{APELLIDOS}</td></tr><tr><td>EMAIL</td><td>{EMAIL}</td></tr><tr><td>TELEFONO</td><td>{TELEFONO}</td></tr><tr><td>SOLUCION</td><td>{SOLUCION}</td></tr></table>')
SET IDENTITY_INSERT [EmailTemplate] OFF
GO




GO
SET IDENTITY_INSERT [Permission] ON 

INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (1, N'Se pueden listar los usuarios y ver sus datos', N'Root.User.List', N'Admin.Usuarios')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (3, N'Se puede editar usuarios', N'Root.User.Edit', N'Admin.Usuarios')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (6, N'Se pueden crear usuarios', N'Root.User.Create', N'Admin.Usuarios')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (8, N'Se pueden listar los registros de nuevos usuarios', N'Root.Registros.List', N'Admin.Registros')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (9, N'Se pueden editar los registros de nuevos usuarios', N'Root.Registros.Edit', N'Admin.Registros')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (10, N'Se pueden crear registros', N'Root.Registros.Create', N'Admin.Registros')
INSERT [Permission] ([ID], [Name], [SystemName], [CategoryName]) VALUES (11, N'Dashboard de Root', N'Root.Dashboard', N'Admin.Dashboard')
SET IDENTITY_INSERT [Permission] OFF
GO
SET IDENTITY_INSERT [Permission_Role_Mapping] ON 

INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (1, 1, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (4, 6, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (5, 3, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (6, 8, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (7, 9, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (8, 10, 2)
INSERT [Permission_Role_Mapping] ([ID], [PermissionID], [RoleID]) VALUES (9, 11, 2)
SET IDENTITY_INSERT [Permission_Role_Mapping] OFF
GO





