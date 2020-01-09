using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AIGEspol_Frontend.Services;

namespace AIGEspol_Frontend.Pages
{
    public class LoginModel : PageModel
    {
        public string ticket { get; set; } = "";
        public IActionResult OnGet()
        {
            //System.Diagnostics.Debug.WriteLine("Entra");
            if (Request.Query.ContainsKey("ticket"))
            {
                this.ticket = Request.Query["ticket"];
                TokenValidator validador = new TokenValidator();
                if (validador.ValidateSessionToken(this.ticket))
                {

                    return RedirectToPage("./Mapa");
                }
                else
                {
                    return RedirectToPage("./Privacy");
                }
            } else
            {
                Response.Redirect("https://auth.espol.edu.ec/login?service=https%3A%2F%2Flocalhost:44319%2FLogin",false);
                return RedirectToPage("./Index");
            }
            
        }

    }
}