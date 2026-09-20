using LibreriaClases;

namespace GestionPedidos;

/// <summary>Instancia compartida del backend durante la ejecución de la aplicación.</summary>
internal static class DatosAplicacion
{
    internal static GestorPedidos Gestor { get; } = new();
}
