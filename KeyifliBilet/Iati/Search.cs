using KeyifliBilet.Iati.Enums;
using KeyifliBilet.Iati.Request;
using KeyifliBilet.Iati.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KeyifliBilet.Iati
{
    public class Search
    {
        //test
       // private static string BaseUrl = "http://testapi.iati.com";
        //prod
        private static string BaseUrl = "https://api.iati.com";
        private static string AuthCode = "74787740818A51EDFA7CA239910E4271";        

        public static async Task<String> GetAirports()
        {
            var url = BaseUrl + string.Format("/rest/airports/{0}", AuthCode);

            return await GetResponseJson(url);

            //String responseText = await Task.Run(() => {
            //    try
            //    {
            //        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //        WebResponse response = request.GetResponse();
            //        Stream responseStream = response.GetResponseStream();
            //        return new StreamReader(responseStream).ReadToEnd();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error: " + e.Message);
            //    }
            //    return null;
            //});
            //return responseText;
        }

        public static async Task<String> GetAirportsByLocale(string countryCode)
        {
            var url = BaseUrl + string.Format("/rest/airports/{0}/{1}", AuthCode, countryCode);
            String responseText = await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Proxy = null;
                httpWebRequest.Method = "POST";

                httpWebRequest.ContentType = "application/json;charset=\"utf-8\"";
                try
                {
                    WebResponse webResponse = httpWebRequest.GetResponse();
                    using (Stream webStream = webResponse.GetResponseStream())
                    {
                        if (webStream != null)
                        {
                            using (StreamReader responseReader = new StreamReader(webStream))
                            {
                                result = responseReader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }
                return result;
            });
            return responseText;
        }

        public static async Task<FlightResponse> GetFlights(FlightRequest model)
        {
            var url = BaseUrl + string.Format("/rest/flightSearch/{0}", AuthCode);
            return await GetResponseModel<FlightRequest, FlightResponse>(model, url);
        }

        public static async Task<PriceDetailResponse> GetPriceDetail(PriceDetailRequest model)
        {
            var url = BaseUrl + string.Format("/rest/priceDetail/{0}", AuthCode);
            return await GetResponseModel<PriceDetailRequest, PriceDetailResponse>(model, url);
        }

        public static async Task<MultiLegFlightResponse> GetMultiLegFlights(MultiLegFlightRequest model)
        {
            var url = BaseUrl + string.Format("/rest/flightSearch/multi-leg/{0}", AuthCode);
            return await GetResponseModel<MultiLegFlightRequest, MultiLegFlightResponse>(model, url);
        }

        public static async Task<MultiLegPriceDetailResponse> GetMultiLegPriceDetail(MultiLegPriceDetailRequest model)
        {
            var url = BaseUrl + string.Format("/rest/priceDetail/multi-leg/{0}", AuthCode);
            return await GetResponseModel<MultiLegPriceDetailRequest, MultiLegPriceDetailResponse>(model, url);
        }

        public static async Task<MakeReservationResponse> MakeReservation(MakeReservationRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeReservation/{0}", AuthCode);
            return await GetResponseModel<MakeReservationRequest, MakeReservationResponse>(model, url);
        }

        public static async Task<MakeTicketFromReservationResponse> MakeTicketFromReservation(MakeTicketFromReservationRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeTicketFromReservation/{0}", AuthCode);
            return await GetResponseModel<MakeTicketFromReservationRequest, MakeTicketFromReservationResponse>(model, url);
        }

        //public static async Task<String> MakeTicket(MakeTicketRequest model)
        //{
        //    var url = BaseUrl + string.Format("/rest/makeTicket/{0}", AuthCode);
        //    return await GetResponseJson<MakeTicketRequest>(model, url);
        //}

        public static async Task<MakeTicketResponse> MakeTicket(MakeTicketRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeTicket/{0}", AuthCode);
            return await GetResponseModel<MakeTicketRequest,MakeTicketResponse>(model, url);
        }

        public static async Task<MakeTicketWithPassportResponse> MakeTicketWithPassport(MakeTicketWithPassportRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeTicket/{0}", AuthCode);
            return await GetResponseModel<MakeTicketWithPassportRequest, MakeTicketWithPassportResponse>(model, url);
        }

        public static async Task<MakeTicketWithFoidResponse> MakeTicketWithFoid(MakeTicketWithFoidRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeTicket/{0}", AuthCode);
            return await GetResponseModel<MakeTicketWithFoidRequest, MakeTicketWithFoidResponse>(model, url);

        }

        public static async Task<MakeTicketWithTcNoResponse> MakeTicketWithTcNo(MakeTicketWithTCNORequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeTicket/{0}", AuthCode);
            return await GetResponseModel<MakeTicketWithTCNORequest, MakeTicketWithTcNoResponse>(model, url);
        }

        public static async Task<MakeDummyTicketResponse> MakeDummyTicket(MakeDummyTicketRequest model)
        {
            var url = BaseUrl + string.Format("/rest/makeDummyTicket/{0}", AuthCode);
            return await GetResponseModel<MakeDummyTicketRequest,MakeDummyTicketResponse>(model, url);
        }

        public static async Task<String> CancelReservation(CancelReservationRequest model)
        {
            var url = BaseUrl + string.Format("/rest/cancelReservation/{0}", AuthCode);
            return await GetResponseJson<CancelReservationRequest>(model, url);
        }

        public static async Task<IsVoidableResponse> IsVoidable(IsVoidableRequest model)
        {
            var url = BaseUrl + string.Format("/rest/isVoidable/{0}", AuthCode);
            return await GetResponseModel<IsVoidableRequest, IsVoidableResponse>(model, url);
        }

        public static async Task<VoidPnrResponse> VoidPnr(VoidPnrRequest model)
        {
            var url = BaseUrl + string.Format("/rest/voidPnr/{0}", AuthCode);
            return await GetResponseModel<VoidPnrRequest, VoidPnrResponse>(model, url);
        }

        public static async Task<GetOrderResponse> GetOrder(GetOrderRequest model)
        {
            var url = BaseUrl + string.Format("/rest/orders/flight/getOrder/{0}", AuthCode);
            return await GetResponseModel<GetOrderRequest, GetOrderResponse>(model, url);
        }

        public static async Task<GetOrderListResponse> GetOrderList(GetOrderListRequest model)
        {
            var url = BaseUrl + string.Format("/rest/orders/flight/getOrderList/{0}", AuthCode);
            return await GetResponseModel<GetOrderListRequest, GetOrderListResponse>(model, url);
        }

        public static async Task<GetItineraryDetailResponse> GetItineraryDetail(GetItineraryDetailRequest model)
        {
            string url = string.Empty;
            if (model.currency == null)
            {
                url = BaseUrl + string.Format("/rest/flight/detail/{0}/{1}", AuthCode, model.id);
            }
            else
            {
                url = BaseUrl + string.Format("/rest/flight/detail/{0}/{1}/{2}", AuthCode, model.id, model.currency.ToString());
            }
            return await GetResponseModel<GetItineraryDetailRequest,GetItineraryDetailResponse>(model,url);
        }

        public static async Task<GetProviderRestrictionsResponse> GetProviderRestrictions(GetProviderRestrictionsRequest model)
        {
            var url = BaseUrl + string.Format("/rest/flight/providerRestrictions/{0}/{1}", AuthCode, model.providerKey);
            return await GetResponseModel<GetProviderRestrictionsResponse>(url);
        }

        public static async Task<GetSpecialOffersResponse> GetSpecialOffers()
        {
            
              var  url  = BaseUrl + string.Format("/rest/spo/{0}", AuthCode);

            return await GetResponseModel<GetSpecialOffersResponse>(url);
        }

        public static async Task<GetCalendarResponse> GetCalendar(GetCalendarRequest model)
        {
            var url = BaseUrl + string.Format("/rest/getCalendar/{0}", AuthCode);
            return await GetResponseModel<GetCalendarRequest, GetCalendarResponse>(model, url);

        }

        public static async Task<GetChangedOrdersResponse> GetChangedOrders()
        {
            var url = BaseUrl + string.Format("/rest/orders/flight/getChangedOrders/{0}", AuthCode);
            return await GetResponseModel<GetChangedOrdersResponse>(url);
        }

        public static async Task<String> GetFareRules(GetFareRulesRequest model)
        {
            var url = BaseUrl + string.Format("/rest/fareRules/{0}", AuthCode);
            return await GetResponseJson(url);
        }

        public static async Task<CurrencyListResponse> GetCurrencyList()
        {
            var url = BaseUrl + string.Format("/rest/common/currency/{0}", AuthCode);
            return await GetResponseModel<CurrencyListResponse>(url);
        }

        public static async Task<BalanceSummaryResponse> GetBalanceSummary()
        {
            var url = BaseUrl + string.Format("/rest/finance/balanceSummary/{0}", AuthCode);
            return await GetResponseModel<BalanceSummaryResponse>(url);
        }

        public static async Task<K> GetResponseModel<T, K>(T request, string url) where T : class where K : class, new()
        {

            K responseText = await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Proxy = null;
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 1200000;
                httpWebRequest.ContentType = "application/json;charset=\"utf-8\"";
                using (var streamWriter = new

                StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var f = JsonConvert.SerializeObject(request);
                    streamWriter.Write(f);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                
                try
                {
                    
                    return JsonConvert.DeserializeObject<K>(result);
                }
                catch (Exception)
                {
                    return new K();
                }
            });
            return responseText;
        }

        public static async Task<String> GetResponseJson<T>(T model, string url) where T : class
        {
            String responseText = await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Proxy = null;
                httpWebRequest.Method = "POST";

                httpWebRequest.ContentType = "application/json;charset=\"utf-8\"";
                using (var streamWriter = new

                StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var f = JsonConvert.SerializeObject(model);
                    streamWriter.Write(f);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            });
            return responseText;
        }

        public static async Task<T> GetResponseModel<T>(string url) where T : class
        {
            T responseText = await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Proxy = null;
                httpWebRequest.Method = "POST";

                httpWebRequest.ContentType = "application/json;charset=\"utf-8\"";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                using (var sr = new StringReader(result))
                using (var jr = new JsonTextReader(sr))
                {
                    var serial = new JsonSerializer();
                    serial.Formatting = Formatting.Indented;
                    var obj = serial.Deserialize<T>(jr);
                    return obj;
                }
            });
            return responseText;
        }

        public static async Task<String> GetResponseJson(string url)
        {
            String responseText = await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Proxy = null;
                httpWebRequest.Method = "POST";

                httpWebRequest.ContentType = "application/json;charset=\"utf-8\"";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            });
            return responseText;
        }

        internal static Task GetSpecialOffers(Currency? currency)
        {
            throw new NotImplementedException();
        }
    }
}