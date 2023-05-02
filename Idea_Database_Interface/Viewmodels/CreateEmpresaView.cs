﻿using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class CreateEmpresaView
    {
        public DateTime FechaDeAlta { get; set; }
        public string CIF { get; set; }
        public string Nombre { get; set; }
        public string? Tamaño { get; set; }
        public string? CNAE { get; set; }
        public string? DetalleCNAE { get; set; }
        public string? Calle { get; set; }
        public string? Número { get; set; }
        public int? CódigoPostal { get; set; }
        public string? Provincia { get; set; }
        public string? Municipio { get; set; }
        public string? Localidad { get; set; }
        public string? Teléfono { get; set; }
        public string? Teléfono2 { get; set; }
        public string? Fax { get; set; }
        public string? Web { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
    }
}
