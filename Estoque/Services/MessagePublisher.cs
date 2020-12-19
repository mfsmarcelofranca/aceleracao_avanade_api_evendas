using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using api_sis_evenda.Estoque.Services;
using System.Threading.Tasks;
using System.Text;

namespace api_sis_evenda.Vendas.Services
{

    namespace api_sis_evenda.estoque.Services
    {
        public class MessagePublisher : IMessagePublisher
        {
            private IQueueClient _queueClient { get; set; }
            private IConfiguration _configuration;

            public MessagePublisher(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public Task Publish<T>(T obj)
            {
                var message = new Message(obj.ToJsonBytes());
                message.ContentType = "application/json";

                return _queueClient.SendAsync(message);
            }

            public Task Publish(string raw)
            {
                var message = new Message(Encoding.UTF8.GetBytes(raw));
                return _queueClient.SendAsync(message);
            }

            public void setQueueClient(string fila)
            {
                _queueClient = new QueueClient(_configuration.GetValue<string>(fila + "ConnectionString"),
                _configuration.GetValue<string>(fila + "QueueName"));
            }
        }
    }        
}