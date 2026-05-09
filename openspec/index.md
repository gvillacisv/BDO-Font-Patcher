# OpenSpec — BDO-Font-Patcher

> Artifact Store para el proyecto BDO-Font-Patcher.
> Creado: 2026-05-09

## Artifacts

| Archivo | Descripción |
|---------|-------------|
| [`sdd-init.md`](./sdd-init.md) | Contexto del proyecto: stack, arquitectura, estructura, dependencias |
| [`testing-capabilities.md`](./testing-capabilities.md) | Capacidades de testing detectadas |
| [`skill-registry.md`](./skill-registry.md) | Registry de skills del proyecto |
| [`explorations/build-dependencies.md`](./explorations/build-dependencies.md) | Investigación de dependencias de build, setup local y distribución |

## Session: 2026-05-09

### Objetivo
Analizar el proyecto BDO-Font-Patcher y entender cómo se compila, configura y distribuye.

### Descubrimientos clave
- **Stack**: .NET Framework 4.7.2, WinForms, C#
- **Dependencias rotas**: Guna.UI.dll y Siticone.UI.dll referenciadas desde rutas absolutas inexistentes
- **Distribución**: Costura.Fody embebe todo en un solo EXE
- **Build requiere Windows**: El proyecto no compila en Linux

### Próximos pasos
- Corregir referencias de DLLs en `.csproj` (NuGet o DLLs locales)
- Restaurar paquetes NuGet con `nuget restore`
- Compilar y verificar que el EXE se genera correctamente
- Crear `.gitignore` para excluir `packages/`, `bin/`, `obj/`
