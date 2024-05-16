using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer
{
    public class UserPanelModel : PageModel
    {
        private int userId = 0;

        public void OnGet()
        {
            isUserLogin();
        }

        private void isUserLogin()
        {
            if (Request.Cookies["UserId"] == null)
                Response.Redirect("/Customer/Login-Register");
            else
                userId = int.Parse(Request.Cookies["UserId"]);
        }   
    }
}
