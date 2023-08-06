using Microsoft.AspNetCore.Mvc;

namespace NetCoreSwaggerWebSiteAdditionExample.Base
{
    [Route($@"Open/[controller]/[action]")]
    [Area("Open")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class OpenBaseController: BaseController
    {
    }
}
