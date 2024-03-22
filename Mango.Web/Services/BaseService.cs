using System.Text;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ITokenProvider _tokenProvider;

		public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
		{
			_httpClientFactory = httpClientFactory;
			_tokenProvider = tokenProvider;
		}

		public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
		{
            var apiResponseDto = new ResponseDto();

            try
			{
				var client = _httpClientFactory.CreateClient("MangoAPI");
				var requestMessage = new HttpRequestMessage();
				requestMessage.Headers.Add("Accept", "application/json");

				if (withBearer)
				{
					requestMessage.Headers.Add("Authorization", $"Bearer { _tokenProvider.GetToken() }");
				}

				requestMessage.RequestUri = new Uri(requestDto.Url);

				if (requestDto.Data != null)
				{
					requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
				}

				switch(requestDto.apiType)
				{
					case Utilities.StaticDetails.ApiType.POST:
						requestMessage.Method = HttpMethod.Post;
						break;
                    case Utilities.StaticDetails.ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case Utilities.StaticDetails.ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }

				var apiResponse = await client.SendAsync(requestMessage);

				switch(apiResponse.StatusCode)
				{
					case System.Net.HttpStatusCode.NotFound:
						apiResponseDto.Message = "Not Found";
						break;
                    case System.Net.HttpStatusCode.Forbidden:
                        apiResponseDto.Message = "Access Denied";
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        apiResponseDto.Message = "Unauthorized";
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        apiResponseDto.Message = "Internal Server Error";
                        break;
                    default:
						var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
						break;
				}
			}
			catch (Exception ex)
			{
				apiResponseDto = new() { Message = ex.Message.ToString() };
			}

            return apiResponseDto;
        }
	}
}

