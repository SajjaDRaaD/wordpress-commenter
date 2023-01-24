using AppDTOs;
using DataAccess.Models.User;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApp.Services.Rest
{
    public class RestClientService : IRestClientService
    {
        protected readonly RestClient _restClient;
        protected readonly RestRequest _restRequest;

        public RestClientService()
        {
            _restClient = new RestClient();
            _restRequest = new RestRequest();
        }

        public async Task<bool> CheckApiAccess(Websites website)
        {
            _restClient.Options.BaseUrl = new Uri($"{website.Url}wp-json/wc/v3/products/");
            _restClient.Authenticator = OAuth1Authenticator.ForRequestToken(website.CustomerKey, website.CustomerSecret);
            _restRequest.Method = Method.Post;
            var result = await _restClient.ExecuteAsync(_restRequest);

            return result.IsSuccessful;
        }

        public async Task<bool> IsWordpress(Websites website)
        {
            _restClient.Options.BaseUrl = new Uri($"{website.Url}/wp-json/wp/v2");
            _restRequest.Method = Method.Get;
            var result = await _restClient.ExecuteAsync(_restRequest);

            return result.IsSuccessful;
        }

        public async Task<MessageDTO> SendComment(Websites website, Comment comment, int postId)
        {
            if (await IsPostExist(postId,website))
            {
                var restClient = new RestClient();
                var restRequest = new RestRequest();

                restClient.Options.BaseUrl = new Uri($"{website.Url}/wp-json/wp/v2/comments/");
                restRequest.Method = Method.Post;
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(new
                {
                    post = postId,
                    author_name = comment.Author,
                    content = comment.Body
                });
                var result = await restClient.ExecuteAsync(restRequest);
                return new MessageDTO
                {
                    Message = result.Content,
                    Status = result.StatusCode.ToString()
                };
            }

            return new MessageDTO
            {
                Message = "پست مورد نظر یافت نشد",
                Status = "404"

            };
        }

        public async Task<MessageDTO> SendReview(Websites website, Review review, int productId)
        {
            if (await IsProductExist(productId, website))
            {
                var restClient = new RestClient();
                var restRequest = new RestRequest();

                restClient.Options.BaseUrl = new Uri($"{website.Url}/wp-json/wc/v3/products/reviews/");
                restClient.Authenticator = OAuth1Authenticator.ForRequestToken(website.CustomerKey, website.CustomerSecret);
                restRequest.Method = Method.Post;
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(new
                {
                    product_id = productId,
                    reviewer = review.Author,
                    review = review.Body,
                    reviewer_email = "",
                    rating = review.Rating
                });
                var result = await restClient.ExecuteAsync(restRequest);

                return new MessageDTO
                {
                    Message = result.Content,
                    Status = result.StatusCode.ToString()
                };
            }

            return new MessageDTO
            {
                Message = "محصول مورد نظر یافت نشد",
                Status = "404"
            };
        }

        public async Task<bool> IsPostExist(int postId, Websites website)
        {
            var restClient = new RestClient();
            var restRequest = new RestRequest();

            restClient.Options.BaseUrl = new Uri($"{website.Url}/wp-json/wp/v2/posts/{postId}/");
            restRequest.Method = Method.Get;
            var result = await restClient.ExecuteAsync(restRequest);

            return result.IsSuccessful;
        }

        public async Task<bool> IsProductExist(int productId, Websites website)
        {
            _restClient.Options.BaseUrl = new Uri($"{website.Url}/wp-json/wc/v3/products/{productId}/");
            _restClient.Authenticator = OAuth1Authenticator.ForRequestToken(website.CustomerKey, website.CustomerSecret);
            _restRequest.Method = Method.Get;
            var result = await _restClient.ExecuteAsync(_restRequest);

            return result.IsSuccessful;
        }
    }
}
