using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slutprojekt_ASP.Net.Models
{
	public class Personalen
	{
		public int Id { get; set; }
        public string Namn { get; set; }
		public string Roll { get; set; }
		public string Telefon { get; set; }
		public string EMail { get; set; }
        public string Min_Resa { get; set; }
        public byte[]? image { get; set; }

        [NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile uploadedimage { get; set; }

        public Personalen()
        {

        }
    }
}

