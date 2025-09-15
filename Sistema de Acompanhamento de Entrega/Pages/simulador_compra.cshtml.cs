using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace Sistema_de_Acompanhamento_de_Entrega.Pages
{
    public class simulador_compraModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public simulador_compraModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [Display(Name = "Nome do Cliente")]
        public string ClienteNome { get; set; } = string.Empty;

        [BindProperty]
        public string ProdutosJson { get; set; } = string.Empty;

        public void OnGet()
        {
            // Método executado quando a página é carregada
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Debug: Log do JSON recebido
                Console.WriteLine($"ProdutosJson recebido: {ProdutosJson}");

                // Verificar se o JSON não está vazio
                if (string.IsNullOrWhiteSpace(ProdutosJson))
                {
                    ModelState.AddModelError("", "Nenhum produto foi enviado");
                    return Page();
                }

                // Configurar opções do JsonSerializer para ser mais flexível
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Deserializar os produtos do JSON
                var produtos = JsonSerializer.Deserialize<List<ProdutoTemp>>(ProdutosJson, options);

                // Debug: Log dos produtos deserializados
                Console.WriteLine($"Produtos deserializados: {produtos?.Count ?? 0}");
                if (produtos != null)
                {
                    foreach (var prod in produtos)
                    {
                        Console.WriteLine($"Produto: {prod.Nome}, Quantidade: {prod.Quantidade}, Preço: {prod.Preco}");
                    }
                }

                if (produtos == null || !produtos.Any())
                {
                    ModelState.AddModelError("", "Adicione pelo menos um produto ao carrinho");
                    return Page();
                }

                // Validar se os produtos têm dados válidos
                var produtosValidos = produtos.Where(p =>
                    !string.IsNullOrWhiteSpace(p.Nome) &&
                    p.Quantidade > 0 &&
                    p.Preco > 0).ToList();

                if (!produtosValidos.Any())
                {
                    ModelState.AddModelError("", "Nenhum produto válido foi encontrado");
                    return Page();
                }

                // Obter o próximo ID do pedido
                var proximoId = await ObterProximoPedidoId();
                var dataAtual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var valorTotal = produtosValidos.Sum(p => p.Quantidade * p.Preco);

                // Caminho do arquivo CSV
                var caminhoArquivo = Path.Combine(_environment.WebRootPath, "files", "pedidos.csv");

                // Garantir que o diretório existe
                var diretorio = Path.GetDirectoryName(caminhoArquivo);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                // Criar as linhas do CSV para cada produto
                var linhasCSV = new List<string>();

                foreach (var produto in produtosValidos)
                {
                    // Escapar aspas duplas no nome do cliente e produto
                    var nomeCliente = ClienteNome.Replace("\"", "\"\"");
                    var nomeProduto = produto.Nome.Replace("\"", "\"\"");

                    var linha = $"{proximoId},\"{nomeCliente}\",{dataAtual},{valorTotal.ToString("F2", CultureInfo.InvariantCulture)},Preparando,\"{nomeProduto}\",{produto.Quantidade},{produto.Preco.ToString("F2", CultureInfo.InvariantCulture)}";
                    linhasCSV.Add(linha);

                    // Debug: Log da linha criada
                    Console.WriteLine($"Linha CSV criada: {linha}");
                }

                // Escrever no arquivo CSV
                await System.IO.File.AppendAllLinesAsync(caminhoArquivo, linhasCSV);

                // Definir mensagem de sucesso
                TempData["Sucesso"] = $"Pedido #{proximoId} criado com sucesso! Total: R$ {valorTotal:F2}";

                // Redirecionar para limpar o formulário
                return RedirectToPage("/visualizar_pedidos");
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError("", $"Erro ao processar os dados dos produtos: {ex.Message}");
                Console.WriteLine($"Erro JSON: {ex.Message}");
                Console.WriteLine($"JSON recebido: {ProdutosJson}");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao processar o pedido: {ex.Message}");
                Console.WriteLine($"Erro geral: {ex.Message}");
                return Page();
            }
        }

        private async Task<int> ObterProximoPedidoId()
        {
            try
            {
                var caminhoArquivo = Path.Combine(_environment.WebRootPath, "files", "pedidos.csv");

                if (!System.IO.File.Exists(caminhoArquivo))
                {
                    return 1; // Se o arquivo não existir, começar com ID 1
                }

                var linhas = await System.IO.File.ReadAllLinesAsync(caminhoArquivo);

                if (linhas.Length <= 1) // Só tem cabeçalho ou está vazio
                {
                    return 1;
                }

                // Ignorar o cabeçalho e encontrar o maior ID
                var maiorId = 0;

                for (int i = 1; i < linhas.Length; i++) // Começar em 1 para pular o cabeçalho
                {
                    if (string.IsNullOrWhiteSpace(linhas[i]))
                        continue;

                    var colunas = linhas[i].Split(',');

                    if (colunas.Length > 0 && int.TryParse(colunas[0], out int id))
                    {
                        if (id > maiorId)
                            maiorId = id;
                    }
                }

                return maiorId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter próximo ID: {ex.Message}");
                return 1; // Em caso de erro, retornar 1
            }
        }
    }

    // Classe auxiliar para deserializar os produtos do JSON
    public class ProdutoTemp
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}