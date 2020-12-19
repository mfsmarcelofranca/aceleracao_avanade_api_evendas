using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;
using api_sis_evenda.estoque.Models;
using Microsoft.Extensions.DependencyInjection;
using api_sis_evenda.Vendas.Services;

namespace api_sis_evenda.Estoque.Services
{
    public class EstoqueProdutoVendidoConsumerHostedService : IHostedService
    {
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);
        private readonly ILogger _logger;
        private readonly string connectionString;
        private readonly string queueName;
        static QueueClient queueClient;
        public static IServiceScopeFactory _serviceScopeFactory;

        public EstoqueProdutoVendidoConsumerHostedService(ILogger<EstoqueProdutoVendidoConsumerHostedService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            connectionString = configuration.GetValue<string>("ServiceBus:Estoque:Consumer:TopicoProdutoVendido:ConnectionString");
            queueName = configuration.GetValue<string>("ServiceBus:Estoque:Consumer:TopicoProdutoVendido:QueueName");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                queueClient = new QueueClient(connectionString, queueName,ReceiveMode.ReceiveAndDelete);
                //queueClient = new QueueClient(connectionString, queueName);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                queueClient.RegisterMessageHandler(ReceiveMessagesAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("erro");
            }
            finally
            {
                //Console.ReadKey();
                //queueClient.CloseAsync();
            }

            new Timer(ExecuteProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
           // new Timer(readMesage, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            
            return Task.CompletedTask;
        }

        private void ExecuteProcess(object state)
        {
            _logger.LogInformation("### Proccess estoque consumer produtovendido start ###");
            _logger.LogInformation($"{DateTime.Now}");
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Proccess estoque consumer produtovendido stoping ###");
            _logger.LogInformation($"{DateTime.Now}");
            return Task.CompletedTask;
        }

        static Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            Produto produtoVendido = message.Body.ParseJson<Produto>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IProdutoService produtoService = scope.ServiceProvider.GetRequiredService<IProdutoService>();

                produtoService.UpdateProdutoVendidoAsync(produtoVendido);
            }
            return Task.CompletedTask;
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}
