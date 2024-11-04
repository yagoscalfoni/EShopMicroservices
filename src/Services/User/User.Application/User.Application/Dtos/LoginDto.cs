using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Dtos
{
    public record LoginDto
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public bool RememberMe { get; init; }
    }
}
