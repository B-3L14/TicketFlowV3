namespace TicketFlow.Contexts.Sales.Domain.ValueObjects
{
    public record PeriodoValidade
    {
        public DateTime CriadaEm { get; init; }
        public DateTime ExpiraEm { get; init; }

        public PeriodoValidade(DateTime criadaEm, int minutosExpiracao)
        {
            // Nova validação: Impede a criação de uma reserva com tempo inválido
            if (minutosExpiracao <= 0)
                throw new ArgumentException("O tempo de expiração em minutos deve ser maior que zero.");

            CriadaEm = criadaEm;
            ExpiraEm = criadaEm.AddMinutes(minutosExpiracao);
        }

        private PeriodoValidade() { }

        // Delegamos o comportamento de checagem para o próprio dado
        public bool EstaAtivo(DateTime momentoAtual) => momentoAtual < ExpiraEm;

        public TimeSpan ObterTempoRestante(DateTime momentoAtual) =>
            EstaAtivo(momentoAtual) ? ExpiraEm - momentoAtual : TimeSpan.Zero;
    }
}