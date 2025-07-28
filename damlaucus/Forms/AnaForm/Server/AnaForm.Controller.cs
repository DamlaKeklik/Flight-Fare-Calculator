using Microsoft.AspNetCore.Mvc;
using Bimser.CSP.FormControls.Api;
using Bimser.Framework.Dependency;
using Bimser.Framework.AspNetCore.Mvc.Attributes;

namespace damlaucus.Forms
{
    [Route("apps/damlaucus/latest/api/AnaForm/[action]")]
    [Route("apps/damlaucus/{v:int:min(1)}/api/AnaForm/[action]")]
    [Route("api/AnaForm/[action]")]
    [Produces("application/json")]
    public class AnaFormController : BaseFormController<AnaForm>
    {
        public AnaFormController(IIocManager iocManager) : base(iocManager)
        {

        }

        [HttpGet]
        [ActionName("Ping")]
        [NoRequestHeaders]
        [NoResponseHeaders]
        public string Ping()
        {
            return "AnaForm API Controller is ok";            
        }
    }
}