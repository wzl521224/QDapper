using System.Collections.Generic;
using Iyosen.QDapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api")]
    public class QDapperController : Controller
    {
        [HttpGet("qdapper")]
        public JsonResult GetData([FromQuery] Dictionary<string, string> param = null)
        {
            return new JsonResult(QDapper.QDapperData(param));
        }
    }
}
