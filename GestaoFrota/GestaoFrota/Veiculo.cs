

using System;

public class Veiculo
{
    // Campos privados
    private int _quilometragem;
    private double _nivelCombustivel;

    // Propriedades automáticas
    public string Matricula { get; set; }
    public int CapacidadeDeposito { get; set; }

    // Propriedade Quilometragem com validação
    public int Quilometragem
    {
        get { return _quilometragem; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Quilometragem",
                    "A quilometragem não pode ser negativa!");
            }

            if (value < _quilometragem)
            {
                throw new InvalidOperationException(
                    "Erro: A quilometragem não pode diminuir!");
            }

            _quilometragem = value;
        }
    }

    // Propriedade NivelCombustivel com validação
    public double NivelCombustivel
    {
        get { return _nivelCombustivel; }
        set
        {
            if (double.IsNaN(value) ||
                double.IsInfinity(value) ||
                value < 0 ||
                value > CapacidadeDeposito)
            {
                throw new ArgumentOutOfRangeException(
                    "NivelCombustivel",
                    "O combustível deve estar entre 0 e a capacidade máxima do depósito!");
            }

            // Verificar se o combustível passou para a reserva
            bool reservaAtingida =
                _nivelCombustivel >= CapacidadeDeposito * 0.10 &&
                value < CapacidadeDeposito * 0.10;

            _nivelCombustivel = value;

            // Disparar o evento de alerta
            if (reservaAtingida)
            {
                ReservaAtingida?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Evento de alerta de reserva
    public event EventHandler ReservaAtingida;

    // Construtor parametrizado
    public Veiculo(string matricula, int capacidadeDeposito)
    {
        if (string.IsNullOrWhiteSpace(matricula))
        {
            throw new ArgumentException(
                "A matrícula é obrigatória!");
        }

        if (capacidadeDeposito <= 0)
        {
            throw new ArgumentException(
                "A capacidade do depósito deve ser superior a zero!");
        }

        Matricula = matricula;
        CapacidadeDeposito = capacidadeDeposito;

        // Valores iniciais
        _quilometragem = 0;
        _nivelCombustivel = capacidadeDeposito;
    }

    // Método para abastecer o veículo
    public void Abastecer(double litros)
    {
        if (double.IsNaN(litros) ||
            double.IsInfinity(litros) ||
            litros <= 0)
        {
            Console.WriteLine(
                "Erro: A quantidade de combustível deve ser superior a zero!");
            return;
        }

        double espacoDisponivel =
            CapacidadeDeposito - NivelCombustivel;

        if (litros > espacoDisponivel)
        {
            NivelCombustivel = CapacidadeDeposito;

            Console.WriteLine(
                "Aviso: O depósito foi atestado até à capacidade máxima permitida!");
        }
        else
        {
            NivelCombustivel += litros;
        }

        Console.WriteLine("Abastecimento realizado com sucesso!");
    }

    // Método para realizar uma viagem
    public bool RealizarViagem(int kmDistancia)
    {
        if (kmDistancia <= 0)
        {
            Console.WriteLine(
                "Erro: A distância da viagem deve ser superior a zero!");
            return false;
        }

        // Consumo de 0,07 litros por quilómetro
        double consumo = kmDistancia * 0.07;

        // Verificar se existe combustível suficiente
        if (NivelCombustivel < consumo)
        {
            Console.WriteLine(
                "Viagem recusada: combustível insuficiente!");
            return false;
        }

        // Atualizar a quilometragem e o combustível
        int novaQuilometragem =
            checked(Quilometragem + kmDistancia);

        Quilometragem = novaQuilometragem;
        NivelCombustivel -= consumo;

        Console.WriteLine(
            "Viagem realizada com sucesso!");

        return true;
    }
}
