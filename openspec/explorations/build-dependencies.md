# Exploration: Build Dependencies & Setup

> Fecha: 2026-05-09
> Estado: Completada
> Próximo paso: Corregir dependencias rotas para que compile en máquina limpia

---

## Resumen

Se investigó el sistema de build, dependencias y proceso de distribución del proyecto. Se encontraron **bloqueantes críticos**: las DLLs de UI (Guna.UI y Siticone.UI) están referenciadas desde rutas absolutas del developer original que no existen en el repo.

---

## Stack Necesario para Compilar

| Requisito | Detalle | Nota |
|-----------|---------|------|
| **OS** | Windows | Obligatorio — WinForms + .NET Framework |
| **Build tool** | Visual Studio 2022 o MSBuild + .NET Framework build tools | SDK de .NET Framework 4.7.2 necesario |
| **Runtime** | .NET Framework 4.7.2 | Viene incluido en Windows 10/11 moderno |
| **NuGet** | `nuget restore` o VS integrated restore | Para descargar dependencias |

> ⚠ **IMPORTANTE**: .NET Framework NO es lo mismo que .NET (Core). Este proyecto SOLO funciona en Windows.

---

## Proceso de Build Paso a Paso

### 1. Clonar el repo

```bash
git clone <repo-url>
cd BDO-Font-Patcher
```

### 2. ⚠ Corregir dependencias rotas (BLOQUEANTE)

Las siguientes referencias en `Universal Font Patcher BDO.csproj` apuntan a rutas inexistentes:

```xml
<!-- ROTA: apunta al escritorio del developer original -->
<Reference Include="Guna.UI">
  <HintPath>..\..\..\..\Desktop\Dev\Guna.UI.dll</HintPath>
</Reference>

<!-- ROTA: apunta al escritorio del developer original -->
<Reference Include="Siticone.UI">
  <HintPath>..\..\..\..\Desktop\Dev\Siticone.UI.dll</HintPath>
</Reference>
```

**Opción A — Migrar a NuGet (recomendado):**

| DLL original | NuGet Package Equivalente |
|-------------|--------------------------|
| Guna.UI.dll | `Guna.UI2.WinForms` v2.0.4.7 |
| Siticone.UI.dll | `Siticone.NetFramework.UI` v2026.2.22 |

Modificar `packages.config` para agregar:
```xml
<package id="Guna.UI2.WinForms" version="2.0.4.7" targetFramework="net472" />
<package id="Siticone.NetFramework.UI" version="2026.2.22" targetFramework="net48" />
```

Modificar `.csproj` para usar los HintPaths de NuGet:
```xml
<Reference Include="Guna.UI2.WinForms">
  <HintPath>..\packages\Guna.UI2.WinForms.2.0.4.7\lib\net472\Guna.UI2.WinForms.dll</HintPath>
</Reference>
<Reference Include="Siticone.UI">
  <HintPath>..\packages\Siticone.NetFramework.UI.2026.2.22\lib\net48\Siticone.UI.dll</HintPath>
</Reference>
```

**Opción B — DLLs locales (rápido):**
Crear carpeta `lib/` en la raiz del repo y colocar ahí las DLL originales. Modificar `.csproj` para apuntar a:
```xml
<HintPath>..\lib\Guna.UI.dll</HintPath>
<HintPath>..\lib\Siticone.UI.dll</HintPath>
```

### 3. Restaurar paquetes NuGet

```bash
cd "Universal Font Patcher BDO"
nuget restore
# O desde Visual Studio: botón derecho solución → Restore NuGet Packages
```

### 4. Compilar

```bash
# MSBuild
msbuild "Universal Font Patcher BDO.sln" /p:Configuration=Release

# O desde Visual Studio: Build → Build Solution (Ctrl+Shift+B)
```

### 5. Output

```
bin\Release\Universal Font Patcher BDO.exe    ← EXE autónomo (~15-20 MB)
```

---

## Cómo Funciona la Distribución (Costura.Fody)

1. MSBuild compila normalmente → genera `.exe` + varias `.dll`
2. **Fody** (IL weaver) se ejecuta post-compilación
3. **Costura.Fody** incrusta TODAS las DLLs como **recursos embebidos** dentro del `.exe`
4. Inyecta un inicializador que carga las DLLs desde memoria bajo demanda

**Resultado:** Un solo `.exe` autónomo — sin DLLs externas requeridas.

Configuración actual en `FodyWeavers.xml`:
```xml
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <Costura />
</Weavers>
```

El `<Costura />` vacío significa que embebe TODOS los assemblies marcados como `Copy Local=true`.

---

## Bloqueantes Actuales

| Bloqueante | Severidad | Solución |
|-----------|-----------|----------|
| **Guna.UI.dll no encontrada** | 🔴 Crítico | Reemplazar referencia por NuGet o DLL local |
| **Siticone.UI.dll no encontrada** | 🔴 Crítico | Reemplazar referencia por NuGet o DLL local |
| **Carpeta `packages/` no existe** | 🟡 Alto | Ejecutar `nuget restore` |
| **No hay `.gitignore`** | 🟡 Bajo | Crear `.gitignore` para excluir `packages/`, `bin/`, `obj/` |

---

## Riesgos

| Riesgo | Impacto |
|--------|---------|
| **Cambios de API** entre Guna.UI (v1 original) y Guna.UI2.WinForms (v2 NuGet) | Namespaces y nombres de controles pueden diferir (ej: `GunaLabel` → `Guna2Label`) |
| **Siticone.NetFramework.UI está deprecated** | El package NuGet puede tener disponibilidad limitada |
| **Los sonidos .wav pueden no funcionar en ciertas configuraciones** | No crítico, falla silenciosa |
| **Compatibilidad de versión de Fody/Costura** | Las versiones en packages.config deben ser compatibles entre sí |

---

## Notas para Windows

Al mover el repo a Windows:
1. Las rutas de Windows usan `\` en vez de `/`
2. Necesitarás Visual Studio 2022 Community (gratuito) o MSBuild standalone
3. El NuGet CLI puede instalarse con `winget install NuGet.CLI` o desde nuget.org
4. Si usas Opción B (DLLs locales), súbelas a `lib/` desde este Linux antes del push
