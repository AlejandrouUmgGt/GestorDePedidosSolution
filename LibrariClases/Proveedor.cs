namespace LibreriaClases
{
    /// <summary>
    /// Proveedor de productos. Extiende Persona y agrega los datos
    /// propios de una entidad comercial.
    /// </summary>
    public class Proveedor : Persona
    {
        public string Nit { get; set; }
        public string NombreComercial { get; set; }
        public string DireccionFiscal { get; set; }
        public string DireccionEntrega { get; set; }

        public Proveedor() : base()
        {
            Nit = string.Empty;
            NombreComercial = string.Empty;
            DireccionFiscal = string.Empty;
            DireccionEntrega = string.Empty;
        }

        public Proveedor(string cui, string nombre, string apellido, string direccion,
                          string nit, string nombreComercial, string direccionFiscal, string direccionEntrega)
            : base(cui, nombre, apellido, direccion)
        {
            Nit = nit;
            NombreComercial = nombreComercial;
            DireccionFiscal = direccionFiscal;
            DireccionEntrega = direccionEntrega;
        }

        public override string ToString()
        {
            return $"{NombreComercial} (NIT: {Nit}) - Contacto: {NombreCompleto}";
        }
    }
}
