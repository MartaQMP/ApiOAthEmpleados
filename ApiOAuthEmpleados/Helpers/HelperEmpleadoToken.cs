using ApiOAuthEmpleados.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiOAuthEmpleados.Helpers
{
    public class HelperEmpleadoToken
    {
        private IHttpContextAccessor accessor;

        public HelperEmpleadoToken(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public EmpleadoModel GetEmpleado()
        {
            Claim claim = this.accessor.HttpContext.User.FindFirst(z => z.Type == "UserData");
            string json = claim.Value;
            string jsonEmpleado = HelperCifrado.DescifrarString(json);
            return JsonConvert.DeserializeObject<EmpleadoModel>(jsonEmpleado);
        }
            
    }
}
