using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaysHotDogs.Core
{
	public class HotDogRepository
	{

        private static List<HotDog> hotDogs = new List<HotDog>();

        string url = "http://192.168.43.115:45459/api/hotdog";

        public HotDogRepository()
        {
            Task.Run(() => this.LoadDataAsync(url)).Wait();
        }

        private async Task LoadDataAsync(string uri)
        {
            if (hotDogs != null)
            {
                string responseJsonString = null;

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        Task<HttpResponseMessage> getResponse = httpClient.GetAsync(uri);

                        HttpResponseMessage response = await getResponse;

                        responseJsonString = await response.Content.ReadAsStringAsync();

                        hotDogs = JsonConvert.DeserializeObject<List<HotDog>>(responseJsonString);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }


        public List<HotDog> GetAllHotDogs()
		{
			return hotDogs.ToList<HotDog>();
		}

		public List<HotDogGroup> GetGroupedHotDogs()
		{
            return new List<HotDogGroup>();
		}

		public List<HotDog> GetHotDogsForGroup(int hotDogGroupId)
		{
			return hotDogs;
		}

		public List<HotDog> GetFavoriteHotDogs()
		{
			return hotDogs.Where(x => x.IsFavorite).ToList<HotDog>();
		}

		public HotDog GetHotDogById(int hotDogId)
		{
			return hotDogs.FirstOrDefault(x => x.HotDogId == hotDogId);
		}

    }
}