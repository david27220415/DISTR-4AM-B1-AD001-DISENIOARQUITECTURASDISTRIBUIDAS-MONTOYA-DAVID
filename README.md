\# ComercioMultibodega — AD01



\## Descripción



Proyecto de arquitectura distribuida desarrollado con ASP.NET Core Web API (.NET 10), Entity Framework Core, SQL Server y Swagger.



La solución contiene tres microservicios REST independientes, cada uno con su propia base de datos.



\## Microservicios



\### Catalogo.Api

Base de datos: `CatalogoDB`



Entidades:

\- Categoria

\- Marca

\- Producto



\### Inventario.Api

Base de datos: `InventarioDB`



Entidades:

\- Bodega

\- Existencia

\- MovimientoInventario

\- ReservaStock



\### Pedidos.Api

Base de datos: `PedidosDB`



Entidades:

\- Cliente

\- Pedido

\- DetallePedido



\## Tecnologías



\- C# y .NET 10

\- ASP.NET Core Web API

\- Entity Framework Core

\- SQL Server

\- Swagger / OpenAPI

\- Visual Studio 2022



\## Arquitectura



Los tres microservicios se ejecutan independientemente y utilizan bases de datos separadas. En esta etapa no existe comunicación HTTP entre los microservicios.



\## Ejecución



1\. Abrir la solución en Visual Studio.

2\. Configurar las conexiones a SQL Server.

3\. Crear las bases de datos y tablas utilizando los scripts SQL del proyecto.

4\. Configurar los tres proyectos como proyectos de inicio.

5\. Ejecutar la solución.



\## Swagger



Con los puertos configurados en `launchSettings.json`:



\- Catálogo: `https://localhost:7001/swagger`

\- Inventario: `https://localhost:7002/swagger`

\- Pedidos: `https://localhost:7003/swagger`



\## Actividad



AD01 — Diseño de Arquitecturas Distribuidas.

