using System;
using Microsoft.AspNetCore.Mvc;

namespace WebService
{
    public class PageController : Controller
    {
        // behøver ikke en delegate Func, da den har en default null value.
        public string Link(string route, int page, int pageSize, int pageInc = 0, Func<bool> f = null)
        {
            // hvis func er null, så benyt UriHelper klassens Link metode og lav selvreference til nuværende page.
            if (f == null) return Url.Link(route, new { page, pageSize });

            // for pagination tjekker den ved prev om pageInc (kommende side) er > 0 og ved next om < totalPages -1.
            return f()
                ? Url.Link(route, new { page = page + pageInc, pageSize })
                : null;
        }

        public int GetTotalPages(int pageSize, int total)
        {
            return (int)Math.Ceiling(total / (double)pageSize);
        }

        public void CheckPageSize(ref int pageSize)
        {
            pageSize = pageSize > 50 ? 50 : pageSize;
        }
    }
}