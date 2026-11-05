
using System;
using System.Collections.Generic;

class Program
{
    // Lista para armazenar os veículos da frota
    static List<Veiculo> frota = new List<Veiculo>();

    static void Main(string[] args)
    {
        Console.Title = "Sistema de Gestão de Frota Automóvel";

        int opcao = 0;

        do
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine("       SISTEMA DE GESTÃO DE FROTA");
            Console.WriteLine("==========================================");
            Console.ResetColor();

            Console.WriteLine("\n1 - Registar veículo");
            Console.WriteLine("2 - Listar veículos");
            Console.WriteLine("3 - Abastecer veículo");
            Console.WriteLine("4 - Realizar viagem");
            Console.WriteLine("5 - Consultar veículo");
            Console.WriteLine("6 - Sair");

            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOpção inválida!");
                Console.ResetColor();

                Pausar();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    RegistarVeiculo();
                    break;

                case 2:
                    ListarVeiculos();
                    break;

                case 3:
                    AbastecerVeiculo();
                    break;

                case 4:
                    RealizarViagem();
                    break;

                case 5:
                    ConsultarVeiculo();
                    break;

                case 6:
                    Console.WriteLine("\nPrograma encerrado. Até breve!");
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nOpção inexistente!");
                    Console.ResetColor();
                    Pausar();
                    break;
            }

        } while (opcao != 6);
    }

    // Registar um novo veículo
    static void RegistarVeiculo()
    {
        Console.Clear();

        Console.WriteLine("=== REGISTAR VEÍCULO ===");

        try
        {
            Console.Write("\nIntroduza a matrícula: ");
            string matricula = Console.ReadLine();

            // Verificar se a matrícula já existe
            foreach (Veiculo v in frota)
            {
                if (v.Matricula.Equals(
                    matricula,
                    StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(
                        "Erro: Já existe um veículo com esta matrícula!");
                    Pausar();
                    return;
                }
            }

            Console.Write("Introduza a capacidade do depósito (litros): ");

            int capacidade;

            if (!int.TryParse(Console.ReadLine(), out capacidade))
            {
                Console.WriteLine("Capacidade inválida!");
                Pausar();
                return;
            }

            Veiculo novoVeiculo =
                new Veiculo(matricula, capacidade);

            // Associar o evento de alerta de reserva
            novoVeiculo.ReservaAtingida +=
                (sender, e) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("\n==========================================");
                    Console.WriteLine("       ALERTA DE RESERVA DE COMBUSTÍVEL");
                    Console.WriteLine("==========================================");
                    Console.WriteLine("Veículo: " + novoVeiculo.Matricula);
                    Console.WriteLine("Atenção: O combustível está abaixo de 10%!");

                    Console.ResetColor();
                };

            frota.Add(novoVeiculo);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nVeículo registado com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nErro: " + ex.Message);
            Console.ResetColor();
        }

        Pausar();
    }

    // Listar todos os veículos
    static void ListarVeiculos()
    {
        Console.Clear();

        Console.WriteLine("=== LISTAGEM DA FROTA ===");

        if (frota.Count == 0)
        {
            Console.WriteLine("\nNão existem veículos registados.");
        }
        else
        {
            foreach (Veiculo veiculo in frota)
            {
                Console.WriteLine("\n------------------------------------------");
                Console.WriteLine("Matrícula: " + veiculo.Matricula);
                Console.WriteLine("Quilometragem: " + veiculo.Quilometragem + " km");

                Console.WriteLine(
                    "Combustível: " +
                    veiculo.NivelCombustivel.ToString("F2") + " litros");

                Console.WriteLine(
                    "Capacidade do depósito: " +
                    veiculo.CapacidadeDeposito + " litros");
            }

            Console.WriteLine("\n------------------------------------------");
        }

        Pausar();
    }

    // Abastecer um veículo
    static void AbastecerVeiculo()
    {
        Console.Clear();

        Console.WriteLine("=== ABASTECER VEÍCULO ===");

        Veiculo veiculo = ProcurarVeiculo();

        if (veiculo != null)
        {
            Console.Write("\nQuantidade de combustível (litros): ");

            double litros;

            if (double.TryParse(Console.ReadLine(), out litros))
            {
                veiculo.Abastecer(litros);

                Console.WriteLine(
                    "Combustível atual: " +
                    veiculo.NivelCombustivel.ToString("F2") +
                    " litros");
            }
            else
            {
                Console.WriteLine("Quantidade inválida!");
            }
        }

        Pausar();
    }

    // Realizar uma viagem
    static void RealizarViagem()
    {
        Console.Clear();

        Console.WriteLine("=== REALIZAR VIAGEM ===");

        Veiculo veiculo = ProcurarVeiculo();

        if (veiculo != null)
        {
            Console.Write("\nIntroduza a distância da viagem (km): ");

            int distancia;

            if (int.TryParse(Console.ReadLine(), out distancia))
            {
                bool sucesso = veiculo.RealizarViagem(distancia);

                if (sucesso)
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine("\nViagem concluída com sucesso!");

                    Console.WriteLine(
                        "Quilometragem atual: " +
                        veiculo.Quilometragem + " km");

                    Console.WriteLine(
                        "Combustível restante: " +
                        veiculo.NivelCombustivel.ToString("F2") +
                        " litros");

                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Distância inválida!");
            }
        }

        Pausar();
    }

    // Consultar informações de um veículo
    static void ConsultarVeiculo()
    {
        Console.Clear();

        Console.WriteLine("=== CONSULTAR VEÍCULO ===");

        Veiculo veiculo = ProcurarVeiculo();

        if (veiculo != null)
        {
            Console.WriteLine("\n========== INFORMAÇÕES DO VEÍCULO ==========");

            Console.WriteLine("Matrícula: " + veiculo.Matricula);

            Console.WriteLine(
                "Quilometragem: " +
                veiculo.Quilometragem + " km");

            Console.WriteLine(
                "Nível de combustível: " +
                veiculo.NivelCombustivel.ToString("F2") +
                " litros");

            Console.WriteLine(
                "Capacidade do depósito: " +
                veiculo.CapacidadeDeposito + " litros");

            double percentagem =
                (veiculo.NivelCombustivel /
                veiculo.CapacidadeDeposito) * 100;

            Console.WriteLine(
                "Percentagem de combustível: " +
                percentagem.ToString("F1") + "%");
        }

        Pausar();
    }

    // Procurar veículo através da matrícula
    static Veiculo ProcurarVeiculo()
    {
        Console.Write("\nIntroduza a matrícula do veículo: ");

        string matricula = Console.ReadLine();

        Veiculo encontrado = frota.Find(
            v => string.Equals(
                v.Matricula,
                matricula,
                StringComparison.OrdinalIgnoreCase));

        if (encontrado == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nVeículo não encontrado!");
            Console.ResetColor();
        }

        return encontrado;
    }

    // Pausar a execução do programa
    static void Pausar()
    {
        Console.WriteLine("\nPrima ENTER para continuar...");
        Console.ReadLine();
    }
}
