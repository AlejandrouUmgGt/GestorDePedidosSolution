namespace LibreriaClases
{
    /// <summary>
    /// Clase base común para cualquier persona relacionada con la empresa
    /// (por ejemplo, un Proveedor). No se instancia directamente.
    /// </summary>
    public abstract class Persona
    {
        public string CUI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }

        protected Persona()
        {
            CUI = string.Empty;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Direccion = string.Empty;
        }

        protected Persona(string cui, string nombre, string apellido, string direccion)
        {
            CUI = cui;
            Nombre = nombre;
            Apellido = apellido;
            Direccion = direccion;
        }

        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

        public override string ToString()
        {
            return $"{NombreCompleto} (CUI: {CUI})";
        }
    }
}
