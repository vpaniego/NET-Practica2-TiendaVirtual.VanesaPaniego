
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/05/2019 23:36:19
-- Generated from EDMX file: C:\MisProyectos\NET\NET-Practica2-TiendaVirtual.VanesaPaniego\TiendaVirtual_CarritoCompra\TiendaVirtual_CarritoCompra\Models\TiendaVirtualCC.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TiendaVirtualCC];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CategoriasProductos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Productos] DROP CONSTRAINT [FK_CategoriasProductos];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticuloCarritoProductos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Carrito] DROP CONSTRAINT [FK_ArticuloCarritoProductos];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categorias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorias];
GO
IF OBJECT_ID(N'[dbo].[Productos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Productos];
GO
IF OBJECT_ID(N'[dbo].[Carrito]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Carrito];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categorias'
CREATE TABLE [dbo].[Categorias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Productos'
CREATE TABLE [dbo].[Productos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [PathImagen] nvarchar(max)  NOT NULL,
    [PrecioUnidad] decimal(18,0)  NOT NULL,
    [Categoria_Id] int  NOT NULL
);
GO

-- Creating table 'Carrito'
CREATE TABLE [dbo].[Carrito] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [UsuarioId] nvarchar(max)  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [Productos_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Categorias'
ALTER TABLE [dbo].[Categorias]
ADD CONSTRAINT [PK_Categorias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [PK_Productos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Carrito'
ALTER TABLE [dbo].[Carrito]
ADD CONSTRAINT [PK_Carrito]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Categoria_Id] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [FK_CategoriasProductos]
    FOREIGN KEY ([Categoria_Id])
    REFERENCES [dbo].[Categorias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriasProductos'
CREATE INDEX [IX_FK_CategoriasProductos]
ON [dbo].[Productos]
    ([Categoria_Id]);
GO

-- Creating foreign key on [Productos_Id] in table 'Carrito'
ALTER TABLE [dbo].[Carrito]
ADD CONSTRAINT [FK_ArticuloCarritoProductos]
    FOREIGN KEY ([Productos_Id])
    REFERENCES [dbo].[Productos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticuloCarritoProductos'
CREATE INDEX [IX_FK_ArticuloCarritoProductos]
ON [dbo].[Carrito]
    ([Productos_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------