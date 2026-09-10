using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Readers.Pages.Account
{
    public class RegisterModel : PageModel
    {
        // This binds the form data to this property when the user clicks "Register"
        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet()
        {
            // Runs when the page first loads
        }

        public void OnPost()
        {
            // Runs when the "Register" button is clicked
            if (ModelState.IsValid)
            {
                // only for testing purposes here 
                var username = Input.Username;
                var email = Input.Email;
                var password = Input.Password;

            }
        }

        // This is the class that defines what fields the form will have
        public class InputModel
        {
            [Required]
            public string? Username { get; set; } = null;

            [Required]
            [EmailAddress]
            public string? Email { get; set; } = null;

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; } = null;
        }
    }
}