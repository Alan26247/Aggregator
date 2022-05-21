using Microsoft.AspNetCore.Mvc;
using Aggregator.Models.Aggregator;
using Aggregator.Services.Aggregator;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aggregator.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase
    {
        public AggregatorController(IHttpClientFactory clientFactory, IAggregatorService aggregatorService)
        {
            this.clientFactory = clientFactory;
            this.newsService = aggregatorService;
        }

        private readonly IHttpClientFactory clientFactory;
        private readonly IAggregatorService newsService;

        [BindProperty]
        public Request value { get; set; }




        // данный запрос получает список новостей из базы данных
        [HttpGet]
        public ActionResult<Rss> Get()
        {
            return newsService.GetNewsList();
        }

        // данный запрос получает список новостей из базы данных
        // используя строку поиска (искать в заголовках)
        [HttpGet("{findStr}")]
        public ActionResult<Rss> Get(string findStr)
        {
            return newsService.GetNewsList(findStr);
        }

        // запрос обновляет список новостей, сохраняет данные в БД
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Request value)
        {
            Rss rssObject;

            HttpClient client = clientFactory.CreateClient("HttpClient");

            // отправляем запрос
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(value.RssUrl);
                // получаем rss xml строку
                string body = await responseMessage.Content.ReadAsStringAsync().
                                                                ConfigureAwait(false);
                // конвертируем xmlRss в Rss объект
                ConvertorXmlToRssObject convertor = new ConvertorXmlToRssObject();
                rssObject = convertor.Convert(body);
                rssObject.Channel.RSS = value.RssUrl;
            }
            catch
            {
                return BadRequest();
            }

            newsService.UpdateChannel(rssObject);

            return Ok("updated");
        }

        // запрос очищает полностью базу данных
        [HttpDelete]
        public ActionResult Delete()
        {
            newsService.Clear();

            return NoContent();
        }



        // =========================== локальные =========================
        public class Request
        {
            [Url]
            public string RssUrl { get; set; }
        }
    }
}
