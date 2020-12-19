using System.Threading.Tasks;

namespace api_sis_evenda.Estoque.Services
{
    public interface IMessagePublisher
    {
        public Task Publish<T>(T obj);

        public Task Publish(string raw);

        public void setQueueClient(string fila);
    }
}