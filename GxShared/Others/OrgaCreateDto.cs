using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class OrgaCreateDto
    {
        // Orga identifiers
        public int Idorg { get; set; } = 0;
        public int Ipays { get; set; } = 0;

        // Customer identity (from order)
        public string? Nom { get; set; }
        public string? Pnom { get; set; }
        public string? Raison { get; set; }
        public string? Sigle { get; set; }
        public int Iwurl { get; set; } = 0;
        public string? Weburl { get; set; }
        public string? Adr { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        // Support bootstrap
        public string? UsernameOrEmail { get; set; }
        public string? Password { get; set; }
        // Optional orga setup
        public int Idoma { get; set; } = 0;
        public string? Sidom { get; set; }
        public int Jdoma { get; set; } = 0;
        public string? Sjdom { get; set; }
        public string Stcible { get; set; } = string.Empty;
        // AppState seed
        public int Iqmax { get; set; } = 0;
        public int Ishare { get; set; } = 0;
        public int Ordid { get; set; } = 0;
    }
    public class ApiResponse<T>
    {
        public int ErrCode { get; set; } = 0;
        public string? ErrMessage { get; set; }
        public T Payload { get; set; }
        public static ApiResponse<T> Success(T payload, string message = "Success")
            => new ApiResponse<T> { ErrCode = 9, ErrMessage = message, Payload = payload };
        public static ApiResponse<T> Failure(int code, string message, T payload = default)
            => new ApiResponse<T> { ErrCode = code, ErrMessage = message, Payload = payload };
    }
    public class ApiRequest<T>
    {
        public Guid CorrelationId { get; set; }
        public string? Action { get; set; }
        public T Payload { get; set; }
    }
    public class RegisterDto
    {
        public int Id { get; set; } = 0;
        public int Idorg { get; set; } = 0;
        public int Idhie { get; set; } = 0;          // hierarchy id
        public int Orig { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // required
        public string ConfirmPassword { get; set; } = string.Empty; // required
        public string Username { get; set; } = string.Empty; // required
        public string Phonenumber { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
        public bool SiSupport { get; set; } = false; // support flag
        public bool SiOwner { get; set; } = false;
        public string Nom { get; set; } = string.Empty;      // last name
        public string Pnom { get; set; } = string.Empty;     // first name
        public string Raison { get; set; } = string.Empty;   // raison sociale
        public string Sigle { get; set; } = string.Empty;    // abbreviation
        public int Utyp { get; set; } = 0;                   // 1 agent, 2 operator, 3 client
        public int Ipays { get; set; } = 0;                  // country id
        public string Ilang { get; set; } = string.Empty;
        public string Weburl { get; set; } = string.Empty;
        public string FbackUrl { get; set; } = string.Empty;
        public string Uip { get; set; } = string.Empty;
        public int Fro { get; set; } = 0;
        public string FroAddress { get; set; } = string.Empty;
        public string Stdoma { get; set; } = string.Empty;
        public string Srole { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
    }
}
