using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Sistema_de_Acompanhamento_de_Entrega.Pages
{
    public class visualizar_pedidosModel : PageModel
    {
        private readonly ILogger<visualizar_pedidosModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public visualizar_pedidosModel(ILogger<visualizar_pedidosModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public List<PedidoViewModel> Pedidos { get; set; } = new List<PedidoViewModel>();

        public void OnGet()
        {
            try
            {
                Pedidos = CarregarPedidosDoCSV();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar pedidos do arquivo CSV");
                Pedidos = new List<PedidoViewModel>();
            }
        }

        private List<PedidoViewModel> CarregarPedidosDoCSV()
        {
            var caminhoArquivo = Path.Combine(_environment.WebRootPath, "files", "pedidos.csv");

            if (!System.IO.File.Exists(caminhoArquivo))
            {
                _logger.LogWarning("Arquivo CSV não encontrado: {CaminhoArquivo}", caminhoArquivo);
                return new List<PedidoViewModel>();
            }

            var pedidosDict = new Dictionary<int, PedidoViewModel>();
            var linhas = System.IO.File.ReadAllLines(caminhoArquivo);

            // Pula o cabeçalho (primeira linha)
            for (int i = 1; i < linhas.Length; i++)
            {
                var linha = linhas[i];
                if (string.IsNullOrWhiteSpace(linha)) continue;

                var campos = ParseCSVLine(linha);
                if (campos.Length < 8) continue;

                try
                {
                    var pedidoId = int.Parse(campos[0]);
                    var clienteNome = campos[1];
                    var data = DateTime.Parse(campos[2], CultureInfo.InvariantCulture);
                    var valorTotal = decimal.Parse(campos[3], CultureInfo.InvariantCulture);
                    var status = campos[4];
                    var produtoNome = campos[5];
                    var quantidade = int.Parse(campos[6]);
                    var precoUnitario = decimal.Parse(campos[7], CultureInfo.InvariantCulture);

                    // Se o pedido ainda não existe no dicionário, cria um novo
                    if (!pedidosDict.ContainsKey(pedidoId))
                    {
                        pedidosDict[pedidoId] = new PedidoViewModel
                        {
                            Id = pedidoId,
                            ClienteNome = clienteNome,
                            Data = data,
                            ValorTotal = valorTotal,
                            Status = status,
                            StatusClass = ObterClasseStatus(status),
                            Itens = new List<ItemPedidoViewModel>()
                        };
                    }

                    // Adiciona o item ao pedido
                    pedidosDict[pedidoId].Itens.Add(new ItemPedidoViewModel
                    {
                        ProdutoNome = produtoNome,
                        Quantidade = quantidade,
                        PrecoUnitario = precoUnitario
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao processar linha {NumeroLinha}: {Linha}", i + 1, linha);
                    continue;
                }
            }

            // Converte o dicionário para lista e ordena por data (mais recentes primeiro)
            return pedidosDict.Values
                .OrderByDescending(p => p.Data)
                .ToList();
        }

        private string[] ParseCSVLine(string linha)
        {
            var campos = new List<string>();
            var campoAtual = "";
            var dentroDeAspas = false;

            for (int i = 0; i < linha.Length; i++)
            {
                var caractere = linha[i];

                if (caractere == '"')
                {
                    dentroDeAspas = !dentroDeAspas;
                }
                else if (caractere == ',' && !dentroDeAspas)
                {
                    campos.Add(campoAtual.Trim());
                    campoAtual = "";
                }
                else
                {
                    campoAtual += caractere;
                }
            }

            // Adiciona o último campo
            campos.Add(campoAtual.Trim());

            return campos.ToArray();
        }

        private string ObterClasseStatus(string status)
        {
            return status switch
            {
                "Em Trânsito" => "bg-yellow-100 text-yellow-800",
                "Entregue" => "bg-green-100 text-green-800",
                "Preparando" => "bg-blue-100 text-blue-800",
                "Cancelado" => "bg-red-100 text-red-800",
                _ => "bg-gray-100 text-gray-800"
            };
        }
    }

    public class PedidoViewModel
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusClass { get; set; } = string.Empty;
        public List<ItemPedidoViewModel> Itens { get; set; } = new List<ItemPedidoViewModel>();
    }

    public class ItemPedidoViewModel
    {
        public string ProdutoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}

