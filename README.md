# CRUD Cliente (SQL Server + .NET 8 + React Vite)

Este repositorio incluye un CRUD completo para la entidad **Cliente**, usando:

- **SQL Server** (tabla + constraints + stored procedures)
- **ASP.NET Core Web API .NET 8** (Dapper + SPs, sin Entity Framework)
- **React + Vite** (axios)

## 1) Estructura del proyecto

```text
/workspace/Prueba
├── sql/
│   └── cliente_crud.sql
├── backend/
│   ├── ClienteApi.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Controllers/
│   │   └── ClientesController.cs
│   ├── Data/
│   │   ├── IClienteRepository.cs
│   │   └── ClienteRepository.cs
│   └── Models/
│       ├── ApiErrorResponse.cs
│       ├── Cliente.cs
│       ├── ClienteCreateDto.cs
│       ├── ClienteUpdateDto.cs
│       ├── ClienteReadDto.cs
│       └── NoFutureDateAttribute.cs
└── frontend/
    ├── package.json
    ├── vite.config.js
    ├── index.html
    └── src/
        ├── main.jsx
        ├── styles.css
        ├── api/
        │   └── clientesApi.js
        ├── components/
        │   ├── ClienteForm.jsx
        │   └── ClientesTable.jsx
        └── pages/
            └── ClientesPage.jsx
```

---

## 2) Parte SQL Server

### Script completo

Ejecuta:

- `sql/cliente_crud.sql`

Este script:

- Crea la base `ClientesDb` (si no existe)
- Crea `dbo.Clientes` con constraints de validación
- Crea índices recomendados
- Crea procedimientos almacenados:
  - `dbo.Cliente_Insert`
  - `dbo.Cliente_Update`
  - `dbo.Cliente_Delete`
  - `dbo.Cliente_GetById`
  - `dbo.Cliente_GetAll`
  - `dbo.Cliente_SearchByNombre` (opcional incluido)
- Incluye ejemplos `EXEC` para pruebas.

---

## 3) Backend .NET 8 Web API (Dapper + Stored Procedures)

### Crear proyecto (si quieres generarlo desde cero)

```bash
dotnet new webapi -n ClienteApi -f net8.0
```

Luego reemplaza archivos con los contenidos de `/backend`.

### Instalar dependencias

```bash
cd backend
dotnet restore
```

### Configurar conexión SQL Server

Editar `backend/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ClientesDb;User Id=sa;Password=TuPassword123!;TrustServerCertificate=True;"
}
```

### Ejecutar API

```bash
cd backend
dotnet run
```

Swagger (en development) estará disponible en la URL que imprima consola, por ejemplo:

- `https://localhost:7xxx/swagger`
- `http://localhost:5xxx/swagger`

### Validaciones implementadas

- `nombre`: requerido, entre 2 y 120 caracteres
- `edad`: 0 a 120
- `fechaNacimiento`: no futura (`NoFutureDateAttribute`)
- `salario`: mayor o igual a 0

### CORS

Configurado en `Program.cs` para permitir:

- `http://localhost:5173`

---

## 4) Endpoints y ejemplos

Base URL: `http://localhost:5099/api` (ajusta al puerto real de tu API)

### GET /api/clientes

**Response 200:**

```json
[
  {
    "clienteId": 1,
    "nombre": "Juan Pérez",
    "edad": 30,
    "fechaNacimiento": "1994-01-20T00:00:00",
    "salario": 2500.00
  }
]
```

### GET /api/clientes/{id}

**Response 200:**

```json
{
  "clienteId": 1,
  "nombre": "Juan Pérez",
  "edad": 30,
  "fechaNacimiento": "1994-01-20T00:00:00",
  "salario": 2500.00
}
```

**Response 404:**

```json
{
  "message": "Cliente con id 99 no encontrado.",
  "details": null
}
```

### POST /api/clientes

**Request:**

```json
{
  "nombre": "Ana Torres",
  "edad": 28,
  "fechaNacimiento": "1996-03-14",
  "salario": 3200.50
}
```

**Response 201 (Created + Location):**

```json
{
  "clienteId": 3,
  "nombre": "Ana Torres",
  "edad": 28,
  "fechaNacimiento": "1996-03-14T00:00:00",
  "salario": 3200.50
}
```

### PUT /api/clientes/{id}

**Request:**

```json
{
  "nombre": "Ana Torres López",
  "edad": 29,
  "fechaNacimiento": "1995-03-14",
  "salario": 3500.00
}
```

**Response 200:** cliente actualizado.

### DELETE /api/clientes/{id}

**Response 204:** sin contenido.

### Errores y códigos

- `400 BadRequest`: validación de DTOs
- `404 NotFound`: recurso no encontrado
- `500 InternalServerError`: error controlado (con `ApiErrorResponse`)

---

## 5) Frontend React (Vite)

### Crear proyecto (si quieres generarlo desde cero)

```bash
npm create vite@latest frontend -- --template react
```

Luego reemplaza archivos con los contenidos de `/frontend`.

### Instalar dependencias

```bash
cd frontend
npm install
```

### Configurar URL del backend

Crear `.env` en `/frontend`:

```bash
VITE_API_BASE_URL=http://localhost:5099/api
```

> Ajusta el puerto al de `dotnet run`.

### Ejecutar frontend

```bash
cd frontend
npm run dev
```

Abrir:

- `http://localhost:5173`

### Funcionalidades incluidas

- Listado en tabla
- Alta (crear cliente)
- Edición (carga por ID y reuso del formulario)
- Baja con confirmación
- Loading/error visual
- Manejo de fechas ISO (`yyyy-MM-dd`) al enviar, formateadas en UI al listar

---

## 6) Flujo de ejecución local paso a paso

1. **SQL Server**: ejecutar `sql/cliente_crud.sql` en SSMS/Azure Data Studio.
2. **Backend**:
   - `cd backend`
   - configurar `appsettings.json`
   - `dotnet restore`
   - `dotnet run`
3. **Frontend**:
   - `cd frontend`
   - crear `.env` con `VITE_API_BASE_URL`
   - `npm install`
   - `npm run dev`
4. Probar CRUD desde la UI React y/o Swagger.
