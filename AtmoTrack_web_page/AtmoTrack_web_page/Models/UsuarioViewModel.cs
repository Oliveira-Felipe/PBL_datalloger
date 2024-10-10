﻿namespace AtmoTrack_web_page.Models
{
    public class UsuarioViewModel
    {
        public Int32 Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string? TelefoneComercial { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public Int16 EstadoId { get; set; }

        public Int32? CidadeId { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
