using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using System.Diagnostics;
using Testcontainers.CosmosDb;

namespace TestProject1
{
    public class UnitTest1 : IAsyncLifetime
    {
        private static readonly IOutputConsumer consumer = Consume.RedirectStdoutAndStderrToStream(new MemoryStream(), new MemoryStream());

        private readonly CosmosDbContainer _cosmosDbContainer = new CosmosDbBuilder()
      .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "3")
      .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "true")
      .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
            .WithEnvironment("TESTCONTAINERS_RYUK_DISABLED ", "true")
            .WithExposedPort(8081)
            .WithExposedPort(10250)
            .WithExposedPort(10251)
            .WithExposedPort(10252)
            .WithExposedPort(10253)
            .WithExposedPort(10254)
            .WithExposedPort(10255)
            .WithPortBinding(8081, true)
            .WithPortBinding(10251, true)
            .WithPortBinding(10252, true)
            .WithPortBinding(10253, true)
            .WithPortBinding(10254, true)
            .WithPortBinding(10255, true)
        .WithOutputConsumer(consumer)
           .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(CosmosDbBuilder.CosmosDbPort))
      .Build();


        public async Task InitializeAsync()
        { // Criando uma instância do Stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Iniciando a medição do tempo
            stopwatch.Start();

            // Método cuja duração será medida
            await _cosmosDbContainer.StartAsync();

            // Parando a medição do tempo
            stopwatch.Stop();

            // Obtendo o tempo decorrido em milissegundos
            long tempoDecorrido = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"O método demorou {tempoDecorrido} ms para executar.");
        

        }

        public async Task DisposeAsync()
        {
            await _cosmosDbContainer.StopAsync();
        }
        [Fact]
        public void Test1()
        {
            var conn = _cosmosDbContainer.GetConnectionString();

            Assert.NotNull(conn);
        }
    }
}