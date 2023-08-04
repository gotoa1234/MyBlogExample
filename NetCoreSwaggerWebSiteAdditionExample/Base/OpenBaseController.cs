using Microsoft.AspNetCore.Mvc;

namespace NetCoreSwaggerWebSiteAdditionExample.Base
{
    [Route($@"Open/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OpenBaseController: BaseController
    {
    }
}
