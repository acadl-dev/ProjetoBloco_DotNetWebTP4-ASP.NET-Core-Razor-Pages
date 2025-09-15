using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sistema_de_Acompanhamento_de_Entrega.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
            // Limpa qualquer mensagem de erro anterior
            ViewData["ErrorMessage"] = null;
        }

        public IActionResult OnPost()
        {
            // Valida��o dos campos obrigat�rios
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewData["ErrorMessage"] = "Por favor, preencha todos os campos.";
                return Page();
            }

            // Valida��o das credenciais
            if (Username.Equals("adm", StringComparison.OrdinalIgnoreCase) && Password == "321")
            {
                // Login bem-sucedido - redireciona para a pr�xima p�gina
                // Substitua "Dashboard" pelo nome da sua pr�xima p�gina
                return RedirectToPage("/simulador_compra");
            }
            else
            {
                // Login falhou
                ViewData["ErrorMessage"] = "Usu�rio ou senha incorretos.";
                return Page();
            }
        }
    }
}