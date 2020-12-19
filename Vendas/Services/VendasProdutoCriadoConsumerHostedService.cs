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

namespace api_sis_evenda.Vendas.Services
{
    public class VendasProdutoCriadoConsumerHostedService : IHostedService
    {
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);
        private readonly ILogger _logger;
        private readonly string connectionString;
        private readonly string queueName;
        static QueueClient queueClient;
        public static IServiceScopeFactory _serviceScopeFactory;

        public VendasProdutoCriadoConsumerHostedService(ILogger<VendasProdutoCriadoConsumerHostedService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            connectionString = configuration.GetValue<string>("ServiceBus:Vendas:Consumer:TopicoProdutoCriado:ConnectionString");
            queueName = configuration.GetValue<string>("ServiceBus:Vendas:Consumer:TopicoProdutoCriado:QueueName");
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
            
            return Task.CompletedTask;
        }

        private void ExecuteProcess(object state)
        {
            _logger.LogInformation("### Proccess vendas consumer produtocriado start ###");
            _logger.LogInformation($"{DateTime.Now}");
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Proccess vendas consumer produtocriado stoping ###");
            _logger.LogInformation($"{DateTime.Now}");
            return Task.CompletedTask;
        }

        static Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            Produto produtoCriado = message.Body.ParseJson<Produto>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IProdutoVendasService produtoVendasService = scope.ServiceProvider.GetRequiredService<IProdutoVendasService>();

               produtoVendasService.AddAsync(produtoCriado);
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
