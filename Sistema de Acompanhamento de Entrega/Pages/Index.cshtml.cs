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
            // Validação dos campos obrigatórios
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewData["ErrorMessage"] = "Por favor, preencha todos os campos.";
                return Page();
            }

            // Validação das credenciais
            if (Username.Equals("adm", StringComparison.OrdinalIgnoreCase) && Password == "321")
            {
                // Login bem-sucedido - redireciona para a próxima página
                // Substitua "Dashboard" pelo nome da sua próxima página
                return RedirectToPage("/simulador_compra");
            }
            else
            {
                // Login falhou
                ViewData["ErrorMessage"] = "Usuário ou senha incorretos.";
                return Page();
            }
        }
    }
}