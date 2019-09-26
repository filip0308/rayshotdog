using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RaysHotDogs.Core
{
    public class OrderHotDog
    {
        public int Quantity { get; set; }
    }

    public class CartRepository
    {
        private static List<Cart> hotDogs = new List<Cart>();

        string url = "http://192.168.43.115:45459/api/hotdog/{id}/order";

        public CartRepository()
        {
            Task.Run(() => this.GetCartAsync()).Wait();
        }

        private async Task AddOrderAsync(int id, int quantity)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var myContent = JsonConvert.SerializeObject(new OrderHotDog { Quantity = quantity });
                    HttpContent c = new StringContent(myContent, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"http://192.168.43.115:45459/api/hotdog/{id}/order"),
                        Content = c
                    };
                                       
                    Task<HttpResponseMessage> getResponse = httpClient.SendAsync(request);

                    HttpResponseMessage response = await getResponse;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private async Task GetCartAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.GetAsync("http://192.168.43.115:45459/api/hotdog/cart");

                    HttpResponseMessage response = await getResponse;

                    var responseJsonString = await response.Content.ReadAsStringAsync();

                    var hotDogs = JsonConvert.DeserializeObject<List<HotDog>>(responseJsonString);
                    foreach(var hotDog in hotDogs)
                    {
                        MainCart.CartItems.Add(new CartItem() { HotDog = hotDog, Amount = 0 });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static Cart MainCart { get; set; } = new Cart() { CartItems = new List<CartItem>() };

        public void AddCartItem(HotDog hotDog, int amount)
        {
            Task.Run(() => this.AddOrderAsync(hotDog.HotDogId, amount)).Wait();
            
            MainCart.CartItems.Add(new CartItem() { HotDog = hotDog, Amount = amount });
        }

        public Cart GetCart()
        {
            return MainCart;
        }
    }
}

